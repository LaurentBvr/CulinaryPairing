using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R19bis — Similitude aromatique (CdC v1.3 §2.5, principe 6).
/// Applicable : RECETTE et BOISSON ont chacune au moins une famille aromatique chargée.
/// Satisfaite : au moins une famille aromatique commune (boisé/boisé, fumé/fumé...).
/// Justification : les accords par similitude créent une harmonie immédiate.
/// Poids : 10.
///
/// IMPORTANT : l'appelant DOIT pré-charger les collections via :
///   .Include(r => r.FamillesAromatiques).ThenInclude(rf => rf.Famille)
///   .Include(b => b.FamillesAromatiques).ThenInclude(bf => bf.Famille)
/// Sans cela, la règle renvoie NonApplicable silencieusement (par sécurité).
/// </summary>
public class R19bis_SimilitudeAromatique : IPairingRule
{
    public string Id => "R19bis";
    public int Poids => 10;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        // Sécurité : si les collections n'ont pas été chargées (oubli d'Include),
        // ou si l'un des deux n'a aucune famille déclarée, on n'évalue pas.
        if (recette.FamillesAromatiques is null || recette.FamillesAromatiques.Count == 0
            || boisson.FamillesAromatiques is null || boisson.FamillesAromatiques.Count == 0)
            return PairingResult.NonApplicable();

        var famillesRecette = recette.FamillesAromatiques
            .Where(rf => rf.Famille is not null)
            .Select(rf => rf.Famille.Nom)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var famillesBoisson = boisson.FamillesAromatiques
            .Where(bf => bf.Famille is not null)
            .Select(bf => bf.Famille.Nom)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var communes = famillesRecette.Intersect(famillesBoisson, StringComparer.OrdinalIgnoreCase)
            .ToList();

        return communes.Count > 0
            ? PairingResult.Satisfait(
                $"Familles partagées : {{{string.Join(", ", communes)}}} → similitude.")
            : PairingResult.NonSatisfait(
                "Aucune famille aromatique commune entre le plat et la boisson.");
    }
}