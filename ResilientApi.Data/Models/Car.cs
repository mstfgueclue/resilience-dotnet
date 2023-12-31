namespace ResilientApi.Data.Models;

public class Car
{
    public int Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public string LicensePlate { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    
    public int? OwnerId { get; set; }
    public Owner? Owner { get; set; }
}