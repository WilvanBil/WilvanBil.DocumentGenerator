using MassTransit;
using WilvanBil.DocumentGenerator.Contracts;

namespace WilvanBil.DocumentGenerator.Api.Consumers;

// This consumer is responsible for generating a single document
public class GenerateDocumentConsumer(ILogger<GenerateDocumentConsumer> logger) : IConsumer<GenerateDocumentCommand>
{
    public async Task Consume(ConsumeContext<GenerateDocumentCommand> context)
    {
        // Logic here to generate the document
        await Task.Delay(1000);

        logger.LogInformation("Document generated for correlation {CorrelationId} with template {TemplateId} with message {MessageId}", context.Message.CorrelationId, context.Message.TemplateId, context.MessageId);
        await context.Publish(new DocumentGeneratedEvent(context.Message.CorrelationId));
    }
}
