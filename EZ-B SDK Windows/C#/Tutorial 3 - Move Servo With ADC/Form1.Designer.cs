﻿namespace Tutorial_3___Move_Servo_With_ADC {
  partial class Form1 {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.label1 = new System.Windows.Forms.Label();
      this.ezB_Connect1 = new EZ_B.UCEZB_Connect();
      this.SuspendLayout();
      // 
      // timer1
      // 
      this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(108, 83);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(35, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "label1";
      // 
      // ezB_Connect1
      // 
      this.ezB_Connect1.Location = new System.Drawing.Point(0, 0);
      this.ezB_Connect1.Name = "ezB_Connect1";
      this.ezB_Connect1.Size = new System.Drawing.Size(283, 31);
      this.ezB_Connect1.TabIndex = 2;
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 128);
      this.Controls.Add(this.ezB_Connect1);
      this.Controls.Add(this.label1);
      this.Name = "Form1";
      this.Text = "Form1";
      this.Load += new System.EventHandler(this.Form1_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.Label label1;
    private EZ_B.UCEZB_Connect ezB_Connect1;
  }
}

