using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Form1 : Form
    {
        // input text to be compiled
        string input;
        string file_input;

        // to store location and size of the form and controls
        private Rectangle textBox1Rect;
        private Rectangle textBox2Rect;
        private Rectangle button1Rect;
        private Rectangle button2Rect;
        private Rectangle button3Rect;
        private Rectangle button4Rect;
        private Rectangle label1Rect;
        private Rectangle label2Rect;
        private Size formSize;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            formSize = this.Size;
            textBox1Rect = new Rectangle(textBox1.Location.X, textBox1.Location.Y, textBox1.Size.Width, textBox1.Size.Height);
            textBox2Rect = new Rectangle(textBox2.Location.X, textBox2.Location.Y, textBox2.Size.Width, textBox2.Size.Height);
            button1Rect = new Rectangle(button1.Location.X, button1.Location.Y, button1.Size.Width, button1.Size.Height);
            button2Rect = new Rectangle(button2.Location.X, button2.Location.Y, button2.Size.Width, button2.Size.Height);
            button3Rect = new Rectangle(button3.Location.X, button3.Location.Y, button3.Size.Width, button3.Size.Height);
            button4Rect = new Rectangle(button4.Location.X, button4.Location.Y, button4.Size.Width, button4.Size.Height);
            label1Rect = new Rectangle(label1.Location.X, label1.Location.Y, label1.Size.Width, label1.Size.Height);
            label2Rect = new Rectangle(label2.Location.X, label2.Location.Y, label2.Size.Width, label2.Size.Height);
        }

        // function to dynamically resize specific control according to resizing the form
        private void resize_control(Rectangle rec, Control ctrl)
        {
            float xRatio = this.Width / (float)(formSize.Width);
            float yRatio = this.Height / (float)(formSize.Height);
            int newX = (int)(rec.X * xRatio);
            int newY = (int)(rec.Y * yRatio);
            int newWidth = (int)(rec.Width * xRatio);
            int newHeight = (int)(rec.Height * yRatio);
            ctrl.Location = new Point(newX, newY);
            ctrl.Size = new Size(newWidth, newHeight);
        }

        // function to resize all used controls
        private void resize_controls()
        {
            resize_control(textBox1Rect, textBox1);
            resize_control(textBox2Rect, textBox2);
            resize_control(button1Rect, button1);
            resize_control(button2Rect, button2);
            resize_control(button3Rect, button3);
            resize_control(button4Rect, button4);
            resize_control(label1Rect, label1);
            resize_control(label2Rect, label2);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            resize_controls();
        }

        // Scan Button
        private void button1_Click_1(object sender, EventArgs e)
        {
            textBox2.Text = "";
            bool flag = true;
            input = textBox1.Text;
            Scanner.scan(input);

            do
            {
                Token t = Scanner.GetToken();
                if (t.token_type == TokenType.SEMICOLON && t.str_lexel == "done" && t.num_lexel == 555)
                {
                    flag = false;
                    break;
                }
                if (t.token_type == TokenType.NUMBER)
                {
                    textBox2.AppendText((t.num_lexel).ToString());
                }
                else
                {
                    textBox2.AppendText(t.str_lexel);
                }
                textBox2.AppendText(",");
                textBox2.AppendText((t.token_type).ToString());
                textBox2.AppendText(Environment.NewLine);
            } while (flag);

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "txt files (*.txt)|*.txt";
            dialog.Title = "Save Project";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (dialog.FileName != "")
                {
                    System.IO.TextWriter txt = new StreamWriter(dialog.FileName);
                    txt.Write(textBox2.Text);
                    txt.Close();
                    MessageBox.Show("Saved");
                }
            }
        }

        // Parse Button
        private void button2_Click(object sender, EventArgs e)
        {
            input = textBox2.Text;
            Parser.parse(input);
        }

        // Browse to scan Button
        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Import File";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string d = dialog.FileName;
                try
                {
                    file_input = File.ReadAllText(d);
                    textBox1.Text = "";
                    textBox1.Text = file_input;
                }
                catch (IOException)
                {
                }
            }
        }

        // Browse to parse Button
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Import File";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string d = dialog.FileName;
                try
                {
                    file_input = File.ReadAllText(d);
                    textBox2.Text = "";
                    textBox2.Text = file_input;
                }
                catch (IOException)
                {
                }
            }
        }
    }
}
