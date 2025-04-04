using static Association.GrapheStation;
using static Association.Bdd;
using MySql.Data.MySqlClient;

namespace Association
{
	internal class Interface
	{
		public static void GererInterface(MySqlConnection conn)
		{
			Console.Clear();
			Menu();
			ChoixModule(conn);
		}

		static void Menu()
		{
			Console.WriteLine("LIVIN PARIS\n\n");
			Console.WriteLine("-> Module Tiers\n-> Module Client\n-> Module Cuisinier\n-> Module Commande\n-> Module Statistiques\n");
		}

		static void ChoixModule(MySqlConnection conn)
		{
			Console.Write("Choisir un module : ");
			string mod = (Console.ReadLine()).ToLower().Trim();
			while (mod != "tiers" && mod != "client" && mod != "cuisinier" && mod != "commande" && mod != "statistiques")
			{
				Console.WriteLine("Rentrer un nom valide de module");
				Console.Write("Choisir un module : ");
				mod = (Console.ReadLine()).ToLower().Trim();
			}
			
			switch (mod)
			{
				case "tiers":
					ModuleTiers(conn); break;
				case "client":
					ModuleClient(conn); break;
				case "cuisinier":
					ModuleCuisinier(conn); break;
				case "commande":
					ModuleCommande(conn); break;
				case "statistiques":
					ModuleStatistiques(conn); break;
			}
		}

		static void RetourMenu(MySqlConnection conn)
		{
			GererInterface(conn);
		}

		static void ModuleTiers(MySqlConnection conn)
		{
            Console.Clear(); Console.WriteLine("Bienvenue sur le module tiers\n");

            Console.WriteLine("Que voulez-faire ? Selectionner le numéro assoicé à l'option ou entrer 'r' pour retourner en arrière: ");
            Console.WriteLine("1 - Ajouter un compte");
			Console.WriteLine("2 - Supprimer un compte");
            Console.Write("\nChoix : ");
            string choix = Console.ReadLine();
            while (choix != "1" && choix != "2" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
			while (choix != "r")
			{
                if (choix == "1")
                {
                    Console.WriteLine("\nCréation d'un compte : ");
                    Console.Write("Entrer un id : "); int idTiers = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Entrer un nom : "); string nom = Console.ReadLine();
                    Console.Write("Entrer un prénom : "); string prenom = Console.ReadLine();
                    Console.Write("Entrer un code postal : "); string cp = Console.ReadLine();
                    Console.Write("Entrer une ville : "); string ville = Console.ReadLine();
                    Console.Write("Entrer une adresse : "); string adresse = Console.ReadLine();
                    Console.Write("Entrer une adresse mail: "); string email = Console.ReadLine();
                    Console.Write("Entrer un numéro de téléphone : "); string tel = Console.ReadLine();
                    CreerUnCompte(conn, idTiers, cp, ville, email, tel, nom, adresse, prenom);
                }

				if (choix == "2")
				{
					Console.WriteLine("\nSupprimer un compte : ");
					Console.WriteLine("Entrer l'id du compte : "); int idTiers = Convert.ToInt32(Console.ReadLine());
					SupprimerUnCompte(conn, "Tiers", idTiers);

                }

                Console.Write("\nChoix : ");
                choix = Console.ReadLine();
                while (choix != "1" && choix != "2" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
            }
            RetourMenu(conn);
        }

		static void ModuleClient(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module client\n");

			Console.WriteLine("Que voulez-faire ? Selectionner le numéro assoicé à l'option ou entrer 'r' pour retourner en arrière: ");
			Console.WriteLine("1 - Ajouter un client");
			Console.Write("\nChoix : ");
			string choix = Console.ReadLine();
			while (choix != "1" && choix != "r") { Console.Write("\nRentre un choix valide : "); choix = Console.ReadLine(); }
			if (choix == "1")
			{
				
			}
			else
			{
                RetourMenu(conn);
            }
		}

		static void ModuleCuisinier(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module cuisinier");
			RetourMenu(conn);
		}

		static void ModuleCommande(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module commande\nOptions : \n");
			Console.WriteLine("1 - Distance entre deux stations de métro\n\n");

			Console.WriteLine("Pour choisir une option, rentrer le numéro au début de l'option");
			Console.WriteLine("Pour retourner au menu principal, entrer 'r'");
			Console.Write("\nChoix : ");
			
			string choix = Console.ReadLine();

			while (choix != "1" && choix != "r") 
			{
				Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
				choix = Console.ReadLine();
			}
			
			while (choix != "r")
			{
				if (choix == "1") { Console.WriteLine(); MetroParis(); }

				Console.Write("\nChoix : ");
				choix = Console.ReadLine();
				while (choix != "1" && choix != "r")
				{
					Console.Write("\nChoix invalide, rentrer un nouveau choix : ");
					choix = Console.ReadLine();
				}

			}
			if (choix == "r") { RetourMenu(conn); return; }

			//RetourMenu();
		}

		static void ModuleStatistiques(MySqlConnection conn)
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module statistiques");
			RetourMenu(conn);
		}
	}
}