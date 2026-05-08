using Microsoft.EntityFrameworkCore;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// CdC §3.5.5 — INGREDIENT_CONTRAINTE : liaisons N-N entre ingrédients et contraintes.
// Cœur fonctionnel des règles R9 (allergies) et R16 (régimes alimentaires).
// Ne lie que les ingrédients réellement présents dans IngredientsSeed (résilient aux IDs).
public static class IngredientsContraintesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (await context.IngredientsContraintes.AnyAsync()) return;

        // Mapping : nom_ingredient -> liste de noms de contraintes violées
        var mapping = new Dictionary<string, string[]>
        {
            // ===== Existant V1.0 (7 ingrédients, 15 liaisons) =====
            ["Boeuf"]         = new[] { "Végétarien", "Végan" },
            ["Poulet"]        = new[] { "Végétarien", "Végan" },
            ["Lardons"]       = new[] { "Végétarien", "Végan", "Halal", "Casher" },
            ["Beurre"]        = new[] { "Végan", "Sans lactose" },
            ["Crème fraîche"] = new[] { "Végan", "Sans lactose" },
            ["Parmesan"]      = new[] { "Végan", "Sans lactose" },
            ["Oeuf"]          = new[] { "Végan" },

            // ===== V1.3 - Protéines additionnelles =====
            ["Saint-Jacques"]            = new[] { "Végétarien", "Végan", "Allergie fruits de mer" },
            ["Crevettes"]                = new[] { "Végétarien", "Végan", "Allergie fruits de mer" },
            ["Magret de canard"]         = new[] { "Végétarien", "Végan" },
            ["Agneau"]                   = new[] { "Végétarien", "Végan" },
            ["Bacon fumé"]               = new[] { "Végétarien", "Végan", "Halal", "Casher" },
            ["Boeuf haché"]              = new[] { "Végétarien", "Végan" },
            ["Filet de boeuf"]           = new[] { "Végétarien", "Végan" },
            ["Boeuf à bouillir"]         = new[] { "Végétarien", "Végan" },

            // ===== V1.3 - Produits laitiers et oeufs additionnels =====
            ["Cheddar"]                  = new[] { "Végan", "Sans lactose" },
            ["Lait"]                     = new[] { "Végan", "Sans lactose" },
            ["Gélatine"]                 = new[] { "Végétarien", "Végan" },  // collagène animal

            // ===== V1.3 - Gluten et allergie céréales =====
            ["Farine de blé"]            = new[] { "Sans gluten" },
            ["Pâte brisée"]              = new[] { "Sans gluten", "Végan", "Sans lactose" },  // contient beurre
            ["Pain à burger"]            = new[] { "Sans gluten" },
            ["Pain (croûtons)"]          = new[] { "Sans gluten" },

            // ===== V1.3 - Pâte de curry industrielle (contient pâte de crevette) =====
            ["Pâte de curry rouge"]      = new[] { "Végétarien", "Végan", "Allergie fruits de mer" },

            // ===== V1.3 - Sauces dérivées de poisson =====
            ["Sauce poisson (nuoc-mâm)"] = new[] { "Végétarien", "Végan", "Allergie fruits de mer" },

            // ===== V1.3 - Sauce soja standard (contient blé fermenté) =====
            ["Sauce soja"]               = new[] { "Sans gluten" },

            // ===== V1.3 - Bouillon de boeuf =====
            ["Bouillon de boeuf"]        = new[] { "Végétarien", "Végan" },

            // ===== V1.3 - Vin de cuisine (alcool : halal/casher selon écoles) =====
            ["Vin rouge de cuisine"]     = new[] { "Halal" },

            // ===== V1.3 - Miel (produit animal non-vegan, mais végé OK) =====
            ["Miel"]                     = new[] { "Végan" }
        };

        // Charge les références une fois (évite N+1)
        var ingredients = await context.Ingredients
            .Where(i => mapping.Keys.Contains(i.Nom))
            .ToDictionaryAsync(i => i.Nom, i => i.IdIngredient);

        var contraintes = await context.ContraintesAlimentaires
            .ToDictionaryAsync(c => c.Nom, c => c.IdContrainte);

        var liaisons = new List<IngredientContrainte>();
        foreach (var (nomIngredient, nomsContraintes) in mapping)
        {
            if (!ingredients.TryGetValue(nomIngredient, out var idIngredient)) continue;

            foreach (var nomContrainte in nomsContraintes)
            {
                if (!contraintes.TryGetValue(nomContrainte, out var idContrainte)) continue;
                liaisons.Add(new IngredientContrainte
                {
                    IdIngredient = idIngredient,
                    IdContrainte = idContrainte
                });
            }
        }

        await context.IngredientsContraintes.AddRangeAsync(liaisons);
        await context.SaveChangesAsync();
    }
}