using static Association.Graphique;


namespace Association
{
    internal class Program
    {
        static void Association()
        {
            Graphe<string> association = Graphique.LireFichier();
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
        }

        static void TestIdentifierLien()
        {
            Graphe<int> graphe = new Graphe<int>(true);

            Noeud<int> n1 = new Noeud<int>(1); graphe.AjouterSommet(n1);
            Noeud<int> n2 = new Noeud<int>(2); graphe.AjouterSommet(n2);
            Noeud<int> n3 = new Noeud<int>(3); graphe.AjouterSommet(n3);
            Noeud<int> n4 = new Noeud<int>(4); graphe.AjouterSommet(n4);
            Lien<int> l1 = new Lien<int>(n1, n2); graphe.AjouterLien(l1);
            Lien<int> l2 = new Lien<int>(n1, n4); graphe.AjouterLien(l2);
            Lien<int> l3 = new Lien<int>(n2, n3); graphe.AjouterLien(l3);
            Lien<int> l4 = new Lien<int>(n3, n4); graphe.AjouterLien(l4);

            Lien<int> lienid = graphe.IdentifierLien(n3, n4);
            Console.WriteLine(lienid);
        }

        static void Main(string[] args)
        {
            


        }
        
    }
}