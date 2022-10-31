
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
            this.components = new System.ComponentModel.Container();
            this.bOk = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabVisualization = new System.Windows.Forms.TabPage();
            this.dgvFastInputFiles = new System.Windows.Forms.DataGridView();
            this.cbOutputFile = new System.Windows.Forms.ComboBox();
            this.lbOutputFile = new System.Windows.Forms.Label();
            this.tabFst = new System.Windows.Forms.TabPage();
            this.cbFstInput = new System.Windows.Forms.ComboBox();
            this.gbSimulationControl = new System.Windows.Forms.GroupBox();
            this.chkFstEcho = new System.Windows.Forms.CheckBox();
            this.txtFstTitle = new System.Windows.Forms.TextBox();
            this.lblFstEcho = new System.Windows.Forms.Label();
            this.lblFstTitle = new System.Windows.Forms.Label();
            this.bSelectFile = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuRun = new System.Windows.Forms.ToolStripMenuItem();
            this.visualizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblFstAbortLevel = new System.Windows.Forms.Label();
            this.txtFstAbortLevel = new System.Windows.Forms.TextBox();
            this.lblFstTMax = new System.Windows.Forms.Label();
            this.txtFstTMax = new System.Windows.Forms.TextBox();
            this.lblFstDT = new System.Windows.Forms.Label();
            this.txtFstDT = new System.Windows.Forms.TextBox();
            this.lblFstInterpOrder = new System.Windows.Forms.Label();
            this.txtFstInterpOrder = new System.Windows.Forms.TextBox();
            this.lblFstNumCrctn = new System.Windows.Forms.Label();
            this.txtFstNumCrctn = new System.Windows.Forms.TextBox();
            this.lblFstDT_UJac = new System.Windows.Forms.Label();
            this.txtFstDT_UJac = new System.Windows.Forms.TextBox();
            this.lblFstUJacSclFact = new System.Windows.Forms.Label();
            this.txtFstUJacSclFact = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabVisualization.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFastInputFiles)).BeginInit();
            this.tabFst.SuspendLayout();
            this.gbSimulationControl.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bOk
            // 
            this.bOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bOk.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bOk.Location = new System.Drawing.Point(371, 343);
            this.bOk.Name = "bOk";
            this.bOk.Size = new System.Drawing.Size(87, 36);
            this.bOk.TabIndex = 6;
            this.bOk.Text = "OK";
            this.bOk.UseVisualStyleBackColor = true;
            this.bOk.Click += new System.EventHandler(this.bOk_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabVisualization);
            this.tabControl1.Controls.Add(this.tabFst);
            this.tabControl1.Location = new System.Drawing.Point(12, 34);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(698, 520);
            this.tabControl1.TabIndex = 9;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabVisualization
            // 
            this.tabVisualization.BackColor = System.Drawing.SystemColors.Control;
            this.tabVisualization.Controls.Add(this.dgvFastInputFiles);
            this.tabVisualization.Controls.Add(this.cbOutputFile);
            this.tabVisualization.Controls.Add(this.bOk);
            this.tabVisualization.Controls.Add(this.lbOutputFile);
            this.tabVisualization.Location = new System.Drawing.Point(4, 22);
            this.tabVisualization.Name = "tabVisualization";
            this.tabVisualization.Padding = new System.Windows.Forms.Padding(3);
            this.tabVisualization.Size = new System.Drawing.Size(690, 494);
            this.tabVisualization.TabIndex = 0;
            this.tabVisualization.Text = "Visualization";
            // 
            // dgvFastInputFiles
            // 
            this.dgvFastInputFiles.AllowUserToAddRows = false;
            this.dgvFastInputFiles.AllowUserToDeleteRows = false;
            this.dgvFastInputFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFastInputFiles.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvFastInputFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFastInputFiles.Location = new System.Drawing.Point(41, 56);
            this.dgvFastInputFiles.Name = "dgvFastInputFiles";
            this.dgvFastInputFiles.RowHeadersWidth = 51;
            this.dgvFastInputFiles.Size = new System.Drawing.Size(595, 210);
            this.dgvFastInputFiles.TabIndex = 11;
            // 
            // cbOutputFile
            // 
            this.cbOutputFile.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOutputFile.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbOutputFile.FormattingEnabled = true;
            this.cbOutputFile.Items.AddRange(new object[] {
            "SACS Python API",
            "ISM"});
            this.cbOutputFile.Location = new System.Drawing.Point(41, 351);
            this.cbOutputFile.Name = "cbOutputFile";
            this.cbOutputFile.Size = new System.Drawing.Size(136, 28);
            this.cbOutputFile.TabIndex = 10;
            this.cbOutputFile.SelectedIndexChanged += new System.EventHandler(this.cbOutputFile_SelectedIndexChanged);
            // 
            // lbOutputFile
            // 
            this.lbOutputFile.AutoSize = true;
            this.lbOutputFile.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbOutputFile.Location = new System.Drawing.Point(41, 310);
            this.lbOutputFile.Name = "lbOutputFile";
            this.lbOutputFile.Size = new System.Drawing.Size(96, 21);
            this.lbOutputFile.TabIndex = 9;
            this.lbOutputFile.Text = "Output File";
            // 
            // tabFst
            // 
            this.tabFst.BackColor = System.Drawing.SystemColors.Control;
            this.tabFst.Controls.Add(this.cbFstInput);
            this.tabFst.Controls.Add(this.gbSimulationControl);
            this.tabFst.Location = new System.Drawing.Point(4, 22);
            this.tabFst.Name = "tabFst";
            this.tabFst.Padding = new System.Windows.Forms.Padding(3);
            this.tabFst.Size = new System.Drawing.Size(690, 494);
            this.tabFst.TabIndex = 1;
            this.tabFst.Text = "fst";
            // 
            // cbFstInput
            // 
            this.cbFstInput.FormattingEnabled = true;
            this.cbFstInput.Items.AddRange(new object[] {
            "SIMULATION CONTROL",
            "FEATURE SWITCHES AND FLAGS",
            "ENVIRONMENTAL CONDITIONS",
            "INPUT FILES",
            "OUTPUT",
            "LINEARIZATION",
            "VISUALIZATION"});
            this.cbFstInput.Location = new System.Drawing.Point(18, 17);
            this.cbFstInput.Name = "cbFstInput";
            this.cbFstInput.Size = new System.Drawing.Size(230, 21);
            this.cbFstInput.TabIndex = 1;
            this.cbFstInput.SelectedIndexChanged += new System.EventHandler(this.cbFstInput_SelectedIndexChanged);
            // 
            // gbSimulationControl
            // 
            this.gbSimulationControl.Controls.Add(this.chkFstEcho);
            this.gbSimulationControl.Controls.Add(this.txtFstUJacSclFact);
            this.gbSimulationControl.Controls.Add(this.txtFstDT_UJac);
            this.gbSimulationControl.Controls.Add(this.txtFstNumCrctn);
            this.gbSimulationControl.Controls.Add(this.txtFstInterpOrder);
            this.gbSimulationControl.Controls.Add(this.txtFstDT);
            this.gbSimulationControl.Controls.Add(this.txtFstTMax);
            this.gbSimulationControl.Controls.Add(this.txtFstAbortLevel);
            this.gbSimulationControl.Controls.Add(this.txtFstTitle);
            this.gbSimulationControl.Controls.Add(this.lblFstUJacSclFact);
            this.gbSimulationControl.Controls.Add(this.lblFstDT_UJac);
            this.gbSimulationControl.Controls.Add(this.lblFstNumCrctn);
            this.gbSimulationControl.Controls.Add(this.lblFstInterpOrder);
            this.gbSimulationControl.Controls.Add(this.lblFstDT);
            this.gbSimulationControl.Controls.Add(this.lblFstTMax);
            this.gbSimulationControl.Controls.Add(this.lblFstAbortLevel);
            this.gbSimulationControl.Controls.Add(this.lblFstEcho);
            this.gbSimulationControl.Controls.Add(this.lblFstTitle);
            this.gbSimulationControl.Location = new System.Drawing.Point(6, 50);
            this.gbSimulationControl.Name = "gbSimulationControl";
            this.gbSimulationControl.Size = new System.Drawing.Size(688, 448);
            this.gbSimulationControl.TabIndex = 0;
            this.gbSimulationControl.TabStop = false;
            this.gbSimulationControl.Text = "SIMULATION CONTROL";
            // 
            // chkFstEcho
            // 
            this.chkFstEcho.AutoSize = true;
            this.chkFstEcho.Location = new System.Drawing.Point(94, 72);
            this.chkFstEcho.Name = "chkFstEcho";
            this.chkFstEcho.Size = new System.Drawing.Size(15, 14);
            this.chkFstEcho.TabIndex = 2;
            this.chkFstEcho.UseVisualStyleBackColor = true;
            // 
            // txtFstTitle
            // 
            this.txtFstTitle.Location = new System.Drawing.Point(94, 35);
            this.txtFstTitle.Name = "txtFstTitle";
            this.txtFstTitle.Size = new System.Drawing.Size(312, 20);
            this.txtFstTitle.TabIndex = 1;
            // 
            // lblFstEcho
            // 
            this.lblFstEcho.AutoSize = true;
            this.lblFstEcho.Location = new System.Drawing.Point(19, 73);
            this.lblFstEcho.Name = "lblFstEcho";
            this.lblFstEcho.Size = new System.Drawing.Size(32, 13);
            this.lblFstEcho.TabIndex = 0;
            this.lblFstEcho.Text = "Echo";
            this.toolTip1.SetToolTip(this.lblFstEcho, "Echo input data to <RootName>.ech (flag)");
            // 
            // lblFstTitle
            // 
            this.lblFstTitle.AutoSize = true;
            this.lblFstTitle.Location = new System.Drawing.Point(19, 38);
            this.lblFstTitle.Name = "lblFstTitle";
            this.lblFstTitle.Size = new System.Drawing.Size(27, 13);
            this.lblFstTitle.TabIndex = 0;
            this.lblFstTitle.Text = "Title";
            // 
            // bSelectFile
            // 
            this.bSelectFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.bSelectFile.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bSelectFile.Location = new System.Drawing.Point(793, 34);
            this.bSelectFile.Name = "bSelectFile";
            this.bSelectFile.Size = new System.Drawing.Size(136, 38);
            this.bSelectFile.TabIndex = 0;
            this.bSelectFile.Text = "Select FAST File";
            this.bSelectFile.UseVisualStyleBackColor = true;
            this.bSelectFile.Click += new System.EventHandler(this.bSelectFile_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.Location = new System.Drawing.Point(789, 95);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(57, 21);
            this.lbStatus.TabIndex = 5;
            this.lbStatus.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(794, 144);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(180, 210);
            this.txtStatus.TabIndex = 7;
            this.txtStatus.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visualizeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.menuRun});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1041, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuRun
            // 
            this.menuRun.Name = "menuRun";
            this.menuRun.Size = new System.Drawing.Size(40, 20);
            this.menuRun.Text = "Run";
            this.menuRun.Click += new System.EventHandler(this.menuRun_Click);
            // 
            // visualizeToolStripMenuItem
            // 
            this.visualizeToolStripMenuItem.Name = "visualizeToolStripMenuItem";
            this.visualizeToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.visualizeToolStripMenuItem.Text = "Visualize";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // lblFstAbortLevel
            // 
            this.lblFstAbortLevel.AutoSize = true;
            this.lblFstAbortLevel.Location = new System.Drawing.Point(19, 100);
            this.lblFstAbortLevel.Name = "lblFstAbortLevel";
            this.lblFstAbortLevel.Size = new System.Drawing.Size(58, 13);
            this.lblFstAbortLevel.TabIndex = 0;
            this.lblFstAbortLevel.Text = "AbortLevel";
            this.toolTip1.SetToolTip(this.lblFstAbortLevel, "Error level when simulation should abort (string) {\"WARNING\", \"SEVERE\", \"FATAL\"}");
            // 
            // txtFstAbortLevel
            // 
            this.txtFstAbortLevel.Location = new System.Drawing.Point(94, 97);
            this.txtFstAbortLevel.Name = "txtFstAbortLevel";
            this.txtFstAbortLevel.Size = new System.Drawing.Size(95, 20);
            this.txtFstAbortLevel.TabIndex = 1;
            // 
            // lblFstTMax
            // 
            this.lblFstTMax.AutoSize = true;
            this.lblFstTMax.Location = new System.Drawing.Point(19, 127);
            this.lblFstTMax.Name = "lblFstTMax";
            this.lblFstTMax.Size = new System.Drawing.Size(34, 13);
            this.lblFstTMax.TabIndex = 0;
            this.lblFstTMax.Text = "TMax";
            this.toolTip1.SetToolTip(this.lblFstTMax, "Total run time (s)");
            // 
            // txtFstTMax
            // 
            this.txtFstTMax.Location = new System.Drawing.Point(94, 124);
            this.txtFstTMax.Name = "txtFstTMax";
            this.txtFstTMax.Size = new System.Drawing.Size(95, 20);
            this.txtFstTMax.TabIndex = 1;
            // 
            // lblFstDT
            // 
            this.lblFstDT.AutoSize = true;
            this.lblFstDT.Location = new System.Drawing.Point(19, 153);
            this.lblFstDT.Name = "lblFstDT";
            this.lblFstDT.Size = new System.Drawing.Size(22, 13);
            this.lblFstDT.TabIndex = 0;
            this.lblFstDT.Text = "DT";
            this.toolTip1.SetToolTip(this.lblFstDT, "Recommended module time step (s)");
            // 
            // txtFstDT
            // 
            this.txtFstDT.Location = new System.Drawing.Point(94, 150);
            this.txtFstDT.Name = "txtFstDT";
            this.txtFstDT.Size = new System.Drawing.Size(95, 20);
            this.txtFstDT.TabIndex = 1;
            // 
            // lblFstInterpOrder
            // 
            this.lblFstInterpOrder.AutoSize = true;
            this.lblFstInterpOrder.Location = new System.Drawing.Point(19, 181);
            this.lblFstInterpOrder.Name = "lblFstInterpOrder";
            this.lblFstInterpOrder.Size = new System.Drawing.Size(60, 13);
            this.lblFstInterpOrder.TabIndex = 0;
            this.lblFstInterpOrder.Text = "InterpOrder";
            this.toolTip1.SetToolTip(this.lblFstInterpOrder, "Interpolation order for input/output time history (-) {1=linear, 2=quadratic}");
            // 
            // txtFstInterpOrder
            // 
            this.txtFstInterpOrder.Location = new System.Drawing.Point(94, 178);
            this.txtFstInterpOrder.Name = "txtFstInterpOrder";
            this.txtFstInterpOrder.Size = new System.Drawing.Size(95, 20);
            this.txtFstInterpOrder.TabIndex = 1;
            // 
            // lblFstNumCrctn
            // 
            this.lblFstNumCrctn.AutoSize = true;
            this.lblFstNumCrctn.Location = new System.Drawing.Point(19, 209);
            this.lblFstNumCrctn.Name = "lblFstNumCrctn";
            this.lblFstNumCrctn.Size = new System.Drawing.Size(54, 13);
            this.lblFstNumCrctn.TabIndex = 0;
            this.lblFstNumCrctn.Text = "NumCrctn";
            this.toolTip1.SetToolTip(this.lblFstNumCrctn, "Number of correction iterations (-) {0=explicit calculation, i.e., no corrections" +
        "}");
            // 
            // txtFstNumCrctn
            // 
            this.txtFstNumCrctn.Location = new System.Drawing.Point(94, 206);
            this.txtFstNumCrctn.Name = "txtFstNumCrctn";
            this.txtFstNumCrctn.Size = new System.Drawing.Size(95, 20);
            this.txtFstNumCrctn.TabIndex = 1;
            // 
            // lblFstDT_UJac
            // 
            this.lblFstDT_UJac.AutoSize = true;
            this.lblFstDT_UJac.Location = new System.Drawing.Point(19, 235);
            this.lblFstDT_UJac.Name = "lblFstDT_UJac";
            this.lblFstDT_UJac.Size = new System.Drawing.Size(53, 13);
            this.lblFstDT_UJac.TabIndex = 0;
            this.lblFstDT_UJac.Text = "DT_UJac";
            this.toolTip1.SetToolTip(this.lblFstDT_UJac, "Time between calls to get Jacobians (s)");
            // 
            // txtFstDT_UJac
            // 
            this.txtFstDT_UJac.Location = new System.Drawing.Point(94, 232);
            this.txtFstDT_UJac.Name = "txtFstDT_UJac";
            this.txtFstDT_UJac.Size = new System.Drawing.Size(95, 20);
            this.txtFstDT_UJac.TabIndex = 1;
            // 
            // lblFstUJacSclFact
            // 
            this.lblFstUJacSclFact.AutoSize = true;
            this.lblFstUJacSclFact.Location = new System.Drawing.Point(19, 261);
            this.lblFstUJacSclFact.Name = "lblFstUJacSclFact";
            this.lblFstUJacSclFact.Size = new System.Drawing.Size(68, 13);
            this.lblFstUJacSclFact.TabIndex = 0;
            this.lblFstUJacSclFact.Text = "UJacSclFact";
            this.toolTip1.SetToolTip(this.lblFstUJacSclFact, "Scaling factor used in Jacobians (-)");
            // 
            // txtFstUJacSclFact
            // 
            this.txtFstUJacSclFact.Location = new System.Drawing.Point(94, 258);
            this.txtFstUJacSclFact.Name = "txtFstUJacSclFact";
            this.txtFstUJacSclFact.Size = new System.Drawing.Size(95, 20);
            this.txtFstUJacSclFact.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1041, 566);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.bSelectFile);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "OpenFAST App";
            this.tabControl1.ResumeLayout(false);
            this.tabVisualization.ResumeLayout(false);
            this.tabVisualization.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFastInputFiles)).EndInit();
            this.tabFst.ResumeLayout(false);
            this.gbSimulationControl.ResumeLayout(false);
            this.gbSimulationControl.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bOk;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabVisualization;
        private System.Windows.Forms.TabPage tabFst;
        private System.Windows.Forms.Button bSelectFile;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.RichTextBox txtStatus;
        private System.Windows.Forms.DataGridView dgvFastInputFiles;
        private System.Windows.Forms.ComboBox cbOutputFile;
        private System.Windows.Forms.Label lbOutputFile;
        private System.Windows.Forms.GroupBox gbSimulationControl;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuRun;
        private System.Windows.Forms.ComboBox cbFstInput;
        private System.Windows.Forms.TextBox txtFstTitle;
        private System.Windows.Forms.Label lblFstTitle;
        private System.Windows.Forms.CheckBox chkFstEcho;
        private System.Windows.Forms.Label lblFstEcho;
        private System.Windows.Forms.ToolStripMenuItem visualizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Label lblFstAbortLevel;
        private System.Windows.Forms.TextBox txtFstAbortLevel;
        private System.Windows.Forms.Label lblFstTMax;
        private System.Windows.Forms.TextBox txtFstTMax;
        private System.Windows.Forms.TextBox txtFstDT;
        private System.Windows.Forms.Label lblFstDT;
        private System.Windows.Forms.TextBox txtFstInterpOrder;
        private System.Windows.Forms.Label lblFstInterpOrder;
        private System.Windows.Forms.TextBox txtFstNumCrctn;
        private System.Windows.Forms.Label lblFstNumCrctn;
        private System.Windows.Forms.TextBox txtFstDT_UJac;
        private System.Windows.Forms.Label lblFstDT_UJac;
        private System.Windows.Forms.TextBox txtFstUJacSclFact;
        private System.Windows.Forms.Label lblFstUJacSclFact;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

