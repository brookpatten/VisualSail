namespace AmphibianSoftware.VisualSail.UI
{
    partial class EditBoat
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nameTB = new System.Windows.Forms.TextBox();
            this.numberTB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.colorBTN = new System.Windows.Forms.Button();
            this.typeCB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.okBTN = new System.Windows.Forms.Button();
            this.colorD = new System.Windows.Forms.ColorDialog();
            this.cancelBTN = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Number";
            // 
            // nameTB
            // 
            this.nameTB.Location = new System.Drawing.Point(73, 6);
            this.nameTB.Name = "nameTB";
            this.nameTB.Size = new System.Drawing.Size(293, 20);
            this.nameTB.TabIndex = 2;
            this.nameTB.TextChanged += new System.EventHandler(this.nameTB_TextChanged);
            // 
            // numberTB
            // 
            this.numberTB.Location = new System.Drawing.Point(73, 28);
            this.numberTB.Name = "numberTB";
            this.numberTB.Size = new System.Drawing.Size(293, 20);
            this.numberTB.TabIndex = 3;
            this.numberTB.TextChanged += new System.EventHandler(this.numberTB_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Color";
            // 
            // colorBTN
            // 
            this.colorBTN.Location = new System.Drawing.Point(73, 51);
            this.colorBTN.Name = "colorBTN";
            this.colorBTN.Size = new System.Drawing.Size(293, 23);
            this.colorBTN.TabIndex = 5;
            this.colorBTN.UseVisualStyleBackColor = true;
            this.colorBTN.Click += new System.EventHandler(this.colorBTN_Click);
            // 
            // typeCB
            // 
            this.typeCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeCB.FormattingEnabled = true;
            this.typeCB.Location = new System.Drawing.Point(73, 80);
            this.typeCB.Name = "typeCB";
            this.typeCB.Size = new System.Drawing.Size(293, 21);
            this.typeCB.TabIndex = 6;
            this.typeCB.Visible = false;
            this.typeCB.SelectedIndexChanged += new System.EventHandler(this.typeCB_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Type/Class";
            this.label4.Visible = false;
            // 
            // okBTN
            // 
            this.okBTN.Enabled = false;
            this.okBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.accept;
            this.okBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.okBTN.Location = new System.Drawing.Point(160, 107);
            this.okBTN.Name = "okBTN";
            this.okBTN.Size = new System.Drawing.Size(100, 23);
            this.okBTN.TabIndex = 8;
            this.okBTN.Text = "Ok";
            this.okBTN.UseVisualStyleBackColor = true;
            this.okBTN.Click += new System.EventHandler(this.okBTN_Click);
            // 
            // cancelBTN
            // 
            this.cancelBTN.Image = global::AmphibianSoftware.VisualSail.Properties.Resources.cancel;
            this.cancelBTN.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelBTN.Location = new System.Drawing.Point(266, 107);
            this.cancelBTN.Name = "cancelBTN";
            this.cancelBTN.Size = new System.Drawing.Size(100, 23);
            this.cancelBTN.TabIndex = 9;
            this.cancelBTN.Text = "Cancel";
            this.cancelBTN.UseVisualStyleBackColor = true;
            this.cancelBTN.Click += new System.EventHandler(this.cancelBTN_Click);
            // 
            // EditBoat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 136);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBTN);
            this.Controls.Add(this.okBTN);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.typeCB);
            this.Controls.Add(this.colorBTN);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numberTB);
            this.Controls.Add(this.nameTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditBoat";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit Boat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nameTB;
        private System.Windows.Forms.TextBox numberTB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button colorBTN;
        private System.Windows.Forms.ComboBox typeCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button okBTN;
        private System.Windows.Forms.ColorDialog colorD;
        private System.Windows.Forms.Button cancelBTN;
    }
}