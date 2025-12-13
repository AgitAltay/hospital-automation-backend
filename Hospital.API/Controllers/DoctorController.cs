using Hospital.Application.DTOs.DoctorDTOs;
using Hospital.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] 
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var doctor = await _doctorService.GetByIdAsync(id);
                return Ok(doctor);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpGet("specialty/{specialtyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBySpecialty(int specialtyId)
        {
            try
            {
                var doctors = await _doctorService.GetBySpecialtyIdAsync(specialtyId);
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Mutlaka eklenmeli ileride
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto createDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);


                await _doctorService.CreateAsync(createDto);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Doktor başarıyla sisteme kaydedildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateDoctorDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                await _doctorService.UpdateAsync(updateDto);
                return Ok(new { Message = "Doktor bilgileri güncellendi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")] 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _doctorService.DeleteAsync(id);
                return Ok(new { Message = "Doktor silindi (Soft delete)." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}