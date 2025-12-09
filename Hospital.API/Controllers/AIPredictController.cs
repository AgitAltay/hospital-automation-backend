using Microsoft.AspNetCore.Mvc;
using AIService;
using AIService.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AıPredictController : ControllerBase
{
    public readonly IAIService _aiService;

    public AıPredictController(IAIService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost("predict")]
    [AllowAnonymous]
    public async Task<IActionResult> PredictCompilant(string complaintText)
    {
        try
        {
            var response = await _aiService.Predict(complaintText);
            return (Ok(response.Label + "   -----  " + response.Score));
        }
        catch (Exception e)
        {
            Console.WriteLine(e + "Predict Başarısız");
            throw;
        }
    }
}