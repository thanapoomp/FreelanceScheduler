using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using freelance.api.Data;
using freelance.api.Dtos;
using freelance.api.Helper;
using freelance.api.Models;
using freelance.api.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace freelance.api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public CustomersController(IConfiguration config,IMapper mapper,DataContext context)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name="GetCustomer")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _context.Customers.Where(x => x.Id == id && x.IsActive == true).FirstOrDefaultAsync();

            if (customer != null)
            {
                return Ok(customer);
            }

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery]CustomerParams customerParams)
        {
            var customers = _context.Customers
                            .Where(x =>
                            (
                                (x.Name.Contains(customerParams.SearchText)) ||
                                (x.Note.Contains(customerParams.SearchText)) ||
                                (x.PhoneNumber.Contains(customerParams.SearchText))
                            )
                            && (x.IsActive == true));

            var result = await PagedList<Customer>.CreateAsync(customers, customerParams.PageNumber, customerParams.PageSize);

            Response.AddPagination(result.CurrentPage,result.PageSize,result.TotalCount,result.TotalPages);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody]CustomerForCreateDto customerForCreateDto)
        {
            var currentUser = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var customer = _mapper.Map<Customer>(customerForCreateDto);
            customer.CreatedBy = currentUser;
            customer.CreatedDate = DateTime.Now;
            customer.LastUpdatedBy = currentUser;
            customer.LastUpdate = DateTime.Now;
            customer.IsActive = true;
            _context.Customers.Add(customer);

            if (await _context.SaveChangesAsync() > 0)
            {
                return CreatedAtRoute("GetCustomer", new {id = customer.Id},customer);
            }

            throw new System.Exception("Creating customer failed on save");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer(int id,CustomerForUpdateDto customerForUpdate)
        {
            var customerFromRepo = await _context.Customers.Where(x => x.Id == id && x.IsActive == true).FirstOrDefaultAsync();

            _mapper.Map(customerForUpdate, customerFromRepo);
            customerFromRepo.LastUpdate = DateTime.Now;
            customerFromRepo.LastUpdatedBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Update successfully.");
            }

            throw new System.Exception("Updating for customer failed on server.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customerToDelete = await _context.Customers.Where(x => x.Id == id && x.IsActive == true).FirstOrDefaultAsync();
            customerToDelete.DeletedBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            customerToDelete.DeletedDate = DateTime.Now;
            customerToDelete.IsActive = false;

            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok("Deleted succesfully.");
            }

            throw new System.Exception("Deleting for customer failed on server");
        }
    }
}