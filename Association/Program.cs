using System.Globalization;
using OfficeOpenXml;
using static Association.GrapheStation;
using static Association.PlusCourtChemin;
using static Association.Graphique;


namespace Association
{
    internal class Program
    {
        static public Graphe<Station> LireStationMetro(string filename) // extrait du fichier MetroParis toutes les stations de métro de Paris
        {
            Graphe<Station> grapheParis = new Graphe<Station>(true); // initialisation du graphe du métro parisien
            List<Station> stationsParis = new List<Station>(); // liste des stations de paris
            Noeud<Station> noeudStation;


            if (!File.Exists(filename)) { Console.WriteLine("Le fichier n'existe pas"); }

            FileInfo fileinfo = new FileInfo(filename);
            using (ExcelPackage package = new ExcelPackage(fileinfo)) // lecture du fichier xlsx
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[0]; // lecture de la première feuille excel -> stations de métro (noeuds)
                ExcelWorksheet worksheet2 = package.Workbook.Worksheets[1]; // lecture de la première feuille excel -> lien entre les stations (arcs)

                // lecture des données de la feuille
                int rowCount1 = worksheet1.Dimension.Rows; // nombre de lignes = nombre de stations
                int rowCount2 = worksheet2.Dimension.Rows;

                // on connait la forme du fichier Excel du métro parisien
                for (int row = 2; row <= rowCount1; row++)
                {
                    Station s = new Station(int.Parse(worksheet1.Cells[row, 1].Text),
                        worksheet1.Cells[row, 2].Text,
                        worksheet1.Cells[row, 3].Text,
                        double.Parse(worksheet1.Cells[row, 4].Text, CultureInfo.InvariantCulture),
                        double.Parse(worksheet1.Cells[row, 5].Text, CultureInfo.InvariantCulture),
                        worksheet1.Cells[row, 6].Text);

                    noeudStation = new Noeud<Station>(s); // "conversion" de la station traitée en noeud
                    stationsParis.Add(s); // ajout de la station à la liste des stations de Paris
                    grapheParis.AjouterSommet(noeudStation); // on ajoute la station au plan du métro parisien
                }

                Console.WriteLine(rowCount2);
                for (int row = 2; row <= rowCount2; row++)
                {

                    string stat = worksheet2.Cells[row, 1].Text;
                    Station statActuelle = IdentifierStationId(stationsParis, Int32.Parse(stat));
                    Noeud<Station> noeudActuel = grapheParis.IdentifierNoeud(statActuelle);

                    //Console.WriteLine(statActuelle);
                    string prec = worksheet2.Cells[row, 3].Text;
                    if (prec != "")
                    {
                        Station statPrec = IdentifierStationId(stationsParis, Int32.Parse(prec));
                        Noeud<Station> noeudPrec = grapheParis.IdentifierNoeud(statPrec);
                        int temps = Int32.Parse(worksheet2.Cells[row, 5].Text);
                        Lien<Station> lienPrecActuelle = new Lien<Station>(noeudPrec, noeudActuel, temps);
                        grapheParis.AjouterLien(lienPrecActuelle);

                        string bidirectionnel = worksheet2.Cells[row, 7].Text;
                        if (bidirectionnel == "1")
                        {
                            Lien<Station> lienInverse = new Lien<Station>(noeudActuel, noeudPrec, temps);
                            grapheParis.AjouterLien(lienInverse);
                        }
                        
                    }

                    string suiv = worksheet2.Cells[row, 4].Text;
                    if (suiv != "") 
                    { 
                        Station statSuiv = IdentifierStationId(stationsParis, Int32.Parse(suiv));
                        Noeud<Station> noeudSuiv = grapheParis.IdentifierNoeud(statSuiv);
                        int temps = Int32.Parse(worksheet2.Cells[row, 6].Text);
                        Lien<Station> lienActuelleSuiv = new Lien<Station>(noeudActuel, noeudSuiv, temps);
                        grapheParis.AjouterLien(lienActuelleSuiv);

                        string bidirectionnel = worksheet2.Cells[row, 7].Text;
                        if (bidirectionnel == "1")
                        {
                            Lien<Station> lienInverse = new Lien<Station>(noeudSuiv, noeudActuel, temps);
                            grapheParis.AjouterLien(lienInverse);
                        }
                    }
                }
                return grapheParis;
            }
        }

        static public void MetroParis()
        {
            Graphe<Station> metroParis = LireStationMetro("MetroParis.xlsx");

            DessinerGrapheStation(metroParis, "metro.png");

            Station s1 = metroParis.Noeuds[134].Nom;
            Station s2 = metroParis.Noeuds[25].Nom;
            FloydWarshall(metroParis, s1, s2);
        }

        static public void TestFloydWarshallInt()
        {
            Graphe<int> g = new Graphe<int>(true);
            Noeud<int> n1 = new Noeud<int>(1); g.AjouterSommet(n1);
            Noeud<int> n2 = new Noeud<int>(2); g.AjouterSommet(n2);
            Noeud<int> n3 = new Noeud<int>(3); g.AjouterSommet(n3);
            Noeud<int> n4 = new Noeud<int>(4); g.AjouterSommet(n4);
            Noeud<int> n5 = new Noeud<int>(5); g.AjouterSommet(n5);
            g.AjouterLien(new Lien<int>(n1, n2, 3));
            g.AjouterLien(new Lien<int>(n1, n3, 8));
            g.AjouterLien(new Lien<int>(n1, n5, -4));
            g.AjouterLien(new Lien<int>(n2, n4, 1));
            g.AjouterLien(new Lien<int>(n2, n5, 7));
            g.AjouterLien(new Lien<int>(n3, n2, 4));
            g.AjouterLien(new Lien<int>(n4, n3, -5));
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
            //FloydWarshall(g, 1, 4);
        }

        public static void Main(string[] args)
        {
            MetroParis();
        }
    }
}