using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Pipeline;

namespace Common
{
    public class PublishFullTypeNameOnlyBehavior : IBehavior<IOutgoingPhysicalMessageContext, IOutgoingPhysicalMessageContext>
    {
        public Task Invoke(IOutgoingPhysicalMessageContext context, Func<IOutgoingPhysicalMessageContext, Task> next)
        {
            var messageIntent = context.Headers[Headers.MessageIntent];
            //if (messageIntent != "Publish")
            //{
            //    return next(context);
            //}

            var types = context.Headers[Headers.EnclosedMessageTypes];

            var enclosedTypes = types.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < enclosedTypes.Length; i++)
            {
                if (enclosedTypes[i].Contains(','))
                {
                    enclosedTypes[i] = enclosedTypes[i].Split(',')[0];
                }
            }
            context.Headers[Headers.EnclosedMessageTypes] = string.Join(";", enclosedTypes);
            return next(context);
        }
    }
}
