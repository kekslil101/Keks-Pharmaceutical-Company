
namespace Pharmacy_Management_System.Models;

public class Medicine
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public int Quantity { get; set; }
    public DateTime ManufactureDate { get; set; }
    public DateTime ExpiryDate { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; }  // Navigation
}
