using AutoMapper;
using MyExpenses.API.DTO;
using MyExpenses.API.Models.Domain;

namespace MyExpenses.API.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Bill, BillDTO>();
        }
    }
}
