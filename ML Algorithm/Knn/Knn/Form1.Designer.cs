namespace Knn
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label_result = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.expect_btn = new System.Windows.Forms.Button();
            this.k_number = new System.Windows.Forms.TextBox();
            this.sample_y = new System.Windows.Forms.TextBox();
            this.sample_x = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.radio_euclidean = new System.Windows.Forms.RadioButton();
            this.radio_max = new System.Windows.Forms.RadioButton();
            this.radio_manhattan = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(125, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(172, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(6, 53);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(291, 388);
            this.dataGridView1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Choose Excel File";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 447);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load Data";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox6);
            this.groupBox3.Controls.Add(this.groupBox5);
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(328, 264);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(428, 195);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Expect";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label_result);
            this.groupBox6.Location = new System.Drawing.Point(6, 146);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(416, 43);
            this.groupBox6.TabIndex = 5;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Result";
            // 
            // label_result
            // 
            this.label_result.AutoSize = true;
            this.label_result.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_result.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label_result.Location = new System.Drawing.Point(53, 18);
            this.label_result.Name = "label_result";
            this.label_result.Size = new System.Drawing.Size(0, 18);
            this.label_result.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.expect_btn);
            this.groupBox5.Controls.Add(this.k_number);
            this.groupBox5.Controls.Add(this.sample_y);
            this.groupBox5.Controls.Add(this.sample_x);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(135, 31);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(287, 109);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sample";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "K nearest ";
            // 
            // expect_btn
            // 
            this.expect_btn.Location = new System.Drawing.Point(177, 19);
            this.expect_btn.Name = "expect_btn";
            this.expect_btn.Size = new System.Drawing.Size(77, 79);
            this.expect_btn.TabIndex = 4;
            this.expect_btn.Text = "Expect";
            this.expect_btn.UseVisualStyleBackColor = true;
            this.expect_btn.Click += new System.EventHandler(this.expect_btn_Click);
            // 
            // k_number
            // 
            this.k_number.Location = new System.Drawing.Point(61, 78);
            this.k_number.Name = "k_number";
            this.k_number.Size = new System.Drawing.Size(72, 20);
            this.k_number.TabIndex = 3;
            this.k_number.Text = "0";
            this.k_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // sample_y
            // 
            this.sample_y.Location = new System.Drawing.Point(61, 49);
            this.sample_y.Name = "sample_y";
            this.sample_y.Size = new System.Drawing.Size(72, 20);
            this.sample_y.TabIndex = 3;
            this.sample_y.Text = "0";
            this.sample_y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // sample_x
            // 
            this.sample_x.Location = new System.Drawing.Point(61, 23);
            this.sample_x.Name = "sample_x";
            this.sample_x.Size = new System.Drawing.Size(72, 20);
            this.sample_x.TabIndex = 2;
            this.sample_x.Text = "0";
            this.sample_x.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Y";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "X";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radio_euclidean);
            this.groupBox4.Controls.Add(this.radio_max);
            this.groupBox4.Controls.Add(this.radio_manhattan);
            this.groupBox4.Location = new System.Drawing.Point(6, 31);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(123, 109);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Distance Function";
            // 
            // radio_euclidean
            // 
            this.radio_euclidean.AutoSize = true;
            this.radio_euclidean.Checked = true;
            this.radio_euclidean.Location = new System.Drawing.Point(6, 19);
            this.radio_euclidean.Name = "radio_euclidean";
            this.radio_euclidean.Size = new System.Drawing.Size(72, 17);
            this.radio_euclidean.TabIndex = 0;
            this.radio_euclidean.TabStop = true;
            this.radio_euclidean.Text = "Euclidean";
            this.radio_euclidean.UseVisualStyleBackColor = true;
            // 
            // radio_max
            // 
            this.radio_max.AutoSize = true;
            this.radio_max.Location = new System.Drawing.Point(6, 65);
            this.radio_max.Name = "radio_max";
            this.radio_max.Size = new System.Drawing.Size(45, 17);
            this.radio_max.TabIndex = 2;
            this.radio_max.Text = "Max";
            this.radio_max.UseVisualStyleBackColor = true;
            // 
            // radio_manhattan
            // 
            this.radio_manhattan.AutoSize = true;
            this.radio_manhattan.Location = new System.Drawing.Point(6, 42);
            this.radio_manhattan.Name = "radio_manhattan";
            this.radio_manhattan.Size = new System.Drawing.Size(76, 17);
            this.radio_manhattan.TabIndex = 1;
            this.radio_manhattan.Text = "Manhattan";
            this.radio_manhattan.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(328, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(428, 246);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 471);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "KNN";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label_result;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button expect_btn;
        private System.Windows.Forms.TextBox sample_y;
        private System.Windows.Forms.TextBox sample_x;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton radio_euclidean;
        private System.Windows.Forms.RadioButton radio_max;
        private System.Windows.Forms.RadioButton radio_manhattan;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox k_number;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

