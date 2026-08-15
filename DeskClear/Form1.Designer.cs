namespace DeskClear
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
            btnCloseAll = new Button();
            SuspendLayout();
            // 
            // btnCloseAll
            // 
            btnCloseAll.Location = new Point(103, 199);
            btnCloseAll.Name = "btnCloseAll";
            btnCloseAll.Size = new Size(140, 45);
            btnCloseAll.TabIndex = 0;
            btnCloseAll.Text = "Windows/zero";
            btnCloseAll.UseVisualStyleBackColor = true;
            btnCloseAll.Click += btnCloseAll_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 450);
            Controls.Add(btnCloseAll);
            Name = "Form1";
            Text = "ウィンドウ全部閉じール";
            ResumeLayout(false);
        }

        #endregion

        private Button btnCloseAll;
    }
}
