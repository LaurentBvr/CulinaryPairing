using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R21bis — Accord avec la sauce (CdC v1.3 §2.5, principe 7).
/// La sauce domine souvent le plat en arômes ; elle est un acteur principal de l'accord.
/// Applicable : type_sauce renseigné ET ≠ Sans.
/// Satisfaite : profil de boisson cohérent avec la sauce (cf. tableau §2.5).
///   - Creme   : vin blanc acide (≥6) ou effervescent
///   - Tomate  : vin rouge moyennement tannique (3-6)
///   - Vin     : vin (cohérence cuisine ↔ table)
///   - Agrume  : boisson à famille aromatique 'agrumes' (V2 — approximation : vin blanc acide)
///   - Beurre  : vin blanc gras (Chardonnay boisé : Corps=Moyen+ ET acidité ≥5)
///   - Jus     : vin rouge moyennement corsé (Corps ∈ {Moyen, Corse})
/// Poids : 15.
/// </summary>
public class R21bis_AccordSauce : IPairingRule
{
    public string Id => "R21bis";
    public int Poids => 15;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.TypeSauce is null || recette.TypeSauce == TypeSauce.Sans)
            return PairingResult.NonApplicable();

        var sauce = recette.TypeSauce.Value;
        bool ok;
        string detail;

        switch (sauce)
        {
            case TypeSauce.Creme:
                ok = (boisson.TypeBoisson == TypeBoisson.VinBlanc && boisson.NiveauAcidite >= 6)
                     || boisson.TypeBoisson == TypeBoisson.VinEffervescent;
                detail = "sauce crème → vin blanc acide ou effervescent";
                break;

            case TypeSauce.Tomate:
                ok = boisson.TypeBoisson == TypeBoisson.VinRouge
                     && boisson.NiveauTannins >= 3 && boisson.NiveauTannins <= 6;
                detail = "sauce tomate → vin rouge moyennement tannique";
                break;

            case TypeSauce.Vin:
                ok = boisson.TypeBoisson == TypeBoisson.VinRouge
                     || boisson.TypeBoisson == TypeBoisson.VinBlanc
                     || boisson.TypeBoisson == TypeBoisson.VinRose;
                detail = "sauce au vin → vin de profil similaire";
                break;

            case TypeSauce.Agrume:
                // Approximation V1.3 : vin blanc acide (faute de table familles aromatiques exploitée ici)
                ok = boisson.TypeBoisson == TypeBoisson.VinBlanc && boisson.NiveauAcidite >= 6;
                detail = "sauce aux agrumes → vin blanc acide";
                break;

            case TypeSauce.Beurre:
                ok = boisson.TypeBoisson == TypeBoisson.VinBlanc
                     && (boisson.Corps == CorpsBoisson.Moyen || boisson.Corps == CorpsBoisson.Corse)
                     && boisson.NiveauAcidite >= 5;
                detail = "sauce beurre → vin blanc gras (Chardonnay boisé)";
                break;

            case TypeSauce.Jus:
                ok = boisson.TypeBoisson == TypeBoisson.VinRouge
                     && (boisson.Corps == CorpsBoisson.Moyen || boisson.Corps == CorpsBoisson.Corse);
                detail = "sauce jus → vin rouge moyennement corsé";
                break;

            default:
                return PairingResult.NonApplicable();
        }

        return ok
            ? PairingResult.Satisfait($"Accord sauce respecté ({detail}).")
            : PairingResult.NonSatisfait($"Accord sauce non respecté ({detail}).");
    }
}