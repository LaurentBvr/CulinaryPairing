namespace CulinaryPairing.BLL.Contraintes;

public interface IContraintesService
{
    /// <summary>Catalogue complet des contraintes (8 entrées seedées V1.3).</summary>
    Task<List<ContrainteDto>> GetAllAsync();

    /// <summary>Contraintes activées par un utilisateur.</summary>
    Task<List<ContrainteDto>> GetByUserAsync(int idUtilisateur);

    /// <summary>Remplace toutes les contraintes du user (delete + insert). Throws ArgumentException si un id invalide.</summary>
    Task UpdateUserContraintesAsync(int idUtilisateur, List<int> idsContraintes);

    /// <summary>
    /// Pour chaque recette de la liste, retourne les contraintes du user qui sont violées
    /// par au moins un ingrédient. Dictionnaire vide si user sans contrainte.
    /// </summary>
    Task<Dictionary<int, List<ContrainteDto>>> GetContraintesVioleesAsync(
        int idUtilisateur,
        List<int> idsRecettes);
        
}