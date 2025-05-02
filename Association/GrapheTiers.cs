using MySql.Data.MySqlClient;
using static Association.Bdd;
using static Association.Graphique;


namespace Association
{
	internal class GrapheTiers
	{
		/// Générer le graphe des clients et des cuisiniers pour pouvoir l'étudier ensuite
		/// chaque noeud représente un tiers qui peut être soit client soit cuisinier
		/// si une commande est faite alors il y a un lien de cuisinier vers client
		/// récupérer chaque commande et créér le noeud du client et du cusinier s'il n'existent pas  
		/// puis créér le lien entre cuisinier et client
		
		static Graphe<int> GrapheClientCuisinier(List<int> listeTiers, List<Commande> listeCommandes)
		{
			Graphe<int> grapheClientCuisinier = new Graphe<int>(false);

			foreach (int tiers in listeTiers)
			{
                Noeud<int> noeudTiers = new Noeud<int>(tiers);
				grapheClientCuisinier.AjouterSommet(noeudTiers);
            }

			foreach(Commande commande in listeCommandes)
			{
				Noeud<int> noeudClient = grapheClientCuisinier.IdentifierNoeud(commande.idClient);
                Noeud<int> noeudCuisinier = grapheClientCuisinier.IdentifierNoeud(commande.idCuisinier);
                Lien<int> lienCommande = new Lien<int>(noeudClient, noeudCuisinier);
				grapheClientCuisinier.AjouterLien(lienCommande);
			}

			return grapheClientCuisinier;
		}
		
		static void ColorationGraphe(Graphe<int> grapheCC) /// grapheClientCuisinier
		{
			Dictionary<Noeud<int>, int> coloration = grapheCC.Coloration();

			foreach (int couleur in coloration.Values) { Console.Write(couleur + " ; "); }
			Console.WriteLine("\nNb chromatique : " + grapheCC.NbChromatique());

			GrapheColorationSommets(grapheCC, coloration, "grapheCCColorié.png");
		}

        static public void AfficherGrapheClientCuisinier(MySqlConnection conn)
        {
            List<int> tiers = RecupererTiers(conn);
            List<Commande> commandes = RecupererCommande(conn);
            Graphe<int> grapheClientsCuisiniers = GrapheClientCuisinier(tiers, commandes);
            //DessinerGraphe(grapheClientsCuisiniers, "grapheClient.png");

            ColorationGraphe(grapheClientsCuisiniers);
			Console.WriteLine("Etude du graphe des clients et des cuisiniers : \n");
			Console.WriteLine("Nombre chromatique : " + grapheClientsCuisiniers.NbChromatique());
			Console.WriteLine("Planaire ? " + grapheClientsCuisiniers.EstPlanaire());
			Console.WriteLine("Biparti ? " + grapheClientsCuisiniers.EstBiparti());
        }
    }
}
