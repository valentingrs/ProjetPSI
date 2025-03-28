using static Association.Graphique;

namespace Association
{
	internal class Association
	{
		/// Fichier contenant les manipulation sur un graphe simple
		/// Pour le rendu n°1 mais pas vraiment utile pour la suite des rendus <summary>
		/// Fichier contenant les manipulation sur un graphe simple

		static public Graphe<string> LireFichier()
		{

			string filename = ("soc-karate.mtx");
			List<string> dico = new List<string>();
			string[] lignes = File.ReadAllLines(filename);

			Graphe<string> association = new Graphe<string>(false); // non orienté car associations réciproques
																	// non pondéré également
			for (int i = 24; i <= 101; i++)
			{
				string[] ligne = lignes[i].Split(' ');

				string s0 = ligne[0];
				Noeud<string> noeud0 = new Noeud<string>("");
				bool existeDeja = false;
				foreach (Noeud<string> k in association.Noeuds)
				{
					if (k.Nom == s0) { noeud0 = k; existeDeja = true; }

				}
				if (existeDeja == false) { noeud0 = new Noeud<string>(s0); }

				existeDeja = false;
				string s1 = ligne[1];
				Noeud<string> noeud1 = new Noeud<string>("");
				foreach (Noeud<string> j in association.Noeuds)
				{
					if (j.Nom == s1) { noeud1 = j; existeDeja = true; }
				}
				if (existeDeja == false) { noeud1 = new Noeud<string>(s1); }

				Lien<string> l = new Lien<string>(noeud0, noeud1);

				if (!association.ContientSommet(noeud0)) { association.AjouterSommet(noeud0); }
				if (!association.ContientSommet(noeud1)) { association.AjouterSommet(noeud1); }

				association.AjouterLien(l);

			}
			return association;

		}

		static public void GrapheAssociation() /// fonction de test du Rendu 1
        {
            Graphe<string> association = LireFichier();
            Console.WriteLine("Liste d'adjacence : ");
            association.AfficherListeAdjacence();

            Console.Write("\nPour les parcours, rentrer un sommet (entre 1 et 34) : ");
            string res = Console.ReadLine();
            Noeud<string> s = new Noeud<string>("");
            foreach (Noeud<string> som in association.Noeuds) { if (som.Nom == res) { s = som; } }

            Console.WriteLine("\nParcours en largeur à partir du sommet " + s.Nom);
            association.ParcoursEnLargeur(s);

            Console.WriteLine("\n\n" + "Parcours en profondeur à partir du sommet " + s.Nom);
            association.ParcoursEnProfondeur(s);
            Console.WriteLine("\n");

            if (association.EstConnexe()) { Console.WriteLine("\nD'après le DFS, le graphe est connexe"); }
            else { Console.WriteLine("D'après le DFS, il n'est pas connexe"); }

            DessinerGraphe(association, "graphe.png");
            Console.WriteLine("\nGraphe affiché dans le dossier bin/Debut/7.0 du projet sous le nom graphe.png");

            string cheminImage = Path.GetFullPath("graphe.png");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = cheminImage,
                UseShellExecute = true  // Permet d'utiliser l'application par défaut pour ouvrir l'image
            });
        }

	}
}