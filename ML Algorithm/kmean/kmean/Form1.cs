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

namespace kmean
{
    public partial class Form1 : Form
    {
        int x_step = 0;
        int y_step = 0;
        int x_max = 0;
        int y_max = 0;

        int k;
        List<SampleData> Data_matrix = new List<SampleData>();
        List<List<int>> G = new List<List<int>>();    // row is class / column is all point
        List<SampleData> Centroid_Cluster = new List<SampleData>(); // for save center cluster

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
        
        private void load_btn_Click(object sender, EventArgs e)
        {
            btnChooseFile_Click(sender, e);
            draw();
        }
        private void traning_btn_Click(object sender, EventArgs e)
        {
            int point_count = Data_matrix.Count;
            k = Int32.Parse(centroid_num_txt.Text);
            List<List<int>> temp_g = new List<List<int>>();  // row is class / column is all point
            List<List<double>> D = new List<List<double>>(); // row is class / column is all point

            init_matrix(temp_g, D,true);

            bool firstLoop = true;

            while (!checkFinish(temp_g,firstLoop))
            {
                // ---------- assume temp_g value into G value ----------------
                copy_detect_cluster_List(temp_g);
                find_center_clusters(firstLoop);
                // ------------------------------------------------------------
                init_matrix(temp_g, D, false);
                if (firstLoop){
                    firstLoop = false;
                }
                distance_calc(D);
                detect_cluster(temp_g, D);
            }

            // --------------- draw Points ------------------------

            Graphics g = pictureBox1.CreateGraphics();
            draw();
            for (int i = 0; i < k; i++)
            {
                drawPoint(Centroid_Cluster[i].X, Centroid_Cluster[i].Y, g, Centroid_Cluster[i].class_number, true);
            }

            //-----------------------------------------------------


        }
        
        void init_matrix(List<List<int>> g, List<List<double>> d,Boolean firstLoop)
        {
            if (firstLoop)
            {
                G.Clear();
                Centroid_Cluster.Clear();
            }
            
            g.Clear();
            d.Clear();
            for (int i = 0; i < k; i++)
            {
                List<int> t = new List<int>();
                List<double> t1 = new List<double>();
                for (int j = 0; j < Data_matrix.Count; j++)
                {
                    t.Add(0);
                    t1.Add(0.0);
                }
                g.Add(t);
                d.Add(t1);
                if (firstLoop)
                {
                    G.Add(t);
                    SampleData sd = new SampleData();
                    sd.X = Data_matrix[i].X;
                    sd.Y = Data_matrix[i].Y;
                    Centroid_Cluster.Add(sd);
                }
                
            }

        }
        Boolean checkFinish(List<List<int>> g, bool firstLoop)
        {
            if(firstLoop)
                return false;

            for (int i = 0; i < g.Count; i++)
            {
                for (int j = 0; j < g[0].Count; j++)
                {
                    if (G[i][j] != g[i][j])
                        return false;
                }
            }

            return true;
        }
        void distance_calc(List<List<double>> d)
        {
            for (int i = 0; i < d.Count; i++)
            {
                double x = Centroid_Cluster[i].X;
                double y = Centroid_Cluster[i].Y;
                double res = 0.0;
                for (int j = 0; j < d[0].Count; j++)
                {
                    double x1 = Data_matrix[j].X;
                    double y1 = Data_matrix[j].Y;
                    res = Math.Pow((x - x1), 2);
                    res += Math.Pow((y - y1), 2);
                    res = Math.Sqrt(res);
                    res = Math.Round(res,2);
                    d[i][j] = res;
                }
            }
        }
        void detect_cluster(List<List<int>> g, List<List<double>> d)
        {
            int min_idx = 0;
            double min_value = 0.0;
            for (int j = 0; j < g[0].Count; j++)
            {
                min_idx = 0;
                min_value = d[0][j];
                for (int i = 0; i < g.Count; i++)
                {
                    if (min_value > d[i][j])
                    {
                        min_idx = i;
                        min_value = d[i][j];
                    }
                }
                for (int i = 0; i < g.Count; i++)
                {
                    if (i == min_idx)
                    {
                        g[i][j] = 1;
                    }
                    else
                    {
                        g[i][j] = 0;
                    }
                }
            }
        }
        void copy_detect_cluster_List(List<List<int>> g)
        {
            for (int i = 0; i < g.Count; i++)
            {
                for (int j = 0; j < g[0].Count; j++)
                {
                    int v = g[i][j];
                    G[i][j] = v;
                }
            }
        }
        void find_center_clusters(Boolean firstLoop)
        {
            if (firstLoop)
                return;
            for (int i = 0; i < k; i++)
            {
                List<SampleData> point_in_same_cluster = find_point_in_same_cluster(i);
                calulate_average_point(i, point_in_same_cluster);
            }
        }
        List<SampleData> find_point_in_same_cluster(int idx)
        {
            List<SampleData> sd = new List<SampleData>();;

            for (int i = 0; i < G[0].Count; i++)
            {
                if (G[idx][i] == 1)
                {
                    SampleData s = new SampleData();
                    s.X = Data_matrix[i].X;
                    s.Y = Data_matrix[i].Y;
                    Data_matrix[i].class_number = idx;
                    sd.Add(s);
                }
            }

            return sd;
        }
        void calulate_average_point(int idx, List<SampleData> points)
        {
            double sum_x = 0.0;
            double sum_y = 0.0;
            int numberOfPoint = points.Count;
            for (int i = 0; i < numberOfPoint; i++)
            {
                sum_x += points[i].X;
                sum_y += points[i].Y;
            }
            Centroid_Cluster[idx].X = Math.Round((sum_x / numberOfPoint),2);
            Centroid_Cluster[idx].Y = Math.Round((sum_y / numberOfPoint),2);
            Centroid_Cluster[idx].class_number = idx;
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
                drawPoint(Data_matrix[i].X, Data_matrix[i].Y, g, Data_matrix[i].class_number, false);
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

            int new_x =(int) (10 + x_step * x - 3);          // -3 for exacut point in center because think 7
            int new_y = (int) (high - 10 - (y_step * y) - 3); // -3 for exacut point in center because think 7

            //Pen p = new Pen(Color.Red, 2);

            int r, b, green, c = idx_point * 220 + 150;
            c = c % 255;
            r = 255 - c;
            green = c;
            b = (c * 2) % 255;
            if (check)
            {
                Color color = Color.FromArgb(255, green, b, r);
                SolidBrush brush = new SolidBrush(color);
                Pen p = new Pen(brush);
                g.FillRectangle(brush, new_x, new_y, 7, 7);

            }
            else
            {
                Color color = Color.FromArgb(255, green, b, r);
                SolidBrush brush = new SolidBrush(color);
                g.FillEllipse(brush, new_x, new_y, 7, 7);
            }

        }
        void find_steps()
        {
            int wid = pictureBox1.Width;
            int high = pictureBox1.Height;

            x_max =(int) Data_matrix[0].X;
            y_max =(int) Data_matrix[0].Y;
            for (int i = 0; i < Data_matrix.Count; i++)
            {
                if (x_max < Data_matrix[i].X)
                {
                    x_max =(int) Data_matrix[i].X;
                }
                if (y_max < Data_matrix[i].Y)
                {
                    y_max =(int) Data_matrix[i].Y;
                }
            }
            x_step = (wid - 20) / (x_max + 1);
            y_step = (high - 20) / (y_max + 1);

        }


    }
}
