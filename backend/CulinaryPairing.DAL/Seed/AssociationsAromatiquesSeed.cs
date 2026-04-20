using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Associations de référence entre boissons/recettes et familles aromatiques
// (CdC 3.5.4). Le CdC précise que ce sont des "exemples de référence" et
// non un jeu exhaustif ; d'autres associations seront ajoutées à mesure
// que le catalogue s'enrichit.
// IDs des familles : 1=agrumes, 2=fruits_rouges, 3=fruits_noirs,
// 4=fruits_blancs, 5=fruits_exotiques, 6=floral, 7=boise, 8=fume,
// 9=epice, 10=mineral, 11=vegetal, 12=herbace, 13=beurre, 14=torrefie.
public static class AssociationsAromatiquesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        // Deux tables distinctes, on vérifie les deux pour l'idempotence
        if (context.BoissonsFamillesAromatiques.Any() ||
            context.RecettesFamillesAromatiques.Any()) return;

        // ===== BOISSONS =====
        var boissonsAssociations = new List<BoissonFamilleAromatique>
        {
            // Chardonnay (id 1) → beurre, boisé, fruits blancs
            new() { IdBoisson = 1, IdFamille = 13 },  // beurre
            new() { IdBoisson = 1, IdFamille = 7  },  // boise
            new() { IdBoisson = 1, IdFamille = 4  },  // fruits_blancs

            // Gewurztraminer (id 3) → floral, fruits exotiques, épice
            new() { IdBoisson = 3, IdFamille = 6  },  // floral
            new() { IdBoisson = 3, IdFamille = 5  },  // fruits_exotiques
            new() { IdBoisson = 3, IdFamille = 9  }   // epice
        };

        // ===== RECETTES =====
        var recettesAssociations = new List<RecetteFamilleAromatique>
        {
            // Risotto aux champignons (id 2) → boisé, herbacé, beurre
            new() { IdRecette = 2, IdFamille = 7  },  // boise
            new() { IdRecette = 2, IdFamille = 12 },  // herbace
            new() { IdRecette = 2, IdFamille = 13 }   // beurre
        };

        await context.BoissonsFamillesAromatiques.AddRangeAsync(boissonsAssociations);
        await context.RecettesFamillesAromatiques.AddRangeAsync(recettesAssociations);
        await context.SaveChangesAsync();
    }
}