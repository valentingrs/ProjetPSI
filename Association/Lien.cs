using System;

namespace Association
{
	public class Lien
	{
		private Sommet sommet1;
		private Sommet sommet2;

		public Lien(Sommet sommet1, Sommet sommet2)
		{
			this.sommet1 = sommet1;
			this.sommet2 = sommet2;
		}

		public Sommet Sommet1
		{
			get { return sommet1; }
			set { sommet1 = value; }
		}

		public Sommet Sommet2
		{
			get { return sommet2; }
			set { sommet2 = value; }
		}

		public override string ToString()
		{
			string s = sommet1.Nom + " - " + sommet2.Nom;
			return s;
		}
	}

}