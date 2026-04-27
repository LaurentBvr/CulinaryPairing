namespace CulinaryPairing.BLL.PairingEngine;

/// <summary>
/// Résultat de l'évaluation d'une règle d'accord (cf. CdC v1.3 §2.5).
/// Trois états possibles : non applicable / applicable+satisfaite / applicable+non-satisfaite.
/// </summary>
public class PairingResult
{
    public bool Applicable { get; init; }
    public bool Satisfaite { get; init; }
    public string? Justification { get; init; }

    private PairingResult() { }

    /// <summary>Règle hors de son domaine d'application (ex : R11bis sur un plat ≠ dessert).</summary>
    public static PairingResult NonApplicable() =>
        new() { Applicable = false, Satisfaite = false };

    /// <summary>Règle applicable et satisfaite (le poids s'ajoute au score).</summary>
    public static PairingResult Satisfait(string justification) =>
        new() { Applicable = true, Satisfaite = true, Justification = justification };

    /// <summary>Règle applicable mais non satisfaite (le poids compte dans la confiance, pas dans le score).</summary>
    public static PairingResult NonSatisfait(string justification) =>
        new() { Applicable = true, Satisfaite = false, Justification = justification };
}