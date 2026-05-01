namespace CulinaryPairing.BLL.Favoris;

public interface IFavorisService
{
    /// <summary>Ajoute la recette aux favoris. Retourne false si déjà présent (idempotent).</summary>
    Task<bool> AddAsync(int idUtilisateur, int idRecette);

    /// <summary>Retire la recette des favoris. Retourne false si pas présente.</summary>
    Task<bool> RemoveAsync(int idUtilisateur, int idRecette);

    /// <summary>True si la recette est dans les favoris de l'utilisateur.</summary>
    Task<bool> IsFavoriAsync(int idUtilisateur, int idRecette);

    /// <summary>Liste des favoris de l'utilisateur, triés DESC par date d'ajout.</summary>
    Task<List<FavoriDto>> GetByUserAsync(int idUtilisateur);

    /// <summary>Set d'IDs recettes favorites de l'utilisateur (optimisation front : 1 appel pour toute la liste).</summary>
    Task<HashSet<int>> GetIdsRecettesAsync(int idUtilisateur);
}