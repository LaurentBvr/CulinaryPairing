using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Associations entre boissons/recettes et familles aromatiques (CdC 3.5.4).
// Ces associations sont essentielles pour les règles R19bis (similitude
// aromatique) et R25bis (arômes épicés via famille 'epice').
//
// IDs des familles : 1=agrumes, 2=fruits_rouges, 3=fruits_noirs,
// 4=fruits_blancs, 5=fruits_exotiques, 6=floral, 7=boise, 8=fume,
// 9=epice, 10=mineral, 11=vegetal, 12=herbace, 13=beurre, 14=torrefie.
public static class AssociationsAromatiquesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        // Idempotence : si l'une des deux tables est déjà peuplée, on quitte
        if (context.BoissonsFamillesAromatiques.Any() ||
            context.RecettesFamillesAromatiques.Any()) return;

        // ===== BOISSONS (32 boissons -> ~80 associations) =====
        var boissonsAssociations = new List<BoissonFamilleAromatique>
        {
            // id 1 : Chardonnay -> beurre, boisé, fruits blancs
            new() { IdBoisson = 1,  IdFamille = 13 },  // beurre
            new() { IdBoisson = 1,  IdFamille = 7  },  // boise
            new() { IdBoisson = 1,  IdFamille = 4  },  // fruits_blancs

            // id 2 : Pinot Grigio -> agrumes, fruits blancs, mineral
            new() { IdBoisson = 2,  IdFamille = 1  },  // agrumes
            new() { IdBoisson = 2,  IdFamille = 4  },  // fruits_blancs
            new() { IdBoisson = 2,  IdFamille = 10 },  // mineral

            // id 3 : Gewurztraminer -> floral, fruits exotiques, épice
            new() { IdBoisson = 3,  IdFamille = 6  },  // floral
            new() { IdBoisson = 3,  IdFamille = 5  },  // fruits_exotiques
            new() { IdBoisson = 3,  IdFamille = 9  },  // epice (R25bis !)

            // id 4 : Merlot -> fruits rouges, fruits noirs, boisé
            new() { IdBoisson = 4,  IdFamille = 2  },  // fruits_rouges
            new() { IdBoisson = 4,  IdFamille = 3  },  // fruits_noirs
            new() { IdBoisson = 4,  IdFamille = 7  },  // boise

            // id 5 : Cabernet Sauvignon -> fruits noirs, boisé, épice
            new() { IdBoisson = 5,  IdFamille = 3  },  // fruits_noirs
            new() { IdBoisson = 5,  IdFamille = 7  },  // boise
            new() { IdBoisson = 5,  IdFamille = 9  },  // epice

            // id 6 : Côtes du Rhône -> fruits rouges, épice, herbacé
            new() { IdBoisson = 6,  IdFamille = 2  },  // fruits_rouges
            new() { IdBoisson = 6,  IdFamille = 9  },  // epice (vins du Rhône, syrah)
            new() { IdBoisson = 6,  IdFamille = 12 },  // herbace (garrigue)

            // id 7 : Limonade citron-menthe -> agrumes, herbacé
            new() { IdBoisson = 7,  IdFamille = 1  },  // agrumes
            new() { IdBoisson = 7,  IdFamille = 12 },  // herbace

            // id 8 : Virgin Mojito -> agrumes, herbacé
            new() { IdBoisson = 8,  IdFamille = 1  },  // agrumes
            new() { IdBoisson = 8,  IdFamille = 12 },  // herbace

            // id 9 : Thé glacé pêche -> fruits blancs
            new() { IdBoisson = 9,  IdFamille = 4  },  // fruits_blancs

            // id 10 : Whisky tourbé (Islay) -> fumé, boisé
            new() { IdBoisson = 10, IdFamille = 8  },  // fume
            new() { IdBoisson = 10, IdFamille = 7  },  // boise

            // id 11 : Bière blonde -> végétal (houblon léger)
            new() { IdBoisson = 11, IdFamille = 11 },  // vegetal

            // id 12 : Moscato d'Asti -> floral, fruits blancs
            new() { IdBoisson = 12, IdFamille = 6  },  // floral
            new() { IdBoisson = 12, IdFamille = 4  },  // fruits_blancs

            // id 13 : Champagne Brut -> agrumes, beurre, mineral
            new() { IdBoisson = 13, IdFamille = 1  },  // agrumes
            new() { IdBoisson = 13, IdFamille = 13 },  // beurre (autolyse, brioche)
            new() { IdBoisson = 13, IdFamille = 10 },  // mineral

            // id 14 : Porto Tawny 10 ans -> fruits noirs, torréfié, boisé
            new() { IdBoisson = 14, IdFamille = 3  },  // fruits_noirs
            new() { IdBoisson = 14, IdFamille = 14 },  // torrefie (caramel, café)
            new() { IdBoisson = 14, IdFamille = 7  },  // boise

            // id 15 : Sauternes -> floral, fruits exotiques, beurre (botrytis)
            new() { IdBoisson = 15, IdFamille = 6  },  // floral
            new() { IdBoisson = 15, IdFamille = 5  },  // fruits_exotiques
            new() { IdBoisson = 15, IdFamille = 13 },  // beurre

            // id 16 : Cidre brut fermier -> fruits blancs (pomme)
            new() { IdBoisson = 16, IdFamille = 4  },  // fruits_blancs

            // id 17 : Saké Junmai -> mineral, fruits blancs
            new() { IdBoisson = 17, IdFamille = 10 },  // mineral
            new() { IdBoisson = 17, IdFamille = 4  },  // fruits_blancs

            // id 18 : IPA artisanale -> agrumes (houblons US), végétal
            new() { IdBoisson = 18, IdFamille = 1  },  // agrumes
            new() { IdBoisson = 18, IdFamille = 11 },  // vegetal

            // id 19 : Stout Impérial -> torréfié, fumé
            new() { IdBoisson = 19, IdFamille = 14 },  // torrefie (chocolat, café)
            new() { IdBoisson = 19, IdFamille = 8  },  // fume

            // id 20 : Negroni -> agrumes, herbacé (Campari)
            new() { IdBoisson = 20, IdFamille = 1  },  // agrumes
            new() { IdBoisson = 20, IdFamille = 12 },  // herbace

            // ===== Nouvelles boissons V1.3 (id 21-32) =====

            // id 21 : Sancerre -> agrumes, mineral, herbacé
            new() { IdBoisson = 21, IdFamille = 1  },  // agrumes
            new() { IdBoisson = 21, IdFamille = 10 },  // mineral
            new() { IdBoisson = 21, IdFamille = 12 },  // herbace (bourgeon de cassis)

            // id 22 : Riesling d'Alsace -> agrumes, mineral, épice
            new() { IdBoisson = 22, IdFamille = 1  },  // agrumes
            new() { IdBoisson = 22, IdFamille = 10 },  // mineral
            new() { IdBoisson = 22, IdFamille = 9  },  // epice (R25bis !)

            // id 23 : Côtes de Provence Rosé -> fruits rouges, floral
            new() { IdBoisson = 23, IdFamille = 2  },  // fruits_rouges
            new() { IdBoisson = 23, IdFamille = 6  },  // floral

            // id 24 : Tavel Rosé -> fruits rouges, épice
            new() { IdBoisson = 24, IdFamille = 2  },  // fruits_rouges
            new() { IdBoisson = 24, IdFamille = 9  },  // epice

            // id 25 : Pinot Noir Bourgogne -> fruits rouges, boisé, floral
            new() { IdBoisson = 25, IdFamille = 2  },  // fruits_rouges
            new() { IdBoisson = 25, IdFamille = 7  },  // boise
            new() { IdBoisson = 25, IdFamille = 6  },  // floral

            // id 26 : Beaujolais Villages -> fruits rouges, floral
            new() { IdBoisson = 26, IdFamille = 2  },  // fruits_rouges
            new() { IdBoisson = 26, IdFamille = 6  },  // floral

            // id 27 : Whisky japonais doux -> floral, boisé, fruits blancs
            new() { IdBoisson = 27, IdFamille = 6  },  // floral
            new() { IdBoisson = 27, IdFamille = 7  },  // boise
            new() { IdBoisson = 27, IdFamille = 4  },  // fruits_blancs

            // id 28 : Margarita -> agrumes
            new() { IdBoisson = 28, IdFamille = 1  },  // agrumes

            // id 29 : Mocktail gingembre-cardamome -> épice (R25bis sans alcool !)
            new() { IdBoisson = 29, IdFamille = 9  },  // epice

            // id 30 : Mocktail fruits rouges -> fruits rouges
            new() { IdBoisson = 30, IdFamille = 2  },  // fruits_rouges

            // id 31 : Saké Daiginjo -> floral, fruits exotiques
            new() { IdBoisson = 31, IdFamille = 6  },  // floral
            new() { IdBoisson = 31, IdFamille = 5  },  // fruits_exotiques

            // id 32 : Cidre doux fermier -> fruits blancs, floral
            new() { IdBoisson = 32, IdFamille = 4  },  // fruits_blancs
            new() { IdBoisson = 32, IdFamille = 6  }   // floral
        };

        // ===== RECETTES (15 recettes -> ~40 associations) =====
        var recettesAssociations = new List<RecetteFamilleAromatique>
        {
            // id 1 : Boeuf Bourguignon -> fruits noirs (vin), boisé, herbacé
            new() { IdRecette = 1, IdFamille = 3  },  // fruits_noirs
            new() { IdRecette = 1, IdFamille = 7  },  // boise (mijote longue)
            new() { IdRecette = 1, IdFamille = 12 },  // herbace (bouquet garni)

            // id 2 : Risotto aux champignons -> boisé, herbacé, beurre
            new() { IdRecette = 2, IdFamille = 7  },  // boise (champignons)
            new() { IdRecette = 2, IdFamille = 12 },  // herbace
            new() { IdRecette = 2, IdFamille = 13 },  // beurre

            // id 3 : Salade César -> agrumes, herbacé
            new() { IdRecette = 3, IdFamille = 1  },  // agrumes (citron sauce)
            new() { IdRecette = 3, IdFamille = 12 },  // herbace (persil)

            // id 4 : Velouté de butternut -> beurre, fruits blancs
            new() { IdRecette = 4, IdFamille = 13 },  // beurre (crème)
            new() { IdRecette = 4, IdFamille = 4  },  // fruits_blancs (courge sucrée)

            // id 5 : Poulet rôti aux herbes -> herbacé, beurre
            new() { IdRecette = 5, IdFamille = 12 },  // herbace (thym, romarin)
            new() { IdRecette = 5, IdFamille = 13 },  // beurre

            // id 6 : Panna cotta fruits rouges -> fruits rouges, beurre, agrumes
            new() { IdRecette = 6, IdFamille = 2  },  // fruits_rouges
            new() { IdRecette = 6, IdFamille = 13 },  // beurre (crème)
            new() { IdRecette = 6, IdFamille = 1  },  // agrumes (zeste citron)

            // id 7 : Carpaccio Saint-Jacques -> agrumes, mineral, herbacé
            new() { IdRecette = 7, IdFamille = 1  },  // agrumes (citron vert, orange)
            new() { IdRecette = 7, IdFamille = 10 },  // mineral (Saint-Jacques iodées)
            new() { IdRecette = 7, IdFamille = 12 },  // herbace (coriandre)

            // id 8 : Tartare de boeuf -> herbacé
            new() { IdRecette = 8, IdFamille = 12 },  // herbace (persil, échalote)

            // id 9 : Soupe Pho au boeuf -> épice, herbacé
            new() { IdRecette = 9, IdFamille = 9  },  // epice (anis, cannelle, R25bis)
            new() { IdRecette = 9, IdFamille = 12 },  // herbace (basilic thaï, coriandre)

            // id 10 : Magret de canard à l'orange -> agrumes, fruits noirs
            new() { IdRecette = 10, IdFamille = 1 },  // agrumes (R19bis avec sauces agrumes)
            new() { IdRecette = 10, IdFamille = 3 },  // fruits_noirs (caramélisation)

            // id 11 : Curry rouge thaï -> épice, herbacé, fruits exotiques
            new() { IdRecette = 11, IdFamille = 9  },  // epice (R25bis !)
            new() { IdRecette = 11, IdFamille = 12 },  // herbace (basilic thaï, coriandre)
            new() { IdRecette = 11, IdFamille = 5  },  // fruits_exotiques (coco, citron vert)

            // id 12 : Burger gourmet bacon-cheddar -> fumé, torréfié
            new() { IdRecette = 12, IdFamille = 8  },  // fume (R13 bacon)
            new() { IdRecette = 12, IdFamille = 14 },  // torrefie (Maillard du steak)

            // id 13 : Tajine d'agneau aux abricots -> épice, fruits exotiques
            new() { IdRecette = 13, IdFamille = 9  },  // epice (R25bis ras-el-hanout)
            new() { IdRecette = 13, IdFamille = 5  },  // fruits_exotiques (abricots secs, miel)

            // id 14 : Quiche lorraine -> fumé, beurre
            new() { IdRecette = 14, IdFamille = 8  },  // fume (R13 lardons fumés)
            new() { IdRecette = 14, IdFamille = 13 },  // beurre (pâte brisée + appareil crème)

            // id 15 : Fondant au chocolat noir -> torréfié, fruits rouges
            new() { IdRecette = 15, IdFamille = 14 },  // torrefie (chocolat noir, cacao)
            new() { IdRecette = 15, IdFamille = 2  }   // fruits_rouges (coulis)
        };

        await context.BoissonsFamillesAromatiques.AddRangeAsync(boissonsAssociations);
        await context.RecettesFamillesAromatiques.AddRangeAsync(recettesAssociations);
        await context.SaveChangesAsync();
    }
}