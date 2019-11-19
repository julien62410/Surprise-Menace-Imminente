# Surprise-Menace-Imminente

SURPRISE est un jeu solo en AR. Le joueur bouge et rotate son téléphone pour évoluer dans l’environnement. A intervalle de plus en plus fréquents, des fantômes apparaissent à 360° autour de lui. Ils vont ensuite se déplacer lentement en direction du joueur. Si un fantôme entre en contact avec le joueur, c’est le Game over.

Pour les neutraliser, il doit les garder dans son champ de vision un certain temps, le brûlant ainsi du regard. Tuer un fantôme rapporte un point, le but est de scorer un maximum.

Le cercle de spawn des fantômes suit le joueur, cependant un fantôme, une fois spawné, ne voit pas sa position altérée par les mouvements du joueur, il essaie simplement de le suivre du mieux qu’il peut.

Difficulté
La difficulté est un nombre entre 0 et 1, qui augmente au fur et à mesure de la partie.
La plupart des variables du jeu sont dépendantes de cette difficulté, possédant une valeur minimale et maximale, qui est ensuite interpolée selon la valeur actuelle de difficulté.

Par exemple, on pourra avoir une variable de Spawn rate, qui pourra être à 0.2/seconde au début et 0.8/seconde une fois la difficulté maximale atteinte. Quand la difficulté actuelle est à 0.5, le spawn rate sera à 0.5/sec.


