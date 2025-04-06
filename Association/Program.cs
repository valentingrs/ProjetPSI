using static Association.Interface;
using static Association.GrapheStation;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.ComponentModel;

namespace Association
{
    internal class Program
    {
        private static readonly string ChaineDeConnexion =
"SERVER=127.0.0.1;PORT=3306;DATABASE=LivinParis;UID=root;PASSWORD=root";
        public static void Main(string[] args)
        {

            //GrapheSimpleTest();
            //GererInterface();

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