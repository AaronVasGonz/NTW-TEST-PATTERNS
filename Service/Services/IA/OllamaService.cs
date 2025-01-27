using Microsoft.Extensions.Logging;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.IA;

public interface IOllamaService
{
    Task<string> GetOllamaResponseAsync(string userPrompt, string model);
}

public class OllamaService(Uri baseUri, ILogger<OllamaService> logger) : IOllamaService
{
    private readonly OllamaApiClient _ollamaApiClient = new OllamaApiClient(baseUri);
    private readonly ILogger<OllamaService> _logger = logger;

    public async Task<string> GetOllamaResponseAsync(string userPrompt, string model)
    {
        try
        {
            _ollamaApiClient.SelectedModel = model;
            var responseBuilder = new StringBuilder();
            await foreach (var stream in _ollamaApiClient.GenerateAsync(userPrompt))
            {
                responseBuilder.Append(stream.Response);
            }
            return responseBuilder.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Ollama response.");
            throw;
        }
    }
}
