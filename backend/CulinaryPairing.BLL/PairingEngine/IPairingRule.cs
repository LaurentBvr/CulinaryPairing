using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine;

/// <summary>
/// Contrat d'une règle d'accord (pattern Strategy, cf. CdC v1.3 §2.5).
/// Chaque règle est isolée, testable indépendamment, et déclarée dans le tableau du moteur.
/// </summary>
public interface IPairingRule
{
    /// <summary>Code de la règle (ex : "R10bis"), persisté dans ACCORD.regles_satisfaites.</summary>
    string Id { get; }

    /// <summary>Poids de la règle (cf. tableau §2.5, somme totale = 260).</summary>
    int Poids { get; }

    /// <summary>Évalue la règle pour un couple (plat, boisson).</summary>
    PairingResult Evaluer(Recette recette, Boisson boisson);
}