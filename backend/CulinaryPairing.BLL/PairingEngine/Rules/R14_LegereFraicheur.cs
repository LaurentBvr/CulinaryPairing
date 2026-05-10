using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R14 — Accord léger/frais (CdC v1.3 §2.5, principe 2).
/// Applicable : plat délicat (gras < 4 ET intensité aromatique < 5).
/// Satisfaite : boisson légère (Corps = Leger) ou non-alcoolisée (mocktail/soda).
/// Justification : un vin lourd masquerait les saveurs délicates.
/// Poids : 15.
/// </summary>
public class R14_LegereFraicheur : IPairingRule
{
    public string Id => "R14";
    public int Poids => 15;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauGras is null || recette.IntensiteAromatique is null)
            return PairingResult.NonApplicable();

        // Hors profil : plat pas assez délicat
        if (recette.NiveauGras >= 4 || recette.IntensiteAromatique >= 5)
            return PairingResult.NonApplicable();

        var estLegere = boisson.Corps == CorpsBoisson.Leger || !boisson.Alcoolise;

        return estLegere
            ? PairingResult.Satisfait(
                "La délicatesse du plat est respectée par une boisson légère qui n'écrase pas les saveurs.")
            : PairingResult.NonSatisfait(
                "La boisson est trop corsée pour la délicatesse du plat : elle masquera ses saveurs subtiles.");
    }
}