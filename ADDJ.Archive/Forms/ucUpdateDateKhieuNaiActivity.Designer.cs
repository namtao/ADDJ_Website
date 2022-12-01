namespace GQKN.Archive
{
    partial class ucUpdateDateKhieuNaiActivity
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnUpdate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKhieuNaiIdStart = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNumberRecord = new System.Windows.Forms.TextBox();
            this.txtKhieuNaiIdTo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(23, 107);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Cập nhật";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Bắt đầu từ KhieuNaiId";
            // 
            // txtKhieuNaiIdStart
            // 
            this.txtKhieuNaiIdStart.Location = new System.Drawing.Point(139, 24);
            this.txtKhieuNaiIdStart.Name = "txtKhieuNaiIdStart";
            this.txtKhieuNaiIdStart.Size = new System.Drawing.Size(100, 20);
            this.txtKhieuNaiIdStart.TabIndex = 2;
            this.txtKhieuNaiIdStart.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Số lượng bản ghi mỗi lần thực hiện";
            // 
            // txtNumberRecord
            // 
            this.txtNumberRecord.Location = new System.Drawing.Point(196, 78);
            this.txtNumberRecord.Name = "txtNumberRecord";
            this.txtNumberRecord.Size = new System.Drawing.Size(100, 20);
            this.txtNumberRecord.TabIndex = 4;
            this.txtNumberRecord.Text = "1000";
            // 
            // txtKhieuNaiIdTo
            // 
            this.txtKhieuNaiIdTo.Location = new System.Drawing.Point(139, 51);
            this.txtKhieuNaiIdTo.Name = "txtKhieuNaiIdTo";
            this.txtKhieuNaiIdTo.Size = new System.Drawing.Size(100, 20);
            this.txtKhieuNaiIdTo.TabIndex = 6;
            this.txtKhieuNaiIdTo.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Đến KhieuNaiId";
            // 
            // ucUpdateDateKhieuNaiActivity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtKhieuNaiIdTo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtNumberRecord);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtKhieuNaiIdStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnUpdate);
            this.Name = "ucUpdateDateKhieuNaiActivity";
            this.Size = new System.Drawing.Size(437, 307);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtKhieuNaiIdStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNumberRecord;
        private System.Windows.Forms.TextBox txtKhieuNaiIdTo;
        private System.Windows.Forms.Label label3;
    }
}
