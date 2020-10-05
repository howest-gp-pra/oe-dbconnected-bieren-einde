using Pra.Bieren.CORE.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pra.Bieren.CORE.Services
{
    public class BierService
    {
        private List<Bier> bieren;

        public List<Bier> Bieren
        {
            get { return bieren; }
        }
		public BierService()
		{
			ReadAllRecords();
		}

		private void ReadAllRecords()
		{
			bieren = new List<Bier>();

			string sql;
			sql = "select * from Bieren order by naam";
			DataTable dtBieren = DBConnector.ExecuteSelect(sql);
			foreach (DataRow rw in dtBieren.Rows)
			{
				bieren.Add(new Bier(int.Parse(rw[0].ToString()), rw[1].ToString(), int.Parse(rw[2].ToString()), double.Parse(rw[3].ToString()), int.Parse(rw[4].ToString()) ));
			}
		}
		public bool AddBier(Bier bier)
		{
			// oppassen met alcohol : hier zit bv 5,3 in.
			// SQL server aanvaardt geen komma in gebroken getallen
			string strAlcohol = bier.Alcohol.ToString();
			strAlcohol = strAlcohol.Replace(",", ".");

			string sql;
			sql = "insert into Bieren(naam, biersoortid, alcohol, score) values (";
			sql += "'" + Helper.HandleQuotes(bier.Naam) + "' , ";
			sql += bier.BierSoortID + " , ";
			sql += bier.Alcohol + " , ";
			sql += bier.Score + " )";
			if (DBConnector.ExecuteCommand(sql))
			{
				// onderstaande instructies nodig omwille van autonummering
				sql = "select max(id) from bieren";
				bier.ID = int.Parse(DBConnector.ExecuteScalaire(sql));

				bieren.Add(bier);
				bieren = bieren.OrderBy(sorteerbier => sorteerbier.Naam).ToList();
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool EditBier(Bier bier)
		{
			// oppassen met alcohol : hier zit bv 5,3 in.
			// SQL server aanvaardt geen komma in gebroken getallen
			string strAlcohol = bier.Alcohol.ToString();
			strAlcohol = strAlcohol.Replace(",", ".");

			string sql;
			sql = "update Bieren ";
			sql += " set naam = '" + Helper.HandleQuotes(bier.Naam) + "' , ";
			sql += " biersoortid = " + bier.BierSoortID + " , ";
			sql += " alcohol = " + strAlcohol + " , ";
			sql += " score = " + bier.Score;
			sql += " where id = " + bier.ID;
			if (DBConnector.ExecuteCommand(sql))
			{
				//// pas het bier in de List aan
				//int positie = bieren.FindIndex(zoek => zoek.ID == bier.ID);
				//bieren[positie].Naam = bier.Naam;
				//bieren[positie].BierSoortID = bier.BierSoortID;
				//bieren[positie].Alcohol = bier.Alcohol;
				//bieren[positie].Score = bier.Score;
				// sorteer de List opnieuw op klantnaam
				bieren = bieren.OrderBy(sorteerbier => sorteerbier.Naam).ToList();
				return true;
			}
			else
			{
				return false;
			}
		}
		public bool DeleteBier(Bier bier)
		{
			string sql = "delete from bieren where id = " + bier.ID;
			if (DBConnector.ExecuteCommand(sql))
			{
				// bier ook uit list verwijderen
				bieren.Remove(bier);
				// List opnieuw sorteren hoeft hier niet
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
