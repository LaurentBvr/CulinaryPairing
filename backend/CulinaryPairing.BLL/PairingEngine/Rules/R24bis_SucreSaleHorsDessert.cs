using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R24bis — Sucré-salé hors dessert (CdC v1.3 §2.5, principe 3) — NOUVEAU V1.3.
/// Applicable : plat.niveau_sucre ≥ 5 ET type_plat ≠ dessert
///              (canard à l'orange, porc au caramel, pad thaï, etc.).
/// Satisfaite : boisson à douceur résiduelle (niveau_sucre ≥ 4).
/// Justification : étend la logique de R11bis aux plats principaux sucré-salés.
/// Poids : 15.
/// </summary>
public class R24bis_SucreSaleHorsDessert : IPairingRule
{
    public string Id => "R24bis";
    public int Poids => 15;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (recette.NiveauSucre is null || boisson.NiveauSucre is null)
            return PairingResult.NonApplicable();

        // Hors profil : plat pas sucré-salé OU c'est un dessert (couvert par R11bis)
        if (recette.NiveauSucre < 5 || recette.TypePlat == TypePlat.Dessert)
            return PairingResult.NonApplicable();

        return boisson.NiveauSucre >= 4
            ? PairingResult.Satisfait(
                "Le caractère sucré-salé du plat (canard à l'orange, porc au caramel…) est respecté par une boisson à douceur résiduelle.")
            : PairingResult.NonSatisfait(
                "Le plat sucré-salé attend une boisson avec douceur, mais celle-ci paraîtra trop sèche en bouche.");
    }
}