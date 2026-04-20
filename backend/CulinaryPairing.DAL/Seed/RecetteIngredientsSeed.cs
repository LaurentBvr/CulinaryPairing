using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Compositions des 6 recettes à partir des 20 ingrédients disponibles.
// Le CdC ne détaille que le Risotto (3.5.1) ; les 5 autres compositions
// sont réalistes mais limitées au catalogue ingrédients existant.
// Les quantités sont pour NombrePersonnesBase (cf RecettesSeed).
public static class RecetteIngredientsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.RecettesIngredients.Any()) return;

        var compositions = new List<RecetteIngredient>
        {
            // ===== Recette 1 : Boeuf Bourguignon (6 personnes) =====
            new() { IdRecette = 1, IdIngredient = 1,  Quantite = 1200 },  // Boeuf 1.2 kg
            new() { IdRecette = 1, IdIngredient = 2,  Quantite = 200 },   // Lardons
            new() { IdRecette = 1, IdIngredient = 6,  Quantite = 300 },   // Champignons de Paris
            new() { IdRecette = 1, IdIngredient = 9,  Quantite = 40 },    // Beurre
            new() { IdRecette = 1, IdIngredient = 10, Quantite = 30 },    // Huile d'olive
            new() { IdRecette = 1, IdIngredient = 17, Quantite = 2 },     // Oignons
            new() { IdRecette = 1, IdIngredient = 18, Quantite = 4 },     // Ail
            new() { IdRecette = 1, IdIngredient = 19, Quantite = 8 },     // Sel
            new() { IdRecette = 1, IdIngredient = 20, Quantite = 3 },     // Poivre

            // ===== Recette 2 : Risotto aux champignons (4 personnes) — CdC 3.5.1 =====
            new() { IdRecette = 2, IdIngredient = 5,  Quantite = 300 },   // Riz arborio
            new() { IdRecette = 2, IdIngredient = 6,  Quantite = 200 },   // Champignons de Paris
            new() { IdRecette = 2, IdIngredient = 7,  Quantite = 50 },    // Parmesan
            new() { IdRecette = 2, IdIngredient = 9,  Quantite = 30 },    // Beurre
            new() { IdRecette = 2, IdIngredient = 17, Quantite = 1 },     // Oignon
            new() { IdRecette = 2, IdIngredient = 18, Quantite = 2 },     // Ail
            new() { IdRecette = 2, IdIngredient = 19, Quantite = 5 },     // Sel
            new() { IdRecette = 2, IdIngredient = 20, Quantite = 2 },     // Poivre

            // ===== Recette 3 : Salade César (2 personnes) =====
            new() { IdRecette = 3, IdIngredient = 13, Quantite = 300 },   // Poulet
            new() { IdRecette = 3, IdIngredient = 7,  Quantite = 30 },    // Parmesan
            new() { IdRecette = 3, IdIngredient = 16, Quantite = 1 },     // Oeuf (pour la sauce)
            new() { IdRecette = 3, IdIngredient = 15, Quantite = 1 },     // Citron
            new() { IdRecette = 3, IdIngredient = 10, Quantite = 40 },    // Huile d'olive
            new() { IdRecette = 3, IdIngredient = 18, Quantite = 1 },     // Ail
            new() { IdRecette = 3, IdIngredient = 19, Quantite = 3 },     // Sel
            new() { IdRecette = 3, IdIngredient = 20, Quantite = 1 },     // Poivre

            // ===== Recette 4 : Velouté de butternut (4 personnes) =====
            new() { IdRecette = 4, IdIngredient = 11, Quantite = 150 },   // Crème fraîche
            new() { IdRecette = 4, IdIngredient = 9,  Quantite = 30 },    // Beurre
            new() { IdRecette = 4, IdIngredient = 17, Quantite = 1 },     // Oignon
            new() { IdRecette = 4, IdIngredient = 18, Quantite = 2 },     // Ail
            new() { IdRecette = 4, IdIngredient = 19, Quantite = 5 },     // Sel
            new() { IdRecette = 4, IdIngredient = 20, Quantite = 2 },     // Poivre

            // ===== Recette 5 : Poulet rôti aux herbes (4 personnes) =====
            new() { IdRecette = 5, IdIngredient = 13, Quantite = 1400 },  // Poulet entier ~1.4 kg
            new() { IdRecette = 5, IdIngredient = 9,  Quantite = 50 },    // Beurre (sous la peau)
            new() { IdRecette = 5, IdIngredient = 10, Quantite = 30 },    // Huile d'olive
            new() { IdRecette = 5, IdIngredient = 15, Quantite = 1 },     // Citron (dans la cavité)
            new() { IdRecette = 5, IdIngredient = 18, Quantite = 4 },     // Ail
            new() { IdRecette = 5, IdIngredient = 19, Quantite = 8 },     // Sel
            new() { IdRecette = 5, IdIngredient = 20, Quantite = 3 },     // Poivre

            // ===== Recette 6 : Panna cotta fruits rouges (4 personnes) =====
            new() { IdRecette = 6, IdIngredient = 11, Quantite = 400 },   // Crème fraîche (base de la panna cotta)
            new() { IdRecette = 6, IdIngredient = 15, Quantite = 1 }      // Citron (zeste pour parfumer)
        };

        await context.RecettesIngredients.AddRangeAsync(compositions);
        await context.SaveChangesAsync();
    }
}