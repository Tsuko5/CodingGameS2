
# 🎮 Morpion Multijoueur (.NET 6)

## 📦 Contenu
- `MorpionServer` : serveur (console)
- `MorpionClientGUI` : client avec interface graphique (Windows Forms)
- `MorpionGame.sln` : solution Visual Studio (facultatif)
- `LancerTout.bat` : script pour tout lancer facilement

---

## ▶️ Lancer le jeu

### ✅ Méthode rapide (recommandée)
1. **Double-clique sur `LancerTout.bat`**
2. Cela ouvrira automatiquement :
   - 1 serveur (console)
   - 2 clients (interface graphique)

🎉 Vous pouvez jouer à deux sur le même PC !

---

## 🕹️ Règles du jeu
- Chaque joueur clique pour jouer à son tour
- Le jeu affiche :
  - Qui doit jouer
  - Le résultat à la fin (victoire / nul)
- Bouton "**Quitter**" pour fermer

---

## 🧱 Prérequis
- Windows
- .NET 6 SDK installé : https://dotnet.microsoft.com/en-us/download/dotnet/6.0

---

## ❓ En cas de bug
- Si une seule fenêtre client s’ouvre :
  - Ouvre un terminal et tape :
    ```
    dotnet run --project MorpionClientGUI
    ```
