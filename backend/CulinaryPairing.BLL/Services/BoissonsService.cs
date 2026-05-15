using CulinaryPairing.BLL.DTOs.Boissons;
using CulinaryPairing.DAL;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Services;

public class BoissonsService : IBoissonsService
{
    private readonly CulinaryPairingDbContext _context;

    public BoissonsService(CulinaryPairingDbContext context)
    {
        _context = context;
    }

    public async Task<List<BoissonListItemDto>> GetAllAsync()
    {
        return await _context.Boissons
            .OrderBy(b => b.Nom)
            .Select(b => new BoissonListItemDto
            {
                IdBoisson = b.IdBoisson,
                Nom = b.Nom,
                TypeBoisson = b.TypeBoisson != null ? b.TypeBoisson.ToString() : null,
                Alcoolise = b.Alcoolise,
                Pays = b.Pays,
                Region = b.Region,
                Appellation = b.Appellation,
                Cepage = b.Cepage,
                DegreAlcool = b.DegreAlcool,
                Corps = b.Corps != null ? b.Corps.ToString() : null,
                ImageUrl = b.ImageUrl
            })
            .ToListAsync();
    }

    public async Task<BoissonDetailDto?> GetByIdAsync(int idBoisson)
    {
        var b = await _context.Boissons
            .Include(x => x.FamillesAromatiques)
                .ThenInclude(bf => bf.Famille)
            .FirstOrDefaultAsync(x => x.IdBoisson == idBoisson);

        if (b is null)
            return null;

        return new BoissonDetailDto
        {
            IdBoisson = b.IdBoisson,
            Nom = b.Nom,
            TypeBoisson = b.TypeBoisson?.ToString(),
            Alcoolise = b.Alcoolise,
            NiveauAcidite = b.NiveauAcidite,
            NiveauSucre = b.NiveauSucre,
            NiveauTannins = b.NiveauTannins,
            NiveauAmertume = b.NiveauAmertume,
            IntensiteAromatique = b.IntensiteAromatique,
            NiveauFume = b.NiveauFume,
            DegreAlcool = b.DegreAlcool,
            Corps = b.Corps?.ToString(),
            TemperatureOptimale = b.TemperatureOptimale,
            ToleranceTemperature = b.ToleranceTemperature,
            Pays = b.Pays,
            Region = b.Region,
            Appellation = b.Appellation,
            Cepage = b.Cepage,
            CoutMoyen = b.CoutMoyen,
            ImageUrl = b.ImageUrl,
            FamillesAromatiques = b.FamillesAromatiques
                .Where(bf => bf.Famille != null)
                .Select(bf => bf.Famille!.Nom)
                .OrderBy(n => n)
                .ToList()
        };
    }
}