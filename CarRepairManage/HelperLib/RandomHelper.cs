using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperLib
{
    public  class RandomHelper
    {
        /// <summary>
        /// 生成指定范围内的不重复随机数
        /// </summary>
        /// <param name="Number">Number随机数个数</param>
        /// <param name="minNum">minNum随机数下限</param>
        /// <param name="maxNum">maxNum随机数上限</param>
        /// <returns></returns>
        public static int[] GetRandomArray(int Number, int minNum, int maxNum)
        {
            int j;
            int[] b = new int[Number];
            Random r = new Random();
            for (j = 0; j < Number; j++)
            {
                int i = r.Next(minNum, maxNum + 1);
                int num = 0;
                for (int k = 0; k < j; k++)
                {
                    if (b[k] == i)
                    {
                        num = num + 1;
                    }
                }
                if (num == 0)
                {
                    b[j] = i;
                }
                else
                {
                    j = j - 1;
                }
            }
            return b;
        }
    }
}
