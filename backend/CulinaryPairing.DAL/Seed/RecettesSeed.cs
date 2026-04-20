using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 6 recettes du CdC section 3.5.1 avec les extensions V1.1/V1.2 (section 3.5.2)
// directement intégrées. Les IDs implicites 1..6 sont référencés par
// RecetteIngredientsSeed, EtapesSeed, QuizSeed et AccordsSeed.
// Velouté de butternut (id 4) : valeurs par défaut V1.2 (pas d'UPDATE dans le CdC).
public static class RecettesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Recettes.Any()) return;

        var recettes = new List<Recette>
        {
            // id = 1 : Boeuf Bourguignon (Admin)
            new() {
                Titre = "Boeuf Bourguignon",
                Description = "Un classique de la cuisine française, mijoté au vin rouge",
                TempsPreparation = 180, Difficulte = Difficulte.Difficile, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 6,
                NiveauGras = 7, NiveauAcidite = 4, NiveauPiquant = 0, NiveauUmami = 6,
                NiveauSucre = 2, NiveauAromeEpice = 2, NiveauSel = 6, IntensiteAromatique = 8,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Friendly,
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Vin,
                EstPubliee = true, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 35.00m, IdUtilisateur = 1
            },

            // id = 2 : Risotto aux champignons (Admin) - CAS PHARE du CdC
            new() {
                Titre = "Risotto aux champignons",
                Description = "Risotto crémeux aux champignons de Paris et parmesan",
                TempsPreparation = 30, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 8, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 8,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 5, IntensiteAromatique = 6,
                ContientUmamiPur = true, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Hostile,  // umami pur + parmesan
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Beurre,
                EstPubliee = true, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 15.00m, IdUtilisateur = 1
            },

            // id = 3 : Salade César (Marie)
            new() {
                Titre = "Salade César",
                Description = "Salade classique avec poulet grillé et sauce César",
                TempsPreparation = 20, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 2,
                NiveauGras = 5, NiveauAcidite = 5, NiveauPiquant = 0, NiveauUmami = 5,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 6, IntensiteAromatique = 5,
                ContientUmamiPur = true, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Hostile,  // œuf cru dans sauce + parmesan
                ModeCuisson = ModeCuisson.Cru, TypeSauce = TypeSauce.Creme,
                EstPubliee = true, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 10.00m, IdUtilisateur = 2
            },

            // id = 4 : Velouté de butternut (Admin) - valeurs V1.2 par défaut
            new() {
                Titre = "Velouté de butternut",
                Description = "Soupe onctueuse à la courge butternut",
                TempsPreparation = 35, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 4,
                NiveauGras = 4, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 3,
                // Pas d'UPDATE V1.2 dans le CdC pour cette recette : on laisse les défauts du schéma
                EstPubliee = true, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 8.00m, IdUtilisateur = 1
            },

            // id = 5 : Poulet rôti aux herbes (Marie)
            new() {
                Titre = "Poulet rôti aux herbes",
                Description = "Poulet entier rôti au four avec thym et romarin",
                TempsPreparation = 75, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 5, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 4,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 5, IntensiteAromatique = 6,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Roti, TypeSauce = TypeSauce.Jus,
                EstPubliee = true, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 18.00m, IdUtilisateur = 2
            },

            // id = 6 : Panna cotta fruits rouges (Admin) - dessert
            new() {
                Titre = "Panna cotta fruits rouges",
                Description = "Dessert italien crémeux aux fruits rouges",
                TempsPreparation = 20, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Dessert,
                NombrePersonnesBase = 4,
                NiveauGras = 6, NiveauAcidite = 4, NiveauPiquant = 0, NiveauUmami = 2,
                NiveauSucre = 7, NiveauAromeEpice = 2, NiveauSel = 1, IntensiteAromatique = 4,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Cru, TypeSauce = TypeSauce.Sans,
                EstPubliee = true, AdaptableVege = true, AdaptableVegan = false,  // contient crème + gélatine
                CoutEstime = 12.00m, IdUtilisateur = 1
            }
        };

        await context.Recettes.AddRangeAsync(recettes);
        await context.SaveChangesAsync();
    }
}