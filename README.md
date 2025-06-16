# 🎮 Morpion Multijoueur (.NET 6)

## 📦 Contenu
- `MorpionServer` : serveur (console)
- `MorpionClientGUI` : client avec interface graphique (Windows Forms)
- `MorpionGame.sln` : solution Visual Studio (facultatif)
- `LancerTout.bat` : script pour tout lancer facilement

---

## ▶️ Lancer le jeu

### ✅ Méthode rapide (recommandée)
1. Double-clique sur `LancerTout.bat`
2. Cela ouvrira automatiquement :
   - 1 console serveur
   - 2 clients graphiques (Windows Forms)

🎉 Prêt à jouer à deux !

---

## 🕹️ Règles du jeu
- Chacun joue à son tour en cliquant
- Le jeu indique :
  - Qui doit jouer
  - Si l’un gagne ou s’il y a égalité
- Bouton **"Quitter"** pour fermer

---

## 🧱 Prérequis
- Windows
- [.NET 6 SDK installé](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

---

## 🧯 Si `LancerTout.bat` ne fonctionne pas

Tu peux tout lancer manuellement en **une seule commande PowerShell** 👇

```powershell
start cmd /k "dotnet run --project MorpionServer" ; Start-Sleep -Seconds 2 ; start cmd /k "dotnet run --project MorpionClientGUI" ; Start-Sleep -Seconds 2 ; start cmd /k "dotnet run --project MorpionClientGUI"
