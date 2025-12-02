using Hospital.Application.DTOs.SpecialtyDTOs;
using Hospital.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var specialties = await _specialtyService.GetAllAsync();
            return Ok(specialties);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var specialty = await _specialtyService.GetByIdAsync(id);
                return Ok(specialty);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto createDto)
        {
            try
            {
                await _specialtyService.CreateAsync(createDto);
                return StatusCode(StatusCodes.Status201Created, new { Message = "Uzmanlık alanı başarıyla oluşturuldu." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UpdateSpecialtyDto updateDto)
        {
            try
            {
                await _specialtyService.UpdateAsync(updateDto);
                return Ok(new { Message = "Uzmanlık alanı başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                    return NotFound(new { Message = ex.Message });

                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _specialtyService.DeleteAsync(id);
                return Ok(new { Message = "Uzmanlık alanı başarıyla silindi." });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("bulunamadı"))
                    return NotFound(new { Message = ex.Message });

                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}