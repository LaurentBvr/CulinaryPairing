using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CulinaryPairing.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BOISSON",
                columns: table => new
                {
                    id_boisson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    type_boisson = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    alcoolise = table.Column<bool>(type: "bit", nullable: false),
                    niveau_acidite = table.Column<int>(type: "int", nullable: true),
                    niveau_sucre = table.Column<int>(type: "int", nullable: true),
                    niveau_tannins = table.Column<int>(type: "int", nullable: true),
                    niveau_amertume = table.Column<int>(type: "int", nullable: true),
                    degre_alcool = table.Column<decimal>(type: "decimal(4,1)", nullable: true),
                    intensite_aromatique = table.Column<int>(type: "int", nullable: true),
                    corps = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    niveau_fume = table.Column<int>(type: "int", nullable: true),
                    temperature_optimale = table.Column<int>(type: "int", nullable: true),
                    tolerance_temperature = table.Column<int>(type: "int", nullable: false),
                    cout_moyen = table.Column<decimal>(type: "decimal(8,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOISSON", x => x.id_boisson);
                });

            migrationBuilder.CreateTable(
                name: "CONTRAINTE_ALIMENTAIRE",
                columns: table => new
                {
                    id_contrainte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CONTRAINTE_ALIMENTAIRE", x => x.id_contrainte);
                });

            migrationBuilder.CreateTable(
                name: "FAMILLE_AROMATIQUE",
                columns: table => new
                {
                    id_famille = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAMILLE_AROMATIQUE", x => x.id_famille);
                });

            migrationBuilder.CreateTable(
                name: "INGREDIENT",
                columns: table => new
                {
                    id_ingredient = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    unite_defaut = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    est_allergene = table.Column<bool>(type: "bit", nullable: false),
                    est_alcool = table.Column<bool>(type: "bit", nullable: false),
                    est_vege = table.Column<bool>(type: "bit", nullable: false),
                    est_vegan = table.Column<bool>(type: "bit", nullable: false),
                    divisible = table.Column<bool>(type: "bit", nullable: false),
                    cout_unitaire = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    date_maj_prix = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INGREDIENT", x => x.id_ingredient);
                });

            migrationBuilder.CreateTable(
                name: "UTILISATEUR",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    mot_de_passe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    preferences_alcool = table.Column<bool>(type: "bit", nullable: false),
                    regime_defaut = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    niveau_cuisine = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UTILISATEUR", x => x.id_utilisateur);
                });

            migrationBuilder.CreateTable(
                name: "BOISSON_FAMILLE_AROMATIQUE",
                columns: table => new
                {
                    id_boisson = table.Column<int>(type: "int", nullable: false),
                    id_famille = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BOISSON_FAMILLE_AROMATIQUE", x => new { x.id_boisson, x.id_famille });
                    table.ForeignKey(
                        name: "FK_BOISSON_FAMILLE_AROMATIQUE_BOISSON_id_boisson",
                        column: x => x.id_boisson,
                        principalTable: "BOISSON",
                        principalColumn: "id_boisson",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BOISSON_FAMILLE_AROMATIQUE_FAMILLE_AROMATIQUE_id_famille",
                        column: x => x.id_famille,
                        principalTable: "FAMILLE_AROMATIQUE",
                        principalColumn: "id_famille",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SUBSTITUTION_INGREDIENT",
                columns: table => new
                {
                    id_substitution = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_ingredient_original = table.Column<int>(type: "int", nullable: false),
                    id_ingredient_substitut = table.Column<int>(type: "int", nullable: false),
                    type_substitution = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ratio_conversion = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    note_cuisson = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SUBSTITUTION_INGREDIENT", x => x.id_substitution);
                    table.ForeignKey(
                        name: "FK_SUBSTITUTION_INGREDIENT_INGREDIENT_id_ingredient_original",
                        column: x => x.id_ingredient_original,
                        principalTable: "INGREDIENT",
                        principalColumn: "id_ingredient",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SUBSTITUTION_INGREDIENT_INGREDIENT_id_ingredient_substitut",
                        column: x => x.id_ingredient_substitut,
                        principalTable: "INGREDIENT",
                        principalColumn: "id_ingredient",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RECETTE",
                columns: table => new
                {
                    id_recette = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titre = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    temps_preparation = table.Column<int>(type: "int", nullable: true),
                    difficulte = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    type_plat = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    nombre_personnes_base = table.Column<int>(type: "int", nullable: true),
                    niveau_gras = table.Column<int>(type: "int", nullable: true),
                    niveau_acidite = table.Column<int>(type: "int", nullable: true),
                    niveau_piquant = table.Column<int>(type: "int", nullable: true),
                    niveau_arome_epice = table.Column<int>(type: "int", nullable: true),
                    niveau_umami = table.Column<int>(type: "int", nullable: true),
                    niveau_sucre = table.Column<int>(type: "int", nullable: true),
                    niveau_sel = table.Column<int>(type: "int", nullable: true),
                    intensite_aromatique = table.Column<int>(type: "int", nullable: true),
                    contient_umami_pur = table.Column<bool>(type: "bit", nullable: false),
                    contient_fume = table.Column<bool>(type: "bit", nullable: false),
                    affinite_tannins = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    mode_cuisson = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    type_sauce = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    est_publiee = table.Column<bool>(type: "bit", nullable: false),
                    adaptable_vege = table.Column<bool>(type: "bit", nullable: false),
                    adaptable_vegan = table.Column<bool>(type: "bit", nullable: false),
                    cout_estime = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    preparable_avance = table.Column<bool>(type: "bit", nullable: false),
                    temps_finition = table.Column<int>(type: "int", nullable: true),
                    date_creation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    id_utilisateur = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECETTE", x => x.id_recette);
                    table.ForeignKey(
                        name: "FK_RECETTE_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SCORE_QUIZ",
                columns: table => new
                {
                    id_score = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    date_quiz = table.Column<DateTime>(type: "datetime2", nullable: false),
                    niveau = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    score_obtenu = table.Column<int>(type: "int", nullable: true),
                    nombre_questions = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SCORE_QUIZ", x => x.id_score);
                    table.ForeignKey(
                        name: "FK_SCORE_QUIZ_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOIREE",
                columns: table => new
                {
                    id_soiree = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    nombre_personnes = table.Column<int>(type: "int", nullable: true),
                    nombre_vegetariens = table.Column<int>(type: "int", nullable: false),
                    nombre_vegans = table.Column<int>(type: "int", nullable: false),
                    budget = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    temps_disponible = table.Column<int>(type: "int", nullable: true),
                    type_soiree = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    preference_alcool = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    date_creation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOIREE", x => x.id_soiree);
                    table.ForeignKey(
                        name: "FK_SOIREE_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UTILISATEUR_CONTRAINTE",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    id_contrainte = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UTILISATEUR_CONTRAINTE", x => new { x.id_utilisateur, x.id_contrainte });
                    table.ForeignKey(
                        name: "FK_UTILISATEUR_CONTRAINTE_CONTRAINTE_ALIMENTAIRE_id_contrainte",
                        column: x => x.id_contrainte,
                        principalTable: "CONTRAINTE_ALIMENTAIRE",
                        principalColumn: "id_contrainte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UTILISATEUR_CONTRAINTE_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ACCORD",
                columns: table => new
                {
                    id_accord = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_accord = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    justification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    score_compatibilite = table.Column<int>(type: "int", nullable: true),
                    niveau_confiance = table.Column<int>(type: "int", nullable: true),
                    regles_satisfaites = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    malus_applique = table.Column<int>(type: "int", nullable: true),
                    id_recette = table.Column<int>(type: "int", nullable: false),
                    id_boisson = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ACCORD", x => x.id_accord);
                    table.ForeignKey(
                        name: "FK_ACCORD_BOISSON_id_boisson",
                        column: x => x.id_boisson,
                        principalTable: "BOISSON",
                        principalColumn: "id_boisson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ACCORD_RECETTE_id_recette",
                        column: x => x.id_recette,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ETAPE",
                columns: table => new
                {
                    id_etape = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    numero_etape = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_recette = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETAPE", x => x.id_etape);
                    table.ForeignKey(
                        name: "FK_ETAPE_RECETTE_id_recette",
                        column: x => x.id_recette,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FAVORI",
                columns: table => new
                {
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    id_recette = table.Column<int>(type: "int", nullable: false),
                    date_ajout = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAVORI", x => new { x.id_utilisateur, x.id_recette });
                    table.ForeignKey(
                        name: "FK_FAVORI_RECETTE_id_recette",
                        column: x => x.id_recette,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FAVORI_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HISTORIQUE_CONSULTATION",
                columns: table => new
                {
                    id_historique = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_utilisateur = table.Column<int>(type: "int", nullable: false),
                    id_recette = table.Column<int>(type: "int", nullable: false),
                    date_consultation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HISTORIQUE_CONSULTATION", x => x.id_historique);
                    table.ForeignKey(
                        name: "FK_HISTORIQUE_CONSULTATION_RECETTE_id_recette",
                        column: x => x.id_recette,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HISTORIQUE_CONSULTATION_UTILISATEUR_id_utilisateur",
                        column: x => x.id_utilisateur,
                        principalTable: "UTILISATEUR",
                        principalColumn: "id_utilisateur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QUESTION_QUIZ",
                columns: table => new
                {
                    id_question = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    texte_question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    difficulte = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    explication = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    id_recette_exemple = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QUESTION_QUIZ", x => x.id_question);
                    table.ForeignKey(
                        name: "FK_QUESTION_QUIZ_RECETTE_id_recette_exemple",
                        column: x => x.id_recette_exemple,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RECETTE_FAMILLE_AROMATIQUE",
                columns: table => new
                {
                    id_recette = table.Column<int>(type: "int", nullable: false),
                    id_famille = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECETTE_FAMILLE_AROMATIQUE", x => new { x.id_recette, x.id_famille });
                    table.ForeignKey(
                        name: "FK_RECETTE_FAMILLE_AROMATIQUE_FAMILLE_AROMATIQUE_id_famille",
                        column: x => x.id_famille,
                        principalTable: "FAMILLE_AROMATIQUE",
                        principalColumn: "id_famille",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RECETTE_FAMILLE_AROMATIQUE_RECETTE_id_recette",
                        column: x => x.id_recette,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RECETTE_INGREDIENT",
                columns: table => new
                {
                    id_recette = table.Column<int>(type: "int", nullable: false),
                    id_ingredient = table.Column<int>(type: "int", nullable: false),
                    quantite = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RECETTE_INGREDIENT", x => new { x.id_recette, x.id_ingredient });
                    table.ForeignKey(
                        name: "FK_RECETTE_INGREDIENT_INGREDIENT_id_ingredient",
                        column: x => x.id_ingredient,
                        principalTable: "INGREDIENT",
                        principalColumn: "id_ingredient",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RECETTE_INGREDIENT_RECETTE_id_recette",
                        column: x => x.id_recette,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MENU_SOIREE",
                columns: table => new
                {
                    id_menu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_soiree = table.Column<int>(type: "int", nullable: false),
                    id_recette_entree = table.Column<int>(type: "int", nullable: true),
                    id_recette_plat = table.Column<int>(type: "int", nullable: true),
                    id_recette_dessert = table.Column<int>(type: "int", nullable: true),
                    id_boisson_entree = table.Column<int>(type: "int", nullable: true),
                    id_boisson_plat = table.Column<int>(type: "int", nullable: true),
                    id_boisson_dessert = table.Column<int>(type: "int", nullable: true),
                    cout_total_estime = table.Column<decimal>(type: "decimal(8,2)", nullable: true),
                    temps_total_estime = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MENU_SOIREE", x => x.id_menu);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_BOISSON_id_boisson_dessert",
                        column: x => x.id_boisson_dessert,
                        principalTable: "BOISSON",
                        principalColumn: "id_boisson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_BOISSON_id_boisson_entree",
                        column: x => x.id_boisson_entree,
                        principalTable: "BOISSON",
                        principalColumn: "id_boisson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_BOISSON_id_boisson_plat",
                        column: x => x.id_boisson_plat,
                        principalTable: "BOISSON",
                        principalColumn: "id_boisson",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_RECETTE_id_recette_dessert",
                        column: x => x.id_recette_dessert,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_RECETTE_id_recette_entree",
                        column: x => x.id_recette_entree,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_RECETTE_id_recette_plat",
                        column: x => x.id_recette_plat,
                        principalTable: "RECETTE",
                        principalColumn: "id_recette",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MENU_SOIREE_SOIREE_id_soiree",
                        column: x => x.id_soiree,
                        principalTable: "SOIREE",
                        principalColumn: "id_soiree",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SOIREE_CONTRAINTE",
                columns: table => new
                {
                    id_soiree = table.Column<int>(type: "int", nullable: false),
                    id_contrainte = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SOIREE_CONTRAINTE", x => new { x.id_soiree, x.id_contrainte });
                    table.ForeignKey(
                        name: "FK_SOIREE_CONTRAINTE_CONTRAINTE_ALIMENTAIRE_id_contrainte",
                        column: x => x.id_contrainte,
                        principalTable: "CONTRAINTE_ALIMENTAIRE",
                        principalColumn: "id_contrainte",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SOIREE_CONTRAINTE_SOIREE_id_soiree",
                        column: x => x.id_soiree,
                        principalTable: "SOIREE",
                        principalColumn: "id_soiree",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "REPONSE_QUIZ",
                columns: table => new
                {
                    id_reponse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_question = table.Column<int>(type: "int", nullable: false),
                    texte_reponse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    est_correcte = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_REPONSE_QUIZ", x => x.id_reponse);
                    table.ForeignKey(
                        name: "FK_REPONSE_QUIZ_QUESTION_QUIZ_id_question",
                        column: x => x.id_question,
                        principalTable: "QUESTION_QUIZ",
                        principalColumn: "id_question",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ACCORD_id_boisson",
                table: "ACCORD",
                column: "id_boisson");

            migrationBuilder.CreateIndex(
                name: "IX_ACCORD_id_recette",
                table: "ACCORD",
                column: "id_recette");

            migrationBuilder.CreateIndex(
                name: "IX_BOISSON_FAMILLE_AROMATIQUE_id_famille",
                table: "BOISSON_FAMILLE_AROMATIQUE",
                column: "id_famille");

            migrationBuilder.CreateIndex(
                name: "IX_CONTRAINTE_ALIMENTAIRE_nom",
                table: "CONTRAINTE_ALIMENTAIRE",
                column: "nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ETAPE_id_recette_numero_etape",
                table: "ETAPE",
                columns: new[] { "id_recette", "numero_etape" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FAMILLE_AROMATIQUE_nom",
                table: "FAMILLE_AROMATIQUE",
                column: "nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FAVORI_id_recette",
                table: "FAVORI",
                column: "id_recette");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIQUE_CONSULTATION_id_recette",
                table: "HISTORIQUE_CONSULTATION",
                column: "id_recette");

            migrationBuilder.CreateIndex(
                name: "IX_HISTORIQUE_CONSULTATION_id_utilisateur",
                table: "HISTORIQUE_CONSULTATION",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_INGREDIENT_nom",
                table: "INGREDIENT",
                column: "nom",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_boisson_dessert",
                table: "MENU_SOIREE",
                column: "id_boisson_dessert");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_boisson_entree",
                table: "MENU_SOIREE",
                column: "id_boisson_entree");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_boisson_plat",
                table: "MENU_SOIREE",
                column: "id_boisson_plat");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_recette_dessert",
                table: "MENU_SOIREE",
                column: "id_recette_dessert");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_recette_entree",
                table: "MENU_SOIREE",
                column: "id_recette_entree");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_recette_plat",
                table: "MENU_SOIREE",
                column: "id_recette_plat");

            migrationBuilder.CreateIndex(
                name: "IX_MENU_SOIREE_id_soiree",
                table: "MENU_SOIREE",
                column: "id_soiree");

            migrationBuilder.CreateIndex(
                name: "IX_QUESTION_QUIZ_id_recette_exemple",
                table: "QUESTION_QUIZ",
                column: "id_recette_exemple");

            migrationBuilder.CreateIndex(
                name: "IX_RECETTE_id_utilisateur",
                table: "RECETTE",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_RECETTE_FAMILLE_AROMATIQUE_id_famille",
                table: "RECETTE_FAMILLE_AROMATIQUE",
                column: "id_famille");

            migrationBuilder.CreateIndex(
                name: "IX_RECETTE_INGREDIENT_id_ingredient",
                table: "RECETTE_INGREDIENT",
                column: "id_ingredient");

            migrationBuilder.CreateIndex(
                name: "IX_REPONSE_QUIZ_id_question",
                table: "REPONSE_QUIZ",
                column: "id_question");

            migrationBuilder.CreateIndex(
                name: "IX_SCORE_QUIZ_id_utilisateur",
                table: "SCORE_QUIZ",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_SOIREE_id_utilisateur",
                table: "SOIREE",
                column: "id_utilisateur");

            migrationBuilder.CreateIndex(
                name: "IX_SOIREE_CONTRAINTE_id_contrainte",
                table: "SOIREE_CONTRAINTE",
                column: "id_contrainte");

            migrationBuilder.CreateIndex(
                name: "IX_SUBSTITUTION_INGREDIENT_id_ingredient_original",
                table: "SUBSTITUTION_INGREDIENT",
                column: "id_ingredient_original");

            migrationBuilder.CreateIndex(
                name: "IX_SUBSTITUTION_INGREDIENT_id_ingredient_substitut",
                table: "SUBSTITUTION_INGREDIENT",
                column: "id_ingredient_substitut");

            migrationBuilder.CreateIndex(
                name: "IX_UTILISATEUR_email",
                table: "UTILISATEUR",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UTILISATEUR_CONTRAINTE_id_contrainte",
                table: "UTILISATEUR_CONTRAINTE",
                column: "id_contrainte");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ACCORD");

            migrationBuilder.DropTable(
                name: "BOISSON_FAMILLE_AROMATIQUE");

            migrationBuilder.DropTable(
                name: "ETAPE");

            migrationBuilder.DropTable(
                name: "FAVORI");

            migrationBuilder.DropTable(
                name: "HISTORIQUE_CONSULTATION");

            migrationBuilder.DropTable(
                name: "MENU_SOIREE");

            migrationBuilder.DropTable(
                name: "RECETTE_FAMILLE_AROMATIQUE");

            migrationBuilder.DropTable(
                name: "RECETTE_INGREDIENT");

            migrationBuilder.DropTable(
                name: "REPONSE_QUIZ");

            migrationBuilder.DropTable(
                name: "SCORE_QUIZ");

            migrationBuilder.DropTable(
                name: "SOIREE_CONTRAINTE");

            migrationBuilder.DropTable(
                name: "SUBSTITUTION_INGREDIENT");

            migrationBuilder.DropTable(
                name: "UTILISATEUR_CONTRAINTE");

            migrationBuilder.DropTable(
                name: "BOISSON");

            migrationBuilder.DropTable(
                name: "FAMILLE_AROMATIQUE");

            migrationBuilder.DropTable(
                name: "QUESTION_QUIZ");

            migrationBuilder.DropTable(
                name: "SOIREE");

            migrationBuilder.DropTable(
                name: "INGREDIENT");

            migrationBuilder.DropTable(
                name: "CONTRAINTE_ALIMENTAIRE");

            migrationBuilder.DropTable(
                name: "RECETTE");

            migrationBuilder.DropTable(
                name: "UTILISATEUR");
        }
    }
}
