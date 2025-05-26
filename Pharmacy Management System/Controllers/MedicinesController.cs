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
    public class MedicinesController : ControllerBase
    {
        private readonly PharmacyDbContext _context;

        public MedicinesController(PharmacyDbContext context)
        {
            _context = context;
        }

        // GET: api/Medicines
        [HttpGet]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetMedicines()
        {
            var medicines = await _context.Medicines
                .Include(m => m.Company)
                .ToListAsync();

            return Ok(medicines);
        }

        // GET: api/Medicines/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<IActionResult> GetMedicine(int id)
        {
            var medicine = await _context.Medicines
                .Include(m => m.Company)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medicine == null)
                return NotFound("Medicine not found.");

            return Ok(medicine);
        }

        // POST: api/Medicines
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateMedicine(Medicine medicine)
        {
            if (!await _context.Companies.AnyAsync(c => c.Id == medicine.CompanyId))
                return BadRequest("Invalid CompanyId.");

            if (medicine.ExpiryDate <= medicine.ManufactureDate)
                return BadRequest("Expiry date must be after manufacture date.");

            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMedicine), new { id = medicine.Id }, medicine);
        }

        // PUT: api/Medicines/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateMedicine(int id, Medicine updatedMedicine)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
                return NotFound("Medicine not found.");

            if (!await _context.Companies.AnyAsync(c => c.Id == updatedMedicine.CompanyId))
                return BadRequest("Invalid CompanyId.");

            medicine.Name = updatedMedicine.Name;
            medicine.Price = updatedMedicine.Price;
            medicine.Quantity = updatedMedicine.Quantity;
            medicine.ManufactureDate = updatedMedicine.ManufactureDate;
            medicine.ExpiryDate = updatedMedicine.ExpiryDate;
            medicine.CompanyId = updatedMedicine.CompanyId;

            await _context.SaveChangesAsync();
            return Ok("Medicine updated successfully.");
        }

        // DELETE: api/Medicines/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicine = await _context.Medicines.FindAsync(id);
            if (medicine == null)
                return NotFound("Medicine not found.");

            _context.Medicines.Remove(medicine);
            await _context.SaveChangesAsync();
            return Ok("Medicine deleted.");
        }
    }
}
