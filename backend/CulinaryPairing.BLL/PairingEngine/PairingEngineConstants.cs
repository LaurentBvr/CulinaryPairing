namespace CulinaryPairing.BLL.PairingEngine;

/// <summary>
/// Constantes du moteur d'accord. Single source of truth pour la version moteur :
/// tout bump de version se fait ici et propage automatiquement vers le service,
/// le DTO de résultat, le service AccordsService et le DTO API (AccordDto).
/// </summary>
public static class PairingEngineConstants
{
    public const string VersionMoteur = "1.3.1";
}