using System;
namespace Association
{
	public class Graphe
	{
		private List<Sommet> sommets;
		private List<Lien> liens;
		private bool oriente;

		public Graphe(bool oriente)
		{
			sommets = new List<Sommet>();
			liens = new List<Lien>();
			this.oriente = oriente;
		}

		public List<Sommet> Sommets
		{
			get { return sommets; }
			set { sommets = value; }
		}

		public List<Lien> Liens
		{
			get { return liens; }
			set { liens = value; }
		}
		public bool Oriente
		{
			get { return oriente; }
			set { oriente = value; }
		}

		public void AjouterSommet(Sommet sommet)
		{
			if (!ContientSommet(sommet)) { sommets.Add(sommet); }
		}

		public bool ContientSommet(Sommet sommetRecherche)
		{
			foreach (Sommet sommet in sommets)
			{
				if (sommet == sommetRecherche) { return true; }
			}
			return false;
		}
		public bool ContientLien(Lien lienRecherche)
		{
			foreach (Lien lien in liens)
			{
				if (lien == lienRecherche) { return true; }
			}
			return false;
		}



		public void AjouterLien(Lien lien)
		{
			if (!ContientLien(lien)) { liens.Add(lien); lien.Sommet1.AjouterVoisin(lien.Sommet2); }

			if (oriente == false)
			{
				Lien reciproque = new Lien(lien.Sommet2, lien.Sommet1);
				if (!ContientLien(reciproque)) { liens.Add(reciproque); reciproque.Sommet1.AjouterVoisin(reciproque.Sommet2); }
			}
		}

		public List<Lien> LiensParSommet(Sommet sommet)
		{
			List<Lien> liensSommet = new List<Lien>();
			foreach (Lien lien in liens)
			{
				if (lien.Sommet1 == sommet)
				{
					liensSommet.Add(lien);
				}
			}
			return liensSommet;
		}

		public Dictionary<Sommet, List<Sommet>> ListeAdjacence()
		{
			Dictionary<Sommet, List<Sommet>> listeAdj = new Dictionary<Sommet, List<Sommet>>();
			foreach (Sommet sommet in sommets)
			{
				listeAdj[sommet] = sommet.Voisins;
			}
			return listeAdj;
		}

		public void AfficherListeAdjacence()
		{
			Dictionary<Sommet, List<Sommet>> listeAdj = ListeAdjacence();  // Récupérer la liste d'adjacence
			foreach (var entry in listeAdj)
			{
				// Afficher le sommet et ses voisins

				Console.WriteLine($"{entry.Key.Nom} -> {string.Join(", ", entry.Value.Select(s => s.Nom))}");
			}
		}

		public void ParcoursEnLargeur(Sommet depart)
		{
			Queue<Sommet> file = new Queue<Sommet>(); // file de sommets FIFO

			Dictionary<Sommet, bool> visite = new Dictionary<Sommet, bool>(); // dictionnaire des marquages

			foreach (Sommet sommet in sommets)
			{
				visite[sommet] = false;
			}

			visite[depart] = true;
			file.Enqueue(depart);

			// Parcours en largeur
			while (file.Count > 0)
			{
				Sommet sommetCourant = file.Dequeue();
				Console.Write(sommetCourant.Nom + " ");

				foreach (Sommet voisin in sommetCourant.Voisins)
				{
					if (visite.ContainsKey(voisin) && !visite[voisin])
					{
						visite[voisin] = true;
						file.Enqueue(voisin);
					}
				}
			}
		}

		public void ParcoursEnProfondeur(Sommet depart)
		{
			// Utilisation d'un dictionnaire pour garder une trace des sommets visités
			Dictionary<Sommet, bool> visite = new Dictionary<Sommet, bool>();

			// Initialisation
			foreach (Sommet sommet in sommets)
			{
				visite[sommet] = false;
			}

			// Appel récursif pour le parcours en profondeur
			ParcoursEnProfondeurRecursif(depart, visite);
		}

		private void ParcoursEnProfondeurRecursif(Sommet sommetCourant, Dictionary<Sommet, bool> visite)
		{
			// Marquer le sommet courant comme visité
			visite[sommetCourant] = true;
			Console.Write(sommetCourant.Nom + " ");

			// Parcourir tous les voisins du sommet courant
			foreach (Sommet voisin in sommetCourant.Voisins)
			{
				if (visite.ContainsKey(voisin) && !visite[voisin])
				{
					ParcoursEnProfondeurRecursif(voisin, visite);
				}
			}
		}

		public bool EstConnexe() // utilsiation du dfs
		{
			if (sommets.Count == 0) return true; 

			Sommet sommetDeDepart = sommets[0];

			Dictionary<Sommet, bool> visite = new Dictionary<Sommet, bool>();

			foreach (Sommet sommet in sommets)
			{
				visite[sommet] = false;
			}

			ParcoursEnProfondeurRecursif(sommetDeDepart, visite);

			int sommetsVisites = visite.Values.Count(v => v == true); 
			return sommetsVisites == sommets.Count;
		}
	}
}