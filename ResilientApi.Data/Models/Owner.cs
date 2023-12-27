using System.Text.Json.Serialization;

namespace ResilientApi.Data.Models;

public class Owner
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    
    [JsonIgnore]
    public ICollection<Car> Cars { get; set; }
}