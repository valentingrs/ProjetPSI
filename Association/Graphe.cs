using Google.Protobuf.Collections;
using Org.BouncyCastle.Asn1.Cmp;
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

        #region Proprietes
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

        #endregion

        #region Methodes de base
        /// Méthodes de base pour manipuler des éléments du graphe
        public void AjouterSommet(Noeud<T> sommet)
		{
			if (!ContientSommet(sommet)) { noeuds.Add(sommet); }
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
        public bool Pondere()
        {
            foreach (Lien<T> lien in this.liens)
            {
                if (lien.Poids != null) { return true; }
            }
            return false;
        }

        public void AfficherNoeuds() /// affiche l'ensemble des noeuds du graphe
		{
			foreach (Noeud<T> noeud in noeuds) { Console.WriteLine(noeud); }
		}

        public void AfficherLiens() /// affiche l'ensemble des liens du graphe
        {
            foreach (Lien<T> lien in liens) { Console.WriteLine(lien); }
        }

		public int Taille()
		{
			return noeuds.Count();
		}
        #endregion

        #region Methodes utiles
        /// Méthodes plus approfondies et/ou utiles pour d'autres méthodes
        public Noeud<T> IdentifierNoeud(T nom)
		/// identifie un noeud à partir d'un élément de type rentré (une station par exemple)
        {
            Noeud<T> noeudTrouve;
            foreach (Noeud<T> noeud in this.Noeuds)
            {
                if (noeud.Nom.Equals(nom)) { return noeud; }
            }
            Console.WriteLine("Pas de noeud trouve.");
            noeudTrouve = null;
            return noeudTrouve;
        }

        public Lien<T> IdentifierLien(Noeud<T> noeud1, Noeud<T> noeud2)
		/// trouver un lien dans le graphe entre deux sommets données
		{
			List<Lien<T>> liensNoeud1 = LiensParNoeud(noeud1);
			foreach (Lien<T> lien in liensNoeud1)
			{
				if (lien.Noeud2 == noeud2) { return lien; }
			}
			return null;
		}

		public List<Lien<T>> LiensParNoeud(Noeud<T> noeud)
		/// retourne les liens partant de ce noeud
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

		public List<Noeud<T>> ListeDegreDecroissant()
		{
			/// liste des sommets par ordre décroissant de degré
			/// Utile pour l'algorithme de Welsh-Powell
			List<Noeud<T>> listeSommets = new List<Noeud<T>>();
			foreach (Noeud<T> noeud in noeuds) { listeSommets.Add(noeud); }

			/// On utilise un tri par séléction pour ne pas trop alourdir le code
			for (int i = 0; i < listeSommets.Count - 1; i++)
			{
				int indiceMin = i;
				for (int j = i + 1; j < listeSommets.Count; j++)
				{
					if (listeSommets[j].Degre() >= listeSommets[i].Degre()) 
					{
						indiceMin = j;

						Noeud<T> temp = listeSommets[i];
						listeSommets[i] = listeSommets[indiceMin];
						listeSommets[indiceMin] = temp;
					}
				}
			}
			return listeSommets;
		}

        #endregion

        #region Affichage
        /// Affichage du graphe
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
			Dictionary<Noeud<T>, List<Noeud<T>>> listeAdj = ListeAdjacence();  /// Récupérer la liste d'adjacence
			foreach (var entry in listeAdj)
			{
				/// Afficher le sommet et ses voisins

				Console.WriteLine($"{entry.Key.Nom} -> {string.Join(", ", entry.Value.Select(s => s.Nom))}");
			}
		}
        public double?[,] MatriceAdjacence()
        {
            /// Stocke le nombre de noeud de G dans V
            int V = noeuds.Count;

            double?[,] matrice_adjacence = new double?[V, V]; /// initialise une matrice de double?

            int i = 0;
			bool estPondere = Pondere();

            foreach (Noeud<T> n1 in noeuds)
            {
                int j = 0;
                foreach (Noeud<T> n2 in noeuds)
                {
					Lien<T> lien = IdentifierLien(n1, n2);
                    matrice_adjacence[i, j] = (lien != null) ? (estPondere ? lien.Poids : 1) : 0;
                    
                    j++;
                }
                i++;
            }
            return matrice_adjacence;
        }
        #endregion


        #region Exploitation graphe
        /// Exploitation du graphe : parcours, distances, connexité, ...
        public void ParcoursEnLargeur(Noeud<T> depart)
		{
			Queue<Noeud<T>> file = new Queue<Noeud<T>>(); /// file de sommets FIFO

			Dictionary<Noeud<T>, bool> visite = new Dictionary<Noeud<T>, bool>(); /// dictionnaire des marquages

			foreach (Noeud<T> sommet in noeuds)
			{
				visite[sommet] = false;
			}

			visite[depart] = true;
			file.Enqueue(depart);

			/// Parcours en largeur
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
			/// Utilisation d'un dictionnaire pour garder une trace des sommets visités
			Dictionary<Noeud<T>, bool> visite = new Dictionary<Noeud<T>, bool>();

			/// Initialisation
			foreach (Noeud<T> sommet in noeuds)
			{
				visite[sommet] = false;
			}

			/// Appel récursif pour le parcours en profondeur
			ParcoursEnProfondeurRecursif(depart, visite);
		}

		private void ParcoursEnProfondeurRecursif(Noeud<T> sommetCourant, Dictionary<Noeud<T>, bool> visite)
		{
			/// Marquer le sommet courant comme visité
			visite[sommetCourant] = true;

			/// Parcourir tous les voisins du sommet courant
			foreach (Noeud<T> voisin in sommetCourant.Voisins)
			{
				if (visite.ContainsKey(voisin) && !visite[voisin])
				{
					ParcoursEnProfondeurRecursif(voisin, visite);
				}
			}
		}

		public bool EstConnexe() /// utilsiation du dfs
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

        public bool EstBiparti()
		{
			return NbChromatique() == 2;
		}

		public bool EstPlanaire()
		{
			return NbChromatique() <= 4; /// plutôt regarder en fcontion des cas
			/// et au pire on teste la coloration pour 4 couleurs
		}

        public Dictionary<Noeud<T>, int> Coloration()
		{
			/// Coloration du graphe en utilisant l'algorithme de Welsh-Powell
			/// Dictionary<Noeud<T>, int> : associe à un sommet une couleur correspodant à un entier
			Dictionary<Noeud<T>, int> coloration = new Dictionary<Noeud<T>, int>();

			if (oriente == false)
			{
				/// coloriable si et seulement si le graphe est non-orienté
				List<Noeud<T>> listeSommetsDec = ListeDegreDecroissant();
				int couleurCourante = 0;

				while (listeSommetsDec.Count != 0)
				{
					couleurCourante++;
					Noeud<T> sommetTraite = listeSommetsDec[0];

					coloration.Add(sommetTraite, couleurCourante); /// colorier le sommet avec la couleur courante
					listeSommetsDec.Remove(sommetTraite); /// on élimine le sommet de la liste des sommets à colorier
					List<Noeud<T>> voisins = sommetTraite.Voisins; /// liste des voisins du sommet traite

					foreach (Noeud<T> sommet in listeSommetsDec)
					{
						if (!voisins.Contains(sommet)) /// si le sommet n'est pas adjacent au sommet qu'on traite
						{

                            coloration.Add(sommet, couleurCourante); /// alors on le colorie avec la couleur courante

							voisins.AddRange(sommet.Voisins);
						}
					}

					foreach (Noeud<T> sommetColorie in coloration.Keys)
					{
						listeSommetsDec.Remove(sommetColorie); /// éliminer les sommets coloriés de la liste des sommets à colorier
					}
				}
			}

            return coloration;
		}

		public int NbChromatique()
		{
			Dictionary<Noeud<T>, int> coloration = Coloration();
			int nbCouleurs = 0;

			foreach (int couleur in coloration.Values)
			{
				if (couleur > nbCouleurs) {  nbCouleurs = couleur;}
			}
			return nbCouleurs;
		}

        #endregion
    }
}