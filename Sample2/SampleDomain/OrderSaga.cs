//using System;
//using System.Threading.Tasks;
//using Messages;
//using NServiceBus;
//using NServiceBus.Logging;

//namespace SampleDomain;

//public class OrderSaga :
//    Saga<OrderSaga.OrderSagaData>,
//    IAmStartedByMessages<StartOrder>,
//    IHandleTimeouts<CancelOrder>
//{
//    public class OrderSagaData :
//    ContainSagaData
//    {
//        public Guid OrderId { get; set; }
//    }

//    static ILog log = LogManager.GetLogger<OrderSaga>();

//    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<OrderSagaData> mapper)
//    {
//        mapper.MapSaga(sagaData => sagaData.OrderId)
//            .ToMessage<StartOrder>(message => message.OrderId);
//    }

//    public async Task Handle(StartOrder message, IMessageHandlerContext context)
//    {
//        // Correlation property Data.OrderId is automatically assigned with the value from message.OrderId;
//        log.Info($"StartOrder received with OrderId {message.OrderId}");

//        var timeout = DateTime.UtcNow.AddSeconds(5);
//        log.Info("Requesting a CancelOrder that will be executed in 5 seconds.");
//        await RequestTimeout<CancelOrder>(context, timeout)
//            .ConfigureAwait(false);
//    }

//    public Task Timeout(CancelOrder state, IMessageHandlerContext context)
//    {
//        log.Info($"Cancelling order OrderId {Data.OrderId}. Calling MarkAsComplete");
//        MarkAsComplete();
//        return Task.CompletedTask;
//    }
//}
