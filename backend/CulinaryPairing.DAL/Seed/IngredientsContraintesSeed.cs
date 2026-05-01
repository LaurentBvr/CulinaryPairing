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
            // Viandes -> non vegetarien/vegan ; lardons (porc) ajoute halal+casher
            ["Boeuf"]         = new[] { "Végétarien", "Végan" },
            ["Poulet"]        = new[] { "Végétarien", "Végan" },
            ["Lardons"]       = new[] { "Végétarien", "Végan", "Halal", "Casher" },

            // Produits laitiers / oeufs -> non vegan ; produits laitiers ajoutent sans lactose
            ["Beurre"]        = new[] { "Végan", "Sans lactose" },
            ["Crème fraîche"] = new[] { "Végan", "Sans lactose" },
            ["Parmesan"]      = new[] { "Végan", "Sans lactose" },
            ["Oeuf"]          = new[] { "Végan" }
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