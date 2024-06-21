using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestApi.Models;

public class VillaDTO
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    [Required] [MaxLength(5)] public string Name { get; set; }

    public int Age { get; set; }

    public DateTime DateTime { get; set; }
}