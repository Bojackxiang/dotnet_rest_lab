using System.ComponentModel.DataAnnotations;

namespace RestApi.Models;

public class VillaCreateDTO
{
    [Required] [MaxLength(5)] public string Name { get; set; }

    public int Age { get; set; }

    public DateTime DateTime { get; set; }
}