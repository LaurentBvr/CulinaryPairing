using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R13 — Accord fumé/tannique (CdC v1.3 §2.5, principe 4).
/// Applicable : plat contient des notes fumées (contient_fume = true).
/// Satisfaite : boisson aux notes fumées (niveau_fume ≥ 4 :
///              vins rouges en fûts boisés, whiskies tourbés).
/// Poids : 15.
/// </summary>
public class R13_FumeTannique : IPairingRule
{
    public string Id => "R13";
    public int Poids => 15;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (!recette.ContientFume)
            return PairingResult.NonApplicable();

        if (boisson.NiveauFume is null)
            return PairingResult.NonApplicable();

        return boisson.NiveauFume >= 4
            ? PairingResult.Satisfait(
                "Les notes fumées du plat trouvent un écho dans la boisson : accord par résonance.")
            : PairingResult.NonSatisfait(
                "Le caractère fumé du plat ne trouve pas d'écho dans la boisson : occasion manquée.");
    }
}