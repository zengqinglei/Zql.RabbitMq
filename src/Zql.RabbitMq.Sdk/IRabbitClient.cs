using System;
using Zac.RabbitMq.Sdk.Common;
using Zac.RabbitMq.Sdk.Configurator;

namespace Zac.RabbitMq.Sdk
{
    /// <summary>
    /// 消息队列服务
    /// </summary>
    public interface IRabbitClient
    {
        /// <summary>
        /// 订阅消息
        /// </summary>
        void Subscribe<TMessage>(
            Action<TMessage, ConsumeContext> subscribeEvent,
            Action<SubscriptionOptions> config = null) where TMessage : class;

        /// <summary>
        /// 发布消息
        /// </summary>
        void Publish<TMessage>(
            TMessage message,
            Action<PublishOptions> config = null) where TMessage : class;
    }
}
