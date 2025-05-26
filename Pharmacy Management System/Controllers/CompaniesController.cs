using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy_Management_System.Models;
using Pharmacy_Management_System.Data;


namespace Pharmacy_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompaniesController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public CompaniesController(PharmacyDbContext context)
        {
            _context = context;
        }

        // GET: api/Companies
        [HttpGet]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _context.Companies.ToListAsync();
            return Ok(companies);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound("Company not found.");

            return Ok(company);
        }

        // POST: api/Companies
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCompany(Company company)
        {
            if (await _context.Companies.AnyAsync(c => c.Phone == company.Phone))
                return BadRequest("Phone number already in use.");

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCompany(int id, Company updatedCompany)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound("Company not found.");

            company.CompanyName = updatedCompany.CompanyName;
            company.Address = updatedCompany.Address;
            company.Experience = updatedCompany.Experience;
            company.Phone = updatedCompany.Phone;

            await _context.SaveChangesAsync();
            return Ok("Company updated successfully.");
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
                return NotFound("Company not found.");

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return Ok("Company deleted.");
        }
    }
}
