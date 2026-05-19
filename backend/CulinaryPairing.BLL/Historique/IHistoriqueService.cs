using CulinaryPairing.BLL.DTOs;

namespace CulinaryPairing.BLL.Historique;

public interface IHistoriqueService
{
    /// <summary>
    /// Enregistre une consultation de recette par un utilisateur.
    /// Déduplique : ignore l'insertion si une consultation de la même recette
    /// par le même utilisateur a eu lieu dans les <see cref="DedupWindowMinutes"/> dernières minutes.
    /// </summary>
    /// <returns>true si une nouvelle ligne a été insérée, false si dedup.</returns>
    Task<bool> AjouterAsync(int idUtilisateur, int idRecette, CancellationToken ct = default);

    /// <summary>
    /// Retourne les N dernières recettes UNIQUES consultées par l'utilisateur,
    /// triées par date de dernière consultation décroissante.
    /// </summary>
    Task<IReadOnlyList<HistoriqueRecetteDto>> GetDernieresRecettesAsync(
        int idUtilisateur,
        int limit = 5,
        CancellationToken ct = default);
}