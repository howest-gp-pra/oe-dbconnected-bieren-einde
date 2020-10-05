using Pra.Bieren.CORE.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pra.Bieren.CORE.Entities
{
    public class Bier
    {
		private int id;
		private string naam;
		private int bierSoortID;
		private BierSoort soort;
		private double alcohol;
		private int score;

		public int ID
		{
			get { return id; }
			set { id = value; }
		}
		public string Naam
		{
			get { return naam; }
			set {
				value = value.Trim();
				if (value.Length == 0)
					throw new Exception("Waarde biernaam kan niet leeg zijn");
				if (value.Length > 50)
					value = value.Substring(0, 30);
				naam = value;
			}
		}
		public int BierSoortID
		{
			get { return bierSoortID; }
			set { 
				bierSoortID = value;
				soort = BierSoortService.GetByID(value);
			}
		}
		public BierSoort Soort
		{
			get { return soort; }
		}
		public double Alcohol
		{
			get { return alcohol; }
			set { alcohol = value; }
		}
		public int Score
		{
			get { return score; }
			set { score = value; }
		}
		public Bier()
		{

		}
		public Bier(int id, string naam, int bierSoortID, double alcohol, int score)
		{
			this.id = id;
			this.naam = naam;
			BierSoortID = bierSoortID; // hier prop gebruiken om setter te laten uitvoeren
			this.alcohol = alcohol;
			this.score = score;
		}

		public override string ToString()
		{
			return $"{naam} - {alcohol.ToString("0.00")}%";
		}




	}
}
