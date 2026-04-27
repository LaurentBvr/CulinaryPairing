using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R11 — Accord piquant/doux (CdC v1.3 §2.5, principe 3).
/// Applicable : plat très piquant (niveau_piquant > 8).
/// Satisfaite : boisson à douceur résiduelle (niveau_sucre ≥ 4).
/// Justification : le sucre tempère le piquant et apaise les papilles.
/// Poids : 20.
/// </summary>
public class R11_PiquantDoux : IPairingRule
{
    public string Id => "R11";
    public int Poids => 20;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauPiquant is null || boisson.NiveauSucre is null)
            return PairingResult.NonApplicable();

        // Hors profil : plat pas suffisamment piquant
        if (recette.NiveauPiquant <= 8)
            return PairingResult.NonApplicable();

        return boisson.NiveauSucre >= 4
            ? PairingResult.Satisfait(
                $"Plat très piquant ({recette.NiveauPiquant}/10) tempéré par "
                + $"une boisson sucrée ({boisson.NiveauSucre}/10).")
            : PairingResult.NonSatisfait(
                $"Plat très piquant ({recette.NiveauPiquant}/10) : la boisson "
                + $"manque de sucre ({boisson.NiveauSucre}/10) pour apaiser.");
    }
}