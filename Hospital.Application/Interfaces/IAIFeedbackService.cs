using Hospital.Application.DTOs.AppointmentDTOs;

namespace Hospital.Application.Interfaces
{
    public interface IAIFeedbackService
    {
        Task CreateFeedbackAsync(CreateAIFeedbackDto feedbackDto, int doctorId);
    }
}
