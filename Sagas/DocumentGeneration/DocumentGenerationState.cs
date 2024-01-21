using MassTransit;

namespace WilvanBil.DocumentGenerator.Api.Sagas.DocumentGeneration;

public class DocumentGenerationState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public Guid TemplateId { get; set; }
    public int InitialDocumentCount { get; set; }
    public int DocumentGeneratedCount { get; set; }
}
