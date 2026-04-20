using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 4 comptes de test du CdC section 3.5.1.
// Les passwords sont hashés à la volée avec BCrypt pour que les comptes
// soient réellement utilisables (le CdC contient des faux hashes marqueurs).
public static class UtilisateursSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        // Idempotence : si déjà peuplé, on sort
        if (context.Utilisateurs.Any()) return;

        var utilisateurs = new List<Utilisateur>
        {
            new()
            {
                Nom = "Admin",
                Email = "admin@culinaire.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = RoleUtilisateur.Admin,
                PreferencesAlcool = true,
                RegimeDefaut = RegimeAlimentaire.Omnivore,
                NiveauCuisine = NiveauCuisine.Avance
            },
            new()
            {
                Nom = "Marie Dupont",
                Email = "marie@email.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Marie123!"),
                Role = RoleUtilisateur.Utilisateur,
                PreferencesAlcool = true,
                RegimeDefaut = RegimeAlimentaire.Omnivore,
                NiveauCuisine = NiveauCuisine.Intermediaire
            },
            new()
            {
                Nom = "Thomas Martin",
                Email = "thomas@email.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Thomas123!"),
                Role = RoleUtilisateur.Utilisateur,
                PreferencesAlcool = true,
                RegimeDefaut = RegimeAlimentaire.Omnivore,
                NiveauCuisine = NiveauCuisine.Debutant
            },
            new()
            {
                Nom = "Sophie Bernard",
                Email = "sophie@email.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Sophie123!"),
                Role = RoleUtilisateur.Utilisateur,
                PreferencesAlcool = false,
                RegimeDefaut = RegimeAlimentaire.Vegetarien,
                NiveauCuisine = NiveauCuisine.Avance
            }
        };

        await context.Utilisateurs.AddRangeAsync(utilisateurs);
        await context.SaveChangesAsync();
    }
}