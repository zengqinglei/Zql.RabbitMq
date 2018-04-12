using System;
using RabbitMQ.Client;

namespace Zac.RabbitMq.Sdk
{
    /// <summary>
    /// 消息队列客户端服务
    /// </summary>
    public partial class RabbitClient : IRabbitClient
    {
        /// <summary>
        /// 创建消息队列服务
        /// </summary>
        public static RabbitClient Create(Action<RabbitOptions> config)
        {
            var rabbitOptions = new RabbitOptions();
            config?.Invoke(rabbitOptions);
            return new RabbitClient(rabbitOptions);
        }

        private readonly RabbitOptions _options;
        private readonly IConnection _connection;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RabbitClient(RabbitOptions options)
        {
            _options = options;

            var connectionFactory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost
            };
            _connection = connectionFactory.CreateConnection();
        }
    }
}
