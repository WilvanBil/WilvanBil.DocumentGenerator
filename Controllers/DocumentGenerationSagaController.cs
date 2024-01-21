using MassTransit;
using Microsoft.AspNetCore.Mvc;
using WilvanBil.DocumentGenerator.Api.Sagas.DocumentGeneration;
using WilvanBil.DocumentGenerator.Contracts;

namespace WilvanBil.DocumentGenerator.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DocumentGenerationSagaController(IPublishEndpoint publishEndpoint, IRequestClient<GenerateDocumentCommand> requestClient) : ControllerBase
{
    [HttpGet()]
    public IEnumerable<Guid> Get()
    {
        var dictionaries = GetDictionaries(5000);
        publishEndpoint.Publish(new GenerateDocumentsCommand(Guid.NewGuid(), Guid.NewGuid(), dictionaries.ToArray()));

        return [Guid.NewGuid()];
    }

    [HttpGet("id")]
    public async Task<IEnumerable<Guid>> Get(Guid Id)
    {
        var status = await requestClient.GetResponse<DocumentGenerationState>(new { CorrelationId = Id });

        Console.WriteLine(status.Message.DocumentGeneratedCount);
        return [Guid.NewGuid()];
    }


    private IEnumerable<Dictionary<string, object>> GetDictionaries(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new Dictionary<string, object>();
        }
    }
}
