# CulinaryPairing
Plateforme web d'aide à la décision culinaire et aux accords boissons - TFE EPHEC 2025-2026
# PITCH - Plateforme d'aide à la décision culinaire

## Le problème

On a tous vécu ça : on ouvre le frigo, on regarde ce qu'il y a, et on ne sait pas quoi cuisiner. Ou alors, on prépare un bon plat et on se demande quel vin servir avec. Pire encore : on reçoit des amis dont certains sont végétariens, et on doit jongler entre plusieurs versions du même plat.

Les sites de recettes actuels proposent des milliers de plats, mais :
- Peu prennent en compte ce qu'on a **vraiment** chez soi
- Aucun n'explique **pourquoi** tel vin va avec tel plat
- Quand ils proposent des alternatives végé, c'est souvent une recette complètement différente, pas une adaptation
- Et quand ils utilisent l'IA ? C'est une boîte noire. Zéro explication.

## La solution

Une application web qui aide vraiment à cuisiner intelligemment :

**🔍 Recherche intelligente**
Trouve des recettes selon ce que tu as dans ton frigo, avec un score de faisabilité.

**🥬 Substitutions végé/végan automatiques**
Le bœuf bourguignon devient "bourguignon aux pleurotes" en un clic. Avec les bons ratios (500g bœuf = 400g pleurotes) et les notes de cuisson adaptées ("ajouter en fin de cuisson"). Plus besoin de chercher une autre recette pour tes amis végés.

**🍷 Accords mets-boissons expliqués**
Pas juste "prenez un Chardonnay", mais "prenez un Chardonnay **parce que** son acidité équilibre le gras du risotto". Tu comprends, tu apprends.

**🔄 Accord inversé**
Tu as une bouteille de vin chez toi et tu veux savoir quoi cuisiner avec ? Personne ne fait ça bien. Nous, on le fait.

**🎓 Mode Apprends**
Un quiz ludique pour comprendre les bases des accords. Pourquoi un vin rouge tannique avec un steak ? Pourquoi éviter les tannins avec les champignons ? Tu deviens ton propre sommelier.

**🎉 Mode Soirée**
Tu reçois 6 amis, dont 2 végétariens et 1 sans gluten, avec un budget de 80€ et 2h pour cuisiner ? L'appli te génère un menu complet avec les accords et les versions adaptées : entrée, plat (version classique + végé), dessert, liste de courses. En 2 clics.

## Ce qui nous rend uniques

| Autres sites | Notre approche |
|--------------|----------------|
| IA opaque | Comparaison règles vs IA, tu vois les deux |
| Accords sans explication | Justification pédagogique systématique |
| Plat → boisson uniquement | Boisson → plat aussi (accord inversé) |
| Recettes végé = autre recette | **Substitutions intelligentes** avec ratios |
| Pas d'apprentissage | Quiz pour devenir autonome |

## Techniquement

- **Backend** : ASP.NET Core 8 + SQL Server
- **Frontend** : Angular 17
- **IA** : Utilisée comme assistant (estimation des caractéristiques gustatives), pas comme décideur
- **Cœur** : Un moteur de règles gastronomiques explicables + un système de substitutions avec ratios

## Pour qui ?

- **Marie, 32 ans** : mère de famille qui veut réduire le gaspillage, cuisiner malgré l'allergie de son fils, et adapter ses plats quand sa sœur végétarienne vient dîner
- **Thomas, 28 ans** : reçoit des amis (souvent mixte végé/non-végé), veut impressionner avec de bons accords sans être sommelier

## Résultat attendu

- Trouver une recette adaptée en **moins de 2 minutes**
- Utiliser **80% des ingrédients** qu'on a déjà
- Adapter n'importe quelle recette en version végé/végan **en 1 clic**
- Comprendre **pourquoi** ce vin va avec ce plat

---

**Projet TFE 2025-2026**

⚠️ *L'abus d'alcool est dangereux pour la santé. À consommer avec modération.*
