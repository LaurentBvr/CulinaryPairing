using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 14 familles aromatiques du CdC section 3.5.4. Couvrent l'ensemble des
// descripteurs olfactifs et gustatifs utilisés par les règles R13, R17 et R22bis.
// Ordre strict : IDs implicites 1..14 référencés par AssociationsAromatiquesSeed.
public static class FamillesAromatiquesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.FamillesAromatiques.Any()) return;

        var familles = new List<FamilleAromatique>
        {
            new() { Nom = "agrumes",          Description = "Citron, orange, pamplemousse, bergamote" },
            new() { Nom = "fruits_rouges",    Description = "Fraise, framboise, cerise, groseille" },
            new() { Nom = "fruits_noirs",     Description = "Mûre, cassis, myrtille, prune" },
            new() { Nom = "fruits_blancs",    Description = "Pomme, poire, pêche, abricot" },
            new() { Nom = "fruits_exotiques", Description = "Mangue, ananas, fruit de la passion, litchi" },
            new() { Nom = "floral",           Description = "Rose, violette, jasmin, fleur d'acacia" },
            new() { Nom = "boise",            Description = "Chêne, vanille, cèdre, toasté" },
            new() { Nom = "fume",             Description = "Fumé, tourbé, grillé" },
            new() { Nom = "epice",            Description = "Poivre, cannelle, clou de girofle, muscade" },
            new() { Nom = "mineral",          Description = "Minéralité, pierre à fusil, silex" },
            new() { Nom = "vegetal",          Description = "Herbe coupée, poivron, asperge, petit pois" },
            new() { Nom = "herbace",          Description = "Thym, romarin, basilic, laurier" },
            new() { Nom = "beurre",           Description = "Beurre, crème, brioche, pâtisserie" },
            new() { Nom = "torrefie",         Description = "Café, cacao, caramel" }
        };

        await context.FamillesAromatiques.AddRangeAsync(familles);
        await context.SaveChangesAsync();
    }
}