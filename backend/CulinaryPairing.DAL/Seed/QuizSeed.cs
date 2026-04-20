using CulinaryPairing.Entities.Enums;
using CulinaryPairing.Entities.Models;

namespace CulinaryPairing.DAL.Seed;

// 5 questions de quiz (CdC 3.5.1) + leurs 4 réponses chacune.
// Le CdC détaille les réponses des questions 1 et 2 ; les réponses
// des questions 3, 4, 5 sont composées ici selon le même principe
// (4 propositions, 1 seule correcte, explication déjà fournie par la question).
public static class QuizSeed
{
    public static async Task SeedAsync(CulinaryPairingDbContext context)
    {
        if (context.QuestionsQuiz.Any()) return;

        // ===== 1. Les 5 questions (CdC 3.5.1) =====
        var questions = new List<QuestionQuiz>
        {
            // id = 1 : débutant, lié au Risotto (id recette = 2)
            new() {
                TexteQuestion = "Un risotto crémeux au parmesan. Quel type de vin privilégier ?",
                Difficulte = DifficulteQuiz.Debutant,
                Explication = "Le risotto est un plat gras (beurre, parmesan, crème). Il nécessite un vin blanc avec de l'acidité pour nettoyer le palais et équilibrer le gras. Le Chardonnay ou le Pinot Grigio sont parfaits.",
                IdRecetteExemple = 2
            },
            // id = 2 : débutant, sans recette associée
            new() {
                TexteQuestion = "Un plat très épicé comme un curry thaï. Quelle boisson éviter absolument ?",
                Difficulte = DifficulteQuiz.Debutant,
                Explication = "L'alcool fort et les tannins amplifient la sensation de brûlure des plats épicés. Il faut privilégier un vin doux (Gewurztraminer) ou un mocktail sucré qui tempère le piquant.",
                IdRecetteExemple = null
            },
            // id = 3 : intermédiaire
            new() {
                TexteQuestion = "Un steak grillé au barbecue avec des notes fumées. Quel accord classique ?",
                Difficulte = DifficulteQuiz.Intermediaire,
                Explication = "Les notes fumées de la viande grillée s'accordent parfaitement avec les tannins d'un vin rouge (Cabernet, Merlot) ou les notes tourbées d'un whisky.",
                IdRecetteExemple = null
            },
            // id = 4 : expert, lié au Risotto
            new() {
                TexteQuestion = "Pourquoi éviter un vin rouge tannique avec un risotto aux champignons ?",
                Difficulte = DifficulteQuiz.Expert,
                Explication = "Les champignons sont très riches en umami (saveur savoureuse). Quand l'umami rencontre les tannins du vin rouge, cela crée une amertume désagréable en bouche. Mieux vaut un vin blanc.",
                IdRecetteExemple = 2
            },
            // id = 5 : débutant
            new() {
                TexteQuestion = "Quel est le principe de base de l'accord gras/acide ?",
                Difficulte = DifficulteQuiz.Debutant,
                Explication = "Un plat gras (crème, beurre, fromage) appelle une boisson acide qui va couper le gras et rafraîchir le palais. C'est pourquoi on sert du vin blanc avec les fromages.",
                IdRecetteExemple = null
            }
        };

        await context.QuestionsQuiz.AddRangeAsync(questions);
        await context.SaveChangesAsync();  // flush pour récupérer les IDs

        // ===== 2. Les réponses (4 par question, 1 seule correcte) =====
        // On récupère les IDs réellement attribués par SQL Server
        var q = questions.ToList();

        var reponses = new List<ReponseQuiz>
        {
            // --- Question 1 : CdC 3.5.1 ---
            new() { IdQuestion = q[0].IdQuestion, TexteReponse = "Un vin rouge tannique (Cabernet)",         EstCorrecte = false },
            new() { IdQuestion = q[0].IdQuestion, TexteReponse = "Un vin blanc avec de l'acidité (Chardonnay)", EstCorrecte = true  },
            new() { IdQuestion = q[0].IdQuestion, TexteReponse = "Un vin très sucré (Porto)",                EstCorrecte = false },
            new() { IdQuestion = q[0].IdQuestion, TexteReponse = "Une bière ambrée",                         EstCorrecte = false },

            // --- Question 2 : CdC 3.5.1 ---
            new() { IdQuestion = q[1].IdQuestion, TexteReponse = "Un mocktail fruité et sucré",              EstCorrecte = true  },
            new() { IdQuestion = q[1].IdQuestion, TexteReponse = "Un vin rouge tannique et corsé",           EstCorrecte = false },
            new() { IdQuestion = q[1].IdQuestion, TexteReponse = "Un thé glacé",                             EstCorrecte = false },
            new() { IdQuestion = q[1].IdQuestion, TexteReponse = "Un vin blanc doux (Gewurztraminer)",       EstCorrecte = false },

            // --- Question 3 : composées (principe : 4 propositions, 1 correcte) ---
            new() { IdQuestion = q[2].IdQuestion, TexteReponse = "Un vin blanc vif et minéral",              EstCorrecte = false },
            new() { IdQuestion = q[2].IdQuestion, TexteReponse = "Un vin rouge tannique (Cabernet Sauvignon)", EstCorrecte = true  },
            new() { IdQuestion = q[2].IdQuestion, TexteReponse = "Un mocktail aux agrumes",                  EstCorrecte = false },
            new() { IdQuestion = q[2].IdQuestion, TexteReponse = "Un vin doux (Moscato d'Asti)",             EstCorrecte = false },

            // --- Question 4 : composées (expert, umami pur + tannins) ---
            new() { IdQuestion = q[3].IdQuestion, TexteReponse = "Parce que les champignons sont trop secs", EstCorrecte = false },
            new() { IdQuestion = q[3].IdQuestion, TexteReponse = "Parce que l'umami des champignons réagit avec les tannins et crée de l'amertume", EstCorrecte = true  },
            new() { IdQuestion = q[3].IdQuestion, TexteReponse = "Parce que le vin rouge est trop acide",    EstCorrecte = false },
            new() { IdQuestion = q[3].IdQuestion, TexteReponse = "Parce que le parmesan est incompatible avec tout vin rouge", EstCorrecte = false },

            // --- Question 5 : composées (débutant, principe général) ---
            new() { IdQuestion = q[4].IdQuestion, TexteReponse = "Le gras amplifie l'acidité, il faut donc un vin doux",  EstCorrecte = false },
            new() { IdQuestion = q[4].IdQuestion, TexteReponse = "L'acidité coupe le gras et rafraîchit le palais",        EstCorrecte = true  },
            new() { IdQuestion = q[4].IdQuestion, TexteReponse = "Il faut toujours éviter l'acidité avec le gras",         EstCorrecte = false },
            new() { IdQuestion = q[4].IdQuestion, TexteReponse = "Seuls les vins rouges peuvent équilibrer un plat gras",  EstCorrecte = false }
        };

        await context.ReponsesQuiz.AddRangeAsync(reponses);
        await context.SaveChangesAsync();
    }
}