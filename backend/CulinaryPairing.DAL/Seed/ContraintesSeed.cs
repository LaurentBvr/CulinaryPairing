using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 8 contraintes alimentaires du CdC section 3.5.1 : 3 types couverts
// (choix de régime, santé, religieux).
public static class ContraintesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.ContraintesAlimentaires.Any()) return;

        var contraintes = new List<ContrainteAlimentaire>
        {
            new() { Nom = "Végétarien",             Type = TypeContrainte.Choix },
            new() { Nom = "Végan",                  Type = TypeContrainte.Choix },
            new() { Nom = "Sans lactose",           Type = TypeContrainte.Sante },
            new() { Nom = "Sans gluten",            Type = TypeContrainte.Sante },
            new() { Nom = "Halal",                  Type = TypeContrainte.Religieux },
            new() { Nom = "Casher",                 Type = TypeContrainte.Religieux },
            new() { Nom = "Allergie arachides",     Type = TypeContrainte.Sante },
            new() { Nom = "Allergie fruits de mer", Type = TypeContrainte.Sante }
        };

        await context.ContraintesAlimentaires.AddRangeAsync(contraintes);
        await context.SaveChangesAsync();
    }
}