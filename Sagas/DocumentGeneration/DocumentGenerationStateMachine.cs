using MassTransit;
using WilvanBil.DocumentGenerator.Contracts;

namespace WilvanBil.DocumentGenerator.Api.Sagas.DocumentGeneration;

public class DocumentGenerationStateMachine : MassTransitStateMachine<DocumentGenerationState>
{
    public State Generating { get; private set; }
    public State Generated { get; private set; }

    public DocumentGenerationStateMachine()
    {
        // CurrentState is where the saga state is stored in the database
        InstanceState(x => x.CurrentState);

        // Configure States here
        Initially(
            When(GenerateDocuments)
            .Then(context =>
            {
                context.Saga.TemplateId = context.Message.TemplateId;
                context.Saga.InitialDocumentCount = context.Message.DocumentValuesArray.Length;
            })
            .TransitionTo(Generating));

        // When documents are generated, we count them and update the saga
        During(Generating,
            When(DocumentGenerated).Then(context => context.Saga.DocumentGeneratedCount++));

        // When all documents are generated, we finalize the saga
        During(Generating, When(DocumentGenerated).If(
            context => context.Saga.DocumentGeneratedCount == context.Saga.InitialDocumentCount,
            binder => binder.TransitionTo(Generated).Then(x => x.Publish(new AllDocumentsGeneratedEvent(x.CorrelationId.Value)))));


        // When all documents are generated, we finalize the saga
        During(Generated,
            When(AllDocumentsGenerated)
            .Finalize());
    }

    public Event<GenerateDocumentsCommand> GenerateDocuments { get; private set; }
    public Event<DocumentGeneratedEvent> DocumentGenerated { get; private set; }
    public Event<AllDocumentsGeneratedEvent> AllDocumentsGenerated { get; private set; }
}
