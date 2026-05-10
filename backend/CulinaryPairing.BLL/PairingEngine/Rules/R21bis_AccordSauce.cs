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
        string messageOk;
        string messageKo;

        switch (sauce)
        {
            case TypeSauce.Creme:
                ok = (boisson.TypeBoisson == TypeBoisson.VinBlanc && boisson.NiveauAcidite >= 6)
                     || boisson.TypeBoisson == TypeBoisson.VinEffervescent;
                messageOk = "La sauce crème appelle un vin blanc vif ou effervescent : la boisson convient parfaitement.";
                messageKo = "La sauce crème appelle un vin blanc vif ou effervescent : la boisson n'a pas le bon profil.";
                break;

            case TypeSauce.Tomate:
                ok = boisson.TypeBoisson == TypeBoisson.VinRouge
                     && boisson.NiveauTannins >= 3 && boisson.NiveauTannins <= 6;
                messageOk = "La sauce tomate s'accorde aux vins rouges moyennement tanniques : profil idéal.";
                messageKo = "La sauce tomate s'accorde aux vins rouges moyennement tanniques : profil non respecté.";
                break;

            case TypeSauce.Vin:
                ok = boisson.TypeBoisson == TypeBoisson.VinRouge
                     || boisson.TypeBoisson == TypeBoisson.VinBlanc
                     || boisson.TypeBoisson == TypeBoisson.VinRose;
                messageOk = "La sauce au vin appelle un vin de profil similaire : cohérence cuisine ↔ table respectée.";
                messageKo = "La sauce au vin appelle un vin de profil similaire : cohérence absente.";
                break;

            case TypeSauce.Agrume:
                // Approximation V1.3 : vin blanc acide (faute de table familles aromatiques exploitée ici)
                ok = boisson.TypeBoisson == TypeBoisson.VinBlanc && boisson.NiveauAcidite >= 6;
                messageOk = "La sauce aux agrumes appelle un vin blanc vif : la boisson convient parfaitement.";
                messageKo = "La sauce aux agrumes appelle un vin blanc vif : la boisson n'a pas le bon profil.";
                break;

            case TypeSauce.Beurre:
                ok = boisson.TypeBoisson == TypeBoisson.VinBlanc
                     && (boisson.Corps == CorpsBoisson.Moyen || boisson.Corps == CorpsBoisson.Corse)
                     && boisson.NiveauAcidite >= 5;
                messageOk = "La sauce au beurre s'accorde à un vin blanc gras et soyeux (Chardonnay boisé).";
                messageKo = "La sauce au beurre attend un vin blanc gras (Chardonnay boisé) : profil non respecté.";
                break;

            case TypeSauce.Jus:
                ok = boisson.TypeBoisson == TypeBoisson.VinRouge
                     && (boisson.Corps == CorpsBoisson.Moyen || boisson.Corps == CorpsBoisson.Corse);
                messageOk = "La sauce au jus de viande appelle un vin rouge moyennement corsé : accord respecté.";
                messageKo = "La sauce au jus de viande appelle un vin rouge moyennement corsé : profil non respecté.";
                break;

            default:
                return PairingResult.NonApplicable();
        }

        return ok
            ? PairingResult.Satisfait(messageOk)
            : PairingResult.NonSatisfait(messageKo);
    }
}