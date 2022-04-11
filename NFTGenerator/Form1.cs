using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        int historyCursor = -1;
        List<Bitmap> history = new List<Bitmap>();
        Random rand = new Random();
        Bitmap lockedImage = null;
        byte takeColorMode = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            AddNewElement();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenImage();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox2.BackColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            HistoryDown();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            HistoryUp();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ClearHistory();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ChangeColor();
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
            LockImage();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            BackToLocked();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            TakeRandomHat();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            TakeRandomBody();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            textBox3.Text = GenerateName(7);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            TakeRandomThing();
        }

        private void TakeRandomThing()
        {
            int countFiles = new DirectoryInfo("things").GetFiles().Length;
            pictureBox1.Image = Image.FromFile($"things/thing{rand.Next(countFiles)}.png");
        }

        private void TakeRandomBody()
        {
            int countFiles = new DirectoryInfo("bodies").GetFiles().Length;
            pictureBox1.Image = Image.FromFile($"bodies/body{rand.Next(countFiles)}.png");
        }

        private void TakeRandomHat()
        {
            int countFiles = new DirectoryInfo("hats").GetFiles().Length;
            pictureBox1.Image = Image.FromFile($"hats/hat{rand.Next(countFiles)}.png");
        }

        private string GenerateName(int len)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[rand.Next(consonants.Length)].ToUpper();
            name += vowels[rand.Next(vowels.Length)];
            int b = 2;
            while (b < len)
            {
                name += consonants[rand.Next(consonants.Length)];
                b++;
                name += vowels[rand.Next(vowels.Length)];
                b++;
            }
            return name;
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

        private void BackToLocked()
        {
            pictureBox1.Image = lockedImage;
        }
        private void LockImage()
        {
            try
            {
                lockedImage = new Bitmap(pictureBox1.Image);
                pictureBox3.Image = lockedImage;
            }
            catch
            {
                MessageBox.Show("Отсутствует изображение!", "Ошибка фиксации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ChangeColor()
        {
            if (pictureBox1.Image != null)
            {
                Color newColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
                Color changeColor;
                Bitmap addImage = new Bitmap(pictureBox1.Image);
                try
                {
                    var colorChannels = textBox2.Text.Split(',');
                    changeColor = Color.FromArgb(int.Parse(colorChannels[0]), int.Parse(colorChannels[1]), int.Parse(colorChannels[2]));
                }
                catch
                {
                    MessageBox.Show("Ошибка изменения изображения!\nПроверьте правильность ввода цветовых каналов!\nЦвет установлен в цвет по умолчанию!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    changeColor = Color.FromArgb(34, 32, 52);
                    textBox1.Text = "34,32,52";
                }
                for (int i = 0; i < pictureBox1.Image.Width; ++i)
                {
                    for (int j = 0; j < pictureBox1.Image.Height; ++j)
                    {
                        if (addImage.GetPixel(i, j) == changeColor)
                        {
                            addImage.SetPixel(i, j, newColor);
                        }
                    }
                }
                pictureBox1.Image = addImage;
                textBox2.Text = $"{newColor.R},{newColor.G},{newColor.B}";
                button10.BackColor = newColor;
            }
        }

        private void HistoryUp()
        {
            if (historyCursor + 1 < history.Count)
            {
                historyCursor++;
                int bufCursor = -1;
                foreach (var item in history)
                {
                    bufCursor++;
                    if (bufCursor == historyCursor)
                    {
                        pictureBox2.Image = item;
                        break;
                    }
                }
            }
        }

        private void ClearHistory()
        {
            pictureBox2.Image = null;
            history = new List<Bitmap>();
            historyCursor = -1;
        }

        private void HistoryDown()
        {
            if (historyCursor - 1 >= 0)
            {
                historyCursor--;
                int bufCursor = -1;
                foreach (var item in history)
                {
                    bufCursor++;
                    if (bufCursor == historyCursor)
                    {
                        pictureBox2.Image = item;
                        break;
                    }
                }
            }
        }

        private void fillImageBackground()
        {
            Bitmap resultImage = new Bitmap(pictureBox2.Image);
            for (int i = 0; i < pictureBox2.Image.Width; ++i)
            {
                for (int j = 0; j < pictureBox2.Image.Height; ++j)
                {
                    if (resultImage.GetPixel(i, j) == Color.FromArgb(0, 0, 0, 0))
                    {
                        resultImage.SetPixel(i, j, pictureBox2.BackColor);
                    }
                }
            }
            pictureBox2.Image = resultImage;
        }

        private void OpenImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.PNG;*.JPG;*.GIF;*.BMP)|*.PNG;*.JPG;*.GIF;*.BMP|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки изображения!", "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveImage()
        {
            if (pictureBox2.Image != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Сохранить изображение как...";
                sfd.OverwritePrompt = true;
                sfd.CheckPathExists = true;
                sfd.ShowHelp = true;
                sfd.Filter = "Image Files(*.PNG)|*.PNG|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.BMP)|*.BMP|All files (*.*)|*.*";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        fillImageBackground();
                        AddName();
                        pictureBox2.Image.Save(sfd.FileName);
                        MessageBox.Show("Изображение успешно сохранено!", "Сохранение успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }
                    catch
                    {
                        MessageBox.Show("Ошибка сохранения изображения!", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void AddName()
        {
            string name = textBox3.Text;
            Graphics gr = Graphics.FromImage(pictureBox2.Image);
            Image img = pictureBox2.Image;
            Font font = new Font(new FontFamily("Courier"), 25, FontStyle.Bold);
            gr.DrawString(name, font, new SolidBrush(Color.Black), 12, 12);
            gr.DrawString(name, font, new SolidBrush(Color.White), 10, 10);
            AddToHistory(new Bitmap(img, img.Width, img.Height));
        }

        private void AddToHistory(Bitmap newBitmap)
        {
            if (history.Count > historyCursor + 1)
            {
                int newHistoryCursor = -1;
                List<Bitmap> newHistory = new List<Bitmap>();
                foreach (var item in history)
                {
                    if (historyCursor > newHistoryCursor)
                    {
                        newHistory.Add(item);
                        newHistoryCursor++;
                    }
                }
                newHistory.Add(new Bitmap(pictureBox2.Image));
                newHistoryCursor++;
                history = new List<Bitmap>();
                foreach (var item in newHistory)
                    history.Add(item);
                historyCursor = newHistoryCursor;
            }
            else
            {
                history.Add(new Bitmap(pictureBox2.Image));
                historyCursor++;
            }
        }

        private void AddNewElement()
        {
            if (pictureBox1.Image != null)
            {
                try
                {
                    Bitmap addImage = new Bitmap(pictureBox1.Image);
                    Bitmap resultImage;
                    if (pictureBox2.Image != null)
                        resultImage = new Bitmap(pictureBox2.Image);
                    else
                        resultImage = new Bitmap(pictureBox1.Image.Width, pictureBox1.Image.Height);
                    Color ignoreColor;
                    try
                    {
                        var colorChannels = textBox1.Text.Split(',');
                        ignoreColor = Color.FromArgb(int.Parse(colorChannels[0]), int.Parse(colorChannels[1]), int.Parse(colorChannels[2]));
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка изменения изображения!\nПроверьте правильность ввода цветовых каналов!\nЦвет установлен в цвет по умолчанию!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ignoreColor = Color.FromArgb(0, 255, 0);
                        textBox1.Text = "0,255,0";
                    }
                    for (int i = 0; i < pictureBox1.Image.Width; ++i)
                    {
                        for (int j = 0; j < pictureBox1.Image.Height; ++j)
                        {
                            if (addImage.GetPixel(i, j) != ignoreColor)
                            {
                                resultImage.SetPixel(i, j, addImage.GetPixel(i, j));
                            }
                        }
                    }
                    pictureBox2.Image = resultImage;
                    AddToHistory(resultImage);
                }
                catch
                {
                    MessageBox.Show("Ошибка изменения изображения!\nПроверьте является ли добавляемое изображение меньшим или одного размера с финальным!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
