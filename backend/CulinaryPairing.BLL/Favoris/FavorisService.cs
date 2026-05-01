using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Favoris;

public class FavorisService : IFavorisService
{
    private readonly CulinaryPairingDbContext _db;

    public FavorisService(CulinaryPairingDbContext db) => _db = db;

    public async Task<bool> AddAsync(int idUtilisateur, int idRecette)
    {
        // Idempotent : pas d'exception si déjà favori, retour false
        var exists = await _db.Favoris
            .AnyAsync(f => f.IdUtilisateur == idUtilisateur && f.IdRecette == idRecette);
        if (exists) return false;

        var recetteExists = await _db.Recettes.AnyAsync(r => r.IdRecette == idRecette);
        if (!recetteExists) throw new KeyNotFoundException("Recette introuvable.");

        _db.Favoris.Add(new Favori
        {
            IdUtilisateur = idUtilisateur,
            IdRecette = idRecette,
            DateAjout = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveAsync(int idUtilisateur, int idRecette)
    {
        var fav = await _db.Favoris
            .FirstOrDefaultAsync(f => f.IdUtilisateur == idUtilisateur && f.IdRecette == idRecette);
        if (fav == null) return false;

        _db.Favoris.Remove(fav);
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<bool> IsFavoriAsync(int idUtilisateur, int idRecette) =>
        _db.Favoris.AnyAsync(f => f.IdUtilisateur == idUtilisateur && f.IdRecette == idRecette);

    public async Task<List<FavoriDto>> GetByUserAsync(int idUtilisateur)
    {
        // Matérialisation puis projection : évite les soucis de traduction LINQ→SQL des enum HasConversion<string>
        var favoris = await _db.Favoris
            .Include(f => f.Recette)
            .Where(f => f.IdUtilisateur == idUtilisateur)
            .OrderByDescending(f => f.DateAjout)
            .ToListAsync();

        return favoris.Select(f => new FavoriDto
        {
            IdRecette = f.Recette.IdRecette,
            Titre = f.Recette.Titre,
            ImageUrl = f.Recette.ImageUrl,
            TypePlat = f.Recette.TypePlat?.ToString(),
            Difficulte = f.Recette.Difficulte?.ToString(),
            TempsPreparation = f.Recette.TempsPreparation,
            TempsCuisson = f.Recette.TempsCuisson,
            DateAjout = f.DateAjout
        }).ToList();
    }

    public async Task<HashSet<int>> GetIdsRecettesAsync(int idUtilisateur)
    {
        var ids = await _db.Favoris
            .Where(f => f.IdUtilisateur == idUtilisateur)
            .Select(f => f.IdRecette)
            .ToListAsync();
        return ids.ToHashSet();
    }
}