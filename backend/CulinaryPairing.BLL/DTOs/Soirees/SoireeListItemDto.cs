namespace CulinaryPairing.BLL.DTOs.Soirees;

public class SoireeListItemDto
{
    public int IdSoiree { get; set; }
    public int NombrePersonnes { get; set; }
    public string? TypeSoiree { get; set; }
    public DateTime DateCreation { get; set; }
    public int NbContraintes { get; set; }
    public bool MenuComplet { get; set; }
}