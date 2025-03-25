// classe pour représenter les stations de métro : 
namespace Association
{
	public class Station
	{
		private int idStation;
		private int ligneMetro;
		private string nomStation;
		private float longitude;
		private float latitude;
		private string commune;

		public Station(int idStation, int ligneMetro,  string nomStation, float longitude, float latitude, string commune)
		{
			this.idStation = idStation;
			this.ligneMetro = ligneMetro;
			this.nomStation = nomStation;
			this.latitude = latitude;	
			this.longitude = longitude;
			this.commune = commune;
		}


	}
}