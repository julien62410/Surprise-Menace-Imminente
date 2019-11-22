# Ghost in the Tel

Ghost in the Tel est un jeu solo en AR. Le joueur bouge et rotate son téléphone pour évoluer dans l’environnement.
A intervalle de plus en plus fréquents, des fantômes apparaissent à 360° autour de lui.
Ils vont ensuite se déplacer lentement en direction du joueur.
Si un fantôme entre en contact avec le joueur il perd de la vie, s'il perd toute sa vie, c’est le Game over.

Pour les neutraliser, il doit les garder dans son champ de vision un certain temps, le brûlant ainsi du regard.
Tuer un fantôme rapporte des points, le but est de scorer un maximum.

Le cercle de spawn des fantômes suit le joueur, cependant, une fois spawné, sa position n'est pas altérée par les mouvements du joueur,
il essaie simplement de le suivre du mieux qu’il peut.


Difficulté
La difficulté augmente au fur et à mesure de la partie.
La plupart des variables du jeu sont dépendantes de cette difficulté, possédant une valeur minimale et maximale,
qui est ensuite interpolée selon la valeur actuelle de difficulté.


Problème connue :

	- Perte possible du Tracking de la caméra.
	- Spawn possible des éléments de gameplay dans les murs.


Gameplay :

	- Tuer les fantômes en les reagardant (consomme de la batterie).
	- Récupérer les batteries en allant dessus (spawn quand batterie courante faible).
	- De même pour les coeurs (spawn quand on découvre une surface).
	- Jeu infinie - But : obtenir le meilleur score possible.


Surprise : 

	- Le joueur peut se faire surprendre par les sons joués lorsqu'un fantôme lui arrive dans le dos.
	- Les objets qui spawnent peuvent êtres assimilé à des surprises (ex: coeur)


Menace Imminente:

	- Le joueur n'a pas de répis, les fantômes peuvent venir de partout
	- La difficulté augmente
	- La batterie du joueur se vide, urgence de trouver une recharge!