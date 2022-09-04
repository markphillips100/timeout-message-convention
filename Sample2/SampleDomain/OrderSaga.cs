using NServiceBus;
using NServiceBus.Logging;
using SampleDomain.Messages;

namespace SampleDomain;

public class BadOrderSaga :
    Saga<BadOrderSaga.OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<OrderExpired>
{
    public class OrderSagaData :
        ContainSagaData
    {
        public Guid OrderId { get; set; }
    }

    static ILog log = LogManager.GetLogger<BadOrderSaga>();

    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
    {
        mapper.MapSaga(sagaData => sagaData.OrderId)
            .ToMessage<StartOrder>(message => message.OrderId);
    }

    public async Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        // Correlation property Data.OrderId is automatically assigned with the value from message.OrderId;
        log.Info($"StartOrder received with OrderId {message.OrderId}");

        var timeout = DateTime.UtcNow.AddSeconds(10);
        log.Info("Expire order in 10 seconds.");
        await RequestTimeout<OrderExpired>(context, timeout)
            .ConfigureAwait(false);
    }

    public Task Timeout(OrderExpired state, IMessageHandlerContext context)
    {
        log.Info($"Cancelling order OrderId {Data.OrderId}. Calling MarkAsComplete");
        MarkAsComplete();
        return Task.CompletedTask;
    }
}
