using MassTransit;
using WilvanBil.DocumentGenerator.Api.Consumers;
using WilvanBil.DocumentGenerator.Api.Sagas.DocumentGeneration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddLogging(builder => builder.AddConsole());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GenerateDocumentConsumer>();
    x.AddConsumer<AllDocumentsGeneratedConsumer>();
    x.AddConsumer<GenerateDocumentsCommandConsumer>();

    x.AddSagaStateMachine<DocumentGenerationStateMachine, DocumentGenerationState>()
        .InMemoryRepository();

    x.UsingRabbitMq((context, cfg) =>
{
    cfg.Host("rabbitmq://localhost");
    cfg.UseInMemoryOutbox();

    cfg.ConfigureEndpoints(context);
});
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
