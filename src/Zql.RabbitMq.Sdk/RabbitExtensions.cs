#if !NET451
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Zac.RabbitMq.Sdk
{
    /// <summary>
    /// 消息队列服务注入拓展
    /// </summary>
    public static class RabbitExtensions
    {
        /// <summary>
        /// 添加消息服务
        /// </summary>
        public static IServiceCollection AddRabbitMQ(this IServiceCollection services, Action<RabbitOptions> config)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
            return services.AddSingleton<IRabbitClient>(RabbitClient.Create(config));
        }
    }
}
#endif
