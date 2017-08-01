namespace app_steganographie
{
    partial class RecordSound
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
            this.lblSamplesRecorded = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSamplesRequired = new System.Windows.Forms.Label();
            this.RecordStopBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSamplesRecorded
            // 
            this.lblSamplesRecorded.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSamplesRecorded.Location = new System.Drawing.Point(311, 39);
            this.lblSamplesRecorded.Name = "lblSamplesRecorded";
            this.lblSamplesRecorded.Size = new System.Drawing.Size(74, 20);
            this.lblSamplesRecorded.TabIndex = 6;
            this.lblSamplesRecorded.Text = "00000000";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(163, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Samples Recorded  :";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lblSamplesRequired);
            this.groupBox1.Controls.Add(this.RecordStopBtn);
            this.groupBox1.Controls.Add(this.lblSamplesRecorded);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 107);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Enregistrement audio";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(163, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 20);
            this.label2.TabIndex = 7;
            this.label2.Text = "Samples Required    :";
            // 
            // lblSamplesRequired
            // 
            this.lblSamplesRequired.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSamplesRequired.Location = new System.Drawing.Point(311, 62);
            this.lblSamplesRequired.Name = "lblSamplesRequired";
            this.lblSamplesRequired.Size = new System.Drawing.Size(74, 20);
            this.lblSamplesRequired.TabIndex = 8;
            this.lblSamplesRequired.Text = "        -";
            // 
            // RecordStopBtn
            // 
            this.RecordStopBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordStopBtn.Image = global::app_steganographie.Properties.Resources.play;
            this.RecordStopBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.RecordStopBtn.Location = new System.Drawing.Point(17, 39);
            this.RecordStopBtn.Name = "RecordStopBtn";
            this.RecordStopBtn.Size = new System.Drawing.Size(140, 40);
            this.RecordStopBtn.TabIndex = 0;
            this.RecordStopBtn.Text = "      Enregistrer";
            this.RecordStopBtn.UseVisualStyleBackColor = true;
            this.RecordStopBtn.Click += new System.EventHandler(this.RecordStopBtn_Click);
            // 
            // RecordSound
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(415, 132);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecordSound";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enregistrer un fichier audio (.wav)";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RecordStopBtn;
        private System.Windows.Forms.Label lblSamplesRecorded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSamplesRequired;
    }
}