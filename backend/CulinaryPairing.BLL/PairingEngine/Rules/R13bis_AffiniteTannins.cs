using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R13bis — Affinité aux tannins (CdC v1.3 §2.5, principe 4).
/// Règle unifiée à 3 valeurs (friendly / neutre / hostile) qui remplace
/// l'ancien système de drapeaux binaires (V1.1).
///   - friendly : viandes rouges, fromages affinés → bonifier vins tanniques (≥6)
///   - hostile  : poisson cru, œuf, crustacés, asperge, artichaut → exclure tannins (>5)
///   - neutre   : règle NonApplicable
/// Poids : 20.
/// </summary>
public class R13bis_AffiniteTannins : IPairingRule
{
    public string Id => "R13bis";
    public int Poids => 20;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (boisson.NiveauTannins is null)
            return PairingResult.NonApplicable();

        var tannins = boisson.NiveauTannins.Value;

        return recette.AffiniteTannins switch
        {
            AffiniteTannins.Friendly => tannins >= 6
                ? PairingResult.Satisfait(
                    "Les viandes rouges et fromages affinés du plat appellent les tannins : la boisson tannique sublime l'accord.")
                : PairingResult.NonSatisfait(
                    "Le plat appelle des tannins structurants, mais la boisson en est dépourvue : structure absente."),

            AffiniteTannins.Hostile => tannins <= 5
                ? PairingResult.Satisfait(
                    "Le plat (poisson cru, œuf, asperge…) ne supporte pas les tannins : la boisson légère lui convient parfaitement.")
                : PairingResult.NonSatisfait(
                    "Le plat redoute les tannins, mais la boisson est tannique : risque de goût métallique en bouche."),

            _ => PairingResult.NonApplicable()  // Neutre
        };
    }
}