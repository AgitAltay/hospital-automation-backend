using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

    
    
        [HttpPost("public/create")]
        [AllowAnonymous] 
        public async Task<IActionResult> CreatePublic([FromBody] CreateAppointmentPublicDto input)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                await _appointmentService.CreatePublicAsync(input);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Randevunuz başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

  
        [HttpPost("public/search")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchPublic([FromBody] ValidatePatientDto input)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var appointments = await _appointmentService.SearchPublicAsync(input);
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("public/cancel")]
        [AllowAnonymous]
        public async Task<IActionResult> CancelPublic([FromBody] CancelAppointmentPublicDto input)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                await _appointmentService.CancelPublicAsync(input);
                return Ok(new { Message = "Randevunuz başarıyla iptal edildi." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı")) return NotFound(new { Message = ex.Message });
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto createDto)
        {
            try
            {
                await _appointmentService.CreateAsync(createDto);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Randevu başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update([FromBody] UpdateAppointmentDto updateDto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Doctor")
                {
 
                }

                await _appointmentService.UpdateAsync(updateDto);
                return Ok(new { Message = "Randevu başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı")) return NotFound(new { Message = ex.Message });
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPut("cancel/{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Cancel(int id, [FromBody] string cancellationReason)
        {
            try
            {
                var appointment = await _appointmentService.GetByIdAsync(id);
                if (appointment == null) return NotFound(new { Message = "Randevu bulunamadı." });

                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Doctor" && appointment.DoctorId != userId)
                {
                     return Unauthorized(new { Message = "Sadece size ait bir randevuyu iptal edebilirsiniz." });
                }
     
                await _appointmentService.CancelAppointmentAsync(id, cancellationReason);
                return Ok(new { Message = "Randevu başarıyla iptal edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            return Ok(appointment);
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllByPatientId(int patientId)
        {
            var appointments = await _appointmentService.GetAllByPatientIdAsync(patientId);
            return Ok(appointments);
        }


        [HttpGet("doctor/{doctorId}/schedule")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetDoctorSchedule(int doctorId, [FromQuery] DateTime date)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Doctor" && doctorId != userId)
            {
                return Unauthorized(new { Message = "Sadece kendi randevu programınızı görüntüleyebilirsiniz." });
            }

            var appointments = await _appointmentService.GetAllByDoctorIdAsync(doctorId, date);
            return Ok(appointments);
        }

 
        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(
            [FromQuery] int? doctorId,
            [FromQuery] int? patientId,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var appointments = await _appointmentService.SearchAppointmentsAsync(doctorId, patientId, startDate, endDate);
            return Ok(appointments);
        }
        
        [HttpGet("my-appointments")]
        [Authorize(Roles = "Doctor")] 
        public async Task<IActionResult> GetMyAppointments()
        {
            try
            {
                var doctorIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (doctorIdClaim == null)
                {
                    return Unauthorized(new { Message = "Kimlik bilgisi doğrulanamadı." });
                }

                int doctorId = int.Parse(doctorIdClaim.Value);

                var result = await _appointmentService.GetDoctorAppointmentsAsync(doctorId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        
        [HttpGet("available-slots")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetAvailableSlots(int doctorId, DateTime date)
        {
            try
            {
                if (date.Date < DateTime.Today)
                {
                    return BadRequest(new { Message = "Geçmiş bir tarih için randevu sorgulanamaz." });
                }

                var slots = await _appointmentService.GetAvailableSlotsAsync(doctorId, date);
                return Ok(slots);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}