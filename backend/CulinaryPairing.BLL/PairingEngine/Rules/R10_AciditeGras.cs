using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R10 — Accord gras/acide (CdC v1.3 §2.5, principe 2).
/// Applicable : plat très gras (>7) ET peu acide (<4).
/// Satisfaite : boisson franchement acide (≥6) qui « coupe » le gras.
/// Poids : 20.
/// </summary>
public class R10_AciditeGras : IPairingRule
{
    public string Id => "R10";
    public int Poids => 20;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauGras is null || recette.NiveauAcidite is null
            || boisson.NiveauAcidite is null)
            return PairingResult.NonApplicable();

        // Hors profil : plat ni gras ni faible en acidité
        if (recette.NiveauGras <= 7 || recette.NiveauAcidite >= 4)
            return PairingResult.NonApplicable();

        return boisson.NiveauAcidite >= 6
            ? PairingResult.Satisfait(
                $"Plat gras+peu acide (gras {recette.NiveauGras}, acidité {recette.NiveauAcidite}) "
                + $"équilibré par une boisson acide ({boisson.NiveauAcidite}/10).")
            : PairingResult.NonSatisfait(
                $"Plat gras+peu acide : la boisson manque d'acidité ({boisson.NiveauAcidite}/10) "
                + "pour couper le gras.");
    }
}