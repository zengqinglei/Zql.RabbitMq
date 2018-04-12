namespace Zac.RabbitMq.Sdk.Configurator
{
    /// <summary>
    /// 订阅消息配置
    /// </summary>
    public class SubscriptionOptions
    {
        /// <summary>
        /// 消息类型名称
        /// </summary>
        public string MessageTypeName { get; set; }
        /// <summary>
        /// 是否自动应答
        /// </summary>
        public bool AutoAck { get; set; }
    }
}
