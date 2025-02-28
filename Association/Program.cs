using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using static Association.Program;

namespace Association
{
    internal class Program
    {
        public class Sommet
        {
            private string nom;
            private bool marque;
            private List<Sommet> voisins;

            public Sommet(string nom)
            {
                voisins = new List<Sommet>();
                this.nom = nom;
            }

            public string Nom
            {
                get { return nom; }
            }


            public void ajouterVoisin(Sommet s)
            {
                voisins.Add(s);
            }

            public void Marquer()
            {
                marque = true;
            }

            public string AfficherSommet()
            {
                string s = nom + " - { ";
                for (int i = 0; i < voisins.Count - 1; i++)
                {
                    nom = nom + voisins[i] + ", ";
                }
                int j = voisins.Count - 1;
                nom = nom + voisins[j];
                return s;
            }

            public override string ToString()
            {
                return nom;
            }

            public int Degre()
            {
                return voisins.Count();
            }


        }

        public class Lien
        {
            private Sommet sommet1;
            private Sommet sommet2;
            //private int poids;

            public Lien(Sommet sommet1, Sommet sommet2) //int poids
            {
                this.sommet1 = sommet1;
                this.sommet2 = sommet2;
                sommet1.ajouterVoisin(sommet2);
                sommet2.ajouterVoisin(sommet1);
                //this.poids =poids;
            }

            public Sommet Sommet1
            {
                get { return sommet1; }
            }

            public Sommet Sommet2
            {
                get { return sommet2; }
            }
            
            public override string ToString()
            {
                string s = sommet1.Nom + " - " + sommet2.Nom;
                return s;
            }
        }

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

            public bool Oriente
            {
                get { return oriente; }
                set { oriente = value; }
            }

            public void AjouterSommet(Sommet sommet)
            {
                sommets.Add(sommet);
            }

            public void AjouterLien(Lien lien)
            {
                liens.Add(lien);
                if (oriente == false)
                {
                    Lien reciproque = new Lien(lien.Sommet2, lien.Sommet1);
                    liens.Add(reciproque);
                }
            }

            public void SupprimerLien(Lien lienSupp)
            {
                foreach(Lien lien in liens)
                {
                    if (lien == lienSupp)
                    {
                        liens.Remove(lien);
                        if (oriente == false)
                        {
                            Lien reciproque = new Lien(lien.Sommet2, lien.Sommet1);
                            liens.Remove(reciproque);
                        }
                    }
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

            public override string ToString()
            {
                string s = "";
                foreach (Sommet sommet in sommets)
                {
                    s = s + sommet.Nom + " - {";
                    List<Lien> liensSommet = LiensParSommet(sommet);
                    for (int i = 0; i < liensSommet.Count; i++)
                    {
                        s = s + liensSommet[i].Sommet2.ToString() + ", ";
                    }
                    s = s + "}\n";
                }
                return s;
            }
        }

        static void TestGrapheOriente()
        {
            List<Sommet> sommets = new List<Sommet>();
            List<Lien> liens = new List<Lien>();
            Graphe graphe = new Graphe(true);
            Sommet A = new Sommet("A"); graphe.AjouterSommet(A);
            Sommet B = new Sommet("B"); graphe.AjouterSommet(B);
            Sommet C = new Sommet("C"); graphe.AjouterSommet(C);
            Sommet D = new Sommet("D"); graphe.AjouterSommet(D);
            Sommet E = new Sommet("E"); graphe.AjouterSommet(E);
            Sommet F = new Sommet("F"); graphe.AjouterSommet(F);
            Lien lienAB = new Lien(A, B); graphe.AjouterLien(lienAB);
            Lien lienAC = new Lien(A, C); graphe.AjouterLien(lienAC);
            Lien lienBD = new Lien(B, D); graphe.AjouterLien(lienBD);
            Lien lienCB = new Lien(C, B); graphe.AjouterLien(lienCB);
            Lien lienCD = new Lien(C, D); graphe.AjouterLien(lienCD);
            Lien lienDB = new Lien(D, B); graphe.AjouterLien(lienDB);
            Lien lienEF = new Lien(E, F); graphe.AjouterLien(lienEF);
            Lien lienFE = new Lien(F, E); graphe.AjouterLien(lienFE);
            Console.WriteLine(A);
            Console.WriteLine(E);
            Console.WriteLine(lienAB);
            Console.WriteLine("Graphe orienté : ");
            Console.WriteLine(graphe);
        }

        static void TestGrapheNonOriente()
        {
            List<Sommet> sommets = new List<Sommet>();
            List<Lien> liens = new List<Lien>();
            Graphe graphe = new Graphe(false);
            Sommet A = new Sommet("A"); graphe.AjouterSommet(A);
            Sommet B = new Sommet("B"); graphe.AjouterSommet(B);
            Sommet C = new Sommet("C"); graphe.AjouterSommet(C);
            Sommet D = new Sommet("D"); graphe.AjouterSommet(D);
            Sommet E = new Sommet("E"); graphe.AjouterSommet(E);
            Sommet F = new Sommet("F"); graphe.AjouterSommet(F);
            Lien lienAB = new Lien(A, B); graphe.AjouterLien(lienAB);
            Lien lienAC = new Lien(A, C); graphe.AjouterLien(lienAC);
            Lien lienBD = new Lien(B, D); graphe.AjouterLien(lienBD);
            Lien lienCB = new Lien(C, B); graphe.AjouterLien(lienCB);
            Lien lienCD = new Lien(C, D); graphe.AjouterLien(lienCD);
            Lien lienEF = new Lien(E, F); graphe.AjouterLien(lienEF);
            Lien lienFE = new Lien(F, E); graphe.AjouterLien(lienFE);
            Console.WriteLine("Graphe non orienté : ");
            Console.WriteLine(graphe);
        }


        static void Main(string[] args)
        {
            TestGrapheOriente();
            TestGrapheNonOriente();

        }
    }
}