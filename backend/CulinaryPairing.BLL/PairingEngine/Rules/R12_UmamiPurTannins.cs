using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R12 — Umami pur et tannins (CdC v1.3 §2.5, principe 4).
/// Applicable : plat contient de l'umami pur (champignons, parmesan vieux, sauce soja).
/// Satisfaite : boisson peu tannique (niveau_tannins ≤ 5).
/// Justification : tannins + umami pur = amertume métallique désagréable.
/// Poids : 20.
/// </summary>
public class R12_UmamiPurTannins : IPairingRule
{
    public string Id => "R12";
    public int Poids => 20;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (!recette.ContientUmamiPur)
            return PairingResult.NonApplicable();

        if (boisson.NiveauTannins is null)
            return PairingResult.NonApplicable();

        return boisson.NiveauTannins <= 5
            ? PairingResult.Satisfait(
                $"Plat à umami pur compatible avec une boisson peu tannique "
                + $"({boisson.NiveauTannins}/10 ≤ 5).")
            : PairingResult.NonSatisfait(
                $"Plat à umami pur + boisson tannique ({boisson.NiveauTannins}/10) "
                + ": amertume métallique attendue.");
    }
}