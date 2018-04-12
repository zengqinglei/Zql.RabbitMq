using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;

namespace Zac.RabbitMq.Sdk.Configurator
{
    /// <summary>
    /// 发布消息配置
    /// </summary>
    public class PublishOptions
    {
        /// <summary>
        /// 消息类型名称
        /// </summary>
        public string MessageTypeName { get; set; }
        /// <summary>
        /// 计划发布时间(默认立即发送)
        /// </summary>
        public DateTime PlanPublishTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 基础属性
        /// </summary>
        public Action<IBasicProperties> PropertyModifier { get; set; }
        /// <summary>
        /// 回调事件
        /// </summary>
        public EventHandler<BasicReturnEventArgs> BasicReturn { get; set; }
        /// <summary>
        /// 是否自动删除
        /// </summary>
        public bool AutoDelete { get; set; }
        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool Durable { get; set; } = true;
        /// <summary>
        /// 是否独占的
        /// </summary>
        public bool Exclusive { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> Arguments { get; set; } = new Dictionary<string, object>();
    }
}
