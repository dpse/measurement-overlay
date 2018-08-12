namespace MeasurementOverlay.Forms
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.hotkeyBoxClearAll = new MeasurementOverlay.Forms.Controls.HotkeyBox();
            this.hotkeyBoxToggleHideAll = new MeasurementOverlay.Forms.Controls.HotkeyBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.hotkeyBoxAddGuidelines = new MeasurementOverlay.Forms.Controls.HotkeyBox();
            this.hotkeyBoxAddGrid = new MeasurementOverlay.Forms.Controls.HotkeyBox();
            this.hotkeyBoxToggleCrosshair = new MeasurementOverlay.Forms.Controls.HotkeyBox();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkBoxAutoStart = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.hotkeyBoxClearAll, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.hotkeyBoxToggleHideAll, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.hotkeyBoxAddGuidelines, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.hotkeyBoxAddGrid, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.hotkeyBoxToggleCrosshair, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonAccept, 2, 6);
            this.tableLayoutPanel1.Controls.Add(this.buttonCancel, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxAutoStart, 1, 5);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(323, 184);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // hotkeyBoxClearAll
            // 
            this.hotkeyBoxClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.hotkeyBoxClearAll, 2);
            this.hotkeyBoxClearAll.KeyData = System.Windows.Forms.Keys.None;
            this.hotkeyBoxClearAll.Location = new System.Drawing.Point(127, 107);
            this.hotkeyBoxClearAll.Name = "hotkeyBoxClearAll";
            this.hotkeyBoxClearAll.Size = new System.Drawing.Size(193, 20);
            this.hotkeyBoxClearAll.TabIndex = 9;
            // 
            // hotkeyBoxToggleHideAll
            // 
            this.hotkeyBoxToggleHideAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.hotkeyBoxToggleHideAll, 2);
            this.hotkeyBoxToggleHideAll.KeyData = System.Windows.Forms.Keys.None;
            this.hotkeyBoxToggleHideAll.Location = new System.Drawing.Point(127, 81);
            this.hotkeyBoxToggleHideAll.Name = "hotkeyBoxToggleHideAll";
            this.hotkeyBoxToggleHideAll.Size = new System.Drawing.Size(193, 20);
            this.hotkeyBoxToggleHideAll.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Add guide&lines";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Add &grid";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Toggle c&rosshair";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "&Hide all (toggle visibility)";
            // 
            // hotkeyBoxAddGuidelines
            // 
            this.hotkeyBoxAddGuidelines.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.hotkeyBoxAddGuidelines, 2);
            this.hotkeyBoxAddGuidelines.KeyData = System.Windows.Forms.Keys.None;
            this.hotkeyBoxAddGuidelines.Location = new System.Drawing.Point(127, 3);
            this.hotkeyBoxAddGuidelines.Name = "hotkeyBoxAddGuidelines";
            this.hotkeyBoxAddGuidelines.Size = new System.Drawing.Size(193, 20);
            this.hotkeyBoxAddGuidelines.TabIndex = 1;
            // 
            // hotkeyBoxAddGrid
            // 
            this.hotkeyBoxAddGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.hotkeyBoxAddGrid, 2);
            this.hotkeyBoxAddGrid.KeyData = System.Windows.Forms.Keys.None;
            this.hotkeyBoxAddGrid.Location = new System.Drawing.Point(127, 29);
            this.hotkeyBoxAddGrid.Name = "hotkeyBoxAddGrid";
            this.hotkeyBoxAddGrid.Size = new System.Drawing.Size(193, 20);
            this.hotkeyBoxAddGrid.TabIndex = 3;
            // 
            // hotkeyBoxToggleCrosshair
            // 
            this.hotkeyBoxToggleCrosshair.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.hotkeyBoxToggleCrosshair, 2);
            this.hotkeyBoxToggleCrosshair.KeyData = System.Windows.Forms.Keys.None;
            this.hotkeyBoxToggleCrosshair.Location = new System.Drawing.Point(127, 55);
            this.hotkeyBoxToggleCrosshair.Name = "hotkeyBoxToggleCrosshair";
            this.hotkeyBoxToggleCrosshair.Size = new System.Drawing.Size(193, 20);
            this.hotkeyBoxToggleCrosshair.TabIndex = 5;
            // 
            // buttonAccept
            // 
            this.buttonAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAccept.Location = new System.Drawing.Point(245, 158);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 11;
            this.buttonAccept.Text = "&Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(164, 158);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 12;
            this.buttonCancel.Text = "&Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "C&lear all";
            // 
            // checkBoxAutoStart
            // 
            this.checkBoxAutoStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAutoStart.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxAutoStart, 2);
            this.checkBoxAutoStart.Location = new System.Drawing.Point(127, 133);
            this.checkBoxAutoStart.Name = "checkBoxAutoStart";
            this.checkBoxAutoStart.Size = new System.Drawing.Size(193, 17);
            this.checkBoxAutoStart.TabIndex = 10;
            this.checkBoxAutoStart.Text = "A&utomatically start on boot";
            this.checkBoxAutoStart.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonAccept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(323, 184);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Text = "Settings";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private Controls.HotkeyBox hotkeyBoxAddGuidelines;
        private Controls.HotkeyBox hotkeyBoxToggleHideAll;
        private Controls.HotkeyBox hotkeyBoxAddGrid;
        private Controls.HotkeyBox hotkeyBoxToggleCrosshair;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
        private Controls.HotkeyBox hotkeyBoxClearAll;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxAutoStart;
    }
}