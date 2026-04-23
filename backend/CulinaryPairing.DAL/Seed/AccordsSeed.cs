using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

public static class AccordsSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Accords.Any()) return;

        var accords = new List<Accord>
        {
            // ===== id 1 : Boeuf Bourguignon =====
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Cabernet Sauvignon, avec ses tanins puissants et ses notes de fruits noirs, s'harmonise parfaitement avec la viande braisée au vin rouge.",
                ScoreCompatibilite = 92,
                IdRecette = 1, IdBoisson = 5
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Les Côtes du Rhône offrent une structure tannique modérée et des arômes d'épices qui complètent la richesse du bourguignon sans l'écraser.",
                ScoreCompatibilite = 85,
                IdRecette = 1, IdBoisson = 6
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Merlot, plus souple, apporte des notes de fruits rouges qui s'accordent bien avec le mijotage long et la sauce au vin.",
                ScoreCompatibilite = 78,
                IdRecette = 1, IdBoisson = 4
            },

            // ===== id 2 : Risotto aux champignons =====
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "L'acidité du Chardonnay équilibre parfaitement le gras du risotto (beurre et parmesan). Les notes beurrées du vin complètent la texture crémeuse.",
                ScoreCompatibilite = 88,
                IdRecette = 2, IdBoisson = 1
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Pinot Grigio, plus léger et vif, apporte fraîcheur et nettoie le palais entre chaque bouchée de ce plat riche en umami.",
                ScoreCompatibilite = 82,
                IdRecette = 2, IdBoisson = 2
            },
            new() {
                TypeAccord = TypeAccord.IA,
                Justification = "Accord suggéré par IA : Le côté minéral du Pinot Grigio s'harmonise avec les notes terreuses des champignons.",
                ScoreCompatibilite = 78,
                IdRecette = 2, IdBoisson = 2
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Alternative sans alcool : La limonade citron-menthe apporte l'acidité nécessaire pour équilibrer le gras, avec une fraîcheur mentholée.",
                ScoreCompatibilite = 75,
                IdRecette = 2, IdBoisson = 7
            },

            // ===== id 3 : Salade César =====
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Pinot Grigio, vif et minéral, tranche avec la richesse de la sauce César et rafraîchit le palais entre chaque bouchée.",
                ScoreCompatibilite = 86,
                IdRecette = 3, IdBoisson = 2
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Champagne Brut, avec son acidité et ses bulles, nettoie le gras de la sauce et s'accorde avec l'umami du parmesan.",
                ScoreCompatibilite = 80,
                IdRecette = 3, IdBoisson = 13
            },

            // ===== id 4 : Velouté de butternut =====
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Gewurztraminer, aromatique et légèrement sucré, s'harmonise avec la douceur naturelle de la courge butternut.",
                ScoreCompatibilite = 89,
                IdRecette = 4, IdBoisson = 3
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Cidre brut fermier, avec son acidité fruitée et ses notes de pomme, complète agréablement la douceur de la courge.",
                ScoreCompatibilite = 76,
                IdRecette = 4, IdBoisson = 16
            },

            // ===== id 5 : Poulet rôti aux herbes =====
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Chardonnay, avec ses notes beurrées et son acidité équilibrée, s'accorde parfaitement avec le poulet rôti et ses herbes aromatiques.",
                ScoreCompatibilite = 87,
                IdRecette = 5, IdBoisson = 1
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Cidre brut fermier est un accord régional classique avec la volaille rôtie, apportant fraîcheur et légèreté.",
                ScoreCompatibilite = 81,
                IdRecette = 5, IdBoisson = 16
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Merlot, aux tanins souples, accompagne bien le poulet sans dominer les arômes de thym et romarin.",
                ScoreCompatibilite = 74,
                IdRecette = 5, IdBoisson = 4
            },

            // ===== id 6 : Panna cotta fruits rouges =====
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Moscato d'Asti, doux et pétillant, s'accorde naturellement avec la fraîcheur des fruits rouges et la crémosité de la panna cotta.",
                ScoreCompatibilite = 91,
                IdRecette = 6, IdBoisson = 12
            },
            new() {
                TypeAccord = TypeAccord.Regle,
                Justification = "Le Sauternes, liquoreux et complexe, apporte une richesse aromatique qui sublime les fruits rouges et la vanille de la panna cotta.",
                ScoreCompatibilite = 84,
                IdRecette = 6, IdBoisson = 15
            }
        };

        await context.Accords.AddRangeAsync(accords);
        await context.SaveChangesAsync();
    }
}