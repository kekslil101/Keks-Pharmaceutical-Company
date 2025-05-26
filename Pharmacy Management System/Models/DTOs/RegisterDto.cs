namespace Pharmacy_Management_System.Models.DTOs;
public class RegisterDto
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string Gender { get; set; }
    public string Role { get; set; } = "Agent"; // Optional: default to Agent
}
