using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Euler
{
    class Program
    {
        static long factorial(int n)
        {
            long retVal = 1;
            for (int i = n; i > 1; i--)
            {
                retVal *= i;
                if (retVal < 0)
                    Console.WriteLine("Overflow in factorial {0}", n);
            }
            return retVal;
        }
        static long PartialFact(int n, int div)
        {
            long retVal = 1;
            for (int i = n; i > div; i--)
            {
                retVal *= i;
                if (retVal < 0)
                    Console.WriteLine("Overflow in factorial {0} {1}", n, div);
            }
            return retVal;
        }
        static long NChooseR(int n, int r)
        {
            if (n < r)
                return 0;
            if (n-r > r)
            {
                return PartialFact(n, n - r) / factorial(r);
            }
            else
            {
                return PartialFact(n, r) / factorial(n - r);
            }
            
        }

        static List<List<int>> IntegerPart(int n)
        {
            List<List<int>> ret = new List<List<int>>();
            int[] v = new int[n + 1];
            v[1] = n;
            int k = 1;
            while (k!=0)
            {
                int x = v[k - 1] + 1;
                int y = v[k] - 1;
                --k;

                while (x<=y)
                {
                    v[k] = x;
                    y -= x;
                    ++k;
                }
                v[k] = x + y;
                if (k==2)
                {
                    List<int> part = new List<int>();
                    for (int a = 0; a <=k; a++)
                        part.Add(v[a]);
                    ret.Add(part);
                }
            }
            return ret;           
        }

        static long GetMultOfPart(List<int> part)
        {
            long mult = 6;
            if (part[0] == part[1])
            {
                if (part[1] == part[2])
                    mult = 1;
                else
                    mult = 3;
            }
            else if (part[1] == part[2])
            {
                mult = 3;
            }
            else if (part[0] == part[2])
                mult = 3;

            return mult;
        }

        static long GetCombo(int n, List<List<List<int>>> AllParts)
        {
            long ret = 0;
            for(int i=3;i<n;i++)
            {
                long zeroStart = NChooseR(n - 1, i);
                long regStart = NChooseR(n - 1, i - 1);
                foreach(var part in AllParts[i-3])
                {
                    long combos = factorial(i);
                    foreach (var num in part)
                        combos /= factorial(num);
                    long mult = GetMultOfPart(part);
                    long add = 6;
                    long extra = n - i - 1;
                    while (extra> 0)
                    {
                        add *= 7;
                        extra--;
                    }
                    ret += add * combos * zeroStart * mult;

                    add = 7;
                    extra = n - i - 1;
                    while (extra >0)
                    {
                        add *= 7;
                        extra--;
                    }
                    ret += add * combos * regStart * mult;
                }
            }

            foreach (var part in AllParts[n - 3])
            {
                long finalCombos = factorial(n);
                foreach (var num in part)
                    finalCombos /= factorial(num);
              

                ret += finalCombos * GetMultOfPart(part);

            }

            return ret;

        }
        
        static void Main(string[] args)
        {
            int pow = 16;
            Stopwatch watch = new Stopwatch();
            List<List<List<int>>> AllParts = new List<List<List<int>>>();
            for (int i=3;i<=16;i++)
            {
                var part = IntegerPart(i);
                AllParts.Add(part);
            }
            
            watch.Start();
          
            long sum = 0;
            for (int i = 3; i <= pow; i++)
                sum += GetCombo(i, AllParts);
            watch.Stop();
            Console.WriteLine("Part two took {0}", watch.Elapsed);
            Console.WriteLine($" {sum}");
          
            Console.ReadKey();
        }
    }
}
