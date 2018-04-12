using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Zac.RabbitMq.Sdk.Tests.Messages;

namespace Zac.RabbitMq.Sdk.Tests
{
    public class RabbitMQTest
    {
        private readonly IRabbitClient _rabbitClient;

        public RabbitMQTest()
        {
            _rabbitClient = RabbitClient.Create(
                config =>
                {
                    config.HostName = "rabbitmq1.esdserver.zhongan.com.cn";
                    config.VirtualHost = "debug";
                });
        }

        [Fact]
        public void Test_SubscribeAndPublish()
        {
            _rabbitClient.Publish(
                new TestMessage()
                {
                    Name = $"当前时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}"
                },
                config =>
                {
                    config.PlanPublishTime = DateTime.Now.AddSeconds(1);
                });

            _rabbitClient.Subscribe<TestMessage>(
                async (message, context) =>
                {
                    Console.WriteLine(message.Name);
                    await Task.FromResult(0);
                });

            Thread.Sleep(3000);
        }
    }
}
