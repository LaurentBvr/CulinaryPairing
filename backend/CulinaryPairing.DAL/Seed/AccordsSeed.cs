namespace CulinaryPairing.DAL.Seed;

/// <summary>
/// SeedAsync est volontairement vide depuis la création du moteur d'accords
/// Le moteur d'accords (BLL/PairingEngine) calcule les accords à la demande
/// et les persiste dans la table ACCORD avec version_moteur='1.3'.
/// Single source of truth = le moteur, pas le seed.
/// </summary>
public static class AccordsSeed
{
    public static Task SeedAsync(CulinaryPairingDbContext context)
    {
        return Task.CompletedTask;
    }
}