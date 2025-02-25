namespace Assignment1_2_windowsProject
{
    partial class CalculatorWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.firstNumberBox = new System.Windows.Forms.TextBox();
            this.secondNumberBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.operatorButton = new System.Windows.Forms.Button();
            this.operatorCheckBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ResultLine = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // firstNumberBox
            // 
            this.firstNumberBox.Location = new System.Drawing.Point(175, 93);
            this.firstNumberBox.MaximumSize = new System.Drawing.Size(200, 50);
            this.firstNumberBox.MinimumSize = new System.Drawing.Size(200, 50);
            this.firstNumberBox.Name = "firstNumberBox";
            this.firstNumberBox.Size = new System.Drawing.Size(200, 50);
            this.firstNumberBox.TabIndex = 0;
            this.firstNumberBox.TextChanged += new System.EventHandler(this.firstNumberBox_TextChanged);
            // 
            // secondNumberBox
            // 
            this.secondNumberBox.Location = new System.Drawing.Point(175, 170);
            this.secondNumberBox.MaximumSize = new System.Drawing.Size(200, 50);
            this.secondNumberBox.MinimumSize = new System.Drawing.Size(200, 50);
            this.secondNumberBox.Name = "secondNumberBox";
            this.secondNumberBox.Size = new System.Drawing.Size(200, 50);
            this.secondNumberBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "输入第一个数";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 180);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "输入第二个数";
            // 
            // operatorButton
            // 
            this.operatorButton.Location = new System.Drawing.Point(65, 337);
            this.operatorButton.Name = "operatorButton";
            this.operatorButton.Size = new System.Drawing.Size(310, 61);
            this.operatorButton.TabIndex = 4;
            this.operatorButton.Text = "点击进行运算";
            this.operatorButton.UseVisualStyleBackColor = true;
            this.operatorButton.Click += new System.EventHandler(this.operatorButton_Click);
            // 
            // operatorCheckBox
            // 
            this.operatorCheckBox.FormattingEnabled = true;
            this.operatorCheckBox.Items.AddRange(new object[] {
            "+",
            "-",
            "*",
            "/",
            "%"});
            this.operatorCheckBox.Location = new System.Drawing.Point(175, 265);
            this.operatorCheckBox.MaximumSize = new System.Drawing.Size(200, 0);
            this.operatorCheckBox.MinimumSize = new System.Drawing.Size(200, 0);
            this.operatorCheckBox.Name = "operatorCheckBox";
            this.operatorCheckBox.Size = new System.Drawing.Size(200, 26);
            this.operatorCheckBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 273);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 18);
            this.label3.TabIndex = 6;
            this.label3.Text = "选择运算符";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(552, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "运算结果";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // ResultLine
            // 
            this.ResultLine.Location = new System.Drawing.Point(498, 191);
            this.ResultLine.Name = "ResultLine";
            this.ResultLine.ReadOnly = true;
            this.ResultLine.Size = new System.Drawing.Size(203, 28);
            this.ResultLine.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ResultLine);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.operatorCheckBox);
            this.Controls.Add(this.operatorButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.secondNumberBox);
            this.Controls.Add(this.firstNumberBox);
            this.Name = "CalculatorWindow";
            this.Text = "CalculatorWindow";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox firstNumberBox;
        private System.Windows.Forms.TextBox secondNumberBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button operatorButton;
        private System.Windows.Forms.ComboBox operatorCheckBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ResultLine;
    }
}

