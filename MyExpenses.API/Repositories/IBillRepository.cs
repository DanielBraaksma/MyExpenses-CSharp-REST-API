using MyExpenses.API.DTO;
using MyExpenses.API.Models.Domain;

namespace MyExpenses.API.Repositories
{
    public interface IBillRepository
    {

        Task<List<Bill>> GetAllAsync(string userId);

        Task<Bill?> GetByIdAsync(Guid id);

        Task<Bill>CreateAsync(Bill bill);

        Task<Bill?> UpdateAsync(Guid id, AddBillRequestDTO bill);
    
        Task<Bill?> DeleteAsync(Guid id);
    }
}
