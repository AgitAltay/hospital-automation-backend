using AIService.Models;

namespace AIService.Interface;

public interface IAIService
{
    Task<PredictionResponse> Predict(string text);
}