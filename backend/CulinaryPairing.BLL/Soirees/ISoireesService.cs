using CulinaryPairing.BLL.DTOs.Soirees;
using CulinaryPairing.BLL.Contraintes;

namespace CulinaryPairing.BLL.Soirees;

public interface ISoireesService
{
    /// <summary>Liste résumée des soirées de l'utilisateur connecté (tri DESC date_creation).</summary>
    Task<List<SoireeListItemDto>> GetMineAsync(int idUtilisateur);

    /// <summary>Détail d'une soirée. Retourne null si inexistante OU non détenue par le user (anti info-disclosure).</summary>
    Task<SoireeDetailDto?> GetByIdAsync(int idSoiree, int idUtilisateur);

    /// <summary>Crée une soirée + ses contraintes saisies. Throws ArgumentException si vegé+vegan > nombre_personnes ou idContrainte invalide.</summary>
    Task<int> CreateAsync(int idUtilisateur, SoireeCreateDto dto);

    /// <summary>Met à jour une soirée + remplace l'ensemble de ses contraintes (delete+insert). Returns false si non détenue.</summary>
    Task<bool> UpdateAsync(int idSoiree, int idUtilisateur, SoireeUpdateDto dto);

    /// <summary>Supprime une soirée (cascade menu + contraintes). Returns false si non détenue.</summary>
    Task<bool> DeleteAsync(int idSoiree, int idUtilisateur);
    
    /// <summary>
    /// Liste des contraintes effectives à appliquer pour la soirée :
    /// contraintes saisies ∪ {Végan si NombreVegans>0, sinon Végétarien si NombreVegetariens>0}.
    /// Retourne liste vide si soirée non trouvée ou non détenue par le user.
    /// Réutilisée au cycle 7 pour filtrer les recettes éligibles.
    /// </summary>
    Task<List<ContrainteDto>> GetContraintesAgregeesAsync(int idSoiree, int idUtilisateur);
}