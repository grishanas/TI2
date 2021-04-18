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
             int q, p;
               do
               {
                   Console.WriteLine("введите первое простое число");
                   var str = Console.ReadLine();
                   q = Convert.ToInt32(str);
                   Console.WriteLine("введите второе простое число");
                   str = Console.ReadLine();
                   p = Convert.ToInt32(str);
               } while ((!Is_simple(q)) && (!Is_simple(p)));
             Console.WriteLine("введите сообщение");
             string stri = Console.ReadLine();
             stri=stri.ToUpper();
             int n = p * q;
             int Euler = (p - 1) * (q - 1);
             int d = Calculate_D(Euler);
             int e = Calculate_E(Euler, d);

            Console.WriteLine($"открытый ключ e={e},n={n}");
            Console.WriteLine($"закрытый ключ d={d},n={n}");
             string tmp=RSA_Encode(stri, n, e);
             Console.WriteLine(tmp);
             Console.WriteLine(RSA_Decode(tmp,d,n));


             
            Console.ReadKey();
        }

        static string RSA_Encode(string input_str,int n,int e)
        {
            StringBuilder result= new StringBuilder();


            for(int i=0;i<input_str.Length;i++)
            {
                int index = Array.IndexOf(alpha, input_str[i]);
                BigInteger tmp = FastPower(index, e);
                tmp = tmp % n;
                result.Append(tmp.ToString()+'.');

            }

            return result.ToString();
            
        }

        static int Calculate_E(int mod,int d)
        {
            int e = 1;

            while (true)
            {
                if ((e * d) % mod == 1)
                    break;
                else
                    e++;
            }

            return e;
        }


        static string RSA_Decode(string input_str,int d, int n)
        
        {
            string[] InpStr = input_str.Split(new char[] { '.' });
            StringBuilder Result = new StringBuilder();
            

            for(int j=0; j<InpStr.Length-1;j++)
            {
                string str = InpStr[j];
                int i = Convert.ToInt32(str);
                var bi = new BigInteger();
                bi = FastPower(i, d);
                bi = bi % n;
                Result.Append(alpha[(int)(bi)]);
                
            }


                        

            return Result.ToString();
        }      

        static int Calculate_D(int Euler)
        {
            int d = Euler - 1;

            for (long i = 2; i <= Euler; i++)
                if ((Euler % i == 0) && (d % i == 0)) 
                {
                    d--;
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
