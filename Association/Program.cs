using System.Globalization;
using static Association.Graphique;
using OfficeOpenXml;
using System.Collections.Generic;


namespace Association
{
    internal class Program
    {
        //station de métro
        static public List<Station> LireStationMetro(string filename) // extrait du fichier MetroParis toutes les stations de métro de Paris
        {
            List<Station> stationsParis = new List<Station>(); 


            if(!File.Exists(filename)) { Console.WriteLine("Le fichier n'existe pas"); }


            FileInfo fileinfo = new FileInfo(filename);
            using (ExcelPackage package = new ExcelPackage(fileinfo)) // lecture du fichier xlsx
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // lecture de la première feuille excel

                // lecture des données de la feuille
                int rowCount = worksheet.Dimension.Rows; // nombre de lignes = nombre de stations

                // on connait la forme du fichier Excel du métro parisien
                for (int row = 2; row <= rowCount; row++)
                {
                    Station s = new Station(int.Parse(worksheet.Cells[row, 1].Text),
                        worksheet.Cells[row, 2].Text,
                        worksheet.Cells[row, 3].Text,
                        double.Parse(worksheet.Cells[row, 4].Text, CultureInfo.InvariantCulture),
                        double.Parse(worksheet.Cells[row, 5].Text, CultureInfo.InvariantCulture),
                        worksheet.Cells[row, 6].Text);

                    stationsParis.Add(s);                   
                }

                return stationsParis;
            }
        }

        public void GrapheAssociation() /// fonction de test du Rendu 1
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

        static void Main(string[] args)
        {
            List<Station> stationsParis = LireStationMetro("MetroParis.xlsx");
            Console.WriteLine(stationsParis[5]);
            Console.WriteLine(stationsParis[8]);
            Console.WriteLine(stationsParis[0]);
            Console.WriteLine(stationsParis[331]);

        }
        
    }
}