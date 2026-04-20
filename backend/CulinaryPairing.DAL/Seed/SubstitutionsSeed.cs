using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 9 substitutions du CdC section 3.5.1 : 4 communes (végé + végan),
// 5 végan-only. Les FK renvoient aux IDs implicites d'IngredientsSeed (1..20).
public static class SubstitutionsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.SubstitutionsIngredients.Any()) return;

        var substitutions = new List<SubstitutionIngredient>
        {
            // Boeuf (1) → Pleurotes (3) : végé + végan
            new() { IdIngredientOriginal = 1, IdIngredientSubstitut = 3,
                    TypeSubstitution = TypeSubstitution.Vegetarien, RatioConversion = 0.8m,
                    NoteCuisson = "Ajouter les pleurotes en fin de cuisson, faire revenir 5 min à feu vif" },
            new() { IdIngredientOriginal = 1, IdIngredientSubstitut = 3,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 0.8m,
                    NoteCuisson = "Ajouter les pleurotes en fin de cuisson, faire revenir 5 min à feu vif" },

            // Lardons (2) → Lardons végétaux (4) : végé + végan
            new() { IdIngredientOriginal = 2, IdIngredientSubstitut = 4,
                    TypeSubstitution = TypeSubstitution.Vegetarien, RatioConversion = 1.0m,
                    NoteCuisson = null },
            new() { IdIngredientOriginal = 2, IdIngredientSubstitut = 4,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 1.0m,
                    NoteCuisson = null },

            // Parmesan (7) → Levure nutritionnelle (8) : végan-only
            new() { IdIngredientOriginal = 7, IdIngredientSubstitut = 8,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 0.3m,
                    NoteCuisson = "Saupoudrer la levure nutritionnelle au moment de servir" },

            // Beurre (9) → Huile d'olive (10) : végan-only
            new() { IdIngredientOriginal = 9, IdIngredientSubstitut = 10,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 0.9m,
                    NoteCuisson = "Utiliser de l'huile d'olive à la place du beurre" },

            // Crème fraîche (11) → Crème de coco (12) : végan-only
            new() { IdIngredientOriginal = 11, IdIngredientSubstitut = 12,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 1.0m,
                    NoteCuisson = "Ajouter la crème de coco hors du feu" },

            // Poulet (13) → Tofu (14) : végé + végan
            new() { IdIngredientOriginal = 13, IdIngredientSubstitut = 14,
                    TypeSubstitution = TypeSubstitution.Vegetarien, RatioConversion = 1.0m,
                    NoteCuisson = "Presser le tofu 30 min avant, puis faire mariner" },
            new() { IdIngredientOriginal = 13, IdIngredientSubstitut = 14,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 1.0m,
                    NoteCuisson = "Presser le tofu 30 min avant, puis faire mariner" }
        };

        await context.SubstitutionsIngredients.AddRangeAsync(substitutions);
        await context.SaveChangesAsync();
    }
}