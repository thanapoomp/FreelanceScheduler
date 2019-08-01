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
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        private readonly DataContext _context;
        public ProductsController(IConfiguration config, IMapper mapper, DataContext context)
        {
            _config = config;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var c = await _context.Customers.CountAsync();

            return Ok(c);
        }
    }
}