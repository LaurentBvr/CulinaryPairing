# 🍽️ CulinaryPairing — Plateforme d'aide à la décision culinaire

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![Angular](https://img.shields.io/badge/Angular-17-DD0031?logo=angular)
![SQL Server](https://img.shields.io/badge/SQL_Server-LocalDB-CC2927?logo=microsoftsqlserver)
![License](https://img.shields.io/badge/License-Academic-blue)

**Projet TFE — Travail de Fin d'Études** — Application web intelligente qui aide à trouver des recettes personnalisées, découvrir les meilleurs accords mets-boissons (fondés sur 7 principes de sommellerie), et adapter automatiquement les recettes en version végétarienne ou végane.

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
- 🍷 **Découvrir les meilleurs accords mets-boissons** avec des explications claires, fondées sur 7 principes de sommellerie
- 🎓 **Apprendre les bases des accords** via un quiz ludique
- 🔁 **Partir d'une bouteille de vin** pour trouver quoi cuisiner (accord inversé)
- 🎉 **Planifier une soirée complète** avec menu et accords (mode soirée)

### Ce qui rend ce projet unique

| Autres plateformes | CulinaryPairing |
|---|---|
| IA opaque | Comparaison règles vs IA, avec visualisation des deux |
| Accords sans explication | Justification pédagogique multi-critères systématique |
| Plat → boisson uniquement | Boisson → plat également (accord inversé) |
| Recettes fixes | Substitutions végé/végan automatiques avec ratios et notes |
| Pas d'apprentissage | Quiz progressif pour devenir autonome en accords |
| Moteur simple (if/else) | Moteur à **18 règles** et **score pondéré sur 100** avec niveau de confiance |

---

## ✨ Fonctionnalités

### Must Have (V1)

- [ ] Authentification (inscription/connexion JWT)
- [ ] Recherche de recettes multi-critères
- [ ] Fiches recettes avec adaptation des portions (fractions supportées)
- [ ] Substitutions végé/végan avec ratios et notes de cuisson
- [ ] Gestion des contraintes alimentaires (allergies, régimes, convictions)
- [ ] **Moteur de règles d'accords à 18 règles avec score pondéré** ⭐
- [ ] Accords expliqués avec justifications multi-critères
- [ ] Accord inversé (boisson → recettes)
- [ ] Favoris et historique

### Should Have

- [ ] Mode vide-frigo
- [ ] Estimation IA des caractéristiques gustatives
- [ ] Comparaison règle métier vs IA
- [ ] Mode Apprends (quiz)
- [ ] Mode Soirée (menu complet)
- [ ] Interface Admin (modération, gestion des données)

### Could Have

- [ ] Mode budget avec estimation des coûts
- [ ] Suggestions saisonnières
- [ ] Export liste de courses (PDF)
- [ ] Upload d'images pour les recettes

---

## 🍷 Le moteur d'accords — le cœur du projet

Le moteur de recommandation d'accords mets-boissons repose sur **7 principes de sommellerie** formalisés et **18 règles** agrégées par un algorithme de score pondéré.

### Les 7 principes

1. **Intensité aromatique** — La boisson ne doit jamais écraser le plat, ni l'inverse.
2. **Acidité** — L'acidité coupe le gras et doit être au moins équivalente à celle du plat.
3. **Sucre** — Un dessert appelle une boisson au moins aussi sucrée ; le sucre apaise le piquant.
4. **Tannins** — Affinité avec les protéines (viande rouge, fromages affinés) ; hostilité avec umami pur, poisson cru, œuf.
5. **Degré d'alcool** — Un alcool élevé amplifie la brûlure du piquant et la salinité.
6. **Profil aromatique** — Accords par similitude ou par contraste sur 14 familles aromatiques.
7. **Préparation culinaire** — Le mode de cuisson et la sauce dominante influencent fortement l'accord.

### Algorithme de score

Pour chaque couple (recette, boisson), le moteur :

1. Applique des **règles éliminatoires** (sans alcool, tannins hostiles, malus alcool > 20)
2. Calcule un **malus gradué** pour la règle R16bis : `malus = max(0, (degré_alcool − 13) × niveau_épice / 10)`
3. Évalue les **18 règles** pondérées (poids de 10 à 25)
4. Normalise : `score = (poids_effectif / poids_max) × 100 − malus`
5. Calcule le **niveau de confiance** : pourcentage de règles applicables au plat

Le score final est affiché sous la forme : **« Score : 86/100 — basé sur 8 critères sur 12 (confiance 67 %) »**

### Exemple : Risotto aux champignons + Chardonnay

Le risotto a `affinite_tannins = hostile` (umami pur) et `type_sauce = beurre`. Le Chardonnay (niveau_tannins = 2, familles aromatiques = [beurre, boisé, fruits blancs]) satisfait 7 règles sur 8 applicables → **score 86/100**. Les règles R10bis (intensité équivalente), R12 (faibles tannins), R21bis (sauce beurre ↔ Chardonnay boisé) et R19bis (famille beurre partagée) sont déterminantes.

---

## 🛠️ Technologies

### Backend
- **ASP.NET Core 8.0** — Framework web
- **Entity Framework Core** — ORM avec migrations
- **SQL Server** — LocalDB (MSSQLLocalDB) en développement, Express en production
- **JWT + ASP.NET Core Identity** — Authentification et gestion utilisateurs
- **Swagger / OpenAPI** — Documentation de l'API
- **Architecture N-Tier** — Séparation Controllers / Services / Data Access

### Frontend
- **Angular 17** — Framework SPA
- **TypeScript** — Langage
- **Tailwind CSS** — Framework UI
- **RxJS** — Programmation réactive

### Intelligence Artificielle
- **API LLM externe** (Claude, GPT ou équivalent) — Appel HTTP/REST depuis le backend
- **Utilisations** : estimation des caractéristiques gustatives (7 dimensions), identification des familles aromatiques, détection de `affinite_tannins` et `mode_cuisson`, génération de justifications enrichies, suggestions d'amélioration
- **Fallback** : saisie manuelle toujours disponible ; mise en cache des estimations pour éviter les appels redondants

### DevOps
- **Git + GitHub** — Versioning et gestion de projet via issues
- **VS Code** — IDE principal (C# Dev Kit + Angular Language Service)

---

## 🚀 Installation

### Prérequis

- **Visual Studio 2022** ou VS Code avec C# Dev Kit
- **.NET 8.0 SDK** — [Télécharger](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 18+** et npm
- **SQL Server LocalDB** (inclus avec Visual Studio ou [installable séparément](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb))
- **Angular CLI** : `npm install -g @angular/cli`
- **Entity Framework CLI** : `dotnet tool install --global dotnet-ef`

### Cloner le projet

```bash
git clone https://github.com/LaurentBvr/CulinaryPairing.git
cd CulinaryPairing
```

### Backend

```bash
cd backend

# Restaurer les packages
dotnet restore

# La chaîne de connexion par défaut pointe sur LocalDB
# (voir appsettings.json) :
# "Server=(localdb)\\MSSQLLocalDB;Database=CulinaryPairing;Trusted_Connection=True;"

# Appliquer les migrations (crée la BDD et les 19 tables)
dotnet ef database update

# Lancer le serveur
dotnet run
```

Le backend tourne sur **https://localhost:5001** (Swagger disponible sur `/swagger`).

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