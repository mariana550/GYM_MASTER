using Microsoft.AspNetCore.Mvc;
using PROYECTO_GYM_MASTER.DTOs;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class ChatbotController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;

    public ChatbotController(IHttpClientFactory httpClientFactory, IConfiguration config)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatDTO dto)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_config["Groq:ApiKey"]}");

            var body = new
            {
                model = "llama-3.1-8b-instant",
                messages = new[]
                {
                    new {
                        role = "system",
                        content = "Eres el asistente virtual de GymMaster. Responde sobre rutinas, ejercicios y nutrición. Sé amable y conciso. Responde en español máximo 3 oraciones."
                    },
                    new {
                        role = "user",
                        content = dto.Mensaje
                    }
                },
                max_tokens = 300,
                temperature = 0.7
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine("API KEY: " + _config["Groq:ApiKey"]);

            var response = await client.PostAsync("https://api.groq.com/openai/v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            // Ver qué responde Groq
            Console.WriteLine("Groq response: " + responseString);

            if (!response.IsSuccessStatusCode)
                return BadRequest(new { respuesta = "Error conectando con el asistente: " + responseString });

            var jsonDoc = JsonDocument.Parse(responseString);
            var respuesta = jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return Ok(new { respuesta });
        }
        catch (Exception ex)
        {
            return BadRequest(new { respuesta = "Error: " + ex.Message });
        }
    }
}