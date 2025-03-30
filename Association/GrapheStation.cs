using SkiaSharp;

namespace Association
{
    internal class GrapheStation
    {
        /// Fonctions supplémentaires pour manipuler le graphe des stations Graphe<Station>
        /// Car il demande des méthodes particulières qui ne peuvent pas être traitées pour des types générique

        public static Station IdentifierStationId(List<Station> stations, int id) // identifier une station à partir de son identifiant
        {
            Station identifiee;
            foreach (Station station in stations)
            {
                if (station.IdStation == id) { identifiee = station; return identifiee; }
            }
            Console.WriteLine("Pas de station ayant ce nom dans la liste");
            return null;
        }

        public static void DessinerGrapheStation(Graphe<Station> graphe, string fichierImage)
        {
            const int largeurImage = 2000;
            const int hauteurImage = 1100;
            const int marge = 50;

            // Calcul des limites pour la normalisation
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

            // Création du bitmap pour l'image
            using (var bitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                SKPaint lienPaint = new SKPaint { Color = SKColors.Black, StrokeWidth = 2, IsAntialias = true };
                SKPaint sommetPaint = new SKPaint { Color = SKColors.Blue, IsAntialias = true, Style = SKPaintStyle.Fill };
                SKPaint textPaint = new SKPaint { Color = SKColors.Black, TextSize = 12, IsAntialias = true };

                Dictionary<Noeud<Station>, SKPoint> positions = new Dictionary<Noeud<Station>, SKPoint>();

                // Positionner les stations en fonction de leur longitude et latitude
                foreach (Noeud<Station> noeud in graphe.Noeuds)
                {
                    if (noeud.Nom is Station station)
                    {
                        float x = (float)((station.Longitude - minLongitude) / (maxLongitude - minLongitude) * (largeurImage - 2 * marge)) + marge;
                        float y = (float)((station.Latitude - minLatitude) / (maxLatitude - minLatitude) * (hauteurImage - 2 * marge)) + marge;
                        y = hauteurImage - y; // Inverser pour correspondre aux coordonnées graphiques
                        positions[noeud] = new SKPoint(x, y);
                    }
                }

                // Dessiner les liens
                foreach (Lien<Station> lien in graphe.Liens)
                {
                    if (positions.ContainsKey(lien.Noeud1) && positions.ContainsKey(lien.Noeud2))
                    {
                        SKPoint point1 = positions[lien.Noeud1];
                        SKPoint point2 = positions[lien.Noeud2];
                        canvas.DrawLine(point1, point2, lienPaint);
                    }
                }

                // Dessiner les sommets
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
                    UseShellExecute = true  // Permet d'utiliser l'application par défaut pour ouvrir l'image
                });
            }
        }

    }
}