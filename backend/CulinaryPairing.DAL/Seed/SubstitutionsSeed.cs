using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 14 substitutions : 9 du CdC v1.3 + 5 ajoutées en V1.3.1 pour le Burger.
// Les FK renvoient aux IDs implicites d'IngredientsSeed.
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
                    NoteCuisson = "Presser le tofu 30 min avant, puis faire mariner" },

            // === Substitutions ajoutées en V1.3.1 pour adapter le Burger ===
            // Boeuf haché (25) → Steak végétal (71) : végé + végan
            new() { IdIngredientOriginal = 25, IdIngredientSubstitut = 71,
                    TypeSubstitution = TypeSubstitution.Vegetarien, RatioConversion = 1.0m,
                    NoteCuisson = "Cuire le steak végétal 3 min par face à feu moyen, ne pas trop saisir" },
            new() { IdIngredientOriginal = 25, IdIngredientSubstitut = 71,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 1.0m,
                    NoteCuisson = "Cuire le steak végétal 3 min par face à feu moyen, ne pas trop saisir" },

            // Bacon fumé (24) → Bacon végétal (72) : végé + végan
            new() { IdIngredientOriginal = 24, IdIngredientSubstitut = 72,
                    TypeSubstitution = TypeSubstitution.Vegetarien, RatioConversion = 1.0m,
                    NoteCuisson = "Poêler 2 min de chaque côté pour développer le croustillant" },
            new() { IdIngredientOriginal = 24, IdIngredientSubstitut = 72,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 1.0m,
                    NoteCuisson = "Poêler 2 min de chaque côté pour développer le croustillant" },

            // Cheddar (39) → Cheddar végétal (73) : végan-only (le cheddar est déjà végé)
            new() { IdIngredientOriginal = 39, IdIngredientSubstitut = 73,
                    TypeSubstitution = TypeSubstitution.Vegan, RatioConversion = 1.0m,
                    NoteCuisson = "Ajouter en fin de cuisson, le cheddar végétal fond plus rapidement" }
        };

        await context.SubstitutionsIngredients.AddRangeAsync(substitutions);
        await context.SaveChangesAsync();
    }
}