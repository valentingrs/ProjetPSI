using static Association.Graphique;
using static Association.PlusCourtChemin;

namespace Association
{
	internal class Association
	{
		/// Fichier contenant les manipulation sur un graphe simple
		/// Pour le rendu n�1 mais pas vraiment utile pour la suite des rendus <summary>
		/// Fichier contenant les manipulation sur un graphe simple

		static public Graphe<string> LireFichier()
		{

			string filename = ("soc-karate.mtx");
			List<string> dico = new List<string>();
			string[] lignes = File.ReadAllLines(filename);

			Graphe<string> association = new Graphe<string>(false); // non orient� car associations r�ciproques
																	// non pond�r� �galement
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

            Console.WriteLine("\nParcours en largeur � partir du sommet " + s.Nom);
            association.ParcoursEnLargeur(s);

            Console.WriteLine("\n\n" + "Parcours en profondeur � partir du sommet " + s.Nom);
            association.ParcoursEnProfondeur(s);
            Console.WriteLine("\n");

            if (association.EstConnexe()) { Console.WriteLine("\nD'apr�s le DFS, le graphe est connexe"); }
            else { Console.WriteLine("D'apr�s le DFS, il n'est pas connexe"); }

            DessinerGraphe(association, "graphe.png");
            Console.WriteLine("\nGraphe affich� dans le dossier bin/Debut/7.0 du projet sous le nom graphe.png");

            string cheminImage = Path.GetFullPath("graphe.png");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = cheminImage,
                UseShellExecute = true  // Permet d'utiliser l'application par d�faut pour ouvrir l'image
            });
        }

        static public void GrapheSimpleTest()
        {
            Graphe<int> g = new Graphe<int>(true);
            Noeud<int> n1 = new Noeud<int>(1); g.AjouterSommet(n1);
            Noeud<int> n2 = new Noeud<int>(2); g.AjouterSommet(n2);
            Noeud<int> n3 = new Noeud<int>(3); g.AjouterSommet(n3);
            Noeud<int> n4 = new Noeud<int>(4); g.AjouterSommet(n4);
            Noeud<int> n5 = new Noeud<int>(5); g.AjouterSommet(n5);
            g.AjouterLien(new Lien<int>(n1, n2, 3));
            g.AjouterLien(new Lien<int>(n1, n3, 8));
            g.AjouterLien(new Lien<int>(n1, n5, 4));
            g.AjouterLien(new Lien<int>(n2, n4, 1));
            g.AjouterLien(new Lien<int>(n2, n5, 7));
            g.AjouterLien(new Lien<int>(n3, n2, 4));
            g.AjouterLien(new Lien<int>(n4, n3, 5));
            g.AjouterLien(new Lien<int>(n4, n1, 2));
            g.AjouterLien(new Lien<int>(n5, n4, 6));

            Console.WriteLine(g.Pondere());
            double?[,] matriceAdj = g.MatriceAdjacence();
            for (int i = 0; i < matriceAdj.GetLength(0); i++)
            {
                for (int j = 0; j < matriceAdj.GetLength(1); j++)
                {
                    Console.Write(matriceAdj[i, j] + " ");
                }
                Console.WriteLine();
            }

            DessinerGraphe(g, "graphe.png");

            //Dijkstra(g, g.Noeuds[2], g.Noeuds[4]);
            //FloydWarshall(g, 3, 5);
            //Bellman_Ford(g, g.Noeuds[2], g.Noeuds[4]);
            // Grosse beuteu
            qsgciudgciuvgovg
        }
    }
}