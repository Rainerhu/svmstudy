using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;
using LibSVMsharp;
using System.IO;
using System.Globalization;


namespace WindowsFormsApplication1
{
    public class SVMsort
    {
        public SVMModel model_1;
        public SVMModel model_2;
      
        public SVMsort()
        {
            model_1 = SVM.LoadModel(@"Model\model_1.txt");
            model_2 = SVM.LoadModel(@"Model\model_2.txt");
        }
        public SVMProblem get_svmproblem(string line)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            SVMProblem problem=new SVMProblem();
            string[] list = line.Trim().Split(' ');

            double y = Convert.ToDouble(list[0].Trim(), provider);

            List<SVMNode> nodes = new List<SVMNode>();
            for (int i = 1; i < list.Length; i++)
            {
                string[] temp = list[i].Split(':');
                SVMNode node = new SVMNode();
                node.Index = Convert.ToInt32(temp[0].Trim());
                node.Value = Convert.ToDouble(temp[1].Trim(), provider);
                nodes.Add(node);
            }

            problem.Add(nodes.ToArray(), y);

            return problem;
        }

        public double get_predicresult(string line)
        {
            double temp = 0;
            SVMProblem problem = this.get_svmproblem(line);
            problem = problem.Normalize(SVMNormType.L2);

            if (problem.Predict(model_1)[0] == 1)//模型1中1为特殊姿态
            {
                temp = problem.Predict(model_2)[0];

            }else if(problem.Predict(model_1)[0] == 2)//模型1中2为日常姿态
            {
                temp = 11;
            }
            return temp;
        }
    }
}
