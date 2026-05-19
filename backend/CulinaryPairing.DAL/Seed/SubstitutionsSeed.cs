using Microsoft.EntityFrameworkCore;
using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Substitutions d'ingrédients. Pattern différentiel par (nom_orig, nom_sub, type) :
// charge l'existant en DB, n'ajoute que les lignes manquantes. Résilient aux IDs
// (aligné sur IngredientsContraintesSeed et IngredientsSeed — homogénéisation V1.4).
//
// V1.0  : 9 substitutions végé/végan (couples laitiers, protéines, fromages).
// V1.3.1: +5 substitutions pour le Burger (Boeuf haché → Steak végétal, etc.).
// V1.4  : +13 substitutions pour modes sans-gluten/sans-lactose (R17bis/R18bis)
//         et complément Vegan (Lait → Lait d'avoine, Gélatine → Agar-agar).
public static class SubstitutionsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        // Catalogue déclaratif : (nom_original, nom_substitut, type, ratio, note).
        // Référencement par nom (et non par ID) pour résilience aux drifts d'ID
        // entre environnements et aux ré-ordonnancements d'IngredientsSeed.
        var catalogue = new List<(string Original, string Substitut, TypeSubstitution Type, decimal Ratio, string? Note)>
        {
            // ===== V1.0 — Substitutions végé/végan (9) =====
            ("Boeuf",         "Pleurotes",            TypeSubstitution.Vegetarien, 0.8m,
                "Ajouter les pleurotes en fin de cuisson, faire revenir 5 min à feu vif"),
            ("Boeuf",         "Pleurotes",            TypeSubstitution.Vegan,      0.8m,
                "Ajouter les pleurotes en fin de cuisson, faire revenir 5 min à feu vif"),
            ("Lardons",       "Lardons végétaux",     TypeSubstitution.Vegetarien, 1.0m, null),
            ("Lardons",       "Lardons végétaux",     TypeSubstitution.Vegan,      1.0m, null),
            ("Parmesan",      "Levure nutritionnelle",TypeSubstitution.Vegan,      0.3m,
                "Saupoudrer la levure nutritionnelle au moment de servir"),
            ("Beurre",        "Huile d'olive",        TypeSubstitution.Vegan,      0.9m,
                "Utiliser de l'huile d'olive à la place du beurre"),
            ("Crème fraîche", "Crème de coco",        TypeSubstitution.Vegan,      1.0m,
                "Ajouter la crème de coco hors du feu"),
            ("Poulet",        "Tofu",                 TypeSubstitution.Vegetarien, 1.0m,
                "Presser le tofu 30 min avant, puis faire mariner"),
            ("Poulet",        "Tofu",                 TypeSubstitution.Vegan,      1.0m,
                "Presser le tofu 30 min avant, puis faire mariner"),

            // ===== V1.3.1 — Adaptations Burger (5) =====
            ("Boeuf haché",   "Steak végétal",        TypeSubstitution.Vegetarien, 1.0m,
                "Cuire le steak végétal 3 min par face à feu moyen, ne pas trop saisir"),
            ("Boeuf haché",   "Steak végétal",        TypeSubstitution.Vegan,      1.0m,
                "Cuire le steak végétal 3 min par face à feu moyen, ne pas trop saisir"),
            ("Bacon fumé",    "Bacon végétal",        TypeSubstitution.Vegetarien, 1.0m,
                "Poêler 2 min de chaque côté pour développer le croustillant"),
            ("Bacon fumé",    "Bacon végétal",        TypeSubstitution.Vegan,      1.0m,
                "Poêler 2 min de chaque côté pour développer le croustillant"),
            ("Cheddar",       "Cheddar végétal",      TypeSubstitution.Vegan,      1.0m,
                "Ajouter en fin de cuisson, le cheddar végétal fond plus rapidement"),

            // ===== V1.4 — Substitutions sans gluten (5, R17bis) =====
            ("Farine de blé",   "Farine de riz",                 TypeSubstitution.SansGluten, 0.9m,
                "Diminuer légèrement la quantité, ajouter un liant (gomme de xanthane) pour pâtes levées"),
            ("Pâte brisée",     "Pâte brisée sans gluten",       TypeSubstitution.SansGluten, 1.0m,
                "Manipuler froide, plus friable que la pâte classique"),
            ("Pain à burger",   "Pain à burger sans gluten",     TypeSubstitution.SansGluten, 1.0m,
                "Toaster légèrement pour éviter l'effritement"),
            ("Pain (croûtons)", "Pain sans gluten en cubes",     TypeSubstitution.SansGluten, 1.0m,
                "Sécher au four 8 min à 180°C avant utilisation"),
            ("Sauce soja",      "Tamari",                        TypeSubstitution.SansGluten, 1.0m,
                "Goût plus prononcé, ajuster les autres assaisonnements"),

            // ===== V1.4 — Substitutions sans lactose (5, R18bis, produits laitiers délactosés) =====
            ("Beurre",        "Beurre sans lactose",          TypeSubstitution.SansLactose, 1.0m,
                "Comportement culinaire identique au beurre classique"),
            ("Crème fraîche", "Crème fraîche sans lactose",   TypeSubstitution.SansLactose, 1.0m,
                "Texture et goût quasi-identiques au produit standard"),
            ("Lait",          "Lait sans lactose",            TypeSubstitution.SansLactose, 1.0m,
                "Légèrement plus sucré (le lactose résiduel est hydrolysé en glucose et galactose)"),
            ("Parmesan",      "Parmesan sans lactose",        TypeSubstitution.SansLactose, 1.0m,
                "Parmesan affiné plus de 36 mois, le lactose disparaît naturellement à la maturation"),
            ("Cheddar",       "Cheddar sans lactose",         TypeSubstitution.SansLactose, 1.0m,
                "Fonte et goût équivalents au cheddar standard"),

            // ===== V1.4 — Complément Vegan (1, trou du seed V1.3) =====
            ("Lait",          "Lait d'avoine",                TypeSubstitution.Vegan,       1.0m,
                "Substitut le plus neutre en goût, polyvalent (pâtisserie, sauce, soupe)"),

            // ===== V1.4 — Substitut Gélatine (2, Vegetarien + Vegan) =====
            ("Gélatine",      "Agar-agar",                    TypeSubstitution.Vegetarien,  0.4m,
                "Beaucoup plus gélifiant : 2g d'agar-agar = 5g de gélatine. Porter à ébullition 2 min"),
            ("Gélatine",      "Agar-agar",                    TypeSubstitution.Vegan,       0.4m,
                "Beaucoup plus gélifiant : 2g d'agar-agar = 5g de gélatine. Porter à ébullition 2 min")
        };

        // Charge les noms d'ingrédients référencés une fois (évite N+1).
        var nomsReferenced = catalogue
            .SelectMany(c => new[] { c.Original, c.Substitut })
            .Distinct()
            .ToList();

        var ingredients = await context.Ingredients
            .Where(i => nomsReferenced.Contains(i.Nom))
            .ToDictionaryAsync(i => i.Nom, i => i.IdIngredient);

        // Charge les liaisons existantes pour le diff (clé composite : orig + sub + type).
        var existantes = await context.SubstitutionsIngredients
            .Select(s => new { s.IdIngredientOriginal, s.IdIngredientSubstitut, s.TypeSubstitution })
            .ToListAsync();

        var setExistant = existantes
            .Select(e => (e.IdIngredientOriginal, e.IdIngredientSubstitut, e.TypeSubstitution))
            .ToHashSet();

        var nouvelles = new List<SubstitutionIngredient>();
        foreach (var (original, substitut, type, ratio, note) in catalogue)
        {
            if (!ingredients.TryGetValue(original, out var idOrig)) continue;
            if (!ingredients.TryGetValue(substitut, out var idSub)) continue;

            if (setExistant.Contains((idOrig, idSub, type))) continue;

            nouvelles.Add(new SubstitutionIngredient
            {
                IdIngredientOriginal = idOrig,
                IdIngredientSubstitut = idSub,
                TypeSubstitution = type,
                RatioConversion = ratio,
                NoteCuisson = note
            });
        }

        if (nouvelles.Count == 0) return;

        await context.SubstitutionsIngredients.AddRangeAsync(nouvelles);
        await context.SaveChangesAsync();
    }
}