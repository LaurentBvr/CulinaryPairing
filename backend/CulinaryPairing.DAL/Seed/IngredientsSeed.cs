using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 20 ingrédients du CdC section 3.5.1, couvrant les 6 recettes de base
// et servant de source pour les 9 substitutions végé/végane.
// Ordre strict : les IDs implicites (1..20) sont référencés par les seeds
// RecetteIngredient et SubstitutionIngredient ci-dessous.
public static class IngredientsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Ingredients.Any()) return;

        var ingredients = new List<Ingredient>
        {
            // id = 1..4 : protéines et leurs substituts végé/végan
            new() { Nom = "Boeuf",                UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.025m },
            new() { Nom = "Lardons",              UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.018m },
            new() { Nom = "Pleurotes",            UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.015m },
            new() { Nom = "Lardons végétaux",     UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.022m },

            // id = 5..8 : risotto + substituts
            new() { Nom = "Riz arborio",          UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.008m },
            new() { Nom = "Champignons de Paris", UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Parmesan",             UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.030m },
            new() { Nom = "Levure nutritionnelle",UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.050m },

            // id = 9..12 : matières grasses et substituts
            new() { Nom = "Beurre",               UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.015m },
            new() { Nom = "Huile d'olive",        UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Crème fraîche",        UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.010m },
            new() { Nom = "Crème de coco",        UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.008m },

            // id = 13..14 : poulet + tofu (substitut végé)
            new() { Nom = "Poulet",               UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Tofu",                 UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.010m },

            // id = 15..20 : aromates et assaisonnement
            new() { Nom = "Citron",               UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.50m  },
            new() { Nom = "Oeuf",                 UniteDefaut = "pièce",  EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = false, CoutUnitaire = 0.35m  },
            new() { Nom = "Oignon",               UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.30m  },
            new() { Nom = "Ail",                  UniteDefaut = "gousse", EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.10m  },
            new() { Nom = "Sel",                  UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.001m },
            new() { Nom = "Poivre",               UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.020m }
        };

        await context.Ingredients.AddRangeAsync(ingredients);
        await context.SaveChangesAsync();
    }
}