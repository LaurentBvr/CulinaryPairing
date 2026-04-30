using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.Substitution;

public class SubstitutionService : ISubstitutionService
{
    public RecetteAdapteeDto AdapterRecette(
        IEnumerable<RecetteIngredient> ingredients,
        IEnumerable<SubstitutionIngredient> substitutionsDisponibles,
        ModeAdaptation mode)
    {
        var result = new RecetteAdapteeDto { Mode = mode };
        var subs = substitutionsDisponibles.ToList();

        foreach (var ri in ingredients)
        {
            var ing = ri.Ingredient;
            var dto = new IngredientAdapteDto
            {
                IdIngredient = ing.IdIngredient,
                Nom = ing.Nom,
                Quantite = ri.Quantite,
                Unite = ing.UniteDefaut,
                EstVege = ing.EstVege,
                EstVegan = ing.EstVegan
            };

            // R17/R18 : substitution requise uniquement si l'ingrédient enfreint le mode demandé
            bool besoinSub = mode switch
            {
                ModeAdaptation.Vegetarien => !ing.EstVege,
                ModeAdaptation.Vegan => !ing.EstVegan,
                _ => false
            };

            if (besoinSub)
            {
                var typeAttendu = mode == ModeAdaptation.Vegan
                    ? TypeSubstitution.Vegan
                    : TypeSubstitution.Vegetarien;

                var sub = subs.FirstOrDefault(s =>
                    s.IdIngredientOriginal == ing.IdIngredient
                    && s.TypeSubstitution == typeAttendu);

                if (sub != null)
                {
                    dto.Substitut = new SubstitutDto
                    {
                        IdIngredient = sub.IdIngredientSubstitut,
                        Nom = sub.IngredientSubstitut.Nom,
                        QuantiteAdaptee = ri.Quantite * sub.RatioConversion,
                        Unite = sub.IngredientSubstitut.UniteDefaut,
                        NoteCuisson = sub.NoteCuisson
                    };
                }
                else
                {
                    result.IngredientsSansSubstitution.Add(ing.Nom);
                }
            }

            result.Ingredients.Add(dto);
        }

        return result;
    }
}