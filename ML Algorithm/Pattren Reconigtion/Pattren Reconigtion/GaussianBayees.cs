using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattren_Reconigtion
{
    class GaussianBayees
    {
        int size_of_second_column;
        int size_of_first_column;
        List<string> First_Column_Data = new List<string>();        // for find index "j"
        List<string> Second_Column_Data = new List<string>();       // for find index "i"
        List<List<string>> Data_training = new List<List<string>>();// for row data

        List<List<int>> Repeatition_Data = new List<List<int>>();   // for table of repeat
        List<List<double>> Repeatiting_Data_proba = new List<List<double>>();  // for table prbability
        
        
        List<double> second_column_proba = new List<double>();  // for target probablity (Evet/Hayir)
        List<int> second_column_sum = new List<int>();          // for target sum (Evet/hayir)

        List<double> first_column_proba = new List<double>();   // for data probablity (yagmur/.)
        List<int> first_column_sum = new List<int>();           //for data sum (yagmur/.)

        public GaussianBayees(List<List<string>> Data_training_row){
            this.Data_training = Data_training_row;
        }
        void find_Data_for_first_column(){
            List<string> temp = new List<string>();
            List<string> data_second = new List<string>();
            // ------------- for know how many data in first column ---------------
            for (int i = 0; i < Data_training.Count; i++)
            {
                if (First_Column_Data.IndexOf(Data_training[i][0]) < 0)
                {
                    First_Column_Data.Add(Data_training[i][0]);
                }
            }
            size_of_first_column = First_Column_Data.Count;
            // - --------------------------------------------------------
        }
        void find_data_for_second_column(){
            // ------------- for know how many data in second column ---------------
            for (int i = 0; i < Data_training.Count; i++)
            {
                if (Second_Column_Data.IndexOf(Data_training[i][1]) < 0)
                {
                    Second_Column_Data.Add(Data_training[i][1]);
                }
            }
            size_of_second_column = Second_Column_Data.Count;
            // ---------------------------
        }
        void find_repeating_data(){
             // -------- find number of repeating for first column ---------
            // --- i ==== presintation number of second data (Evet/Hayir/...) -----
            // --- j ==== presintation number of first data (Yagmurlu/bulut/gunis/...) ------
            for (int i = 0; i < this.size_of_second_column; i++)
            {
                List<int> init_row = new List<int>();
                for (int j = 0; j < this.size_of_first_column; j++)
                {
                    init_row.Add(0);
                }
                this.Repeatition_Data.Add(init_row);
            }

            for (int m = 0; m < this.Data_training.Count; m++)
            {
                
                int i = this.Second_Column_Data.IndexOf(this.Data_training[m][1]);
                int j = this.First_Column_Data.IndexOf(this.Data_training[m][0]);
                this.Repeatition_Data[i][j] += 1;
            }

        
        }
        public void process()
        {
            this.find_Data_for_first_column();
            this.find_data_for_second_column();
            this.find_repeating_data();
            find_second_column_probability();
            find_first_column_probability();
            find_repeating_data_probability();
        }
        void find_second_column_probability()
        {
            for (int count = 0; count < this.Second_Column_Data.Count; count++)
            {
                int sum = 0;
                for (int j = 0; j < this.First_Column_Data.Count; j++)
                {
                    sum += this.Repeatition_Data[count][j];
                }
                this.second_column_sum.Add(sum);
                this.second_column_proba.Add((sum * 1.0) / this.Data_training.Count);
            }
        }
        void find_first_column_probability()
        {
            for (int count = 0; count < this.First_Column_Data.Count; count++)
            {
                int sum = 0;
                for (int j = 0; j < this.Second_Column_Data.Count; j++)
                {
                    sum += this.Repeatition_Data[j][count];
                }
                this.first_column_sum.Add(sum);
                this.first_column_proba.Add((sum * 1.0) / this.Data_training.Count);
            }
        }
        void find_repeating_data_probability()
        {
            for (int i = 0; i < this.Second_Column_Data.Count; i++)
            {
                List<double> te = new List<double>();
                int total_row = this.second_column_sum[i];
                for (int j = 0; j < this.First_Column_Data.Count; j++)
                {
                    te.Add((this.Repeatition_Data[i][j] * 1.0) / total_row);
                }
                this.Repeatiting_Data_proba.Add(te);
            }
        }
        public double calculate_probablity(string target, string feature) // target
        {
            double result = 0.0;

            int idx_feature = this.First_Column_Data.IndexOf(feature);
            int idx_target = this.Second_Column_Data.IndexOf(target);

            result = this.Repeatiting_Data_proba[idx_target][idx_feature] * this.second_column_proba[idx_target];
            result = result / this.first_column_proba[idx_feature];

            return result;
        }
        public List<string> getFeatures()
        {
            return this.First_Column_Data;
        }
        public List<string> getTarget()
        {
            return this.Second_Column_Data;
        }
    }
}
