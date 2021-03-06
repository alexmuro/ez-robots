﻿using System;
using System.Drawing;
using System.Windows.Forms;
using EZ_B;

namespace Serial_Test {

  public partial class Form1 : Form {

    private Graphics       _graphics;
    private bool           _dir      = true;
    private int         [] _values   = new int[30];

    public Form1() {

      InitializeComponent();
    }

    delegate void SetLogCallback(string text, params object[] vals);
    private void SetLog(string text, params object[] vals) {

      if (this.textBox1.InvokeRequired) {

        SetLogCallback d = new SetLogCallback(SetLog);
        this.Invoke(d, new object[] { text, vals });
      } else {

        this.textBox1.AppendText(string.Format(text, vals));
        this.textBox1.AppendText(Environment.NewLine);
      }
    }

    void Worker_OnLog(string logTxt) {

      SetLog(logTxt);
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {

      timer1.Stop();

      _ezb.Disconnect();

      if (_ezb.IsConnected) {

        SetLog("Can not close port");

        e.Cancel = true;
      }
    }

    private void trackBar3_Scroll(object sender, EventArgs e) {

      _ezb.Servo.SetServoPosition(Servo.ServoPortEnum.D10, trackBar3.Value);
    }

    private void button7_Click(object sender, EventArgs e) {

      _ezb.Movement.GoStop();

      timer1.Stop();
    }

    private void button3_Click(object sender, EventArgs e) {

      _ezb.Movement.GoForward();

      if (cbCollosion.Checked) 
        timer1.Start();
    }

    private void button4_Click(object sender, EventArgs e) {

      _ezb.Movement.GoLeft();

      timer1.Stop();
    }

    private void button5_Click(object sender, EventArgs e) {

      _ezb.Movement.GoRight();

      timer1.Stop();
    }

    private void button6_Click(object sender, EventArgs e) {

      _ezb.Movement.GoReverse();

      timer1.Stop();
    }

    private void button8_Click(object sender, EventArgs e) {

      textBox1.Clear();
    }

    private void timer1_Tick(object sender, EventArgs e) {

      if (!_ezb.IsConnected)
        return;

      int position = _ezb.Servo.GetServoPosition(Servo.ServoPortEnum.D12);

      if (position == 0)
        position = (int)numericUpDown1.Value;

      if (position <= numericUpDown1.Value)
        _dir = true;

      if (position >= numericUpDown2.Value)
        _dir = false;

      if (_dir)
        position++;
      else
        position--;

      trackBar2.Value = position;

      _ezb.Servo.SetServoPosition(Servo.ServoPortEnum.D12, position);

      int val = _ezb.ADC.GetADCValue(ADC.ADCPortEnum.ADC0);

      position = Servo.SERVO_MAX - position;

      _values[position] = val;

      drawRadarImage(position);
    }

    private void trackBar1_Scroll(object sender, EventArgs e) {

      _ezb.Servo.SetServoPosition(Servo.ServoPortEnum.D11, trackBar1.Maximum - trackBar1.Value);
    }

    private void trackBar2_Scroll(object sender, EventArgs e) {

      _ezb.Servo.SetServoPosition(Servo.ServoPortEnum.D12, trackBar2.Value);
    }

    private void Form1_Load(object sender, EventArgs e) {

      ezB_Connect1.EZB = _ezb;

      _ezb.OnLog += new EZB.OnLogHandler(Worker_OnLog);

      trackBar1.Maximum = Servo.SERVO_MAX;
      trackBar1.Minimum = Servo.SERVO_MIN;
      trackBar1.Value = Servo.SERVO_CENTER;

      trackBar2.Maximum = Servo.SERVO_MAX;
      trackBar2.Minimum = Servo.SERVO_MIN;
      trackBar2.Value = Servo.SERVO_CENTER;

      trackBar3.Maximum = Servo.SERVO_MAX;
      trackBar3.Minimum = Servo.SERVO_MIN;
      trackBar3.Value = Servo.SERVO_CENTER;

      numericUpDown1.Maximum = Servo.SERVO_MAX;
      numericUpDown1.Minimum = Servo.SERVO_MIN;

      numericUpDown2.Maximum = Servo.SERVO_MAX;
      numericUpDown2.Minimum = Servo.SERVO_MIN;

      pictureBox1.Image = (Image)new Bitmap(pictureBox1.Width, pictureBox1.Height);
      _graphics = Graphics.FromImage(pictureBox1.Image);
      _graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

      drawRadarImage(3);

      timer1.Interval = 350;

      SetLog(Resource1.Intro);
    }

    void _ezRobot_OnAutoCollision(int EyeValue) {

      _ezb.Movement.GoStop();

      timer1.Stop();

      SetLog(string.Format("Auto Collision Detected. Eye: {0}", EyeValue));
    }

    private void cbVerboseLogging_CheckedChanged(object sender, EventArgs e) {

      _ezb.VerboseLogging = cbVerboseLogging.Checked;
    }

    private void button2_Click(object sender, EventArgs e) {

      _ezb.Servo.ReleaseAllServos();
    }

    private void drawRadarImage(int position) {

      int centerX = pictureBox1.Width / 2;

      _graphics.FillRectangle(new SolidBrush(Color.Black), 0, 0, pictureBox1.Width, pictureBox1.Height);

      Point [] backPoints = { new Point(10, 0),
                              new Point(pictureBox1.Width - 15, 0),
                              new Point(centerX, pictureBox1.Height  + 100),
                              new Point(10, 0)};

      _graphics.FillPolygon(
        new SolidBrush(Color.FromArgb(10, 50, 10)),
        backPoints);

      for (int x=1; x < Servo.SERVO_MAX; x++) {

        Pen pen;

        if (x == position)
          pen = new Pen(Color.FromArgb(50, 255, 50), 1);
        else
          pen = new Pen(Color.FromArgb(50, 100, 50), 1);

        _graphics.DrawLine(
          pen,
          centerX,
          pictureBox1.Height + 100,
          x * (pictureBox1.Width / Servo.SERVO_MAX),
          0);

        if (_values[x] > 50)
          _graphics.FillEllipse(
            new SolidBrush(Color.FromArgb(100, Color.Red)),
            x * (pictureBox1.Width / Servo.SERVO_MAX),
            pictureBox1.Height - (180 - _values[x]),
            10, 10);
      }

      // draw center ball at bottm of screen
      for (int x=0; x < 50; x++)
        _graphics.DrawEllipse(
          new Pen(Color.FromArgb(0, 255 - (x * 5), 0), 2),
          (centerX - 50) + x,
          (pictureBox1.Height - 50) + x,
          100 - x,
          100 - x);

      pictureBox1.Refresh();

      label7.Text = string.Format("Head Position: {0}  Value: {1}", position, _values[position]);
    }
  }
}
