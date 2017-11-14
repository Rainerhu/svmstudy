using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WindowsFormsApplication1

{

    /// <summary>
    /// 储存一帧16个压力数据类
    /// </summary>
    public class data_save
    {
       public double[] data=new double[16] ;
        public void get_data(double[] temp)
        {
            Array.Copy(temp, 0,this.data, 0, 16);
        }
    };

    public class It_data
    {
        public double I_L ;
        public double I_R ;

        public It_data(double a=0,double b=0)
        {
            I_L = a;
            I_R = b;
        }

        public static bool operator > (It_data a, It_data b)
        {
            if (a.I_L > b.I_L || a.I_R > b.I_R)
            {
                return true;
            }else
            {
                return false;
            }

            
        }
        public static bool operator < (It_data a, It_data b)
        {
            if (a.I_L < b.I_L && a.I_R < b.I_R)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    };

    public class Pressure
    {
        const int len = 9;
        const int NUM = 16;
        const double ALPHA = 0.3;
        //  public List<int> Left_1 = new List<int>(len), Left_2 = new List<int>(len), Left_3 = new List<int>(len), Left_4 = new List<int>(len), Left_5 = new List<int>(len), Left_6 = new List<int>(len), Left_7 = new List<int>(len), Left_8 = new List<int>(len), Right_1 = new List<int>(len), Right_2 = new List<int>(len), Right_3 = new List<int>(len), Right_4 = new List<int>(len), Right_5 = new List<int>(len), Right_6 = new List<int>(len), Right_7 = new List<int>(len), Right_8 = new List<int>(len);
        public List<data_save> pressure_data = new List<data_save>(len);
        public int count = 0;
        public bool EndFlag = false;
        public double[] data = new double[16];

        public  const  int before_len = 5;
        public const int end_len = 4;
        public List<data_save> before_frame = new List<data_save>(before_len);
        public List<data_save> middle_frame = new List<data_save>(1);
       // public List<data_save> end_frame = new List<data_save>(end_len);
        //private double[,] end_frame = new double[4, 16];
        public bool It_Flag = false;
        public int buffer_times = 0;
        public readonly It_data I_tm = new It_data(5000,5000);
        public List<It_data> It_data = new List<It_data>(1);
        public List<It_data> It_temp = new List<It_data>(1);


        //开始定义
        private double[] deri_of_sensors = new double[NUM];   //每个传感器的变化量（导数）
        private double[] Left_1_EMA = new double[len];        //每个传感器的EMA
        private double[] Left_2_EMA = new double[len];
        private double[] Left_3_EMA = new double[len];
        private double[] Left_4_EMA = new double[len];
        private double[] Left_5_EMA = new double[len];
        private double[] Left_6_EMA = new double[len];
        private double[] Left_7_EMA = new double[len];
        private double[] Left_8_EMA = new double[len];
        private double[] Right_1_EMA = new double[len];        //每个传感器的EMA
        private double[] Right_2_EMA = new double[len];
        private double[] Right_3_EMA = new double[len];
        private double[] Right_4_EMA = new double[len];
        private double[] Right_5_EMA = new double[len];
        private double[] Right_6_EMA = new double[len];
        private double[] Right_7_EMA = new double[len];
        private double[] Right_8_EMA = new double[len];


        private double[] diff = new double[NUM];              //传感器数值-对应的EMA
        private int max_index_L = 0;
        private int max_index_R = 0;    //最大差对应的索引
        private int min_index_L = 0;
        private int min_index_R = 0;//最小差对应的索引
        public double I_L = 0;
        public double I_R = 0;
        //定义结束

        /// <summary>
        /// 获取某个传感器多帧数据，返回值为list形式
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public List<double> get_onepressure(int order)
        {
            List<double> temp = new List<double>(len);
            for(int i = 0; i < len; i++)
            {
                temp.Add(this.pressure_data[i].data[order]);
            }
            return temp;
        }

        //methods:
        /// <summary>
        /// 设置每个传感器的导数
        /// </summary>
        public void SetDeriOfSensor()
        {
          
           for(int i = 0; i < NUM; i++)
            {
                deri_of_sensors[i] = this.pressure_data[len - 1].data[i] - this.pressure_data[len - 2].data[i];

            }
        }

        /// <summary>
        /// 设置EMA的值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetEMA(double[] x, List<double> y)
        {
            x[0] = y[0];
            for (int i = 1; i < 8; i++)
            {
                x[i] = ALPHA * y[i] + (1 - ALPHA) * x[i - 1];
            }
        }

        /// <summary>
        /// 计算传感器数据和其EMA的差值
        /// </summary>
        public void SetDiff()
        {
            int i = 0;
            diff[i] = this.pressure_data[8].data[i++] - Left_1_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_2_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_3_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_4_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_5_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_6_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_7_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Left_8_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_1_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_2_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_3_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_4_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_5_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_6_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_7_EMA[8];
            diff[i] = this.pressure_data[8].data[i++] - Right_8_EMA[8];
            i = 0;
        }

        /// <summary>
        /// 求最大差值
        /// </summary>
        public void MaxDiff()
        {
            max_index_L = 0;
            double max = diff[0];
            for (int i = 0; i <= 7; i++)
            {
                if (diff[i] > max)
                {
                    max = diff[i];
                    max_index_L = i;
                }
            }
            max_index_R = 8;
            max = diff[8];
            for (int i = 8; i <= 15; i++)
            {
                if (diff[i] > max)
                {
                    max = diff[i];
                    max_index_R = i;
                }
            }
        }

        /// <summary>
        /// 求最小差值
        /// </summary>
        public void MinDiff()
        {
            min_index_L = 0;
            double min = diff[0];
            for (int i = 0; i <= 7; i++)
            {
                if (diff[i] < min)
                {
                    min = diff[i];
                    min_index_L = i;
                }
            }
            min_index_R = 8;
            min = diff[8];
            for (int i = 8; i <= 15; i++)
            {
                if (diff[i] < min)
                {
                    min = diff[i];
                    min_index_R = i;
                }
            }
        }

        /// <summary>
        /// 计算最终的I(t)
        /// </summary>
        public void CalI()
        {
            if (EndFlag)
            {
                int i = 0;
                SetDeriOfSensor();
                SetEMA(Left_1_EMA, this.get_onepressure(i++));
                SetEMA(Left_2_EMA, this.get_onepressure(i++));
                SetEMA(Left_3_EMA, this.get_onepressure(i++));
                SetEMA(Left_4_EMA, this.get_onepressure(i++));
                SetEMA(Left_5_EMA, this.get_onepressure(i++));
                SetEMA(Left_6_EMA, this.get_onepressure(i++));
                SetEMA(Left_7_EMA, this.get_onepressure(i++));
                SetEMA(Left_8_EMA, this.get_onepressure(i++));
                SetEMA(Right_1_EMA, this.get_onepressure(i++));
                SetEMA(Right_2_EMA, this.get_onepressure(i++));
                SetEMA(Right_3_EMA, this.get_onepressure(i++));
                SetEMA(Right_4_EMA, this.get_onepressure(i++));
                SetEMA(Right_5_EMA, this.get_onepressure(i++));
                SetEMA(Right_6_EMA, this.get_onepressure(i++));
                SetEMA(Right_7_EMA, this.get_onepressure(i++));
                SetEMA(Right_8_EMA, this.get_onepressure(i++));
                SetDiff();
                MaxDiff();
                MinDiff();
                I_L = (diff[max_index_L] - diff[min_index_L]) * Math.Max(0, deri_of_sensors[max_index_L]);
                I_R = (diff[max_index_R] - diff[min_index_R]) * Math.Max(0, deri_of_sensors[max_index_R]);
            }
            else
            {
                I_L = 0;
                I_R = 0;
            }
        }
        /// <summary>
        /// 获取I(t)
        /// </summary>
        /// <returns></returns>
        public void GetI()
        {
            CalI();
        }

        /// <summary>
        /// 获取某只脚I(t)的List，foot=0左脚，foot=1右脚
        /// </summary>
        /// <param name="foot"></param>
        /// <returns></returns>
        public List<double> getIL(int foot)
        {
            List<double> temp = new List<double>(1);
            if (foot == 0)
            {
                for (int i =0 ; i <It_data.Count; i++)
                {
                    temp.Add(It_data[i].I_L);
                }
            }else if (foot == 1)
            {
                for (int i = 0; i < It_data.Count; i++)
                {
                    temp.Add(It_data[i].I_R);
                }
            }

            return temp;
        }

        /// <summary>
        /// 转换为参数计算需要的数据，返回一个数组[16][帧数]
        /// </summary>
        public double[,] chang_data()
        {
            double[,] temp = new double[16, It_data.Count];
            int frame = 0;
            for (int i = 0; i < before_frame.Count; i++)//i为帧数j为传感器序数
            {
                for (int j = 0; j < 16; j++)
                {
                    temp[j, frame] = before_frame[i].data[j];
                }
                frame++;
            }
            for (int i = 0; i < middle_frame.Count; i++)//i为帧数j为传感器序数
            {
                for (int j = 0; j < 16; j++)
                {
                    temp[j, frame] = middle_frame[i].data[j];
                }
                frame++;
            }
          /*  for (int i = 0; i < end_frame.Count; i++)//i为帧数j为传感器序数
            {
                for (int j = 0; j < 16; j++)
                {
                    temp[j, frame] = end_frame[i].data[j];
                }
                frame++;
            }*/
            frame = 0;
            before_frame.Clear();
            middle_frame.Clear();
           // end_frame.Clear();
            return temp;
        }


        /// <summary>
        /// 获取串口数据
        /// </summary>
        /// <param name="combyt"></param>
        public void Get_data(byte[] combyt)
        {
            // const byte sign_change = 0x7D;
            //const byte byte_change = 0x20;
            const int start_order = 15;      //从开始位到数据位的字节数
            data_save temp = new data_save();
            //   bool whether_change = false;
            //  int pressure_order = 0;

            int j = 0;
            for (int i = start_order; i < 47; i++)
            {
                data[j++] = combyt[i++] * 16 * 16 + combyt[i];

                /*     if (combyt[i] != sign_change && !whether_change)
                     {
                         data[j++] = combyt[i];
                     }
                     else if (whether_change)
                     {
                         data[j++] = combyt[i] ^ byte_change;
                         whether_change = false;

                     }
                     else if (combyt[i] == sign_change)
                     {
                         whether_change = true;

                     }*/
            }
            j = 0;

            if (count <= len  && !EndFlag)
            {
                count++;
            }
            else
            {
                EndFlag = true;
            }
            if (EndFlag|| pressure_data.Count>=10)
            {

                this.pressure_data.RemoveAt(0);
            }
            temp.get_data(data);
            this.pressure_data.Add(temp);
        }

            /*
            this.Left_1.RemoveAt(0);
            this.Left_2.RemoveAt(0);
            this.Left_3.RemoveAt(0);
            this.Left_4.RemoveAt(0);
            this.Left_5.RemoveAt(0);
            this.Left_6.RemoveAt(0);
            this.Left_7.RemoveAt(0);
            this.Left_8.RemoveAt(0);
            this.Right_1.RemoveAt(0);
            this.Right_2.RemoveAt(0);
            this.Right_3.RemoveAt(0);
            this.Right_4.RemoveAt(0);
            this.Right_5.RemoveAt(0);
            this.Right_6.RemoveAt(0);
            this.Right_7.RemoveAt(0);
            this.Right_8.RemoveAt(0);*/




            /*
                        for (int i = 0; i < 16;)
                        {
                            switch (pressure_order)
                            {
                                case 0:
                                    {

                                        this.Left_1.Add(data[i++]);
                                        pressure_order++;
                                        break;
                                    }
                                case 1:
                                    {
                                        this.Left_2.Add(data[i++]);
                                        pressure_order++;
                                        break;
                                    }
                                case 2:
                                    {
                                        this.Left_3.Add(data[i++]);
                                        pressure_order++;
                                        break;
                                    }
                                case 3:
                                    {
                                        this.Left_4.Add(data[i++]);
                                        pressure_order++;
                                        break;
                                    }
                                case 4:
                                    {
                                        this.Left_5.Add(data[i++]);
                                        pressure_order++;
                                        break;
                                    }
                                case 5:
                                    {
                                        pressure_order++;
                                        this.Left_6.Add(data[i++]);
                                        break;
                                    }
                                case 6:
                                    {
                                        pressure_order++;
                                        this.Left_7.Add(data[i++]);
                                        break;
                                    }
                                case 7:
                                    {
                                        pressure_order++;
                                        this.Left_8.Add(data[i++]);
                                        break;
                                    }
                                case 8:
                                    {
                                        pressure_order++;
                                        this.Right_1.Add(data[i++]);
                                        break;
                                    }
                                case 9:
                                    {
                                        pressure_order++;
                                        this.Right_2.Add(data[i++]);
                                        break;
                                    }
                                case 10:
                                    {
                                        pressure_order++;
                                        this.Right_3.Add(data[i++]);
                                        break;
                                    }
                                case 11:
                                    {
                                        pressure_order++;
                                        this.Right_4.Add(data[i++]);
                                        break;
                                    }
                                case 12:
                                    {
                                        pressure_order++;
                                        this.Right_5.Add(data[i++]);
                                        break;
                                    }
                                case 13:
                                    {
                                        pressure_order++;
                                        this.Right_6.Add(data[i++]);
                                        break;
                                    }
                                case 14:
                                    {
                                        pressure_order++;
                                        this.Right_7.Add(data[i++]);
                                        break;
                                    }
                                case 15:
                                    {
                                        pressure_order++;
                                        this.Right_8.Add(data[i++]);
                                        break;
                                    }

                            }

                        }*/

       

    }



}
