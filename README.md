# 🍽️ CulinaryPairing - Plateforme d'aide à la décision culinaire

[![.NET](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-17-red)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-blue)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green)](LICENSE)

> **Projet TFE** - Application web intelligente pour trouver des recettes personnalisées, découvrir les meilleurs accords mets-boissons, et adapter automatiquement les recettes en version végétarienne ou végane.

---

## 📋 Table des matières

- [Présentation](#-présentation)
- [Fonctionnalités](#-fonctionnalités)
- [Technologies](#-technologies)
- [Installation](#-installation)
- [Structure du projet](#-structure-du-projet)
- [Documentation](#-documentation)
- [Roadmap](#-roadmap)
- [Auteur](#-auteur)

---

## 🎯 Présentation

**CulinaryPairing** est une plateforme web qui aide les utilisateurs à :
- Trouver des recettes adaptées à ce qu'ils ont chez eux (mode vide-frigo)
- **Adapter automatiquement les recettes en version végé/végan** avec ratios et notes de cuisson
- Découvrir les meilleurs accords mets-boissons avec des **explications claires**
- Apprendre les bases des accords via un **quiz ludique**
- Partir d'une bouteille de vin pour trouver quoi cuisiner (**accord inversé**)
- Planifier une soirée complète avec menu et accords (**mode soirée**)

### Ce qui rend ce projet unique

| Autres sites | Notre approche |
|--------------|----------------|
| IA opaque | Comparaison règles vs IA, tu vois les deux |
| Accords sans explication | Justification pédagogique systématique |
| Plat → boisson uniquement | Boisson → plat aussi (accord inversé) |
| Recettes fixes | **Substitutions végé/végan automatiques** avec ratios |
| Pas d'apprentissage | Quiz pour devenir autonome |

---

## ✨ Fonctionnalités

### Must Have (V1)
- [x] Authentification (inscription/connexion)
- [ ] Recherche de recettes multi-critères
- [ ] Fiches recettes avec adaptation des portions (fractions supportées)
- [ ] **Substitutions végé/végan** avec ratios et notes de cuisson
- [ ] Gestion des contraintes alimentaires
- [ ] **Moteur de règles d'accords boissons** ⭐
- [ ] Accords expliqués avec justifications
- [ ] **Accord inversé** (boisson → recettes)
- [ ] Favoris et historique

### Should Have
- [ ] Mode vide-frigo
- [ ] **Estimation IA des caractéristiques gustatives**
- [ ] Comparaison règle métier vs IA
- [ ] **Mode Apprends** (quiz)
- [ ] **Mode Soirée** (menu complet)
- [ ] Interface Admin (modération, gestion données)

### Could Have
- [ ] Mode budget avec estimation des coûts
- [ ] Suggestions saisonnières
- [ ] Export liste de courses (PDF)
- [ ] Upload d'images pour les recettes

---

## 🛠️ Technologies

### Backend
- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core** - ORM
- **SQL Server** (LocalDB pour dev, SQL Server Express pour prod)
- **JWT** - Authentification
- **Swagger** - Documentation API

### Frontend
- **Angular 17** - Framework SPA
- **TypeScript** - Langage
- **Angular Material** / **Tailwind CSS** - UI
- **RxJS** - Programmation réactive

### Intelligence Artificielle
- **API externe** (Claude, GPT, ou autre LLM)
- Utilisée pour : estimation caractéristiques, suggestions, accords alternatifs

### DevOps
- **Git** + **GitHub** - Versioning
- **Visual Studio 2022** - IDE principal

---

## 🚀 Installation

### Prérequis

- [Visual Studio 2022](https://visualstudio.microsoft.com/) avec workload ASP.NET
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (inclus avec Visual Studio)
- [Angular CLI](https://angular.io/cli) : `npm install -g @angular/cli`

### Backend

```bash
# Cloner le repo
git clone https://github.com/[TON_USERNAME]/tfe-culinary-pairing.git
cd tfe-culinary-pairing

# Aller dans le backend
cd backend

# Restaurer les packages
dotnet restore

# Configurer la connexion BDD (appsettings.json)
# ConnectionString par défaut : "Server=(localdb)\\mssqllocaldb;Database=CulinaryPairing;Trusted_Connection=True;"

# Appliquer les migrations
dotnet ef database update

# Lancer le serveur
dotnet run
```

Le backend tourne sur `https://localhost:5001`

### Frontend

```bash
# Dans un nouveau terminal
cd frontend

# Installer les dépendances
npm install

# Lancer le serveur de dev
ng serve
```

L'application est accessible sur `http://localhost:4200`

---

## 📁 Structure du projet

```
tfe-culinary-pairing/
├── backend/
│   ├── Controllers/          # Points d'entrée API
│   │   ├── AuthController.cs
│   │   ├── RecipesController.cs
│   │   ├── PairingController.cs
│   │   ├── SubstitutionController.cs  
│   │   ├── QuizController.cs
│   │   ├── PartyController.cs
│   │   └── AdminController.cs         
│   ├── Services/             # Logique métier
│   │   ├── PairingEngine.cs           # Moteur de règles
│   │   ├── SubstitutionService.cs     # ⭐ Gestion végé/végan
│   │   ├── InversePairingService.cs
│   │   ├── QuizService.cs
│   │   ├── PartyService.cs
│   │   └── AiService.cs
│   ├── Models/               # Entités (17 tables)
│   ├── DTOs/                 # Data Transfer Objects
│   ├── Data/                 # DbContext + Migrations
│   └── Program.cs
│
├── frontend/
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/   # Composants réutilisables
│   │   │   ├── pages/        # Pages/écrans (11 écrans)
│   │   │   ├── services/     # Services HTTP
│   │   │   ├── models/       # Interfaces TypeScript
│   │   │   └── guards/       # Auth guards + Admin guard
│   │   └── assets/
│   └── angular.json
│
├── docs/
│   ├── cahier-des-charges-v1.pdf
│   ├── diagramme-ea.png
│   └── architecture.png
│
├── .gitignore
└── README.md
```

---

## 📚 Documentation

| Document | Description |
|----------|-------------|
| [Cahier des charges V1](docs/cahier-des-charges-v1.pdf) | Spécifications complètes |
| [Diagramme E-A](docs/diagramme-ea.png) | Modèle de données (17 tables) |
| [Architecture](docs/architecture.png) | Schéma technique |

### API Endpoints (aperçu)

| Méthode | Endpoint | Description |
|---------|----------|-------------|
| POST | `/api/auth/register` | Inscription |
| POST | `/api/auth/login` | Connexion |
| GET | `/api/recipes` | Liste des recettes |
| GET | `/api/recipes/{id}` | Détail d'une recette |
| GET | `/api/recipes/{id}/substitutions` | ⭐ Substitutions végé/végan |
| GET | `/api/recipes/{id}/pairings` | Accords pour une recette |
| GET | `/api/pairings/inverse/{beverageId}` | Accord inversé |
| GET | `/api/quiz/questions` | Questions du quiz |
| POST | `/api/party/generate` | Générer menu soirée |
| GET | `/api/admin/ingredients` | ⭐ Admin - Liste ingrédients |
| PUT | `/api/admin/ingredients/{id}/price` | ⭐ Admin - MAJ prix |

---

## 🗓️ Roadmap

| Semaine | Objectif | Status |
|---------|----------|--------|
| S1-S2 | Setup + Auth + BDD + Admin basique | 🔄 En cours |
| S3-S4 | CRUD Recettes + Recherche | ⏳ À faire |
| S5-S6 | **Substitutions végé/végan** + Vide-frigo | ⏳ À faire |
| S7-S8 | Moteur de règles + Accords | ⏳ À faire |
| S9 | Accord inversé | ⏳ À faire |
| S10 | Intégration IA | ⏳ À faire |
| S11 | Mode Apprends (Quiz) | ⏳ À faire |
| S12-S13 | Mode Soirée | ⏳ À faire |
| S14 | Tests + Polish | ⏳ À faire |

---

## ⚠️ Avertissement

> **L'abus d'alcool est dangereux pour la santé. À consommer avec modération.**

---

## 👤 Auteur

BIVER LAURENT

- École : EPHEC
- Année : 2025-2026
- Projet : Travail de Fin d'Études (TFE)

---

## 📄 License

Ce projet est développé dans un cadre académique. Tous droits réservés.
---

**Projet TFE 2025-2026**

⚠️ *L'abus d'alcool est dangereux pour la santé. À consommer avec modération.*
