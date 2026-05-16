using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 20 boissons : 12 de base (CdC 3.5.1) + 8 V1.2 (CdC 3.5.3).
// V1.3 : enrichissement de l'origine polymorphe (pays/region/appellation/cepage).
// Champs nullables individuellement : un mocktail n'a pas d'origine ; un whisky
// a pays+region mais pas d'appellation ni de cépage ; un vin a les 4 champs.
public static class BoissonsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Boissons.Any()) return;

        var boissons = new List<Boisson>
        {
            // ===== 12 boissons V1.0 (avec updates V1.1/V1.2 intégrés) =====

            // id = 1 : Chardonnay
            new() {
                Nom = "Chardonnay", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 6, NiveauSucre = 3, NiveauTannins = 2, CoutMoyen = 12.00m,
                DegreAlcool = 13.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Moyen,
                NiveauFume = 1, NiveauAmertume = 1, TemperatureOptimale = 10, ToleranceTemperature = 2,
                Pays = "France", Region = "Bourgogne", Appellation = "Bourgogne AOC", Cepage = "Chardonnay"
            },
            // id = 2 : Pinot Grigio
            new() {
                Nom = "Pinot Grigio", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 7, NiveauSucre = 2, NiveauTannins = 1, CoutMoyen = 10.00m,
                DegreAlcool = 12.5m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 9, ToleranceTemperature = 2,
                Pays = "Italie", Region = "Vénétie", Appellation = "Pinot Grigio delle Venezie DOC", Cepage = "Pinot Gris"
            },
            // id = 3 : Gewurztraminer
            new() {
                Nom = "Gewurztraminer", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 7, NiveauTannins = 1, CoutMoyen = 15.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 8, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 8, ToleranceTemperature = 2,
                Pays = "France", Region = "Alsace", Appellation = "Alsace AOC", Cepage = "Gewurztraminer"
            },
            // id = 4 : Merlot
            new() {
                Nom = "Merlot", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 2, NiveauTannins = 6, CoutMoyen = 11.00m,
                DegreAlcool = 13.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Moyen,
                NiveauFume = 2, NiveauAmertume = 1, TemperatureOptimale = 16, ToleranceTemperature = 2,
                Pays = "France", Region = "Bordeaux", Appellation = "Bordeaux AOC", Cepage = "Merlot"
            },
            // id = 5 : Cabernet Sauvignon
            new() {
                Nom = "Cabernet Sauvignon", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 2, NiveauTannins = 8, CoutMoyen = 14.00m,
                DegreAlcool = 14.0m, IntensiteAromatique = 8, Corps = CorpsBoisson.Corse,
                NiveauFume = 3, NiveauAmertume = 1, TemperatureOptimale = 17, ToleranceTemperature = 2,
                Pays = "France", Region = "Bordeaux", Appellation = "Médoc AOC", Cepage = "Cabernet Sauvignon"
            },
            // id = 6 : Côtes du Rhône
            new() {
                Nom = "Côtes du Rhône", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 3, NiveauTannins = 5, CoutMoyen = 10.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 1, NiveauAmertume = 2, TemperatureOptimale = 16, ToleranceTemperature = 2,
                Pays = "France", Region = "Vallée du Rhône", Appellation = "Côtes du Rhône AOC", Cepage = "Grenache-Syrah-Mourvèdre"
            },
            // id = 7 : Limonade citron-menthe (mocktail, pas d'origine)
            new() {
                Nom = "Limonade citron-menthe", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 8, NiveauSucre = 5, NiveauTannins = 0, CoutMoyen = 3.00m,
                DegreAlcool = 0.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 8 : Virgin Mojito (mocktail, pas d'origine)
            new() {
                Nom = "Virgin Mojito", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 7, NiveauSucre = 6, NiveauTannins = 0, CoutMoyen = 4.00m,
                DegreAlcool = 0.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 9 : Thé glacé pêche (mocktail, pas d'origine)
            new() {
                Nom = "Thé glacé pêche", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 3, NiveauSucre = 6, NiveauTannins = 0, CoutMoyen = 2.50m,
                DegreAlcool = 0.0m, IntensiteAromatique = 4, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 10 : Whisky tourbé (Islay) - pays+region, pas d'appellation ni cépage
            new() {
                Nom = "Whisky tourbé", TypeBoisson = TypeBoisson.Whisky, Alcoolise = true,
                NiveauAcidite = 2, NiveauSucre = 1, NiveauTannins = 3, CoutMoyen = 8.00m,
                DegreAlcool = 46.0m, IntensiteAromatique = 9, Corps = CorpsBoisson.Corse,
                NiveauFume = 9, NiveauAmertume = 1, TemperatureOptimale = 18, ToleranceTemperature = 3,
                Pays = "Royaume-Uni", Region = "Islay (Écosse)"
            },
            // id = 11 : Bière blonde (pays uniquement)
            new() {
                Nom = "Bière blonde", TypeBoisson = TypeBoisson.Biere, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 3, NiveauTannins = 2, CoutMoyen = 3.00m,
                DegreAlcool = 5.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 3, TemperatureOptimale = 6, ToleranceTemperature = 2,
                Pays = "Belgique"
            },
            // id = 12 : Moscato d'Asti
            new() {
                Nom = "Moscato d'Asti", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 8, NiveauTannins = 0, CoutMoyen = 9.00m,
                DegreAlcool = 5.5m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 7, ToleranceTemperature = 2,
                Pays = "Italie", Region = "Piémont", Appellation = "Moscato d'Asti DOCG", Cepage = "Muscat à petits grains"
            },

            // ===== 8 nouvelles boissons V1.2 =====

            // id = 13 : Champagne Brut
            new() {
                Nom = "Champagne Brut", TypeBoisson = TypeBoisson.VinEffervescent, Alcoolise = true,
                NiveauAcidite = 7, NiveauSucre = 2, NiveauTannins = 0, CoutMoyen = 35.00m,
                DegreAlcool = 12.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 7, ToleranceTemperature = 2,
                Pays = "France", Region = "Champagne", Appellation = "Champagne AOC", Cepage = "Chardonnay-Pinot Noir-Pinot Meunier"
            },
            // id = 14 : Porto Tawny 10 ans
            new() {
                Nom = "Porto Tawny 10 ans", TypeBoisson = TypeBoisson.VinDouxNaturel, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 8, NiveauTannins = 3, CoutMoyen = 25.00m,
                DegreAlcool = 20.0m, IntensiteAromatique = 8, Corps = CorpsBoisson.Corse,
                NiveauFume = 1, NiveauAmertume = 2, TemperatureOptimale = 15, ToleranceTemperature = 3,
                Pays = "Portugal", Region = "Vallée du Douro", Appellation = "Porto DOC", Cepage = "Touriga Nacional"
            },
            // id = 15 : Sauternes
            new() {
                Nom = "Sauternes", TypeBoisson = TypeBoisson.VinDouxNaturel, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 9, NiveauTannins = 1, CoutMoyen = 40.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 8, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 9, ToleranceTemperature = 2,
                Pays = "France", Region = "Bordeaux", Appellation = "Sauternes AOC", Cepage = "Sémillon-Sauvignon Blanc"
            },
            // id = 16 : Cidre brut fermier
            new() {
                Nom = "Cidre brut fermier", TypeBoisson = TypeBoisson.Cidre, Alcoolise = true,
                NiveauAcidite = 6, NiveauSucre = 3, NiveauTannins = 1, CoutMoyen = 5.00m,
                DegreAlcool = 5.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 3, TemperatureOptimale = 8, ToleranceTemperature = 2,
                Pays = "France", Region = "Bretagne", Appellation = "Cornouaille AOC"
            },
            // id = 17 : Saké Junmai
            new() {
                Nom = "Saké Junmai", TypeBoisson = TypeBoisson.Sake, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 2, NiveauTannins = 0, CoutMoyen = 20.00m,
                DegreAlcool = 15.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 12, ToleranceTemperature = 2,
                Pays = "Japon", Region = "Niigata", Cepage = "Yamadanishiki"
            },
            // id = 18 : IPA artisanale
            new() {
                Nom = "IPA artisanale", TypeBoisson = TypeBoisson.Biere, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 2, NiveauTannins = 0, CoutMoyen = 4.50m,
                DegreAlcool = 6.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 8, TemperatureOptimale = 8, ToleranceTemperature = 2,
                Pays = "Belgique", Region = "Wallonie"
            },
            // id = 19 : Stout Impérial
            new() {
                Nom = "Stout Impérial", TypeBoisson = TypeBoisson.Biere, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 3, NiveauTannins = 2, CoutMoyen = 6.00m,
                DegreAlcool = 9.0m, IntensiteAromatique = 9, Corps = CorpsBoisson.Corse,
                NiveauFume = 4, NiveauAmertume = 7, TemperatureOptimale = 12, ToleranceTemperature = 3,
                Pays = "Irlande"
            },
            // id = 20 : Negroni (cocktail)
            new() {
                Nom = "Negroni", TypeBoisson = TypeBoisson.Cocktail, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 4, NiveauTannins = 1, CoutMoyen = 9.00m,
                DegreAlcool = 24.0m, IntensiteAromatique = 8, Corps = CorpsBoisson.Corse,
                NiveauFume = 0, NiveauAmertume = 7, TemperatureOptimale = 10, ToleranceTemperature = 2,
                Pays = "Italie", Region = "Florence"
            },

            // ===== 12 nouvelles boissons V1.3 (couverture règles + diversité enum) =====

            // id = 21 : Sancerre - acidité maximale (Loire), cible R14bis et R10
            new() {
                Nom = "Sancerre", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 9, NiveauSucre = 1, NiveauTannins = 0, CoutMoyen = 22.00m,
                DegreAlcool = 12.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 10, ToleranceTemperature = 2,
                Pays = "France", Region = "Loire", Appellation = "Sancerre AOC", Cepage = "Sauvignon Blanc"
            },
            // id = 22 : Riesling d'Alsace - profil épicé, cible R25bis
            new() {
                Nom = "Riesling d'Alsace", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 8, NiveauSucre = 4, NiveauTannins = 0, CoutMoyen = 16.00m,
                DegreAlcool = 12.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 9, ToleranceTemperature = 2,
                Pays = "France", Region = "Alsace", Appellation = "Alsace Grand Cru AOC", Cepage = "Riesling"
            },
            // id = 23 : Côtes de Provence Rosé - comble vin_rose absent du seed
            new() {
                Nom = "Côtes de Provence Rosé", TypeBoisson = TypeBoisson.VinRose, Alcoolise = true,
                NiveauAcidite = 6, NiveauSucre = 2, NiveauTannins = 1, CoutMoyen = 12.00m,
                DegreAlcool = 12.5m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 10, ToleranceTemperature = 2,
                Pays = "France", Region = "Provence", Appellation = "Côtes de Provence AOC", Cepage = "Grenache-Cinsault"
            },
            // id = 24 : Tavel Rosé - rosé corsé, R20bis grillades
            new() {
                Nom = "Tavel Rosé", TypeBoisson = TypeBoisson.VinRose, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 2, NiveauTannins = 3, CoutMoyen = 14.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 12, ToleranceTemperature = 2,
                Pays = "France", Region = "Vallée du Rhône", Appellation = "Tavel AOC", Cepage = "Grenache"
            },
            // id = 25 : Pinot Noir Bourgogne - rouge léger compatible R12 (umami pur)
            new() {
                Nom = "Pinot Noir Bourgogne", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 6, NiveauSucre = 1, NiveauTannins = 4, CoutMoyen = 18.00m,
                DegreAlcool = 13.0m, IntensiteAromatique = 7, Corps = CorpsBoisson.Leger,
                NiveauFume = 1, NiveauAmertume = 2, TemperatureOptimale = 16, ToleranceTemperature = 2,
                Pays = "France", Region = "Bourgogne", Appellation = "Bourgogne AOC", Cepage = "Pinot Noir"
            },
            // id = 26 : Beaujolais Villages - rouge fruité très léger
            new() {
                Nom = "Beaujolais Villages", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 6, NiveauSucre = 2, NiveauTannins = 3, CoutMoyen = 11.00m,
                DegreAlcool = 12.5m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 14, ToleranceTemperature = 2,
                Pays = "France", Region = "Beaujolais", Appellation = "Beaujolais Villages AOC", Cepage = "Gamay"
            },
            // id = 27 : Whisky japonais doux - contraste R13 avec le tourbé d'Islay
            new() {
                Nom = "Whisky japonais doux", TypeBoisson = TypeBoisson.Whisky, Alcoolise = true,
                NiveauAcidite = 2, NiveauSucre = 2, NiveauTannins = 3, CoutMoyen = 12.00m,
                DegreAlcool = 43.0m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 1, NiveauAmertume = 2, TemperatureOptimale = 18, ToleranceTemperature = 3,
                Pays = "Japon", Region = "Yamazaki"
            },
            // id = 28 : Margarita - cocktail acide non-vinicole pour R10
            new() {
                Nom = "Margarita", TypeBoisson = TypeBoisson.Cocktail, Alcoolise = true,
                NiveauAcidite = 8, NiveauSucre = 4, NiveauTannins = 0, CoutMoyen = 9.00m,
                DegreAlcool = 22.0m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 5, ToleranceTemperature = 2,
                Pays = "Mexique"
            },
            // id = 29 : Mocktail gingembre-cardamome - SANS ALCOOL + famille épice (R25bis + R16)
            new() {
                Nom = "Mocktail gingembre-cardamome", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 5, NiveauSucre = 5, NiveauTannins = 0, CoutMoyen = 4.50m,
                DegreAlcool = 0.0m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 30 : Mocktail fruits rouges - sans alcool + sucré pour desserts (R11bis + R16)
            new() {
                Nom = "Mocktail fruits rouges", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 4, NiveauSucre = 7, NiveauTannins = 1, CoutMoyen = 4.00m,
                DegreAlcool = 0.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 31 : Saké Daiginjo - saké premium pour cuisine asiatique raffinée
            new() {
                Nom = "Saké Daiginjo", TypeBoisson = TypeBoisson.Sake, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 3, NiveauTannins = 0, CoutMoyen = 35.00m,
                DegreAlcool = 16.0m, IntensiteAromatique = 7, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 10, ToleranceTemperature = 2,
                Pays = "Japon", Region = "Hyōgo", Cepage = "Yamadanishiki"
            },
            // id = 32 : Cidre doux fermier - sucre=6 pour déclencher R24bis (canard à l'orange, tajine)
            new() {
                Nom = "Cidre doux fermier", TypeBoisson = TypeBoisson.Cidre, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 6, NiveauTannins = 1, CoutMoyen = 5.50m,
                DegreAlcool = 3.5m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 8, ToleranceTemperature = 2,
                Pays = "France", Region = "Normandie", Appellation = "Pays d'Auge AOC"
            }
        };

        await context.Boissons.AddRangeAsync(boissons);
        await context.SaveChangesAsync();

        // V1.4 : association des images après attribution des IDs par la DB.
        // Convention : /assets/boissons/{id_boisson}.jpg (cf. frontend/public/assets/boissons/).
        foreach (var b in boissons)
        {
            b.ImageUrl = $"/assets/boissons/{b.IdBoisson}.jpg";
        }
        await context.SaveChangesAsync();
    }
}