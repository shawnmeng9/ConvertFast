
namespace ConvertFast
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
            this.bSelectFile = new System.Windows.Forms.Button();
            this.lbFastFile = new System.Windows.Forms.Label();
            this.lbOutputFile = new System.Windows.Forms.Label();
            this.cbFastFile = new System.Windows.Forms.ComboBox();
            this.cbOutputFile = new System.Windows.Forms.ComboBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // bSelectFile
            // 
            this.bSelectFile.Location = new System.Drawing.Point(248, 91);
            this.bSelectFile.Name = "bSelectFile";
            this.bSelectFile.Size = new System.Drawing.Size(136, 38);
            this.bSelectFile.TabIndex = 0;
            this.bSelectFile.Text = "Select FAST File";
            this.bSelectFile.UseVisualStyleBackColor = true;
            this.bSelectFile.Click += new System.EventHandler(this.bSelectFile_Click);
            // 
            // lbFastFile
            // 
            this.lbFastFile.AutoSize = true;
            this.lbFastFile.Location = new System.Drawing.Point(73, 74);
            this.lbFastFile.Name = "lbFastFile";
            this.lbFastFile.Size = new System.Drawing.Size(53, 13);
            this.lbFastFile.TabIndex = 1;
            this.lbFastFile.Text = "FAST File";
            // 
            // lbOutputFile
            // 
            this.lbOutputFile.AutoSize = true;
            this.lbOutputFile.Location = new System.Drawing.Point(76, 238);
            this.lbOutputFile.Name = "lbOutputFile";
            this.lbOutputFile.Size = new System.Drawing.Size(58, 13);
            this.lbOutputFile.TabIndex = 2;
            this.lbOutputFile.Text = "Output File";
            // 
            // cbFastFile
            // 
            this.cbFastFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFastFile.FormattingEnabled = true;
            this.cbFastFile.Items.AddRange(new object[] {
            "HydroDyn"});
            this.cbFastFile.Location = new System.Drawing.Point(76, 101);
            this.cbFastFile.Name = "cbFastFile";
            this.cbFastFile.Size = new System.Drawing.Size(121, 21);
            this.cbFastFile.TabIndex = 3;
            this.cbFastFile.SelectedIndexChanged += new System.EventHandler(this.cbFastFile_SelectedIndexChanged);
            // 
            // cbOutputFile
            // 
            this.cbOutputFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutputFile.FormattingEnabled = true;
            this.cbOutputFile.Items.AddRange(new object[] {
            "SACS Python API",
            "ISM"});
            this.cbOutputFile.Location = new System.Drawing.Point(76, 273);
            this.cbOutputFile.Name = "cbOutputFile";
            this.cbOutputFile.Size = new System.Drawing.Size(121, 21);
            this.cbOutputFile.TabIndex = 4;
            this.cbOutputFile.SelectedIndexChanged += new System.EventHandler(this.cbOutputFile_SelectedIndexChanged);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(509, 48);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(37, 13);
            this.lbStatus.TabIndex = 5;
            this.lbStatus.Text = "Status";
            // 
            // bOk
            // 
            this.bOk.Location = new System.Drawing.Point(470, 348);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(87, 36);
            this.bOk.TabIndex = 6;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(512, 84);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(180, 210);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.cbOutputFile);
            this.Controls.Add(this.cbFastFile);
            this.Controls.Add(this.lbOutputFile);
            this.Controls.Add(this.lbFastFile);
            this.Controls.Add(this.bSelectFile);
            this.Name = "Form1";
            this.Text = "Convert FAST";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSelectFile;
        private System.Windows.Forms.Label lbFastFile;
        private System.Windows.Forms.Label lbOutputFile;
        private System.Windows.Forms.ComboBox cbFastFile;
        private System.Windows.Forms.ComboBox cbOutputFile;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.RichTextBox txtStatus;
    }
}

