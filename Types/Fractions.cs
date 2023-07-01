using System;
using System.Text.RegularExpressions;
using MathUtils;

namespace Polish {
    public struct Fraction {
        //TODO: make operators FractionLite's

        #region // -- Fields --
        public long numerator { get; set; }
        public long denominator { get; set; }
        #endregion

        #region// -- utilities --
        public long whole => numerator*denominator==0 ? 0 : numerator/denominator;  // ..relying on long rounding..
        public bool integer => denominator==1;                                  // ..relying on reduced..
        public string fractional {//supposedly never called if an integer...?
            get {
                if (numerator*denominator==0 || denominator==1) {
                    return "";
                } else {
                    return $@"{(numerator-(numerator/denominator)*denominator)*(whole<0 ? -1 : 1)}/{denominator}";
                    // (Drop sign if whole signed)
                }
            }
        }
        //TODO: Cater for mixed input
        private void SignCorrect() {
            //if(denominator<0){
            //    denominator*=-1;
            //    numerator*=-1;
            //}
        }
        //TODO: This could be made more flexible and be given better guards
        //TODO: reject >1 implied signs for mixed's
        #endregion

        #region// -- constructors --
        public Fraction(string f, bool r = true) {
            long w = 0, n = 0, d = 0;
            f = Regex.Replace(f, @"\+", @"");
            //if(!Regex.Match(f,@"^(?>![ |0])$|^\-?\d+$|^(?<int>\-?\d+ )?(?(int)|\-?)\d+\/(?(int)|\-?)\d+").Success )
            //throw new ArgumentException($@"Invalid string in fraction constructor: {f}");
            numerator=0;
            denominator=0;

            //TODO: Test for duff input
            //if(!Regex.Match(f,@"^[\-\+]?\d+ [\-\+]?\d+\/[\-\+]?\d+$|^[\-\+]?\d+\/[\-\+]?\d+|^[\-\+]?\d+$").Success )
            //if(!Regex.Match(f,@"^([\-\+]?\d+[ |\/]){0,2}[\-\+]?\d+$").Success )
            if (!Regex.Match(f, @"^([\-\+]?\d+[ |\/]?){1,3}").Success)
                throw new ArgumentException($@"Invalid string in fraction constructor: {f}");
            // Console.WriteLine("bf is " + f);
            //  x  y/ z
            //  x  y/-z
            //  x -y/ z
            // -x  y/ z
            //  x -y/-z
            // -x -y/ z
            // -x  y/-z
            // -x -y/-z
            //     y/ z
            //     y/-z
            //    -y/ z
            //    -y/-z
            //  x
            // -x
            //  x.y
            // -x.y

            string[] inComing = Regex.Split(f, $@" |/");
            int length = inComing.Length;

            if (length==1) { //int
                if (Regex.Match(f, @"\.").Success) {
                    decimal dec = 0.0m;
                    decimal.TryParse(inComing[0], out dec);
                    Fraction tmp = new Fraction(dec, false);
                    numerator = tmp.numerator;
                    denominator=tmp.denominator;
                } else {
                    long.TryParse(inComing[0], out w);
                    numerator = w;
                    denominator=1;
                }
            }
            if (length==2) { //proper
                long.TryParse(inComing[0], out n);
                long.TryParse(inComing[1], out d);
                numerator = n;
                denominator=d;
            }
            if (length==3) { //mixed
                long.TryParse(inComing[0], out w);
                long.TryParse(inComing[1], out n);
                long.TryParse(inComing[2], out d);
                numerator=(n*(f[0]=='-' ? -1 : 1))+w*d;
                denominator=d;
            }
            if (numerator==0) {
                denominator=0;
                return;
            }
            SignCorrect();
            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
        }
        public Fraction(int n, int d) {
            numerator = n;
            denominator=d;
        }
        public Fraction(long n, long d, bool r = true) {
            numerator=n;
            if (numerator==0) {
                denominator=0;
                return;
            }
            denominator=d;
            SignCorrect();
            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
        }
        public Fraction(long w, long n, long d, bool r = true) {
            if (n<0 || d<0)
                throw new ArgumentException($@"Invalid parameters in fraction constructor: {w},{n},{d}");
            numerator=(n*(w<0 ? -1 : 1))+w*d;
            denominator=d;
            if (numerator==0) {
                denominator=0;
                return;
            }
            SignCorrect();
            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
        }
        public Fraction(Fraction f, bool r = true) {
            numerator=f.numerator;
            denominator=f.denominator;
            if (numerator==0) {
                denominator=0;
                return;
            }
            SignCorrect();
            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
        }
        public Fraction(long i, Fraction f, bool r = true) {
            numerator=f.numerator+i*f.denominator;
            denominator= f.denominator;
            if (numerator==0) {
                denominator=0;
                return;
            }
            SignCorrect();
            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
        }
        public Fraction(long i, bool r = true) {
            numerator=i;
            denominator=1;
            if (numerator==0) {
                denominator=0;
                return;
            }
            SignCorrect();
            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
        }
        public Fraction(decimal dec, bool r = true) {
            long characteristic, mantissa;
            int sign = dec<0 ? -1 : 1;

            decimal decStrDec;
            string strDec = dec.ToString().Substring(0, Math.Min(long.MaxValue.ToString().Length+1, dec.ToString().Length)); //over/underflow
            decimal.TryParse(strDec, out decStrDec);

            long.TryParse(Regex.Match(strDec, @"^[\-\+]?\d+").Value, out characteristic);
            long.TryParse(Regex.Match(strDec, @"[\-\+]?\d+$").Value, out mantissa);

            int dropCount = Regex.Match(decStrDec.ToString(), @"\d+$").Value.Length-mantissa.ToString().Length;//b2u

            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //.Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   .Value.Length   
            //(I am v. close to swearing)

            denominator=(long)Math.Pow(10, mantissa.ToString().Length+dropCount);
            numerator=characteristic*denominator+((characteristic<0 ? -1 : 1)*mantissa); //sign
            numerator*=sign;

            //Console.WriteLine($@"dec:            {dec}");
            //Console.WriteLine($@"strDec:         {strDec}");
            //Console.WriteLine($@"decStrDec:      {decStrDec}");
            //Console.WriteLine($@"characteristic: {characteristic}");
            //Console.WriteLine($@"mantissa:       {mantissa}");
            //Console.WriteLine($@"dropCount:      {dropCount}");
            //Console.WriteLine($@"numerator:      {numerator}");
            //Console.WriteLine($@"denominator:    {denominator}");
            //Console.WriteLine($@"");
            //Console.WriteLine($@"");
            //Console.WriteLine($@"");

            if (r) {
                FractionLite red = FractionUtils.Reduce(numerator, denominator);
                numerator=red.numerator;
                denominator=red.denominator;
            }
            SignCorrect();
            if (numerator==0) {
                denominator=0;
                return;
            }
        }
        #endregion

        #region // -- output --
        public string ToMixed() {
            if (numerator*denominator==0)
                return "0";
            if (Math.Abs(numerator)<Math.Abs(denominator)) { //proper
                return $@"{numerator}{(numerator==0 ? "" : "/"+denominator)}"; //"0" for zero
            } else if (numerator-(numerator/denominator)*denominator==0) { //int
                return $@"{whole}";
            } else { //mixed
                return $@"{whole} {fractional}";
            }
        }
        //public string SmallTopHeavy()=>numerator*denominator==0?"0":$@"{FractionUtils.SmallNum(numerator)}⁄{FractionUtils.SmallDen(denominator)}";
        public string TopHeavy() => numerator*denominator==0 ? "0" : $@"{numerator}/{denominator}";
        public decimal ToDecimal() => numerator*denominator==0 ? 0m : (decimal)numerator/(decimal)denominator;
        public override string ToString() => ToMixed();
        #endregion

        #region // -- operators --
        public static Fraction operator +(Fraction a, Fraction b) => Add(a, b);
        public static Fraction operator +(Fraction a, int b) => Add(a, new Fraction(b, false));
        public static Fraction operator +(int a, Fraction b) => Add(new Fraction(a, false), b);
        public static Fraction operator +(Fraction a, decimal b) => Add(a, new Fraction(b, false));
        public static Fraction operator +(decimal a, Fraction b) => Add(new Fraction(a, false), b);

        public static Fraction operator -(Fraction a, Fraction b) => Subtract(a, b);
        public static Fraction operator -(Fraction a, int b) => Subtract(a, new Fraction(b, false));
        public static Fraction operator -(int a, Fraction b) => Subtract(new Fraction(a, false), b);
        public static Fraction operator -(Fraction a, decimal b) => Subtract(a, new Fraction(b, false));
        public static Fraction operator -(decimal a, Fraction b) => Subtract(new Fraction(a, false), b);

        public static Fraction operator *(Fraction a, Fraction b) => Multiply(a, b);
        public static Fraction operator *(Fraction a, int b) => Multiply(a, new Fraction(b, false));
        public static Fraction operator *(int a, Fraction b) => Multiply(new Fraction(a, false), b);
        public static Fraction operator *(Fraction a, decimal b) => Multiply(a, new Fraction(b, false));
        public static Fraction operator *(decimal a, Fraction b) => Multiply(new Fraction(a, false), b);

        public static Fraction operator /(Fraction a, Fraction b) => Divide(a, b);
        public static Fraction operator /(Fraction a, int b) => Divide(a, new Fraction(b, false));
        public static Fraction operator /(int a, Fraction b) => Divide(new Fraction(a, false), b);
        public static Fraction operator /(Fraction a, decimal b) => Divide(a, new Fraction(b, false));
        public static Fraction operator /(decimal a, Fraction b) => Divide(new Fraction(a, false), b);

        public static Fraction Add(Fraction a, Fraction b) => SafeLongAddSub(a, b, '+');
        public static Fraction Subtract(Fraction a, Fraction b) => SafeLongAddSub(a, b, '-');
        public static Fraction Multiply(Fraction a, Fraction b) => SafeLongMultDiv(a, b, '*');
        public static Fraction Divide(Fraction a, Fraction b) => SafeLongMultDiv(a, b, '/');

        public static Fraction SafeLongMultDiv(Fraction a, Fraction b, char op) {

            // -- divide by zero's --
            bool aZero = a.numerator ==0 || a.denominator==0;
            bool bZero = b.numerator ==0 || b.denominator==0;
            if (bZero) {
                throw new DivideByZeroException("Divide by zero attenpted in Fraction class");
            } else if (aZero && !bZero) {
                return new Fraction(0);
            }

            // -- calcs --
            double srLongMax = Math.Sqrt(long.MaxValue);
            // -- small & safe --
            if (Math.Abs(a.numerator)<srLongMax && Math.Abs(a.denominator)<srLongMax &&
               Math.Abs(b.numerator)<srLongMax && Math.Abs(b.denominator)<srLongMax) {
                if (op=='*') {
                    return new Fraction(a.numerator*b.numerator, a.denominator*b.denominator);
                } else if (op=='/') {
                    return new Fraction(a.numerator*b.denominator, a.denominator*b.numerator);
                }
            }
            // --else --
            decimal shrunkMax = Utils.Shrink(long.MaxValue, 10);

            decimal aNumerator = (decimal)a.numerator;
            decimal aDenominator = (decimal)a.denominator;
            decimal bNumerator = (decimal)b.numerator;
            decimal bDenominator = (decimal)b.denominator;

            decimal shrunkOp1 = 1m;
            decimal shrunkOp2 = 1m;

            if (op=='*') {
                shrunkOp1=Utils.Shrink(aNumerator, 10)*Utils.Shrink(bNumerator, 10);
                shrunkOp2=Utils.Shrink(aDenominator, 10)*Utils.Shrink(bDenominator, 10);
            } else if (op=='/') {
                shrunkOp1=Utils.Shrink(aNumerator, 10)*Utils.Shrink(bDenominator, 10);
                shrunkOp2=Utils.Shrink(aDenominator, 10)*Utils.Shrink(bNumerator, 10);
            }//else type properly

            //Console.WriteLine($@"shrunkOps@ {shrunkOp1}, {shrunkOp2}");

            decimal num = 1m;
            decimal denom = 1m;

            num=shrunkOp1;
            denom=shrunkOp2;

            //Console.WriteLine($@"Before: {num}, {denom}");

            while (Math.Abs(num) < long.MaxValue-1000000 && Math.Abs(denom) < long.MaxValue-1000000) { //Abs
                                                                                                       //Console.WriteLine($@"During: {num}, {denom}");
                num=Utils.Shrink(num, -1);
                denom=Utils.Shrink(denom, -1);
            }
            //Console.WriteLine($@"After: {num}, {denom}");

            num=Utils.Shrink(num, 2);
            denom=Utils.Shrink(denom, 2);
            //Console.WriteLine($@"Really after: {num}, {denom}");
            //Console.WriteLine($@"result = {num}/{denom}");
            //Console.WriteLine($@"OK what does longing do? {(long)num},{(long)denom}");
            //Console.WriteLine("OK barst...:" + new Fraction((long)num, (long)denom,false));

            //SwearBox.Deposit(coins);

            return new Fraction((long)num, (long)denom);
        }
        public static Fraction SafeLongAddSub(Fraction a, Fraction b, char op) {

            // -- divide by zero's --
            bool aZero = a.numerator==0 || a.denominator==0;
            bool bZero = b.numerator==0 || b.denominator==0;
            if (aZero && !bZero) {
                return b;
            } else if (bZero && !aZero) {
                return a;
            } else if (aZero && bZero) {
                return new Fraction(0);
            }

            // -- calcs --
            double srLongMax = Math.Sqrt(long.MaxValue);

            // -- small & safe --
            if (Math.Abs(a.numerator)<srLongMax/2 && Math.Abs(a.denominator)<srLongMax/2 &&
               Math.Abs(b.numerator)<srLongMax/2 && Math.Abs(b.denominator)<srLongMax/2) {
                if (op=='+') {
                    return new Fraction(a.numerator*b.denominator+b.numerator*a.denominator, a.denominator*b.denominator);
                } else if (op=='-') {
                    return new Fraction(a.numerator*b.denominator-b.numerator*a.denominator, a.denominator*b.denominator);
                }
            }

            // --else --
            decimal shrunkMax = Utils.Shrink(long.MaxValue, 10);

            decimal aNumerator = (decimal)a.numerator;
            decimal aDenominator = (decimal)a.denominator;
            decimal bNumerator = (decimal)b.numerator;
            decimal bDenominator = (decimal)b.denominator;

            decimal shrunkOp1 = Utils.Shrink(aNumerator, 10)*Utils.Shrink(bDenominator, 10);   //a*b
            decimal shrunkOp2 = Utils.Shrink(bNumerator, 10)*Utils.Shrink(aDenominator, 10);   //b*a
            decimal shrunkOp4 = Utils.Shrink(aDenominator, 10)*Utils.Shrink(bDenominator, 10); //d*d
            decimal shrunkOp3 = 1m;

            if (op=='+') {
                shrunkOp3=shrunkOp1+shrunkOp2;
            } else if (op=='-') {
                shrunkOp3=shrunkOp1-shrunkOp2;
            }//else type properly

            decimal num = 1;
            decimal denom = shrunkOp4;
            num=shrunkOp3; //No sloppy copy-paste

            while (Math.Abs(num) < long.MaxValue-1000000 && Math.Abs(denom) < long.MaxValue-1000000) { //Keep related things in sync.
                num=Utils.Shrink(num, -1);
                denom=Utils.Shrink(denom, -1);
            }
            num=Utils.Shrink(num, 2);
            denom=Utils.Shrink(denom, 2);

            //SwearBox.Deposit(coins);

            return new Fraction((long)num, (long)denom);
        }
        #endregion
    }
    public struct FractionLite {
        public long numerator { get; set; }
        public long denominator { get; set; }
        public FractionLite(long x, long y) {
            numerator=x;
            denominator=y;
        }
    }
    public static class FractionUtils {
        public static FractionLite Reduce(long n, long d) {
            int nSign = n<0 ? -1 : 1;
            int dSign = d<0 ? -1 : 1;
            n=Math.Abs(n);
            d=Math.Abs(d);
            int[] primes = Utils.Primes();
            long max = (n>d ? n : d);
            while (n>primes[primes.Length-1] || d>primes[primes.Length-1]) {
                n/=10;
                d/=10;
            }
            for (int i = 0; n>=primes[i]&&d>=primes[i]; i++) {
                if (n-(n/primes[i])*primes[i]==0  && d-(d/primes[i])*primes[i]==0) {
                    n/=primes[i];
                    d/=primes[i];
                    i=-1;
                }
            }
            return new FractionLite(n*nSign, d*dSign);
        }
        //public static FractionLite LameReduce(long n, long d){
        //    int[] primes= Utils.Primes();
        //    //int[] primes= Utils.Primes((n>d?n:d)/2);
        //    for(int i=0; i< primes.Length; i++){
        //        if(n-(n/primes[i])*primes[i]==0 ){
        //            if(d-(d/primes[i])*primes[i]==0){
        //                n/=primes[i];
        //                d/=primes[i];
        //                i=-1;
        //            } 
        //        }
        //    }
        //    return new FractionLite(n,d);
        //}

    }

}
