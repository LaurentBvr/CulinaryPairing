using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R23bis — Sel et tannins (CdC v1.3 §2.5, principe 4) — NOUVEAU V1.3.
/// Applicable : plat salé (niveau_sel ≥ 6) ET boisson tannique (niveau_tannins ≥ 6).
/// Satisfaite : par construction (la simple combinaison bonifie l'accord —
///              charcuterie / fromage affiné + vin rouge tannique).
/// Justification : le sel diminue physiologiquement la perception de l'amertume
///                 et adoucit les tannins.
/// Poids : 10.
/// </summary>
public class R23bis_SelTannins : IPairingRule
{
    public string Id => "R23bis";
    public int Poids => 10;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauSel is null || boisson.NiveauTannins is null)
            return PairingResult.NonApplicable();

        // Hors profil : pas de combinaison sel élevé + tannins élevés
        if (recette.NiveauSel < 6 || boisson.NiveauTannins < 6)
            return PairingResult.NonApplicable();

        // Combinaison observée → bonification (toujours satisfaite quand applicable)
        return PairingResult.Satisfait(
            $"Plat salé ({recette.NiveauSel}/10) + boisson tannique "
            + $"({boisson.NiveauTannins}/10) : le sel adoucit les tannins.");
    }
}