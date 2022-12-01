namespace GQKN.Archive
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnArchive = new System.Windows.Forms.Button();
            this.lblEndTime = new System.Windows.Forms.Label();
            this.tblCommitSolr = new System.Windows.Forms.TabControl();
            this.tabArchive = new System.Windows.Forms.TabPage();
            this.btnClearTextbox = new System.Windows.Forms.Button();
            this.lblGuide = new System.Windows.Forms.Label();
            this.btnUpdateTemp = new System.Windows.Forms.Button();
            this.btnCommitSolr2 = new System.Windows.Forms.Button();
            this.btnComitSolr = new System.Windows.Forms.Button();
            this.lblDelete = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblCopy = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblStartTime = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnActive2End = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtMessageUpdateSolr = new System.Windows.Forms.TextBox();
            this.btnUpdateArchive = new System.Windows.Forms.Button();
            this.lblArchiveId = new System.Windows.Forms.Label();
            this.lblKhieuNaiId = new System.Windows.Forms.Label();
            this.txtArchiveId = new System.Windows.Forms.TextBox();
            this.txtKhieuNaiId = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tblCommitSolr.SuspendLayout();
            this.tabArchive.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(25, 71);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(46, 13);
            this.lblFromDate.TabIndex = 0;
            this.lblFromDate.Text = "Từ ngày";
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(293, 71);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(53, 13);
            this.lblToDate.TabIndex = 1;
            this.lblToDate.Text = "Đến ngày";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.CustomFormat = "dd/MM/yyyy";
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFromDate.Location = new System.Drawing.Point(118, 64);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(100, 20);
            this.dtpFromDate.TabIndex = 2;
            this.dtpFromDate.Value = new System.DateTime(2016, 1, 1, 0, 0, 0, 0);
            // 
            // dtpToDate
            // 
            this.dtpToDate.CustomFormat = "dd/MM/yyyy";
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpToDate.Location = new System.Drawing.Point(359, 64);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(100, 20);
            this.dtpToDate.TabIndex = 3;
            this.dtpToDate.Value = new System.DateTime(2016, 1, 31, 0, 0, 0, 0);
            // 
            // btnArchive
            // 
            this.btnArchive.Location = new System.Drawing.Point(28, 102);
            this.btnArchive.Name = "btnArchive";
            this.btnArchive.Size = new System.Drawing.Size(90, 23);
            this.btnArchive.TabIndex = 4;
            this.btnArchive.Text = "Lưu trữ";
            this.btnArchive.UseVisualStyleBackColor = true;
            this.btnArchive.Click += new System.EventHandler(this.btnArchive_Click);
            // 
            // lblEndTime
            // 
            this.lblEndTime.AutoSize = true;
            this.lblEndTime.Location = new System.Drawing.Point(346, 141);
            this.lblEndTime.Name = "lblEndTime";
            this.lblEndTime.Size = new System.Drawing.Size(49, 13);
            this.lblEndTime.TabIndex = 7;
            this.lblEndTime.Text = "00:00:00";
            // 
            // tblCommitSolr
            // 
            this.tblCommitSolr.Controls.Add(this.tabArchive);
            this.tblCommitSolr.Controls.Add(this.tabPage1);
            this.tblCommitSolr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblCommitSolr.Location = new System.Drawing.Point(0, 24);
            this.tblCommitSolr.Name = "tblCommitSolr";
            this.tblCommitSolr.SelectedIndex = 0;
            this.tblCommitSolr.Size = new System.Drawing.Size(769, 484);
            this.tblCommitSolr.TabIndex = 9;
            // 
            // tabArchive
            // 
            this.tabArchive.Controls.Add(this.btnClearTextbox);
            this.tabArchive.Controls.Add(this.lblGuide);
            this.tabArchive.Controls.Add(this.btnUpdateTemp);
            this.tabArchive.Controls.Add(this.btnCommitSolr2);
            this.tabArchive.Controls.Add(this.btnComitSolr);
            this.tabArchive.Controls.Add(this.lblDelete);
            this.tabArchive.Controls.Add(this.lblTotal);
            this.tabArchive.Controls.Add(this.lblCopy);
            this.tabArchive.Controls.Add(this.label4);
            this.tabArchive.Controls.Add(this.label5);
            this.tabArchive.Controls.Add(this.label3);
            this.tabArchive.Controls.Add(this.btnStop);
            this.tabArchive.Controls.Add(this.txtMessage);
            this.tabArchive.Controls.Add(this.lblFromDate);
            this.tabArchive.Controls.Add(this.lblStartTime);
            this.tabArchive.Controls.Add(this.lblToDate);
            this.tabArchive.Controls.Add(this.lblEndTime);
            this.tabArchive.Controls.Add(this.dtpFromDate);
            this.tabArchive.Controls.Add(this.label8);
            this.tabArchive.Controls.Add(this.dtpToDate);
            this.tabArchive.Controls.Add(this.label7);
            this.tabArchive.Controls.Add(this.btnActive2End);
            this.tabArchive.Controls.Add(this.btnArchive);
            this.tabArchive.Location = new System.Drawing.Point(4, 22);
            this.tabArchive.Name = "tabArchive";
            this.tabArchive.Padding = new System.Windows.Forms.Padding(3);
            this.tabArchive.Size = new System.Drawing.Size(761, 458);
            this.tabArchive.TabIndex = 0;
            this.tabArchive.Text = "Archive dữ liệu";
            this.tabArchive.UseVisualStyleBackColor = true;
            // 
            // btnClearTextbox
            // 
            this.btnClearTextbox.Location = new System.Drawing.Point(663, 161);
            this.btnClearTextbox.Name = "btnClearTextbox";
            this.btnClearTextbox.Size = new System.Drawing.Size(90, 23);
            this.btnClearTextbox.TabIndex = 18;
            this.btnClearTextbox.Text = "Clear Message";
            this.btnClearTextbox.UseVisualStyleBackColor = true;
            this.btnClearTextbox.Click += new System.EventHandler(this.btnClearTextbox_Click);
            // 
            // lblGuide
            // 
            this.lblGuide.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuide.Location = new System.Drawing.Point(25, 13);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(709, 48);
            this.lblGuide.TabIndex = 17;
            this.lblGuide.Text = resources.GetString("lblGuide.Text");
            // 
            // btnUpdateTemp
            // 
            this.btnUpdateTemp.Location = new System.Drawing.Point(228, 102);
            this.btnUpdateTemp.Name = "btnUpdateTemp";
            this.btnUpdateTemp.Size = new System.Drawing.Size(118, 23);
            this.btnUpdateTemp.TabIndex = 15;
            this.btnUpdateTemp.Text = "Bảng tạm 4 Solr";
            this.btnUpdateTemp.UseVisualStyleBackColor = true;
            this.btnUpdateTemp.Click += new System.EventHandler(this.btnUpdateTemp_Click);
            // 
            // btnCommitSolr2
            // 
            this.btnCommitSolr2.Location = new System.Drawing.Point(470, 102);
            this.btnCommitSolr2.Name = "btnCommitSolr2";
            this.btnCommitSolr2.Size = new System.Drawing.Size(104, 23);
            this.btnCommitSolr2.TabIndex = 15;
            this.btnCommitSolr2.Text = "Cập nhật Solr (∞)";
            this.btnCommitSolr2.UseVisualStyleBackColor = true;
            this.btnCommitSolr2.Click += new System.EventHandler(this.btnCommitSolr2_Click);
            // 
            // btnComitSolr
            // 
            this.btnComitSolr.Location = new System.Drawing.Point(356, 102);
            this.btnComitSolr.Name = "btnComitSolr";
            this.btnComitSolr.Size = new System.Drawing.Size(104, 23);
            this.btnComitSolr.TabIndex = 15;
            this.btnComitSolr.Text = "Cập nhật Solr";
            this.btnComitSolr.UseVisualStyleBackColor = true;
            this.btnComitSolr.Click += new System.EventHandler(this.btnCommitSolr_Click);
            // 
            // lblDelete
            // 
            this.lblDelete.AutoSize = true;
            this.lblDelete.Location = new System.Drawing.Point(584, 171);
            this.lblDelete.Name = "lblDelete";
            this.lblDelete.Size = new System.Drawing.Size(13, 13);
            this.lblDelete.TabIndex = 14;
            this.lblDelete.Text = "0";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(132, 170);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(13, 13);
            this.lblTotal.TabIndex = 13;
            this.lblTotal.Text = "0";
            // 
            // lblCopy
            // 
            this.lblCopy.AutoSize = true;
            this.lblCopy.Location = new System.Drawing.Point(346, 170);
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(13, 13);
            this.lblCopy.TabIndex = 13;
            this.lblCopy.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(431, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Tổng số bản ghi delete";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 170);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Tổng số bản ghi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Số lượng xử lý";
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(470, 61);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(104, 23);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "Dừng tiến trình";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(28, 200);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessage.Size = new System.Drawing.Size(733, 258);
            this.txtMessage.TabIndex = 9;
            // 
            // lblStartTime
            // 
            this.lblStartTime.AutoSize = true;
            this.lblStartTime.Location = new System.Drawing.Point(132, 141);
            this.lblStartTime.Name = "lblStartTime";
            this.lblStartTime.Size = new System.Drawing.Size(49, 13);
            this.lblStartTime.TabIndex = 8;
            this.lblStartTime.Text = "00:00:00";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(239, 141);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 6;
            this.label8.Text = "Thời gian kết thúc";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Thời gian bắt đầu";
            // 
            // btnActive2End
            // 
            this.btnActive2End.Location = new System.Drawing.Point(128, 102);
            this.btnActive2End.Name = "btnActive2End";
            this.btnActive2End.Size = new System.Drawing.Size(90, 23);
            this.btnActive2End.TabIndex = 4;
            this.btnActive2End.Text = "Lưu trữ (∞)";
            this.btnActive2End.UseVisualStyleBackColor = true;
            this.btnActive2End.Click += new System.EventHandler(this.btnActive2End_Click);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtMessageUpdateSolr);
            this.tabPage1.Controls.Add(this.btnUpdateArchive);
            this.tabPage1.Controls.Add(this.lblArchiveId);
            this.tabPage1.Controls.Add(this.lblKhieuNaiId);
            this.tabPage1.Controls.Add(this.txtArchiveId);
            this.tabPage1.Controls.Add(this.txtKhieuNaiId);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(761, 458);
            this.tabPage1.TabIndex = 1;
            this.tabPage1.Text = "Cập nhật Solr";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtMessageUpdateSolr
            // 
            this.txtMessageUpdateSolr.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageUpdateSolr.Location = new System.Drawing.Point(16, 100);
            this.txtMessageUpdateSolr.Multiline = true;
            this.txtMessageUpdateSolr.Name = "txtMessageUpdateSolr";
            this.txtMessageUpdateSolr.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMessageUpdateSolr.Size = new System.Drawing.Size(745, 358);
            this.txtMessageUpdateSolr.TabIndex = 10;
            // 
            // btnUpdateArchive
            // 
            this.btnUpdateArchive.Location = new System.Drawing.Point(213, 39);
            this.btnUpdateArchive.Name = "btnUpdateArchive";
            this.btnUpdateArchive.Size = new System.Drawing.Size(101, 23);
            this.btnUpdateArchive.TabIndex = 2;
            this.btnUpdateArchive.Text = "Cập nhật solr";
            this.btnUpdateArchive.UseVisualStyleBackColor = true;
            this.btnUpdateArchive.Click += new System.EventHandler(this.btnUpdateArchive_Click);
            // 
            // lblArchiveId
            // 
            this.lblArchiveId.AutoSize = true;
            this.lblArchiveId.Location = new System.Drawing.Point(22, 23);
            this.lblArchiveId.Name = "lblArchiveId";
            this.lblArchiveId.Size = new System.Drawing.Size(52, 13);
            this.lblArchiveId.TabIndex = 1;
            this.lblArchiveId.Text = "ArchiveId";
            // 
            // lblKhieuNaiId
            // 
            this.lblKhieuNaiId.AutoSize = true;
            this.lblKhieuNaiId.Location = new System.Drawing.Point(22, 49);
            this.lblKhieuNaiId.Name = "lblKhieuNaiId";
            this.lblKhieuNaiId.Size = new System.Drawing.Size(63, 13);
            this.lblKhieuNaiId.TabIndex = 1;
            this.lblKhieuNaiId.Text = "Khiếu nại Id";
            // 
            // txtArchiveId
            // 
            this.txtArchiveId.Location = new System.Drawing.Point(107, 16);
            this.txtArchiveId.Name = "txtArchiveId";
            this.txtArchiveId.Size = new System.Drawing.Size(84, 20);
            this.txtArchiveId.TabIndex = 0;
            // 
            // txtKhieuNaiId
            // 
            this.txtKhieuNaiId.Location = new System.Drawing.Point(107, 42);
            this.txtKhieuNaiId.Name = "txtKhieuNaiId";
            this.txtKhieuNaiId.Size = new System.Drawing.Size(84, 20);
            this.txtKhieuNaiId.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(769, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // cậpNhậtLoạiKhiếuNạiToolStripMenuItem
            // 
            this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem.Name = "cậpNhậtLoạiKhiếuNạiToolStripMenuItem";
            this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem.Size = new System.Drawing.Size(140, 20);
            this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem.Text = "Cập nhật loại khiếu nại";
            this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem.Click += new System.EventHandler(this.cậpNhậtLoạiKhiếuNạiToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 508);
            this.Controls.Add(this.tblCommitSolr);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Text = "Lưu trữ dữ liệu";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tblCommitSolr.ResumeLayout(false);
            this.tabArchive.ResumeLayout(false);
            this.tabArchive.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnArchive;
        private System.Windows.Forms.Label lblEndTime;
        private System.Windows.Forms.TabControl tblCommitSolr;
        private System.Windows.Forms.TabPage tabArchive;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cậpNhậtLoạiKhiếuNạiToolStripMenuItem;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblDelete;
        private System.Windows.Forms.Label lblCopy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnComitSolr;
        private System.Windows.Forms.Label lblGuide;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button btnUpdateArchive;
        private System.Windows.Forms.Label lblArchiveId;
        private System.Windows.Forms.Label lblKhieuNaiId;
        private System.Windows.Forms.TextBox txtArchiveId;
        private System.Windows.Forms.TextBox txtKhieuNaiId;
        private System.Windows.Forms.TextBox txtMessageUpdateSolr;
        private System.Windows.Forms.Button btnClearTextbox;
        private System.Windows.Forms.Button btnActive2End;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStartTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCommitSolr2;
        private System.Windows.Forms.Button btnUpdateTemp;
    }
}

