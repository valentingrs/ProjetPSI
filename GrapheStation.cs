namespace Association
{
	internal class GrapheStation
	{
		/// Fonctions supplémentaires pour manipuler le graphe des stations Graphe<Station>
		/// Car il demande des méthodes particulières qui ne peuvent être traitées pour des types générique

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

		public Noeud<Station> IdentifierNoeudStation(Station nom, List<Noeud<Station>> noeudsStation)
		// même principe que la méthode précédente
		// mais pour identifier un noeud à partir d'une station dans une liste de noeuds
		{
			Noeud<Station> noeudTrouve;
			foreach (Noeud<Station> noeud in noeudsStation)
			{
				if (noeud.Nom.Equals(nom)) { return noeud; }
			}
			Console.WriteLine("Pas de noeud trouve.");
			noeudTrouve = null;
			return noeudTrouve;
		}

		public static void DessinerGrapheStation(Graphe<Station> graphe, string fichierImage)
		// Dessiner le graphe des stations de métro de Paris
		{
			const int largeurImage = 1600;
			const int hauteurImage = 1000;
			const int marge = 50;
			const float rayonSommet = 6;

			double minLong = graphe.Noeuds.Min(n => n.Nom.Longitude);
			double maxLong = graphe.Noeuds.Max(n => n.Nom.Longitude);
			double minLat = graphe.Noeuds.Min(n => n.Nom.Latitude);
			double maxLat = graphe.Noeuds.Max(n => n.Nom.Latitude);

			using (var bitmap = new SKBitmap(largeurImage, hauteurImage))
			using (var canvas = new SKCanvas(bitmap))
			{
				canvas.Clear(SKColors.White);

				SKPaint lienPaint = new SKPaint
				{
					Color = SKColors.Gray,
					StrokeWidth = 2,
					IsAntialias = true,
					Style = SKPaintStyle.Stroke
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
					TextSize = 10,
					IsAntialias = true
				};

				Dictionary<Noeud<Station>, SKPoint> positions = new Dictionary<Noeud<Station>, SKPoint>();

				foreach (var noeud in graphe.Noeuds)
				{
					Station station = noeud.Nom;
					float x = (float)((station.Longitude - minLong) / (maxLong - minLong) * (largeurImage - 2 * marge) + marge);
					float y = (float)((1 - (station.Latitude - minLat) / (maxLat - minLat)) * (hauteurImage - 2 * marge) + marge);
					positions[noeud] = new SKPoint(x, y);
				}

				foreach (Lien<Station> lien in graphe.Liens)
				{
					if (!positions.ContainsKey(lien.Noeud1) || !positions.ContainsKey(lien.Noeud2)) continue;

					SKPoint p1 = positions[lien.Noeud1];
					SKPoint p2 = positions[lien.Noeud2];

					SKPoint controlPoint = new SKPoint((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2 - 10);

					using (var path = new SKPath())
					{
						path.MoveTo(p1);
						path.QuadTo(controlPoint, p2);
						canvas.DrawPath(path, lienPaint);
					}
				}

				foreach (var entry in positions)
				{
					SKPoint position = entry.Value;
					canvas.DrawCircle(position, rayonSommet, sommetPaint);
					canvas.DrawText(entry.Key.Nom.NomStation, position.X + 5, position.Y - 5, textPaint);
				}

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