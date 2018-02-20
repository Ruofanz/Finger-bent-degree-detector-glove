using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace ReadGlove
{
    public partial class Form1 : Form
    {
        //定义小拇指弯曲度
        int A1 = 0 ;
        int A2 = 0;
        int A3 = 0;
        int A4 = 0;
        int whichone = 0;
        int cnt = 0;
        SerialPort sp = null;
        bool isOpen = false;
        bool isSetProperty = false;
        bool isHex = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Width = 0;
            pictureBox2.Width = 0;
            pictureBox3.Width = 0;
            pictureBox4.Width = 0;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
            this.MaximizeBox = false;
            for (int i = 0; i < 10; i++)
            {
                comboBox1.Items.Add("COM" + (i + 1).ToString());
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Add("1200");
            comboBox2.Items.Add("2400");
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("19200");
            comboBox2.Items.Add("38400");
            comboBox2.Items.Add("43000");
            comboBox2.Items.Add("56000");
            comboBox2.Items.Add("57600");
            comboBox2.Items.Add("115200");
            comboBox2.SelectedIndex = 3;

            comboBox3.Items.Add("0");
            comboBox3.Items.Add("1");
            comboBox3.Items.Add("1.5");
            comboBox3.Items.Add("2");
            comboBox3.SelectedIndex = 1;

            comboBox5.Items.Add("8");
            comboBox5.Items.Add("7");
            comboBox5.Items.Add("6");
            comboBox5.Items.Add("5");
            comboBox5.SelectedIndex = 0;

            comboBox4.Items.Add("无");
            comboBox4.Items.Add("奇校验");
            comboBox4.Items.Add("偶校验");
            comboBox4.SelectedIndex = 0;
            radioButton1.Checked = true;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool comExistence = false;
            comboBox1.Items.Clear();
            for(int i=0; i<10;i++)
            {
                try
                {
                    SerialPort sp = new SerialPort("COM" + (i + 1).ToString());
                    sp.Open();
                    sp.Close();
                    comboBox1.Items.Add("COM" + (i + 1).ToString());
                    comExistence = true;

                }
                catch (Exception)
                {
                    continue;
                }
              }
                if (comExistence)
                {
                    comboBox1.SelectedIndex = 0;

                }
                else
                {
                    MessageBox.Show("没有找到可用串口！","错误提示");
                }
            }

        private bool CheckPortSetting()
        {
            if (comboBox1.Text.Trim() == "") return false;
            if (comboBox2.Text.Trim() == "") return false;
            if (comboBox3.Text.Trim() == "") return false;
            if (comboBox4.Text.Trim() == "") return false;
            if (comboBox5.Text.Trim() == "") return false;
            return true;
        }

        private bool CheckSendData()
        {
            if (textBox2.Text.Trim() == "") return false;
            return true;
        }

        private void SetPortProperty()
        {
            sp = new SerialPort();
            sp.PortName = comboBox1.Text.Trim();
            sp.BaudRate = Convert.ToInt32(comboBox2.Text.Trim());
            float f = Convert.ToSingle(comboBox3.Text.Trim());
            if (f == 0)
            {
                sp.StopBits = StopBits.None;
            }
            else if (f == 1.5)
            {
                sp.StopBits = StopBits.OnePointFive;
            }
            else if (f == 1)
            {
                sp.StopBits = StopBits.One;
            }
            else if (f == 2)
            {
                sp.StopBits = StopBits.Two;
            }
            else
            {
                sp.StopBits = StopBits.One;
            }
            sp.DataBits = Convert.ToInt16(comboBox5.Text.Trim());

            string s = comboBox4.Text.Trim();
            if (s.CompareTo("无") == 0)
            {
                sp.Parity = Parity.None;
            }
            else if (s.CompareTo("奇校验") == 0)
            {
                sp.Parity = Parity.Odd;
            }
            else if (s.CompareTo("偶校验") == 0)
            {
                sp.Parity = Parity.Even;
            }
            else
            {
                sp.Parity = Parity.None;
            }

            sp.ReadTimeout = -1;
            sp.RtsEnable = true;
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
            if (radioButton2.Checked)
            {
                isHex = true;
            }
            else
            {
                isHex = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                try
                {
                    sp.WriteLine(textBox2.Text);

                }
                catch (Exception)
                {
                    MessageBox.Show("发送数据时发生错误！", "错误提示");
                    return;
                }
            }
            else
            {
                MessageBox.Show("串口未打开！", "错误提示");
                return;
            }
            if (!CheckSendData())
            {
                MessageBox.Show("请输入要发送的数据！","错误提示");
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (isOpen == false)
            {
                if (!CheckPortSetting())
                {
                    MessageBox.Show("串口未设置！", "错误提示");
                    return;
                }
                if (!isSetProperty)
                {
                    SetPortProperty();
                    isSetProperty = true;
                }
                try
                {
                    sp.Open();
                    isOpen = true;
                    button2.Text = "关闭串口";
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;

                }
                catch (Exception)
                {
                    isSetProperty = false;
                    isOpen = false;
                    MessageBox.Show("串口无效或已被占用！", "错误提示");
                }
            }
            else
            {
                
                try
                {
                    sp.Close();
                    isOpen = false;
                    isSetProperty = false;
                    button2.Text = "打开串口";
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                    radioButton1.Enabled = true;
                    radioButton2.Enabled = true;
                }
                catch (Exception)
                {
                   //注意这里
                    MessageBox.Show("关闭串口时发生错误", "错误提示");
                }
            }
        }
        private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            System.Threading.Thread.Sleep(15);
            this.Invoke((EventHandler)(delegate
            {
                if (isHex == false)
                {

                    cnt++;
                   
                    Byte[] ReceivedData = new Byte[sp.BytesToRead];
                    sp.Read(ReceivedData, 0, ReceivedData.Length);
                    String RecvDataText = null;
                    for (int i = 0; i < ReceivedData.Length-8 ; i++)
                    {
                        RecvDataText += (((char)(ReceivedData[i])).ToString()  );
                    }
                    A1 = Convert.ToInt16(RecvDataText);
                    RecvDataText = null;
                    for (int i = 2; i < ReceivedData.Length - 6; i++)
                    {
                        RecvDataText += (((char)(ReceivedData[i])).ToString());
                    }
                    A2 = Convert.ToInt16(RecvDataText);
                    RecvDataText = null;
                    for (int i = 4; i < ReceivedData.Length - 4; i++)
                    {
                        RecvDataText += (((char)(ReceivedData[i])).ToString());
                    }
                    A3 = Convert.ToInt16(RecvDataText);
                    RecvDataText = null;
                    for (int i = 6; i < ReceivedData.Length - 2; i++)
                    {
                        RecvDataText += (((char)(ReceivedData[i])).ToString());
                    }
                    A4 = Convert.ToInt16(RecvDataText);
                    RecvDataText = null;

                    if (A1 != 0 && A2 != 0 && A2 != 0 && A4 != 0)
                    {
                        pictureBox1.Width = A1 * 5;
                        pictureBox2.Width = A2 * 5;
                        pictureBox3.Width = A3 * 5;
                        pictureBox4.Width = A4 * 5;
                    }
                    textBox1.Text += Convert.ToString(A1);
                    textBox1.Text += " ";
                    textBox1.Text += Convert.ToString(A2);
                    textBox1.Text += " ";
                    textBox1.Text += Convert.ToString(A3);
                    textBox1.Text += " ";
                    textBox1.Text += Convert.ToString(A4);
                    textBox1.Text += " ";
                    if (cnt == 35)
                    {
                        cnt = 0;
                        textBox1.Text = "";
                    }
                }
                else
                {
                    Byte[] ReceivedData = new Byte[sp.BytesToRead];
                    sp.Read(ReceivedData, 0, ReceivedData.Length);
                    String RecvDataText = null;
                    for (int i = 0; i < ReceivedData.Length -1; i++)
                    {
                        RecvDataText += ("0x" + ReceivedData[i].ToString("X2") + " ");
                    }
                    textBox1.Text += RecvDataText;

                    
                    

                }
                sp.DiscardInBuffer();
            }));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
     }
 }

