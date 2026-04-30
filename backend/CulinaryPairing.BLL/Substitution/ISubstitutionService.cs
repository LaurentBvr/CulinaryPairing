using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.Substitution;

public interface ISubstitutionService
{
    RecetteAdapteeDto AdapterRecette(
        IEnumerable<RecetteIngredient> ingredients,
        IEnumerable<SubstitutionIngredient> substitutionsDisponibles,
        ModeAdaptation mode);
}