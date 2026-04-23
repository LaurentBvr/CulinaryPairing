using CulinaryPairing.BLL.DTOs.Accords;
using CulinaryPairing.DAL;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.Services;

public class AccordsService : IAccordsService
{
    private readonly CulinaryPairingDbContext _context;

    public AccordsService(CulinaryPairingDbContext context)
    {
        _context = context;
    }

    public async Task<List<AccordDto>> GetAccordsByRecetteAsync(int idRecette)
    {
        return await _context.Accords
            .Include(a => a.Boisson)
            .Where(a => a.IdRecette == idRecette)
            .OrderByDescending(a => a.ScoreCompatibilite)
            .Select(a => new AccordDto
            {
                IdAccord = a.IdAccord,
                TypeAccord = a.TypeAccord.ToString(),
                Justification = a.Justification,
                ScoreCompatibilite = a.ScoreCompatibilite,
                NiveauConfiance = a.NiveauConfiance,
                MalusApplique = a.MalusApplique,
                ReglesSatisfaites = a.ReglesSatisfaites,
                IdBoisson = a.IdBoisson,
                NomBoisson = a.Boisson.Nom,
                TypeBoisson = a.Boisson.TypeBoisson.ToString()
            })
            .ToListAsync();
    }
}