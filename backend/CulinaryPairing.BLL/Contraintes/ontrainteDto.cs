namespace CulinaryPairing.BLL.Contraintes;

/// <summary>DTO d'une contrainte alimentaire (catalogue + contraintes user).</summary>
public class ContrainteDto
{
    public int IdContrainte { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}

/// <summary>Payload PUT /api/contraintes/me : remplace toutes les contraintes du user.</summary>
public class UpdateContraintesDto
{
    public List<int> IdsContraintes { get; set; } = new();
}