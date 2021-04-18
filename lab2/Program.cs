using System;
using System.Text;
using System.Numerics;

namespace lab2
{
    class Program
    {
        static char[] alpha = new char[]
        {
            ' ','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','P','Q','R','S','T','U','V','W','X','Y','Z'
        };
        static void Main(string[] args)
        {
            Console.WriteLine("0- генерация ключей");
            Console.WriteLine("1-зашифровать");
            Console.WriteLine("2-Дешифровать");
            string key = Console.ReadLine();
            while (key != "-1")
            {
                switch (key)
                {
                    case "0":
                        {
                            Console.WriteLine("Введите p и q");


                            var p = BigInteger.Parse(Console.ReadLine());
                            var q = BigInteger.Parse(Console.ReadLine());


                            BigInteger n = p * q;
                            BigInteger m = (p - 1) * (q - 1);



                            BigInteger d = Calculate_D(m); ;
                            BigInteger e = Calculate_E(d,m);

                            Console.WriteLine($"The public key is  ({d}, {n})");
                            Console.WriteLine($"The private key is ({e}, {n})");
                            break;
                        }
                    case "1":
                        {
                            Console.WriteLine("введите d");
                            int d = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("введите n");
                            int n = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите сообщение для шифрации");
                            string tmp = Console.ReadLine();
                            Console.WriteLine(RSA_Encode(tmp, n, d));
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("введите e");
                            int e = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("введите n");
                            int n = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Введите зашифрованное сообщение");
                            string tmp = Console.ReadLine();
                            Console.WriteLine(RSA_Decode(tmp, n, e));
                            break;
                        }
                }
                key = Console.ReadLine();
            }         
            Console.ReadKey();
        }

        static string RSA_Encode(string input_str,int n,int d)
        {
            StringBuilder result= new StringBuilder();


            for(int i=0;i<input_str.Length;i++)
            {
                int index = Array.IndexOf(alpha, input_str[i]);
                BigInteger tmp = fastexp((BigInteger)index, d, n);
                result.Append(tmp.ToString()+'.');

            }

            return result.ToString();
            
        }


        private static BigInteger fastexp(BigInteger a, BigInteger z, BigInteger m)
        {
            BigInteger x = 1;

            for (; !z.IsZero; --z)
            {
                while (z.IsEven)
                {
                    z /= 2;

                    a = (a * a) % m;
                }

                x = (x * a) % m;
            }

            return x;
        }

        static string RSA_Decode(string input_str,int n, int e)
        
        {
            string[] InpStr = input_str.Split(new char[] { '.' });
            StringBuilder Result = new StringBuilder();

            for (int j=0; j<InpStr.Length-1;j++)
            {
                string str = InpStr[j];
                BigInteger i = BigInteger.Parse(str);
                BigInteger bi=fastexp(i, e, n);
                Result.Append(alpha[(int)(bi)]);
                
            }
            return Result.ToString();
        }

        static BigInteger Calculate_E(BigInteger d,BigInteger m)
        {
            
            BigInteger e = 3;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
        static BigInteger Calculate_D(BigInteger m)
        {
            BigInteger d = 3;

            for (BigInteger i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) 
                {
                    d++;
                    i = 1;
                }

            return d;
        }

        static bool Is_simple(int a)
        {
            if (a < 2)
                return false;
            if (a == 2)
                return true;
            for(int i=2;i<(int)(a/2);i++)
            {
                if (a % i == 0)
                    return false;
            }
            return true;
        }

        static BigInteger FastPower(int number,int power)
        {
            BigInteger result = 1,shift=0x8000;
            for(ulong i =16;i>0; i--)
            {
                result = result * result;
                if ( (shift&(ulong)power) >0)
                {
                    result = result * (ulong)number;

                }
                shift=shift >> 1;
                
            }


            return result;

        }
    }
}
