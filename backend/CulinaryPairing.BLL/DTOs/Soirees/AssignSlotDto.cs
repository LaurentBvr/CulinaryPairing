using System.ComponentModel.DataAnnotations;

namespace CulinaryPairing.BLL.DTOs.Soirees;

public class AssignSlotDto
{
    [Required]
    public int IdRecette { get; set; }
}