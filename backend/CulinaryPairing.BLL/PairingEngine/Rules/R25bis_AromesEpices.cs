using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R25bis — Arômes épicés (CdC v1.3 §2.5, principe 6) — NOUVEAU V1.3.
/// Applicable : plat avec arômes épicés marqués (niveau_arome_epice ≥ 5).
/// Satisfaite : boisson appartenant à la famille aromatique 'epice'
///              (Gewurztraminer, Syrah, vins du Rhône, whiskies épicés).
/// Justification : exploite le split V1.2 entre piquant (tactile) et arôme épicé
///                 (cannelle, cardamome, cumin, poivre).
/// Poids : 10.
///
/// IMPORTANT : l'appelant doit pré-charger boisson.FamillesAromatiques
///             (cf. R19bis pour le détail des Include).
/// </summary>
public class R25bis_AromesEpices : IPairingRule
{
    public string Id => "R25bis";
    public int Poids => 10;

    private const string FamilleEpice = "epice";

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauAromeEpice is null || recette.NiveauAromeEpice < 5)
            return PairingResult.NonApplicable();

        // Sécurité : si les familles boisson ne sont pas chargées, on n'évalue pas
        if (boisson.FamillesAromatiques is null || boisson.FamillesAromatiques.Count == 0)
            return PairingResult.NonApplicable();

        var boissonEstEpicee = boisson.FamillesAromatiques
            .Where(bf => bf.Famille is not null)
            .Any(bf => string.Equals(bf.Famille.Nom, FamilleEpice,
                StringComparison.OrdinalIgnoreCase));

        return boissonEstEpicee
            ? PairingResult.Satisfait(
                $"Plat aux arômes épicés (niveau {recette.NiveauAromeEpice}/10) "
                + "résonant avec une boisson épicée.")
            : PairingResult.NonSatisfait(
                $"Plat aux arômes épicés (niveau {recette.NiveauAromeEpice}/10) "
                + "sans boisson de la famille 'epice' pour résonner.");
    }
}