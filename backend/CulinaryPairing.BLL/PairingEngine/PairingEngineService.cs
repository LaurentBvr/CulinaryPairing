using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine;

/// <summary>
/// Moteur d'accords (CdC v1.3 §2.5).
/// Pattern Strategy : les 16 règles sont injectées via DI et évaluées en boucle.
/// Le malus gradué R16bis est traité hors boucle (avant et après le score brut).
/// </summary>
public class PairingEngineService : IPairingEngineService
{
    private const string VersionMoteur = "1.3";
    private const int PoidsTotalMaximum = 260; // 25+25 + 20×4 + 15×6 + 10×4 (cf. CdC §2.5)

    private readonly IEnumerable<IPairingRule> _regles;

    public PairingEngineService(IEnumerable<IPairingRule> regles)
    {
        _regles = regles;
    }

    public PairingEngineResult CalculerScore(Recette recette, Boisson boisson)
    {
        // Étape 1 : Malus gradué R16bis (alcool fort + plat piquant)
        // Si l'une des deux données est manquante, malus non calculable → 0.
        var malus = 0;
        if (boisson.DegreAlcool is not null && recette.NiveauPiquant is not null)
        {
            malus = (int)Math.Max(0m,
                (boisson.DegreAlcool.Value - 13m) * recette.NiveauPiquant.Value / 10m);
        }

        if (malus > 20)
        {
            return new PairingEngineResult
            {
                Score = 0,
                Confiance = 0,
                MalusApplique = malus,
                Eliminatoire = true,
                ReglesSatisfaites = new List<string> { $"R16bis éliminatoire (malus: {malus})" },
                VersionMoteur = VersionMoteur,
                Justification = "Accord déconseillé : alcool fort + plat très piquant."
            };
        }

        // Étapes 2-4 : Évaluation de chaque règle (Strategy)
        var scoreTotal = 0;
        var poidsTotal = 0;
        var reglesSatisfaites = new List<string>();
        var justifications = new List<string>();

        foreach (var regle in _regles)
        {
            var res = regle.Evaluer(recette, boisson);
            if (!res.Applicable) continue;

            poidsTotal += regle.Poids;
            if (res.Satisfaite)
            {
                scoreTotal += regle.Poids;
                reglesSatisfaites.Add(regle.Id);
                if (!string.IsNullOrWhiteSpace(res.Justification))
                    justifications.Add(res.Justification);
            }
        }

        // Étape 5 : Score brut sur 100
        var scoreBrut = poidsTotal > 0
            ? (double)scoreTotal / poidsTotal * 100
            : 50.0; // score neutre si aucune règle applicable

        // Étape 6 : Application du malus gradué (0 < malus ≤ 20)
        var scoreFinal = scoreBrut;
        if (malus > 0)
        {
            scoreFinal = Math.Max(0, scoreBrut - malus);
            reglesSatisfaites.Add($"R16bis (malus: -{malus})");
        }

        // Étape 7 : Niveau de confiance (proportion de règles applicables)
        var confiance = (int)Math.Round((double)poidsTotal / PoidsTotalMaximum * 100);

        // Étape 8 : Retour structuré
        return new PairingEngineResult
        {
            Score = (int)Math.Round(scoreFinal),
            Confiance = confiance,
            MalusApplique = malus,
            Eliminatoire = false,
            ReglesSatisfaites = reglesSatisfaites,
            VersionMoteur = VersionMoteur,
            Justification = string.Join(" ", justifications)
        };
    }
}