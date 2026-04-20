using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 4 accords exemples du CdC 3.5.1, tous sur le Risotto aux champignons (id 2).
// Illustrent la cohabitation entre accords générés par règles (type 'regle')
// et accords suggérés par IA (type 'ia'). En production, ces accords seraient
// générés dynamiquement par le moteur (BLL) ; ici on seed des valeurs de
// référence pour pouvoir démontrer l'affichage sans avoir à déployer le moteur.
public static class AccordsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Accords.Any()) return;

        var accords = new List<Accord>
        {
            // Risotto + Chardonnay : accord de référence (score 88)
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "L'acidité du Chardonnay équilibre parfaitement le gras du risotto (beurre et parmesan). Les notes beurrées du vin complètent la texture crémeuse.",
                ScoreCompatibilite = 88,
                IdRecette = 2,
                IdBoisson = 1   // Chardonnay
            },

            // Risotto + Pinot Grigio : alternative plus légère (score 82)
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Pinot Grigio, plus léger et vif, apporte fraîcheur et nettoie le palais entre chaque bouchée de ce plat riche en umami.",
                ScoreCompatibilite = 82,
                IdRecette = 2,
                IdBoisson = 2   // Pinot Grigio
            },

            // Risotto + Pinot Grigio (second accord, type IA) : illustre la cohabitation
            new() {
                TypeAccord = TypeAccord.IA,
                Justification = "Accord suggéré par IA : Le côté minéral du Pinot Grigio s'harmonise avec les notes terreuses des champignons.",
                ScoreCompatibilite = 78,
                IdRecette = 2,
                IdBoisson = 2   // Pinot Grigio
            },

            // Risotto + Limonade citron-menthe : alternative sans alcool (score 75)
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Alternative sans alcool : La limonade citron-menthe apporte l'acidité nécessaire pour équilibrer le gras, avec une fraîcheur mentholée.",
                ScoreCompatibilite = 75,
                IdRecette = 2,
                IdBoisson = 7   // Limonade citron-menthe
            }
        };

        await context.Accords.AddRangeAsync(accords);
        await context.SaveChangesAsync();
    }
}