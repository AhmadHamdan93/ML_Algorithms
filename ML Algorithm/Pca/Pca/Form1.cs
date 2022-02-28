using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pca
{
    public partial class Form1 : Form
    {
        List<List<Double>> Data_matrix;
        List<double> average_matrix;
        List<List<double>> Data_average_matrix;
        List<List<double>> solution_matrix;

        public Form1()
        {
            InitializeComponent();
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

            for (int i = 0; i < column_number; i++)
            {
                List<Double> temp_row = new List<Double>();
                for (int j = 1; j < row_number; j++)
                {
                   temp_row.Add(Double.Parse(dt.Rows[j][i].ToString()));
                }
                Data_matrix.Add(temp_row);
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
                label1.Text = Path.GetFileName(filePath);

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


        private void load_data_btn_Click(object sender, EventArgs e)
        {
            Data_matrix = new List<List<Double>>();
            btnChooseFile_Click(sender, e);
        }

        private void calc_btn_Click(object sender, EventArgs e)
        {
            solution_matrix = new List<List<double>>();
            Data_average_matrix = new List<List<double>>();
            average_matrix = new List<double>();
            Averages_calculate();
            data_minus_average();
            find_solution();
            show_data();
        }

        void Averages_calculate()
        {
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                double avg = 0.0;
                for (int j = 0; j < Data_matrix[0].Count; j++)
                {
                    avg += Data_matrix[i][j];
                }
                avg = Math.Round((avg / Data_matrix[0].Count),4);
                average_matrix.Add(avg);
            }
        }
        void data_minus_average()
        {
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                double avg = average_matrix[i];
                List<double> temp = new List<double>();
                for (int j = 0; j < Data_matrix[0].Count; j++)
                {
                    temp.Add(Math.Round((Data_matrix[i][j] - avg),4));
                    
                }
                Data_average_matrix.Add(temp);
            }
        }
        void find_solution()
        {
            int features_num = Data_matrix.Count;
            for (int i = 0; i < features_num; i++)
            {
                List<double> temp = new List<double>();
                for (int j = 0; j < features_num; j++)
                {
                    double r = cov(i, j);
                    temp.Add(r);
                }
                solution_matrix.Add(temp);
            }
        }
        double cov(int i, int j)
        {
            double res = 0.0, sum=0.0;
            int data_size = Data_average_matrix[i].Count;
            int row = Data_average_matrix.Count;

            for (int idx = 0; idx < Data_average_matrix[0].Count; idx++)
            {
                sum += Data_average_matrix[i][idx] * Data_average_matrix[j][idx]; 
            }
            res = sum / (data_size - 1);
            res = Math.Round(res, 4);
            return res;
        }
        void show_data()
        {
          
            DataTable dt = new DataTable();
            dt.Clear();
            
            
            for (int i = 0; i < solution_matrix.Count; i++)
            {
                dt.Columns.Add(""+i);
            }
           
            for (int i = 0; i < solution_matrix.Count; i++)
            {
                DataRow row = dt.NewRow();
                for (int j = 0; j < solution_matrix.Count; j++)
                {
                   row[""+j] = solution_matrix[i][j].ToString();
                }
                dt.Rows.Add(row);
            }
                 
            dataGridView2.DataSource = dt;
        }


    }
}
