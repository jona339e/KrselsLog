using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogLib
{
    public class MyTable
    {
        private const string sqlConnect = @"Data Source=DummyServerOne;Initial Catalog=KLP;User ID=jonas;Password=Password!";
        private string sql;
        private string holdme;
        private string s;
        private List<string> TabNavn = new List<string>();
        private List<string> ValNavn = new List<string>();

        private SqlCommand cmnd;
        private SqlDataReader DR;
        private SqlDataAdapter adapt = new SqlDataAdapter();
        private SqlConnection sqlconn = new SqlConnection(sqlConnect);

        public void InsertIntoBruger(string name, string plade)
        {
            sql = "insert into bruger (navn, dato, nr_plade) values('" + name + "', '" + DateTime.Now.ToString("d") + "', '" + plade + "')";
            sqlconn.Open();
            {
                cmnd = new SqlCommand(sql, sqlconn);
                adapt.InsertCommand = new SqlCommand(sql, sqlconn);
                adapt.InsertCommand.ExecuteNonQuery();
            }
            cmnd.Dispose();
            adapt.Dispose();
            sqlconn.Close();
        }

        public void InsertIntoKoerselsLog(string name, string dato, string plade, string opgave)
        {

            sql = "insert into koersels_log (navn, dato, nr_plade, opgave_beskrivelse) values('" + name + "', '" + dato + "', '" + plade + "', '" + opgave + "')";
            sqlconn.Open();
            {
                cmnd = new SqlCommand(sql, sqlconn);
                adapt.InsertCommand = new SqlCommand(sql, sqlconn);
                adapt.InsertCommand.ExecuteNonQuery();
            }
            cmnd.Dispose();
            adapt.Dispose();
            sqlconn.Close();
        }

        public void DeleteBruger(string name)
        {

            sql = "delete bruger where navn='" + name + "'";
            sqlconn.Open();
            {
                adapt.DeleteCommand = new SqlCommand(sql, sqlconn);
                adapt.DeleteCommand.ExecuteNonQuery();
            }
            adapt.Dispose();
            sqlconn.Close();

        }

        public void EditUser(string name, string plade)
        {
            sqlconn.Open();
            sql = "update bruger set nr_plade = '" + plade + "' where navn = '" + name + "'";
            cmnd = new SqlCommand(sql, sqlconn);
            {
                adapt.UpdateCommand = new SqlCommand(sql, sqlconn);
                adapt.UpdateCommand.ExecuteNonQuery();
            }
            cmnd.Dispose();
            adapt.Dispose();
            sqlconn.Close();

        }

        public List<string> GetNavn()
        {
            sql = "Select navn from bruger";
            sqlconn.Open();
            {
                cmnd = new SqlCommand(sql, sqlconn);
                DR = cmnd.ExecuteReader();

                while (DR.Read())
                {
                    TabNavn.Add("" + DR.GetValue(0));
                }
                DR.Close();
                cmnd.Dispose();
                sqlconn.Close();

                return TabNavn;
            }
        }

        public bool ValidateConnection()
        {
            try
            {
                sqlconn.Open();
                sqlconn.Close();
                return true;
            }
            catch (SqlException)
            {
                MessageBox.Show("Connection Error");
                return false;
                throw;
            }
        }

        public string GetDato(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                sql = "select dato from bruger where navn ='" + name + "'";
                sqlconn.Open();
                {
                    cmnd = new SqlCommand(sql, sqlconn);
                    DR = cmnd.ExecuteReader();
                    while (DR.Read())
                    {
                        holdme = "" + DR.GetValue(0);
                    }
                }
                DR.Close();
                cmnd.Dispose();
                sqlconn.Close();
                return holdme;
            }
            return "";
        }

        public string GetPlade(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                sql = "select nr_plade from bruger where navn ='" + name + "'";
                sqlconn.Open();
                {
                    cmnd = new SqlCommand(sql, sqlconn);
                    DR = cmnd.ExecuteReader();
                    while (DR.Read())
                    {
                        holdme = "" + DR.GetValue(0);
                    }
                }
                DR.Close();
                cmnd.Dispose();
                sqlconn.Close();
                return holdme;
            }
            return "";
        }

        public bool ValidateName(string name)
        {
            if (!NavnDupe(name))
            {
                MessageBox.Show("Input Fejl:\nNavn eksistere i forvejen");
                return false;
            }
            else if (!NavnLength(name))
            {
                MessageBox.Show("Input Fejl:\nNavn er for kort");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool NavnDupe(string name)
        {

            sql = "select navn from bruger";

            sqlconn.Open();
            {
                cmnd = new SqlCommand(sql, sqlconn);
                DR = cmnd.ExecuteReader();

                while (DR.Read())
                {
                    s = "" + DR.GetValue(0);
                }
                DR.Close();
                cmnd.Dispose();
                sqlconn.Close();
            }

                if (s == name)
                {
                    return false;
                }

            return true;
        }

        private bool NavnLength(string name)
        {
            if (name.Length < 3)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidatePlade(string plade)
        {
            if (!PladeLength(plade))
            {
                MessageBox.Show("Input Fejl:\nPlade opfylder ikke længde-krav");
                return false;
            }
            else if (!PladeStart(plade))
            {
                MessageBox.Show("Input Fejl:\nDe to første tegn I nummerpladen skal være tal");
                return false;
            }
            else if (PladeNumbers(plade))
            {
                MessageBox.Show("Input Fejl:\nPlade skal indeholde fem tal efter bogstaverne");
                return false;
            }
                return true;
        }

        private bool PladeStart(string plade)
        {
            if (char.IsLetter(plade[0]) && char.IsLetter(plade[1]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PladeLength(string plade)
        {
            if (plade.Length == 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool PladeNumbers(string plade)
        {
            for (int i = 1; i < plade.Length-1; i++)
            {
                if (Char.IsLetter(plade[i]))
                {
                    return false;
                }
            }
            return true;
        }


    }
}