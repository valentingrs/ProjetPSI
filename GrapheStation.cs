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
	}
}