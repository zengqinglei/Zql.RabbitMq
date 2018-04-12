namespace Zac.RabbitMq.Sdk
{
    /// <summary>
    /// Mq配置
    /// </summary>
    public class RabbitOptions
    {
        /// <summary>
        /// 队列等名称字首
        /// </summary>
        public string Prefix { get; set; } = "zql";
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; set; } = 5672;
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = "guest";
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "guest";
        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; } = "/";
    }
}
