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
        // string der skal bruges til forbindelse mellem databasen og c#
        private const string sqlConnect = @"Data Source=DummyServerOne;Initial Catalog=KLP;User ID=jonas;Password=Password!";

        // string der bliver brugt til at lave sql queries.
        private string sql;

        // string der bliver brugt til at returne dato
        private string holdme;

        // string der bruges til at tjekke om et navn findes i databasen
        private string s;

        // string list der bruges til at returnere data fra kolonnen navn i tabellen bruger
        private List<string> TabNavn = new List<string>();


        private SqlCommand cmnd;
        private SqlDataReader DR;
        private SqlDataAdapter adapt = new SqlDataAdapter();
        private SqlConnection sqlconn = new SqlConnection(sqlConnect);


        /* Hvad der kan gøres bedre

        Lav constructor der tager en string parameter, denne parameter skal være = sqlConnect således at dette library kan bruges
        I sammenhæng med andre projekter. Når klassen bliver initialiseret I main() skal den tage en sqlConnect string'en.

        opret flere klasser så mine metoder bliver inddelt i klasserne 'connection', 'dataManipulation', 'getData' & 'verificer'.

        lade vær med at initialisere strings der kun bruges temporary i klassen, men opret dem i metoderne således at de bliver
        frigivet fra memory når metoden er kørt igennem.

        */


        public void InsertIntoBruger(string name, string plade) //metode der indsætter input og dato ind i tabellen bruger
        {
            sql = "insert into bruger (navn, dato, nr_plade) values('" + name + "', '" + DateTime.Now.ToString("d") + "', '" + plade + "')";
            sqlconn.Open();
            {
                cmnd = new SqlCommand(sql, sqlconn);
                adapt.InsertCommand = new SqlCommand(sql, sqlconn); //SqlDataAdapter bruges til at opdatere eller indsætte data ind i en sql database, udfra query og connection
                adapt.InsertCommand.ExecuteNonQuery(); //eksekverer opdateringen
            }
            cmnd.Dispose();
            adapt.Dispose(); ///lukker for adapt.
            sqlconn.Close();
        }

        public void InsertIntoKoerselsLog(string name, string dato, string plade, string opgave) //metode der indsætter input ind i tabellen koersels_log
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

        public void DeleteBruger(string name) //metode der slætter en række i tabellen bruger.
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

        public void EditUser(string name, string plade) //metode der ændre nummerpladen i tabellen bruger for udvalgt navn
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

        public List<string> GetNavn() //metode der henter navne fra tabellen bruger og gemmer dem i en liste.
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

        public bool ValidateConnection() //metode der tester om der er forbindelse til serveren.
        {
            try //prøver om følgende kan lade sig gøre
            {
                sqlconn.Open(); //åbner forbindelse til sql serveren
                sqlconn.Close(); //lukker forbindelse til sql serveren
                return true;
            }
            catch (SqlException) //hvis der ikke er forbindelse til sereveren og man derved får en sql exception, vil følgende blive kørt
            {
                MessageBox.Show("Connection Error"); //popup med meddelelse
                return false;
                //throw; //skal det her stå her?
            }
        }

        public string GetDato(string name) //metode der henter dato fra tabellen bruger
        {
            if (!string.IsNullOrEmpty(name))
            {
                sql = "select dato from bruger where navn ='" + name + "'"; //da der ikke kan være duplicate navne, vil der kune være en data entry og derfor kun et element der kan hentes
                sqlconn.Open();
                {
                    cmnd = new SqlCommand(sql, sqlconn); //query og connection
                    DR = cmnd.ExecuteReader(); //eksekverer datareader, der læser data udfra ens query
                    while (DR.Read())//sørgere for at gå igennem alt dataen.
                    {
                        holdme = "" + DR.GetValue(0); // er det her en acceptabel måde at gøre det på eller skal man bruge .tostring?
                        //sætter en string lig med den værdi vi har queriet.
                    }
                }
                DR.Close();     //lukker datareader
                cmnd.Dispose(); //lukker sqlcommand
                sqlconn.Close(); //lukker sqlConnection
                return holdme;
            }
            return "";
        }

        public string GetPlade(string name) //metode der henter nr_plade fra tabellen bruger.
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

        public bool ValidateName(string name) //metode der validere om kriterierne for navn er opfyldt
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

        private bool NavnDupe(string name) //metode der tjekker om input allerede findes i tabellen bruger
        {

            sql = "select navn from bruger";

            sqlconn.Open();
            {
                cmnd = new SqlCommand(sql, sqlconn);
                DR = cmnd.ExecuteReader();
                while (DR.Read())
                {
                    s = "" + DR.GetValue(0);
                    if (s == name)
                    {
                        DR.Close();
                        cmnd.Dispose();
                        sqlconn.Close();
                        return false;
                    }
                }
                DR.Close();
                cmnd.Dispose();
                sqlconn.Close();
            }
            return true;
        }

        private bool NavnLength(string name) //tjekker om input er mindre end 3 tegn langt, hvis det er returneres false, hvis ikke returneres true
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

        public bool ValidatePlade(string plade) // metode der validere om input lever op til kriterier for nummerplader
        {
            if (!PladeLength(plade)) //kalder funktioner - hvis der returneres false sker følgende
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

        private bool PladeStart(string plade) //metode der tjekker om de to første tegn i input er bogstaver
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

        private bool PladeLength(string plade) //metode der tjekker om inputs længde er 7
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

        private bool PladeNumbers(string plade) //metode der tjekker om input indeholder bogstaver hvor der skal være tal.
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