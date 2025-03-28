using SkiaSharp;
using System.Drawing;

namespace Association
{
	internal class Graphique
	{
        static public void DessinerGraphe<T>(Graphe<T> graphe, string fichierImage)
        {
            const int largeurImage = 1000;
            const int hauteurImage = 1000;

            // Création du bitmap pour l'image
            using (var bitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var canvas = new SKCanvas(bitmap))
            {
                // Remplir le fond en blanc
                canvas.Clear(SKColors.White);

                // Créer un pinceau pour dessiner les liens
                SKPaint lienPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2,
                    IsAntialias = true
                };

                // Créer un pinceau pour dessiner les sommets
                SKPaint sommetPaint = new SKPaint
                {
                    Color = SKColors.Blue,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill
                };

                // Créer un pinceau pour dessiner le texte des sommets
                SKPaint textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 30,
                    IsAntialias = true
                };

                // Position des sommets (un exemple simple)
                Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();
                int angleStep = 360 / graphe.Noeuds.Count;
                int rayon = 450;
                SKPoint centre = new SKPoint(largeurImage / 2, hauteurImage / 2);
                int angle = 0;

                // Calculer les positions des sommets en cercle
                foreach (Noeud<T> noeud in graphe.Noeuds)
                {
                    float x = centre.X + rayon * (float)Math.Cos(Math.PI * angle / 180);
                    float y = centre.Y + rayon * (float)Math.Sin(Math.PI * angle / 180);
                    positions[noeud] = new SKPoint(x, y);
                    angle += angleStep;
                }

                // Dessiner les liens entre les sommets
                foreach (Lien<T> lien in graphe.Liens)
                {
                    // Vérifier si les sommets existent dans le dictionnaire avant de dessiner
                    if (positions.ContainsKey(lien.Noeud1) && positions.ContainsKey(lien.Noeud2))
                    {
                        SKPoint point1 = positions[lien.Noeud1];
                        SKPoint point2 = positions[lien.Noeud2];
                        canvas.DrawLine(point1, point2, lienPaint);
                    }
                }

                // Dessiner les sommets
                foreach (KeyValuePair<Noeud<T>, SKPoint> entry in positions)
                {
                    SKPoint position = entry.Value;
                    canvas.DrawCircle(position, 20, sommetPaint);  // Dessiner le sommet comme un cercle
                    canvas.DrawText(entry.Key.Nom.ToString(), position.X - 10, position.Y + 10, textPaint);  // Dessiner le nom du sommet
                }

                // Sauvegarder l'image dans un fichier
                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode())
                using (var stream = File.OpenWrite(fichierImage))
                {
                    data.SaveTo(stream);
                }
            }
        }
    }
}