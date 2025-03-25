using System;

namespace Association
{
	public class Noeud
	{
		private string nom;
		private List<Noeud> voisins;

		public Noeud(string nom)
		{
			voisins = new List<Noeud>();
			this.nom = nom;
		}

		public string Nom
		{
			get { return nom; }
			set { nom = value; }
		}

		public List<Noeud> Voisins
		{
			get { return voisins; }
			set { voisins = value; }
		}

		public void AjouterVoisin(Noeud s)
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

		public bool Equals(Noeud sommet2)
		{
			return (nom == sommet2.nom);
		}

		public static bool operator ==(Noeud s1, Noeud s2)
		{
			return s1.Equals(s2);
		}

		public static bool operator !=(Noeud s1, Noeud s2)
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
				foreach (Noeud voisin in voisins)
				{
					Console.Write(voisin.Nom  + " ");
				}
				Console.WriteLine("");
			}
		}

	}

}

