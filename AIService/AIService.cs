using System.Text;
using System.Text.Json;
using AIService.Interface;
using AIService.Models;

namespace AIService;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:8002";
    
    public AIService()
    {
        _httpClient = new HttpClient();
    }
    
    public async Task<PredictionResponse> Predict(string complaintText)
    {
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
            Console.WriteLine(e + "Predict Yapılamadı");
            throw;
        }
    }
}