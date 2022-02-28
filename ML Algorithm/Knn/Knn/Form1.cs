using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Knn
{
    public partial class Form1 : Form
    {
        int x_step = 0;
        int y_step = 0;
        int x_max = 0;
        int y_max = 0;
        List<string> classes_name = new List<string>();
        List<List<int>> Data_matrix = new List<List<int>>();
        List<List<double>> distance_matrix;

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

            for (int i = 1; i < row_number; i++)
            {
                List<int> temp_row = new List<int>();
                for (int j = 0; j < column_number; j++)
                {
                    if (j == 0)
                    {
                        int idx = classes_name.IndexOf((dt.Rows[i][j].ToString()));
                        if ( idx != -1)
                        {
                            temp_row.Add(idx);
                        }
                        else
                        {
                            classes_name.Add(dt.Rows[i][j].ToString());
                            idx = classes_name.Count - 1;
                            temp_row.Add(idx);
                        }
                        
                    }
                    else
                    {
                        temp_row.Add(Int32.Parse(dt.Rows[i][j].ToString()));
                    }
                    
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

        private void button1_Click(object sender, EventArgs e)
        {
            btnChooseFile_Click(sender, e);
            draw();
        }

        private void expect_btn_Click(object sender, EventArgs e)
        {
            distance_matrix = new List<List<double>>();
            draw();
            int x = Int32.Parse(sample_x.Text.ToString());
            int y = Int32.Parse(sample_y.Text.ToString());
            int k = Int32.Parse(k_number.Text.ToString());

            if (radio_euclidean.Checked)
            {
                euclidean_distance(x, y);
            }
            if (radio_manhattan.Checked)
            {
                manhattan_distance(x, y);
            }
            if (radio_max.Checked)
            {
                max_distance(x, y);
            }

            if (k != 0)
            {
                List<List<double>> near = find_k_nearest_neighbor(k);

                classifer_function(near);

                Graphics g = pictureBox1.CreateGraphics();

                drawPoint(x, y, g, 0, true);
            }
            

        }
        void manhattan_distance(int x , int y)
        {
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                List<double> temp = new List<double>();
                int class_no = Data_matrix[i][0];
                int point_x = Data_matrix[i][1];
                int point_y = Data_matrix[i][2];

                double dis = 0.0;
                dis = Math.Abs(point_x - x);
                dis += Math.Abs(point_y - y);

                temp.Add(class_no);
                temp.Add(dis);

                distance_matrix.Add(temp);
            }
        }
        void euclidean_distance(int x , int y)
        {
            
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                List<double> temp = new List<double>();
                int class_no = Data_matrix[i][0];
                int point_x = Data_matrix[i][1];
                int point_y = Data_matrix[i][2];

                double dis = 0.0;
                dis = Math.Pow((point_x - x), 2);
                dis += Math.Pow((point_y - y), 2);
                dis = Math.Sqrt(dis);

                temp.Add(class_no);
                temp.Add(dis);

                distance_matrix.Add(temp);
            }

        }
        void max_distance(int x, int y)
        {
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                List<double> temp = new List<double>();
                int class_no = Data_matrix[i][0];
                int point_x = Data_matrix[i][1];
                int point_y = Data_matrix[i][2];

                double dis = 0.0;
                if (Math.Abs(point_x - x) > Math.Abs(point_y - y))
                {
                    dis = Math.Abs(point_x - x);
                }
                else{
                    dis = Math.Abs(point_y - y);
                }
                
                

                temp.Add(class_no);
                temp.Add(dis);

                distance_matrix.Add(temp);
            }
        }
        List<List<double>> find_k_nearest_neighbor(int k)
        {
            List<List<double>> temp = new List<List<double>>();

            for (int i = 0; i < distance_matrix.Count - 1; i++)
            {
                for (int j = 0; j < distance_matrix.Count - i - 1; j++)
                {
                    if (distance_matrix[j][1] > distance_matrix[j + 1][1])
                    {
                        List<double> t = distance_matrix[j];
                        distance_matrix[j] = distance_matrix[j + 1];
                        distance_matrix[j + 1] = t;
                    }
                }
            }

            for (int i = 0; i < k; i++)
            {
                temp.Add(distance_matrix[i]);
            }
                return temp;
        }
        void classifer_function(List<List<double>> near)
        {
            List<List<int>> frequncy_matrix = new List<List<int>>();
            for (int i = 0; i < near.Count; i++)
            {
                
                List<int> temp = new List<int>();
                if (frequncy_matrix.Count == 0)
                {
                    temp.Add(Int32.Parse(near[i][0].ToString()));
                    temp.Add(1);
                    frequncy_matrix.Add(temp);
                }
                else
                {
                    Boolean exist = false;
                    int id_class = Int32.Parse(near[i][0].ToString());
                    for (int j = 0; j < frequncy_matrix.Count; j++)
                    {
                        if (id_class == frequncy_matrix[j][0])
                        {
                            exist = true;
                            frequncy_matrix[j][1] += 1;
                        }
                    }
                    if (!exist)
                    {
                        List<int> a = new List<int>();
                        a.Add(id_class);
                        a.Add(1);
                        frequncy_matrix.Add(a);
                    }
                }
            }

            // --- find class -----

            int max_value = frequncy_matrix[0][1];
            int max_id = frequncy_matrix[0][0];
            for (int i = 0; i < frequncy_matrix.Count; i++)
            {
                if (frequncy_matrix[i][1] > max_value)
                {
                    max_value = frequncy_matrix[i][1];
                    max_id = frequncy_matrix[i][0];
                }
            }


            // --------------show result --------

            label_result.Text = "This Sample belongs to the category : "+classes_name[max_id];
        }
        void draw()
        {
            // ---------------
            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox1.Refresh();
            // -------------------------

            Graphics g = pictureBox1.CreateGraphics();


            int wid = pictureBox1.Width;
            int high = pictureBox1.Height;
            find_steps();
            // draw xox' 
            Pen pen_x = new Pen(Color.Blue);
            pen_x.EndCap = LineCap.ArrowAnchor;
            Point p1_x = new Point(10, high - 10);
            Point p2_x = new Point(wid - 10, high - 10);
            g.DrawLine(pen_x, p1_x, p2_x);
            for (int i = 0; i < x_max + 1; i++)
            {
                x_step_draw(i, 0, g);
            }
            //------------
            // draw yoy'
            Pen pen_y = new Pen(Color.Blue);
            pen_y.EndCap = LineCap.ArrowAnchor;
            Point p1_y = new Point(10, high - 10);
            Point p2_y = new Point(10, 10);
            g.DrawLine(pen_y, p1_y, p2_y);
            for (int j = 0; j < y_max + 1; j++)
            {
                y_step_draw(0, j, g);

            }
            //pen_y.DashStyle = DashStyle.DashDotDot;
            //g.DrawLine(pen_y, 50, 50, 100, 100);
            // -------------

            // ------ draw points of solutions ------------

            for (int i = 0; i < Data_matrix.Count; i++)
            {
                drawPoint(Data_matrix[i][1], Data_matrix[i][2], g, Data_matrix[i][0],false);
            }


        }
        void x_step_draw(int x, int y, Graphics g)
        {
            int high = pictureBox1.Height;

            int new_x = 10 + x_step * x;
            int new_y = high - 10 - (y_step * y);



            Color color = Color.Blue;
            SolidBrush brush = new SolidBrush(color);
            g.FillEllipse(brush, new_x, new_y, 3, 3);
            // ---------------------
            Font myFont = new Font("Arial", 7);
            SolidBrush brush1 = new SolidBrush(Color.Blue);
            if (x != 0)
            {
                g.DrawString("" + x, myFont, brush1, new_x - 2, new_y - 10);
            }

        }
        void y_step_draw(int x, int y, Graphics g)
        {
            int high = pictureBox1.Height;

            int new_x = 10 + x_step * x;
            int new_y = high - 10 - (y_step * y);

            Color color = Color.Blue;
            SolidBrush brush = new SolidBrush(color);
            g.FillEllipse(brush, new_x, new_y, 3, 3);
            // ---------------------
            Font myFont = new Font("Arial", 7);
            SolidBrush brush1 = new SolidBrush(Color.Blue);
            if (y != 0)
            {
                g.DrawString("" + y, myFont, brush1, new_x - 10, new_y - 5);
            }

        }
        void drawPoint(int x, int y, Graphics g, int idx_point, Boolean check)
        {
            //int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            int new_x = 10 + x_step * x - 3;          // -3 for exacut point in center because think 7
            int new_y = high - 10 - (y_step * y) - 3; // -3 for exacut point in center because think 7

            //Pen p = new Pen(Color.Red, 2);
            
            int r,b,green,c = idx_point * 220 + 150;
            c = c % 255;
            if (check)
            {
                r = 0;
                green = 255;
                b = 0;

                Color color = Color.FromArgb(255, green, b, r);

                SolidBrush brush = new SolidBrush(color);
                Pen p = new Pen(brush);
                
                g.FillRectangle(brush, new_x, new_y, 7, 7);
               
            }
            else
            {
                r = 255 - c;
                green = c;
                b = (c * 2) % 255;

                Color color = Color.FromArgb(255, green, b, r);
                SolidBrush brush = new SolidBrush(color);
                g.FillEllipse(brush, new_x, new_y, 7, 7);
            }
            

            
            

        }
        void find_steps()
        {
            int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            x_max = Data_matrix[0][1];
            y_max = Data_matrix[0][2];
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                if (x_max < Data_matrix[i][1])
                {
                    x_max = Data_matrix[i][1];
                }
                if (y_max < Data_matrix[i][2])
                {
                    y_max = Data_matrix[i][2];
                }
            }
            x_step = (wid - 20) / (x_max + 1);
            y_step = (high - 20) / (y_max + 1);

        }


    }
}
