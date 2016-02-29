using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WpfApplication2.Util
{
    class ConvertByteAndHex
    {
        public static string BytesToHex(byte[] bytes)
        {
            string strTemp = "";

            for (int i = 0; i < bytes.Length; i++)
            {
                strTemp += bytes[i].ToString("X2") + ' ';
            }
            return strTemp;
        }

        //格式示例 "0A 0B 1C ..."
        public static byte[] HexToBytes(string s)
        {
            string[] byteString = s.Split(new char[] { ' ' });
            byte[] xx = new byte[byteString.Length];
            for (int i = 0; i < byteString.Length; i++)
            {
                string cip = byteString[i];
                if (cip.Length != 0)
                {
                    xx[i] = byte.Parse(cip.ToString(), System.Globalization.NumberStyles.AllowHexSpecifier);
                }
            }
            return xx;
        }
    }
}
