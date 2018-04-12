using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using Zac.RabbitMq.Sdk.Configurator;

namespace Zac.RabbitMq.Sdk
{
    /// <summary>
    /// 消息队列客户端服务
    /// </summary>
    public partial class RabbitClient
    {
        /// <summary>
        /// 发布消息
        /// </summary>
        public virtual void Publish<TMessage>(
            TMessage message,
            Action<PublishOptions> config = null) where TMessage : class
        {
            // 配置初始化
            var publishOptions = new PublishOptions();
            config?.Invoke(publishOptions);

            using (var channel = _connection.CreateModel())
            {
                var properties = channel.CreateBasicProperties();
                properties.MessageId = Guid.NewGuid().ToString();
                properties.DeliveryMode = 2;
                if (publishOptions.PropertyModifier != null)
                {
                    publishOptions.PropertyModifier.Invoke(properties);
                }
                if (publishOptions.BasicReturn != null)
                {
                    channel.BasicReturn += publishOptions.BasicReturn;
                }

                // 队列声明
                var messageTypeName = publishOptions.MessageTypeName ?? typeof(TMessage).Name;
                var reciveQueueName = $"{_options.Prefix}-rmq-{messageTypeName}";
                var reciveExchangeName = $"{_options.Prefix}-rexc-{messageTypeName}";
                channel.ExchangeDeclare(exchange: reciveExchangeName, type: "direct");

                var bufferQueueName = $"{_options.Prefix}-bmq-{messageTypeName}";
                var bufferExchangeName = $"{_options.Prefix}-bexc-{messageTypeName}";
                channel.ExchangeDeclare(exchange: bufferExchangeName, type: "direct");

                // 定时发送
                if (publishOptions.PlanPublishTime > DateTime.Now)
                {
                    properties.Expiration = (publishOptions.PlanPublishTime - DateTime.Now).TotalMilliseconds.ToString("F0");
                }
                else
                {
                    properties.Expiration = "0";
                }
                publishOptions.Arguments.Add(
                    "x-dead-letter-exchange",
                    reciveExchangeName);
                publishOptions.Arguments.Add(
                    "x-dead-letter-routing-key",
                    reciveQueueName);

                // 创建缓存区队列完成延时发送功能
                channel.QueueDeclare(
                    queue: bufferQueueName,
                    autoDelete: publishOptions.AutoDelete,
                    durable: publishOptions.Durable,
                    exclusive: publishOptions.Exclusive,
                    arguments: publishOptions.Arguments);
                channel.QueueBind(
                    queue: bufferQueueName,
                    exchange: bufferExchangeName,
                    routingKey: bufferQueueName);

                // 创建接收消息队列
                channel.QueueDeclare(
                    queue: reciveQueueName,
                    autoDelete: publishOptions.AutoDelete,
                    durable: publishOptions.Durable,
                    exclusive: publishOptions.Exclusive,
                    arguments: publishOptions.Arguments);
                channel.QueueBind(
                    queue: reciveQueueName,
                    exchange: reciveExchangeName,
                    routingKey: reciveQueueName);

                // 发送消息
                var data = JsonConvert.SerializeObject(message);
                channel.BasicPublish(
                    exchange: bufferExchangeName,
                    routingKey: bufferQueueName,
                    basicProperties: properties,
                    body: Encoding.UTF8.GetBytes(data),
                    mandatory: (publishOptions.BasicReturn != null));
            }
        }
    }
}
