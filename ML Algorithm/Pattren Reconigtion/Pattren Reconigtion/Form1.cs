using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;

namespace Pattren_Reconigtion
{
    public partial class Form1 : Form
    {
        GaussianBayees gb;
        List<List<string>> Data_training = new List<List<string>>();
        
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            btnChooseFile_Click(sender,e);
        }
        public DataTable ReadExcel(string fileName, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"; //for below excel 2007  
            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName + ";Extended Properties='Excel 12.0;HDR=NO';"; //for above excel 2007  
            using (OleDbConnection con = new OleDbConnection(conn))
            {
                try
                {
                    OleDbDataAdapter oleAdpt = new OleDbDataAdapter("select * from [sheet1$]", con); //here we read data from sheet1  //  Sayfa1 // Sheet1$
                    oleAdpt.Fill(dtexcel); //fill excel data into dataTable  
                }
                catch { }
            }
            return dtexcel;
        }
        void read_data_table(DataTable dt)
        {
   
            int row_number = dt.Rows.Count;
            int column_number = dt.Columns.Count;
         
            for (int i = 1; i < row_number; i++)
            {
                List<string> temp_row = new List<string>();
                for (int j = 0; j < column_number; j++)
                {
                    temp_row.Add((dt.Rows[i][j].ToString()).ToString());
                }
                Data_training.Add(temp_row);
            }
            
        }
        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            OpenFileDialog file = new OpenFileDialog(); //open dialog to choose file  
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK) //if there is a file choosen by the user  
            {
                filePath = file.FileName; //get the path of the file  
                //masar.Text = filePath;

                fileExt = Path.GetExtension(filePath); //get the file extension  
                if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                {
                    try
                    {
                        DataTable dtExcel = new DataTable();
                        dtExcel = ReadExcel(filePath, fileExt); //read excel file  
                        dataGridView1.Visible = true;
                        //dtExcel.Columns[1].ColumnName = "A";
                        // -----------------------------
                        //dtExcel.Rows.RemoveAt(0);
                        read_data_table(dtExcel);
                        // -------------------------
                        dataGridView1.DataSource = dtExcel;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
                else
                {
                    MessageBox.Show("Please choose .xls or .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error); //custom messageBox to show error  
                }
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); //to close the window(Form1)  
        }
        private void button2_Click(object sender, EventArgs e)
        {
            gb = new GaussianBayees(Data_training);
            gb.process();
            
            List<string> featurs_name = gb.getFeatures();
            List<string> target_name =gb.getTarget();
            comboBox1.DataSource = featurs_name;
            comboBox2.DataSource = target_name;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            string f = comboBox1.SelectedItem.ToString();
            string t = comboBox2.SelectedItem.ToString();
            string s = "P( ";
            s += t;
            s += " | ";
            s += f;
            s += " ) = ";
            s += gb.calculate_probablity(t, f).ToString();
            label1.Text = s;
        }

    }
}
