using System;

namespace Association
{
	public class Sommet
	{
		private string nom;
		private List<Sommet> voisins;

		public Sommet(string nom)
		{
			voisins = new List<Sommet>();
			this.nom = nom;
		}

		public string Nom
		{
			get { return nom; }
			set { nom = value; }
		}

		public List<Sommet> Voisins
		{
			get { return voisins; }
			set { voisins = value; }
		}

		public void AjouterVoisin(Sommet s)
		{
			voisins.Add(s);
		}

		public override string ToString()
		{
			return nom;
		}

		public int Degre()
		{
			return voisins.Count;
		}

		public bool Equals(Sommet sommet2)
		{
			return (nom == sommet2.nom);
		}

		public static bool operator ==(Sommet s1, Sommet s2)
		{
			return s1.Equals(s2);
		}

		public static bool operator !=(Sommet s1, Sommet s2)
		{
			return !(s1 == s2);
		}

		public void AfficherVoisins()
		{
			if (voisins.Count == 0)
			{
				Console.WriteLine($"{nom} n'a pas de voisins.");
			}
			else
			{
				Console.WriteLine($"Les voisins de {nom} sont :");
				foreach (Sommet voisin in voisins)
				{
					Console.Write(voisin.Nom  + " ");
				}
				Console.WriteLine("");
			}
		}

	}

}

