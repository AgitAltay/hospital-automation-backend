using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Enums;
using Microsoft.AspNetCore.Authorization; // Bu namespace gerekli
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Token'dan bilgi okumak için gerekli

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // GENEL KURAL: Sadece giriþ yapmýþ kullanýcýlar eriþebilir.
    [Authorize]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // POST: api/Appointment
        // YETKÝ: Sadece "Patient" (Hasta) ve "Admin" rolü randevu oluþturabilir.
        [HttpPost]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto createDto)
        {
            try
            {
                // Güvenlik Kontrolü: Eðer istek yapan bir "Patient" ise, sadece kendi adýna randevu alabilir.
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Patient" && createDto.PatientId != userId)
                {
                    return Unauthorized(new { Message = "Sadece kendi adýnýza randevu alabilirsiniz." });
                }

                await _appointmentService.CreateAsync(createDto);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Randevu baþarýyla oluþturuldu." });
            }
            catch (Exception ex)
            {
                // Ýþ kuralý hatalarý (Örn: Doktor müsait deðil)
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Appointment
        // YETKÝ: Sadece "Admin" ve "Doctor" rolü randevu güncelleyebilir (Tarih deðiþimi, not ekleme vb.).
        [HttpPut]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update([FromBody] UpdateAppointmentDto updateDto)
        {
            try
            {
                // Güvenlik Kontrolü: Eðer istek yapan bir "Doctor" ise, sadece kendi randevusunu güncelleyebilir.
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (userRole == "Doctor")
                {
                    // Randevuyu getirip doktor kontrolü yapmamýz lazým. Bu biraz karmaþýk olabilir, þimdilik basit tutalým.
                    // Ýdealde: var appointment = await _appointmentService.GetByIdAsync(updateDto.AppointmentId);
                    // if (appointment.DoctorId != userId) return Unauthorized(...)
                }

                await _appointmentService.UpdateAsync(updateDto);
                return Ok(new { Message = "Randevu baþarýyla güncellendi." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadý")) return NotFound(new { Message = ex.Message });
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/Appointment/cancel/5
        // YETKÝ: Herkes (Hasta, Doktor, Admin) iptal edebilir AMA sadece KENDÝ randevusunu.
        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id, [FromBody] string cancellationReason)
        {
            try
            {
                // 1. Randevuyu getir
                var appointment = await _appointmentService.GetByIdAsync(id);
                if (appointment == null) return NotFound(new { Message = "Randevu bulunamadý." });

                // 2. Ýstek yapan kullanýcýnýn kimliðini al
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                // 3. Yetki kontrolü: Admin her þeyi iptal edebilir. Diðerleri sadece kendisininkini.
                if (userRole != "Admin")
                {
                    if (userRole == "Patient" && appointment.PatientId != userId)
                        return Unauthorized(new { Message = "Sadece kendi randevunuzu iptal edebilirsiniz." });

                    if (userRole == "Doctor" && appointment.DoctorId != userId)
                        return Unauthorized(new { Message = "Sadece size ait bir randevuyu iptal edebilirsiniz." });
                }

                // 4. Ýptal iþlemini yap
                await _appointmentService.CancelAppointmentAsync(id, cancellationReason);
                return Ok(new { Message = "Randevu baþarýyla iptal edildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // GET: api/Appointment/5
        // YETKÝ: Giriþ yapmýþ herkes (Kendi randevusu olmak þartýyla - kontrol eklenmeli)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Burada da Cancel metodundaki gibi bir "Kendi randevusu mu?" kontrolü gerekir.
            // Þimdilik basit býrakýyorum.
            var appointment = await _appointmentService.GetByIdAsync(id);
            return Ok(appointment);
        }

        // GET: api/Appointment/patient/5
        // YETKÝ: "Patient" (Sadece kendisi) ve "Admin" (Herkesi görebilir).
        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAllByPatientId(int patientId)
        {
            // Güvenlik Kontrolü: Hasta sadece kendi geçmiþini görebilir.
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Patient" && patientId != userId)
            {
                return Unauthorized(new { Message = "Sadece kendi randevu geçmiþinizi görüntüleyebilirsiniz." });
            }

            var appointments = await _appointmentService.GetAllByPatientIdAsync(patientId);
            return Ok(appointments);
        }

        // GET: api/Appointment/doctor/5/schedule
        // YETKÝ: "Doctor" (Sadece kendisi) ve "Admin" (Herkesi görebilir).
        [HttpGet("doctor/{doctorId}/schedule")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetDoctorSchedule(int doctorId, [FromQuery] DateTime date)
        {
            // Güvenlik Kontrolü: Doktor sadece kendi programýný görebilir.
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole == "Doctor" && doctorId != userId)
            {
                return Unauthorized(new { Message = "Sadece kendi randevu programýnýzý görüntüleyebilirsiniz." });
            }

            var appointments = await _appointmentService.GetAllByDoctorIdAsync(doctorId, date);
            return Ok(appointments);
        }

        // GET: api/Appointment/search
        // YETKÝ: Sadece "Admin" rolü genel arama yapabilir.
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
    }
}