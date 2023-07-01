using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Polish {



    //TODO: Exceptions.
    //TODO: Null checking.
    /// <summary>
    /// General Maths utility methods
    /// </summary>
    public static class pUtils {

        /// <summary>
        /// Used by crypt random.
        /// </summary>
        private static byte[] z = { 0, 0, 0, 2 };

        /// <summary>
        /// Used by crypt random.
        /// </summary>
        private static RNGCryptoServiceProvider rngCSP { get; set; } = new RNGCryptoServiceProvider();

        /// <summary>
        /// Converts a regular number into a currency string using string formatting flag "C".
        /// </summary>
        /// <remarks>Conceptual lubricant.</remarks>
        /// <param name="dec">regular number representing a cash amount. Decimal component optional.</param>
        /// <returns>A built-in, string formatted representation of a cash amount.</returns>
        public static string ToMoney(decimal dec) => dec.ToString("C");
        //{             
        //    dec+=+.001m;
        //    string pence=(Math.Round((dec-Math.Floor(dec)),2)).ToString();
        //    if(pence!="0.00")
        //        pence+="p"; 
        //    return $@"£{(int)Math.Floor(dec)}.{pence.Substring(2,pence.Length-2)}";
        //}

        /// <summary>
        /// Generates a random int pair from which to create e.g., a fraction.
        /// </summary>
        /// <param name="numWidth">Digit width of numerator.</param>
        /// <param name="denWidth">Digit width of denominator.</param>
        /// <returns>An (int, int) pair.</returns>
        public static (int, int ) Fr(int numWidth, int denWidth) {
            int num = 10*(numWidth-1)+R(1, 9);
            int den = num+ (10*(denWidth-1)-Width(num))+R(1, 9);
            return (num, den);
        }

        //TODO: It's one too high
        /// <summary>
        /// Generates a random decimal number.
        /// </summary>
        /// <param name="x">Whole number, from parameter.</param>
        /// <param name="y">Whole number, to parameter.</param>
        /// <param name="z">Decimal places.</param>
        /// <returns>A decimal number.</returns>
        public static decimal Dr(int x, int y, int z) => R(x, y)+R(1, (int)Math.Pow(10, z))*(decimal)Math.Pow(10, -z);

        //TODO: It's one too high
        /// <summary>
        /// Generates a crypt-random integer.
        /// </summary>
        /// <param name="x">From.</param>
        /// <param name="y">To.</param>
        /// <returns>An integer.</returns>
        public static int R(int x, int y) {
            if (x==y) return x;
            rngCSP.GetBytes(z);
            return (x--+(BitConverter.ToUInt16(z, 0)%(y-x))+1)-1;
        }

        //TODO: review. Learn to use arrays
        /// <summary>
        /// Loads a list of prime number into 'memory'.
        /// </summary>
        public static List<int> primes = Regex.Matches(
                                        File.ReadAllText("1000000.txt"), $@"\d+")
                                        .Cast<Match>().Select(m => m.Value).Select(int.Parse).ToList();

        //TODO: Find out about faster ways.
        /// <summary>
        /// Checks if a number is prime.
        /// </summary>
        /// <param name="n">An integer.</param>
        /// <returns>True if n is prime, otherwise false.</returns>
        public static bool IsPrime(int n) {
            bool rtn = true;
            double num = (double)n;
            if (num>2 && num % 2 < 0.0001) { //TODO: I already have a pita.
                return false;
            }
            if (num<=maxPrime) {
                rtn = primes.Contains(n); //use file
                                          //if(rtn)
                                          //cw($@"IsPriming {num}, returning {rtn}");
            } else {
                for (double i = 3; i<num; i++) {
                    if (num % i<0.0001) { //TODO: I already have a pita.
                        return false;
                    }
                }
            }
            //if(rtn)
            //cw($@"IsPriming {num}, returning {rtn}");
            return rtn;
        }

        /// <summary>
        /// Checks the in-memory primes list for an upper bound.
        /// </summary>
        public static int maxPrime = primes[primes.Count()-1];

        /// <summary>
        /// Returns the first n primes from the in-memory primes list.
        /// </summary>
        /// <param name="n">Upper limit of primes. Needn't be prime. 0 returns the full list.</param>
        /// <returns>An integer list of prime numbers.</returns>
        public static int[] Primes(long n = 0) => n==0 ? primes.ToArray() : primes.Where(i => i<=n).ToArray();

        /// <summary>
        /// Returns the prime factors of a number.
        /// </summary>
        /// <param name="num">Number for which to find Prime Factors.</param>
        /// <returns>An integer list of the Prime Factors of num.</returns>
        /// <exception cref="ArgumentException">Thrown if the in-memory primes list is too small for the input.</exception>
        public static int[] PrimeFactors(int num) {
            List<int> rtn = new List<int>();
            if (num>maxPrime) {
                //cw(""+primes.Count());
                for (int i = 0; num>=primes[i]; i++) {
                    //cw($@"num:{num} i:{i}");
                    if (num-(num/primes[i])*primes[i]==0) {
                        rtn.Add(primes[i]);
                        num/=primes[i];
                        i=-1;
                    }
                    if (i==primes.Count()-1)
                        throw new ArgumentException("Not enough primes in the primes list.");
                }
            }
            for (int i = 0; num>=primes[i]; i++) {
                if (num-(num/primes[i])*primes[i]==0) {
                    rtn.Add(primes[i]);
                    num/=primes[i];
                    i=-1;
                }
            }

            return rtn.ToArray();
        }


        //TODO: Check overall Fraction operator overloading mechanism for beginnery maths/coding.
        /// <summary>
        /// Moves the decimal point to the left or right.
        /// </summary>
        /// <param name="dec">The input number.</param>
        /// <param name="power">How far to move the decimal point. Signed for direction.</param>
        /// <returns>The [dec] input with the decimal point shifted left or right by [power].</returns>
        public static decimal Shrink(decimal dec, int power) => dec/(decimal)Math.Pow(10, power);//base, power, numeric.

        /// <summary>
        /// Returns the number of digits > 0
        /// </summary>
        /// <param name="numeric">Floating point number to measure.</param>
        /// <returns>A count of the number of digits (excludes decimal points).</returns>
        public static int Width(int numeric) => CharacteristicWidth((decimal)numeric);

        //public static int Width(long numeric) => CharacteristicWidth((decimal)numeric);
        //public static int Width(decimal numeric) => CharacteristicWidth(numeric)+MantissaWidth(numeric);

        /// <summary>
        /// Returns the number of digits > 0
        /// </summary>
        /// <remarks>Paired with MantissaWidth</remarks>
        /// <param name="numeric">Floating point number to measure.</param>
        /// <returns>A count of the number of digits (excludes decimal points).</returns>
        public static int CharacteristicWidth(decimal numeric) {
            int shifts = 0;
            if (numeric<0) {
                while (numeric<-1) {
                    numeric/=10;
                    shifts++;
                }
            } else {
                while (numeric>1) {
                    numeric*=0.1m;
                    shifts++;
                }
            }
            return shifts;
        }

        /// <summary>
        /// Returns the number of decimal point digits.
        /// </summary>
        /// <param name="numeric">The floating point number to measure.</param>
        /// <returns>A count of the deimal points (excludes whole numbers).</returns>
        public static int MantissaWidth(decimal numeric) {
            int i = 0;
            decimal original = numeric;
            decimal change = 0.0m;
            decimal diff = 0.0m;
            decimal prevDiff = 0.0m;
            while (!IsInt(numeric)) {
                numeric*=10;
                i++;
                diff=numeric - Math.Floor(numeric);
                change=prevDiff-diff;
                if (change>1) {
                    return i;
                }
                prevDiff=diff;
            }
            return i;
        }


        //public static int MantissaWidth(double numeric) => MantissaWidth((decimal)numeric);

        /// <summary>
        /// Least common multiple.
        /// </summary>
        /// <param name="set">The set of integers to analyse.</param>
        /// <returns>The least common multiple of the input set.</returns>
        public static int lcm(int[] set) { //TODO: Taking a risk on the long here..
            long num = (long)set[0];
            for (int t = 1; t<set.Count(); t++)
                num*=(long)set[t];
            long lcm = num;
            bool bSkip = false;
            // -- loop mults --
            for (long m = num; m>0; m--) {
                bSkip=false;
                foreach (int t in set) {
                    if (!(m-(m/t)*t==0)) {
                        bSkip=true;
                        break;
                    }
                }
                if (!bSkip) lcm=m;
            }
            return (int)lcm;
        }

        /// <summary>
        /// Highest common factor.
        /// </summary>
        /// <param name="set">The set of numbers to analyse.</param>
        /// <returns>The highest common factor from the input set.</returns>
        public static int hcf(int[] set) {
            // -- calculate HCF --
            int hcf = 0;
            bool skip = false;
            //int maximum = max(set);
            int minimum = min(set)+1;
            int h = 0;
            //for (h=1; h<maximum/2; h++) {
            for (h=1; h<minimum; h++) {
                    for (int t = 0; t<set.Count(); t++) {
                    if (set[t]-(set[t]/h)*h!=0) {
                        skip=true;
                        break;
                    }
                }
                if (!skip) hcf = h;
                skip=false;
            }
            return hcf;
        }

        /// <summary>
        /// List-based Math.Max function.
        /// </summary>
        /// <param name="set">The set of long input numbers to analyse.</param>
        /// <returns>The maximum value from the input set.</returns>
        public static long max(long[] set) {
            long max = long.MinValue;
            foreach (long i in set) {
                if (i>max)
                    max=i;
            }
            return max;
        }


        /// <summary>
        /// List-based Math.Max function.
        /// </summary>
        /// <param name="set">The set of int input numbers to analyse.</param>
        /// <returns>The maximum value from the input set.</returns>
        public static int max(int[] set) {
            int max = Int32.MinValue;
            foreach (int i in set) {
                if (i>max)
                    max=i;
            }
            return max;
        }

        /// <summary>
        /// List-based Math.Min function.
        /// </summary>
        /// <param name="set">The set of int input numbers to analyse.</param>
        /// <returns>Returns the minimum-valued number from the input set.</returns>
        public static int min(int[] set) {
            int min = Int32.MaxValue;
            foreach (int i in set) {
                if (i<min)
                    min=i;
            }
            return min;
        }


        //public static long hcf(long[] set){
        //    // -- calculate HCF --
        //    long hcf =0;
        //    bool skip=false;
        //    long maximum = max(set);
        //    long h=0;
        //    for(h=1;h<maximum/2;h++){
        //        for(int t=0; t<set.Count();t++){
        //            if(set[t]-(set[t]/h)*h!=0){
        //                skip=true;  
        //                break;
        //            }
        //        }
        //        if(!skip) hcf = h;
        //        skip=false;
        //    }
        //    return hcf;
        //}
        //public static long min(long[] set){
        //    long min = long.MaxValue;
        //    foreach(long i in set){
        //        if(i<min)
        //            min=i;
        //    }
        //    return min;
        //}
        //public static int min(int[] set){
        //    int min = Int32.MaxValue;
        //    foreach(int i in set){
        //        if(i<min)
        //            min=i;
        //    }
        //    return min;
        //}

        /// <summary>
        /// Calculates significant figures for an input type with a significant figure count.
        /// </summary>
        /// <param name="num">The number to analyse.</param>
        /// <param name="figures">The number of significant figures to return.</param>
        /// <returns>The input number [num] converted to [figures] significant figures.</returns>
        public static string significantFigures(decimal num, int figures) {
            string rslt = string.Empty;
            int shifts = 0;
            if (num<1) {
                while (num<1) {
                    num*=10;
                    shifts--;
                }
                num*=0.1m;
                shifts++;
            } else {
                while (num>1) {
                    num*=0.1m;
                    shifts++;
                }
            }
            num*=(decimal)Math.Pow(10, figures);
            num=(decimal)((int)Math.Round(num));
            num*=(decimal)Math.Pow(10, shifts-figures);

            // -- format --
            rslt=num.ToString();
            if (figures-Regex.Matches(rslt, @"\d").Count>0) {
                if (!rslt.Contains("."))
                    rslt+='.';
                rslt=rslt.PadRight(figures+1, '0');
            }
            return rslt;
        }

        //TODO: One that just returns indices?
        //TODO: Extension method?

        /// <summary>
        /// Returns a subset of a given size with no duplicates.
        /// </summary>
        /// <remarks>As things stand, the source set can contain duplicates.</remarks>
        /// <typeparam name="T">Any generic type as long as it supports new().</typeparam>
        /// <param name="size">Subset size.</param>
        /// <param name="set">Set to choose from.</param>
        /// <returns>A subset of the set with no duplicates.</returns>
        public static T[] Subset<T>(int size, T[] set) where T : new() {
            List<T> rtn = new List<T>();
            List<int> dones = new List<int>();
            int i = 0, j = 0;
            for (i=0; i<size; i++) {
                while (dones.Contains(j))
                    j=R(0, size);
                dones.Add(j);
                rtn.Add(set[j]);
            }
            return rtn.ToArray();
        }

        //Not strictly a maths function. Poor man's sample.
        /// <summary>
        /// Chooses a list entry for you at random.
        /// </summary>
        /// <param name="list">A list of ints to choose from.</param>
        /// <returns>An entry from the list, chosen randomly.</returns>
        public static int Choose(List<int> list) => list[R(0, list.Count()-1)];

        //Not strictly a maths function. Poor man's sample.
        /// <summary>
        /// Chooses a list entry for you at random.
        /// </summary>
        /// <param name="list">A generic list to choose from.</param>
        /// <returns>An entry from the list, chosen randomly.</returns>
        public static T Choose<T>(params T[] list) => list[R(0, list.Count()-1)];

        /// <summary>
        /// Checks if a double is an integer.
        /// </summary>
        /// <param name="num">The number to analyse.</param>
        /// <returns>True if num is an integer.</returns>
        public static bool IsInt(double num) => num - Math.Floor(num) < .0000001d;

        /// <summary>
        /// Checks if a decimal is an integer.
        /// </summary>
        /// <param name="num">The number to analyse.</param>
        /// <returns>True if num is an integer.</returns>
        public static bool IsInt(decimal num) => num - Math.Floor(num) < .0000001m;

        //TODO: Needs review.
        /// <summary>
        /// Part of a prototype hand-rolled number formatting mechanism.
        /// </summary>
        /// <remarks>Mechanism meant to apply an unscientific, dynamic 'Just make it sensible' formatting.</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="num">The number to format.</param>
        /// <returns></returns>
        public static int RoundNice<T>(T num) where T : struct {
            StringBuilder sbRpt = new StringBuilder();
            //if(typeof(T)==typeof(Int32)){
            //    Int32.TryParse(num.ToString(),out int x);
            //}
            string mantissa = string.Empty;
            if ($@"{num}".IndexOf(".")==-1) return 0;
            mantissa = (""+num).Substring((""+num).IndexOf(".")+1);
            if (mantissa.Length==1) return 1;
            if (mantissa[0]==mantissa[1] && mantissa[1]!='0') return 2;
            char curr;
            bool walking = true; ;
            int i = 0;
            curr=mantissa[0];
            sbRpt.Append(curr);
            while (walking && i<10) { //Walk the mantissa to find repeat
                i++;
                curr=mantissa[i];
                if (i==mantissa.Length-1) {
                    walking=false;
                } else if ($@"{sbRpt}"[0]==mantissa[i] && mantissa[i]!='0') { //Might be repeating..
                    i++;
                    curr=mantissa[i];
                    if (curr==mantissa[1]) { //Two repeats, enough?
                        i-=1;
                        walking=false;
                    }
                } else {
                    sbRpt.Append(curr);
                }
            }
            return i+1;
        }

        // TODO: Needs review..
        /// <summary>
        /// Part of a prototype hand-rolled number formatting mechanism.
        /// </summary>
        /// <param name="num">The number to analyse.</param>
        /// <param name="options">An enum representing how to format.</param>
        /// <param name="places">How many decimal places to return.
        /// -1 calls RoundNice() for 'sensible' formatting.</param>
        /// <returns>A string representation of the input number, formatted according to the arguments provided.</returns>
        //public static MyFormat MyDecimal(decimal num, DP options, int places = 1) {
        //    // -- Single value answers only --
        //    MyFormat rtn = new MyFormat();
        //    string strNum = $@"{num}";
        //    decimal answer = 0m;
        //    decimal mantissa = 0m;
        //    bool IsInt = false;
        //    // -- make ints into ints --
        //    if (strNum.EndsWith(".00") ||
        //       strNum.EndsWith(".0")  ||
        //      !strNum.Contains(".")   ||
        //      (DP.specify == (DP.specify & options)) && places==0) {
        //        rtn.answer=$@"{(int)num}";
        //        mantissa=MantissaWidth(answer);
        //        IsInt=true;
        //    } else {
        //        // -- format --
        //        if (DP.none == (DP.none & options)) {
        //            answer=num;
        //        } else if ((DP.nice == (DP.nice & options)) || places==-1) {
        //            answer=Math.Round(num, RoundNice(num));
        //        } else if (DP.specify == (DP.specify & options)) {
        //            answer=Math.Round(num, places);
        //        }
        //        mantissa=MantissaWidth(answer);
        //        try {
        //            rtn.answer=Regex.Replace($@"{answer}", @"(0)+$", string.Empty);
        //        } catch (Exception e) {
        //            System.Windows.Forms.MessageBox.Show($@"Error Regexing {answer} in QADecimal: {e.Message}");
        //        }
        //    }
        //    if (DP.prompt == (DP.prompt & options) && !IsInt && mantissa>0)
        //        rtn.dp = $@"({mantissa} dp)";
        //    return rtn;
        //}
        
        // NOT MATHS - MORE LOGIC
        /// <summary>
        /// Keep running a function for as long as it's returning the specified, undesired result.
        /// </summary>
        /// <typeparam name="T">Any generic type as long as it's a struct.</typeparam>
        /// <param name="notThis">The result to avoid.</param>
        /// <param name="f">The function to run.</param>
        /// <returns>The first desirable result, defined as not the undersirable one (notThis).</returns>
        public static T Not<T>(T notThis, Func<T> f) where T : struct {
            T t;
            for (t=notThis; t.Equals(notThis); t=f()) ;
            return t;
        }

        /// <summary>
        /// Runs a function until the result exceeds or meets the provided threshold desired.
        /// </summary>
        /// <typeparam name="T">Any generic type as long as it implements IComparable.</typeparam>
        /// <param name="greaterThanThis">The threshold value to exceed or meet before returning a result.</param>
        /// <param name="f">The function to run.</param>
        /// <returns>The first result that exceeds or meets the threshold provided.</returns>
        public static T Gt<T>(T greaterThanThis, Func<T> f) where T : IComparable {
            T t;
            for (t=greaterThanThis; t.CompareTo(greaterThanThis)<=0; t=f()) ;
            return t;
        }

        /// <summary>
        /// Runs a function until the result falls below or meets the desired threshold provided.
        /// </summary>
        /// <typeparam name="T">Any generic type as long as it implements IComparable.</typeparam>
        /// <param name="lessThanThis">The threshold to fall below or meet.</param>
        /// <param name="f">The function to run.</param>
        /// <returns>The first result that falls below or meets the threshold provided.</returns>
        public static T Lt<T>(T lessThanThis, Func<T> f) where T : IComparable {
            T t;
            for (t=lessThanThis; t.CompareTo(lessThanThis)<=0; t=f()) ;
            return t;
        }

        /// <summary>
        /// Returns a guaranteed square number whose root is between x and y.
        /// </summary>
        /// <param name="x">Minimum root.</param>
        /// <param name="y">Maximum root.</param>
        /// <returns>A number between x and y, squared.</returns>
        public static int Sq(int x, int y) => (int)Math.Pow(R(x, y), 2);

        /// <summary>
        /// Returns a number from between x and y that is guaranteed not to be a square number.
        /// </summary>
        /// <param name="x">From parameter.</param>
        /// <param name="y">To parameter.</param>
        /// <returns>A number from between x and y guaranteed not to be a square number.</returns>
        public static int NotSq(int x, int y) {
            ;
            int rtn = 4;
            while (IsInt(Math.Sqrt(rtn))) rtn=R(x, y);
            return rtn;
        }

        /// <summary>
        /// Checks if a number is square.
        /// </summary>
        /// <param name="num">The number to analyse.</param>
        /// <returns>True if the number is square.</returns>
        public static bool IsSquare(int num) => IsInt(Math.Sqrt(num));
    }

    // -- Early expression evaluators --
    public class BasicTermOpEvaluator {
        //Passive ----------------------------------
        public string term { get; set; }
        public char op { get; set; }

        //Active -----------------------------------
        public List<BasicTermOpEvaluator> termOps { get; set; } = new List<BasicTermOpEvaluator>();
        public void ParseToList() {
            BasicTermOpEvaluator toc;
            int braces = 0;
            for (int i = 0; i<term.Length; i++) {
                //Braces
                if (term[i]=='(') {
                    toc = new BasicTermOpEvaluator();
                    int b = i;
                    braces = 1;
                    do {
                        i++;
                        if (term[i] == '(')
                            braces++;
                        if (term[i] == ')')
                            braces--;
                    } while (braces > 0);
                    toc.term = term.Substring(b, (i+1)-b);
                    //op
                    if (i<term.Length) {
                        while (!$@"+-*/".Contains(term[i])) {
                            i++;
                            if (i==term.Length) //@eos
                                break;
                        }
                        if (i!=term.Length)
                            toc.op=term[i];
                    }
                    termOps.Add(toc);

                    //Digits
                } else if (Char.IsDigit(term[i])) {
                    toc = new BasicTermOpEvaluator();
                    int d = i;
                    while (Char.IsDigit(term[i])) {
                        i++;
                        if (i==term.Length) //@eos
                            break;
                    }
                    toc.term = term.Substring(d, i-d);
                    //op
                    if (i<term.Length) { //must be an op
                        while (!$@"+-*/".Contains(term[i])) //trimmed spaces
                            i++;
                        toc.op=term[i];
                    }
                    termOps.Add(toc);
                }
            }
        }
        public double Evaluate() {
            double rtn, t1, t2 = 0.0;
            //  -- brackets pass --
            for (int i = 0; i<termOps.Count; i++) {
                if (termOps[i].term[0] == '(') {
                    //Remove brackets wrap
                    termOps[i].term=termOps[i].term.Substring(1, termOps[i].term.Length-2);
                    termOps[i].ParseToList();
                    termOps[i].term = termOps[i].Evaluate().ToString();
                }
            }

            while (termOps.Count>1) {
                //  -- div / mult pass --
                for (int i = 0; i<termOps.Count && termOps.Count-i>1; i++) {
                    Double.TryParse(termOps[i].term, out t1);
                    Double.TryParse(termOps[i+1].term, out t2);
                    if (termOps[i].op=='/') {
                        termOps[i+1].term = (t1 / t2).ToString();
                        termOps.RemoveAt(i);
                        i--;
                    } else if (termOps[i].op=='*') {
                        termOps[i+1].term = (t1 * t2).ToString();
                        termOps.RemoveAt(i);
                        i--;
                    }
                }
                //  -- minus / plus pass --
                for (int i = 0; i<termOps.Count && termOps.Count-i>1; i++) {
                    Double.TryParse(termOps[i].term, out t1);
                    Double.TryParse(termOps[i+1].term, out t2);
                    if (termOps[i].op=='-') {
                        termOps[i+1].term = (t1 - t2).ToString();
                        termOps.RemoveAt(i);
                        i--;
                    } else if (termOps[i].op=='+') {
                        termOps[i+1].term = (t1 + t2).ToString();
                        termOps.RemoveAt(i);
                        i--;
                    }
                }
            }
            Double.TryParse(termOps[0].term, out rtn);
            return rtn;
        }
    }
    public class FractionTermOpEvaluator {
        public string term { get; set; }
        public char op { get; set; }
        public List<FractionTermOpEvaluator> termOps { get; set; } = new List<FractionTermOpEvaluator>();

        public bool showParse = false;
        public bool showEval = false;
        public bool show = false;
        public void showMe(bool show, string stuff) {
            if (show)
                Console.WriteLine(stuff);
        }
        public int i;
        public void SkipWhite(int id = 0) {
            if (id!=0)
                showMe(show, $@"SkipeWhite {id}");
            while (term[i]==' ')
                i++;
        }
        public bool eosTest(int id = 0) {
            if (id!=0)
                showMe(show, $@"eosTest {id}");
            return i==term.Length;
        }
        public object RejectIf(bool invalid, int id = 0) {
            if (id!=0)
                showMe(show, $@"RejectIf {id}");
            return (invalid) ? throw new ArgumentException("invalid input string") : "";
        }
        public void Reject() => RejectIf(true);

        public void ParseToList() {
            FractionTermOpEvaluator ftoe;
            string sign = string.Empty;
            int braces = 0;
            term=term.Trim();
            RejectIf(!"-+(".Contains(term[0])&& !char.IsDigit(term[0]) || term.Length==0, 1);

            showMe(showParse, term);
            for (i=0; i<term.Length; i++) {
                SkipWhite(2);
                // -- sign ----------------------------------------------
                if (term[i]=='-') { //not ops because ops are taken care of in the branches
                    sign="-";
                    i++;
                    SkipWhite(3);
                    RejectIf("*/".Contains(term[i]), 4); // -- sign, op --
                }
                if (term[i]=='+') {
                    i++;
                    SkipWhite(5);
                    RejectIf("*/".Contains(term[i]), 6); // -- sign, op --
                }
                // -- braces ----------------------------------------------
                if (term[i]=='(') {
                    ftoe = new FractionTermOpEvaluator();
                    int b = i;
                    braces = 1;
                    do {
                        i++;
                        if (eosTest()) {
                            Reject();
                        } else if (term[i] == '(') {
                            braces++;
                        } else if (term[i] == ')') {
                            braces--;
                        }
                    } while (braces > 0);
                    ftoe.term = sign+term.Substring(b, (i+1)-b);
                    // -- op --
                    if (i<term.Length) {
                        while (!$@"+-*/".Contains(term[i])) {
                            i++;
                            if (eosTest(8)) { //@eos
                                break;
                            } else if (char.IsDigit(term[i])) {
                                Reject();
                            }
                        }
                        if (!eosTest(9))
                            ftoe.op=term[i];
                    }
                    termOps.Add(ftoe);
                    sign=string.Empty;

                } else if (Char.IsDigit(term[i]) && !eosTest(10)) {

                    // -- digits ----------------------------------------------
                    ftoe = new FractionTermOpEvaluator();
                    int d = i;
                    while (Char.IsDigit(term[i])) {
                        i++;
                        if (eosTest(11)) //@eos, integer
                            break;
                    }
                    if (eosTest(12)) { //integer $
                        ftoe.term = sign+term.Substring(d, i-d);
                    } else {
                        // -- int or mixed --
                        if (term[i]==' ') {
                            SkipWhite(13);
                            RejectIf(term[i]=='(', 14);
                            if (char.IsDigit(term[i])) { //       -- mixed --

                                // -- fractional part --
                                // -- numerator --
                                while (Char.IsDigit(term[i])) {
                                    i++;
                                    RejectIf(eosTest(15));
                                }

                                RejectIf(term[i]!='/', 16);
                                i++; // -- slash --
                                RejectIf(!Char.IsDigit(term[i]), 17);

                                // -- denominator --
                                while (Char.IsDigit(term[i])) {
                                    i++;
                                    if (eosTest(18))
                                        break;
                                }
                                ftoe.term = sign+term.Substring(d, i-d);
                                // -- op --
                                if (!eosTest(19)) {
                                    while (!eosTest() && !$@"+-*/".Contains(term[i]))
                                        i++;
                                    if (!eosTest(20))
                                        ftoe.op=term[i];
                                }

                            } else { // /*-+   sign or op         -- integer -- 
                                // -- integer --
                                i--; //????
                                ftoe.term = sign+term.Substring(d, i-d);
                                // -- op --
                                i++;
                                ftoe.op=term[i];

                            }
                        } else if (term[i]=='/') { // -- int or fraction --
                            i++;
                            if (term[i]==' ') {          //       -- integer --
                                // -- integer --
                                i--;
                                ftoe.term = sign+term.Substring(d, i-d);//??? back on the slash?
                                // -- op --
                                i++;//here
                                ftoe.op=term[i];
                                RejectIf(eosTest(21));

                            } else if (char.IsDigit(term[i])) { // -- fraction --
                                // -- denominator --
                                while (Char.IsDigit(term[i])) {
                                    i++;
                                    if (eosTest(22))
                                        break;
                                }
                                ftoe.term = sign+term.Substring(d, i-d);
                                // -- op --
                                if (!eosTest(23)) {
                                    while (!eosTest(24)&& !$@"+-*/".Contains(term[i]))
                                        i++;
                                    if (!eosTest(25))
                                        ftoe.op=term[i];
                                }

                            } else if ("-+".Contains(term[i])) { // -- integer --
                                // -- integer --
                                i--;
                                ftoe.term = sign+term.Substring(d, i-d);
                                // -- op --
                                ftoe.op=term[i];

                            } else if ("*/".Contains(term[i])) { // -- two ops --
                                Reject();
                            }

                        } else if ("*-+".Contains(term[i])) { // -- integer
                            ftoe.term = sign+term.Substring(d, i-d);
                            // -- op --
                            //i++;
                            ftoe.op=term[i];
                            showMe(showParse, $@"here' {ftoe.term}, {ftoe.op}");
                        }
                    }
                    termOps.Add(ftoe);
                    sign=string.Empty;
                }
            }
        }
        public decimal Evaluate() {

            Fraction t1 = new Fraction();
            Fraction t2 = new Fraction();
            int sign = 1;
            i=0;
            //  -- brackets pass --
            for (int t = 0; t<termOps.Count; t++) {
                showMe(showEval, "(brackets) Evaluate: " + termOps[t].term + ", " + termOps[t].op);
                // -- sign --
                if ("-".Contains(termOps[t].term[0])) {
                    sign=-1;
                    i++;
                }
                if (termOps[t].term[i] == '(') {
                    //Remove brackets wrap
                    termOps[t].term=termOps[t].term.Substring(1+i, termOps[t].term.Length-(2+i)).Trim();
                    termOps[t].ParseToList();
                    termOps[t].term =(sign*termOps[t].Evaluate()).ToString();
                }
                i=0;
            }
            // -- ops pass --
            while (termOps.Count>1) {
                //  -- div / mult pass --
                for (i=0; i<termOps.Count && termOps.Count-i>1; i++) {
                    if (termOps[i].op=='/') {
                        showMe(showEval, "/ What's here: "+termOps[i].term + "  " + termOps[i].op);
                        showMe(showEval, "/ What's also here: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        t1=new Fraction(termOps[i].term);
                        t2=new Fraction(termOps[i+1].term);
                        termOps[i+1].term = (t1 / t2).ToString();
                        showMe(showEval, "/ t1= " + t1 + ", t2= " + t2);
                        showMe(showEval, "/  Now: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        termOps.RemoveAt(i);
                        i--;
                    } else if (termOps[i].op=='*') {
                        showMe(showEval, "* What's here: "+termOps[i].term + "  " + termOps[i].op);
                        showMe(showEval, "* What's also here: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        t1=new Fraction(termOps[i].term);
                        t2=new Fraction(termOps[i+1].term);
                        termOps[i+1].term = (t1 * t2).ToString();
                        showMe(showEval, "* t1= " + t1 + ", t2= " + t2);
                        showMe(showEval, "*  Now: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        termOps.RemoveAt(i);
                        i--;
                    }
                }
                //  -- minus / plus pass --
                for (i=0; i<termOps.Count && termOps.Count-i>1; i++) {
                    if (termOps[i].op=='-') {
                        showMe(showEval, "- What's here: "+termOps[i].term + "  " + termOps[i].op);
                        showMe(showEval, "- What's also here: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        t1=new Fraction(termOps[i].term);
                        t2=new Fraction(termOps[i+1].term);
                        termOps[i+1].term = (t1 - t2).ToString();
                        showMe(showEval, "- t1= " + t1 + ", t2= " + t2);
                        showMe(showEval, "-  Now: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        termOps.RemoveAt(i);
                        i--;
                    } else if (termOps[i].op=='+') {
                        showMe(showEval, "+ What's here: "+termOps[i].term + "  " + termOps[i].op);
                        showMe(showEval, "+ What's also here: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        t1=new Fraction(termOps[i].term);
                        t2=new Fraction(termOps[i+1].term);
                        termOps[i+1].term = (t1 + t2).ToString();
                        showMe(showEval, "+ t1= " + t1 + ", t2= " + t2);
                        showMe(showEval, "+  Now: "+termOps[i+1].term + "  " + termOps[i+1].op);
                        termOps.RemoveAt(i);
                        i--;
                    }
                }
            }
            showMe(showEval, "returning new Fraction( " + termOps[0].term + " )");
            return new Fraction(termOps[0].term).ToDecimal();
        }
    }
    public class Algebraic {
        #region// -- Fields -- //
        #endregion
        #region// -- Constructors -- //
        #endregion
        #region// -- Utilities -- //
        #endregion
        #region// -- Output -- //
        #endregion
        #region// -- Operators -- //
        public static Algebraic operator +(Algebraic a, Algebraic b) => Add(a, b);
        public static Algebraic operator -(Algebraic a, Algebraic b) => Subtract(a, b);
        public static Algebraic operator /(Algebraic a, Algebraic b) => Divide(a, b);
        public static Algebraic operator *(Algebraic a, Algebraic b) => Multiply(a, b);
        public static Algebraic Add(Algebraic a, Algebraic b) { return new Algebraic(); }
        public static Algebraic Subtract(Algebraic a, Algebraic b) { return new Algebraic(); }
        public static Algebraic Divide(Algebraic a, Algebraic b) { return new Algebraic(); }
        public static Algebraic Multiply(Algebraic a, Algebraic b) { return new Algebraic(); }
        #endregion
    }
    public struct flingy {
        public string doobrey { get; set; }
        public string wotsit { get; set; }
        public string thingy { get; set; }
        public flingy(string t, string m, string u) {
            doobrey=t;
            wotsit=m;
            thingy=u;
        }
    }
    public struct flangy {
        public string doobreys { get; set; }
        public string wotsits { get; set; }
        public string thingies { get; set; }
        public flangy(string t, string m, string u) {
            doobreys=t;
            wotsits=m;
            thingies=u;
        }
    }
    public struct flongy {
        public string bish { get; set; }
        public string bash { get; set; }
        public string bosh { get; set; }
        public int shrubbery { get; set; }
        public flingy eki { get; set; }
        public flangy ftang { get; set; }
        public flongy(string m, string mu, string pv, int mt, flingy s, flangy p) {
            bish = m;
            bash = mu;
            bosh = pv;
            shrubbery=mt;
            eki=s;
            ftang=p;
        }
    }
    public struct thangy {
        public string slarty { get; set; }
        public string bartfast { get; set; }
        public thangy(string _slarty, string _bartfast) {
            slarty=_slarty;
            bartfast=_bartfast;
        }
    }
    //public struct MyFormat {
    //    public string dp { get; set; }
    //    public string answer { get; set; }
    //}
    //[Flags]
    //public enum DP : int {
    //    nice = 1,
    //    specify = 2,
    //    none = 4,
    //    prompt = 8
    //}
}
