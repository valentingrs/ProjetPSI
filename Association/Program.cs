using static Association.Interface;
using static Association.GrapheStation;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.ComponentModel;
using static Association.PlusCourtChemin;
using static Association.Association;


namespace Association
{
    internal class Program
    {
        private static readonly string ChaineDeConnexion =
"SERVER=127.0.0.1;PORT=3306;DATABASE=LivinParis;UID=root;PASSWORD=root";
        public static void Main(string[] args)
        {
            //TestColoration();

            //TestAlgoGraphes();
            MainInterface();
        }

        public static void MainInterface()
        {
            MySqlConnection maConnexion = null;
            try
            {
                maConnexion = new MySqlConnection(ChaineDeConnexion);
                maConnexion.Open();
                GererInterface(maConnexion);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
            }
        }
    }
}