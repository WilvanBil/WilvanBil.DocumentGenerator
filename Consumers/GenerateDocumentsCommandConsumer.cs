using MassTransit;
using WilvanBil.DocumentGenerator.Contracts;

namespace WilvanBil.DocumentGenerator.Api.Consumers;

/// <summary>
/// This is the entry point. It will start the saga.
/// </summary>
public class GenerateDocumentsCommandConsumer(ILogger<GenerateDocumentsCommandConsumer> logger) : IConsumer<GenerateDocumentsCommand>
{
    public async Task Consume(ConsumeContext<GenerateDocumentsCommand> context)
    {
        var documentsToGenerate = context.Message.DocumentValuesArray;

        logger.LogInformation(
            "Generating {DocumentCount} documents for correlation {CorrelationId} with template {TemplateId} with message {MessageId}",
            documentsToGenerate.Length,
            context.Message.CorrelationId,
            context.Message.TemplateId,
            context.MessageId);

        foreach (var document in documentsToGenerate)
        {
            var command = new GenerateDocumentCommand(context.Message.CorrelationId, context.Message.TemplateId, document);
            await context.Publish(command, context.CancellationToken);
        }
    }
}
