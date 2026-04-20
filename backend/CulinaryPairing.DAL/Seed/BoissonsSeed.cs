using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 20 boissons : 12 de base (CdC 3.5.1) + 8 V1.2 (CdC 3.5.3).
// En SQL le CdC fait INSERT puis UPDATE pour les attributs V1.1/V1.2 ;
// en C# on peut directement créer l'objet complet, c'est plus lisible.
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
                NiveauFume = 1, NiveauAmertume = 1, TemperatureOptimale = 10, ToleranceTemperature = 2
            },
            // id = 2 : Pinot Grigio
            new() {
                Nom = "Pinot Grigio", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 7, NiveauSucre = 2, NiveauTannins = 1, CoutMoyen = 10.00m,
                DegreAlcool = 12.5m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 9, ToleranceTemperature = 2
            },
            // id = 3 : Gewurztraminer
            new() {
                Nom = "Gewurztraminer", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 7, NiveauTannins = 1, CoutMoyen = 15.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 8, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 8, ToleranceTemperature = 2
            },
            // id = 4 : Merlot
            new() {
                Nom = "Merlot", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 2, NiveauTannins = 6, CoutMoyen = 11.00m,
                DegreAlcool = 13.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Moyen,
                NiveauFume = 2, NiveauAmertume = 1, TemperatureOptimale = 16, ToleranceTemperature = 2
            },
            // id = 5 : Cabernet Sauvignon
            new() {
                Nom = "Cabernet Sauvignon", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 2, NiveauTannins = 8, CoutMoyen = 14.00m,
                DegreAlcool = 14.0m, IntensiteAromatique = 8, Corps = CorpsBoisson.Corse,
                NiveauFume = 3, NiveauAmertume = 1, TemperatureOptimale = 17, ToleranceTemperature = 2
            },
            // id = 6 : Côtes du Rhône
            new() {
                Nom = "Côtes du Rhône", TypeBoisson = TypeBoisson.VinRouge, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 3, NiveauTannins = 5, CoutMoyen = 10.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 1, NiveauAmertume = 2, TemperatureOptimale = 16, ToleranceTemperature = 2
            },
            // id = 7 : Limonade citron-menthe
            new() {
                Nom = "Limonade citron-menthe", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 8, NiveauSucre = 5, NiveauTannins = 0, CoutMoyen = 3.00m,
                DegreAlcool = 0.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 8 : Virgin Mojito
            new() {
                Nom = "Virgin Mojito", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 7, NiveauSucre = 6, NiveauTannins = 0, CoutMoyen = 4.00m,
                DegreAlcool = 0.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 9 : Thé glacé pêche
            new() {
                Nom = "Thé glacé pêche", TypeBoisson = TypeBoisson.Mocktail, Alcoolise = false,
                NiveauAcidite = 3, NiveauSucre = 6, NiveauTannins = 0, CoutMoyen = 2.50m,
                DegreAlcool = 0.0m, IntensiteAromatique = 4, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 5, ToleranceTemperature = 2
            },
            // id = 10 : Whisky tourbé (Islay)
            new() {
                Nom = "Whisky tourbé", TypeBoisson = TypeBoisson.Whisky, Alcoolise = true,
                NiveauAcidite = 2, NiveauSucre = 1, NiveauTannins = 3, CoutMoyen = 8.00m,
                DegreAlcool = 46.0m, IntensiteAromatique = 9, Corps = CorpsBoisson.Corse,
                NiveauFume = 9, NiveauAmertume = 1, TemperatureOptimale = 18, ToleranceTemperature = 3
            },
            // id = 11 : Bière blonde
            new() {
                Nom = "Bière blonde", TypeBoisson = TypeBoisson.Biere, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 3, NiveauTannins = 2, CoutMoyen = 3.00m,
                DegreAlcool = 5.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 3, TemperatureOptimale = 6, ToleranceTemperature = 2
            },
            // id = 12 : Moscato d'Asti
            new() {
                Nom = "Moscato d'Asti", TypeBoisson = TypeBoisson.VinBlanc, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 8, NiveauTannins = 0, CoutMoyen = 9.00m,
                DegreAlcool = 5.5m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 7, ToleranceTemperature = 2
            },

            // ===== 8 nouvelles boissons V1.2 =====

            // id = 13 : Champagne Brut
            new() {
                Nom = "Champagne Brut", TypeBoisson = TypeBoisson.VinEffervescent, Alcoolise = true,
                NiveauAcidite = 7, NiveauSucre = 2, NiveauTannins = 0, CoutMoyen = 35.00m,
                DegreAlcool = 12.0m, IntensiteAromatique = 6, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 7, ToleranceTemperature = 2
            },
            // id = 14 : Porto Tawny 10 ans
            new() {
                Nom = "Porto Tawny 10 ans", TypeBoisson = TypeBoisson.VinDouxNaturel, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 8, NiveauTannins = 3, CoutMoyen = 25.00m,
                DegreAlcool = 20.0m, IntensiteAromatique = 8, Corps = CorpsBoisson.Corse,
                NiveauFume = 1, NiveauAmertume = 2, TemperatureOptimale = 15, ToleranceTemperature = 3
            },
            // id = 15 : Sauternes
            new() {
                Nom = "Sauternes", TypeBoisson = TypeBoisson.VinDouxNaturel, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 9, NiveauTannins = 1, CoutMoyen = 40.00m,
                DegreAlcool = 13.5m, IntensiteAromatique = 8, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 1, TemperatureOptimale = 9, ToleranceTemperature = 2
            },
            // id = 16 : Cidre brut fermier
            new() {
                Nom = "Cidre brut fermier", TypeBoisson = TypeBoisson.Cidre, Alcoolise = true,
                NiveauAcidite = 6, NiveauSucre = 3, NiveauTannins = 1, CoutMoyen = 5.00m,
                DegreAlcool = 5.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Leger,
                NiveauFume = 0, NiveauAmertume = 3, TemperatureOptimale = 8, ToleranceTemperature = 2
            },
            // id = 17 : Saké Junmai
            new() {
                Nom = "Saké Junmai", TypeBoisson = TypeBoisson.Sake, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 2, NiveauTannins = 0, CoutMoyen = 20.00m,
                DegreAlcool = 15.0m, IntensiteAromatique = 5, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 2, TemperatureOptimale = 12, ToleranceTemperature = 2
            },
            // id = 18 : IPA artisanale
            new() {
                Nom = "IPA artisanale", TypeBoisson = TypeBoisson.Biere, Alcoolise = true,
                NiveauAcidite = 4, NiveauSucre = 2, NiveauTannins = 0, CoutMoyen = 4.50m,
                DegreAlcool = 6.5m, IntensiteAromatique = 7, Corps = CorpsBoisson.Moyen,
                NiveauFume = 0, NiveauAmertume = 8, TemperatureOptimale = 8, ToleranceTemperature = 2
            },
            // id = 19 : Stout Impérial
            new() {
                Nom = "Stout Impérial", TypeBoisson = TypeBoisson.Biere, Alcoolise = true,
                NiveauAcidite = 3, NiveauSucre = 3, NiveauTannins = 2, CoutMoyen = 6.00m,
                DegreAlcool = 9.0m, IntensiteAromatique = 9, Corps = CorpsBoisson.Corse,
                NiveauFume = 4, NiveauAmertume = 7, TemperatureOptimale = 12, ToleranceTemperature = 3
            },
            // id = 20 : Negroni (cocktail)
            new() {
                Nom = "Negroni", TypeBoisson = TypeBoisson.Cocktail, Alcoolise = true,
                NiveauAcidite = 5, NiveauSucre = 4, NiveauTannins = 1, CoutMoyen = 9.00m,
                DegreAlcool = 24.0m, IntensiteAromatique = 8, Corps = CorpsBoisson.Corse,
                NiveauFume = 0, NiveauAmertume = 7, TemperatureOptimale = 10, ToleranceTemperature = 2
            }
        };

        await context.Boissons.AddRangeAsync(boissons);
        await context.SaveChangesAsync();
    }
}