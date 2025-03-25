using SkiaSharp;


namespace Association
{
	internal class Graphique
	{
		static public Graphe<string> LireFichier()
		{

			string filename = ("soc-karate.mtx");
			List<string> dico = new List<string>();
			string[] lignes = File.ReadAllLines(filename);

			Graphe<string> association = new Graphe<string>(false); // non orienté car associations réciproques
			// non pondéré également
			for (int i = 24; i <= 101; i++)
			{
				string[] ligne = lignes[i].Split(' ');

				string s0 = ligne[0];
				Noeud<string> noeud0 = new Noeud<string>("");
				bool existeDeja = false;
				foreach (Noeud<string> k in association.Noeuds)
				{
					if (k.Nom == s0) { noeud0 = k; existeDeja = true; }

				}
				if (existeDeja == false) { noeud0 = new Noeud<string>(s0); }

				existeDeja = false;
				string s1 = ligne[1];
				Noeud<string> noeud1 = new Noeud<string>("");
				foreach (Noeud<string> j in association.Noeuds)
				{
					if (j.Nom == s1) { noeud1 = j; existeDeja = true; }
				}
				if (existeDeja == false) { noeud1 = new Noeud<string>(s1); }

				Lien<string> l = new Lien<string>(noeud0, noeud1);

				if (!association.ContientSommet(noeud0)) { association.AjouterSommet(noeud0); }
				if (!association.ContientSommet(noeud1)) { association.AjouterSommet(noeud1); }

				association.AjouterLien(l);

			}
			return association;

		}

		static public void DessinerGraphe(Graphe<string> graphe, string fichierImage)
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
				Dictionary<Noeud<string>, SKPoint> positions = new Dictionary<Noeud<string>, SKPoint>();
				int angleStep = 360 / graphe.Noeuds.Count;
				int rayon = 350;
				SKPoint centre = new SKPoint(largeurImage / 2, hauteurImage / 2);
				int angle = 0;

				// Calculer les positions des sommets en cercle
				foreach (Noeud<string> noeud in graphe.Noeuds)
				{
					float x = centre.X + rayon * (float)Math.Cos(Math.PI * angle / 180);
					float y = centre.Y + rayon * (float)Math.Sin(Math.PI * angle / 180);
					positions[noeud] = new SKPoint(x, y);
					angle += angleStep;
				}

				// Dessiner les liens entre les sommets
				foreach (Lien<string> lien in graphe.Liens)
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
				foreach (KeyValuePair<Noeud<string>, SKPoint> entry in positions)
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