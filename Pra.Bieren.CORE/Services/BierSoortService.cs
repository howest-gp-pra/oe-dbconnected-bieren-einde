using Pra.Bieren.CORE.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Pra.Bieren.CORE.Services
{
    public class BierSoortService
    {
        private List<BierSoort> biersoorten;

        public List<BierSoort> BierSoorten
        {
            get { return biersoorten; }
        }
        public BierSoortService()
        {
            ReadAllRecords();
        }
        private void ReadAllRecords()
        {
            biersoorten = new List<BierSoort>();

            string sql;
            sql = "select * from biersoorten order by soort";
            DataTable dtBierSoorten = DBConnector.ExecuteSelect(sql);
            foreach (DataRow rw in dtBierSoorten.Rows)
            {
                biersoorten.Add(new BierSoort(int.Parse(rw[0].ToString()), rw[1].ToString()));
            }
        }
        public bool AddBierSoort(BierSoort biersoort)
        {
            string sql;
            sql = "insert into biersoorten(soort) values (";
            sql += "'" + Helper.HandleQuotes(biersoort.Soort) + "' ) ";
            if (DBConnector.ExecuteCommand(sql))
            {
                // onderstaande instructies nodig omwille van autonummering
                sql = "select max(id) from biersoorten";
                biersoort.ID = int.Parse(DBConnector.ExecuteScalaire(sql));

                biersoorten.Add(biersoort);
                biersoorten = biersoorten.OrderBy(sorteerbiersoort => sorteerbiersoort.Soort).ToList();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool EditBierSoort(BierSoort biersoort)
        {
            string sql;
            sql = "update biersoorten ";
            sql += " set soort = '" + Helper.HandleQuotes(biersoort.Soort) + "'  ";
            sql += " where id = " + biersoort.ID;
            if (DBConnector.ExecuteCommand(sql))
            {
                //// pas het bier in de List aan
                //int positie = biersoorten.FindIndex(zoek => zoek.ID == biersoort.ID);
                //biersoorten[positie].Soort = biersoort.Soort;

                biersoorten = biersoorten.OrderBy(sorteerbier => sorteerbier.Soort).ToList();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeleteBierSoort(BierSoort biersoort)
        {
            string sql;
            sql = "select count(*) from bieren where biersoortid = " + biersoort.ID;
            if (int.Parse(DBConnector.ExecuteScalaire(sql)) != 0)
                return false;
            sql = "delete from biersoorten where id = " + biersoort.ID;
            if (DBConnector.ExecuteCommand(sql))
            {
                // bier ook uit list verwijderen
                biersoorten.Remove(biersoort);
                // List opnieuw sorteren hoeft hier niet
                return true;
            }
            else
            {
                return false;
            }
        }

        // OPGEPAST : methode hieronder is STATIC
        public static BierSoort GetByID(int id)
        {
            BierSoort bierSoort;
            string sql;
            sql = "select * from biersoorten where id = " + id;
            DataTable dtBierSoorten = DBConnector.ExecuteSelect(sql);
            if (dtBierSoorten.Rows.Count == 0)
                return null;
            else
            {
                DataRow rw = dtBierSoorten.Rows[0];
                bierSoort = new BierSoort(int.Parse(rw[0].ToString()), rw[1].ToString());
            }
            return bierSoort;
        }
    }
}
