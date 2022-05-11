using System;
using System.Drawing;
using System.Windows.Forms;

namespace NFTGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button9.BackColor = Color.FromArgb(0, 255, 0);
            button10.BackColor = Color.FromArgb(34, 32, 52);
            pictureBox1.MouseClick += PictureBox1_MouseClick;
        }

        NFTGeneratorClass NFTGenerator = new NFTGeneratorClass();
        Random rand = new Random();
        byte takeColorMode = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            NFTGenerator.AddNewElement(pictureBox1, pictureBox2, textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NFTGenerator.OpenImage(pictureBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NFTGenerator.SaveImage(pictureBox2, textBox3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            NFTGenerator.HistoryDown(pictureBox2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            NFTGenerator.HistoryUp(pictureBox2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            NFTGenerator.ClearHistory(pictureBox2);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            NFTGenerator.ChangeColor(pictureBox1, button10, textBox2);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ChangeTakeColorMode(1, button9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ChangeTakeColorMode(2, button10);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            NFTGenerator.LockImage(pictureBox1.Image, pictureBox3);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            NFTGenerator.BackToLocked(pictureBox1);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            NFTGenerator.TakeRandomHat(pictureBox1);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            NFTGenerator.TakeRandomBody(pictureBox1);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            NFTGenerator.TakeRandomThing(pictureBox1);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox3.Text = NFTGenerator.GenerateName(7);
        }
        
        private void ChangeTakeColorMode(byte id, Button button)
        {
            if (takeColorMode != id)
            {
                takeColorMode = id;
                pictureBox1.Cursor = Cursors.Hand;
                button.Cursor = Cursors.WaitCursor;
            }
            else
            {
                takeColorMode = 0;
                button.Cursor = Cursors.Default;
                pictureBox1.Cursor = Cursors.Default;
            }
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            switch (takeColorMode)
            {
                case 1:
                    {
                        Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        pictureBox1.DrawToBitmap(image, pictureBox1.ClientRectangle);
                        Color selectedColor = image.GetPixel(e.X, e.Y);
                        textBox1.Text = $"{selectedColor.R},{selectedColor.G},{selectedColor.B}";
                        button9.BackColor = selectedColor;
                        ChangeTakeColorMode(1, button9);
                        break;
                    }
                case 2:
                    {
                        Bitmap image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                        pictureBox1.DrawToBitmap(image, pictureBox1.ClientRectangle);
                        Color selectedColor = image.GetPixel(e.X, e.Y);
                        textBox2.Text = $"{selectedColor.R},{selectedColor.G},{selectedColor.B}";
                        button10.BackColor = selectedColor;
                        ChangeTakeColorMode(2, button10);
                        break;
                    }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                button9.Visible = true;
                var colorChannels = textBox1.Text.Split(',');
                Color ignoreColor = Color.FromArgb(int.Parse(colorChannels[0]), int.Parse(colorChannels[1]), int.Parse(colorChannels[2]));
                button9.BackColor = ignoreColor;
            }
            catch
            {
                button9.Visible = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                button10.Visible = true;
                var colorChannels = textBox2.Text.Split(',');
                Color changeColor = Color.FromArgb(int.Parse(colorChannels[0]), int.Parse(colorChannels[1]), int.Parse(colorChannels[2]));
                button10.BackColor = changeColor;
            }
            catch
            {
                button10.Visible = false;
            }
        }   
    }
}
