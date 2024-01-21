using MassTransit;
using WilvanBil.DocumentGenerator.Contracts;

namespace WilvanBil.DocumentGenerator.Api.Consumers;

public class AllDocumentsGeneratedConsumer(ILogger<AllDocumentsGeneratedConsumer> logger) : IConsumer<AllDocumentsGeneratedEvent>
{
    public async Task Consume(ConsumeContext<AllDocumentsGeneratedEvent> context)
    {
        logger.LogInformation("All documents generated for correlation {CorrelationId}", context.Message.CorrelationId);
    }
}
