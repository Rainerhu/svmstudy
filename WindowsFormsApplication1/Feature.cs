using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Transformations;

/// <summary>
/// 使用时注意构造函数的格式，直接调用GetFeature函数可以返回一个一维数组；
/// </summary>
namespace WindowsFormsApplication1
{
    class Feature
    {
        private RealFourierTransformation rft = new RealFourierTransformation();

        private double[] Amplitude = new double[48];                   //用于储存幅值大小
        private double[] Phase = new double[48];                       //用于储存相位大小
        private const int NUM_OF_SENSOR = 16;
        private const int NUM_OF_LEFT_SENSOR = 8;                      //左脚的传感器数目
        private const int NUM_OF_RIGHT_SENSOR = 8;                     //右脚的传感器数目
        private double[] max_per_sensor = new double[NUM_OF_SENSOR];   //每个传感器的最大值
        private double[] min_per_sensor = new double[NUM_OF_SENSOR];   //每个传感器的最小值
        private double[] inner_product = new double[NUM_OF_SENSOR];    //I(t)和df/dt的内积
        private double min_of_sum_right;
        private double min_of_sum_left;                                //每只脚的压力数值和的最小值
        private readonly int LENGTH_OF_DATA;                           //数据段长度，即time span||segment 的长度
                                                                       //注意：第一个数据只用作求导数，没有别的意义；

        private readonly double[,] sensor_pressure;                    //一个二维数组，存储每个传感器在确定帧数的压力值

        private readonly List<double> I_Left;
        private readonly List<double> I_Right;
        private double[] data_feature = new double[146];              //最终的数据，存在一个146长度的数组中

        /// <summary>
        /// 定义用于傅里叶变换的数组，注：因数据特殊性，此部分数据只能用于傅里叶变换使用；
        /// </summary>
        private readonly double[] Left1;
        private readonly double[] Left2;
        private readonly double[] Left3;
        private readonly double[] Left4;
        private readonly double[] Left5;
        private readonly double[] Left6;
        private readonly double[] Left7;
        private readonly double[] Left8;
        private readonly double[] Right1;
        private readonly double[] Right2;
        private readonly double[] Right3;
        private readonly double[] Right4;
        private readonly double[] Right5;
        private readonly double[] Right6;
        private readonly double[] Right7;
        private readonly double[] Right8;
        private double[] RealOutLeft1;
        private double[] RealOutLeft2;
        private double[] RealOutLeft3;
        private double[] RealOutLeft4;
        private double[] RealOutLeft5;
        private double[] RealOutLeft6;
        private double[] RealOutLeft7;
        private double[] RealOutLeft8;
        private double[] RealOutRight1;
        private double[] RealOutRight2;
        private double[] RealOutRight3;
        private double[] RealOutRight4;
        private double[] RealOutRight5;
        private double[] RealOutRight6;
        private double[] RealOutRight7;
        private double[] RealOutRight8;
        private double[] ImagOutLeft1;
        private double[] ImagOutLeft2;
        private double[] ImagOutLeft3;
        private double[] ImagOutLeft4;
        private double[] ImagOutLeft5;
        private double[] ImagOutLeft6;
        private double[] ImagOutLeft7;
        private double[] ImagOutLeft8;
        private double[] ImagOutRight1;
        private double[] ImagOutRight2;
        private double[] ImagOutRight3;
        private double[] ImagOutRight4;
        private double[] ImagOutRight5;
        private double[] ImagOutRight6;
        private double[] ImagOutRight7;
        private double[] ImagOutRight8;

        /// <summary>
        /// 构造函数，参数分别为传感器数据，总帧数和It数据
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="count"></param>
        /// <param name="It"></param>
        public Feature(double[,] arr, int count, List<double> IL, List<double> IR)
        {
            sensor_pressure = arr;
            LENGTH_OF_DATA = count;
            I_Left = IL;
            I_Right = IR;
            if (LENGTH_OF_DATA <= 17)
            {
                Left1 = new double[16];
                Left2 = new double[16];
                Left3 = new double[16];
                Left4 = new double[16];
                Left5 = new double[16];
                Left6 = new double[16];
                Left7 = new double[16];
                Left8 = new double[16];
                Right1 = new double[16];
                Right2 = new double[16];
                Right3 = new double[16];
                Right4 = new double[16];
                Right5 = new double[16];
                Right6 = new double[16];
                Right7 = new double[16];
                Right8 = new double[16];
                for (int x = 0; x < LENGTH_OF_DATA - 1; x++)
                {
                    Left1[x] = sensor_pressure[0, x + 1];
                    Left2[x] = sensor_pressure[1, x + 1];
                    Left3[x] = sensor_pressure[2, x + 1];
                    Left4[x] = sensor_pressure[3, x + 1];
                    Left5[x] = sensor_pressure[4, x + 1];
                    Left6[x] = sensor_pressure[5, x + 1];
                    Left7[x] = sensor_pressure[6, x + 1];
                    Left8[x] = sensor_pressure[7, x + 1];
                    Right1[x] = sensor_pressure[8, x + 1];
                    Right2[x] = sensor_pressure[9, x + 1];
                    Right3[x] = sensor_pressure[10, x + 1];
                    Right4[x] = sensor_pressure[11, x + 1];
                    Right5[x] = sensor_pressure[12, x + 1];
                    Right6[x] = sensor_pressure[13, x + 1];
                    Right7[x] = sensor_pressure[14, x + 1];
                    Right8[x] = sensor_pressure[15, x + 1];
                }
                for (int x = LENGTH_OF_DATA - 1; x < 16; x++)
                {
                    Left1[x] = 0;
                    Left2[x] = 0;
                    Left3[x] = 0;
                    Left4[x] = 0;
                    Left5[x] = 0;
                    Left6[x] = 0;
                    Left7[x] = 0;
                    Left8[x] = 0;
                    Right1[x] = 0;
                    Right2[x] = 0;
                    Right3[x] = 0;
                    Right4[x] = 0;
                    Right5[x] = 0;
                    Right6[x] = 0;
                    Right7[x] = 0;
                    Right8[x] = 0;
                }
            }
            else if (LENGTH_OF_DATA <= 33)
            {
                Left1 = new double[32];
                Left2 = new double[32];
                Left3 = new double[32];
                Left4 = new double[32];
                Left5 = new double[32];
                Left6 = new double[32];
                Left7 = new double[32];
                Left8 = new double[32];
                Right1 = new double[32];
                Right2 = new double[32];
                Right3 = new double[32];
                Right4 = new double[32];
                Right5 = new double[32];
                Right6 = new double[32];
                Right7 = new double[32];
                Right8 = new double[32];
                for (int x = 0; x < LENGTH_OF_DATA - 1; x++)
                {
                    Left1[x] = sensor_pressure[0, x + 1];
                    Left2[x] = sensor_pressure[1, x + 1];
                    Left3[x] = sensor_pressure[2, x + 1];
                    Left4[x] = sensor_pressure[3, x + 1];
                    Left5[x] = sensor_pressure[4, x + 1];
                    Left6[x] = sensor_pressure[5, x + 1];
                    Left7[x] = sensor_pressure[6, x + 1];
                    Left8[x] = sensor_pressure[7, x + 1];
                    Right1[x] = sensor_pressure[8, x + 1];
                    Right2[x] = sensor_pressure[9, x + 1];
                    Right3[x] = sensor_pressure[10, x + 1];
                    Right4[x] = sensor_pressure[11, x + 1];
                    Right5[x] = sensor_pressure[12, x + 1];
                    Right6[x] = sensor_pressure[13, x + 1];
                    Right7[x] = sensor_pressure[14, x + 1];
                    Right8[x] = sensor_pressure[15, x + 1];
                }
                for (int x = LENGTH_OF_DATA - 1; x < 32; x++)
                {
                    Left1[x] = 0;
                    Left2[x] = 0;
                    Left3[x] = 0;
                    Left4[x] = 0;
                    Left5[x] = 0;
                    Left6[x] = 0;
                    Left7[x] = 0;
                    Left8[x] = 0;
                    Right1[x] = 0;
                    Right2[x] = 0;
                    Right3[x] = 0;
                    Right4[x] = 0;
                    Right5[x] = 0;
                    Right6[x] = 0;
                    Right7[x] = 0;
                    Right8[x] = 0;
                }
            }
            else
            {
                Left1 = new double[64];
                Left2 = new double[64];
                Left3 = new double[64];
                Left4 = new double[64];
                Left5 = new double[64];
                Left6 = new double[64];
                Left7 = new double[64];
                Left8 = new double[64];
                Right1 = new double[64];
                Right2 = new double[64];
                Right3 = new double[64];
                Right4 = new double[64];
                Right5 = new double[64];
                Right6 = new double[64];
                Right7 = new double[64];
                Right8 = new double[64];
                if (LENGTH_OF_DATA <= 65)
                {
                    for (int x = 0; x < LENGTH_OF_DATA - 1; x++)
                    {
                        Left1[x] = sensor_pressure[0, x + 1];
                        Left2[x] = sensor_pressure[1, x + 1];
                        Left3[x] = sensor_pressure[2, x + 1];
                        Left4[x] = sensor_pressure[3, x + 1];
                        Left5[x] = sensor_pressure[4, x + 1];
                        Left6[x] = sensor_pressure[5, x + 1];
                        Left7[x] = sensor_pressure[6, x + 1];
                        Left8[x] = sensor_pressure[7, x + 1];
                        Right1[x] = sensor_pressure[8, x + 1];
                        Right2[x] = sensor_pressure[9, x + 1];
                        Right3[x] = sensor_pressure[10, x + 1];
                        Right4[x] = sensor_pressure[11, x + 1];
                        Right5[x] = sensor_pressure[12, x + 1];
                        Right6[x] = sensor_pressure[13, x + 1];
                        Right7[x] = sensor_pressure[14, x + 1];
                        Right8[x] = sensor_pressure[15, x + 1];
                    }
                    for (int x = LENGTH_OF_DATA - 1; x < 64; x++)
                    {
                        Left1[x] = 0;
                        Left2[x] = 0;
                        Left3[x] = 0;
                        Left4[x] = 0;
                        Left5[x] = 0;
                        Left6[x] = 0;
                        Left7[x] = 0;
                        Left8[x] = 0;
                        Right1[x] = 0;
                        Right2[x] = 0;
                        Right3[x] = 0;
                        Right4[x] = 0;
                        Right5[x] = 0;
                        Right6[x] = 0;
                        Right7[x] = 0;
                        Right8[x] = 0;
                    }
                }
                else
                {
                    for (int x = 0; x < 64; x++)
                    {
                        Left1[x] = sensor_pressure[0, x + 1];
                        Left2[x] = sensor_pressure[1, x + 1];
                        Left3[x] = sensor_pressure[2, x + 1];
                        Left4[x] = sensor_pressure[3, x + 1];
                        Left5[x] = sensor_pressure[4, x + 1];
                        Left6[x] = sensor_pressure[5, x + 1];
                        Left7[x] = sensor_pressure[6, x + 1];
                        Left8[x] = sensor_pressure[7, x + 1];
                        Right1[x] = sensor_pressure[8, x + 1];
                        Right2[x] = sensor_pressure[9, x + 1];
                        Right3[x] = sensor_pressure[10, x + 1];
                        Right4[x] = sensor_pressure[11, x + 1];
                        Right5[x] = sensor_pressure[12, x + 1];
                        Right6[x] = sensor_pressure[13, x + 1];
                        Right7[x] = sensor_pressure[14, x + 1];
                        Right8[x] = sensor_pressure[15, x + 1];
                    }
                }
            }
        }
        /// <summary>
        /// 求每个传感器的最大值
        /// </summary>
        private void SetMaxValue()
        {
            for (int i = 0; i < NUM_OF_SENSOR; i++)
            {
                max_per_sensor[i] = sensor_pressure[i, 0];
                for (int j = 1; j < LENGTH_OF_DATA; j++)               //j从1开始是因为j=0的数据只用作求导数
                {
                    if (sensor_pressure[i, j] > max_per_sensor[i])
                        max_per_sensor[i] = sensor_pressure[i, j];
                }
            }
        }

        /// <summary>
        /// 求每个传感器的最小值
        /// </summary>
        private void SetMinValue()
        {
            for (int i = 0; i < NUM_OF_SENSOR; i++)
            {
                min_per_sensor[i] = sensor_pressure[i, 0];
                for (int j = 1; j < LENGTH_OF_DATA; j++)               //j从1开始是因为j=0的数据只用作求导数
                {
                    if (sensor_pressure[i, j] < min_per_sensor[i])
                        min_per_sensor[i] = sensor_pressure[i, j];
                }
            }
        }

        /// <summary>
        /// 求每个传感器的内积
        /// 此处所用导数为两帧之间的差值，后续可以根据讨论修改
        /// </summary>
        private void SetInnerProduct()
        {
            for (int i = 0; i < NUM_OF_LEFT_SENSOR; i++)
            {
                double sum = I_Left[1] * (sensor_pressure[i, 1] - sensor_pressure[i, 0]);
                for (int j = 2; j < LENGTH_OF_DATA; j++)
                    sum = sum + I_Left[j] * (sensor_pressure[i, j] - sensor_pressure[i, j - 1]);
                inner_product[i] = sum;
            }
            for (int i = 8; i < NUM_OF_SENSOR; i++)
            {
                double sum = I_Right[1] * (sensor_pressure[i, 1] - sensor_pressure[i, 0]);
                for (int j = 2; j < LENGTH_OF_DATA; j++)
                    sum = sum + I_Right[j] * (sensor_pressure[i, j] - sensor_pressure[i, j - 1]);
                inner_product[i] = sum;
            }
        }

        /// <summary>
        /// 求两只脚的压力和的最小值
        /// </summary>
        private void SetMinOfSum()
        {
            double[] LeftPressureSum = new double[LENGTH_OF_DATA];      //存放左脚每帧的压力和
            LeftPressureSum[0] = 0;                                     //初值设置为0，因为不考虑第一帧
            double[] RightPressureSum = new double[LENGTH_OF_DATA];     //存放右脚每帧的压力和
            RightPressureSum[0] = 0;                                    //初值设置为0
            for (int j = 1; j < LENGTH_OF_DATA; j++)
            {
                double sum_left = sensor_pressure[0, j];
                double sum_right = sensor_pressure[8, j];
                for (int i = 1; i < NUM_OF_SENSOR; i++)
                {
                    if (i < NUM_OF_LEFT_SENSOR)
                        sum_left = sum_left + sensor_pressure[i, j];
                    if (i > NUM_OF_LEFT_SENSOR)
                        sum_right = sum_right + sensor_pressure[i, j];
                }
                LeftPressureSum[j] = sum_left;
                RightPressureSum[j] = sum_right;
            }

            double min_left = LeftPressureSum[1];
            for (int j = 1; j < LENGTH_OF_DATA; j++)
                if (LeftPressureSum[j] < min_left)
                    min_left = LeftPressureSum[j];

            double min_right = RightPressureSum[1];
            for (int j = 1; j < LENGTH_OF_DATA; j++)
                if (RightPressureSum[j] < min_right)
                    min_right = RightPressureSum[j];

            min_of_sum_left = min_left;
            min_of_sum_right = min_right;
        }

        /// <summary>
        /// 快速傅里叶变换
        /// </summary>
        private void FastFourierTransformation()
        {
            rft.Convention = TransformationConvention.Matlab;
            rft.TransformForward(Left1, out RealOutLeft1, out ImagOutLeft1);
            rft.TransformForward(Left2, out RealOutLeft2, out ImagOutLeft2);
            rft.TransformForward(Left3, out RealOutLeft3, out ImagOutLeft3);
            rft.TransformForward(Left4, out RealOutLeft4, out ImagOutLeft4);
            rft.TransformForward(Left5, out RealOutLeft5, out ImagOutLeft5);
            rft.TransformForward(Left6, out RealOutLeft6, out ImagOutLeft6);
            rft.TransformForward(Left7, out RealOutLeft7, out ImagOutLeft7);
            rft.TransformForward(Left8, out RealOutLeft8, out ImagOutLeft8);
            rft.TransformForward(Right1, out RealOutRight1, out ImagOutRight1);
            rft.TransformForward(Right2, out RealOutRight2, out ImagOutRight2);
            rft.TransformForward(Right3, out RealOutRight3, out ImagOutRight3);
            rft.TransformForward(Right4, out RealOutRight4, out ImagOutRight4);
            rft.TransformForward(Right5, out RealOutRight5, out ImagOutRight5);
            rft.TransformForward(Right6, out RealOutRight6, out ImagOutRight6);
            rft.TransformForward(Right7, out RealOutRight7, out ImagOutRight7);
            rft.TransformForward(Right8, out RealOutRight8, out ImagOutRight8);
        }

        private double GetAmp(double real, double image)
        {
            double temp = Math.Sqrt(real * real + image * image);
            return temp;
        }

        private double GetPhase(double real, double image)
        {
            double temp = Math.Atan2(image, real);
            return temp;
        }

        /// <summary>
        /// 获得最低三次频率的幅度，存入Phase[]中；
        /// </summary>
        private void GetAmpofTLF()
        {
            int i = 0;
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft1[j], ImagOutLeft1[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft2[j], ImagOutLeft2[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft3[j], ImagOutLeft3[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft4[j], ImagOutLeft4[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft5[j], ImagOutLeft5[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft6[j], ImagOutLeft6[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft7[j], ImagOutLeft7[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutLeft8[j], ImagOutLeft8[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight1[j], ImagOutRight1[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight2[j], ImagOutRight2[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight3[j], ImagOutRight3[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight4[j], ImagOutRight4[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight5[j], ImagOutRight5[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight6[j], ImagOutRight6[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight7[j], ImagOutRight7[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Amplitude[i] = GetAmp(RealOutRight8[j], ImagOutRight8[j]);
                i++;
            }
        }

        /// <summary>
        /// 获得最低三次频率的相位，存入Phase[]中；
        /// </summary>
        private void GetPhofTLF()
        {
            int i = 0;
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft1[j], ImagOutLeft1[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft2[j], ImagOutLeft2[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft3[j], ImagOutLeft3[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft4[j], ImagOutLeft4[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft5[j], ImagOutLeft5[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft6[j], ImagOutLeft6[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft7[j], ImagOutLeft7[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutLeft8[j], ImagOutLeft8[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight1[j], ImagOutRight1[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight2[j], ImagOutRight2[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight3[j], ImagOutRight3[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight4[j], ImagOutRight4[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight5[j], ImagOutRight5[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight6[j], ImagOutRight6[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight7[j], ImagOutRight7[j]);
                i++;
            }
            for (int j = 0; j < 3; j++)
            {
                Phase[i] = GetPhase(RealOutRight8[j], ImagOutRight8[j]);
                i++;
            }
        }

        /// <summary>
        /// 此函数将所有数据传送到data_feature数组里
        /// </summary>
        private void TransferFeature()
        {
            int i = 0;
            for (int j = 0; j < 16; j++, i++)
            {
                data_feature[i] = max_per_sensor[j];
            }
            for (int j = 0; j < 16; j++, i++)
            {
                data_feature[i] = min_per_sensor[j];
            }
            for (int j = 0; j < 16; j++, i++)
            {
                data_feature[i] = inner_product[j];
            }
            data_feature[i++] = min_of_sum_left;
            //i++
            data_feature[i++] = min_of_sum_right;
           // i++;
            for (int j = 0; j < 48; j++, i++)
            {
                data_feature[i] = Amplitude[j];
            }
            for (int j = 0; j < 48; j++, i++)
            {
                data_feature[i] = Phase[j];
            }
        }

        public double[] GetFeature()
        {
            SetMaxValue();
            SetMinValue();
            SetInnerProduct();
            SetMinOfSum();
            FastFourierTransformation();
            GetAmpofTLF();
            GetPhofTLF();
            TransferFeature();
            return data_feature;
        }
    }
}
