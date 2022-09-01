using Messages;
using Messages.Events;
using NServiceBus;
using NServiceBus.Logging;
using SampleDomain.Messages.Events;

namespace SampleDomain;

public class BadOrderSaga :
    Saga<BadOrderSaga.OrderSagaData>,
    IAmStartedByMessages<StartOrder>,
    IHandleTimeouts<CancelOrderEvent>
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
            .ToMessage<StartOrder>(message => message.OrderId)
            .ToMessage<SampleEvent>(message => message.OrderId);
    }

    public async Task Handle(StartOrder message, IMessageHandlerContext context)
    {
        // Correlation property Data.OrderId is automatically assigned with the value from message.OrderId;
        log.Info($"StartOrder received with OrderId {message.OrderId}");

        var timeout = DateTime.UtcNow.AddSeconds(5);
        log.Info("Requesting a CancelOrderEvent that will be executed in 5 seconds.");
        await RequestTimeout<CancelOrderEvent>(context, timeout)
            .ConfigureAwait(false);
    }

    public Task Timeout(CancelOrderEvent state, IMessageHandlerContext context)
    {
        log.Info($"Cancelling order OrderId {Data.OrderId}. Calling MarkAsComplete");
        MarkAsComplete();
        return Task.CompletedTask;
    }
}
