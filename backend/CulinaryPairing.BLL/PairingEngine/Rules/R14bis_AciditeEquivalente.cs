using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R14bis — Acidité équivalente (CdC v1.3 §2.5, principe 2).
/// Applicable : les deux niveaux d'acidité sont renseignés.
/// Satisfaite : boisson.niveau_acidite ≥ plat.niveau_acidite.
/// Justification : une boisson moins acide que le plat paraît plate.
/// Poids : 15.
/// </summary>
public class R14bis_AciditeEquivalente : IPairingRule
{
    public string Id => "R14bis";
    public int Poids => 15;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauAcidite is null || boisson.NiveauAcidite is null)
            return PairingResult.NonApplicable();

        return boisson.NiveauAcidite >= recette.NiveauAcidite
            ? PairingResult.Satisfait(
                $"Acidité boisson ({boisson.NiveauAcidite}) ≥ acidité plat "
                + $"({recette.NiveauAcidite}) : équilibre respecté.")
            : PairingResult.NonSatisfait(
                $"Boisson moins acide ({boisson.NiveauAcidite}) que le plat "
                + $"({recette.NiveauAcidite}) : risque d'effet plat.");
    }
}