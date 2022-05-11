using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NFTGenerator
{
    public class NFTGeneratorClass
    {
        private Random _rand;
        private int _historyCursor;
        private List<Bitmap> _history;
        private Bitmap _lockedImage;

        public NFTGeneratorClass()
        {
            _rand = new Random();
            _historyCursor = -1;
            _history = new List<Bitmap>();
            _lockedImage = null;
        }

        public void AddNewElement(PictureBox pictureBox, PictureBox resultPictureBox, string maskColor)
        {
            if (pictureBox.Image != null)
            {
                try
                {
                    Bitmap addImage = new Bitmap(pictureBox.Image);
                    Bitmap resultImage;
                    if (resultPictureBox.Image != null)
                        resultImage = new Bitmap(resultPictureBox.Image);
                    else
                        resultImage = new Bitmap(pictureBox.Image.Width, pictureBox.Image.Height);
                    Color ignoreColor;
                    try
                    {
                        var colorChannels = maskColor.Split(',');
                        ignoreColor = Color.FromArgb(int.Parse(colorChannels[0]), int.Parse(colorChannels[1]), int.Parse(colorChannels[2]));
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка изменения изображения!\nПроверьте правильность ввода цветовых каналов!\nВыбран цвет маски по умолчанию!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        ignoreColor = Color.FromArgb(0, 255, 0);
                    }
                    for (int i = 0; i < pictureBox.Image.Width; ++i)
                    {
                        for (int j = 0; j < pictureBox.Image.Height; ++j)
                        {
                            if (addImage.GetPixel(i, j) != ignoreColor)
                            {
                                resultImage.SetPixel(i, j, addImage.GetPixel(i, j));
                            }
                        }
                    }
                    resultPictureBox.Image = resultImage;
                    AddToHistory(resultImage);
                }
                catch
                {
                    MessageBox.Show("Ошибка изменения изображения!\nПроверьте является ли добавляемое изображение меньшим или одного размера с финальным!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AddToHistory(Bitmap newBitmap)
        {
            if (_history.Count > _historyCursor + 1)
            {
                int newHistoryCursor = -1;
                List<Bitmap> newHistory = new List<Bitmap>();
                foreach (var item in _history)
                {
                    if (_historyCursor > newHistoryCursor)
                    {
                        newHistory.Add(item);
                        newHistoryCursor++;
                    }
                }
                newHistory.Add(new Bitmap(newBitmap));
                newHistoryCursor++;
                _history = new List<Bitmap>();
                foreach (var item in newHistory)
                    _history.Add(item);
                _historyCursor = newHistoryCursor;
            }
            else
            {
                _history.Add(new Bitmap(newBitmap));
                _historyCursor++;
            }
        }

        public void TakeRandomThing(PictureBox picture)
        {
            int countFiles = new DirectoryInfo("things").GetFiles().Length;
            picture.Image = Image.FromFile($"things/thing{_rand.Next(countFiles)}.png");
        }

        public void TakeRandomBody(PictureBox picture)
        {
            int countFiles = new DirectoryInfo("bodies").GetFiles().Length;
            picture.Image = Image.FromFile($"bodies/body{_rand.Next(countFiles)}.png");
        }

        public void TakeRandomHat(PictureBox picture)
        {
            int countFiles = new DirectoryInfo("hats").GetFiles().Length;
            picture.Image = Image.FromFile($"hats/hat{_rand.Next(countFiles)}.png");
        }

        public string GenerateName(int len)
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[_rand.Next(consonants.Length)].ToUpper();
            name += vowels[_rand.Next(vowels.Length)];
            int b = 2;
            while (b < len)
            {
                name += consonants[_rand.Next(consonants.Length)];
                b++;
                name += vowels[_rand.Next(vowels.Length)];
                b++;
            }
            return name;
        }

        public void BackToLocked(PictureBox picture)
        {
            picture.Image = _lockedImage;
        }
        public void LockImage(Image image, PictureBox bufPictureBox)
        {
            try
            {
                _lockedImage = new Bitmap(image);
                bufPictureBox.Image = _lockedImage;
            }
            catch
            {
                MessageBox.Show("Отсутствует изображение!", "Ошибка фиксации", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ChangeColor(PictureBox pictureBox, Button button, TextBox colorTextBox)
        {
            if (pictureBox.Image != null)
            {
                Color newColor = Color.FromArgb(_rand.Next(255), _rand.Next(255), _rand.Next(255));
                Color changeColor;
                Bitmap addImage = new Bitmap(pictureBox.Image);
                try
                {
                    var colorChannels = colorTextBox.Text.Split(',');
                    changeColor = Color.FromArgb(int.Parse(colorChannels[0]), int.Parse(colorChannels[1]), int.Parse(colorChannels[2]));
                }
                catch
                {
                    MessageBox.Show("Ошибка изменения изображения!\nПроверьте правильность ввода цветовых каналов!\nЦвет установлен в цвет по умолчанию!", "Ошибка изменения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                for (int i = 0; i < pictureBox.Image.Width; ++i)
                {
                    for (int j = 0; j < pictureBox.Image.Height; ++j)
                    {
                        if (addImage.GetPixel(i, j) == changeColor)
                        {
                            addImage.SetPixel(i, j, newColor);
                        }
                    }
                }
                pictureBox.Image = addImage;
                colorTextBox.Text = $"{newColor.R},{newColor.G},{newColor.B}";
                button.BackColor = newColor;
            }
        }

        public void HistoryUp(PictureBox pictureBox)
        {
            if (_historyCursor + 1 < _history.Count)
            {
                _historyCursor++;
                int bufCursor = -1;
                foreach (var item in _history)
                {
                    bufCursor++;
                    if (bufCursor == _historyCursor)
                    {
                        pictureBox.Image = item;
                        break;
                    }
                }
            }
        }

        public void HistoryDown(PictureBox pictureBox)
        {
            if (_historyCursor - 1 >= 0)
            {
                _historyCursor--;
                int bufCursor = -1;
                foreach (var item in _history)
                {
                    bufCursor++;
                    if (bufCursor == _historyCursor)
                    {
                        pictureBox.Image = item;
                        break;
                    }
                }
            }
        }

        public void ClearHistory(PictureBox pictureBox)
        {
            pictureBox.Image = null;
            _history = new List<Bitmap>();
            _historyCursor = -1;
        }

        public void fillImageBackground(PictureBox pictureBox)
        {
            Bitmap resultImage = new Bitmap(pictureBox.Image);
            for (int i = 0; i < pictureBox.Image.Width; ++i)
            {
                for (int j = 0; j < pictureBox.Image.Height; ++j)
                {
                    if (resultImage.GetPixel(i, j) == Color.FromArgb(0, 0, 0, 0))
                    {
                        resultImage.SetPixel(i, j, pictureBox.BackColor);
                    }
                }
            }
            pictureBox.Image = resultImage;
        }

        public void OpenImage(PictureBox pictureBox)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.PNG;*.JPG;*.GIF;*.BMP)|*.PNG;*.JPG;*.GIF;*.BMP|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox.Image = new Bitmap(ofd.FileName);
                }
                catch
                {
                    MessageBox.Show("Ошибка загрузки изображения!", "Ошибка загрузки", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void SaveImage(PictureBox pictureBox, TextBox nameTextBox)
        {
            if (pictureBox.Image != null)
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
                        fillImageBackground(pictureBox);
                        AddName(nameTextBox, pictureBox);
                        pictureBox.Image.Save(sfd.FileName);
                        MessageBox.Show("Изображение успешно сохранено!", "Сохранение успешно", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                    }
                    catch
                    {
                        MessageBox.Show("Ошибка сохранения изображения!", "Ошибка сохранения", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void AddName(TextBox nameTextBox, PictureBox pictureBox)
        {
            string name = nameTextBox.Text;
            Graphics gr = Graphics.FromImage(pictureBox.Image);
            Image img = pictureBox.Image;
            Font font = new Font(new FontFamily("Courier"), 25, FontStyle.Bold);
            gr.DrawString(name, font, new SolidBrush(Color.Black), 12, 12);
            gr.DrawString(name, font, new SolidBrush(Color.White), 10, 10);
            AddToHistory(new Bitmap(img, img.Width, img.Height));
        }
    }
}
