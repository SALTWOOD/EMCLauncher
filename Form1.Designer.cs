namespace EMCL
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLaunch = new Button();
            cmbJavaList = new ComboBox();
            btnChooseJava = new Button();
            btnJavaSearch = new Button();
            lblTips = new Label();
            SuspendLayout();
            // 
            // btnLaunch
            // 
            btnLaunch.Location = new Point(12, 376);
            btnLaunch.Name = "btnLaunch";
            btnLaunch.Size = new Size(137, 62);
            btnLaunch.TabIndex = 0;
            btnLaunch.Text = "启动游戏";
            btnLaunch.UseVisualStyleBackColor = true;
            btnLaunch.Click += btnLaunch_Click;
            // 
            // cmbJavaList
            // 
            cmbJavaList.FormattingEnabled = true;
            cmbJavaList.Location = new Point(155, 410);
            cmbJavaList.Name = "cmbJavaList";
            cmbJavaList.Size = new Size(633, 28);
            cmbJavaList.TabIndex = 1;
            cmbJavaList.Text = "Java 列表";
            cmbJavaList.SelectedIndexChanged += cmbJavaList_SelectedIndexChanged;
            // 
            // btnChooseJava
            // 
            btnChooseJava.Location = new Point(155, 376);
            btnChooseJava.Name = "btnChooseJava";
            btnChooseJava.Size = new Size(94, 29);
            btnChooseJava.TabIndex = 2;
            btnChooseJava.Text = "手动选择";
            btnChooseJava.UseVisualStyleBackColor = true;
            btnChooseJava.Click += btnChooseJava_Click;
            // 
            // btnJavaSearch
            // 
            btnJavaSearch.Location = new Point(255, 376);
            btnJavaSearch.Name = "btnJavaSearch";
            btnJavaSearch.Size = new Size(94, 29);
            btnJavaSearch.TabIndex = 3;
            btnJavaSearch.Text = "扫描 Java";
            btnJavaSearch.UseVisualStyleBackColor = true;
            btnJavaSearch.Click += btnJavaSearch_Click;
            // 
            // lblTips
            // 
            lblTips.AutoSize = true;
            lblTips.Location = new Point(12, 9);
            lblTips.Name = "lblTips";
            lblTips.Size = new Size(0, 20);
            lblTips.TabIndex = 4;
            lblTips.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblTips);
            Controls.Add(btnJavaSearch);
            Controls.Add(btnChooseJava);
            Controls.Add(cmbJavaList);
            Controls.Add(btnLaunch);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            FormClosed += Form1_FormClosed;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLaunch;
        private ComboBox cmbJavaList;
        private Button btnChooseJava;
        private Button btnJavaSearch;
        private Label lblTips;
    }
}