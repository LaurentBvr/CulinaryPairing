using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R11bis — Sucre sur dessert (CdC v1.3 §2.5, principe 3).
/// Applicable : type_plat = dessert.
/// Satisfaite : boisson.niveau_sucre ≥ plat.niveau_sucre.
/// Justification : une boisson moins sucrée qu'un dessert paraît acide et amère.
/// Poids : 25 (la plus forte pondération avec R10bis).
/// </summary>
public class R11bis_SucreDessert : IPairingRule
{
    public string Id => "R11bis";
    public int Poids => 25;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.TypePlat != TypePlat.Dessert)
            return PairingResult.NonApplicable();

        if (recette.NiveauSucre is null || boisson.NiveauSucre is null)
            return PairingResult.NonApplicable();

        return boisson.NiveauSucre >= recette.NiveauSucre
            ? PairingResult.Satisfait(
                $"Dessert (sucre {recette.NiveauSucre}) accompagné d'une boisson "
                + $"au moins aussi sucrée ({boisson.NiveauSucre}).")
            : PairingResult.NonSatisfait(
                $"Boisson moins sucrée ({boisson.NiveauSucre}) que le dessert "
                + $"({recette.NiveauSucre}) : effet acide et amer.");
    }
}