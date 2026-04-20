using Microsoft.EntityFrameworkCore;
using CulinaryPairing.DAL;

namespace CulinaryPairing.DAL.Seed;

// Point d'entrée du seed. Orchestre l'ordre d'insertion pour respecter les FK
// et garantit l'idempotence : chaque bloc vérifie si ses données existent déjà.
public static class DbInitializer
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        // S'assure que la BDD existe et que les migrations sont appliquées
        await context.Database.MigrateAsync();

        // Ordre strict : parents d'abord, puis liaisons, puis updates
        await UtilisateursSeed.SeedAsync(context);
        await ContraintesSeed.SeedAsync(context);
        await IngredientsSeed.SeedAsync(context);
        await BoissonsSeed.SeedAsync(context);
        await FamillesAromatiquesSeed.SeedAsync(context);

        await SubstitutionsSeed.SeedAsync(context);
        await RecettesSeed.SeedAsync(context);
        await RecetteIngredientsSeed.SeedAsync(context);
        await EtapesSeed.SeedAsync(context);

        await AssociationsAromatiquesSeed.SeedAsync(context);
        await QuizSeed.SeedAsync(context);
        await AccordsSeed.SeedAsync(context);
    }
}