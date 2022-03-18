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
using LogLib;

namespace KoerselsLog
{
    public partial class Form1 : Form
    {
        public MyTable tab = new MyTable();
        public List<string> NavnList = new List<string>();

        static string brugerText = "Opret en bruger:\nIndtast et navn og en nummerplade. Klik på OK for at oprette en record og få det vist i nedenstående liste. Ved klik på Cancel resettes felterne.";
        static string redigerText = "Rediger en bruger:\nVælg en bruger fra drop down listen. Rediger nummerplade teksten. Klik på Ok for at gemme den opdaterede record, og få det vist i nedenstående skema. Ved klik på Cancel resettes felterne";
        static string sletText = "Slet en bruger:\nVælg en bruger fra drop down listen. Klik Ok for at slette den bruger (record) fra den nedenstående list. Ved klik på Cancel resettes navne feltet.";
        static string registrerText = "Registrer en kørsel:\nVælg en bruger fra dop down listen. Indtast en kort opgave tekst. Klik på Ok for at gemme den opdaterede record og få det vist i nedenstående skema. Ved klik på Cancel resettes felterne.";
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

            if (tab.ValidateConnection())
            {
                // TODO: This line of code loads data into the 'kLPDataSet.bruger' table. You can move, or remove it, as needed.
                this.brugerTableAdapter.Fill(this.kLPDataSet.bruger);
                // TODO: This line of code loads data into the 'kLPDataSet2.koersels_log' table. You can move, or remove it, as needed.
                this.koersels_logTableAdapter1.Fill(this.kLPDataSet2.koersels_log);
                NavnList = tab.GetNavn();
                foreach (string i in NavnList)
                {
                    navn_pick1.Items.Add(i);
                    navn_pick2.Items.Add(i);
                    navn_pick3.Items.Add(i);
                }
            }
            richTextBox.Text = brugerText;
            richTextBox1.Text = redigerText;
            richTextBox2.Text = sletText;
            richTextBox3.Text = registrerText;
            Dato.Text = DateTime.Now.ToString("d");
            Dato3.Text = DateTime.Now.ToString("d");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            navn_input.Text = "<text>";
            nr_plade_input.Text = "<text>";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) //navn_pick1
        {
            if (tab.ValidateConnection())
            {
                Dato2.Text = tab.GetDato(navn_pick1.Text);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tab.ValidateConnection())
            {
                if (tab.ValidatePlade(nr_plade_input2.Text))
                {
                    tab.EditUser(navn_pick1.Text, nr_plade_input2.Text);
                }
                else
                {
                    MessageBox.Show("Input blev ikke indsat I databasen");
                }
                nr_plade_input2.Text = "<text>";
                this.brugerTableAdapter.Fill(this.kLPDataSet.bruger);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tab.ValidateConnection())
            {
                tab.DeleteBruger(navn_pick2.Text);
                this.brugerTableAdapter.Fill(this.kLPDataSet.bruger);

                navn_pick1.Items.Remove(navn_pick2.Text);
                navn_pick2.Items.Remove(navn_pick2.Text);
                //navn_pick3.Items.Remove(navn_pick2.Text); //hvorfor virker den her ikke?

                NavnList.Remove(navn_pick2.Text); //det her virker
                navn_pick3.Items.Clear();
                foreach (string i in NavnList)
                {
                    navn_pick3.Items.Add(i);
                }

            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void nr_plade_input_TextChanged(object sender, EventArgs e)
        {

        }

        private void ok_Click(object sender, EventArgs e)
        {
            if (tab.ValidateConnection())
            {
                if (tab.ValidateName(navn_input.Text) && tab.ValidatePlade(nr_plade_input.Text))
                {
                    tab.InsertIntoBruger(navn_input.Text, nr_plade_input.Text);
                }
                else
                {
                    MessageBox.Show("Input blev ikke indsat I databasen");
                }

                navn_pick1.Items.Add(navn_input.Text);
                navn_pick2.Items.Add(navn_input.Text);
                navn_pick3.Items.Add(navn_input.Text);


                navn_input.Text = "<text>";
                nr_plade_input.Text = "<text>";
                this.brugerTableAdapter.Fill(this.kLPDataSet.bruger);
            }

        }

        private void navn_pick2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cancel2_Click(object sender, EventArgs e)
        {
            navn_input.Text = "<text>";
            nr_plade_input.Text = "<text>";
            navn_pick1.SelectedIndex = -1;
        }

        private void nr_plade_input2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cancel3_Click(object sender, EventArgs e)
        {
            navn_pick2.SelectedIndex = -1;
        }

        private void groupBox4_Enter(object sender, EventArgs e) 
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (tab.ValidateConnection())
            {
                Nr_plade_get.Text = tab.GetPlade(navn_pick3.Text);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (tab.ValidateConnection())
            {
                tab.InsertIntoKoerselsLog(navn_pick3.Text, Dato3.Text, Nr_plade_get.Text, opgave_text.Text);
                this.koersels_logTableAdapter1.Fill(this.kLPDataSet2.koersels_log);
                opgave_text.Text = "<text>";

            }
        }

        private void groupBox4_Enter_1(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Dato3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            navn_pick3.SelectedIndex = -1;
            opgave_text.Text = "<text>";
        }
    }
}
