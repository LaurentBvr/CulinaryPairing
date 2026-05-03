using CulinaryPairing.BLL.DTOs.Soirees;

namespace CulinaryPairing.BLL.Soirees;

/// <summary>
/// Gestion du contenu structurel d'un menu de soirée (slots + recalculs).
/// Distinct de SoireesService qui gère le cycle de vie de la soirée.
/// </summary>
public interface IMenusService
{
    /// <summary>
    /// Récupère le menu de la soirée, ou en crée un vide si aucun (MVP : 1 menu par soirée).
    /// Retourne null si la soirée n'existe pas ou n'appartient pas à l'utilisateur.
    /// </summary>
    Task<MenuDto?> GetOrCreateAsync(int idSoiree, int idUtilisateur);

    /// <summary>
    /// Assigne une recette à un slot (entree|plat|dessert). Recalcule cout/temps totaux.
    /// Throws ArgumentException : slot invalide, recette inexistante,
    /// type_plat ne matche pas le slot, ou contrainte agrégée violée.
    /// Returns null si soirée non détenue.
    /// </summary>
    Task<MenuDto?> AssignSlotAsync(int idSoiree, string slot, int idRecette, int idUtilisateur);

    /// <summary>Retire la recette d'un slot. Recalcule. Returns null si soirée non détenue.</summary>
    Task<MenuDto?> UnassignSlotAsync(int idSoiree, string slot, int idUtilisateur);
}