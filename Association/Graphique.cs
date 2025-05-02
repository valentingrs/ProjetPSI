using SkiaSharp;
using System.Drawing;

namespace Association
{
	internal class Graphique
	{

        static public void DessinerGraphe<T>(Graphe<T> graphe, string fichierImage)
        {
            if (graphe.Oriente == true)
            {
                DessinerGrapheOriente(graphe, fichierImage);
            }
            else { DessinerGrapheNonOriente(graphe, fichierImage); }

            string cheminImage = Path.GetFullPath(fichierImage);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = cheminImage,
                UseShellExecute = true  // Permet d'utiliser l'application par défaut pour ouvrir l'image
            });
        }

        static public void DessinerGrapheNonOriente<T>(Graphe<T> graphe, string fichierImage)
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

        static private void DessinerGrapheOriente<T>(Graphe<T> graphe, string fichierImage)
        {
            const int largeurImage = 1000;
            const int hauteurImage = 1000;

            using (var bitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                SKPaint lienPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2,
                    IsAntialias = true
                };

                SKPaint sommetPaint = new SKPaint
                {
                    Color = SKColors.Blue,
                    IsAntialias = true,
                    Style = SKPaintStyle.Fill
                };

                SKPaint textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 30,
                    IsAntialias = true
                };

                SKPaint poidsPaint = new SKPaint
                {
                    Color = SKColors.Red,
                    TextSize = 25,
                    IsAntialias = true
                };

                Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();
                int angleStep = 360 / graphe.Noeuds.Count;
                int rayon = 450;
                SKPoint centre = new SKPoint(largeurImage / 2, hauteurImage / 2);
                int angle = 0;

                foreach (Noeud<T> noeud in graphe.Noeuds)
                {
                    float x = centre.X + rayon * (float)Math.Cos(Math.PI * angle / 180);
                    float y = centre.Y + rayon * (float)Math.Sin(Math.PI * angle / 180);
                    positions[noeud] = new SKPoint(x, y);
                    angle += angleStep;
                }

                foreach (Lien<T> lien in graphe.Liens)
                {
                    if (positions.ContainsKey(lien.Noeud1) && positions.ContainsKey(lien.Noeud2))
                    {
                        SKPoint point1 = positions[lien.Noeud1];
                        SKPoint point2 = positions[lien.Noeud2];
                        canvas.DrawLine(point1, point2, lienPaint);
                        DessinerFleche(canvas, point1, point2, lienPaint);

                        // Affichage du poids si présent avec un léger décalage
                        if (lien.Poids.HasValue)
                        {
                            SKPoint milieu = new SKPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
                            float angleLien = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
                            float decalage = 15;
                            SKPoint positionPoids = new SKPoint(
                                milieu.X + decalage * (float)Math.Cos(angleLien + Math.PI / 2),
                                milieu.Y + decalage * (float)Math.Sin(angleLien + Math.PI / 2)
                            );
                            string poidsTexte = lien.Poids.Value.ToString("0.##");
                            canvas.DrawText(poidsTexte, positionPoids.X, positionPoids.Y, poidsPaint);
                        }
                    }
                }

                foreach (KeyValuePair<Noeud<T>, SKPoint> entry in positions)
                {
                    SKPoint position = entry.Value;
                    canvas.DrawCircle(position, 20, sommetPaint);
                    canvas.DrawText(entry.Key.Nom.ToString(), position.X - 10, position.Y + 10, textPaint);
                }

                using (var image = SKImage.FromBitmap(bitmap))
                using (var data = image.Encode())
                using (var stream = File.OpenWrite(fichierImage))
                {
                    data.SaveTo(stream);
                }

                
            }
        }

        static void DessinerFleche(SKCanvas canvas, SKPoint debut, SKPoint fin, SKPaint paint)
        {
            float tailleFleche = 15;
            float distance = 20;
            float angle = (float)Math.Atan2(fin.Y - debut.Y, fin.X - debut.X);

            SKPoint baseFleche = new SKPoint(
                fin.X - distance * (float)Math.Cos(angle),
                fin.Y - distance * (float)Math.Sin(angle)
            );

            SKPoint p1 = new SKPoint(
                baseFleche.X - tailleFleche * (float)Math.Cos(angle - Math.PI / 6),
                baseFleche.Y - tailleFleche * (float)Math.Sin(angle - Math.PI / 6)
            );

            SKPoint p2 = new SKPoint(
                baseFleche.X - tailleFleche * (float)Math.Cos(angle + Math.PI / 6),
                baseFleche.Y - tailleFleche * (float)Math.Sin(angle + Math.PI / 6)
            );

            canvas.DrawLine(fin, p1, paint);
            canvas.DrawLine(fin, p2, paint);
        }

        static public void GrapheColorationSommets<T>(Graphe<T> graphe, Dictionary<Noeud<T>, int> coloration, string fichierImage)
        {
            if (graphe.Oriente)
            {
                Console.WriteLine("La coloration ne s'applique qu'aux graphes non orientés.");
                return;
            }

            const int largeurImage = 1000;
            const int hauteurImage = 1000;

            // Palette de couleurs de base
            SKColor[] palette = new SKColor[]
            {
        SKColors.Red, SKColors.Green, SKColors.Blue, SKColors.Orange, SKColors.Purple,
        SKColors.Brown, SKColors.Cyan, SKColors.Magenta, SKColors.Yellow, SKColors.Gray
            };

            using (var bitmap = new SKBitmap(largeurImage, hauteurImage))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                SKPaint lienPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 2,
                    IsAntialias = true
                };

                SKPaint textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 30,
                    IsAntialias = true
                };

                // Position des sommets en cercle
                Dictionary<Noeud<T>, SKPoint> positions = new Dictionary<Noeud<T>, SKPoint>();
                int angleStep = 360 / graphe.Noeuds.Count;
                int rayon = 450;
                SKPoint centre = new SKPoint(largeurImage / 2, hauteurImage / 2);
                int angle = 0;

                foreach (Noeud<T> noeud in graphe.Noeuds)
                {
                    float x = centre.X + rayon * (float)Math.Cos(Math.PI * angle / 180);
                    float y = centre.Y + rayon * (float)Math.Sin(Math.PI * angle / 180);
                    positions[noeud] = new SKPoint(x, y);
                    angle += angleStep;
                }

                // Dessiner les liens
                foreach (Lien<T> lien in graphe.Liens)
                {
                    if (positions.ContainsKey(lien.Noeud1) && positions.ContainsKey(lien.Noeud2))
                    {
                        SKPoint p1 = positions[lien.Noeud1];
                        SKPoint p2 = positions[lien.Noeud2];
                        canvas.DrawLine(p1, p2, lienPaint);
                    }
                }

                // Dessiner les sommets avec couleur
                foreach (var kvp in positions)
                {
                    Noeud<T> noeud = kvp.Key;
                    SKPoint position = kvp.Value;

                    int couleurIndex = coloration.ContainsKey(noeud) ? coloration[noeud] % palette.Length : 0;
                    SKPaint sommetPaint = new SKPaint
                    {
                        Color = palette[couleurIndex],
                        IsAntialias = true,
                        Style = SKPaintStyle.Fill
                    };

                    canvas.DrawCircle(position, 20, sommetPaint);
                    canvas.DrawText(noeud.Nom.ToString(), position.X - 10, position.Y + 10, textPaint);
                }

                // Sauvegarder
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
                    UseShellExecute = true
                });
            }
        }
    }
}