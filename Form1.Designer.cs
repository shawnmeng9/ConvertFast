
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
            this.lbOutputFile = new System.Windows.Forms.Label();
            this.cbOutputFile = new System.Windows.Forms.ComboBox();
            this.lbStatus = new System.Windows.Forms.Label();
            this.bOk = new System.Windows.Forms.Button();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.dgvFastInputFiles = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFastInputFiles)).BeginInit();
            this.SuspendLayout();
            // 
            // bSelectFile
            // 
            this.bSelectFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bSelectFile.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSelectFile.Location = new System.Drawing.Point(51, 23);
            this.bSelectFile.Name = "bSelectFile";
            this.bSelectFile.Size = new System.Drawing.Size(136, 38);
            this.bSelectFile.TabIndex = 0;
            this.bSelectFile.Text = "Select FAST File";
            this.bSelectFile.UseVisualStyleBackColor = true;
            this.bSelectFile.Click += new System.EventHandler(this.bSelectFile_Click);
            // 
            // lbOutputFile
            // 
            this.lbOutputFile.AutoSize = true;
            this.lbOutputFile.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOutputFile.Location = new System.Drawing.Point(51, 333);
            this.lbOutputFile.Name = "lbOutputFile";
            this.lbOutputFile.Size = new System.Drawing.Size(96, 21);
            this.lbOutputFile.TabIndex = 2;
            this.lbOutputFile.Text = "Output File";
            // 
            // cbOutputFile
            // 
            this.cbOutputFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutputFile.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOutputFile.FormattingEnabled = true;
            this.cbOutputFile.Items.AddRange(new object[] {
            "SACS Python API",
            "ISM"});
            this.cbOutputFile.Location = new System.Drawing.Point(51, 374);
            this.cbOutputFile.Name = "cbOutputFile";
            this.cbOutputFile.Size = new System.Drawing.Size(136, 28);
            this.cbOutputFile.TabIndex = 4;
            this.cbOutputFile.SelectedIndexChanged += new System.EventHandler(this.cbOutputFile_SelectedIndexChanged);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(713, 35);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(57, 21);
            this.lbStatus.TabIndex = 5;
            this.lbStatus.Text = "Status";
            // 
            // bOk
            // 
            this.bOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOk.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bOk.Location = new System.Drawing.Point(718, 335);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(87, 36);
            this.bOk.TabIndex = 6;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(718, 84);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(180, 210);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.Text = "";
            // 
            // dgvFastInputFiles
            // 
            this.dgvFastInputFiles.AllowUserToAddRows = false;
            this.dgvFastInputFiles.AllowUserToDeleteRows = false;
            this.dgvFastInputFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFastInputFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvFastInputFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFastInputFiles.Location = new System.Drawing.Point(51, 84);
            this.dgvFastInputFiles.Name = "dgvFastInputFiles";
            this.dgvFastInputFiles.RowHeadersWidth = 51;
            this.dgvFastInputFiles.Size = new System.Drawing.Size(595, 210);
            this.dgvFastInputFiles.TabIndex = 8;
            this.dgvFastInputFiles.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFastInputFiles_CellValueChanged);
            this.dgvFastInputFiles.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvFastInputFiles_CurrentCellDirtyStateChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 566);
            this.Controls.Add(this.dgvFastInputFiles);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.bOk);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.cbOutputFile);
            this.Controls.Add(this.lbOutputFile);
            this.Controls.Add(this.bSelectFile);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Convert FAST";
            ((System.ComponentModel.ISupportInitialize)(this.dgvFastInputFiles)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSelectFile;
        private System.Windows.Forms.Label lbOutputFile;
        private System.Windows.Forms.ComboBox cbOutputFile;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.DataGridView dgvFastInputFiles;
    }
}

