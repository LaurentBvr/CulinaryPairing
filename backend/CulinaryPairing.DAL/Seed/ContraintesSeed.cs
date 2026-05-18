using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;
namespace CulinaryPairing.DAL.Seed;
// 8 contraintes alimentaires du CdC section 3.5.1 : 3 types couverts
// (allergies, régimes alimentaires, convictions).
public static class ContraintesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.ContraintesAlimentaires.Any()) return;
        var contraintes = new List<ContrainteAlimentaire>
        {
            new() { Nom = "Végétarien",             Type = TypeContrainte.Regime },
            new() { Nom = "Végan",                  Type = TypeContrainte.Regime },
            new() { Nom = "Sans lactose",           Type = TypeContrainte.Regime },
            new() { Nom = "Sans gluten",            Type = TypeContrainte.Regime },
            new() { Nom = "Halal",                  Type = TypeContrainte.Conviction },
            new() { Nom = "Casher",                 Type = TypeContrainte.Conviction },
            new() { Nom = "Allergie arachides",     Type = TypeContrainte.Allergie },
            new() { Nom = "Allergie fruits de mer", Type = TypeContrainte.Allergie }
        };
        await context.ContraintesAlimentaires.AddRangeAsync(contraintes);
        await context.SaveChangesAsync();
    }
}