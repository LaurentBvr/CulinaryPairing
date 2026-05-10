using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R20bis — Mode de cuisson et affinité tannique (CdC v1.3 §2.5, principe 7).
/// Applicable : cuisson sèche ∈ {grille, roti, four} ET affinite_tannins = friendly.
/// Satisfaite : vin rouge structuré (tannins ≥ 6 ET corps ∈ {Moyen, Corse}).
/// Justification : la caramélisation/Maillard s'accorde aux tannins structurés
///                 (classique « steak BBQ + Cabernet »).
/// Poids : 10.
/// </summary>
public class R20bis_CuissonTanninsStructures : IPairingRule
{
    public string Id => "R20bis";
    public int Poids => 10;

    private static readonly HashSet<ModeCuisson> CuissonsSeches = new()
    {
        ModeCuisson.Grille, ModeCuisson.Roti, ModeCuisson.Four
    };

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.ModeCuisson is null
            || !CuissonsSeches.Contains(recette.ModeCuisson.Value)
            || recette.AffiniteTannins != AffiniteTannins.Friendly)
            return PairingResult.NonApplicable();

        if (boisson.NiveauTannins is null || boisson.Corps is null)
            return PairingResult.NonApplicable();

        var corpsOk = boisson.Corps == CorpsBoisson.Moyen
                      || boisson.Corps == CorpsBoisson.Corse;
        var tanninsOk = boisson.NiveauTannins >= 6;

        var libelleCuisson = recette.ModeCuisson.Value switch
        {
            ModeCuisson.Grille => "La cuisson au gril",
            ModeCuisson.Roti => "La cuisson rôtie",
            ModeCuisson.Four => "La cuisson au four",
            _ => "La cuisson"
        };

        return tanninsOk && corpsOk
            ? PairingResult.Satisfait(
                $"{libelleCuisson} appelle un vin structuré : accord classique avec un vin tannique.")
            : PairingResult.NonSatisfait(
                $"{libelleCuisson} appelle un vin structuré, mais la boisson manque de corps ou de tannins.");
    }
}