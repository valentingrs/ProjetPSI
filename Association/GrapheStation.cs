namespace Association
{
	internal class GrapheStation
	{
		/// Fonctions suppl�mentaires pour manipuler le graphe des stations Graphe<Station>
		/// Car il demande des m�thodes particuli�res qui ne peuvent �tre trait�es pour des types g�n�rique
		
		public static Station IdentifierStationId(List<Station> stations, int id) // identifier une station � partir de son identifiant
		{
			Station identifiee;
			foreach (Station station in stations)
			{
				if (station.IdStation == id) { identifiee = station; return identifiee; }
			}
			Console.WriteLine("Pas de station ayant ce nom dans la liste");
			return null;
		}
	}
}