using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor,Admin")]
    public class AIFeedbackController : ControllerBase
    {
        private readonly IAIFeedbackService _aiFeedbackService;

        public AIFeedbackController(IAIFeedbackService aiFeedbackService)
        {
            _aiFeedbackService = aiFeedbackService;
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitFeedback([FromBody] CreateAIFeedbackDto feedbackDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var doctorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

                await _aiFeedbackService.CreateFeedbackAsync(feedbackDto, doctorId);
                return Ok(new { Message = "Yapay zeka değerlendirmeniz başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
