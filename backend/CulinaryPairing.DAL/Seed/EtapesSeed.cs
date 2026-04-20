using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Étapes de cuisson. Risotto repris littéralement du CdC 3.5.1.
// Les 5 autres recettes sont composées avec des étapes réalistes
// alignées sur le ModeCuisson et le TempsPreparation déclarés dans RecettesSeed.
public static class EtapesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Etapes.Any()) return;

        var etapes = new List<Etape>
        {
            // ===== Recette 1 : Boeuf Bourguignon (3h, mijote) =====
            new() { IdRecette = 1, NumeroEtape = 1, Description = "Couper le boeuf en gros cubes de 4 cm. Éplucher et émincer les oignons, hacher l'ail." },
            new() { IdRecette = 1, NumeroEtape = 2, Description = "Dans une cocotte, faire revenir les lardons dans l'huile d'olive jusqu'à ce qu'ils soient dorés. Réserver." },
            new() { IdRecette = 1, NumeroEtape = 3, Description = "Dans la même cocotte, saisir les morceaux de boeuf sur toutes les faces à feu vif. Saler, poivrer." },
            new() { IdRecette = 1, NumeroEtape = 4, Description = "Ajouter les oignons et l'ail. Faire suer 5 minutes jusqu'à translucidité." },
            new() { IdRecette = 1, NumeroEtape = 5, Description = "Mouiller avec le vin rouge jusqu'à couvrir la viande. Porter à ébullition puis baisser le feu." },
            new() { IdRecette = 1, NumeroEtape = 6, Description = "Couvrir et laisser mijoter à feu doux pendant 2h30, en remuant régulièrement." },
            new() { IdRecette = 1, NumeroEtape = 7, Description = "30 minutes avant la fin, ajouter les champignons de Paris émincés et les lardons réservés." },
            new() { IdRecette = 1, NumeroEtape = 8, Description = "Rectifier l'assaisonnement. Servir chaud avec des pommes de terre vapeur ou des pâtes fraîches." },

            // ===== Recette 2 : Risotto aux champignons (30 min, mijote) — CdC 3.5.1 =====
            new() { IdRecette = 2, NumeroEtape = 1, Description = "Émincer finement l'oignon et l'ail. Nettoyer et trancher les champignons." },
            new() { IdRecette = 2, NumeroEtape = 2, Description = "Dans une grande poêle, faire fondre le beurre et faire revenir l'oignon jusqu'à ce qu'il soit translucide." },
            new() { IdRecette = 2, NumeroEtape = 3, Description = "Ajouter les champignons et les faire sauter 5 minutes jusqu'à ce qu'ils soient dorés." },
            new() { IdRecette = 2, NumeroEtape = 4, Description = "Ajouter le riz et le nacrer pendant 2 minutes en remuant." },
            new() { IdRecette = 2, NumeroEtape = 5, Description = "Verser une louche de bouillon chaud et remuer jusqu'à absorption. Répéter jusqu'à cuisson du riz (environ 18 min)." },
            new() { IdRecette = 2, NumeroEtape = 6, Description = "Hors du feu, incorporer le parmesan râpé. Assaisonner et servir immédiatement." },

            // ===== Recette 3 : Salade César (20 min, cru) =====
            new() { IdRecette = 3, NumeroEtape = 1, Description = "Saler et poivrer les blancs de poulet. Les griller 4 minutes de chaque côté à feu vif. Laisser tiédir puis trancher en lamelles." },
            new() { IdRecette = 3, NumeroEtape = 2, Description = "Préparer la sauce : dans un bol, fouetter l'oeuf avec le jus de citron, l'ail écrasé et l'huile d'olive jusqu'à obtenir une émulsion." },
            new() { IdRecette = 3, NumeroEtape = 3, Description = "Ajouter la moitié du parmesan râpé à la sauce. Saler, poivrer, mélanger." },
            new() { IdRecette = 3, NumeroEtape = 4, Description = "Disposer la salade dans l'assiette. Ajouter le poulet tiède, napper de sauce, parsemer du reste de parmesan." },

            // ===== Recette 4 : Velouté de butternut (35 min, mijote) =====
            new() { IdRecette = 4, NumeroEtape = 1, Description = "Peler et couper la courge butternut en cubes. Émincer l'oignon, écraser l'ail." },
            new() { IdRecette = 4, NumeroEtape = 2, Description = "Dans une grande casserole, faire fondre le beurre et faire suer l'oignon et l'ail 3 minutes." },
            new() { IdRecette = 4, NumeroEtape = 3, Description = "Ajouter les cubes de butternut. Couvrir d'eau à hauteur. Saler, poivrer." },
            new() { IdRecette = 4, NumeroEtape = 4, Description = "Porter à ébullition puis laisser mijoter 25 minutes jusqu'à ce que la courge soit tendre." },
            new() { IdRecette = 4, NumeroEtape = 5, Description = "Mixer au blender jusqu'à obtention d'une texture lisse. Incorporer la crème fraîche hors du feu." },
            new() { IdRecette = 4, NumeroEtape = 6, Description = "Rectifier l'assaisonnement. Servir chaud avec un filet d'huile d'olive." },

            // ===== Recette 5 : Poulet rôti aux herbes (75 min, roti) =====
            new() { IdRecette = 5, NumeroEtape = 1, Description = "Préchauffer le four à 200°C (th. 6-7)." },
            new() { IdRecette = 5, NumeroEtape = 2, Description = "Sortir le poulet du réfrigérateur 30 minutes avant cuisson. Le saler et poivrer généreusement à l'intérieur et à l'extérieur." },
            new() { IdRecette = 5, NumeroEtape = 3, Description = "Glisser le beurre ramolli sous la peau du poulet, côté poitrine. Piquer l'ail et le citron coupé en deux dans la cavité." },
            new() { IdRecette = 5, NumeroEtape = 4, Description = "Badigeonner le poulet d'huile d'olive. Le déposer dans un plat à four, cuisses vers le haut." },
            new() { IdRecette = 5, NumeroEtape = 5, Description = "Enfourner pour 60 minutes, en arrosant le poulet avec son jus toutes les 20 minutes." },
            new() { IdRecette = 5, NumeroEtape = 6, Description = "Vérifier la cuisson : le jus qui s'écoule de la cuisse doit être clair. Laisser reposer 10 minutes avant de découper." },

            // ===== Recette 6 : Panna cotta fruits rouges (20 min + repos, cru) =====
            new() { IdRecette = 6, NumeroEtape = 1, Description = "Dans une casserole, porter la crème fraîche à frémissement avec le zeste de citron. Ne pas faire bouillir." },
            new() { IdRecette = 6, NumeroEtape = 2, Description = "Hors du feu, incorporer le sucre et la gélatine préalablement ramollie. Bien mélanger pour dissoudre." },
            new() { IdRecette = 6, NumeroEtape = 3, Description = "Filtrer la préparation pour retirer le zeste. Répartir dans 4 ramequins." },
            new() { IdRecette = 6, NumeroEtape = 4, Description = "Laisser tiédir à température ambiante puis réfrigérer au moins 4 heures jusqu'à complète prise." },
            new() { IdRecette = 6, NumeroEtape = 5, Description = "Au moment de servir, démouler et napper de coulis de fruits rouges." }
        };

        await context.Etapes.AddRangeAsync(etapes);
        await context.SaveChangesAsync();
    }
}