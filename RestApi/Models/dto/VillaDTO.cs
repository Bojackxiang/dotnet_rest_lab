using System.ComponentModel.DataAnnotations;

namespace RestApi.Models;

public class VillaDTO
{
    public string Id { get; set; }

    [Required] [MaxLength(5)] public string Name { get; set; }

    public DateTime DateTime { get; set; }
}