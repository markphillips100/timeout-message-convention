using NServiceBus;

namespace Common
{
    public static class EndpointConfigurationExtensions
    {
        public static EndpointConfiguration ConfigureEndpoint(this EndpointConfiguration config)
        {
            config.UsePersistence<LearningPersistence>();
            config.UseTransport<LearningTransport>();

            config.Conventions()
                .DefiningMessagesAs(t =>
                    t.Namespace?.EndsWith(".Messages") == true);

            config.UseSerialization<NewtonsoftSerializer>();

            config.Pipeline.Register(
                typeof(PublishFullTypeNameOnlyBehavior).FullName,
                typeof(PublishFullTypeNameOnlyBehavior),
                "Configures endpoints to publish messages without defined assembly names.");

            return config;
        }
    }
}
