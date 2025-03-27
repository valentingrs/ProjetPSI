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
		//private Station precedente;
		//private Station suivante;


		public Station(int idStation, string ligneMetro, string nomStation, double longitude, double latitude, string commune)
		{
			this.idStation = idStation;
			this.ligneMetro = ligneMetro;
			this.nomStation = nomStation;
			this.latitude = latitude;
			this.longitude = longitude;
			this.commune = commune;
		}

		public int IdStation
		{
			get { return idStation; }
		}

        public string NomStation
        {
            get { return nomStation; }
        }
        public double Longitude
        {
            get { return longitude; }
        }
        public double Latitude
        {
            get { return latitude; }
        }

        public override string ToString()
		{
			string s = "" + idStation;
			//string s = nomStation + " - Ligne " + ligneMetro + " (" + longitude + ", " + latitude + ")";
			//s = s + "Précédente : " + precedente.nomStation + " ; Suivante : " + suivante.nomStation;
			return s;
		}

	}
}