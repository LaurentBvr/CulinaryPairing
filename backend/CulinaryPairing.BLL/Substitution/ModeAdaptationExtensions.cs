namespace CulinaryPairing.BLL.Substitution;

/// <summary>
/// Mapping bidirectionnel enum/string pour ModeAdaptation.
/// Convention kebab-case pour les query params API (sans-gluten, sans-lactose)
/// alignée sur les standards REST. Single source of truth pour l'ajout
/// futur de nouveaux modes (R19/R20 etc.).
/// </summary>
public static class ModeAdaptationExtensions
{
    private static readonly Dictionary<ModeAdaptation, string> _toKebab = new()
    {
        { ModeAdaptation.Original, "original" },
        { ModeAdaptation.Vegetarien, "vegetarien" },
        { ModeAdaptation.Vegan, "vegan" },
        { ModeAdaptation.SansGluten, "sans-gluten" },
        { ModeAdaptation.SansLactose, "sans-lactose" }
    };

    private static readonly Dictionary<string, ModeAdaptation> _fromKebab =
        _toKebab.ToDictionary(kv => kv.Value, kv => kv.Key, StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Retourne la représentation kebab-case du mode (ex: SansGluten → "sans-gluten").
    /// </summary>
    public static string ToKebabCase(this ModeAdaptation mode)
        => _toKebab.TryGetValue(mode, out var s) ? s : mode.ToString().ToLowerInvariant();

    /// <summary>
    /// Parse une chaîne kebab-case vers le mode enum. Null/whitespace → Original.
    /// Insensible à la casse. Retourne false si la chaîne ne matche aucun mode connu.
    /// </summary>
    public static bool TryParseMode(string? input, out ModeAdaptation mode)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            mode = ModeAdaptation.Original;
            return true;
        }
        return _fromKebab.TryGetValue(input.Trim(), out mode);
    }

    /// <summary>
    /// Liste des valeurs kebab-case acceptées par TryParseMode.
    /// Utilisé pour les messages d'erreur (400 Bad Request).
    /// </summary>
    public static IReadOnlyCollection<string> AllKebabValues => _toKebab.Values;
}