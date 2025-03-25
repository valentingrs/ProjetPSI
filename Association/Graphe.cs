using System;
namespace Association
{
	public class Graphe
	{
		private List<Noeud> noeuds;
		private List<Lien> liens;
		private bool oriente;

		public Graphe(bool oriente)
		{
            noeuds = new List<Noeud>();
			liens = new List<Lien>();
			this.oriente = oriente;
		}

		public List<Noeud> Noeuds
		{
			get { return noeuds; }
			set { noeuds = value; }
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

		public void AjouterSommet(Noeud sommet)
		{
			if (!ContientSommet(sommet)) { noeuds.Add(sommet); }
		}

		public bool ContientSommet(Noeud sommetRecherche)
		{
			foreach (Noeud sommet in noeuds)
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
			if (!ContientLien(lien)) { liens.Add(lien); lien.Noeud1.AjouterVoisin(lien.Noeud2); }

			if (oriente == false)
			{
				Lien reciproque = new Lien(lien.Noeud2, lien.Noeud1);
				if (!ContientLien(reciproque)) { liens.Add(reciproque); reciproque.Noeud1.AjouterVoisin(reciproque.Noeud2); }
			}
		}

		public List<Lien> LiensParNoeud(Noeud noeud)
		{
			List<Lien> liensNoeud = new List<Lien>();
			foreach (Lien lien in liens)
			{
				if (lien.Noeud1 == noeud)
				{
                    liensNoeud.Add(lien);
				}
			}
			return liensNoeud;
		}

		public Dictionary<Noeud, List<Noeud>> ListeAdjacence()
		{
			Dictionary<Noeud, List<Noeud>> listeAdj = new Dictionary<Noeud, List<Noeud>>();
			foreach (Noeud sommet in noeuds)
			{
				listeAdj[sommet] = sommet.Voisins;
			}
			return listeAdj;
		}

		public void AfficherListeAdjacence()
		{
			Dictionary<Noeud, List<Noeud>> listeAdj = ListeAdjacence();  // Récupérer la liste d'adjacence
			foreach (var entry in listeAdj)
			{
				// Afficher le sommet et ses voisins

				Console.WriteLine($"{entry.Key.Nom} -> {string.Join(", ", entry.Value.Select(s => s.Nom))}");
			}
		}

		public void ParcoursEnLargeur(Noeud depart)
		{
			Queue<Noeud> file = new Queue<Noeud>(); // file de sommets FIFO

			Dictionary<Noeud, bool> visite = new Dictionary<Noeud, bool>(); // dictionnaire des marquages

			foreach (Noeud sommet in noeuds)
			{
				visite[sommet] = false;
			}

			visite[depart] = true;
			file.Enqueue(depart);

			// Parcours en largeur
			while (file.Count > 0)
			{
                Noeud sommetCourant = file.Dequeue();
				Console.Write(sommetCourant.Nom + " ");

				foreach (Noeud voisin in sommetCourant.Voisins)
				{
					if (visite.ContainsKey(voisin) && !visite[voisin])
					{
						visite[voisin] = true;
						file.Enqueue(voisin);
					}
				}
			}
		}

		public void ParcoursEnProfondeur(Noeud depart)
		{
			// Utilisation d'un dictionnaire pour garder une trace des sommets visités
			Dictionary<Noeud, bool> visite = new Dictionary<Noeud, bool>();

			// Initialisation
			foreach (Noeud sommet in noeuds)
			{
				visite[sommet] = false;
			}

			// Appel récursif pour le parcours en profondeur
			ParcoursEnProfondeurRecursif(depart, visite);
		}

		private void ParcoursEnProfondeurRecursif(Noeud sommetCourant, Dictionary<Noeud, bool> visite)
		{
			// Marquer le sommet courant comme visité
			visite[sommetCourant] = true;
			Console.Write(sommetCourant.Nom + " ");

			// Parcourir tous les voisins du sommet courant
			foreach (Noeud voisin in sommetCourant.Voisins)
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

            Noeud sommetDeDepart = noeuds[0];

			Dictionary<Noeud, bool> visite = new Dictionary<Noeud, bool>();

			foreach (Noeud sommet in noeuds)
			{
				visite[sommet] = false;
			}

			ParcoursEnProfondeurRecursif(sommetDeDepart, visite);

			int sommetsVisites = visite.Values.Count(v => v == true); 
			return sommetsVisites == noeuds.Count;
		}
	}
}