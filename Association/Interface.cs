using static Association.GrapheStation;


namespace Association
{
	internal class Interface
	{

		public static void GererInterface()
		{
			Console.Clear();
			Menu();
			ChoixModule();
		}

		static void Menu()
		{
			Console.WriteLine("LIVIN PARIS\n\n");
			Console.WriteLine("-> Module Client\n-> Module Cuisinier\n-> Module Commande\n-> Module Statistiques\n");
		}

		static void ChoixModule()
		{
			Console.Write("Choisir un module : ");
			string mod = (Console.ReadLine()).ToLower().Trim();
			while (mod != "client" && mod != "cuisinier" && mod != "commande" && mod != "statistiques")
			{
				Console.WriteLine("Rentrer un nom valide de module");
				Console.Write("Choisir un module : ");
				mod = (Console.ReadLine()).ToLower().Trim();
			}
			
			switch (mod)
			{
				case "client":
					ModuleClient(); break;
				case "cuisinier":
					ModuleCuisinier(); break;
				case "commande":
					ModuleCommande(); break;
				case "statistiques":
					ModuleStatistiques(); break;
			}
		}

		static void RetourMenu()
		{
			GererInterface();
		}

		static void ModuleClient()
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module client");
			RetourMenu();
		}

		static void ModuleCuisinier()
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module cuisinier");
			RetourMenu();
		}

		static void ModuleCommande()
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
			if (choix == "r") { RetourMenu(); return; }

			//RetourMenu();
		}

		static void ModuleStatistiques()
		{
			Console.Clear(); Console.WriteLine("Bienvenue sur le module statistiques");
			RetourMenu();
		}
	}
}