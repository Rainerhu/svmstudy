

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


using System.Windows.Forms.DataVisualization.Charting;


namespace WindowsFormsApplication1
{
    
    public partial class Form1 : Form
    {
        private Pressure My_Pressure = new Pressure();
        private bool Listening = false;//是否没有执行完invoke相关操作  
        private bool Closing = false;//是否正在关闭串口，执行Application.DoEvents，并阻止再次invoke  

        private SerialPort myport;

        private string temp = "";

        /// <summary>
        /// 存储变化帧
        /// </summary>
        //private double[,] before_frame = new double[4,16];
        private const int before_len = 5;
        private const int end_len = 4;
        private bool end_flag=false;

        private DateTime time_record;
        private const int Time = 800;
        private TimeSpan time_span = new TimeSpan(0,0,0,0,Time);

        //SVMsort svmsort = new SVMsort();

        private List<byte> buffer = new List<byte>(4096);

        const int sign_start = 0x7E;  //开始标志位
        private byte[] buf = new byte[100];
        private readonly int[] start_frame = new int[6] { 0x7E, 0, 0x2C, 0x90, 0, 0x13 };
        /*
                private bool whether_start = false;  //是否已经开始一帧
                private bool whether_data = false;   //是否开始处理到数据
                private int in_date;
                private int data_order = 0;
                const int start_order = 16;      //从开始位到数据位的字节数
                private int start_times = 0;   //处理了几位开始处的数据
          //转义定义
                const int sign_change = 0x7D;   //转义标志
                const int byte_change = 0x20;   //与0x20异或进行还原
                private bool whether_change = false;   //是否需要转义
                //校验变量定义
                const int len_start = 17;
                // private int left_1,left_2, left_3, left_4, left_5, left_6, left_7, left_8, right_1;
                private int[] data = new int[100];   //数据存储数组
                private int check_times = 0;     //校验通过次数


                private bool end_check = false;

                */
        public Form1()
        {
           
            InitializeComponent();
            

        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            MessageBox.Show(serialPort1.ReadLine());
        }


        private void Start_bt_Click(object sender, EventArgs e)
        {
            try
            {
             
                myport.Open();
                myport.DiscardInBuffer();
                data_tx.Text = "";
                svmdata_text.Text = "";

                buffer.Clear();
                for(int i = 0; i < buf.Length; i++)
                {
                    buf[i] = 0;
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }

        }

        private void Myport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (Closing) return;
            try
            {
                Listening = true;
                int n = myport.BytesToRead;
                byte[] byt = new byte[n];
                myport.Read(byt, 0, n);
                buffer.AddRange(byt);
                this.Invoke(new EventHandler(displaydate_eve));
            }
            finally
            {
                Listening = false;//我用完了，ui可以关闭串口了。  
            }
        }

        private string bytetostring(double[] combyt, int len)  //int数组转为字符串
        {
            /* string returnstr = "@";
             for (int i = 0; i < len; i++)
             {
                 returnstr +=" "+(i+1)+":"+ Convert.ToString(combyt[i]);
             }
            // DateTime datetime = DateTime.Now;
            // string time = datetime.Minute + ":" + datetime.Second + ":" + datetime.Millisecond;
           //  returnstr += time;*/
            string returnstr = "";
            for (int i = 0; i < len; i++)
            {
                returnstr +=Convert.ToString(combyt[i])+"\t";
            }
            return returnstr;
        }
        private string toSVMstring(double[] combyt, int len=146)  //int数组转为字符串
        {
            string returnstr = "@";
            int j = 1;
            for (int i = 0; i < len; i++)
            {
                returnstr += " "+Convert.ToString(j++)+":"+Convert.ToString(combyt[i]) ;
            }
          //  DateTime datetime = DateTime.Now;
           // string time = datetime.Minute + ":" + datetime.Second + ":" + datetime.Millisecond;
         //   returnstr += time;
            return returnstr;
        }


        private void displaydate_eve(object sender, EventArgs e)
        {
            It_data I_t=new It_data();
            


            while (buffer.Count >= 48) //至少包含帧头、长度（1字节）根据设计不同而不同
            {
                //2.1 查找数据头
                if (buffer[0] == sign_start&&buffer[1]==start_frame[1]&&buffer[2]==start_frame[2]&& buffer[3] == start_frame[3]) //传输数据有帧头，用于判断
                {
                    int len = buffer[1] * 16 * 16 + buffer[2];
                    if (buffer.Count < len + 4 )//数据区尚未接收完整
                    {
                        break;
                    }
                    buffer.CopyTo(0, buf, 0, len + 4);
                    buffer.RemoveRange(0, len + 4);
                    My_Pressure.Get_data(buf);
                    //data_tx.AppendText(bytetostring(My_Pressure.data, 16)+"\n");
                    temp += bytetostring(My_Pressure.data, 16) + "\n";
                    My_Pressure.GetI();
                    I_t.I_L = My_Pressure.I_L;
                    I_t.I_R = My_Pressure.I_R;
                    My_Pressure.It_temp.Add(I_t);


                    if (I_t.I_L < 20000 && I_t.I_L >= 0)
                    {
                        progressBar1.Value = Convert.ToInt32(I_t.I_L);
                    }
                    else
                    {
                        progressBar1.Value = 20000;
                    }
                    if (I_t.I_R < 20000&& I_t.I_R>=0)
                    {
                        progressBar2.Value = Convert.ToInt32(I_t.I_R);
                    }
                    else
                    {
                        progressBar2.Value = 20000;
                    }



                    if (My_Pressure.It_temp.Count > before_len+1)
                    {
                        My_Pressure.It_temp.RemoveAt(0);
                    }

                 //   data_tx.AppendText("\t"+I_t.I_L.ToString("0.000") +"\t"+ I_t.I_R.ToString("0.000") + "\n");
                  

                   if (I_t > My_Pressure.I_tm || My_Pressure.It_Flag)
                    {
                        if (My_Pressure.It_Flag && I_t < My_Pressure.I_tm)
                        {
                            My_Pressure.It_Flag = false;
                            My_Pressure.buffer_times = end_len;
                            end_flag = true;

                        }else if(!My_Pressure.It_Flag && I_t > My_Pressure.I_tm)
                        {
                            if (!end_flag)
                            {
                                for (int i = 0; i < before_len+1; i++)
                                {
                                    My_Pressure.before_frame.Add(My_Pressure.pressure_data[i+3]);
                                    My_Pressure.It_data.Add(My_Pressure.It_temp[i]);
                                }
                            }
                            else
                            {
                                end_flag = false;
                                My_Pressure.middle_frame.Add(My_Pressure.pressure_data[My_Pressure.pressure_data.Count - 1]);
                                My_Pressure.It_data.Add(I_t);
                            }
                            
                            My_Pressure.It_Flag = true;
                            My_Pressure.buffer_times = 0;
                        }
                        else if(My_Pressure.It_Flag && I_t > My_Pressure.I_tm)
                        {
                            My_Pressure.middle_frame.Add(My_Pressure.pressure_data[My_Pressure.pressure_data.Count - 1]);
                            My_Pressure.It_data.Add(I_t);
                        }

                    }
                    if (My_Pressure.buffer_times > 0&&end_flag)
                    {
                        My_Pressure.middle_frame.Add(My_Pressure.pressure_data[My_Pressure.pressure_data.Count - 1]);
                        My_Pressure.It_data.Add(I_t);
                        My_Pressure.buffer_times--;
                        if (My_Pressure.buffer_times == 0)
                        {
                            end_flag = false;
                            //   this.Invoke(new EventHandler(action_event));
                            action_event();

                        }
                    }

                }
                else //帧头不正确时，记得清除
                {
                   
                    buffer.RemoveAt(0);
                }
            }
        }
       

        /// <summary>
        /// 检测到一次完整动作后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void action_event( )
        {
            Feature My_feature=new Feature(My_Pressure.chang_data(), My_Pressure.It_data.Count, My_Pressure.getIL(0), My_Pressure.getIL(1));
            DateTime now_time = DateTime.Now;
          //  double gesture = 0;
            
            if (now_time - time_record > time_span)
            {
                svmdata_text.AppendText(toSVMstring(My_feature.GetFeature()) + "\n");

              //  gesture = svmsort.get_predicresult(toSVMstring(My_feature.GetFeature()) + "\n");
               // svmdata_text.AppendText(gesture.ToString());
                time_record = now_time;
            }
            My_Pressure.It_data.Clear();//使用过之后清空list
        }

    

        private void stop_bt_Click(object sender, EventArgs e)
        {
            try
            {
                Closing = true;
                while (Listening) Application.DoEvents();
                //打开时点击，则关闭串口  
                myport.Close();
                Closing = false;
            }
            catch (Exception ex2)
            {
                MessageBox.Show(ex2.Message, "Error");
            }
        }

        private void save_bt_Click(object sender, EventArgs e)
        {
            try
            {
                string pathfile = @"C:\Users\hjt\Desktop\";
                string filename = "pressure_data.txt";
                temp = temp.Replace("\n", "\r\n");
                //  System.IO.File.WriteAllText(pathfile + filename, data_tx.Text);
                System.IO.File.WriteAllText(pathfile + filename, temp);
                temp = "";
                pathfile = @"C:\Users\hjt\Desktop\";
                filename = "SVM_data.txt";
                svmdata_text.Text = svmdata_text.Text.Replace("\n", "\r\n");
                System.IO.File.WriteAllText(pathfile + filename, svmdata_text.Text);
                MessageBox.Show("Data has been saved to " + pathfile, "save file");
            }
            catch (Exception ex3)
            {
                MessageBox.Show(ex3.Message, "Error");
            }
        }

        private static void SetPortValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
            {
                obj.Items.Add(str);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SetPortValues(comboBoxDKH);
            if (comboBoxDKH.Items.Count == 0)
            {
                MessageBox.Show("未扫描到该计算机的COM口，请添加硬件后重新打开程序！", "提示信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            comboBoxDKH.SelectedIndex = 0;
            comboBoxDKH.SelectedIndexChanged += (o, ex) => { myport.PortName = comboBoxDKH.SelectedItem.ToString(); };
            myport = new SerialPort();
            myport.BaudRate = 9600;
            myport.PortName = comboBoxDKH.SelectedItem.ToString();
            myport.Parity = Parity.None;
            myport.DataBits = 8;
            myport.StopBits = StopBits.One;
            myport.DataReceived += Myport_DataReceived;

        }

        private void comboBoxDKH_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
