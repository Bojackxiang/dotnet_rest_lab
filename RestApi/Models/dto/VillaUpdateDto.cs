using System.ComponentModel.DataAnnotations;

namespace RestApi.Models;

public class VillaUpdateDto
{
    [MaxLength(5)] public string Name { get; set; }

    public int Age { get; set; }
}