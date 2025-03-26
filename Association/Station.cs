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

		public Station(int idStation, string ligneMetro,  string nomStation, double longitude, double latitude, string commune)
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
			string s = nomStation + " - Ligne " + ligneMetro + " (" + longitude + ", " + latitude + ")";
			return s;
        }

    }
}