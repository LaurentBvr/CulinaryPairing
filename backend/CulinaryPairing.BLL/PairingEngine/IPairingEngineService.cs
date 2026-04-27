using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine;

/// <summary>
/// Moteur de scoring des accords mets-boissons (CdC v1.3 §2.5).
/// </summary>
public interface IPairingEngineService
{
    PairingEngineResult CalculerScore(Recette recette, Boisson boisson);
}

/// <summary>
/// Résultat agrégé du moteur (cf. pseudocode §2.5 étape 8).
/// Mappable directement vers ACCORD : score_compatibilite, niveau_confiance,
/// regles_satisfaites, malus_applique, version_moteur.
/// </summary>
public class PairingEngineResult
{
    public int Score { get; init; }                 // 0-100, après malus
    public int Confiance { get; init; }             // 0-100, % règles applicables
    public int MalusApplique { get; init; }         // R16bis
    public bool Eliminatoire { get; init; }         // true si malus > 20
    public List<string> ReglesSatisfaites { get; init; } = new();
    public string VersionMoteur { get; init; } = "1.3";
    public string Justification { get; init; } = "";
}