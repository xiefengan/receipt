﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using WinFormCharpWebCam;

namespace WindowsFormsApp8
{

    public partial class Form1 : Form
    {
        Thread t1;
        string[] number = new string[8];
        public Form1()
        {
            InitializeComponent();
            webcam = new WebCam();
            webcam.InitializeWebCam(ref pictureBox2);
        }
        //***************Global varies********************
        WebCam webcam;
        string str;
        Form2 fem= new Form2();
        Point Form_Point;
        //************************************************
        private void Form1_Load(object sender, EventArgs e)
        {
            SetBtnTransparent();
            SetLabelTransparent();
            CenterToScreen();
            textBox1.Enabled = false;
            timer2.Enabled = true;
            t1 = new Thread(getNumber);
            t1.Start();
            webcam.Start();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (t1.IsAlive == false)
            {
                //button1.Enabled = true;
                textBox1.Enabled = true;
                label10.Text = "請輸入想要查詢的發票號碼";
                //button1.Text = "查詢發票號碼";
                HandlingNuber();
                timer1.Enabled = false;
                //button1.Visible = true;
                fem.Close();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //判斷中了多少錢
            price(); 
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            fem.Show();
            fem.Opacity = 0.8;
            timer2.Enabled = false;
        }
        
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            //if(e.KeyCode)
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.KeyCode.ToString() == "Delete")
            {
                textBox1.Text = "";
            }
        }
        private void label5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("我也好想中1000萬喔");
        }




        /*********************************************************/
        void price()
        {
            string num = textBox1.Text;
            int n;
            if (textBox1.Text == "")
            {
                label10.Text = "請輸入想要查詢的發票號碼";
            }
            else if ((Encoding.Default.GetBytes(textBox1.Text).Length != 8) || (!int.TryParse(textBox1.Text, out n)))
            {
                label10.Text = "請輸入正確的發票號碼";
            }
            else if (textBox1.Text == label5.Text)
            {
                label10.Text = "恭喜您中了1000萬大獎!!!!!";
            }
            else if (textBox1.Text == label6.Text)
            {
                label10.Text = "恭喜您中了200萬元大獎!!";
            }
            else if (textBox1.Text == label7.Text.Substring(0, 8) || textBox1.Text == label7.Text.Substring(9, 8) || textBox1.Text == label7.Text.Substring(18, 8))
            {
                label10.Text = "恭喜您中了20萬元大獎!";
            }
            else if (num.Substring(1, 7) == label7.Text.Substring(1, 7) || num.Substring(1, 7) == label7.Text.Substring(10, 7) || num.Substring(1, 7) == label7.Text.Substring(19, 7))
            {
                label10.Text = "恭喜您中了4萬元大獎!";
            }
            else if (num.Substring(2, 6) == label7.Text.Substring(2, 6) || num.Substring(2, 6) == label7.Text.Substring(11, 6) || num.Substring(2, 6) == label7.Text.Substring(20, 6))
            {
                label10.Text = "恭喜您中了1萬元!";
            }
            else if (num.Substring(3, 5) == label7.Text.Substring(3, 5) || num.Substring(3, 5) == label7.Text.Substring(12, 5) || num.Substring(3, 5) == label7.Text.Substring(21, 5))
            {
                label10.Text = "恭喜您中了4千元!";
            }
            else if (num.Substring(4, 4) == label7.Text.Substring(4, 4) || num.Substring(4, 4) == label7.Text.Substring(13, 4) || num.Substring(4, 4) == label7.Text.Substring(22, 4))
            {
                label10.Text = "恭喜您中了1千元!";
            }
            else if (num.Substring(5, 3) == label7.Text.Substring(5, 3) || num.Substring(5, 3) == label7.Text.Substring(14, 3) || num.Substring(5, 3) == label7.Text.Substring(23, 3))
            {
                label10.Text = "恭喜您中了200元!";
            }
            else if (num.Substring(5, 3) == label9.Text.Substring(0, 3) || num.Substring(5, 3) == label9.Text.Substring(4, 3) || num.Substring(5, 3) == label9.Text.Substring(8, 3))
            {
                label10.Text = "恭喜您中了200元!";
            }
            else
            {
                label10.Text = "真可惜沒有中獎QQ";
            }
        }
        private void getNumber()
        {
            WebRequest myrequest =
            WebRequest.Create(@"http://invoice.etax.nat.gov.tw/");
            myrequest.Method = "GET";
            WebResponse respon = myrequest.GetResponse();
            myrequest.Timeout = 1000;
            StreamReader sr = new StreamReader(respon.GetResponseStream());
            str = sr.ReadToEnd();
            sr.Close();
            respon.Close();
        }
        private void HandlingNuber()
        {
            int StrLocation = str.IndexOf("t18Red\">");

            //搜尋特別獎
            number[0] = str.Substring(StrLocation + 8, 8);
            label5.Text = number[0];

            //搜尋特獎
            StrLocation = str.IndexOf("t18Red\">", StrLocation + 10);
            number[1] = str.Substring(StrLocation + 8, 8);
            label6.Text = number[1];

            //搜尋頭獎
            StrLocation = str.IndexOf("t18Red\">", StrLocation + 10);
            StrLocation += 8;
            label7.Text = "";
            for (int i = 0; i < 3; i++)
            {
                number[i + 2] = str.Substring(StrLocation, 8);
                StrLocation += 9;
                if (i > 0)
                    label7.Text += "　" + number[i + 2];
                else
                    label7.Text += number[i + 2];
            }

            //搜尋增開六獎
            StrLocation = str.IndexOf("t18Red\">", StrLocation + 10);
            StrLocation += 8;
            label9.Text = "";
            for (int i = 0; i < 3; i++)
            {
                number[i + 5] = str.Substring(StrLocation, 3);
                StrLocation += 4;
                if (i > 0)
                    label9.Text += "　" + number[i + 5];
                else
                    label9.Text += number[i + 5];
            }

            //搜尋兌獎發票月份
            StrLocation = str.IndexOf("<h2>106");

            string one = str.Substring(StrLocation + 4, 10);
            label1.Text = "統一發票兌獎:" + one;
        }
        private void SetBtn(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.ForeColor = Color.Transparent;
            btn.BackColor = Color.Transparent;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btn.FlatAppearance.MouseDownBackColor = Color.Transparent;
        }
        private void SetBtnTransparent()
        {
            SetBtn(button2);
            SetBtn(button3);
            SetBtn(ButtonScan);
        }
        private void SetLabel(Label la)
        {
            la.FlatStyle = FlatStyle.Flat;
         //   la.ForeColor = Color.Transparent;
            la.BackColor = Color.Transparent;
        }
        private void SetLabelTransparent()
        {
            SetLabel(label1);
            SetLabel(label2);
            SetLabel(label3);
            SetLabel(label4);
            SetLabel(label5);
            SetLabel(label6);
            SetLabel(label7);
            SetLabel(label8);
            SetLabel(label9);
            SetLabel(label10);
            SetLabel(label11);

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            System.Drawing.Bitmap bitmap = null;
            //宣告 QRCode Reader 物件

            ZXing.IBarcodeReader reader = new ZXing.BarcodeReader();

            bitmap = (Bitmap)pictureBox2.Image;
            //進行解碼的動作
            ZXing.Result result = reader.Decode(bitmap);

            if (result != null)
            {   //如果有成功解讀，則顯示文字
                label10.Text = result.Text;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (timer3.Enabled == false)
            {
                timer3.Enabled = true;
                ButtonScan.Text = "ON";
            }
            else
            {
                timer3.Enabled = false;
                ButtonScan.Text = "OFF";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
            button2.Image = Image.FromFile(Application.StartupPath+"\\smallpress.png");
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.Image = Image.FromFile(Application.StartupPath + "\\small.png");
        }

        private void button3_MouseHover(object sender, EventArgs e)
        {
            button3.Image = Image.FromFile(Application.StartupPath + "\\exitpress.png");
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.Image = Image.FromFile(Application.StartupPath + "\\exit.png");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ButtonScan_Click(object sender, EventArgs e)
        {
            if (timer3.Enabled == false)
            {
                timer3.Enabled = true;
                ButtonScan.Text = "ON";
                pictureBox2.Visible = true;
            }
            else
            {
                timer3.Enabled = false;
                ButtonScan.Text = "OFF";
                pictureBox2.Visible = false;
            }
        }

        private void ButtonScan_MouseHover(object sender, EventArgs e)
        {
            ButtonScan.Image = Image.FromFile(Application.StartupPath + "\\buttonpress.png");
        }

        private void ButtonScan_MouseLeave(object sender, EventArgs e)
        {
            ButtonScan.Image = Image.FromFile(Application.StartupPath + "\\button.png");
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            Form_Point = new Point(-e.X + SystemInformation.FrameBorderSize.Width, -e.Y - SystemInformation.FrameBorderSize.Height);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePos = Control.MousePosition;
                //新視窗的位置
                mousePos.Offset(Form_Point.X, Form_Point.Y);
                //改變視窗位置
                Location = mousePos;
            }
        }
    }
    
}
