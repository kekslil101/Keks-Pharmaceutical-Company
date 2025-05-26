using System.ComponentModel.DataAnnotations;

namespace Pharmacy_Management_System.Models;

public class Agent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Gender { get; set; }

    public string Role { get; set; } = "Agent"; // default role
}
