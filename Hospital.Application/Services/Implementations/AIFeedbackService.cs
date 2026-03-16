using AutoMapper;
using Hospital.Application.DTOs.AppointmentDTOs;
using Hospital.Application.Interfaces;
using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;

namespace Hospital.Application.Services.Implementations
{
    public class AIFeedbackService : IAIFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AIFeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateFeedbackAsync(CreateAIFeedbackDto feedbackDto, int doctorId)
        {
            var appointment = await _unitOfWork.Appointments.GetByIdAsync(feedbackDto.AppointmentId);
            
            if (appointment == null)
                throw new Exception("İlgili randevu bulunamadı.");

            if (appointment.DoctorId != doctorId)
                throw new Exception("Sadece kendi randevularınızı değerlendirebilirsiniz.");

            // Önceden feedback verilip verilmediğini kontrol et
            var existingFeedbacks = await _unitOfWork.AIFeedbacks.FindAsync(f => f.AppointmentId == feedbackDto.AppointmentId);
            if (existingFeedbacks.Any())
                throw new Exception("Bu randevu için yapay zeka değerlendirmesi daha önceden yapılmış.");

            var feedback = _mapper.Map<AIFeedback>(feedbackDto);
            feedback.DoctorId = doctorId;

            await _unitOfWork.AIFeedbacks.AddAsync(feedback);
            await _unitOfWork.CompleteAsync();
        }
    }
}
