namespace TestUtility
{
    partial class MainWindow
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
            this.gViewer1 = new Microsoft.Glee.GraphViewerGdi.GViewer();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.IterationsCountBoxRun = new System.Windows.Forms.NumericUpDown();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.IterationsCountBoxBuild = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.PerfTestButton = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.TestRegexEqualityButton = new System.Windows.Forms.Button();
            this.ORegexPatternBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RegexPatternBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.InputTextBox = new System.Windows.Forms.RichTextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IterationsCountBoxRun)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IterationsCountBoxBuild)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // gViewer1
            // 
            this.gViewer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gViewer1.AsyncLayout = false;
            this.gViewer1.AutoScroll = true;
            this.gViewer1.BackColor = System.Drawing.Color.White;
            this.gViewer1.BackwardEnabled = false;
            this.gViewer1.ForwardEnabled = false;
            this.gViewer1.Graph = null;
            this.gViewer1.Location = new System.Drawing.Point(299, 6);
            this.gViewer1.MouseHitDistance = 0.05D;
            this.gViewer1.Name = "gViewer1";
            this.gViewer1.NavigationVisible = false;
            this.gViewer1.PanButtonPressed = false;
            this.gViewer1.SaveButtonVisible = false;
            this.gViewer1.Size = new System.Drawing.Size(583, 352);
            this.gViewer1.TabIndex = 0;
            this.gViewer1.ZoomF = 1D;
            this.gViewer1.ZoomFraction = 0.5D;
            this.gViewer1.ZoomWindowThreshold = 0.05D;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.ForeColor = System.Drawing.Color.Black;
            this.richTextBox1.Location = new System.Drawing.Point(6, 134);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(287, 199);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "^\n\t\t\t{a}(?<group1>{a})\n| {a}{a}*?\n| ({a}{a}({a})?)\n\n///i write some regex\n/*asdfs" +
    "df*/\n\n| [{a}{a}]\n| [^{a}{a}]\n| (?<={a})\n| .\n| {a}{2,}\n| {a}{2,3}?\n$";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(6, 339);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(287, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Render";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBox2.Location = new System.Drawing.Point(6, 6);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.ReadOnly = true;
            this.richTextBox2.Size = new System.Drawing.Size(287, 94);
            this.richTextBox2.TabIndex = 3;
            this.richTextBox2.Text = "Welcome to Object Regular Expressions test utility.\n\nSyntax is very .NET Regular " +
    "Expression\'like except it can contain standard C\'like comments.\n\nVariable defini" +
    "tion: {varName}\n";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(899, 394);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.richTextBox1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.gViewer1);
            this.tabPage1.Controls.Add(this.richTextBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(891, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Expression Tests";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.IterationsCountBoxRun);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.IterationsCountBoxBuild);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.PerfTestButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(891, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Performance Tests";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 206);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Iterations count:";
            // 
            // IterationsCountBoxRun
            // 
            this.IterationsCountBoxRun.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.IterationsCountBoxRun.Location = new System.Drawing.Point(100, 125);
            this.IterationsCountBoxRun.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.IterationsCountBoxRun.Name = "IterationsCountBoxRun";
            this.IterationsCountBoxRun.Size = new System.Drawing.Size(123, 20);
            this.IterationsCountBoxRun.TabIndex = 5;
            this.IterationsCountBoxRun.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(8, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(216, 52);
            this.button2.TabIndex = 4;
            this.button2.Text = "Run performance test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Iterations count:";
            // 
            // IterationsCountBoxBuild
            // 
            this.IterationsCountBoxBuild.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.IterationsCountBoxBuild.Location = new System.Drawing.Point(100, 6);
            this.IterationsCountBoxBuild.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.IterationsCountBoxBuild.Name = "IterationsCountBoxBuild";
            this.IterationsCountBoxBuild.Size = new System.Drawing.Size(123, 20);
            this.IterationsCountBoxBuild.TabIndex = 2;
            this.IterationsCountBoxBuild.Value = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 1;
            // 
            // PerfTestButton
            // 
            this.PerfTestButton.Location = new System.Drawing.Point(8, 32);
            this.PerfTestButton.Name = "PerfTestButton";
            this.PerfTestButton.Size = new System.Drawing.Size(215, 51);
            this.PerfTestButton.TabIndex = 0;
            this.PerfTestButton.Text = "Build performance test";
            this.PerfTestButton.UseVisualStyleBackColor = true;
            this.PerfTestButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.TestRegexEqualityButton);
            this.tabPage3.Controls.Add(this.ORegexPatternBox);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.RegexPatternBox);
            this.tabPage3.Controls.Add(this.label4);
            this.tabPage3.Controls.Add(this.InputTextBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(891, 368);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Regex Test";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // TestRegexEqualityButton
            // 
            this.TestRegexEqualityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.TestRegexEqualityButton.Location = new System.Drawing.Point(43, 310);
            this.TestRegexEqualityButton.Name = "TestRegexEqualityButton";
            this.TestRegexEqualityButton.Size = new System.Drawing.Size(184, 23);
            this.TestRegexEqualityButton.TabIndex = 5;
            this.TestRegexEqualityButton.Text = "Test patterns equality";
            this.TestRegexEqualityButton.UseVisualStyleBackColor = true;
            this.TestRegexEqualityButton.Click += new System.EventHandler(this.TestRegexEqualityButton_Click);
            // 
            // ORegexPatternBox
            // 
            this.ORegexPatternBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ORegexPatternBox.Location = new System.Drawing.Point(12, 74);
            this.ORegexPatternBox.Name = "ORegexPatternBox";
            this.ORegexPatternBox.Size = new System.Drawing.Size(407, 20);
            this.ORegexPatternBox.TabIndex = 4;
            this.ORegexPatternBox.Text = "{a}(?<g1>{b})+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "ORegex pattern";
            // 
            // RegexPatternBox
            // 
            this.RegexPatternBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RegexPatternBox.Location = new System.Drawing.Point(12, 21);
            this.RegexPatternBox.Name = "RegexPatternBox";
            this.RegexPatternBox.Size = new System.Drawing.Size(407, 20);
            this.RegexPatternBox.TabIndex = 2;
            this.RegexPatternBox.Text = "a(?<g1>b)+";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Regex pattern";
            // 
            // InputTextBox
            // 
            this.InputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputTextBox.Location = new System.Drawing.Point(425, 3);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(457, 355);
            this.InputTextBox.TabIndex = 0;
            this.InputTextBox.Text = "abbbbabbbbbb";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 392);
            this.Controls.Add(this.tabControl1);
            this.MinimumSize = new System.Drawing.Size(521, 282);
            this.Name = "MainWindow";
            this.Text = "ORegex Test Utility";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IterationsCountBoxRun)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IterationsCountBoxBuild)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Glee.GraphViewerGdi.GViewer gViewer1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Button PerfTestButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown IterationsCountBoxBuild;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox InputTextBox;
        private System.Windows.Forms.TextBox RegexPatternBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ORegexPatternBox;
        private System.Windows.Forms.Button TestRegexEqualityButton;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown IterationsCountBoxRun;
        private System.Windows.Forms.Label label7;
    }
}

