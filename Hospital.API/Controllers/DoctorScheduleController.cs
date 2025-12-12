using Hospital.Application.DTOs.ScheduleDTOs;
using Hospital.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Doctor")] 
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorScheduleService _scheduleService;

        public DoctorScheduleController(IDoctorScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        private int GetCurrentDoctorId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null) throw new UnauthorizedAccessException("Doktor kimliği doğrulanamadı.");
            return int.Parse(claim.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetMySchedules()
        {
            var schedules = await _scheduleService.GetSchedulesByDoctorIdAsync(GetCurrentDoctorId());
            return Ok(schedules);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _scheduleService.CreateAsync(createDto, GetCurrentDoctorId());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateScheduleDto updateDto)
        {
            try
            {
                await _scheduleService.UpdateAsync(updateDto, GetCurrentDoctorId());
                return Ok(new { Message = "Çalışma saati güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _scheduleService.DeleteAsync(id, GetCurrentDoctorId());
                return Ok(new { Message = "Kayıt silindi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}