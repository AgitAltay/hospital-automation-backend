using Microsoft.AspNetCore.Mvc;
using AIService;
using AIService.Interface;
using LoggerManager.Interface;
using Microsoft.AspNetCore.Authorization;

namespace Hospital.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AıPredictController : ControllerBase
{
    private readonly IAIService _aiService;
    private readonly ILoggerManager _logger;

    public AıPredictController(IAIService aiService, ILoggerManager logger)
    {
        _logger = logger;
        _aiService = aiService;
    }

    [HttpPost("predict")]
    [AllowAnonymous]
    public async Task<IActionResult> PredictCompilant(string complaintText)
    {
        try
        {
            var response = await _aiService.Predict(complaintText);
            var result = response.Label + "-------" +  response.Score;
            _logger.LogInfo(result + "Predict Başarılı.");
            return Ok(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message + "Predict Başarısız.");
            return BadRequest();
        }
    }
}