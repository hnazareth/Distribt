using Distribt.Services.Subscriptions.Dtos;
using Distribt.Shared.Communication.Publisher.Integration;

WebApplication app = DefaultDistribtWebApplication.Create(webappBuilder =>
{
    webappBuilder.Services.AddReverseProxy()
        .LoadFromConfig(webappBuilder.Configuration.GetSection("ReverseProxy"));
    webappBuilder.Services.AddServiceBusIntegrationPublisher(webappBuilder.Configuration);
});

app.MapGet("/", () => "Hello World!");

app.MapPost("/subscribe", async (SubscriptionDto subscriptionDto) =>
{
    IIntegrationMessagePublisher publisher = app.Services.GetRequiredService<IIntegrationMessagePublisher>();
    await publisher.Publish(subscriptionDto, routingKey: "subscription");
});

app.MapReverseProxy();


DefaultDistribtWebApplication.Run(app);