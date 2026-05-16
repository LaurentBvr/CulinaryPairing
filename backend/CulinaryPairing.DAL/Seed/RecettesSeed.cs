using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 6 recettes du CdC section 3.5.1 + 9 nouvelles V1.3 (couverture règles).
// Les IDs implicites 1..15 sont référencés par RecetteIngredientsSeed,
// EtapesSeed, QuizSeed et AccordsSeed.
public static class RecettesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Recettes.Any()) return;

        var recettes = new List<Recette>
        {
            // ===== 6 recettes V1.0/V1.1/V1.2 =====

            // id = 1 : Boeuf Bourguignon (Admin)
            new() {
                Titre = "Boeuf Bourguignon",
                Description = "Un classique de la cuisine française, mijoté au vin rouge",
                TempsPreparation = 30, TempsCuisson = 150, Difficulte = Difficulte.Difficile, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 6,
                NiveauGras = 7, NiveauAcidite = 4, NiveauPiquant = 0, NiveauUmami = 6,
                NiveauSucre = 2, NiveauAromeEpice = 2, NiveauSel = 6, IntensiteAromatique = 8,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Friendly,
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Vin,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 35.00m, IdUtilisateur = 1
            },

            // id = 2 : Risotto aux champignons (Admin) - CAS PHARE du CdC
            new() {
                Titre = "Risotto aux champignons",
                Description = "Risotto crémeux aux champignons de Paris et parmesan",
                TempsPreparation = 30, TempsCuisson = 25, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 8, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 8,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 5, IntensiteAromatique = 6,
                ContientUmamiPur = true, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Hostile,
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Beurre,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 15.00m, IdUtilisateur = 1
            },

            // id = 3 : Salade César (Marie)
            new() {
                Titre = "Salade César",
                Description = "Salade classique avec poulet grillé et sauce César",
                TempsPreparation = 20, TempsCuisson = 0, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 2,
                NiveauGras = 5, NiveauAcidite = 5, NiveauPiquant = 0, NiveauUmami = 5,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 6, IntensiteAromatique = 5,
                ContientUmamiPur = true, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Hostile,
                ModeCuisson = ModeCuisson.Cru, TypeSauce = TypeSauce.Creme,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 10.00m, IdUtilisateur = 2
            },

            // id = 4 : Velouté de butternut (Admin) - valeurs V1.2 par défaut
            new() {
                Titre = "Velouté de butternut",
                Description = "Soupe onctueuse à la courge butternut",
                TempsPreparation = 10, TempsCuisson = 25, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 4,
                NiveauGras = 4, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 3,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 8.00m, IdUtilisateur = 1
            },

            // id = 5 : Poulet rôti aux herbes (Marie)
            new() {
                Titre = "Poulet rôti aux herbes",
                Description = "Poulet entier rôti au four avec thym et romarin",
                TempsPreparation = 15, TempsCuisson = 60, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 5, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 4,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 5, IntensiteAromatique = 6,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Roti, TypeSauce = TypeSauce.Jus,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 18.00m, IdUtilisateur = 2
            },

            // id = 6 : Panna cotta fruits rouges (Admin) - dessert
            new() {
                Titre = "Panna cotta fruits rouges",
                Description = "Dessert italien crémeux aux fruits rouges",
                TempsPreparation = 15, TempsCuisson = 5, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Dessert,
                NombrePersonnesBase = 4,
                NiveauGras = 6, NiveauAcidite = 4, NiveauPiquant = 0, NiveauUmami = 2,
                NiveauSucre = 7, NiveauAromeEpice = 2, NiveauSel = 1, IntensiteAromatique = 4,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Cru, TypeSauce = TypeSauce.Sans,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = false,
                CoutEstime = 12.00m, IdUtilisateur = 1
            },

            // ===== 9 nouvelles recettes V1.3 (couverture règles + diversité) =====

            // id = 7 : Carpaccio de Saint-Jacques aux agrumes (Marie) - cible R14
            new() {
                Titre = "Carpaccio de Saint-Jacques aux agrumes",
                Description = "Saint-Jacques crues finement tranchées, marinées au citron vert et à l'huile d'olive, parfumées de coriandre fraîche.",
                TempsPreparation = 20, TempsCuisson = 0, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 4,
                NiveauGras = 3, NiveauAcidite = 7, NiveauPiquant = 0, NiveauUmami = 4,
                NiveauSucre = 1, NiveauAromeEpice = 1, NiveauSel = 4, IntensiteAromatique = 4,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Hostile,
                ModeCuisson = ModeCuisson.Cru, TypeSauce = TypeSauce.Agrume,
                Statut = StatutRecette.Publiee, AdaptableVege = false, AdaptableVegan = false,
                CoutEstime = 28.00m, IdUtilisateur = 2
            },

            // id = 8 : Tartare de boeuf classique (Thomas) - cible R13bis hostile
            new() {
                Titre = "Tartare de boeuf classique",
                Description = "Boeuf cru haché au couteau, oeuf, échalote, cornichons et câpres. Service à la cuillère.",
                TempsPreparation = 25, TempsCuisson = 0, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 2,
                NiveauGras = 5, NiveauAcidite = 4, NiveauPiquant = 2, NiveauUmami = 6,
                NiveauSucre = 1, NiveauAromeEpice = 2, NiveauSel = 5, IntensiteAromatique = 6,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Hostile,
                ModeCuisson = ModeCuisson.Cru, TypeSauce = TypeSauce.Sans,
                Statut = StatutRecette.Publiee, AdaptableVege = false, AdaptableVegan = false,
                CoutEstime = 18.00m, IdUtilisateur = 3
            },

            // id = 9 : Soupe Pho au boeuf (Marie) - long temps de cuisson, R19bis épice/herbacé
            new() {
                Titre = "Soupe Pho au boeuf",
                Description = "Bouillon vietnamien parfumé à l'anis étoilé et à la cannelle, nouilles de riz et fines tranches de boeuf saisi.",
                TempsPreparation = 30, TempsCuisson = 240, Difficulte = Difficulte.Difficile, TypePlat = TypePlat.Entree,
                NombrePersonnesBase = 4,
                NiveauGras = 4, NiveauAcidite = 3, NiveauPiquant = 2, NiveauUmami = 7,
                NiveauSucre = 2, NiveauAromeEpice = 5, NiveauSel = 6, IntensiteAromatique = 7,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Sans,
                Statut = StatutRecette.Publiee, AdaptableVege = false, AdaptableVegan = false,
                CoutEstime = 22.00m, IdUtilisateur = 2
            },

            // id = 10 : Magret de canard à l'orange (Admin) - cible R24bis + R10 + R20bis
            new() {
                Titre = "Magret de canard à l'orange",
                Description = "Magret de canard rôti, peau croustillante, sauce caramélisée à l'orange et au miel.",
                TempsPreparation = 20, TempsCuisson = 25, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 8, NiveauAcidite = 3, NiveauPiquant = 0, NiveauUmami = 5,
                NiveauSucre = 6, NiveauAromeEpice = 2, NiveauSel = 5, IntensiteAromatique = 7,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Friendly,
                ModeCuisson = ModeCuisson.Roti, TypeSauce = TypeSauce.Agrume,
                Statut = StatutRecette.Publiee, AdaptableVege = false, AdaptableVegan = false,
                CoutEstime = 26.00m, IdUtilisateur = 1
            },

            // id = 11 : Curry rouge thaï au poulet (Marie) - cible R11 + R25bis + R16bis malus
            new() {
                Titre = "Curry rouge thaï au poulet",
                Description = "Poulet mijoté dans une sauce épicée au curry rouge, lait de coco et basilic thaï frais.",
                TempsPreparation = 15, TempsCuisson = 30, Difficulte = Difficulte.Moyen, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 6, NiveauAcidite = 3, NiveauPiquant = 9, NiveauUmami = 5,
                NiveauSucre = 5, NiveauAromeEpice = 7, NiveauSel = 5, IntensiteAromatique = 9,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Creme,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 16.00m, IdUtilisateur = 2
            },

            // id = 12 : Burger gourmet bacon-cheddar (Admin) - cible R22bis + R13 fumé + R20bis
            new() {
                Titre = "Burger gourmet bacon-cheddar",
                Description = "Steak haché grillé, cheddar fondu, bacon fumé croustillant, salade et tomate dans un pain brioché.",
                TempsPreparation = 15, TempsCuisson = 15, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 4,
                NiveauGras = 8, NiveauAcidite = 3, NiveauPiquant = 1, NiveauUmami = 7,
                NiveauSucre = 2, NiveauAromeEpice = 1, NiveauSel = 7, IntensiteAromatique = 7,
                ContientUmamiPur = false, ContientFume = true,
                AffiniteTannins = AffiniteTannins.Friendly,
                ModeCuisson = ModeCuisson.Grille, TypeSauce = TypeSauce.Sans,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = true,
                CoutEstime = 14.00m, IdUtilisateur = 1
            },

            // id = 13 : Tajine d'agneau aux abricots (Admin) - cible R25bis + R24bis
            new() {
                Titre = "Tajine d'agneau aux abricots",
                Description = "Agneau mijoté longuement avec abricots secs, ras-el-hanout, cannelle et amandes. Cuisine maghrébine traditionnelle.",
                TempsPreparation = 25, TempsCuisson = 120, Difficulte = Difficulte.Difficile, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 6,
                NiveauGras = 6, NiveauAcidite = 3, NiveauPiquant = 1, NiveauUmami = 6,
                NiveauSucre = 5, NiveauAromeEpice = 8, NiveauSel = 5, IntensiteAromatique = 8,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Friendly,
                ModeCuisson = ModeCuisson.Mijote, TypeSauce = TypeSauce.Jus,
                Statut = StatutRecette.Publiee, AdaptableVege = false, AdaptableVegan = false,
                CoutEstime = 28.00m, IdUtilisateur = 1
            },

            // id = 14 : Quiche lorraine maison (Admin) - cible R10 + R13 fumé + R23bis
            new() {
                Titre = "Quiche lorraine maison",
                Description = "Pâte brisée garnie de lardons fumés et d'un appareil à la crème et aux oeufs. Le grand classique alsacien.",
                TempsPreparation = 20, TempsCuisson = 45, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Plat,
                NombrePersonnesBase = 6,
                NiveauGras = 8, NiveauAcidite = 3, NiveauPiquant = 0, NiveauUmami = 6,
                NiveauSucre = 1, NiveauAromeEpice = 1, NiveauSel = 7, IntensiteAromatique = 6,
                ContientUmamiPur = false, ContientFume = true,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Four, TypeSauce = TypeSauce.Creme,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = false,
                CoutEstime = 12.00m, IdUtilisateur = 1
            },

            // id = 15 : Fondant au chocolat noir (Admin) - dessert sucré, R11bis pousse Porto
            new() {
                Titre = "Fondant au chocolat noir",
                Description = "Coeur coulant au chocolat noir 70%, beurre demi-sel, à servir tiède avec un coulis de fruits rouges.",
                TempsPreparation = 15, TempsCuisson = 12, Difficulte = Difficulte.Facile, TypePlat = TypePlat.Dessert,
                NombrePersonnesBase = 4,
                NiveauGras = 8, NiveauAcidite = 2, NiveauPiquant = 0, NiveauUmami = 2,
                NiveauSucre = 8, NiveauAromeEpice = 1, NiveauSel = 2, IntensiteAromatique = 7,
                ContientUmamiPur = false, ContientFume = false,
                AffiniteTannins = AffiniteTannins.Neutre,
                ModeCuisson = ModeCuisson.Four, TypeSauce = TypeSauce.Sans,
                Statut = StatutRecette.Publiee, AdaptableVege = true, AdaptableVegan = false,
                CoutEstime = 8.00m, IdUtilisateur = 1
            }
        };

        await context.Recettes.AddRangeAsync(recettes);
        await context.SaveChangesAsync();

        // V1.4 : association des images après attribution des IDs par la DB.
        // Convention : /assets/recipes/{id_recette}.jpg (cf. frontend/public/assets/recipes/).
        foreach (var r in recettes)
        {
            r.ImageUrl = $"/assets/recipes/{r.IdRecette}.jpg";
        }
        await context.SaveChangesAsync();
    }
}