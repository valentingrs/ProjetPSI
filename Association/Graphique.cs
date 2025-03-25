using SkiaSharp;


namespace Association
{
	internal class Graphique
	{


		static public Graphe LireFichier()
		{

			string filename = ("soc-karate.mtx");
			List<string> dico = new List<string>();
			string[] lignes = File.ReadAllLines(filename);

			Graphe association = new Graphe(false); // non orienté car associations réciproques
			for (int i = 24; i <= 101; i++)
			{
				string[] ligne = lignes[i].Split(' ');

				string s0 = ligne[0];
				Sommet sommet0 = new Sommet("");
				bool existeDeja = false;
				foreach (Sommet k in association.Sommets)
				{
					if (k.Nom == s0) { sommet0 = k; existeDeja = true; }

				}
				if (existeDeja == false) { sommet0 = new Sommet(s0); }

				existeDeja = false;
				string s1 = ligne[1];
				Sommet sommet1 = new Sommet("");
				foreach (Sommet j in association.Sommets)
				{
					if (j.Nom == s1) { sommet1 = j; existeDeja = true; }
				}
				if (existeDeja == false) { sommet1 = new Sommet(s1); }

				Lien l = new Lien(sommet0, sommet1);

				if (!association.ContientSommet(sommet0)) { association.AjouterSommet(sommet0); }
				if (!association.ContientSommet(sommet1)) { association.AjouterSommet(sommet1); }

				association.AjouterLien(l);

			}
			return association;

		}

		static public void DessinerGraphe(Graphe graphe, string fichierImage)
		{
			const int largeurImage = 900;
			const int hauteurImage = 800;

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
				Dictionary<Sommet, SKPoint> positions = new Dictionary<Sommet, SKPoint>();
				int angleStep = 360 / graphe.Sommets.Count;
				int rayon = 350;
				SKPoint centre = new SKPoint(largeurImage / 2, hauteurImage / 2);
				int angle = 0;

				// Calculer les positions des sommets en cercle
				foreach (Sommet sommet in graphe.Sommets)
				{
					float x = centre.X + rayon * (float)Math.Cos(Math.PI * angle / 180);
					float y = centre.Y + rayon * (float)Math.Sin(Math.PI * angle / 180);
					positions[sommet] = new SKPoint(x, y);
					angle += angleStep;
				}

				// Dessiner les liens entre les sommets
				foreach (Lien lien in graphe.Liens)
				{
					// Vérifier si les sommets existent dans le dictionnaire avant de dessiner
					if (positions.ContainsKey(lien.Sommet1) && positions.ContainsKey(lien.Sommet2))
					{
						SKPoint point1 = positions[lien.Sommet1];
						SKPoint point2 = positions[lien.Sommet2];
						canvas.DrawLine(point1, point2, lienPaint);
					}
				}

				// Dessiner les sommets
				foreach (KeyValuePair<Sommet, SKPoint> entry in positions)
				{
					SKPoint position = entry.Value;
					canvas.DrawCircle(position, 20, sommetPaint);  // Dessiner le sommet comme un cercle
					canvas.DrawText(entry.Key.Nom, position.X - 10, position.Y + 10, textPaint);  // Dessiner le nom du sommet
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