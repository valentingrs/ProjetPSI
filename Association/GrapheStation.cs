using OfficeOpenXml;
using SkiaSharp;
using System.Globalization;
using static Association.PlusCourtChemin;

namespace Association
{
    internal class GrapheStation
    {
        /// Fonctions supplémentaires pour manipuler le graphe des stations Graphe<Station>
        /// Car il demande des méthodes particulières qui ne peuvent pas être traitées pour des types générique

        public static void AfficherGrapheStationMetroParis()
        {
            Graphe<Station> metroParis = LireStationMetro("MetroParis.xlsx");

            DessinerGrapheStation(metroParis, "metro.png");
        }

        static public void MetroParis(string s1, string s2)
        {
            Graphe<Station> metroParis = LireStationMetro("MetroParis.xlsx");

            Station stat1 = TrouverStationParNom(metroParis, s1);
            while (stat1 is null)
            {
                Console.Write("Entrer un nom de station de départ valide (attention aux accents, tirets et espaces, regarder la carte) : ");
                s1 = Console.ReadLine();
                stat1 = TrouverStationParNom(metroParis, s1);
            }
            Station stat2 = TrouverStationParNom(metroParis, s2);
            while (stat2 is null)
            {
                Console.Write("Entrer un nom de station d'arrivée valide (attention aux accents, tirets et espaces, regarder la carte) : ");
                s2 = Console.ReadLine();
                stat2 = TrouverStationParNom(metroParis, s2);
            }
            Dijkstra(metroParis, metroParis.IdentifierNoeud(stat1), metroParis.IdentifierNoeud(stat2));
            /// Console.WriteLine("\n\nFloyd Warshall : ");
            /// FloydWarshall(metroParis, metroParis.IdentifierNoeud(stat1), metroParis.IdentifierNoeud(stat2));
            /// Console.WriteLine("\n\nBellman Ford : ");
            /// Bellman_Ford(metroParis, metroParis.IdentifierNoeud(stat1), metroParis.IdentifierNoeud(stat2));
        }

        public static Station IdentifierStationId(List<Station> stations, int id) /// identifier une station à partir de son identifiant
        {
            Station identifiee;
            foreach (Station station in stations)
            {
                if (station.IdStation == id) { identifiee = station; return identifiee; }
            }
            Console.WriteLine("Pas de station ayant ce nom dans la liste");
            return null;
        }

        public static Station TrouverStationParNom(Graphe<Station> graphe, string nomStation)
        {
            foreach (Noeud<Station> noeud in graphe.Noeuds)
            {
                if (noeud.Nom.NomStation.Equals(nomStation, StringComparison.OrdinalIgnoreCase))
                {
                    return noeud.Nom;
                }
            }
            return null;
        }

        static public Graphe<Station> LireStationMetro(string filename) /// extrait du fichier MetroParis toutes les stations de métro de Paris
        {
            Graphe<Station> grapheParis = new Graphe<Station>(true); /// initialisation du graphe du métro parisien
            List<Station> stationsParis = new List<Station>(); /// liste des stations de paris
            Noeud<Station> noeudStation;


            if (!File.Exists(filename)) { Console.WriteLine("Le fichier n'existe pas"); }

            FileInfo fileinfo = new FileInfo(filename);
            using (ExcelPackage package = new ExcelPackage(fileinfo)) /// lecture du fichier xlsx
            {
                ExcelWorksheet worksheet1 = package.Workbook.Worksheets[0]; /// lecture de la première feuille excel -> stations de métro (noeuds)
                ExcelWorksheet worksheet2 = package.Workbook.Worksheets[1]; /// lecture de la première feuille excel -> lien entre les stations (arcs)

                /// lecture des données de la feuille
                int rowCount1 = worksheet1.Dimension.Rows; /// nombre de lignes = nombre de stations
                int rowCount2 = worksheet2.Dimension.Rows;

                /// on connait la forme du fichier Excel du métro parisien
                for (int row = 2; row <= rowCount1; row++)
                {
                    Station s = new Station(int.Parse(worksheet1.Cells[row, 1].Text),
                        worksheet1.Cells[row, 2].Text,
                        worksheet1.Cells[row, 3].Text,
                        double.Parse(worksheet1.Cells[row, 4].Text, CultureInfo.InvariantCulture),
                        double.Parse(worksheet1.Cells[row, 5].Text, CultureInfo.InvariantCulture),
                        worksheet1.Cells[row, 6].Text);
                    noeudStation = new Noeud<Station>(s); /// "conversion" de la station traitée en noeud
                    stationsParis.Add(s); /// ajout de la station à la liste des stations de Paris
                    grapheParis.AjouterSommet(noeudStation); /// on ajoute la station au plan du métro parisien
                }

                for (int row = 2; row <= rowCount2; row++)
                {
                    string stat = worksheet2.Cells[row, 1].Text;
                    Station statActuelle = IdentifierStationId(stationsParis, int.Parse(stat));
                    Noeud<Station> noeudActuel = grapheParis.IdentifierNoeud(statActuelle);

                    string prec = worksheet2.Cells[row, 3].Text;
                    if (prec != "")
                    {
                        Station statPrec = IdentifierStationId(stationsParis, int.Parse(prec));
                        Noeud<Station> noeudPrec = grapheParis.IdentifierNoeud(statPrec);
                        int temps = int.Parse(worksheet2.Cells[row, 5].Text);
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

        public static void DessinerGrapheStation(Graphe<Station> graphe, string fichierImage)
        {
            const int largeurImage = 2000;
            const int hauteurImage = 1100;
            const int marge = 50;

            /// Calcul des limites pour la normalisation
            double minLongitude = double.MaxValue, maxLongitude = double.MinValue;
            double minLatitude = double.MaxValue, maxLatitude = double.MinValue;

            foreach (Noeud<Station> noeud in graphe.Noeuds)
            {
                if (noeud.Nom is Station station)
                {
                    minLongitude = Math.Min(minLongitude, station.Longitude);
                    maxLongitude = Math.Max(maxLongitude, station.Longitude);
                    minLatitude = Math.Min(minLatitude, station.Latitude);
                    maxLatitude = Math.Max(maxLatitude, station.Latitude);
                }
            }

            /// Création du bitmap pour l'image
            using (var bitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                SKPaint lienPaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };
                SKPaint sommetPaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true, Style = SKPaintStyle.Fill };
                SKPaint textPaint = new SKPaint { Color = SKColors.Black, TextSize = 12, IsAntialias = true };

                Dictionary<Noeud<Station>, SKPoint> positions = new Dictionary<Noeud<Station>, SKPoint>();

                /// Positionner les stations en fonction de leur longitude et latitude
                foreach (Noeud<Station> noeud in graphe.Noeuds)
                {
                    if (noeud.Nom is Station station)
                    {
                        float x = (float)((station.Longitude - minLongitude) / (maxLongitude - minLongitude) * (largeurImage - 2 * marge)) + marge;
                        float y = (float)((station.Latitude - minLatitude) / (maxLatitude - minLatitude) * (hauteurImage - 2 * marge)) + marge;
                        y = hauteurImage - y; /// Inverser pour correspondre aux coordonnées graphiques
                        positions[noeud] = new SKPoint(x, y);
                    }
                }

                /// Dessiner les liens
                foreach (Lien<Station> lien in graphe.Liens)
                {
                    if (positions.ContainsKey(lien.Noeud1) && positions.ContainsKey(lien.Noeud2))
                    {
                        SKPoint point1 = positions[lien.Noeud1];
                        SKPoint point2 = positions[lien.Noeud2];
                        canvas.DrawLine(point1, point2, lienPaint);
                    }
                }

                /// Dessiner les sommets
                foreach (var entry in positions)
                {
                    SKPoint position = entry.Value;
                    canvas.DrawCircle(position, 5, sommetPaint);
                    canvas.DrawText(((Station)(entry.Key.Nom)).NomStation, position.X + 5, position.Y - 5, textPaint);
                }

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode())
                using (var stream = File.OpenWrite(fichierImage))
                {
                    data.SaveTo(stream);
                }

                string cheminImage = Path.GetFullPath(fichierImage);
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = cheminImage,
                    UseShellExecute = true  /// Permet d'utiliser l'application par défaut pour ouvrir l'image
                });
            }
        }

        
    }
}