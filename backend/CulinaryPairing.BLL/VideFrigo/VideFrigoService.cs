using CulinaryPairing.DAL;
using CulinaryPairing.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.BLL.VideFrigo;

public class VideFrigoService(CulinaryPairingDbContext db)
{
    public async Task<List<VideFrigoResultDto>> RechercherAsync(VideFrigoRequestDto req)
    {
        var recettes = await db.Recettes
            .Where(r => r.Statut == StatutRecette.Publiee)
            .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                    .ThenInclude(i => i.SubstitutionsOriginales)
            .ToListAsync();

        var results = new List<VideFrigoResultDto>();

        foreach (var recette in recettes)
        {
            var total = recette.Ingredients.Count;
            if (total == 0) continue;

           var presents = recette.Ingredients
                .Where(ri => req.IngredientIds.Contains(ri.IdIngredient))
                .Select(ri => ri.Ingredient.Nom)
                .ToList();

            var manquants = recette.Ingredients
                .Where(ri => !req.IngredientIds.Contains(ri.IdIngredient))
                .Select(ri => ri.Ingredient.Nom)
                .ToList();

            var disponibles = presents.Count;

            // Au moins un ingrédient utilisateur doit être présent
            if (disponibles == 0) continue;

            var score = (int)Math.Round((double)disponibles / total * 100);
            var badgeVeg = recette.Ingredients.Any(ri => ri.Ingredient.SubstitutionsOriginales.Any());

            results.Add(new VideFrigoResultDto
            {
                RecetteId = recette.IdRecette,
                Titre = recette.Titre,
                Score = score,
                IngredientsPresents = presents,
                IngredientsManquants = manquants,
                BadgeVeg = badgeVeg
            });
        }

        return results
            .OrderByDescending(r => r.Score)
            .Take(req.NombreResultats)
            .ToList();
    }
}