namespace CulinaryPairing.BLL.Contraintes;

public interface IContraintesService
{
    /// <summary>Catalogue complet des contraintes (8 entrées seedées V1.3).</summary>
    Task<List<ContrainteDto>> GetAllAsync();

    /// <summary>Contraintes activées par un utilisateur.</summary>
    Task<List<ContrainteDto>> GetByUserAsync(int idUtilisateur);

    /// <summary>Remplace toutes les contraintes du user (delete + insert). Throws ArgumentException si un id invalide.</summary>
    Task UpdateUserContraintesAsync(int idUtilisateur, List<int> idsContraintes);
}