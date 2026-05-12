using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// Étapes de cuisson. Risotto repris littéralement du CdC 3.5.1.
// Les autres recettes sont composées avec des étapes réalistes
// alignées sur le ModeCuisson et le TempsPreparation déclarés dans RecettesSeed.
// V1.3.1 : enrichissement des étapes pour refléter les ingrédients ajoutés
// (carottes/persil Bourguignon, butternut/persil Velouté, herbes Poulet rôti,
// sucre/gélatine/fraises/menthe Panna cotta, anchois/croûtons César,
// moutarde/câpres Tartare, pamplemousse/ciboulette Carpaccio, cebette Pho,
// coriandre Magret, moutarde/ketchup Burger).
public static class EtapesSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Etapes.Any()) return;

        var etapes = new List<Etape>
        {
            // ===== Recette 1 : Boeuf Bourguignon (3h, mijote) — V1.3.1 (carottes + persil) =====
            new() { IdRecette = 1, NumeroEtape = 1, Description = "Couper le boeuf en gros cubes de 4 cm. Éplucher et émincer les oignons, hacher l'ail. Tailler les carottes en rondelles épaisses." },
            new() { IdRecette = 1, NumeroEtape = 2, Description = "Dans une cocotte, faire revenir les lardons dans l'huile d'olive jusqu'à ce qu'ils soient dorés. Réserver." },
            new() { IdRecette = 1, NumeroEtape = 3, Description = "Dans la même cocotte, saisir les morceaux de boeuf sur toutes les faces à feu vif. Saler, poivrer." },
            new() { IdRecette = 1, NumeroEtape = 4, Description = "Ajouter les oignons, l'ail et les carottes. Faire suer 5 minutes jusqu'à translucidité." },
            new() { IdRecette = 1, NumeroEtape = 5, Description = "Mouiller avec le vin rouge jusqu'à couvrir la viande. Porter à ébullition puis baisser le feu." },
            new() { IdRecette = 1, NumeroEtape = 6, Description = "Couvrir et laisser mijoter à feu doux pendant 2h30, en remuant régulièrement." },
            new() { IdRecette = 1, NumeroEtape = 7, Description = "30 minutes avant la fin, ajouter les champignons de Paris émincés et les lardons réservés." },
            new() { IdRecette = 1, NumeroEtape = 8, Description = "Rectifier l'assaisonnement. Parsemer de persil frais ciselé. Servir chaud avec des pommes de terre vapeur ou des pâtes fraîches." },

            // ===== Recette 2 : Risotto aux champignons (30 min, mijote) — CdC 3.5.1 =====
            new() { IdRecette = 2, NumeroEtape = 1, Description = "Émincer finement l'oignon et l'ail. Nettoyer et trancher les champignons." },
            new() { IdRecette = 2, NumeroEtape = 2, Description = "Dans une grande poêle, faire fondre le beurre et faire revenir l'oignon jusqu'à ce qu'il soit translucide." },
            new() { IdRecette = 2, NumeroEtape = 3, Description = "Ajouter les champignons et les faire sauter 5 minutes jusqu'à ce qu'ils soient dorés." },
            new() { IdRecette = 2, NumeroEtape = 4, Description = "Ajouter le riz et le nacrer pendant 2 minutes en remuant." },
            new() { IdRecette = 2, NumeroEtape = 5, Description = "Verser une louche de bouillon de légumes chaud et remuer jusqu'à absorption. Répéter jusqu'à cuisson du riz (environ 18 min)." },
            new() { IdRecette = 2, NumeroEtape = 6, Description = "Hors du feu, incorporer le parmesan râpé. Assaisonner et servir immédiatement." },

            // ===== Recette 3 : Salade César (20 min, cru) — V1.3.1 (anchois + croûtons + vinaigre) =====
            new() { IdRecette = 3, NumeroEtape = 1, Description = "Saler et poivrer les blancs de poulet. Les griller 4 minutes de chaque côté à feu vif. Laisser tiédir puis trancher en lamelles." },
            new() { IdRecette = 3, NumeroEtape = 2, Description = "Préparer la sauce César : dans un bol, écraser les anchois en pâte. Ajouter l'oeuf, le jus de citron, l'ail écrasé et un trait de vinaigre balsamique. Émulsionner avec l'huile d'olive." },
            new() { IdRecette = 3, NumeroEtape = 3, Description = "Ajouter la moitié du parmesan râpé à la sauce. Saler avec parcimonie (les anchois salent déjà), poivrer, mélanger." },
            new() { IdRecette = 3, NumeroEtape = 4, Description = "Disposer la salade dans l'assiette. Ajouter le poulet tiède, napper de sauce, parsemer du reste de parmesan et des croûtons croustillants. Servir aussitôt." },

            // ===== Recette 4 : Velouté de butternut (35 min, mijote) — V1.3.1 (bouillon + persil) =====
            new() { IdRecette = 4, NumeroEtape = 1, Description = "Peler et couper la courge butternut en cubes. Émincer l'oignon, écraser l'ail." },
            new() { IdRecette = 4, NumeroEtape = 2, Description = "Dans une grande casserole, faire fondre le beurre et faire suer l'oignon et l'ail 3 minutes." },
            new() { IdRecette = 4, NumeroEtape = 3, Description = "Ajouter les cubes de butternut. Mouiller à hauteur avec le bouillon de légumes chaud. Saler, poivrer." },
            new() { IdRecette = 4, NumeroEtape = 4, Description = "Porter à ébullition puis laisser mijoter 25 minutes jusqu'à ce que la courge soit tendre." },
            new() { IdRecette = 4, NumeroEtape = 5, Description = "Mixer au blender jusqu'à obtention d'une texture lisse. Incorporer la crème fraîche hors du feu." },
            new() { IdRecette = 4, NumeroEtape = 6, Description = "Rectifier l'assaisonnement. Servir chaud avec un filet d'huile d'olive et une pincée de persil frais ciselé." },

            // ===== Recette 5 : Poulet rôti aux herbes (75 min, roti) — V1.3.1 (thym + romarin) =====
            new() { IdRecette = 5, NumeroEtape = 1, Description = "Préchauffer le four à 200°C (th. 6-7)." },
            new() { IdRecette = 5, NumeroEtape = 2, Description = "Sortir le poulet du réfrigérateur 30 minutes avant cuisson. Le saler et poivrer généreusement à l'intérieur et à l'extérieur." },
            new() { IdRecette = 5, NumeroEtape = 3, Description = "Glisser le beurre ramolli sous la peau du poulet, côté poitrine. Piquer l'ail, le citron coupé en deux et quelques branches de thym et romarin dans la cavité." },
            new() { IdRecette = 5, NumeroEtape = 4, Description = "Badigeonner le poulet d'huile d'olive. Le déposer dans un plat à four, cuisses vers le haut. Parsemer le reste du thym et du romarin sur la peau." },
            new() { IdRecette = 5, NumeroEtape = 5, Description = "Enfourner pour 60 minutes, en arrosant le poulet avec son jus toutes les 20 minutes." },
            new() { IdRecette = 5, NumeroEtape = 6, Description = "Vérifier la cuisson : le jus qui s'écoule de la cuisse doit être clair. Laisser reposer 10 minutes avant de découper." },

            // ===== Recette 6 : Panna cotta fruits rouges (20 min + repos, cru) — V1.3.1 (sucre/gélatine/lait/fraises/menthe) =====
            new() { IdRecette = 6, NumeroEtape = 1, Description = "Faire ramollir la gélatine dans un bol d'eau froide pendant 5 minutes." },
            new() { IdRecette = 6, NumeroEtape = 2, Description = "Dans une casserole, porter la crème fraîche et le lait à frémissement avec le zeste de citron. Ne pas faire bouillir." },
            new() { IdRecette = 6, NumeroEtape = 3, Description = "Hors du feu, incorporer le sucre puis la gélatine essorée. Bien mélanger pour dissoudre complètement." },
            new() { IdRecette = 6, NumeroEtape = 4, Description = "Filtrer la préparation pour retirer le zeste. Répartir dans 4 ramequins. Laisser tiédir à température ambiante puis réfrigérer au moins 4 heures jusqu'à complète prise." },
            new() { IdRecette = 6, NumeroEtape = 5, Description = "Au moment de servir, démouler, napper de coulis de fruits rouges, parsemer de fraises fraîches coupées en quartiers et de feuilles de menthe fraîche." },

            // ===== Recette 7 : Carpaccio de Saint-Jacques aux agrumes (20 min, cru) — V1.3.1 (pamplemousse + ciboulette + crème balsamique) =====
            new() { IdRecette = 7, NumeroEtape = 1, Description = "Sortir les Saint-Jacques au dernier moment. Les trancher très finement (2-3 mm) à l'aide d'un couteau bien affûté, et les disposer en rosace sur des assiettes froides." },
            new() { IdRecette = 7, NumeroEtape = 2, Description = "Prélever les suprêmes du pamplemousse au couteau, en retirant soigneusement les peaux blanches. Réserver au frais." },
            new() { IdRecette = 7, NumeroEtape = 3, Description = "Préparer la marinade : presser le citron vert et l'orange, mélanger les jus avec l'huile d'olive. Saler à la fleur de sel, poivrer." },
            new() { IdRecette = 7, NumeroEtape = 4, Description = "Arroser les Saint-Jacques de marinade 5 minutes avant de servir : la chair va légèrement raffermir et se parfumer." },
            new() { IdRecette = 7, NumeroEtape = 5, Description = "Disposer les suprêmes de pamplemousse autour des Saint-Jacques. Parsemer de ciboulette ciselée et finir par quelques traits de crème balsamique. Servir immédiatement." },

            // ===== Recette 8 : Tartare de boeuf classique (25 min, cru) — V1.3.1 (moutarde + câpres) =====
            new() { IdRecette = 8, NumeroEtape = 1, Description = "Hacher le filet de boeuf au couteau (jamais au mixeur) en petits cubes de 3 mm. Réserver au frais." },
            new() { IdRecette = 8, NumeroEtape = 2, Description = "Émincer finement l'échalote, les cornichons et les câpres. Hacher le persil." },
            new() { IdRecette = 8, NumeroEtape = 3, Description = "Dans un saladier, mélanger le boeuf haché avec l'échalote, les cornichons, les câpres et le persil. Ajouter la moutarde, saler, poivrer." },
            new() { IdRecette = 8, NumeroEtape = 4, Description = "Mouler en cercle dans une assiette, déposer délicatement le jaune d'oeuf au centre. Servir immédiatement avec frites maison ou pain de campagne grillé." },

            // ===== Recette 9 : Soupe Pho au boeuf (4h30, mijote long) — V1.3.1 (cebette) =====
            new() { IdRecette = 9, NumeroEtape = 1, Description = "Faire griller à sec l'oignon coupé en deux, le gingembre, l'anis étoilé et la cannelle dans une poêle jusqu'à ce qu'ils soient légèrement noircis (libère les arômes)." },
            new() { IdRecette = 9, NumeroEtape = 2, Description = "Dans une grande marmite, mettre le boeuf à bouillir, couvrir d'eau froide. Porter à ébullition puis vider l'eau et rincer la viande (dégraissage)." },
            new() { IdRecette = 9, NumeroEtape = 3, Description = "Remettre la viande dans la marmite avec le bouillon de boeuf, les épices grillées, l'oignon. Ajouter la sauce poisson et le sel." },
            new() { IdRecette = 9, NumeroEtape = 4, Description = "Laisser frémir 4 heures à découvert, en écumant régulièrement. Le bouillon doit être limpide et profondément parfumé." },
            new() { IdRecette = 9, NumeroEtape = 5, Description = "Cuire les nouilles de riz selon les instructions du paquet. Trancher le filet de boeuf en lamelles très fines (cru, il cuira au contact du bouillon brûlant). Émincer la cebette en fines rondelles." },
            new() { IdRecette = 9, NumeroEtape = 6, Description = "Dans des bols, disposer les nouilles, les tranches de boeuf cru. Verser le bouillon brûlant filtré. Servir avec coriandre, basilic thaï, cebette ciselée et quartiers de citron vert." },

            // ===== Recette 10 : Magret de canard à l'orange (45 min, roti) — V1.3.1 (coriandre) =====
            new() { IdRecette = 10, NumeroEtape = 1, Description = "Préchauffer le four à 200°C. Quadriller la peau du magret au couteau sans entamer la chair (favorise la fonte du gras)." },
            new() { IdRecette = 10, NumeroEtape = 2, Description = "Saisir les magrets côté peau dans une poêle froide à feu moyen 6-8 minutes : la graisse fond, la peau dore. Saler, poivrer." },
            new() { IdRecette = 10, NumeroEtape = 3, Description = "Retourner les magrets, saisir 2 minutes côté chair, puis enfourner 8 minutes pour une cuisson rosée. Laisser reposer 5 minutes." },
            new() { IdRecette = 10, NumeroEtape = 4, Description = "Pendant ce temps, préparer la sauce : presser 2 oranges, prélever le zeste de la troisième. Faire réduire le jus avec le miel et le vinaigre balsamique jusqu'à consistance sirupeuse." },
            new() { IdRecette = 10, NumeroEtape = 5, Description = "Hors du feu, monter la sauce avec une noisette de beurre froid pour la rendre brillante. Ajouter le zeste." },
            new() { IdRecette = 10, NumeroEtape = 6, Description = "Trancher les magrets en biais, napper de sauce à l'orange, parsemer de coriandre fraîche ciselée. Servir avec une purée de pommes de terre ou des légumes glacés." },

            // ===== Recette 11 : Curry rouge thaï au poulet (45 min, mijote) =====
            new() { IdRecette = 11, NumeroEtape = 1, Description = "Couper le poulet en morceaux de 3 cm. Émincer l'ail, râper le gingembre. Effeuiller le basilic thaï." },
            new() { IdRecette = 11, NumeroEtape = 2, Description = "Dans un wok, faire revenir la pâte de curry rouge avec un peu d'huile pendant 2 minutes pour libérer ses arômes." },
            new() { IdRecette = 11, NumeroEtape = 3, Description = "Ajouter l'ail et le gingembre, puis le poulet. Saisir 5 minutes en remuant régulièrement." },
            new() { IdRecette = 11, NumeroEtape = 4, Description = "Verser le lait de coco, ajouter la sauce poisson et le sucre. Laisser mijoter 25 minutes à feu doux pour que les saveurs se développent." },
            new() { IdRecette = 11, NumeroEtape = 5, Description = "Hors du feu, ajouter le basilic thaï frais et le jus du citron vert. Servir avec du riz thaï parfumé." },

            // ===== Recette 12 : Burger gourmet bacon-cheddar (30 min, grille) — V1.3.1 (moutarde + ketchup) =====
            new() { IdRecette = 12, NumeroEtape = 1, Description = "Sortir le boeuf haché du frigo 15 minutes avant. Le diviser en 4 portions de 150g, façonner des steaks d'1.5 cm d'épaisseur. Saler et poivrer généreusement." },
            new() { IdRecette = 12, NumeroEtape = 2, Description = "Faire revenir les tranches de bacon à sec dans une poêle jusqu'à ce qu'elles soient croustillantes. Réserver sur du papier absorbant." },
            new() { IdRecette = 12, NumeroEtape = 3, Description = "Saisir les steaks dans la même poêle (ou sur grill bien chaud) 3 minutes par face pour une cuisson à point. Déposer une tranche de cheddar sur chaque steak en fin de cuisson pour qu'il fonde." },
            new() { IdRecette = 12, NumeroEtape = 4, Description = "Toaster les pains à burger côté mie. Trancher les tomates, l'oignon en rondelles fines. Détacher les feuilles de salade. Tartiner la base du pain de moutarde et le chapeau de ketchup." },
            new() { IdRecette = 12, NumeroEtape = 5, Description = "Monter le burger dans l'ordre : pain (avec moutarde), salade, tomate, steak au cheddar, bacon, oignon, pain (avec ketchup). Servir immédiatement avec frites maison." },

            // ===== Recette 13 : Tajine d'agneau aux abricots (2h25, mijote long) =====
            new() { IdRecette = 13, NumeroEtape = 1, Description = "Couper l'agneau en cubes de 5 cm. Émincer les oignons, hacher l'ail. Tailler les carottes en rondelles épaisses." },
            new() { IdRecette = 13, NumeroEtape = 2, Description = "Dans un tajine ou une cocotte, faire chauffer l'huile d'olive. Saisir les morceaux d'agneau sur toutes les faces à feu vif. Saler généreusement." },
            new() { IdRecette = 13, NumeroEtape = 3, Description = "Ajouter les oignons et l'ail. Faire suer 5 minutes. Saupoudrer de ras-el-hanout, cumin et cannelle. Mélanger pour enrober la viande." },
            new() { IdRecette = 13, NumeroEtape = 4, Description = "Verser de l'eau à mi-hauteur, couvrir et laisser mijoter 1h30 à feu très doux." },
            new() { IdRecette = 13, NumeroEtape = 5, Description = "Ajouter les carottes, les abricots secs et le miel. Poursuivre la cuisson 30 minutes : la viande doit se défaire à la fourchette." },
            new() { IdRecette = 13, NumeroEtape = 6, Description = "Servir parsemé de coriandre fraîche, accompagné de semoule ou de pain plat." },

            // ===== Recette 14 : Quiche lorraine maison (1h05, four) =====
            new() { IdRecette = 14, NumeroEtape = 1, Description = "Préchauffer le four à 200°C. Étaler la pâte brisée dans un moule à tarte de 26 cm, piquer le fond à la fourchette." },
            new() { IdRecette = 14, NumeroEtape = 2, Description = "Faire revenir les lardons à sec dans une poêle 5 minutes pour qu'ils rendent leur graisse et dorent. Égoutter sur papier absorbant." },
            new() { IdRecette = 14, NumeroEtape = 3, Description = "Battre les oeufs avec la crème fraîche et le lait. Saler modérément (les lardons sont déjà salés), poivrer généreusement." },
            new() { IdRecette = 14, NumeroEtape = 4, Description = "Répartir les lardons sur le fond de tarte. Verser l'appareil aux oeufs par-dessus." },
            new() { IdRecette = 14, NumeroEtape = 5, Description = "Enfourner 35-40 minutes : la quiche doit être dorée et le centre tout juste pris. Laisser tiédir 5 minutes avant de servir." },

            // ===== Recette 15 : Fondant au chocolat noir (27 min, four) =====
            new() { IdRecette = 15, NumeroEtape = 1, Description = "Préchauffer le four à 200°C. Beurrer et fariner 4 ramequins individuels." },
            new() { IdRecette = 15, NumeroEtape = 2, Description = "Faire fondre le chocolat noir avec le beurre au bain-marie ou au micro-ondes par tranches de 30 secondes. Bien lisser." },
            new() { IdRecette = 15, NumeroEtape = 3, Description = "Dans un saladier, fouetter les oeufs avec le sucre jusqu'à ce que le mélange blanchisse. Incorporer le chocolat fondu tiède, puis la farine tamisée." },
            new() { IdRecette = 15, NumeroEtape = 4, Description = "Répartir la pâte dans les ramequins. Enfourner exactement 10-12 minutes : les bords doivent être pris, le centre encore tremblotant." },
            new() { IdRecette = 15, NumeroEtape = 5, Description = "Démouler tiède sur assiette, napper de coulis de fruits rouges. Servir immédiatement (le coeur doit être coulant)." }
        };

        await context.Etapes.AddRangeAsync(etapes);
        await context.SaveChangesAsync();
    }
}