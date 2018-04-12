namespace Zac.RabbitMq.Sdk.Common
{
    /// <summary>
    /// 消费上下文信息
    /// </summary>
    public class ConsumeContext
    {
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string RoutingKey { get; set; }
        /// <summary>
        /// 消费标签
        /// </summary>
        public string ConsumerTag { get; set; }
        /// <summary>
        /// 交付标签
        /// </summary>
        public ulong DeliveryTag { get; set; }
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }
    }
}
