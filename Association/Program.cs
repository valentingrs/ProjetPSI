using System.Globalization;
using OfficeOpenXml;
using static Association.GrapheStation;
using static Association.PlusCourtChemin;
using static Association.Graphique;
using static Association.Interface;

namespace Association
{
    internal class Program
    {
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
        }

        public static void Main(string[] args)
        {
            //MetroParis();
            //GrapheSimpleTest();
            GererInterface();
        }
    }
}