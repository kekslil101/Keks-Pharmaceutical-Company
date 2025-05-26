using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacy_Management_System.Models;
using Pharmacy_Management_System.Data;

namespace Pharmacy_Management_System.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgentsController : ControllerBase
{
    private readonly PharmacyDbContext _context;

    public AgentsController(PharmacyDbContext context)
    {
        _context = context;
    }

    // ✅ Allow Admin and Agent to read agents
    [HttpGet]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> GetAgents()
    {
        var agents = await _context.Agents.ToListAsync();
        if (agents == null || !agents.Any())
            return NotFound("No agents found.");

        return Ok(agents);
    }

    // ✅ Get one agent by ID
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Agent")]
    public async Task<IActionResult> GetAgent(int id)
    {
        var agent = await _context.Agents.FindAsync(id);
        if (agent == null)
            return NotFound("Agent not found.");

        return Ok(agent);
    }

    // ✅ Only Admin can create
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateAgent(Agent agent)
    {
        if (await _context.Agents.AnyAsync(a => a.Phone == agent.Phone))
            return BadRequest("Agent with this phone already exists.");

        agent.Password = BCrypt.Net.BCrypt.HashPassword(agent.Password);
        _context.Agents.Add(agent);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAgent), new { id = agent.Id }, agent);
    }

    // ✅ Only Admin can update
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAgent(int id, Agent updatedAgent)
    {
        var existing = await _context.Agents.FindAsync(id);
        if (existing == null)
            return NotFound("Agent not found.");

        existing.Name = updatedAgent.Name;
        existing.Age = updatedAgent.Age;
        existing.Phone = updatedAgent.Phone;
        existing.Gender = updatedAgent.Gender;

        if (!string.IsNullOrEmpty(updatedAgent.Password))
            existing.Password = BCrypt.Net.BCrypt.HashPassword(updatedAgent.Password);

        await _context.SaveChangesAsync();
        return Ok("Agent updated successfully.");
    }

    // ✅ Only Admin can delete
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAgent(int id)
    {
        var agent = await _context.Agents.FindAsync(id);
        if (agent == null)
            return NotFound("Agent not found.");

        _context.Agents.Remove(agent);
        await _context.SaveChangesAsync();

        return Ok("Agent deleted successfully.");
    }
}
