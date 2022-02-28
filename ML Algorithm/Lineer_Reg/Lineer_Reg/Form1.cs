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

namespace Lineer_Reg
{
    public partial class Form1 : Form
    {
        int x_step = 0;
        int y_step = 0;
        int x_max = 0;
        int y_max = 0;

        Regression r;
        List<SampleData> Data_matrix = new List<SampleData>();
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
            Data_matrix.Clear();
            int row_number = dt.Rows.Count;
            int column_number = dt.Columns.Count;

            for (int i = 1; i < row_number; i++)
            {
                SampleData point = new SampleData();

                point.X = Int32.Parse(dt.Rows[i][0].ToString());
                point.Y = Int32.Parse(dt.Rows[i][1].ToString());

                Data_matrix.Add(point);
            }
            dt.Rows.RemoveAt(0);
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
                drawPoint(Data_matrix[i].X, Data_matrix[i].Y, g, 0, false);
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
        void drawPoint(double x, double y, Graphics g, int idx_point, Boolean check)
        {
            //int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            int new_x = (int)(10 + x_step * x - 3);          // -3 for exacut point in center because think 7
            int new_y = (int)(high - 10 - (y_step * y) - 3); // -3 for exacut point in center because think 7

            //Pen p = new Pen(Color.Red, 2);

            int r, b, green, c = idx_point * 220 + 150;
            c = c % 255;
            r = 255 - c;
            green = c;
            b = (c * 2) % 255;
            if (check)
            {
                Color color = Color.FromArgb(255, 255, 0, 255);
                SolidBrush brush = new SolidBrush(color);
                Pen p = new Pen(brush);
                g.FillRectangle(brush, new_x, new_y, 7, 7);

            }
            else
            {
                Color color = Color.FromArgb(255, 0, 0, 0);
                SolidBrush brush = new SolidBrush(color);
                g.FillEllipse(brush, new_x, new_y, 7, 7);
            }

        }
        void find_steps()
        {
            int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            x_max = (int)Data_matrix[0].X;
            y_max = (int)Data_matrix[0].Y;
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                if (x_max < Data_matrix[i].X)
                {
                    x_max = (int)Data_matrix[i].X;
                }
                if (y_max < Data_matrix[i].Y)
                {
                    y_max = (int)Data_matrix[i].Y;
                }
            }
            x_step = (wid - 20) / (x_max + 1);
            y_step = (high - 20) / (y_max + 1);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            r = new Regression(Data_matrix);
            r.find_const();

            double a = r.get_a();
            double b = r.get_b();
            string s = "Y = " + a;
            if (b > 0)
            {
                s += " + " + b + " * X"; 
            }
            else
            {
                b = b * (-1.0);
                s += " - " + b + " * X";
            }
            
            label2.Text = s;

            // -------------------  draw line for regrission --------------------
            drawLine();
            // -----------------------------------------------------------------

        }

        private void button4_Click(object sender, EventArgs e)
        {
            double x = Double.Parse(textBox1.Text.ToString());
            double y = r.calculate(x);
            label5.Text = y.ToString();
            Graphics g = pictureBox1.CreateGraphics();
            drawPoint(x, y, g, 45, true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            double a = r.get_a();
            double b = r.get_b();
            double error = r.find_rate_error();
            string s = "Y = " + a;
            if (b > 0)
            {
                s += " + " + b + " * X";
            }
            else
            {
                b = b * (-1.0);
                s += " - " + b + " * X";
            }
            if (error > 0)
            {
                s += " + " + error;
            }
            else
            {
                error = error * (-1.0);
                s += " + " + error;
            }
            label3.Text = s;


        }

        void drawLine()
        {


            int high = pictureBox1.Height;
            Graphics g = pictureBox1.CreateGraphics();
            
            int idx = find_min_index();
            int x1 = (int)(10 + x_step * Data_matrix[0].X - 3);          // -3 for exacut point in center because think 7
            int y1 = (int)(high - 10 - (y_step * r.calculate(Data_matrix[0].X)) - 3); // -3 for exacut point in center because think 7

            idx = find_max_index();
            int x2 = (int)(10 + x_step * Data_matrix[idx].X - 3);          // -3 for exacut point in center because think 7
            int y2 = (int)(high - 10 - (y_step * r.calculate(Data_matrix[idx].X)) - 3); // -3 for exacut point in center because think 7

            Pen p = new Pen(Color.Red,2);
            g.DrawLine(p, x1, y1, x2, y2);
        }
        int find_max_index()
        {
            int max = Data_matrix[0].X;
            int idx = 0;
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                if (Data_matrix[i].X > max)
                {
                    max = Data_matrix[i].X;
                    idx = i;
                }
            }
            return idx;
        }
        int find_min_index()
        {
            int min = Data_matrix[0].X;
            int idx = 0;
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                if (Data_matrix[i].X < min)
                {
                    min = Data_matrix[i].X;
                    idx = i;
                }
            }
            return idx;
        }
    }
}
