using System;
using System.Drawing;
using System.Windows.Forms;

namespace SegrigAgrigVisualisation
{
    public partial class Form1 : Form
    {
        private Field f;
        private FieldA fA;
        private bool rbc;

        public Form1()
        {
            InitializeComponent();
        }

        private void ExitBut_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            particleLabel.Text = Convert.ToString(trackBar1.Value);
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            trackBar1.Maximum = pictureBox1.Height / trackBar2.Value;
            trackBar1.Value = trackBar1.Maximum / 2;
            particleLabel.Text = Convert.ToString(trackBar1.Value);
            String res = String.Concat(Convert.ToString(pictureBox1.Width/trackBar2.Value), " x ",
                                       Convert.ToString(pictureBox1.Height/trackBar2.Value));
            fieldLabel.Text = res;
            pictureBox1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            if (checkBox1.Checked)
            {
                this.Height = 695;
                timer1.Interval = 300;
            }
            else
            {
                this.Height = 580;
                timer1.Interval = 50;
            }
            if (rbc)
            {
                f.Move();
                if (checkBox1.Checked)
                {
                    textCur1.Text = Convert.ToString(f.countC1);
                    textCur2.Text = Convert.ToString(f.countC2);
                    textLast1.Text = Convert.ToString(f.countL1);
                    textLast2.Text = Convert.ToString(f.countL2);
                    textTotal1.Text = Convert.ToString(f.countT1);
                    textTotal2.Text = Convert.ToString(f.countT2);
                }
            }
            else
            {
                fA.Move();
                if (checkBox1.Checked)
                {
                    textCur1.Text = Convert.ToString(fA.countC1);
                    textCur2.Text = Convert.ToString(fA.countC2);
                    textLast1.Text = Convert.ToString(fA.countL1);
                    textLast2.Text = Convert.ToString(fA.countL2);
                    textTotal1.Text = Convert.ToString(fA.countT1);
                    textTotal2.Text = Convert.ToString(fA.countT2);
                }
            }
        }

        private void ClearBut_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            f = null;
            fA = null;
            trackBar1.Visible = true;
            trackBar2.Visible = true;
            checkBox1.Visible = false;
            butPause.Visible = false;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            pictureBox1.Refresh();
        }

        private void StartBut_Click(object sender, EventArgs e)
        {
            trackBar1.Visible = false;
            trackBar2.Visible = false;
            checkBox1.Visible = true;
            butPause.Visible = true;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;

            if(rbc)
                f = new Field(pictureBox1.Width,pictureBox1.Height,trackBar2.Value,trackBar1.Value);
            else
                fA = new FieldA(pictureBox1.Width,pictureBox1.Height,trackBar2.Value,trackBar1.Value);
            
            pictureBox1.Refresh();
            timer1.Start();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int grid_step = trackBar2.Value;
            int kX = pictureBox1.Width / grid_step;
            int kY = pictureBox1.Height / grid_step;

            for (int i = 0; i <= kX; ++i)
            {
                e.Graphics.DrawLine(Pens.LightGray, i * grid_step, 0, i * grid_step, pictureBox1.Height);
            }

            for (int j = 0; j <= kY; ++j)
            {
                e.Graphics.DrawLine(Pens.LightGray, 0, j * grid_step, pictureBox1.Width, j * grid_step);
            }

            Brush b = new SolidBrush(Color.Fuchsia);
            Pen p = new Pen(b,7);
            e.Graphics.DrawLine(p, ((kX / 3) - 1) * grid_step, 0, ((kX / 3) -1) * grid_step, pictureBox1.Height);
            e.Graphics.DrawLine(p, ((kX / 3) * 2 + 1) * grid_step, 0, ((kX / 3) * 2 + 1) * grid_step, pictureBox1.Height);

            if (f != null) f.Draw(e.Graphics);
            if (fA != null) fA.Draw(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                rbc = true;
                fA = null;
                trackBar1.Maximum = pictureBox1.Height / trackBar2.Value;
                trackBar1.Value = trackBar1.Maximum / 2;
            }
            else
            {
                rbc = false;
                f = null;
                trackBar1.Maximum = 40;
                trackBar1.Value = 20;
            }
            particleLabel.Text = trackBar1.Value.ToString();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                rbc = true;
                fA = null;
                trackBar1.Maximum = pictureBox1.Height / trackBar2.Value;
                trackBar1.Value = trackBar1.Maximum / 2;
                label8.Text = "Верхний канал";
                label7.Text = "Нижний канал";
                label3.Text = "Текущий шаг";
                label4.Text = "Предыдущий шаг";
            }
            else
            {
                rbc = false;
                f = null;
                trackBar1.Maximum = 40;
                trackBar1.Value = 20;
                label8.Text = "Текущий шаг";
                label7.Text = "Предыдущий шаг";
                label3.Text = "Верхний канал";
                label4.Text = "Нижний канал";
            }
            particleLabel.Text = trackBar1.Value.ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void butPause_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                butPause.Text = "Продолжить";
            }
            else
            {
                timer1.Start();
                butPause.Text = "Пауза";
            }
        }
    }
}
