using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyExpenses.API.Data;
using MyExpenses.API.DTO;
using MyExpenses.API.Models.Domain;
using MyExpenses.API.Repositories;
using System.Security.Claims;

namespace MyExpenses.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillsController : ControllerBase
    {

        private readonly BillsDbContext dbContext;
        private readonly IBillRepository billRepository;
        private readonly IMapper mapper;
        public BillsController(BillsDbContext dbContext, IBillRepository billRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.billRepository = billRepository;
            this.mapper = mapper;
        }

        // Get all bills
        // https://localhost:7019/api/Bills
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = GetUserIdFromToken();

            if (userId == null)
            {
                return BadRequest("User not authenticated.");
            }

            var bills = await billRepository.GetAllAsync(userId);
           
            var billsDtos = mapper.Map<List<BillDTO>>(bills);

            return Ok(billsDtos);
        }

        // Get a bill
        // https://localhost:7019/api/Bill/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var bill = await billRepository.GetByIdAsync(id);

            if (bill == null)
            {
                return NotFound();
            }

            var billDto = mapper.Map<BillDTO>(bill);
            return Ok(billDto);
        }

        // create a bill
        // https://localhost:7019/api/Bill/
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddBillRequestDTO addBillRequestDTO)
        {
            if (ModelState.IsValid)
            {
                // will need to get id from Header here
                // var test = Guid.NewGuid();

                var userId = GetUserIdFromToken();

                if (userId == null)
                {
                    return BadRequest("User not authenticated.");
                }

                Guid idAsGuid = Guid.Parse(userId);

                var billDomainModel = new Bill
                {
                    UserId = idAsGuid,
                    Name = addBillRequestDTO.Name,
                    Amount = addBillRequestDTO.Amount,
                    Date = addBillRequestDTO.Date,
                    Category = addBillRequestDTO.Category,
                };

                billDomainModel = await billRepository.CreateAsync(billDomainModel);

                var billDto = mapper.Map<BillDTO>(billDomainModel);

                return CreatedAtAction(nameof(GetById), new { id = billDto.Id }, billDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateById([FromRoute] Guid id, AddBillRequestDTO addBillRequestDTO)
        {
            // Find the bill by id from the database.
            // var bill = dbContext.Bills.Find(id);
            var bill = await billRepository.UpdateAsync(id, addBillRequestDTO);

            // Check if the bill with the specified id exists.
            if (bill == null)
            {
                return NotFound();
            }
          
            return Ok("your bill was updated"); // Return a successful response with no content.
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBill ([FromRoute] Guid id)
        {
            var billDomainModel = await billRepository.DeleteAsync(id);
            if (billDomainModel == null)
            {
                return NotFound();
            }

            return Ok("your bill was deleted"); // Return a successful response with no content.
        }

        private string GetUserIdFromToken()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim != null)
            {
                return userIdClaim.Value;
            }

            return null; // Return null or handle the case where UserId cannot be extracted.
        }
    }
}