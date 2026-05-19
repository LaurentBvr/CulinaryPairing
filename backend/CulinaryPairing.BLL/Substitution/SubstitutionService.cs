using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.Substitution;

public class SubstitutionService : ISubstitutionService
{
    // Noms exacts des contraintes définis dans ContraintesSeed.cs.
    // Référencés ici pour les modes SansGluten/SansLactose (R17bis/R18bis, V1.4).
    private const string CONTRAINTE_SANS_GLUTEN = "Sans gluten";
    private const string CONTRAINTE_SANS_LACTOSE = "Sans lactose";

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

            // R17/R18 + R17bis/R18bis : détection du besoin de substitution
            // selon le mode demandé.
            // - Vege/Vegan : flags EstVege/EstVegan sur Ingredient (V1.0).
            // - SansGluten/SansLactose : réutilisation de IngredientContrainte
            //   (table V1.3 R9/R16) pour ne pas dupliquer la connaissance domaine.
            //   ⚠️ Prérequis : le caller DOIT inclure ing.Contraintes.Contrainte
            //   via .ThenInclude() pour les modes sans-gluten/sans-lactose.
            bool besoinSub = mode switch
            {
                ModeAdaptation.Vegetarien => !ing.EstVege,
                ModeAdaptation.Vegan => !ing.EstVegan,
                ModeAdaptation.SansGluten => ing.Contraintes
                    .Any(ic => ic.Contrainte?.Nom == CONTRAINTE_SANS_GLUTEN),
                ModeAdaptation.SansLactose => ing.Contraintes
                    .Any(ic => ic.Contrainte?.Nom == CONTRAINTE_SANS_LACTOSE),
                _ => false
            };

            if (besoinSub)
            {
                var typeAttendu = mode switch
                {
                    ModeAdaptation.Vegetarien => TypeSubstitution.Vegetarien,
                    ModeAdaptation.Vegan => TypeSubstitution.Vegan,
                    ModeAdaptation.SansGluten => TypeSubstitution.SansGluten,
                    ModeAdaptation.SansLactose => TypeSubstitution.SansLactose,
                    _ => throw new InvalidOperationException($"Mode non géré : {mode}")
                };

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