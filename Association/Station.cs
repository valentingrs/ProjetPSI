// classe pour représenter les stations de métro : 
namespace Association
{
	public class Station
	{
		private int idStation;
		private string ligneMetro;
		private string nomStation;
		private double longitude;
		private double latitude;
		private string commune;
		private Station precedente;
		private Station suivante;


		public Station(int idStation, string ligneMetro, string nomStation, double longitude, double latitude, string commune)
		{
			this.idStation = idStation;
			this.ligneMetro = ligneMetro;
			this.nomStation = nomStation;
			this.latitude = latitude;
			this.longitude = longitude;
			this.commune = commune;
		}

		public override string ToString()
		{
			if (precedente == null) { precedente.nomStation = "Terminus"; }
			if (suivante == null) { suivante.nomStation = "Terminus"; }

			string s = nomStation + " - Ligne " + ligneMetro + " (" + longitude + ", " + latitude + ")";
			s = s + "Précédente : " + precedente.nomStation + " ; Suivante : " + suivante.nomStation;
			return s;
		}

	}
}