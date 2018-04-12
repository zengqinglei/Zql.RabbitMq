using System;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using Zac.RabbitMq.Sdk.Common;
using Zac.RabbitMq.Sdk.Configurator;

namespace Zac.RabbitMq.Sdk
{
    /// <summary>
    /// 消息队列客户端服务
    /// </summary>
    public partial class RabbitClient
    {
        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe<TMessage>(
            Action<TMessage, ConsumeContext> subscribeEvent,
            Action<SubscriptionOptions> config = null) where TMessage : class
        {
            // 配置初始化
            var subscriptionOptions = new SubscriptionOptions();
            config?.Invoke(subscriptionOptions);

            var channel = _connection.CreateModel();
            // 回调事件
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var consumeContext = new ConsumeContext()
                {
                    Exchange = args.Exchange,
                    RoutingKey = args.RoutingKey,
                    ConsumerTag = args.ConsumerTag,
                    DeliveryTag = args.DeliveryTag,
                    MessageId = args.BasicProperties.MessageId
                };

                // 读取消息
                var body = args.Body;
                var data = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<TMessage>(data);

                // 执行订阅事件
                subscribeEvent(message, consumeContext);

                // 交付确认
                if (!subscriptionOptions.AutoAck)
                {
                    channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                }
            };

            // 声明队列名称
            var messageTypeName = subscriptionOptions.MessageTypeName ?? typeof(TMessage).Name;
            var reciveQueueName = $"{_options.Prefix}-rmq-{messageTypeName}";
            var reciveExchangeName = $"{_options.Prefix}-rexc-{messageTypeName}";

            // 绑定接收队列
            channel.ExchangeDeclare(exchange: reciveExchangeName, type: "direct");
            channel.QueueBind(
                queue: reciveQueueName,
                exchange: reciveExchangeName,
                routingKey: reciveQueueName);
            channel.BasicConsume(
                reciveQueueName,
                subscriptionOptions.AutoAck,
                consumer);
        }
    }
}
