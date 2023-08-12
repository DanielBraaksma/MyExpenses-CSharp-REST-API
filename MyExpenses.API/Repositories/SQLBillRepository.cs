using Microsoft.EntityFrameworkCore;
using MyExpenses.API.Data;
using MyExpenses.API.DTO;
using MyExpenses.API.Models.Domain;

namespace MyExpenses.API.Repositories
{
    public class SQLBillRepository :IBillRepository
    {
        public SQLBillRepository(BillsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private BillsDbContext dbContext;

        public async Task<Bill> CreateAsync(Bill bill)
        {
            await dbContext.AddAsync(bill);
            await dbContext.SaveChangesAsync();
            return bill;
        }

        public async Task<List<Bill>> GetAllAsync(string userId)
        {
            Guid idAsGuid = Guid.Parse(userId);
            return await dbContext.Bills.Where(b => b.UserId == idAsGuid).ToListAsync();
        }

        public async Task<Bill?> GetByIdAsync(Guid id)
        {
           return await dbContext.Bills.FirstOrDefaultAsync(x=> x.Id ==  id);
        }

        public async Task<Bill?> UpdateAsync(Guid id, AddBillRequestDTO addBillRequestDTO)
        {
            var existingBill = await dbContext.Bills.FirstOrDefaultAsync(x => x.Id == id);
            
            if  (existingBill == null)
            {
                return null;
            }

            existingBill.Name = addBillRequestDTO.Name;
            existingBill.Amount = addBillRequestDTO.Amount;
            existingBill.Date = addBillRequestDTO.Date;
            existingBill.Category = addBillRequestDTO.Category;

            await dbContext.SaveChangesAsync();
            return existingBill;
        }

        public async Task<Bill?> DeleteAsync(Guid id)
        {
            var existingBill = await dbContext.Bills.FirstOrDefaultAsync(x => x.Id == id);
        
            if (existingBill == null)
            {
                return null;
            }

            dbContext.Bills.Remove(existingBill);
            await dbContext.SaveChangesAsync();
            return existingBill;
        }
    }
}
