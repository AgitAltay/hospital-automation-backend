using System.Text;
using System.Text.Json;
using AIService.Interface;
using AIService.Models;

namespace AIService;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:8003";
    private readonly ILocalAIFilterService _localAIFilterService;
    
    public AIService(ILocalAIFilterService localAIFilterService)
    {
        _httpClient = new HttpClient();
        _localAIFilterService = localAIFilterService;
    }
    
    public async Task<PredictionResponse> Predict(string complaintText)
    {
        // Önce yerel ML modeline metnin anlamlı olup olmadığını sor
        bool isMeaningful = _localAIFilterService.IsMeaningful(complaintText);
        if (!isMeaningful)
        {
            // Metin anlamsızsa (rastgele harflerse) Python API'sine gitmeden isteği reddet.
            return new PredictionResponse 
            { 
                Label = "Geçersiz", 
                Score = 0.0f,
                ErrorMessage = "Lütfen geçerli ve anlamlı bir şikayet metni giriniz." 
            };
        }

        var request = new ComplaintRequest { Text = complaintText };
        var jsoncontent = JsonSerializer.Serialize(request);
        var httpContent = new StringContent(jsoncontent, Encoding.UTF8, "application/json");
        try
        {
            var response = await _httpClient.PostAsync($"{_baseUrl}/predict", httpContent);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var prediction = JsonSerializer.Deserialize<PredictionResponse>(responseString);
            if (prediction == null)
            {
                throw new InvalidOperationException("Deserialize response Başarısız");
            }
            return prediction;
        }
        catch (Exception e)
        {
            throw new Exception("Predict başarısız: ", e);
        }
    }
}