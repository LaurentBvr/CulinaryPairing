using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R10bis — Équilibre des intensités aromatiques (CdC v1.3 §2.5, principe 1).
/// Condition : |intensite_plat − intensite_boisson| ≤ 2.
/// Justification : un écart trop important fait que l'un des deux masque l'autre.
/// Poids : 25 (la plus forte pondération avec R11bis).
/// Non applicable si l'intensité n'est pas renseignée d'un des deux côtés.
/// </summary>
public class R10bis_IntensiteAromatique : IPairingRule
{
    public string Id => "R10bis";
    public int Poids => 25;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.IntensiteAromatique is null || boisson.IntensiteAromatique is null)
            return PairingResult.NonApplicable();

        var ecart = Math.Abs(recette.IntensiteAromatique.Value - boisson.IntensiteAromatique.Value);

        return ecart <= 2
            ? PairingResult.Satisfait(
                $"Intensités aromatiques équilibrées (écart = {ecart} ≤ 2).")
            : PairingResult.NonSatisfait(
                $"Écart d'intensité trop important ({ecart} > 2) : l'un masque l'autre.");
    }
}