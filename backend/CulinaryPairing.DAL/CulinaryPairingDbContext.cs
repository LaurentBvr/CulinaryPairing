using CulinaryPairing.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CulinaryPairing.DAL;

public class CulinaryPairingDbContext : DbContext
{
    public CulinaryPairingDbContext(DbContextOptions<CulinaryPairingDbContext> options)
        : base(options) { }

    // ===== Utilisateurs & Contraintes =====
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
    public DbSet<ContrainteAlimentaire> ContraintesAlimentaires => Set<ContrainteAlimentaire>();
    public DbSet<UtilisateurContrainte> UtilisateursContraintes => Set<UtilisateurContrainte>();

    // ===== Recettes & Ingrédients =====
    public DbSet<Recette> Recettes => Set<Recette>();
    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<IngredientContrainte> IngredientsContraintes => Set<IngredientContrainte>();
    public DbSet<RecetteIngredient> RecettesIngredients => Set<RecetteIngredient>();
    public DbSet<Etape> Etapes => Set<Etape>();
    public DbSet<SubstitutionIngredient> SubstitutionsIngredients => Set<SubstitutionIngredient>();

    // ===== Boissons & Accords =====
    public DbSet<Boisson> Boissons => Set<Boisson>();
    public DbSet<Accord> Accords => Set<Accord>();

    // ===== Familles aromatiques =====
    public DbSet<FamilleAromatique> FamillesAromatiques => Set<FamilleAromatique>();
    public DbSet<RecetteFamilleAromatique> RecettesFamillesAromatiques => Set<RecetteFamilleAromatique>();
    public DbSet<BoissonFamilleAromatique> BoissonsFamillesAromatiques => Set<BoissonFamilleAromatique>();

    // ===== Quiz =====
    public DbSet<QuestionQuiz> QuestionsQuiz => Set<QuestionQuiz>();
    public DbSet<ReponseQuiz> ReponsesQuiz => Set<ReponseQuiz>();
    public DbSet<ScoreQuiz> ScoresQuiz => Set<ScoreQuiz>();

    // ===== Soirée =====
    public DbSet<Soiree> Soirees => Set<Soiree>();
    public DbSet<SoireeContrainte> SoireesContraintes => Set<SoireeContrainte>();
    public DbSet<MenuSoiree> MenusSoiree => Set<MenuSoiree>();

    // ===== Favoris & Historique =====
    public DbSet<Favori> Favoris => Set<Favori>();
    public DbSet<HistoriqueConsultation> HistoriquesConsultations => Set<HistoriqueConsultation>();
    public DbSet<FavoriMenu> FavorisMenus => Set<FavoriMenu>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ========== ENUMS → STOCKÉS EN STRING ==========
        modelBuilder.Entity<Utilisateur>().Property(u => u.Role).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Utilisateur>().Property(u => u.RegimeDefaut).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Utilisateur>().Property(u => u.NiveauCuisine).HasConversion<string>().HasMaxLength(20);

        modelBuilder.Entity<ContrainteAlimentaire>().Property(c => c.Type).HasConversion<string>().HasMaxLength(20);

        modelBuilder.Entity<Recette>().Property(r => r.Difficulte).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Recette>().Property(r => r.TypePlat).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Recette>().Property(r => r.AffiniteTannins).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Recette>().Property(r => r.ModeCuisson).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Recette>().Property(r => r.TypeSauce).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Recette>().Property(r => r.Statut).HasConversion<string>().HasMaxLength(20);

        modelBuilder.Entity<SubstitutionIngredient>().Property(s => s.TypeSubstitution).HasConversion<string>().HasMaxLength(20);

        modelBuilder.Entity<Boisson>().Property(b => b.TypeBoisson).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Boisson>().Property(b => b.Corps).HasConversion<string>().HasMaxLength(20);

        modelBuilder.Entity<Accord>().Property(a => a.TypeAccord).HasConversion<string>().HasMaxLength(10);

        modelBuilder.Entity<QuestionQuiz>().Property(q => q.Difficulte).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<ScoreQuiz>().Property(s => s.Niveau).HasConversion<string>().HasMaxLength(20);

        modelBuilder.Entity<Soiree>().Property(s => s.TypeSoiree).HasConversion<string>().HasMaxLength(20);
        modelBuilder.Entity<Soiree>().Property(s => s.PreferenceAlcool).HasConversion<string>().HasMaxLength(10);

        // ========== INDEX UNIQUES ==========
        modelBuilder.Entity<Utilisateur>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<ContrainteAlimentaire>().HasIndex(c => c.Nom).IsUnique();
        modelBuilder.Entity<Ingredient>().HasIndex(i => i.Nom).IsUnique();
        modelBuilder.Entity<FamilleAromatique>().HasIndex(f => f.Nom).IsUnique();

        // ========== CLÉS COMPOSITES ==========
        modelBuilder.Entity<UtilisateurContrainte>().HasKey(uc => new { uc.IdUtilisateur, uc.IdContrainte });
        modelBuilder.Entity<IngredientContrainte>().HasKey(ic => new { ic.IdIngredient, ic.IdContrainte });
        modelBuilder.Entity<RecetteIngredient>().HasKey(ri => new { ri.IdRecette, ri.IdIngredient });
        modelBuilder.Entity<RecetteFamilleAromatique>().HasKey(rf => new { rf.IdRecette, rf.IdFamille });
        modelBuilder.Entity<BoissonFamilleAromatique>().HasKey(bf => new { bf.IdBoisson, bf.IdFamille });
        modelBuilder.Entity<SoireeContrainte>().HasKey(sc => new { sc.IdSoiree, sc.IdContrainte });
        modelBuilder.Entity<Favori>().HasKey(f => new { f.IdUtilisateur, f.IdRecette });
        modelBuilder.Entity<FavoriMenu>().HasKey(fm => new { fm.IdUtilisateur, fm.IdMenu });

        // ========== RELATIONS EXPLICITES (HasForeignKey → pas de shadow) ==========

        // ----- Utilisateur -----
        modelBuilder.Entity<Utilisateur>()
            .Property(u => u.EstActif)
            .HasDefaultValue(true);
        modelBuilder.Entity<Recette>()
            .HasOne(r => r.Utilisateur)
            .WithMany(u => u.RecettesCreees)
            .HasForeignKey(r => r.IdUtilisateur)
            .OnDelete(DeleteBehavior.Restrict);

        // ----- UtilisateurContrainte (M:N) -----
        modelBuilder.Entity<UtilisateurContrainte>()
            .HasOne(uc => uc.Utilisateur)
            .WithMany(u => u.Contraintes)
            .HasForeignKey(uc => uc.IdUtilisateur)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UtilisateurContrainte>()
            .HasOne(uc => uc.Contrainte)
            .WithMany(c => c.Utilisateurs)
            .HasForeignKey(uc => uc.IdContrainte)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- RecetteIngredient (M:N) -----
        modelBuilder.Entity<RecetteIngredient>()
            .HasOne(ri => ri.Recette)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(ri => ri.IdRecette)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecetteIngredient>()
            .HasOne(ri => ri.Ingredient)
            .WithMany(i => i.Recettes)
            .HasForeignKey(ri => ri.IdIngredient)
            .OnDelete(DeleteBehavior.Restrict);

        // ----- Etape -----
        modelBuilder.Entity<Etape>()
            .HasOne(e => e.Recette)
            .WithMany(r => r.Etapes)
            .HasForeignKey(e => e.IdRecette)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Etape>().HasIndex(e => new { e.IdRecette, e.NumeroEtape }).IsUnique();

        // ----- SubstitutionIngredient (2 FK vers Ingredient) -----
        modelBuilder.Entity<SubstitutionIngredient>()
            .HasOne(s => s.IngredientOriginal)
            .WithMany(i => i.SubstitutionsOriginales)
            .HasForeignKey(s => s.IdIngredientOriginal)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SubstitutionIngredient>()
            .HasOne(s => s.IngredientSubstitut)
            .WithMany(i => i.SubstitutionsSubstituts)
            .HasForeignKey(s => s.IdIngredientSubstitut)
            .OnDelete(DeleteBehavior.Restrict);

        // ----- IngredientContrainte (M:N) -----
        modelBuilder.Entity<IngredientContrainte>()
            .HasOne(ic => ic.Ingredient)
            .WithMany(i => i.Contraintes)
            .HasForeignKey(ic => ic.IdIngredient)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IngredientContrainte>()
            .HasOne(ic => ic.Contrainte)
            .WithMany(c => c.Ingredients)
            .HasForeignKey(ic => ic.IdContrainte)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- Accord -----
        modelBuilder.Entity<Accord>()
            .HasOne(a => a.Recette)
            .WithMany(r => r.Accords)
            .HasForeignKey(a => a.IdRecette)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Accord>()
            .HasOne(a => a.Boisson)
            .WithMany(b => b.Accords)
            .HasForeignKey(a => a.IdBoisson)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Accord>()
            .HasIndex(a => new { a.IdRecette, a.IdBoisson, a.TypeAccord })
            .IsUnique();

        // ----- Familles aromatiques (M:N) -----
        modelBuilder.Entity<RecetteFamilleAromatique>()
            .HasOne(rf => rf.Recette)
            .WithMany(r => r.FamillesAromatiques)
            .HasForeignKey(rf => rf.IdRecette)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RecetteFamilleAromatique>()
            .HasOne(rf => rf.Famille)
            .WithMany(f => f.Recettes)
            .HasForeignKey(rf => rf.IdFamille)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoissonFamilleAromatique>()
            .HasOne(bf => bf.Boisson)
            .WithMany(b => b.FamillesAromatiques)
            .HasForeignKey(bf => bf.IdBoisson)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BoissonFamilleAromatique>()
            .HasOne(bf => bf.Famille)
            .WithMany(f => f.Boissons)
            .HasForeignKey(bf => bf.IdFamille)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- Quiz -----
        modelBuilder.Entity<QuestionQuiz>()
            .HasOne(q => q.RecetteExemple)
            .WithMany()
            .HasForeignKey(q => q.IdRecetteExemple)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ReponseQuiz>()
            .HasOne(r => r.Question)
            .WithMany(q => q.Reponses)
            .HasForeignKey(r => r.IdQuestion)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ScoreQuiz>()
            .HasOne(s => s.Utilisateur)
            .WithMany(u => u.Scores)
            .HasForeignKey(s => s.IdUtilisateur)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- Soiree -----
        modelBuilder.Entity<Soiree>()
            .HasOne(s => s.Utilisateur)
            .WithMany(u => u.Soirees)
            .HasForeignKey(s => s.IdUtilisateur)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SoireeContrainte>()
            .HasOne(sc => sc.Soiree)
            .WithMany(s => s.Contraintes)
            .HasForeignKey(sc => sc.IdSoiree)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SoireeContrainte>()
            .HasOne(sc => sc.Contrainte)
            .WithMany(c => c.Soirees)
            .HasForeignKey(sc => sc.IdContrainte)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- MenuSoiree (1 FK Soiree + 3 FK Recette + 3 FK Boisson) -----
        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.Soiree)
            .WithMany(s => s.Menus)
            .HasForeignKey(m => m.IdSoiree)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.RecetteEntree)
            .WithMany()
            .HasForeignKey(m => m.IdRecetteEntree)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.RecettePlat)
            .WithMany()
            .HasForeignKey(m => m.IdRecettePlat)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.RecetteDessert)
            .WithMany()
            .HasForeignKey(m => m.IdRecetteDessert)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.BoissonEntree)
            .WithMany()
            .HasForeignKey(m => m.IdBoissonEntree)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.BoissonPlat)
            .WithMany()
            .HasForeignKey(m => m.IdBoissonPlat)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuSoiree>()
            .HasOne(m => m.BoissonDessert)
            .WithMany()
            .HasForeignKey(m => m.IdBoissonDessert)
            .OnDelete(DeleteBehavior.Restrict);

        // ----- Favori -----
        modelBuilder.Entity<Favori>()
            .HasOne(f => f.Utilisateur)
            .WithMany(u => u.Favoris)
            .HasForeignKey(f => f.IdUtilisateur)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favori>()
            .HasOne(f => f.Recette)
            .WithMany(r => r.Favoris)
            .HasForeignKey(f => f.IdRecette)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- FavoriMenu -----
        modelBuilder.Entity<FavoriMenu>()
            .HasOne(fm => fm.Utilisateur)
            .WithMany(u => u.FavorisMenus)
            .HasForeignKey(fm => fm.IdUtilisateur)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FavoriMenu>()
            .HasOne(fm => fm.Menu)
            .WithMany(m => m.Favoris)
            .HasForeignKey(fm => fm.IdMenu)
            .OnDelete(DeleteBehavior.Cascade);

        // ----- HistoriqueConsultation -----
        modelBuilder.Entity<HistoriqueConsultation>()
            .HasOne(h => h.Utilisateur)
            .WithMany(u => u.Historique)
            .HasForeignKey(h => h.IdUtilisateur)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<HistoriqueConsultation>()
            .HasOne(h => h.Recette)
            .WithMany(r => r.Historiques)
            .HasForeignKey(h => h.IdRecette)
            .OnDelete(DeleteBehavior.Restrict);
    }
}