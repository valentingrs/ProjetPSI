﻿using static Association.Interface;
using static Association.GrapheStation;
using MySql.Data.MySqlClient;
using OfficeOpenXml;
using System.ComponentModel;
using static Association.PlusCourtChemin;
using static Association.Association;
using static Association.GrapheTiers;
using static Association.XmlJson;


namespace Association
{
    internal class Program
    {
        private static readonly string ChaineDeConnexion =
"SERVER=127.0.0.1;PORT=3306;DATABASE=LivinParis;UID=root;PASSWORD=root";

        public static void Main(string[] args)
        {
            //AfficherGrapheStationMetroParis();
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
                AfficherGrapheClientCuisinier(maConnexion);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(" ErreurConnexion : " + e.ToString());
            }
        }
    }
}