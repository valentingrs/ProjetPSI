using System;
namespace Association
{
	public class Graphe<T>
	{
		private List<Noeud<T>> noeuds;
		private List<Lien<T>> liens;
		private bool oriente;

		public Graphe(bool oriente)
		{
            noeuds = new List<Noeud<T>>();
			liens = new List<Lien<T>> ();
			this.oriente = oriente;
		}

		public List<Noeud<T>> Noeuds
		{
			get { return noeuds; }
			set { noeuds = value; }
		}

		public List<Lien<T>> Liens
		{
			get { return liens; }
			set { liens = value; }
		}
		public bool Oriente
		{
			get { return oriente; }
			set { oriente = value; }
		}

		public void AjouterSommet(Noeud<T> sommet)
		{
			if (!ContientSommet(sommet)) { noeuds.Add(sommet); }
		}

		public bool ContientSommet(Noeud<T> sommetRecherche)
		{
			foreach (Noeud<T> sommet in noeuds)
			{
				if (sommet == sommetRecherche) { return true; }
			}
			return false;
		}
		public bool ContientLien(Lien<T> lienRecherche)
		{
			foreach (Lien<T> lien in liens)
			{
				if (lien == lienRecherche) { return true; }
			}
			return false;
		}

		public Lien<T> IdentifierLien(Noeud<T> noeud1, Noeud<T> noeud2)
		// trouver un lien dans le graphe entre deux sommets données
		{
			List<Lien<T>> liensNoeud1 = LiensParNoeud(noeud1);
			Lien<T> lienReturn = new Lien<T>(null, null);
			foreach (Lien<T> lien in liensNoeud1)
			{
				lienReturn = lien;
				if (lien.Noeud2 == noeud2) { return lienReturn; }
			}

			Console.WriteLine("Pas de lien trouvé");
			return lienReturn;
		}

		public Noeud<T> IdentifierNoeud(T nom)
		{
			Noeud<T> noeudTrouve;
			foreach(Noeud<T> noeud in this.Noeuds)
			{
				if (noeud.Nom.Equals(nom)) {  return noeud; }
			}
			Console.WriteLine("Pas de noeud trouve.");
            noeudTrouve = null;
			return noeudTrouve;
		}

		public void AjouterLien(Lien<T> lien)
		{
			if (!ContientLien(lien)) { liens.Add(lien); lien.Noeud1.AjouterVoisin(lien.Noeud2); }

			if (oriente == false)
			{
				Lien<T> reciproque = new Lien<T>(lien.Noeud2, lien.Noeud1);
				if (!ContientLien(reciproque)) { liens.Add(reciproque); reciproque.Noeud1.AjouterVoisin(reciproque.Noeud2); }
			}
		}

		public List<Lien<T>> LiensParNoeud(Noeud<T> noeud)
		{
			List<Lien<T>> liensNoeud = new List<Lien<T>> ();
			foreach (Lien<T> lien in liens)
			{
				if (lien.Noeud1 == noeud)
				{
                    liensNoeud.Add(lien);
				}
			}
			return liensNoeud;
		}

		public Dictionary<Noeud<T>, List<Noeud<T>>> ListeAdjacence()
		{
			Dictionary<Noeud<T>, List<Noeud<T>>> listeAdj = new Dictionary<Noeud<T>, List<Noeud<T>>>();
			foreach (Noeud<T> sommet in noeuds)
			{
				listeAdj[sommet] = sommet.Voisins;
			}
			return listeAdj;
		}

		public void AfficherListeAdjacence()
		{
			Dictionary<Noeud<T>, List<Noeud<T>>> listeAdj = ListeAdjacence();  // Récupérer la liste d'adjacence
			foreach (var entry in listeAdj)
			{
				// Afficher le sommet et ses voisins

				Console.WriteLine($"{entry.Key.Nom} -> {string.Join(", ", entry.Value.Select(s => s.Nom))}");
			}
		}

		public void ParcoursEnLargeur(Noeud<T> depart)
		{
			Queue<Noeud<T>> file = new Queue<Noeud<T>>(); // file de sommets FIFO

			Dictionary<Noeud<T>, bool> visite = new Dictionary<Noeud<T>, bool>(); // dictionnaire des marquages

			foreach (Noeud<T> sommet in noeuds)
			{
				visite[sommet] = false;
			}

			visite[depart] = true;
			file.Enqueue(depart);

			// Parcours en largeur
			while (file.Count > 0)
			{
                Noeud<T> sommetCourant = file.Dequeue();
				Console.Write(sommetCourant.Nom + " ");

				foreach (Noeud<T> voisin in sommetCourant.Voisins)
				{
					if (visite.ContainsKey(voisin) && !visite[voisin])
					{
						visite[voisin] = true;
						file.Enqueue(voisin);
					}
				}
			}
		}

		public void ParcoursEnProfondeur(Noeud<T> depart)
		{
			// Utilisation d'un dictionnaire pour garder une trace des sommets visités
			Dictionary<Noeud<T>, bool> visite = new Dictionary<Noeud<T>, bool>();

			// Initialisation
			foreach (Noeud<T> sommet in noeuds)
			{
				visite[sommet] = false;
			}

			// Appel récursif pour le parcours en profondeur
			ParcoursEnProfondeurRecursif(depart, visite);
		}

		private void ParcoursEnProfondeurRecursif(Noeud<T> sommetCourant, Dictionary<Noeud<T>, bool> visite)
		{
			// Marquer le sommet courant comme visité
			visite[sommetCourant] = true;
			Console.Write(sommetCourant.Nom + " ");

			// Parcourir tous les voisins du sommet courant
			foreach (Noeud<T> voisin in sommetCourant.Voisins)
			{
				if (visite.ContainsKey(voisin) && !visite[voisin])
				{
					ParcoursEnProfondeurRecursif(voisin, visite);
				}
			}
		}

		public bool EstConnexe() // utilsiation du dfs
		{
			if (noeuds.Count == 0) return true;

            Noeud<T> sommetDeDepart = noeuds[0];

			Dictionary<Noeud<T>, bool> visite = new Dictionary<Noeud<T>, bool>();

			foreach (Noeud<T> sommet in noeuds)
			{
				visite[sommet] = false;
			}

			ParcoursEnProfondeurRecursif(sommetDeDepart, visite);

			int sommetsVisites = visite.Values.Count(v => v == true); 
			return sommetsVisites == noeuds.Count;
		}
	}
}