# 🍽️ CulinaryPairing — Plateforme d'aide à la décision culinaire

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-20.1.3-DD0031?logo=angular)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-Academic-blue)
![Status](https://img.shields.io/badge/Status-V1.3_Livré-success)

**Projet TFE — Travail de Fin d'Études** — Application web qui aide à trouver des recettes personnalisées, découvrir les meilleurs accords mets-boissons fondés sur **8 principes de sommellerie**, et adapter automatiquement les recettes en versions végétarienne ou végane.

---

## 📋 Table des matières

- [Présentation](#-présentation)
- [Fonctionnalités](#-fonctionnalités)
- [Le moteur d'accords](#-le-moteur-daccords--le-cœur-du-projet)
- [Technologies](#️-technologies)
- [Installation](#-installation)
- [Structure du projet](#-structure-du-projet)
- [Documentation](#-documentation)
- [Roadmap](#️-roadmap)
- [Auteur](#-auteur)

---

## 🎯 Présentation

**CulinaryPairing** est une plateforme web qui aide les utilisateurs à :

- 🔍 **Trouver des recettes** adaptées à ce qu'ils ont chez eux (mode vide-frigo)
- 🌱 **Adapter automatiquement les recettes** en version végé/végan avec ratios et notes de cuisson
- 🍷 **Découvrir les meilleurs accords mets-boissons** avec des explications claires, fondées sur 8 principes de sommellerie
- 🎓 **Apprendre les bases des accords** via un quiz progressif
- 🔁 **Partir d'une bouteille de vin** pour trouver quoi cuisiner (accord inversé)
- 🎉 **Planifier une soirée complète** avec un menu structuré entrée / plat / dessert (mode Soirée)

### Ce qui rend ce projet unique

| Autres plateformes | CulinaryPairing |
|---|---|
| IA opaque | Architecture transparente : règles métier explicites + IA optionnelle |
| Accords sans explication | Justification pédagogique multi-critères systématique |
| Plat → boisson uniquement | Boisson → plat également (accord inversé) |
| Recettes fixes | Substitutions végé/végan automatiques avec ratios et notes |
| Pas d'apprentissage | Quiz progressif pour devenir autonome en accords |
| Moteur simple (if/else) | Moteur à **16 règles**, **score pondéré sur 100**, **niveau de confiance** et **malus gradué d'alcool** |

---

## ✨ Fonctionnalités

Statut au 7 mai 2026 : **toutes les Must-have V1 sont livrées**, ainsi que la majorité des Should-have. Liste alignée sur le Cahier des Charges V1.3, section 1.6.

### Must Have (V1) — ✅ Livrées

- [x] Inscription / connexion utilisateur (BCrypt + JWT)
- [x] Recherche de recettes multi-critères
- [x] Fiches recettes complètes avec adaptation des portions (R6 / R7, fractions supportées)
- [x] Gestion des contraintes alimentaires via la table `INGREDIENT_CONTRAINTE` (R9 / R16)
- [x] Substitutions végétariennes / véganes avec notes de cuisson (R17 / R18 / R19)
- [x] **Moteur d'accords à 16 règles avec score pondéré** ⭐ — cœur du projet
- [x] Accords boissons expliqués (justifications multi-critères)
- [x] Accord inversé (boisson → plat)
- [x] Gestion des favoris (recettes)

### Should Have — ✅ Livrées (partiellement)

- [x] Mode vide-frigo (R8)
- [x] Mode Soirée (menu structuré entrée / plat / dessert + sauvegarde menu en favori via `FAVORI_MENU`)
- [x] Historique des recettes consultées
- [x] Interface Admin basique (archivage de recettes, désactivation de comptes)
- [ ] Alternatives sans alcool systématiques
- [ ] Estimation IA des caractéristiques gustatives
- [ ] Comparaison règle métier vs IA
- [ ] Mode Apprends (quiz basique) — base prévue, contenu à enrichir

### Could Have — Reportées V2

- [ ] Mode budget avec estimation des coûts
- [ ] Suggestions saisonnières
- [ ] Export liste de courses (PDF)
- [ ] Badges et progression dans le quiz
- [ ] Upload d'images pour les recettes

---

## 🍷 Le moteur d'accords — le cœur du projet

Le moteur de recommandation d'accords mets-boissons repose sur **8 principes de sommellerie** formalisés et **16 règles additives** agrégées par un algorithme de score pondéré, complétées par un **malus gradué éliminatoire (R16bis)** sur le degré d'alcool.

### Les 8 principes

1. **Intensité aromatique** — La boisson ne doit jamais écraser le plat, ni l'inverse.
2. **Acidité** — L'acidité coupe le gras et doit être au moins équivalente à celle du plat.
3. **Sucre (douceur)** — Un dessert appelle une boisson au moins aussi sucrée ; le sucre apaise le piquant.
4. **Tannins** — Affinité avec les protéines (viande rouge, fromages affinés) ; hostilité avec umami pur, poisson cru, œuf.
5. **Degré d'alcool** — Un alcool élevé amplifie la brûlure du piquant et la salinité (malus gradué éliminatoire).
6. **Profil aromatique** — Accords par similitude ou par contraste sur 14 familles aromatiques.
7. **Préparation culinaire** — Le mode de cuisson et la sauce dominante influencent fortement l'accord.
8. **Amertume** — Cinquième goût fondamental, essentiel pour les bières (IPA, stout) et cocktails amers (Negroni, Spritz Campari) ; bonifie les plats gras, frits et umami.

### Algorithme de score

Pour chaque couple `(recette, boisson)`, le moteur :

1. Applique des **règles éliminatoires** (sans alcool, tannins hostiles)
2. Calcule un **malus gradué** R16bis : `malus = max(0, (degré_alcool − 13) × niveau_piquant / 10)` — score forcé à 0 au-delà de 20
3. Évalue les **16 règles additives** pondérées (somme des poids = 260)
4. Normalise : `score = (poids_effectif / poids_max) × 100 − malus`
5. Calcule un **niveau de confiance** : pourcentage de règles applicables au plat

Le score final est affiché sous la forme : **« Score : 86/100 — basé sur 8 critères sur 12 (confiance 67 %) »**

### Cache de calcul

Les scores sont **cachés en base** dans la table `ACCORD` avec un champ `version_moteur` (figé à `1.3`). Toute évolution du moteur incrémente cette version, invalidant automatiquement les entrées de cache obsolètes.

### Exemple : Risotto aux champignons + Chardonnay

Le risotto a `affinite_tannins = hostile` (umami pur) et `type_sauce = beurre`. Le Chardonnay (`niveau_tannins = 2`, familles aromatiques = `[beurre, boisé, fruits blancs]`) satisfait 7 règles sur 8 applicables → **score 86/100**. Les règles R10bis (intensité équivalente), R12 (faibles tannins), R21bis (sauce beurre ↔ Chardonnay boisé) et R19bis (famille beurre partagée) sont déterminantes.

---

## 🛠️ Technologies

### Backend (.NET 10, architecture N-Tier 4 couches)

- **.NET 10** + **ASP.NET Core 10** — Framework web et API REST
- **Entity Framework Core 10** — ORM avec migrations (9 migrations appliquées en V1.3)
- **SQL Server LocalDB** (`MSSQLLocalDB`) — Base `CulinaryPairingDb` en développement
- **BCrypt.Net-Next** + **System.IdentityModel.Tokens.Jwt** — Authentification (choix assumé contre ASP.NET Identity, voir CdC §5.1.1)
- **Swagger / OpenAPI** — Documentation interactive de l'API
- **Architecture N-Tier** — 4 projets : `API` / `BLL` / `DAL` / `Entities` avec dépendances unidirectionnelles

### Frontend

- **Angular 20.1.3 standalone** (sans zone.js, sans suffixe de composant) — Framework SPA
- **TypeScript** — Langage
- **SCSS** — Styles
- **Angular signals** + **Reactive forms** — Réactivité et formulaires
- **Node.js 22.17.1** — Runtime

### Intelligence Artificielle (Should-have, prévu)

- **API LLM externe** (Claude API ou équivalent) — Appel HTTP/REST depuis le backend
- **Cache table `ACCORD`** — Le champ `type_accord` distingue déjà les valeurs `'regle'` et `'ia'` ; le moteur règle est livré en V1.3, le module IA est planifié pour V2 avec stockage parallèle des deux types d'accord
- **Fallback** : saisie manuelle toujours disponible

### DevOps

- **Git + GitHub** — Versioning et gestion de projet
- **VS Code** — IDE principal (C# Dev Kit + Angular Language Service)
- **sqlcmd** + **EF Core CLI** — Administration BDD et migrations

---

## 🚀 Installation

### Prérequis

- **Visual Studio 2022+** ou **VS Code** avec C# Dev Kit
- **.NET 10 SDK** — [Télécharger](https://dotnet.microsoft.com/download/dotnet/10.0)
- **Node.js 22.17.1+** et npm
- **SQL Server LocalDB** (inclus avec Visual Studio ou [installable séparément](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb))
- **Angular CLI** : `npm install -g @angular/cli@20`
- **Entity Framework CLI** : `dotnet tool install --global dotnet-ef`

### Cloner le projet

```bash
git clone https://github.com/LaurentBvr/CulinaryPairing.git
cd CulinaryPairing
```

### Backend

```bash
cd CulinaryPairing.API

# Restaurer les packages
dotnet restore

# La chaîne de connexion par défaut pointe sur LocalDB
# (voir appsettings.json) :
# "Server=(localdb)\\MSSQLLocalDB;Database=CulinaryPairingDb;Trusted_Connection=True;"

# Appliquer les migrations (crée la BDD et les 23 tables)
dotnet ef database update --project ../CulinaryPairing.DAL --startup-project .

# Lancer le serveur
dotnet run
```

Le backend tourne sur **http://localhost:5011** (Swagger disponible sur `/swagger`).

### Frontend

Dans un nouveau terminal :

```bash
cd frontend

# Installer les dépendances
npm install

# Lancer le serveur de dev
ng serve
```

L'application est accessible sur **http://localhost:4200**.

---

## 📁 Structure du projet

```
CulinaryPairing/
├── CulinaryPairing.Entities/         # Couche Entités (sans dépendance externe)
│   ├── Models/                       # 23 classes d'entité avec annotations EF Core
│   └── Enums/                        # 16 énumérations (TypeBoisson, Statut, CorpsBoisson…)
│
├── CulinaryPairing.DAL/              # Couche d'accès aux données
│   ├── CulinaryPairingDbContext.cs   # Configuration EF Core, CHECK, UNIQUE, conversions
│   ├── Migrations/                   # 9 migrations EF Core
│   └── Seed/                         # Initialisation seed data V1.3
│
├── CulinaryPairing.BLL/              # Couche logique métier
│   ├── Services/                     # IRecetteService, IAccordService, ISoireeService…
│   ├── PairingEngine/                # ⭐ Moteur d'accords (Strategy Pattern)
│   │   ├── PairingEngineService.cs   # Orchestrateur — 16 règles + R16bis
│   │   └── Rules/                    # Une classe par règle additive
│   │       ├── R10_Intensite.cs
│   │       ├── R13bis_AffiniteTannins.cs
│   │       ├── R16bis_MalusAlcool.cs
│   │       ├── R22bis_Amertume.cs
│   │       └── ...
│   ├── DTOs/                         # Data Transfer Objects (RegisterDto, AccordDto…)
│   └── Validators/                   # Validations métier
│
├── CulinaryPairing.API/              # Couche présentation (REST)
│   ├── Controllers/                  # AuthController, RecettesController, AccordsController…
│   ├── Middleware/                   # Authentification JWT
│   └── Program.cs                    # Setup DI, CORS, JWT, OpenAPI
│
├── frontend/                         # Application Angular 20 standalone
│   ├── src/app/
│   │   ├── pages/                    # 11 écrans principaux
│   │   ├── shared/components/        # Composants réutilisables (AccordCard, RecettePicker…)
│   │   ├── services/                 # Services HTTP avec signals
│   │   ├── models/                   # Interfaces TypeScript
│   │   └── guards/                   # AuthGuard, AdminGuard
│   └── angular.json
│
├── docs/
│   ├── CdC_v1.3_Biver_Laurent.docx   # Cahier des charges V1.3
│   ├── ERD_v1.3.dbml                 # Modèle entité-association (DBML)
│   ├── ERD_v1.3.png                  # ERD rendu (export dbdiagram.io)
│   ├── architecture_v1.3.png         # Schéma N-Tier (Mermaid)
│   └── workflow_substitution_v1.3.png # Workflow substitution végé (Mermaid)
│
├── .gitignore
└── README.md
```

---

## 📚 Documentation

| Document | Description |
|---|---|
| [Cahier des charges V1.3](docs/CdC_v1.3_Biver_Laurent.docx) | Spécifications complètes — 16 règles, 23 tables, 8 principes |
| [ERD V1.3](docs/ERD_v1.3.dbml) | Modèle de données au format DBML (à coller sur dbdiagram.io) |
| [Architecture N-Tier](docs/architecture_v1.3.png) | Schéma technique des 4 couches |
| [Workflow substitution](docs/workflow_substitution_v1.3.png) | Processus de substitution végé / végan |

### Aperçu des endpoints API

| Méthode | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Inscription |
| POST | `/api/auth/login` | Connexion (retourne JWT) |
| GET | `/api/auth/me` | Profil utilisateur courant |
| GET | `/api/recettes` | Liste des recettes (filtrable, statut `Publiee`) |
| GET | `/api/recettes/{id}` | Détail d'une recette avec étapes |
| GET | `/api/recettes/{id}/substitutions` | Substitutions végé/végan disponibles |
| GET | `/api/accords/recette/{id}` | Accords pour une recette (tri par score) |
| GET | `/api/accords/inverse/{boissonId}` | ⭐ Accord inversé (boisson → recettes) |
| GET | `/api/vide-frigo` | Recettes compatibles avec ingrédients en stock |
| POST | `/api/soirees` | Créer une soirée |
| POST | `/api/soirees/{id}/menu` | Composer un menu (entrée / plat / dessert) |
| GET | `/api/quiz/questions` | Questions du quiz |
| GET | `/api/admin/recettes` | Admin — Liste recettes (avec brouillons / archivées) |

Documentation interactive complète disponible via **Swagger** sur `/swagger` (en mode développement).

---

## 🗓️ Roadmap

Planning condensé sur **3 semaines** (du 19 avril au 11 mai 2026). Soumission de l'application et du cahier des charges le **11 mai** ; soutenance entre le 11 mai et le 11 juin.

| Période | Dates | Objectif | Statut |
|---|---|---|---|
| Semaine 1 | 19-25 avril | Setup projet, BDD, JWT, profil, CRUD recettes | ✅ Terminé |
| Semaine 1-2 | 25 avril - 1er mai | Recherche, fiche recette, portions, substitutions végé/végan | ✅ Terminé |
| Semaine 2 | 2-4 mai | Moteur d'accords à 16 règles + score pondéré + accord inversé | ✅ Terminé |
| Semaine 2-3 | 5-7 mai | Mode Soirée (V2 anticipée), polish, enrichissement seed | ✅ Terminé |
| Semaine 3 | 8-10 mai | Vide-frigo, finalisation CdC V1.3, dette technique documentée | ✅ Terminé |
| **Rendu** | **11 mai** | **Livraison application + cahier des charges V1.3** | 🎯 Imminent |
| Soutenance | 11 mai - 11 juin | Préparation et oral de défense | ⏳ |

### Roadmap V2 (post-soutenance)

Voir CdC V1.3 §5.4 — Perspectives V2 :
- Cohortes nominatives par invité dans le Mode Soirée
- Module IA pour les accords (LLM avec score parallèle au moteur règles)
- Migration vers Clean Architecture
- Application mobile native (.NET MAUI ou Flutter)
- Internationalisation et expansion catalogue

---

## ⚠️ Avertissement

**L'abus d'alcool est dangereux pour la santé. À consommer avec modération.**

Cette mention est affichée sur toutes les pages de l'application proposant des accords avec boissons alcoolisées.

---

## 👤 Auteur

**BIVER Laurent**

- 🏫 **École** : EPHEC
- 📅 **Année académique** : 2025-2026
- 🎓 **Projet** : Travail de Fin d'Études (TFE)
- 🔗 **GitHub** : [@LaurentBvr](https://github.com/LaurentBvr)

---

## 📄 License

Ce projet est développé dans un cadre académique. Tous droits réservés.

**Projet TFE 2025-2026 — Version 1.3**

---

⚠️ *L'abus d'alcool est dangereux pour la santé. À consommer avec modération.*
