using System;
using System.Collections.Generic;

namespace Polish {

    class MainClass
    {
        //enum format: int
        //{
        //    option1 = 2,
        //    option2 = 4,
        //    option3 = 8,
        //    option4 = 16,
        //    option5 = 32,
        //    option6 = 64,
        //    option7 = 128,
        //}

        public class MyException : Exception {
            public MyException() {            }
            public MyException(string name) : base($@"Keep calm and carry on: {name}"){ }

        }
        public static void StackIncrement(Stack<int> stack) {
            var tmp= new Stack<int> { };
            while (stack.Count>0) tmp.Push(stack.Pop());
            while (tmp.Count>0) stack.Push(tmp.Pop()+1);
        }
        public enum testEnum : int {
            first =1,
            second =2,
            third =4,
            fourth= 8,
            fifth= 16
        }
        public class TreeNode {
            public string id { get; set; }
            public string parentID { get; set; }
            public List<TreeNode> Children { get; set; } = new List<TreeNode>{};
            public TreeNode Parent { get; set; }
        }
        public static void orphansToFro() {
            if (orphans==orphansTo) {
                orphans=orphansFro;
            } else {
                orphans=orphansTo;
            }
        }
        public static List<TreeNode> orphans = new List<TreeNode> { };
        public static List<TreeNode> orphansTo = new List<TreeNode> { };
        public static List<TreeNode> orphansFro = new List<TreeNode> { };
        public static void Main(string[] args) {
            #region moretests

            var _directoryData = new List<TreeNode> { };
            var roots = new List<TreeNode> { };
            var parented = new List<TreeNode> { };

            var orphans = orphansTo;
            bool orphaned = true;

            // Loop main list once \\
            foreach (var node in _directoryData) {
                if (node.parentID is null) { // Root? \\
                    roots.Add(node);
                } else {
                    // Factored out in non-sttic context ..
                    orphaned=true;
                    foreach (var nodeTo in roots) { // Check roots \\
                        if (nodeTo.id == node.id) {
                            nodeTo.Children.Add(node);
                            node.Parent = nodeTo;
                            orphaned=false;
                            parented.Add(node);
                            break;
                        }
                    }
                    if (orphaned) {
                        foreach (var nodeTo in parented) { // check parenteds \\
                            if (nodeTo.id == node.id) {
                                nodeTo.Children.Add(node);
                                node.Parent = nodeTo;
                                orphaned=false;
                                parented.Add(node);
                                break;
                            }
                        }
                    }
                    if (orphaned) {
                        foreach (var nodeTo in orphans) { // check still orphaneds \\
                            if (nodeTo.id == node.id) {
                                nodeTo.Children.Add(node);
                                node.Parent = nodeTo;
                                orphaned=false;
                                parented.Add(node);
                                break;
                            }
                        }
                    }
                    if (orphaned)
                        orphans.Add(node);  // Still an orphan
                }
            }
            // Loop orphans list \\
            while (orphansTo.Count>0 && orphansFro.Count>0) {
                foreach (var node in orphans){
                    orphaned=true;
                    foreach (var nodeTo in roots) {
                        if (nodeTo.id == node.id) {
                            nodeTo.Children.Add(node);
                            node.Parent = nodeTo;
                            parented.Add(node);
                            orphaned=false;
                            break;
                        }
                    }
                    if (orphaned) {
                        foreach (var nodeTo in parented) {
                            if (nodeTo.id == node.id) {
                                nodeTo.Children.Add(node);
                                node.Parent = nodeTo;
                                parented.Add(node);
                                orphaned=false;
                                break;
                            }
                        }
                    }
                    if (orphaned) {
                        foreach (var nodeTo in orphans) {
                            if (nodeTo.id == node.id) {
                                nodeTo.Children.Add(node);
                                node.Parent = nodeTo;
                                parented.Add(node);
                                orphaned=false;
                                break;
                            }
                        }
                    }
                    if (orphaned) {
                        orphansToFro();
                        orphans.Add(node);  // Still an orphan
                        orphansToFro();
                    }
                }
                orphansToFro();
                orphans.Clear();
            }


            //var incomingEnumery = testEnum.fifth | testEnum.second;

            //if (testEnum.first == (testEnum.first & incomingEnumery)) { Console.WriteLine("first"); }
            //if (testEnum.second == (testEnum.second & incomingEnumery)) { Console.WriteLine("second"); }
            //if (testEnum.third == (testEnum.third & incomingEnumery)) { Console.WriteLine("third"); }
            //if (testEnum.fourth == (testEnum.fourth & incomingEnumery)) { Console.WriteLine("fourth"); }
            //if (testEnum.fifth== (testEnum.fifth& incomingEnumery)) { Console.WriteLine("fifth"); }
            //if ((testEnum.first | testEnum.fifth) == ((testEnum.first | testEnum.fifth)& incomingEnumery)) { Console.WriteLine("first & fifth"); }
            //if ((testEnum.second| testEnum.fifth) == ((testEnum.second| testEnum.fifth)& incomingEnumery)) { Console.WriteLine("second & fifth"); }

            //if (testEnum. == (testEnum.& incomingEnumery)) { }


            //var stack1 = new Stack<int> { };

            //stack1.Push(1);
            //stack1.Push(2);
            //stack1.Push(3);
            //stack1.Push(4);

            //foreach (var i in stack1) Console.WriteLine($@"stack1: {i}");

            //StackIncrement(stack1);

            //foreach (var i in stack1) Console.WriteLine($@"stack1: {i}");


            //int cum = 0;
            ////1,3,6,10,15,21
            ////1,3,   7,12,18
            //    for (int i = 0; i<6; i++) {
            //    try {

            //        if (i!=3) {
            //            cum+=i;
            //        } else {
            //            throw new MyException("Something happened..");
            //        }
            //        Console.WriteLine($@"cum = {cum}");

            //    } catch (MyException e) {
            //        Console.WriteLine("Catching exception..");
            //        continue;
            //    } catch (Exception e) {
            //    }
            //}
            //Console.WriteLine("Out here now..");
            //int a = 64;
            //int b = 7;
            //Console.WriteLine(Utils.IsSquare(a));
            //Console.WriteLine(Utils.IsSquare(b));
            //Surd a = new Surd();
            //Surd b = new Surd();
            //a.rooted = 4;
            //b.rooted = 16;
            //Console.WriteLine(a*b);

            //int ia=3;
            //b.rooted=25;
            //Console.WriteLine(ia*b);

            //ia = 2;
            //b.rooted=9;
            //b.prefix = 3;
            //Console.WriteLine(ia*b);

            //Fraction f = new Fraction(10, 12);
            //int iiii = 5;
            //long llll = 12l;
            //decimal dddd = 2.0m;

            //Console.WriteLine($@"Fraction: {f}   int: {iiii}   long: {llll}    double: {dddd}");
            //Console.WriteLine(f*iiii);
            //Console.WriteLine(f*llll);
            //Console.WriteLine(f*dddd);

            //for (int i = 0; i<100; i++)
            //    Console.WriteLine(Utils.NotSq(2, 100));

            //decimal d1 = 22.44m;
            //string s1 = ""+d1;
            //Console.WriteLine(s1);
            //Console.WriteLine(s1.Substring(Regex.Match(s1, @"\.").Index+1));
            //Console.WriteLine(s1.Substring(0,Regex.Match(s1,@"\.").Index));

            //Font font = new Font("Arial", 12);
            //qParameters qParams = new qParameters(
            //    "/*-+", new int[,] { { 2, 10 } }, true, 50, false, false,
            //    true, true, 200, true, true, true, 1, 1, true, 2, 2, 2);

            //QuestionBuilder qb = new QuestionBuilder(qParams, font);
            //decimal[] zx = qb.RangeSampleDec();
            //foreach( var n in zx) {
            //    Console.WriteLine($@"decimal = {n}");
            //}


            //Console.WriteLine($@"{(0&1)==1}");
            //Console.WriteLine($@"{(1&1)==1}");
            //Console.WriteLine($@"{(2&1)==1}");
            //Console.WriteLine($@"{(3&1)==1}");
            //Console.WriteLine($@"{(0>>1)==1}");
            //Console.WriteLine($@"{(1>>1)==1}");
            //Console.WriteLine($@"{(2>>1)==1}");
            //Console.WriteLine($@"{(3>>1)==1}");
            //int test = Utils.r(0,3);
            //Console.WriteLine($@"x={test}");
            //Console.WriteLine($@"y={test}");

            //Bitmap picture = new Bitmap(100,100);
            //Graphics g = Graphics.FromImage(picture);
            //g.Clear(Color.White);
            //Pen p = new Pen(Color.Black);
            //g.DrawLine(p,10,10,70,80);
            //picture.Save(@"/home/havoc/Desktop/picture");



            //double ii;
            //ii=0.75;
            //Console.WriteLine(Math.Round(ii,Utils.RoundNice(ii)));
            //ii=0.1;
            //Console.WriteLine(Math.Round(ii,Utils.RoundNice(ii)));
            //ii=75;
            //Console.WriteLine(Math.Round(ii,Utils.RoundNice(ii)));
            //ii=75.0002;
            //Console.WriteLine(Math.Round(ii,Utils.RoundNice(ii)));
            //ii=75.0006;
            //Console.WriteLine(Math.Round(ii,Utils.RoundNice(ii)));



            //int n=Utils.r(1,500);
            ////bool rslt =true;;
            ////double r=0d;
            ////double sq=0d;
            //for(int i=0;i<50;i++){
            //    Console.WriteLine($@"{Utils.Not(
            //        4,
            //        (x)=>Utils.IsInt(Math.Sqrt(x)),
            //        ()=>Utils.r(1,20))
            //    }");
            //    //r=Utils.r(1,500);
            //    //sq=Math.Sqrt(r);
            //    //rslt=Utils.IsInt(sq);
            //    //Console.WriteLine($@"r: {r}. sq: {sq}, rslt: {rslt.ToString()}");

            //}

            //var options = format.option1 | format.option2 | format.option6;//70

            //Console.WriteLine(""+((options & format.option6)==format.option7));

            //Console.WriteLine($@"{Utils.dp(12m)}");
            //Console.WriteLine($@"{Utils.dp(12.00m)}");
            //Console.WriteLine($@"{Utils.dp(12.345m)}");
            //Console.WriteLine($@"{Utils.dp(12.345m,0)}");
            //Console.WriteLine($@"{Utils.dp(12.345m,1)}");
            //Console.WriteLine($@"{Utils.dp(12.346m,2)}");
            //Console.WriteLine($@"{Utils.dp(12.00m,0)}");
            //Console.WriteLine($@"{Utils.dp(12.00m,2)}");

            //Console.WriteLine($@"{Utils.MantissaWidth(123.00m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(123m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(123.4m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(123.345m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(123.345m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(123.003m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(0m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(0.4m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(0.02m)}");
            //Console.WriteLine($@"{Utils.MantissaWidth(.003m)}");

            //decimal test1 = 0.12341m;
            //decimal test1b = 0.123412341234m;
            //decimal test2 = 0.333m;
            //decimal test3 = 3.1415926536m;
            //decimal test4 = 0.1857142857890123m;
            //Console.WriteLine($@"{test1}".PadLeft(18,' ') + $@": {Math.Round(test1, Utils.RoundNice(test1))}".PadLeft(18,' '));
            //Console.WriteLine($@"{test1b}".PadLeft(18,' ') + $@": {Math.Round(test1b, Utils.RoundNice(test1b))}".PadLeft(18,' '));
            //Console.WriteLine($@"{test2}".PadLeft(18,' ') + $@": {Math.Round(test2, Utils.RoundNice(test2))}".PadLeft(18,' '));
            //Console.WriteLine($@"{test3}".PadLeft(18,' ') + $@": {Math.Round(test3, Utils.RoundNice(test3))}".PadLeft(18,' '));
            //Console.WriteLine($@"{test4}".PadLeft(18,' ') + $@": {Math.Round(test4, Utils.RoundNice(test4))}".PadLeft(18,' '));

            //double test5 = 1d;
            //double test6 = 1.1d;
            //double test7 = 5.0d;
            //double test8 = 1.001d;
            //double test9 = 0.9999999999999d;
            //double test10 = -1d;
            //double test11 = -1.1d;
            //double test12 = -5.0d;
            //double test13 = -1.001d;
            //double test14 = -0.9999999999999d;
            //Console.WriteLine($@"test5  {test5} :{ Utils.IsInt(test5)}");
            //Console.WriteLine($@"test6  {test6} :{ Utils.IsInt(test6)}");
            //Console.WriteLine($@"test7  {test7} :{ Utils.IsInt(test7)}");
            //Console.WriteLine($@"test8  {test8} :{ Utils.IsInt(test8)}");
            //Console.WriteLine($@"test9  {test9} :{ Utils.IsInt(test9)}");
            //Console.WriteLine($@"test10 {test10} :{Utils.IsInt(test10)}");
            //Console.WriteLine($@"test11 {test11} :{Utils.IsInt(test11)}");
            //Console.WriteLine($@"test12 {test12} :{Utils.IsInt(test12)}");
            //Console.WriteLine($@"test13 {test13} :{Utils.IsInt(test13)}");
            //Console.WriteLine($@"test14 {test14} :{Utils.IsInt(test14)}");

            //    double power = 4d;
            //    double bass = 8d;
            //    double rslt = Math.Pow(bass,power);
            //Console.WriteLine(rslt);
            //            double ⁿᵘᵐ = 2d;//Utils.r(2,5);
            //            double ᵈᵉⁿ = 3d;//Utils.r(2,5); //power

            //            double x = 4;//Utils.Choose(Utils.Primes(6));
            //            double xʸ = Math.Pow(x, ⁿᵘᵐ);//16
            //            string answer = $@"{Utils.NthRoot(xʸ,(int)ᵈᵉⁿ)}";

            //Console.WriteLine("x  = " +x);
            //Console.WriteLine("xʸ = " +xʸ);
            //Console.WriteLine("(int)ᵈᵉⁿ = "+(int)ᵈᵉⁿ);
            //Console.WriteLine();
            //Console.WriteLine( "answer = " + answer);
            Console.ReadLine();
            return;

            //Console.WriteLine(Math.Floor(1.5+1.75));
            //return;

            //List<int>list = Utils.Partition(7,4);
            //Console.WriteLine($@"size:{7}");
            //Console.WriteLine($@"partitions:{4}, "+string.Join(",",list)+ " = " + list.Sum());
            //return;


            //List<int> list = new List<int>();
            //int ci=0, cj=0;
            //for(ci=1;ci<20;ci++){
            //    Console.WriteLine($@"size:{ci}");
            //    for(cj=1;cj<10;cj++){
            //        list = Utils.Partition(ci,cj,3);
            //        Console.WriteLine($@"partitions:{cj}, "+string.Join(",",list)+ " = " + list.Sum());
            //    }
            //}
            //return;

            //Console.WriteLine(Utils.Choose(1,2,3,4,5,6,7,8,9,10));
            //return;
            //int[] ints = {1,1,1,1,1,1};
            //List<List<int>> rslt = new List<List<int>>();

            //for(int j=2;j<=ints.Count()+2;j++){
            //    for(int i=0;i<10;i++){
            //        rslt=Utils.Partition(ints,j);
            //        foreach(var list in rslt)
            //            Console.Write(list.Sum()+", ");
            //        Console.WriteLine();
            //    }
            //}
            //return;
            //Stopwatch sw;
            //int t =50000;
            //int nfrom = 10000;
            //int nto =100000;
            //int dfrom = 50000;
            //int dto =150000;

            //Stopwatch sw;
            //int number = Utils.r(1299828,2299827);
            //List<int> answers= new List<int>();

            //int t=10000;
            //Console.WriteLine("Go...");
            //sw =Stopwatch.StartNew();
            //for(int i=0;i<t;i++){
            //    number = Utils.r(91299828,102299827);
            //    Console.WriteLine($@"Doing: {number} ");
            //    //number = Utils.r(5000,10000);
            //    answers=Utils.PrimeFactors(number).ToList();
            //    Console.WriteLine($@"number: {number} = {string.Join(",",answers)}");
            //}
            //sw.Stop();
            //Console.WriteLine("...Done "+sw.Elapsed);

            //Console.WriteLine("Go...");
            //sw =Stopwatch.StartNew();
            //for(int i=0;i<t;i++){
            //    number = Utils.r(Int32.MaxValue/2,Int32.MaxValue-1);
            //    //number =1073751027;
            //    //number = Utils.r(5000,10000);
            //    //number=2399950;
            //    answers=Utils.PrimeFactors(number).ToList();
            //    Console.WriteLine($@"number: {number} = {string.Join(",",answers)}");
            //}
            //sw.Stop();
            //Console.WriteLine("...Done "+sw.Elapsed);

            //return;

            //Console.WriteLine("Go...");
            //sw =Stopwatch.StartNew();
            //for(int i=0;i<t;i++){
            //    FractionUtils.Reduce(Utils.r(nfrom,nto),Utils.r(dfrom,dto));
            //}
            //sw.Stop();
            //Console.WriteLine("...Done "+sw.Elapsed);
            //Console.WriteLine("Go...");
            //sw =Stopwatch.StartNew();
            //for(int i=0;i<t;i++){
            //    FractionUtils.Reduce4(Utils.r(nfrom,nto),Utils.r(dfrom,dto));
            //}
            //sw.Stop();
            //Console.WriteLine("...Done "+sw.Elapsed);
            //FractionUtils.Reduce5(27,999);
            //return;

            //Console.WriteLine("Go...");
            //sw =Stopwatch.StartNew();
            //for(int i=0;i<t;i++){
            //    FractionUtils.Reduce5(Utils.r(nfrom,nto),Utils.r(dfrom,dto));
            //}
            //sw.Stop();
            //Console.WriteLine("...Done "+sw.Elapsed);
            //return;
            //TODO: G17, G9, N/F/G, 0/#
            //decimal meDec=1234.3456m;
            //Console.WriteLine(meDec);
            //Console.WriteLine(meDec.ToString(""));
            //Console.WriteLine("G "+meDec.ToString("G"));
            //Console.WriteLine("G2 "+meDec.ToString("G2"));
            //Console.WriteLine("G3 "+meDec.ToString("G3"));
            //Console.WriteLine("G4 "+meDec.ToString("G4"));
            //Console.WriteLine("G5 "+meDec.ToString("G5"));
            //Console.WriteLine("G6 "+meDec.ToString("G6"));
            //Console.WriteLine("G7 "+meDec.ToString("G7"));
            //Console.WriteLine("G8 "+meDec.ToString("G8"));
            //Console.WriteLine("G9 "+meDec.ToString("G9"));
            //Console.WriteLine("G17 "+meDec.ToString("G17"));
            //Console.WriteLine("N "+meDec.ToString("N"));
            //Console.WriteLine("N2 "+meDec.ToString("N2"));
            //Console.WriteLine("N3 "+meDec.ToString("N3"));
            //Console.WriteLine("N4 "+meDec.ToString("N4"));
            //Console.WriteLine("F "+meDec.ToString("F"));
            //Console.WriteLine("F2 "+meDec.ToString("F2"));
            //Console.WriteLine("F3 "+meDec.ToString("F3"));
            //Console.WriteLine("F4 "+meDec.ToString("F4"));
            //Console.WriteLine();
            //return;


            //Console.WriteLine("long max: " + long.MaxValue);
            //Console.WriteLine("dec  max: " + decimal.MaxValue);
            //Fraction STILL = new Fraction(1.5833333333333333333333333333m);
            ////Console.WriteLine(STILL);q
            //Fraction STILL2 = new Fraction(-9.5m);
            //Console.WriteLine($@"{STILL} + {STILL2}");
            //Console.WriteLine(STILL + STILL2);
            //return;

            //decimal one = 12345.6789m;
            //decimal two = -12345.6789m;
            //for(int sh=1;sh<10; sh++){
            //    Console.WriteLine(Utils.Shrink(one,sh).ToString() + ",  " + Utils.Shrink(two,sh).ToString());
            //}
            //return;

            //Fraction still = new Fraction(0.666666667m);
            //Console.WriteLine(still);
            //return;

//            //    //$@"1/",
//            //    //$@"/1",
//            //    //$@"12/",
//            //    //$@"/12",
//            //    //$@"1/2/3",
//            //    //$@"13/24/35",
//            //    //$@"10 10",
//            //    //$@"10 /3",
//            //    //$@"10 1/",

            
//            string[] inputs = { 
//                $@"5 1/2 + 14 2/4 - 4 2/6 / 2 1/3",
//                $@"1/2 + 3/4 - 5/6 / 7/8 * 9/1",
//                $@"10/20 + 30/40 - 50/60 / 70/80 * 90/10",
//                $@"1/20 + 3/40 - 5/60 / 7/8 * 9/10",
//                $@"10/2 + 30/4 - 50/6 / 70/8 * 90/1",
//                $@"1/200 + 3/400 - 5/600 / 7/800 * 9/100",
//                $@"100/2 + 300/4 - 500/6 / 700/8 * 900/1",
//                $@"1/2 + 30/4 - 5/60 / 700/8 * 9/100",
//                $@"-1/2 + 3/4 - -5/6 / 7/8 * -9/1",
//                $@"-10/20 + +30/40 - -50/60 / +70/80 * -90/10",
//                $@"1 1/20 + 3/40 - 1 5/60 / 7/8 * 1 9/10",
//                $@"10 10/2 + 1 30/4 - 10 50/6 / 1 70/8 * 10 90/1",
//                $@"10 1/200 + -4 3/400 - +10 5/600 / -100 7/800 * 9/100",
//                $@"10 + -4 3/400 - +10 / -100 7/800 * 9",
                
//                $@"5 1/2+14 2/4-4 2/6/2 1/3",
//                $@"1/2+3/4-5/6/7/8*9/1",
//                $@"10/20+30/40-50/60/70/80*90/10",
//                $@"1/20+3/40-5/60/7/8*9/10",
//                $@"10/2+30/4-50/6/70/8*90/1",
//                $@"1/200+3/400-5/600/7/800*9/100",
//                $@"100/2+300/4-500/6/700/8*900/1",
//                $@"1/2+30/4-5/60/700/8*9/100",
//                $@"-1/2+3/4--5/6/7/8*-9/1",
//                $@"-10/20++30/40--50/60/+70/80*-90/10",
//                $@"1 1/20+3/40-1 5/60/7/8*1 9/10",
//                $@"10 10/2+1 30/4-10 50/6/1 70/8*10 90/1",
//                $@"10 1/200+-4 3/400-+10 5/600/-100 7/800*9/100",
//               $@"10+-4 3/400-+10/-100 7/800*9",

//                $@"1/5/(1/3/1/2)",
//                $@"(2 4/5+1 1/4)/(3 3/5)-5/16",
//                $@"1 4/5/2 1/3",

//                     $@"3 3/14+-(1 1/49*7/10)",
//                $@"1/4/(1/8*2/5)",
//                $@"1 2/3/(3/5/9/10)",
//                $@"(1 7/8*2 2/5)-3 2/3",
//                       $@"-(2 2/3+1 1/5)/5 4/5",
//                $@"3 2/3/(2/3+4/5)",
//                $@"( 5 3/5-3 1/2*2/3)/2 1/3",
//                $@"2/5*(2/3-1/4)+1/2",
//                      $@"(3 9/16*4/9)/-(2+6 1/4*1 1/5)",
//                      $@"(5/9-7/15)/(1-5/9*7/15)",
//$@"-6/9 + -3 2/12 * (-4/11 * -9 2/11 + -3 6/15 )",
//$@"-8/9 / (-7/15 + 8/17 - 1/6 + 9 3/6 )",
//$@"-5/13 / (-5/11 * 7 3/4 - 5/11 )",
//$@"-9/17 / (9/13 / 1/5 - -2 5/12 )",
//$@"-9/15 / (6/15 * -10/16 + -5 8/11 + 1/7 )",
//$@" 3 10/11 + -4 1/5 + (-7 1/9 - -5 6/16 - 8/9 )",
//$@"-6/14 / (7 3/10 / -1 8/10 * -4/11 )",
//$@"-3/6 / (-2/8 * 5 4/6 * 1 1/8 )",
//$@"-10 10/14 - (6 9/15 * 5/15 * -3/4 )",
//$@"-3 5/13 / (7/15 / -3/13 + -2 3/5 )",
//$@"  (-9 1/3 / 1/5 - (-2/6 / 6/8 * 5 5/13 ) )",
//$@"-6 1/6 / (4 6/14 * 8 6/9 + 7 1/6 * -4 3/4 )",
//$@"-9 7/10 / (-4/7 * 3 2/6 * -1 10/17 )",
//$@"9/17 - 4 8/13 * (4/7 / 9 2/5 - -9 3/11 )",
//$@"-1/10 / (-5 10/20 * -1/2 - -2 7/12 )",
//$@"((4/7 - 4/10 + -6/14 ) / 10/20 - -2 4/9  )",
//$@"((120/190 + 46/81 * -14/118 + 34/138 ) / (-20 91/156 * -41 101/176 * 73 84/182 * -106 75/140 - -114/137 * 91/133 ) )",
//$@"((-35/71 * -92/122 / -116 80/120 ) / (34 11/59 - 82 13/73 - 100 69/129 ) / -95/173 / 108/161 / -41 82/121 * -43/140  )",
//$@"(18/53 * -77/180 / 41 15/121 * 110/163 ) / (113/145 / 11/131 + -98 43/157 / 105/116  * (-13 90/207 * -12 71/147 ) )",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//$@"",
//            };

//            MyTermOpEvaluator ftoe = new MyTermOpEvaluator();
//            foreach(string term in inputs){
//                //Console.Write(term + " = ");
//                ftoe.term = term;
//                ftoe.ParseToList();
//                Console.WriteLine(ftoe.Evaluate());
////                Console.WriteLine(new Fraction(ftoe.Evaluate()).ToMixed());
            //    ftoe.termOps.Clear();
            //    ftoe.op='\0';
            //    ftoe.term=string.Empty;
            //}
            //return;

            //List<Fraction>fracs=new List<Fraction>(){ 
            //    new Fraction(1,2),
            //    new Fraction(2,3),
            //    new Fraction(3,4),
            //    new Fraction(1,2),
            //    new Fraction(5,6),
            //};
            //Fraction frac=fracs[3];
            //fracs.Remove(fracs[3]);
            //Console.WriteLine(3.5m*new Fraction(1,2)+new Fraction(43.00125m)-new Fraction("7/15")+new Fraction(-25));
            //return;
            //new Fraction()
            //Console.WriteLine(2+new Fraction(3,5));
            //Console.WriteLine(5*new Fraction(-2,14));
            //Console.WriteLine(3/new Fraction(5));
            //Console.WriteLine(2-new Fraction(1,2));
            //Console.WriteLine(new Fraction(3,7)/6);
            //Console.WriteLine(new Fraction(1,5)+6);
            //Console.WriteLine(new Fraction(2,4)-8);
            //Console.WriteLine(new Fraction(4,10)*2);
            //Console.WriteLine(new Fraction(234.1235m)*new Fraction(-321.456m));
            //Console.WriteLine(new Fraction(0.123456m)/new Fraction(654.123m));
            //Console.WriteLine(new Fraction(345.654m)+new Fraction(0.123m));
            //Console.WriteLine(new Fraction(5.5m)-new Fraction(-456.23m));
            //Console.WriteLine(2+new Fraction("3/5"));
            //Console.WriteLine(5*new Fraction("-2/14"));
            //Console.WriteLine(3/new Fraction("5"));
            //Console.WriteLine(2-new Fraction("1/2"));
            //Console.WriteLine(new Fraction("3/7")/6);
            //Console.WriteLine(new Fraction("1/5")+6);
            //Console.WriteLine(new Fraction("2/4")-8);
            //Console.WriteLine(new Fraction("4/10")*2);
            //Console.WriteLine(new Fraction("234/1235")*new Fraction("-321/456"));
            //Console.WriteLine(new Fraction("1/23456")/new Fraction("654/123"));
            //Console.WriteLine(new Fraction("345/654")+new Fraction("1/23"));
            //Console.WriteLine(new Fraction("5/5")-new Fraction("-456/23"));
            //return;

            //Fraction asd = new Fraction(10,11);
            //Console.WriteLine(asd);
            //Console.WriteLine(asd.whole + "  " + asd.fractional);
            //return;
            //TODO: next
            //Console.WriteLine(11111.2222222m.ToString());
            //Console.WriteLine(Regex.Match(11111.2222222m.ToString(),@"\d+$").Value);
            //Fraction a = new Fraction(11111.22222m);
            //Console.WriteLine(a.TopHeavy() + " = " + a.ToMixed());
            //return;
            //123456789d
            //erm.

            //decimal di;
            //for(int d=0;d<50;d++){
            //    di=Utils.dr(1, 10000, Utils.r(1,15));
            //    Console.WriteLine($@"{di} = ({Utils.CharacteristicWidth(di)}).({Utils.MantissaWidth(di)})");
            //}
            //return;

            //TODO: SafeOps finish

            //keep
            //Fraction uy=new Fraction();
            //Console.WriteLine(uy);
            //return;

            //Console.WriteLine($@"2/4 / 2/5 = {new Fraction(2,4) / new Fraction(2,5)}");

            //keep
            //Console.WriteLine($@"2/4 - 2/4 = {new Fraction(2,4) - new Fraction(2,4)}");
            //var x1 = new Fraction(2,4);
            //var x2 = new Fraction(2,4);
            //Console.WriteLine($@"{x1} - {x2} = {x1-x2}");

            //Console.WriteLine($@"3/6 - 1/2 = {new Fraction(3,6) - new Fraction(1,2)}");
            //Console.WriteLine($@"2/4 - 3/6 = {new Fraction(2,4) - new Fraction(3,6)}");

            //return;

            //keep?
            //Fraction fa = new Fraction(3,4);
            //Fraction fb = new Fraction(1,3);   
            //Console.WriteLine($@"{fa}+{fb} = {fa+fb}");
            //return;

            //Keeper
            //Fraction f1=new Fraction(7,11);
            //Fraction f2=new Fraction(2,5);
            //Console.WriteLine($@"{f1} + {f2} = {f1+f2}");
            //return;


            //Fraction f1=new Fraction();
            //Fraction f2=new Fraction();
            //long num1=0;long num2=0;
            //for(int fi=1;fi<20;fi++){
            //    num1=Utils.r(1,5);
            //    num2=Utils.r(1,5);
            //    f1=new Fraction(num1, num1+Utils.r(1,5));
            //    f2=new Fraction(num2, num2+Utils.r(1,5));
            //    //Console.WriteLine($@"{f1}+{f2}+{f1}={f1+f2+f1}");
            //    Console.Write($@"{f1}-{f2}={f1-f2},   ");
            //    Console.WriteLine($@"-{f1}={f1-f2-f1}");
            //}            
            //return;
            #endregion
            
            #region Tests
            bool BasicArithmetic = true;
            bool Factors = true;
            bool Fractions = true;
            bool Decimals = true;
            bool Ratios = true;
            bool Proportions = true;
            bool Percentages = true;
            bool Averages=true;
            IQuestion question;

            #region params
            #region Defaults
            string ops = "/*-+";
            int[,] numbers = { {2,10} };
            bool brackets = true;
            int terms = 3;
            bool fractions = false;
            bool decimals = false;
            bool integers = true;
            bool negatives=true;
            int limit = 200;
            bool includeFailure = true;
            bool unique = true;
            bool mixed = true;
            int mantissa = 2;
            int characteristic = 2;
            bool brevity = true;
            int decimalPoints =2;
            int numerator=2;
            int denominator=2;
            #endregion
            #region Basic arithmetic
            qParameters qParamsBasicArithmeticAddition = new qParameters("+", new int[,]{ {1,100 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsBasicArithmeticSubtraction= new qParameters("-", new int[,]{ {1,100 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsBasicArithmeticMultiply = new qParameters("*", new int[,]{ {2,100 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsBasicArithmeticDivision = new qParameters("/", new int[,]{ {2,100 } }, false, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsBasicArithmeticExpression = new qParameters("/*-+", new int[,]{ {2,20 } }, brackets, 4, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsLowestCommonMultiple = new qParameters(ops, new int[,]{ { 2,10} }, brackets, terms, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsDecimalPlaces = new qParameters(ops, new int[,]{ { 1,10} }, brackets, 5, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsSignificantFigures= new qParameters(ops, new int[,]{ { 1,10} }, brackets, 5, fractions, true, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            #endregion
            #region Factors
            qParameters qParamsFactors = new qParameters(ops, new int[,]{ {2,5} }, brackets, 4, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsHighestCommonFactor = new qParameters(ops, new int[,]{ {2,5} }, brackets, 3, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            #endregion
            #region fractions
            qParameters qParamsFractionsUpDown = new qParameters(ops, new int[,]{ {1,10} }, brackets, 1, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsFractionCommonDenominators = new qParameters(ops, new int[,]{ {1,9} }, brackets, 4, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsFractionCancel = new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 2, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsFractionBasic = new qParameters(ops, new int[,]{ {1,3},{2,7},{2,4},{2,5} }, brackets, 5, fractions, decimals, integers, negatives, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsFractionArithmetic= new qParameters("/-+*", new int[,]{ {1,15 } }, brackets, 6, fractions, decimals, integers, true, limit, includeFailure, false, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            #endregion
            #region decimals
            qParameters qParamsDecimals= new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsDecimalMultDiv= new qParameters(ops, new int[,]{ {2,9} }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsDecimalLongMultDiv= new qParameters(ops, new int[,]{ {2,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, mixed, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            #endregion
            #region ratios
            qParameters qParamsRatios= new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 1, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            qParameters qParamsRatioDistribute= new qParameters(ops, new int[,]{ {1,9}, {2,20 } }, brackets, 4, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            #endregion
            #region proportions
            qParameters qParamsProportions= new qParameters(ops, new int[,]{ {2,9} }, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, decimalPoints, numerator, denominator);
            #endregion
            #region percentages
            qParameters qParamsPercentages= new qParameters(ops, new int[,]{ {1,9} }, brackets, 2, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 2, numerator, denominator);
            #endregion
            #region averages
            qParameters qParamsAverages= new qParameters(ops, numbers, brackets, 5, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator);
            qParameters qParamsAveragesCompose= new qParameters(ops, numbers, brackets, 3, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator);
            qParameters qParamsAveragesSubset= new qParameters(ops, new int[,]{{1,10}}, brackets, 10, fractions, decimals, integers, false, limit, includeFailure, unique, false, mantissa, characteristic, brevity, 0, numerator, denominator);
            #endregion
            #endregion
            #region Basic arithmetic
            if (BasicArithmetic){
                qBasicArithmeticFactory fact = new qBasicArithmeticFactory();
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsBasicArithmeticAddition,1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsBasicArithmeticSubtraction,1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsBasicArithmeticMultiply,1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsBasicArithmeticDivision,1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsBasicArithmeticExpression,1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsLowestCommonMultiple,2);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsDecimalPlaces,3);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = fact.Request(i, qParamsSignificantFigures,4);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
            }
            #endregion
            #region Factors
            if(Factors){ 
                qFactorsFactory fFact = new qFactorsFactory();
                qParameters[] fParams = { qParamsFactors };
                for(int i = 1;i<5;i++){
                    question = fFact.Request(i, fParams[0], 1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }

                for(int i = 1;i<5;i++){
                    question = fFact.Request(i, qParamsFactors,2);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }

                for(int i = 1;i<5;i++){
                    question = fFact.Request(i, qParamsHighestCommonFactor,3);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
            }
            #endregion
            #region Fractions
            if(Fractions){ 
                qFractionsFactory frFact = new qFractionsFactory();
                qParameters[] fParams = { qParamsFractionsUpDown };
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, fParams[0], 1);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, fParams[0], 2);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, fParams[0], 3); //Reduce
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, fParams[0], 4);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, fParams[0], 5);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, qParamsFractionCommonDenominators, 6);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(i, qParamsFractionCancel, 7);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(1, qParamsFractionCancel, 8);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(2, qParamsFractionCancel, 9);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(3, qParamsFractionCancel, 10);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = frFact.Request(4, qParamsFractionCancel, 11);
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                question = frFact.Request(5, qParamsFractionBasic, 8);
                question.GenerateQuestion();
                outputQuestion(question);
                question = frFact.Request(5, qParamsFractionBasic, 9);
                question.GenerateQuestion();
                outputQuestion(question);
                question = frFact.Request(5, qParamsFractionBasic, 10);
                question.GenerateQuestion();
                outputQuestion(question);
                question = frFact.Request(5, qParamsFractionBasic, 11);
                question.GenerateQuestion();
                outputQuestion(question);

                for(int i = 1;i<5;i++){
                    question = frFact.Request(4, qParamsFractionArithmetic, 12);//Fraction Arithmetic
                    question.GenerateQuestion();
                    outputQuestion(question);
                }

                //big old fractions test
                //qParameters qParams = qParamsFractionArithmetic;
                //qParams.numberRanges = new int[,]{ {11,119 } };
                //string[] fops = { "+",   "-",   "*",   "/",
                //                  "+-",  "+*",  "+/",  "-*","-/","*/",
                //                  "/*-", "/*+", "*-+", 
                //                  "/*-+"};
                //foreach (string op in fops){
                //    for(int fterms=2;fterms<20;fterms+=2){
                //        qParams.ops = op;
                //        qParams.terms=fterms;
                //        qParams.negatives=true;
                //        qParams.brackets=true;
                //        for(int i=0;i<25;i++){
                //            question = frFact.Request(4, 1, qParams, 12);
                //            question.GenerateQuestion();
                //            outputQuestion(question);
                //        }
                //        qParams.negatives=true;
                //        qParams.brackets=false;
                //        for(int i=0;i<25;i++){
                //            question = frFact.Request(4, 1, qParams, 12);
                //            question.GenerateQuestion();
                //            outputQuestion(question);
                //        }
                //        qParams.negatives=false;
                //        qParams.brackets=true;
                //        for(int i=0;i<25;i++){
                //            question = frFact.Request(4, 1, qParams, 12);
                //            question.GenerateQuestion();
                //            outputQuestion(question);
                //        }
                //        qParams.negatives=false;
                //        qParams.brackets=false;
                //        for(int i=0;i<25;i++){
                //            question = frFact.Request(4, 1, qParams, 12);
                //            question.GenerateQuestion();
                //            outputQuestion(question);
                //        }
                //    }
                //}

            }
            #endregion
            #region Decimals
            if(Decimals){
                qDecimalsFactory decFact = new qDecimalsFactory();
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimals, 1); //add
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimals, 2); //subtract
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimalMultDiv, 3); //multiply
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimalMultDiv, 4); //divide
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimalLongMultDiv, 5); //long multiply
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimalLongMultDiv, 6); //long divide
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimals, 7); //fraction2decimal
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = decFact.Request(i, qParamsDecimals, 8); //decimal2fraction
                    question.GenerateQuestion();
                    outputQuestion(question);
                }

            }
            #endregion
            #region Ratios
            if(Ratios){
                qRatioProportionFactory RatPropFact = new qRatioProportionFactory();
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatios, 1); //Fraction2Ratio
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatios, 2); //Ratio2Fraction
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatios, 3); //qRatioPencePound
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatios, 4); //qRatioMixed
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatios, 5); //qRatioFind
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatioDistribute, 6); //qRatioDistribute
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsRatioDistribute, 7); //qRatioDivide
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsProportions, 1); //qProportionBuysCosts
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = RatPropFact.Request(i, qParamsProportions, 2); //qProportionThingMoves
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
            }
            #endregion
            #region percentages
            if(Percentages){
                qPercentagesFactory percFact = new qPercentagesFactory();
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 1); //qFraction2Percent
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 2); //qPercent2Fraction
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 3); //qDecimal2Percent
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 4); //qPercent2Decimal
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 5); //qPercentNoResult
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 6); //qPercentNoNumber
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 7); //qPercentNoPercentage
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 8); //qPercentProfitLoss
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 9); //qPercentDiscount
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 10); //qPercentChangeNoCost
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 11); //qPercentChangeNpSell
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                for(int i = 1;i<5;i++){
                    question = percFact.Request(i, qParamsPercentages, 12); //qPercentChangeNoPL
                    question.GenerateQuestion();
                    outputQuestion(question);
                }
                
            }
            #endregion
            #region averages
            //if(Averages){
            //    qAveragesFactory aveFact = new qAveragesFactory();
            //    for(int i = 1;i<5;i++){
            //        question = aveFact.Request(i, qParamsAverages, 1); //qAveragesMean
            //        question.GenerateQuestion();
            //        outputQuestion(question);
            //    }
            //    for(int i = 1;i<5;i++){
            //        question = aveFact.Request(i, qParamsAverages, 2); //qAveragesHowMany
            //        question.GenerateQuestion();
            //        outputQuestion(question);
            //    }
            //    for(int i = 1;i<5;i++){
            //        question = aveFact.Request(i, qParamsAveragesCompose, 3); //qAveragesMeanCompose
            //        question.GenerateQuestion();
            //        outputQuestion(question);
            //    }
            //    for(int i = 1;i<5;i++){
            //        question = aveFact.Request(i, qParamsAveragesSubset, 4); //qAveragesMeanSubset
            //        question.GenerateQuestion();
            //        outputQuestion(question);
            //    }
            //    for(int i = 1;i<5;i++){
            //        question = aveFact.Request(i, qParamsAverages, 5); //qAveragesMeanSubtract
            //        question.GenerateQuestion();
            //        outputQuestion(question);
            //    }
            //    //for(int i = 1;i<5;i++){
            //    //    question = aveFact.Request(i, qParamsAverages, 6); //qAveragesMeanCompound
            //    //    question.GenerateQuestion();
            //    //    outputQuestion(question);
            //    //}
            //}
            #endregion
            #endregion
        }
        public static void outputQuestion(IQuestion question){ 
            Console.Write(question.topText);
            string bot = "";
            //for (int j=0; j<question.botText.Count; j++)
            //    bot += question.botText[j].elementChunk;
            Console.Write("  =  " +bot);
            Console.WriteLine();
        }
    }    
}
