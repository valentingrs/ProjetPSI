using System.Globalization;
using OfficeOpenXml;
using static Association.GrapheStation;

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

                for (int row = 2; row <= rowCount2; row++)
                {

                    string stat = worksheet2.Cells[row, 1].Text;
                    Station statActuelle = IdentifierStationId(stationsParis, Int32.Parse(stat));
                    Noeud<Station> noeudActuel = grapheParis.IdentifierNoeud(statActuelle);


                    string prec = worksheet2.Cells[row, 3].Text;
                    if (prec != "")
                    {
                        Station statPrec = IdentifierStationId(stationsParis, Int32.Parse(prec));
                        Noeud<Station> noeudPrec = grapheParis.IdentifierNoeud(statPrec);
                        int temps = Int32.Parse(worksheet2.Cells[row, 5].Text);
                        Lien<Station> lienPrecActuelle = new Lien<Station>(noeudPrec, noeudActuel, temps);
                        grapheParis.AjouterLien(lienPrecActuelle);

                    }

                    string suiv = worksheet2.Cells[row, 4].Text;
                    if (suiv != "") 
                    { 
                        Station statSuiv = IdentifierStationId(stationsParis, Int32.Parse(suiv));
                        Noeud<Station> noeudSuiv = grapheParis.IdentifierNoeud(statSuiv);
                        int temps = Int32.Parse(worksheet2.Cells[row, 6].Text);
                        Lien<Station> lienActuelleSuiv = new Lien<Station>(noeudActuel, noeudSuiv, temps);
                        grapheParis.AjouterLien(lienActuelleSuiv);
                    }
                }
                return grapheParis;
            }
        }

        public static void Main(string[] args)
        {
            Graphe<Station> metroParis = LireStationMetro("MetroParis.xlsx");

            DessinerGrapheStation(metroParis, "metro.png");
            string cheminImage = Path.GetFullPath("metro.png");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = cheminImage,
                UseShellExecute = true  // Permet d'utiliser l'application par défaut pour ouvrir l'image
            });

            //GrapheAssociation();
        }

    }
}