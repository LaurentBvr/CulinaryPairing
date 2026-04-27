using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.BLL.PairingEngine.Rules;

/// <summary>
/// R22bis — Amertume (CdC v1.3 §2.5, principe 8).
/// Modélise les accords avec bières (IPA, stout), cocktails amers (Negroni, Spritz Campari)
/// et vins très tanniques.
/// Applicable : boisson amère (niveau_amertume ≥ 6).
/// Satisfaite : plat compatible :
///   - plats gras/frits (niveau_gras ≥ 6) : l'amertume nettoie le palais
///   - plats umami / charcuteries (contient_umami_pur)
///   - PAS un plat délicat (intensite_aromatique &lt; 4) : l'amertume dominerait.
/// Poids : 15.
/// </summary>
public class R22bis_Amertume : IPairingRule
{
    public string Id => "R22bis";
    public int Poids => 15;

    public PairingResult Evaluer(Recette recette, Boisson boisson)
    {
        if (boisson.NiveauAmertume is null || boisson.NiveauAmertume < 6)
            return PairingResult.NonApplicable();

        // Exclusion : plat délicat (l'amertume dominerait)
        if (recette.IntensiteAromatique is not null && recette.IntensiteAromatique < 4)
            return PairingResult.NonSatisfait(
                $"Boisson amère ({boisson.NiveauAmertume}/10) sur plat délicat "
                + $"(intensité {recette.IntensiteAromatique}) : l'amertume domine.");

        // Bonification : plat gras/frit OU umami pur
        var bonifie = (recette.NiveauGras is not null && recette.NiveauGras >= 6)
                      || recette.ContientUmamiPur;

        return bonifie
            ? PairingResult.Satisfait(
                $"Boisson amère ({boisson.NiveauAmertume}/10) bonifiée par "
                + (recette.ContientUmamiPur
                    ? "umami pur du plat."
                    : $"plat gras ({recette.NiveauGras}/10) : l'amertume nettoie le palais."))
            : PairingResult.NonSatisfait(
                $"Boisson amère ({boisson.NiveauAmertume}/10) sans plat gras "
                + "ni umami pour la valoriser.");
    }
}