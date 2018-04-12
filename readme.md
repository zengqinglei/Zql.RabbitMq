##### 安装SDK
```
Install-Package Zac.RabbitMQ.Sdk
```
##### 初始化配置
###### net451平台
```
// 1. 静态实例方式使用
public readonly IRabbitClient RabbitClient;

static RabbitMQTest()
{
    RabbitClient = RabbitClient.Create(
        config =>
        {
            config.HostName = "{hostname}";
            config.VirtualHost = "debug";
        });
}
```

```
// 2. 依赖注入方式使用(Autofac)
var builder = new ContainerBuilder();
builder.Register<IRabbitClient>(
    RabbitClient.Create(
        config =>
        {
            config.HostName = "{hostname}";
            config.VirtualHost = "debug";
        });
```
###### net461及net core 平台
```
// Startup.cs中ConfigureServices方法增加如下配置
services.AddRabbitMQ(
    config =>
    {
        config.HostName = "{hostname}";
        config.VirtualHost = "debug";
    });
```

##### 发布消息
```
public class RabbitMessageModel
{
    public string Name { get; set; }
}

[Fact]
public void Test_Publish()
{
    _rabbitClient.Publish(
        new RabbitMessageModel()
        {
            Name = DateTime.Now.Ticks.ToString()
        },
        config =>
        {
            // 延时发送(默认立即发送)
            config.PlanPublishTime = DateTime.Now.AddSeconds(1);
        });
}
```

##### 订阅消息
```
[Fact]
public void Test_Subscribe()
{
    _rabbitClient.Subscribe<RabbitMessageModel>(
        (message, context) =>
        {
            Console.WriteLine(message.Name);
        },
        config =>
        {

        });
}
```

## Change Log

### v1.0.0(2018-04-12)

#### Features
- 支持消息订阅\发布
- 支持消息延时发布