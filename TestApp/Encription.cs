using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    class Encription
    {
        private string T;
        public string Text
        {
            get { return T; }
            set { T = value; }
        }
        public string EncryptedText
        {
            get { return Encrypt(); }
        }
        public string DecryptedText
        {
            get { return Decrypt(); }
        }
        public Encription(string s)
        {
            Text = s;
        }
        public Encription()
        {
            T = "";
        }
        private string Encrypt()
        {
            try
            {
                string s = T;
                long lSeed = DateTime.Now.Ticks;
                int seed = (int)lSeed;
                Random rand = new Random(seed);
                int bitShiftS = rand.Next(1, 7);
                int bitShift;
                char[] c = new char[s.Length + seed.ToString().Length + 1];
                int[] iA = new int[s.Length];
                for (int i = seed.ToString().Length + 1; i < s.Length + seed.ToString().Length + 1; i++)
                {
                    iA[i - seed.ToString().Length - 1] = rand.Next(s.Length);
                    c[i] = s[i - seed.ToString().Length - 1];
                }
                for (int i = seed.ToString().Length + 1; i < c.Length; i++)
                {
                    int tempI = iA[i - seed.ToString().Length - 1] + seed.ToString().Length + 1;
                    char tempC = c[i];
                    c[i] = c[tempI];
                    c[tempI] = tempC;
                }
                int jseed = Math.Abs(seed);
                for (int i = 0; i < seed.ToString().Length + 1; i++)
                {
                    if (i == seed.ToString().Length)
                        c[i] = '#';
                    else if (seed < 0 && i == 0)
                    {
                        c[i] = '_';
                    }
                    else
                    {
                        c[i] = intToChar(jseed % 10);
                        jseed /= 10;
                    }
                }
                byte[] b = ASCIIEncoding.Default.GetBytes(c);
                for (int i = 0; i < b.Length; i++)
                {
                    if (i < seed.ToString().Length + 1)
                        bitShift = bitShiftS;
                    else
                        bitShift = rand.Next(1, 7);
                    for (int j = 0; j < bitShift; j++)
                    {
                        if (b[i] % 2 == 1)
                        {
                            b[i] = Convert.ToByte((b[i] >> 1) + 64);
                        }
                        else
                        {
                            b[i] = Convert.ToByte(b[i] >> 1);
                        }
                    }
                }
                for (int i = 0; i < b.Length; i++)
                {
                    c[i] = Convert.ToChar(b[i]);
                }
                s = "";
                foreach (char ch in c)
                {
                    s += ch;
                }
                s = s.Substring(0, s.Length / 2) + bitShiftS.ToString() + s.Substring(s.Length / 2, s.Length / 2 + s.Length % 2);
                return s;
            }
            catch
            {
                return T;
            }
        }
        private string Decrypt()
        {
            try
            {
                string s = T;
                int bitShiftS = Convert.ToInt32(s.Substring((s.Length - 1) / 2, 1));
                s = s.Substring(0, (s.Length - 1) / 2) + s.Substring((s.Length + 1) / 2, (s.Length - 1) / 2 + (s.Length - 1) % 2);
                byte[] b = ASCIIEncoding.Default.GetBytes(s);
                for (int i = 0; i < b.Length; i++)
                {
                    for (int j = 0; j < bitShiftS; j++)
                    {
                        if (b[i] > 63)
                        {
                            b[i] = Convert.ToByte((b[i] << 1) - 127);
                        }
                        else
                        {
                            b[i] = Convert.ToByte(b[i] << 1);
                        }
                    }
                }
                s = "";
                for (int i = 0; i < b.Length; i++)
                {
                    s += Convert.ToChar(b[i]);
                }
                string sSeed = s.Substring(0, s.IndexOf('#'));
                string iSeed = "";
                for (int i = 0; i < sSeed.Length; i++)
                {
                    iSeed += charToInt(sSeed[i]);
                }
                sSeed = "";
                if (iSeed[0] == '-')
                {
                    sSeed = "-";
                    iSeed = iSeed.Substring(1);
                }
                for (int i = 0; i < iSeed.Length; i++)
                {
                    sSeed += iSeed[iSeed.Length - i - 1];
                }
                int seed = Convert.ToInt32(sSeed);
                s = s.Substring(s.IndexOf('#') + 1);
                Random rand = new Random(seed);
                rand.Next();
                int[] iA = new int[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    iA[s.Length - 1 - i] = rand.Next(s.Length);
                }
                b = ASCIIEncoding.Default.GetBytes(s);

                for (int i = 0; i < b.Length; i++)
                {
                    int bitShift = rand.Next(1, 7) - bitShiftS;
                    if (bitShift < 0)
                        bitShift += 7;
                    for (int j = 0; j < bitShift; j++)
                    {
                        if (b[i] > 63)
                        {
                            b[i] = Convert.ToByte((b[i] << 1) - 127);
                        }
                        else
                        {
                            b[i] = Convert.ToByte(b[i] << 1);
                        }
                    }
                }
                b = ASCIIEncoding.Convert(ASCIIEncoding.Default, ASCIIEncoding.Default, b);
                s = "";
                for (int i = 0; i < b.Length; i++)
                {
                    s += Convert.ToChar(b[i]);
                }
                char[] c = new char[s.Length];
                for (int i = 0; i < s.Length; i++)
                {
                    c[i] = s[i];
                }
                for (int i = 0; i < c.Length; i++)
                {
                    int tempI = iA[i];
                    char tempC = c[c.Length - 1 - i];
                    c[c.Length - 1 - i] = c[tempI];
                    c[tempI] = tempC;
                }
                s = "";
                foreach (char ch in c)
                {
                    s += ch;
                }
                return s;
            }
            catch
            {
                return T;
            }
        }
        private char intToChar(int i)
        {
            char c;
            switch (i)
            {
                case 0:
                    c = '@';
                    break;
                case 1:
                    c = '|';
                    break;
                case 2:
                    c = 'z';
                    break;
                case 3:
                    c = '{';
                    break;
                case 4:
                    c = 'A';
                    break;
                case 5:
                    c = '$';
                    break;
                case 6:
                    c = 'G';
                    break;
                case 7:
                    c = '+';
                    break;
                case 8:
                    c = '&';
                    break;
                case 9:
                    c = '?';
                    break;
                default:
                    c = ' ';
                    System.Windows.Forms.MessageBox.Show("Error");
                    break;
            }
            return c;
        }
        private char charToInt(char c)
        {
            char i = ' ';
            if (c == '_')
                i = '-';
            else if(c == '@')
                i = '0';
            else if(c == '|')
                i = '1';
            else if(c == 'z')
                i = '2';
            else if(c == '{')
                i = '3';
            else if(c == 'A')
                i = '4';
            else if(c == '$')
                i = '5';
            else if(c == 'G')
                i = '6';
            else if(c == '+')
                i = '7';
            else if(c == '&')
                i = '8';
            else if(c == '?')
                i = '9';
            else
            {
                i = ' ';
                System.Windows.Forms.MessageBox.Show("Error");
            }
            return i;
        }
    }
}
