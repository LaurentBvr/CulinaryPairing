using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

public static class UtilisateursSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.Utilisateurs.Any()) return;

        var utilisateurs = new List<Utilisateur>
        {
            new()
            {
                Prenom = "Admin",
                Nom = "Culinaire",
                Email = "admin@culinaire.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = RoleUtilisateur.Admin,
                PreferencesAlcool = true,
                RegimeDefaut = RegimeAlimentaire.Omnivore,
                NiveauCuisine = NiveauCuisine.Avance
            },
            new()
            {
                Prenom = "Marie",
                Nom = "Dupont",
                Email = "marie@email.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Marie123!"),
                Role = RoleUtilisateur.Utilisateur,
                PreferencesAlcool = true,
                RegimeDefaut = RegimeAlimentaire.Omnivore,
                NiveauCuisine = NiveauCuisine.Intermediaire
            },
            new()
            {
                Prenom = "Thomas",
                Nom = "Martin",
                Email = "thomas@email.com",
                MotDePasse = BCrypt.Net.BCrypt.HashPassword("Thomas123!"),
                Role = RoleUtilisateur.Utilisateur,
                PreferencesAlcool = true,
                RegimeDefaut = RegimeAlimentaire.Omnivore,
                NiveauCuisine = NiveauCuisine.Debutant
            },
            new()
            {
                Prenom = "Sophie",
                Nom = "Bernard",
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