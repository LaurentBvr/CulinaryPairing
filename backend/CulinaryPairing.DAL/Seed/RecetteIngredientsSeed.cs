using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Compositions des 15 recettes à partir du catalogue d'ingrédients.
// V1.3 : 6 recettes initiales (CdC 3.5.1) + 9 recettes étendues (id 7-15).
// V1.3.1 : enrichissements post-remise pour combler les incohérences titre/ingrédients
// identifiées lors de la phase de polish (Bourguignon sans vin/carottes, Velouté
// sans butternut, Panna cotta sans gélatine/sucre/fraises, etc.) et pour ajouter
// moutarde/ketchup/anchois cohérents avec les noms des recettes.
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
            new() { IdRecette = 1, IdIngredient = 29, Quantite = 200 },   // V1.3.1 : Carotte
            new() { IdRecette = 1, IdIngredient = 65, Quantite = 500 },   // V1.3.1 : Vin rouge de cuisine
            new() { IdRecette = 1, IdIngredient = 60, Quantite = 15 },    // V1.3.1 : Persil en finition

            // ===== Recette 2 : Risotto aux champignons (4 personnes) — CdC 3.5.1 =====
            new() { IdRecette = 2, IdIngredient = 5,  Quantite = 300 },   // Riz arborio
            new() { IdRecette = 2, IdIngredient = 6,  Quantite = 200 },   // Champignons de Paris
            new() { IdRecette = 2, IdIngredient = 7,  Quantite = 50 },    // Parmesan
            new() { IdRecette = 2, IdIngredient = 9,  Quantite = 30 },    // Beurre
            new() { IdRecette = 2, IdIngredient = 17, Quantite = 1 },     // Oignon
            new() { IdRecette = 2, IdIngredient = 18, Quantite = 2 },     // Ail
            new() { IdRecette = 2, IdIngredient = 19, Quantite = 5 },     // Sel
            new() { IdRecette = 2, IdIngredient = 20, Quantite = 2 },     // Poivre
            new() { IdRecette = 2, IdIngredient = 75, Quantite = 1000 },  // V1.3.1 : Bouillon de légumes (base risotto)

            // ===== Recette 3 : Salade César (2 personnes) =====
            new() { IdRecette = 3, IdIngredient = 13, Quantite = 300 },   // Poulet
            new() { IdRecette = 3, IdIngredient = 7,  Quantite = 30 },    // Parmesan
            new() { IdRecette = 3, IdIngredient = 16, Quantite = 1 },     // Oeuf (pour la sauce)
            new() { IdRecette = 3, IdIngredient = 15, Quantite = 1 },     // Citron
            new() { IdRecette = 3, IdIngredient = 10, Quantite = 40 },    // Huile d'olive
            new() { IdRecette = 3, IdIngredient = 18, Quantite = 1 },     // Ail
            new() { IdRecette = 3, IdIngredient = 19, Quantite = 3 },     // Sel
            new() { IdRecette = 3, IdIngredient = 20, Quantite = 1 },     // Poivre
            new() { IdRecette = 3, IdIngredient = 67, Quantite = 1 },     // V1.3.1 : Vinaigre balsamique (sauce)
            new() { IdRecette = 3, IdIngredient = 76, Quantite = 50 },    // V1.3.1 : Croûtons
            new() { IdRecette = 3, IdIngredient = 77, Quantite = 30 },    // V1.3.1 : Anchois (sauce César traditionnelle)

            // ===== Recette 4 : Velouté de butternut (4 personnes) =====
            new() { IdRecette = 4, IdIngredient = 11, Quantite = 150 },   // Crème fraîche
            new() { IdRecette = 4, IdIngredient = 9,  Quantite = 30 },    // Beurre
            new() { IdRecette = 4, IdIngredient = 17, Quantite = 1 },     // Oignon
            new() { IdRecette = 4, IdIngredient = 18, Quantite = 2 },     // Ail
            new() { IdRecette = 4, IdIngredient = 19, Quantite = 5 },     // Sel
            new() { IdRecette = 4, IdIngredient = 20, Quantite = 2 },     // Poivre
            new() { IdRecette = 4, IdIngredient = 74, Quantite = 800 },   // V1.3.1 : Courge butternut (ingrédient principal)
            new() { IdRecette = 4, IdIngredient = 75, Quantite = 800 },   // V1.3.1 : Bouillon de légumes
            new() { IdRecette = 4, IdIngredient = 60, Quantite = 10 },    // V1.3.1 : Persil en finition

            // ===== Recette 5 : Poulet rôti aux herbes (4 personnes) =====
            new() { IdRecette = 5, IdIngredient = 13, Quantite = 1400 },  // Poulet entier ~1.4 kg
            new() { IdRecette = 5, IdIngredient = 9,  Quantite = 50 },    // Beurre (sous la peau)
            new() { IdRecette = 5, IdIngredient = 10, Quantite = 30 },    // Huile d'olive
            new() { IdRecette = 5, IdIngredient = 15, Quantite = 1 },     // Citron (dans la cavité)
            new() { IdRecette = 5, IdIngredient = 18, Quantite = 4 },     // Ail
            new() { IdRecette = 5, IdIngredient = 19, Quantite = 8 },     // Sel
            new() { IdRecette = 5, IdIngredient = 20, Quantite = 3 },     // Poivre
            new() { IdRecette = 5, IdIngredient = 58, Quantite = 10 },    // V1.3.1 : Thym (titre = "aux herbes")
            new() { IdRecette = 5, IdIngredient = 59, Quantite = 10 },    // V1.3.1 : Romarin (titre = "aux herbes")

            // ===== Recette 6 : Panna cotta fruits rouges (4 personnes) =====
            new() { IdRecette = 6, IdIngredient = 11, Quantite = 400 },   // Crème fraîche (base de la panna cotta)
            new() { IdRecette = 6, IdIngredient = 15, Quantite = 1 },     // Citron (zeste pour parfumer)
            new() { IdRecette = 6, IdIngredient = 68, Quantite = 80 },    // V1.3.1 : Sucre
            new() { IdRecette = 6, IdIngredient = 41, Quantite = 6 },     // V1.3.1 : Gélatine (texture caractéristique)
            new() { IdRecette = 6, IdIngredient = 40, Quantite = 300 },   // V1.3.1 : Lait
            new() { IdRecette = 6, IdIngredient = 70, Quantite = 100 },   // V1.3.1 : Coulis de fruits rouges
            new() { IdRecette = 6, IdIngredient = 81, Quantite = 200 },   // V1.3.1 : Fraises fraîches
            new() { IdRecette = 6, IdIngredient = 57, Quantite = 10 },    // V1.3.1 : Menthe fraîche en finition

            // ===== Recette 7 : Carpaccio de Saint-Jacques aux agrumes (4 personnes) =====
            // V1.3.1 : remplacement coriandre → ciboulette + ajout pamplemousse et crème balsamique
            new() { IdRecette = 7, IdIngredient = 21, Quantite = 12 },    // Saint-Jacques (3 par personne)
            new() { IdRecette = 7, IdIngredient = 32, Quantite = 2 },     // Citron vert
            new() { IdRecette = 7, IdIngredient = 33, Quantite = 1 },     // Orange
            new() { IdRecette = 7, IdIngredient = 10, Quantite = 60 },    // Huile d'olive
            new() { IdRecette = 7, IdIngredient = 82, Quantite = 8 },     // V1.3.1 : Ciboulette (remplace coriandre)
            new() { IdRecette = 7, IdIngredient = 83, Quantite = 1 },     // V1.3.1 : Pamplemousse en suprêmes
            new() { IdRecette = 7, IdIngredient = 84, Quantite = 15 },    // V1.3.1 : Crème balsamique en décoration
            new() { IdRecette = 7, IdIngredient = 19, Quantite = 2 },     // Sel (fleur de sel)
            new() { IdRecette = 7, IdIngredient = 20, Quantite = 1 },     // Poivre

            // ===== Recette 8 : Tartare de boeuf classique (2 personnes) =====
            new() { IdRecette = 8, IdIngredient = 26, Quantite = 300 },   // Filet de boeuf
            new() { IdRecette = 8, IdIngredient = 16, Quantite = 2 },     // Oeuf (jaune)
            new() { IdRecette = 8, IdIngredient = 30, Quantite = 2 },     // Échalote
            new() { IdRecette = 8, IdIngredient = 37, Quantite = 4 },     // Cornichon
            new() { IdRecette = 8, IdIngredient = 60, Quantite = 10 },    // Persil
            new() { IdRecette = 8, IdIngredient = 19, Quantite = 2 },     // Sel
            new() { IdRecette = 8, IdIngredient = 20, Quantite = 2 },     // Poivre
            new() { IdRecette = 8, IdIngredient = 78, Quantite = 10 },    // V1.3.1 : Moutarde
            new() { IdRecette = 8, IdIngredient = 80, Quantite = 15 },    // V1.3.1 : Câpres

            // ===== Recette 9 : Soupe Pho au boeuf (4 personnes) =====
            new() { IdRecette = 9, IdIngredient = 28, Quantite = 800 },   // Boeuf à bouillir
            new() { IdRecette = 9, IdIngredient = 26, Quantite = 200 },   // Filet de boeuf (tranches finales)
            new() { IdRecette = 9, IdIngredient = 46, Quantite = 300 },   // Nouilles de riz
            new() { IdRecette = 9, IdIngredient = 64, Quantite = 1500 },  // Bouillon de boeuf
            new() { IdRecette = 9, IdIngredient = 31, Quantite = 50 },    // Gingembre frais
            new() { IdRecette = 9, IdIngredient = 17, Quantite = 1 },     // Oignon
            new() { IdRecette = 9, IdIngredient = 53, Quantite = 4 },     // Anis étoilé
            new() { IdRecette = 9, IdIngredient = 54, Quantite = 1 },     // Bâton de cannelle
            new() { IdRecette = 9, IdIngredient = 55, Quantite = 20 },    // Basilic thaï
            new() { IdRecette = 9, IdIngredient = 56, Quantite = 20 },    // Coriandre fraîche
            new() { IdRecette = 9, IdIngredient = 32, Quantite = 2 },     // Citron vert
            new() { IdRecette = 9, IdIngredient = 62, Quantite = 30 },    // Sauce poisson
            new() { IdRecette = 9, IdIngredient = 19, Quantite = 8 },     // Sel
            new() { IdRecette = 9, IdIngredient = 85, Quantite = 4 },     // V1.3.1 : Cebette (classique vietnamien)

            // ===== Recette 10 : Magret de canard à l'orange (4 personnes) =====
            new() { IdRecette = 10, IdIngredient = 22, Quantite = 800 },  // Magret de canard (2 magrets)
            new() { IdRecette = 10, IdIngredient = 33, Quantite = 3 },    // Orange
            new() { IdRecette = 10, IdIngredient = 66, Quantite = 30 },   // Miel
            new() { IdRecette = 10, IdIngredient = 67, Quantite = 30 },   // Vinaigre balsamique
            new() { IdRecette = 10, IdIngredient = 9,  Quantite = 30 },   // Beurre
            new() { IdRecette = 10, IdIngredient = 19, Quantite = 5 },    // Sel
            new() { IdRecette = 10, IdIngredient = 20, Quantite = 2 },    // Poivre
            new() { IdRecette = 10, IdIngredient = 56, Quantite = 10 },   // V1.3.1 : Coriandre fraîche en finition

            // ===== Recette 11 : Curry rouge thaï au poulet (4 personnes) =====
            new() { IdRecette = 11, IdIngredient = 13, Quantite = 600 },  // Poulet
            new() { IdRecette = 11, IdIngredient = 50, Quantite = 60 },   // Pâte de curry rouge
            new() { IdRecette = 11, IdIngredient = 61, Quantite = 400 },  // Lait de coco
            new() { IdRecette = 11, IdIngredient = 62, Quantite = 20 },   // Sauce poisson
            new() { IdRecette = 11, IdIngredient = 31, Quantite = 20 },   // Gingembre frais
            new() { IdRecette = 11, IdIngredient = 18, Quantite = 3 },    // Ail
            new() { IdRecette = 11, IdIngredient = 32, Quantite = 1 },    // Citron vert
            new() { IdRecette = 11, IdIngredient = 55, Quantite = 15 },   // Basilic thaï
            new() { IdRecette = 11, IdIngredient = 68, Quantite = 10 },   // Sucre

            // ===== Recette 12 : Burger gourmet bacon-cheddar (4 personnes) =====
            new() { IdRecette = 12, IdIngredient = 25, Quantite = 600 },  // Boeuf haché (150g/burger)
            new() { IdRecette = 12, IdIngredient = 24, Quantite = 200 },  // Bacon fumé
            new() { IdRecette = 12, IdIngredient = 39, Quantite = 120 },  // Cheddar
            new() { IdRecette = 12, IdIngredient = 44, Quantite = 4 },    // Pain à burger
            new() { IdRecette = 12, IdIngredient = 35, Quantite = 1 },    // Salade romaine
            new() { IdRecette = 12, IdIngredient = 34, Quantite = 2 },    // Tomate
            new() { IdRecette = 12, IdIngredient = 17, Quantite = 1 },    // Oignon
            new() { IdRecette = 12, IdIngredient = 19, Quantite = 4 },    // Sel
            new() { IdRecette = 12, IdIngredient = 20, Quantite = 2 },    // Poivre
            new() { IdRecette = 12, IdIngredient = 78, Quantite = 15 },   // V1.3.1 : Moutarde
            new() { IdRecette = 12, IdIngredient = 79, Quantite = 20 },   // V1.3.1 : Ketchup

            // ===== Recette 13 : Tajine d'agneau aux abricots (6 personnes) =====
            new() { IdRecette = 13, IdIngredient = 23, Quantite = 1200 }, // Agneau (épaule)
            new() { IdRecette = 13, IdIngredient = 38, Quantite = 200 },  // Abricots secs
            new() { IdRecette = 13, IdIngredient = 52, Quantite = 15 },   // Ras-el-hanout
            new() { IdRecette = 13, IdIngredient = 47, Quantite = 5 },    // Cannelle
            new() { IdRecette = 13, IdIngredient = 49, Quantite = 5 },    // Cumin
            new() { IdRecette = 13, IdIngredient = 17, Quantite = 2 },    // Oignon
            new() { IdRecette = 13, IdIngredient = 29, Quantite = 4 },    // Carotte
            new() { IdRecette = 13, IdIngredient = 18, Quantite = 4 },    // Ail
            new() { IdRecette = 13, IdIngredient = 66, Quantite = 30 },   // Miel
            new() { IdRecette = 13, IdIngredient = 56, Quantite = 15 },   // Coriandre fraîche
            new() { IdRecette = 13, IdIngredient = 10, Quantite = 40 },   // Huile d'olive
            new() { IdRecette = 13, IdIngredient = 19, Quantite = 8 },    // Sel

            // ===== Recette 14 : Quiche lorraine maison (6 personnes) =====
            new() { IdRecette = 14, IdIngredient = 43, Quantite = 250 },  // Pâte brisée
            new() { IdRecette = 14, IdIngredient = 2,  Quantite = 200 },  // Lardons
            new() { IdRecette = 14, IdIngredient = 16, Quantite = 4 },    // Oeuf
            new() { IdRecette = 14, IdIngredient = 11, Quantite = 250 },  // Crème fraîche
            new() { IdRecette = 14, IdIngredient = 40, Quantite = 100 },  // Lait
            new() { IdRecette = 14, IdIngredient = 19, Quantite = 3 },    // Sel
            new() { IdRecette = 14, IdIngredient = 20, Quantite = 2 },    // Poivre

            // ===== Recette 15 : Fondant au chocolat noir (4 personnes) =====
            new() { IdRecette = 15, IdIngredient = 69, Quantite = 200 },  // Chocolat noir 70%
            new() { IdRecette = 15, IdIngredient = 9,  Quantite = 100 },  // Beurre
            new() { IdRecette = 15, IdIngredient = 16, Quantite = 4 },    // Oeuf
            new() { IdRecette = 15, IdIngredient = 68, Quantite = 80 },   // Sucre
            new() { IdRecette = 15, IdIngredient = 42, Quantite = 50 },   // Farine de blé
            new() { IdRecette = 15, IdIngredient = 70, Quantite = 100 }   // Coulis de fruits rouges
        };

        await context.RecettesIngredients.AddRangeAsync(compositions);
        await context.SaveChangesAsync();
    }
}