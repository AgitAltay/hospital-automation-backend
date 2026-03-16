using Hospital.Domain.Entities;
using Hospital.Domain.Interfaces;
using Hospital.Infrastructure.Data;

namespace Hospital.Infrastructure.Repositories
{
    public class AIFeedbackRepository : GenericRepository<AIFeedback>, IAIFeedbackRepository
    {
        public AIFeedbackRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
