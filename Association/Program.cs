namespace Association
{
    internal class Program
    {
        public class Noeud
        {
            private int sommet;


            public Noeud(int sommet)
            {
                this.sommet = sommet;
            }
        }

        public class Lien
        {
            private bool lien;
            private Noeud noeud1;
            private Noeud noeud2;

            public Lien(bool lien, Noeud noeud1, Noeud noeud2)
            {
                this.lien = lien;
                this.noeud1 = noeud1;
                this.noeud2 = noeud2;
            }

            public bool CreerLien(Noeud initial, Noeud final)
            {

            }
        }

        public class Graphe
        {


        }
        static void Main(string[] args)
        {
            Console.WriteLine("Test");
        }
    }
}