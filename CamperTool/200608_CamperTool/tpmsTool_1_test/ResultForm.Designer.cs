namespace camperTool_TEST
{
    partial class ResultForm
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
            this.debugTxtBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // debugTxtBox
            // 
            this.debugTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugTxtBox.BackColor = System.Drawing.Color.White;
            this.debugTxtBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.debugTxtBox.Location = new System.Drawing.Point(12, 12);
            this.debugTxtBox.Name = "debugTxtBox";
            this.debugTxtBox.ReadOnly = true;
            this.debugTxtBox.Size = new System.Drawing.Size(267, 634);
            this.debugTxtBox.TabIndex = 40;
            this.debugTxtBox.Text = "";
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(291, 678);
            this.Controls.Add(this.debugTxtBox);
            this.MaximizeBox = false;
            this.Name = "ResultForm";
            this.Text = "ResultForm";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.RichTextBox debugTxtBox;
    }
}