using Microsoft.EntityFrameworkCore;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Catalogue des ingrédients. Pattern différentiel par nom (résilient aux IDs) :
// charge les noms existants en DB, n'ajoute que ce qui manque. Permet d'étendre
// le catalogue dans le temps sans purge ni reset DB. Aligné sur le pattern
// utilisé par IngredientsContraintesSeed et SubstitutionsSeed (homogénéisation V1.4).
//
// V1.0 : 20 ingrédients de base + 9 substitutions végé/végan.
// V1.3 : +50 ingrédients pour les recettes 7..15 (canard, agneau, crevettes, etc.).
// V1.3.1 : +3 substituts végétaux (Burger) + 12 compléments recettes.
// V1.4 : +12 substituts pour modes sans-gluten, sans-lactose et compléments Vegan (R17bis/R18bis).
public static class IngredientsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        // Catalogue complet. L'ordre est conservé pour préserver les IDs implicites
        // V1.0/V1.3 référencés historiquement par les autres seeds (RecetteIngredient, etc.).
        // Les nouveaux ingrédients V1.4 sont ajoutés à la fin → IDs 86+ (non-bloquants).
        var catalogue = new List<Ingredient>
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
            new() { Nom = "Poivre",               UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.020m },

            // ===== 50 nouveaux ingrédients V1.3 (recettes 7 à 15) =====
            // EstAllergene reste à false par défaut (legacy V1.0, dette V1.3 : remplacé par INGREDIENT_CONTRAINTE)

            // id = 21..28 : protéines (mer, terre, transformées)
            new() { Nom = "Saint-Jacques",        UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = false, CoutUnitaire = 3.50m  },
            new() { Nom = "Magret de canard",     UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.030m },
            new() { Nom = "Agneau",               UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.028m },
            new() { Nom = "Bacon fumé",           UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.022m },
            new() { Nom = "Boeuf haché",          UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.020m },
            new() { Nom = "Filet de boeuf",       UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.045m },
            new() { Nom = "Crevettes",            UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.025m },
            new() { Nom = "Boeuf à bouillir",     UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.018m },

            // id = 29..38 : légumes et fruits frais
            new() { Nom = "Carotte",              UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.30m  },
            new() { Nom = "Échalote",             UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.20m  },
            new() { Nom = "Gingembre frais",      UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Citron vert",          UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.40m  },
            new() { Nom = "Orange",               UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.50m  },
            new() { Nom = "Tomate",               UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.35m  },
            new() { Nom = "Salade romaine",       UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 1.20m  },
            new() { Nom = "Pomme de terre",       UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.002m },
            new() { Nom = "Cornichon",            UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.10m  },
            new() { Nom = "Abricots secs",        UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.018m },

            // id = 39..41 : produits laitiers et oeufs supplémentaires
            new() { Nom = "Cheddar",              UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.025m },
            new() { Nom = "Lait",                 UniteDefaut = "ml",     EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.001m },
            new() { Nom = "Gélatine",             UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.150m },

            // id = 42..46 : féculents, farines, pains
            new() { Nom = "Farine de blé",        UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.002m },
            new() { Nom = "Pâte brisée",          UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.008m },
            new() { Nom = "Pain à burger",        UniteDefaut = "pièce",  EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.80m  },
            new() { Nom = "Pain (croûtons)",      UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.005m },
            new() { Nom = "Nouilles de riz",      UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.006m },

            // id = 47..54 : épices et aromates secs
            new() { Nom = "Cannelle",             UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.080m },
            new() { Nom = "Cardamome",            UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.150m },
            new() { Nom = "Cumin",                UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.060m },
            new() { Nom = "Pâte de curry rouge",  UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.040m },
            new() { Nom = "Paprika",              UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.050m },
            new() { Nom = "Ras-el-hanout",        UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.090m },
            new() { Nom = "Anis étoilé",          UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.05m  },
            new() { Nom = "Bâton de cannelle",    UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 0.30m  },

            // id = 55..60 : aromates frais
            new() { Nom = "Basilic thaï",         UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.060m },
            new() { Nom = "Coriandre fraîche",    UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.050m },
            new() { Nom = "Menthe fraîche",       UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.050m },
            new() { Nom = "Thym",                 UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.040m },
            new() { Nom = "Romarin",              UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.040m },
            new() { Nom = "Persil",               UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.030m },

            // id = 61..67 : liquides et sauces
            new() { Nom = "Lait de coco",         UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.005m },
            new() { Nom = "Sauce poisson (nuoc-mâm)", UniteDefaut = "ml", EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.008m },
            new() { Nom = "Sauce soja",           UniteDefaut = "ml",     EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.010m },
            new() { Nom = "Bouillon de boeuf",    UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.003m },
            new() { Nom = "Vin rouge de cuisine", UniteDefaut = "ml",     EstAllergene = false, EstAlcool = true,  EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.005m },
            new() { Nom = "Miel",                 UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Vinaigre balsamique",  UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.015m },

            // id = 68..70 : sucre et chocolat (pour fondant)
            new() { Nom = "Sucre",                UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.002m },
            new() { Nom = "Chocolat noir 70%",    UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.020m },
            new() { Nom = "Coulis de fruits rouges", UniteDefaut = "ml",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.012m },

            // === Substituts végétaux ajoutés en V1.3.1 pour adapter le Burger ===
            new() { Nom = "Steak végétal",        UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.040m },
            new() { Nom = "Bacon végétal",        UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.030m },
            new() { Nom = "Cheddar végétal",      UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.035m },

            // === V1.3.1 (suite) : ingrédients pour combler les recettes (Bourguignon, Velouté, César, Tartare, Burger, Panna cotta, Carpaccio, Pho) ===
            // id = 74..80 : compléments recettes (audit V1.3.1)
            new() { Nom = "Courge butternut",     UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.004m },
            new() { Nom = "Bouillon de légumes",  UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.003m },
            new() { Nom = "Croûtons",             UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Anchois",              UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = true,  CoutUnitaire = 0.040m },
            new() { Nom = "Moutarde",             UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.008m },
            new() { Nom = "Ketchup",              UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.006m },
            new() { Nom = "Câpres",               UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.025m },

            // id = 81..85 : enrichissement Panna cotta, Carpaccio, Pho
            new() { Nom = "Fraises",              UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.015m },
            new() { Nom = "Ciboulette",           UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.045m },
            new() { Nom = "Pamplemousse",         UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 1.20m  },
            new() { Nom = "Crème balsamique",     UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.025m },
            new() { Nom = "Cebette",              UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = false, EstVegan = false, Divisible = false, CoutUnitaire = 0.35m  },

            // ===== V1.4 — Substituts pour modes sans-gluten et sans-lactose (R17bis/R18bis) =====
            // id = 86..91 : substituts sans gluten (5)
            new() { Nom = "Farine de riz",                UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.004m },
            new() { Nom = "Pâte brisée sans gluten",      UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.012m },
            new() { Nom = "Pain à burger sans gluten",    UniteDefaut = "pièce",  EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = false, CoutUnitaire = 1.20m  },
            new() { Nom = "Pain sans gluten en cubes",    UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.008m },
            new() { Nom = "Tamari",                       UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.015m },

            // id = 92..96 : substituts sans lactose (5, produits laitiers délactosés)
            new() { Nom = "Beurre sans lactose",          UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.020m },
            new() { Nom = "Crème fraîche sans lactose",   UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.015m },
            new() { Nom = "Lait sans lactose",            UniteDefaut = "ml",     EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.002m },
            new() { Nom = "Parmesan sans lactose",        UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.040m },
            new() { Nom = "Cheddar sans lactose",         UniteDefaut = "g",      EstAllergene = true,  EstAlcool = false, EstVege = true,  EstVegan = false, Divisible = true,  CoutUnitaire = 0.030m },

            // id = 97 : complément Vegan (Lait d'avoine pour substituer Lait, trou du seed V1.3)
            new() { Nom = "Lait d'avoine",                UniteDefaut = "ml",     EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.004m },

            // id = 98 : substitut Vegetarien + Vegan pour Gélatine
            new() { Nom = "Agar-agar",                    UniteDefaut = "g",      EstAllergene = false, EstAlcool = false, EstVege = true,  EstVegan = true,  Divisible = true,  CoutUnitaire = 0.180m }
        };

        // Pattern différentiel : charge les noms déjà en DB, n'insère que les manquants.
        // Idempotent — peut être relancé N fois sans dupliquer ni casser les FK existantes.
        var nomsExistants = await context.Ingredients
            .Select(i => i.Nom)
            .ToListAsync();

        var nouveaux = catalogue
            .Where(c => !nomsExistants.Contains(c.Nom))
            .ToList();

        if (nouveaux.Count == 0) return;

        await context.Ingredients.AddRangeAsync(nouveaux);
        await context.SaveChangesAsync();
    }
}