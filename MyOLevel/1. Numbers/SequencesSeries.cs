using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathUtils;

namespace Polish.OLevel.Numbers {

    public class qSigma1 : Question {
        public qSigma1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- ask
            var s = qb.Sigma(21,5000);
            //askBuilder.AddTextDraw($@"ask", qb.alphaFont, new Point(0, 0));
            askBuilder.AddColumn(s, new Point(0, 0));
            askBuilder.AddColumn(qb.ToColumnFraction("a","b",false,false,true), new Point(60, 10));

            // -- answer
            var askColumns = new List<qColumn> { };
            askColumns.Add(s);
            askColumns.Add(qb.ToColumnFraction("a", "b", false, false, true));
            qb.possibleAnswerFromColumns(this, askColumns);

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints ∞ ∑ Σ
        }
    }


    public class qSeqAPFindnthTerm1 : Question {
        public qSeqAPFindnthTerm1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(1, 10)-5;
            var d = Utils.R(1, 5);
            var n = Utils.R(6, 21);
            string postFix = n==1 ? "st" : n==2 ? "nd" : n==3 ? "rd" : "th";
            int[] prog = new int[5];
            prog[0]=a;
            for (int i = 1; i<5; i++) 
                prog[i]=prog[i-1]+d;

            // -- ask
            askBuilder.AddTextDraw($@"Find the {n}{postFix} term in the ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"arithmetic progression", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"{String.Join(",", prog)}", qb.alphaFont, new Point(15, 3*qb.lineSpace));
 

            var ans = a+(n-1)*d;
            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{ans}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints = qb.Maybe()?"a + (n - 1)d":"dn + (a - d)";
        }
    }
    public class qSeqAPFindnthTerm2 : Question {
        public qSeqAPFindnthTerm2(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            var askColumns = new List<qColumn>{ };

            var a = Utils.R(1, 10);
            var numerator = Utils.R(1, 10);
            var fl = FractionUtils.Reduce(numerator, numerator+Utils.R(1, 10));
            var d = new Fraction(fl.numerator, fl.denominator);

            var n = Utils.R(6, 21);
            string postFix = n==1 ? "st" : n==2 ? "nd" : n==3 ? "rd" : "th";
            Fraction[] prog = new Fraction[5];
            prog[0]=new Fraction(a, 1);
            askColumns.Add(qb.ToColumnFraction(prog[0]));
            askColumns.Add(qb.CommaColumn());
            for (int i = 1; i<5; i++) {
                prog[i]=prog[i-1]+d;
                askColumns.Add(qb.ToColumnFraction(prog[i]));
                if(i<4)
                    askColumns.Add(qb.CommaColumn());
            }

            // -- ask
            askBuilder.AddTextDraw($@"Find the {n}{postFix} term in the arithmetic progression", qb.alphaFont, new Point(0, qb.letterHeight));
            //askBuilder.AddTextDraw($@"{String.Join(",", prog)}", qb.alphaFont, new Point(15, 2*qb.lineSpace));
            askBuilder.AddColumns(askColumns, new Point(0, 3*qb.lineSpace));

            //var ans = a+(n-1)*d;
            var ans = prog[0]+(n-1)*d;
            // -- answer
            //qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{ans}"));
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(ans));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints = qb.Maybe() ? "a + (n - 1)d" : "dn + (a - d)";
        }
    }
    public class qSeqAPFindnmthTerms : Question {
        public qSeqAPFindnmthTerms(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(1, 10)-5;
            var d = Utils.R(1, 5);
            var n = Utils.R(6, 21);
            var m = n + Utils.R(6, 10);
            string nPostfix = GraphicsUtils.PostFix(n);
            string mPostfix = GraphicsUtils.PostFix(m);
            int[] prog = new int[5];
            prog[0]=a;
            for (int i = 1; i<5; i++)
                prog[i]=prog[i-1]+d;

            // -- ask
            askBuilder.AddTextDraw($@"Find the {n}{nPostfix} and {m}{mPostfix} terms ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"in the arithmetic progression", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"{String.Join(",", prog)}", qb.alphaFont, new Point(15, 3*qb.lineSpace));

            var nAns = a+(n-1)*d;
            var mAns = a+(m-1)*d;

            // -- answer
            var ansCols = new List<qColumn> { };
            ansCols.Add(qb.ToSingleInteger($@"{nAns}"));
            ansCols.Add(qb.CommaColumn());
            ansCols.Add(qb.ToSingleInteger($@"{mAns}"));
            qb.possibleAnswerFromColumns(this, ansCols);

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints = qb.Maybe() ? "a + (n - 1)d" : "dn + (a - d)";
        }
    }
    public class qSeqAPFind3rdTerm : Question {
        public qSeqAPFind3rdTerm(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(1, 10)-5;
            var d = Utils.R(1, 5);
            var n = Utils.R(1, 5);
            var m = n + Utils.R(6, 10);
            var o = m + Utils.R(6, 10);
            string nPostfix = GraphicsUtils.PostFix(n);
            string mPostfix = GraphicsUtils.PostFix(m);
            string oPostfix = GraphicsUtils.PostFix(o);
            int[] prog = new int[o];
            prog[0]=a;
            for (int i = 1; i<o; i++)
                prog[i]=prog[i-1]+d;

            // -- ask
            askBuilder.AddTextDraw($@"The {n}{nPostfix} term of an arithmetic progression", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"is {prog[n-1]} and the {m}{nPostfix} term is {prog[m-1]}.", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"What is the {o}{oPostfix} term?", qb.alphaFont, new Point(0, 2*qb.lineSpace));

            var ans = prog[o-1];

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(ans));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintBitmap = askBuilder.Reset();
            hintBitmap.AddTextDraw("  a+(n-1)d", qb.alphaFont,new Point(0,0));
            hintBitmap.AddTextDraw("- a+(m-1)d", qb.alphaFont, new Point(0, qb.lineSpace));
            hintBitmap.AddLineDraw(Pens.Black,new Point(3, 2*qb.lineSpace+3), new Point(95, 2*qb.lineSpace+3));
            hintsBitmap=hintBitmap.Commit();
        }
    }
    public class qSeqAPWhichTerm : Question {
        public qSeqAPWhichTerm(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(1, 10)-5;
            var d = Utils.R(1, 5);
            var n = Utils.R(6, 21);
            
            int[] prog = new int[n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]+d;

            var number = prog[n-1];

            // -- ask
            askBuilder.AddTextDraw($@"Which term is {number} in the following ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"arithmetic progression:", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"{String.Join(",", prog.Take(4))}...?", qb.alphaFont, new Point(15, 3*qb.lineSpace));

            var ans = n;
            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{ans}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints = qb.Maybe() ? "a + (n - 1)d" : "dn + (a - d)";
        }
    }
    public class qSeqAPArithmeticMeans : Question {
        public qSeqAPArithmeticMeans(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(2, 45)-10;
            decimal end = a + Utils.R(20, 100);
            decimal n = Utils.R(5, 10);
            decimal d = (end-a)/(n-1);

            decimal[] prog = new decimal[(int)n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]+d;
            prog[(int)n-1]=end;

            // -- ask
            askBuilder.AddTextDraw($@"Insert {n-2} arithmetic means ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"between {prog.First()} and {prog.Last()}", qb.alphaFont, new Point(0, qb.lineSpace));

            //Utils.MyDecimal(prog.Last(), DP.specify|DP.prompt, qParams.mantissa).answer;

            var ansCols = new List<qColumn> { };
            //ansCols.Add(qb.ToSingleInteger(""+prog[0], true));
            ansCols.Add(qb.ToSingleInteger(Utils.MyDecimal(prog[0], DP.specify|DP.prompt, qParams.mantissa).answer, true));
            foreach (var i in prog.Skip(1).Take((int)n-2)) {
                //ansCols.Add(qb.ToSingleInteger(""+i));
                ansCols.Add(qb.ToSingleInteger(Utils.MyDecimal(i, DP.specify|DP.prompt, qParams.mantissa).answer));
            }
            //ansCols.Add(qb.ToSingleInteger(""+prog.Last(), true));
            ansCols.Add(qb.ToSingleInteger(Utils.MyDecimal(prog.Last(), DP.specify|DP.prompt, qParams.mantissa).answer, true));

            // -- answer
            qb.possibleAnswerFromColumns(this, ansCols);

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints = qb.Maybe()?$@"{prog[0]} + ({n} - 1)d = {prog[(int)n-1]}":$@"d * {n} + ({prog[0]} - d) = {prog[(int)n-1]}";
        }
    }
    public class qSeqAPSum1 : Question {
        public qSeqAPSum1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(1, 50)-10;
            decimal d = Utils.R(1, 20)*(qb.Maybe()?1:-1);
            decimal n = Utils.R(5, 15);

            decimal[] prog = new decimal[(int)n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]+d;

            //decimal a = Utils.r(2, 45)-10;
            //decimal end = a + Utils.r(20, 100);
            //decimal n = Utils.r(5, 10);
            //decimal d = (end-a)/(n-1);
            var sum = n/2*(a+prog.Last());

            // -- ask
            askBuilder.AddTextDraw($@"Find the sum of the series", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"{a}, {a+d}, {a+2*d}, ...", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"if there are {n} terms in the series.", qb.alphaFont, new Point(0, 2*qb.lineSpace));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(
                Utils.MyDecimal(sum, DP.specify|DP.prompt, qParams.mantissa).answer
            ));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintsBuilder = askBuilder.Reset();
            var hintColumns = new List<qColumn> { };
            hintColumns.Add(qb.ToSingleInteger("S", true));
            hintColumns.Add(qb.ToSingleInteger("=", true));
            if (qb.Maybe()) {
                hintColumns.Add(qb.ToColumnFraction("n", "2", true, true, true));
                hintColumns.Add(qb.ToSingleInteger("( first + last )", true));
            } else {
                hintColumns.Add(qb.ToSingleInteger("n", true));
                hintColumns.Add(qb.ToColumnFraction("(first + last)", "2", true, true, true));
            }
            hintsBuilder.AddColumns(hintColumns, new Point(0, 0));
            hintsBitmap= hintsBuilder.Commit();

        }
    }
    public class qSeqAPSum2 : Question {
        public qSeqAPSum2(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(1, 50)-10;
            decimal d = Utils.R(3, 20);
            decimal n = Utils.R(15, 35); //15

            var x = Utils.R(3, 8); //8
            var y = x + Utils.R(3, (int)n-(4+x));
            string xPostfix = GraphicsUtils.PostFix(x);
            string yPostfix = GraphicsUtils.PostFix(y);

            decimal[] prog = new decimal[(int)n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]+d;

            // -- ask
            askBuilder.AddTextDraw($@"An arithmetic progression has its ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"{x}{xPostfix} term equal to {prog[x-1]} and ", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"its {y}{yPostfix} term equal to {prog[y-1]}.", qb.alphaFont, new Point(0, 2*qb.lineSpace));
            askBuilder.AddTextDraw($@"If it has {n} terms, what's its sum?", qb.alphaFont, new Point(0, 3*qb.lineSpace));

            // -- answer
            var sum = n/2*(a+prog.Last());
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(
                Utils.MyDecimal(sum, DP.specify|DP.prompt, qParams.mantissa).answer
            ));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintsBuilder = askBuilder.Reset();
            var hintColumns = new List<qColumn> { };
            hintColumns.Add(qb.ToSingleInteger("S",true));
            hintColumns.Add(qb.ToSingleInteger("=", true));
            if (qb.Maybe()) {
                hintColumns.Add(qb.ToColumnFraction("n", "2", true, true, true));
                hintColumns.Add(qb.ToSingleInteger("( first + last )", true));
            } else if (qb.Maybe()) {
                hintColumns.Add(qb.ToColumnFraction("n", "2", true, true, true));
                hintColumns.Add(qb.ToSingleInteger("(2*first+(n-1)*d)", true));
            } else {
                hintColumns.Add(qb.ToSingleInteger("n", true));
                hintColumns.Add(qb.ToColumnFraction("(first + last)", "2", true, true, true));
            }

            hintsBuilder.AddColumns(hintColumns,new Point(0,0));
            hintsBitmap= hintsBuilder.Commit();
        }
    }
    public class qSeqAPSum3 : Question {
        public qSeqAPSum3(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(1, 50)-10;
            decimal d = Utils.R(3, 20);
            decimal n = Utils.R(5, 15);

            string nPostfix = GraphicsUtils.PostFix((int)n);

            decimal[] prog = new decimal[(int)n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]+d;

            // -- ask
            askBuilder.AddTextDraw($@"Find the sum of an arithmetic progression ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"which has a first term of {a} and", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"its {n}{nPostfix} and last term as {prog.Last()}.", qb.alphaFont, new Point(0, 2*qb.lineSpace));

            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));

            // -- answer
            var sum = n/2*(a+prog.Last());
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(
                Utils.MyDecimal(sum, DP.specify|DP.prompt, qParams.mantissa).answer
            ));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintsBuilder = askBuilder.Reset();
            var hintColumns = new List<qColumn> { };
            hintColumns.Add(qb.ToSingleInteger("S", true));
            hintColumns.Add(qb.ToSingleInteger("=", true));
            if (qb.Maybe()) {
                hintColumns.Add(qb.ToColumnFraction("n", "2", true, true, true));
                hintColumns.Add(qb.ToSingleInteger("( first + last )", true));
            } else if (qb.Maybe()) {
                hintColumns.Add(qb.ToColumnFraction("n", "2", true, true, true));
                hintColumns.Add(qb.ToSingleInteger("(2*first+(n-1)*d)", true));
            } else {
                hintColumns.Add(qb.ToSingleInteger("n", true));
                hintColumns.Add(qb.ToColumnFraction("(first + last)", "2", true, true, true));
            }
            hintsBuilder.AddColumns(hintColumns, new Point(0, 0));
            hintsBitmap= hintsBuilder.Commit();
        }
    }
    public class qSeqAPSumRange : Question {
        public qSeqAPSumRange(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(5, 50);
            decimal d = Utils.R(3, 20);
            int n = Utils.R(25, 50);

            string nPostfix = GraphicsUtils.PostFix((int)n);

            decimal[] prog = new decimal[(int)n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]+d;

            var from = Utils.R(10,20);
            var to = n;
            var fromPostFix = GraphicsUtils.PostFix(from);
            var toPostfix = GraphicsUtils.PostFix(n);

            // -- ask
            askBuilder.AddTextDraw($@"Sum from the {from}{fromPostFix} to {to}{toPostfix}", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"terms in the following arithmetic", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"progression:", qb.alphaFont, new Point(0, 2*qb.lineSpace));
            askBuilder.AddTextDraw($@"{String.Join(",", prog.Take(3))}, ...", qb.alphaFont, new Point(15, 3*qb.lineSpace));

            // -- answer
            var all = n/2*(a+prog.Last());
            var befores = n/2*(a+prog[from-1]);
            var ans = all-befores;
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(
                Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa).answer
            ));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintsBuilder = askBuilder.Reset();
            var hintColumns = new List<qColumn> { };
            hintColumns.Add(qb.Sigma(1,n));
            hintColumns.Add(qb.ToSingleInteger("     -     ", true));
            hintColumns.Add(qb.Sigma(1,from-1));
            hintsBuilder.AddColumns(hintColumns, new Point(0, 0));
            hintsBitmap= hintsBuilder.Commit();
        }
    }

    public class qSeqHPFindnthTerm : Question {
        public qSeqHPFindnthTerm(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            var askColumns = new List<qColumn> { };

            var a = Utils.R(1, 50);
            var d = Utils.R(3, 15);
            var n = Utils.R(6, 21);
            string postFix = n==1 ? "st" : n==2 ? "nd" : n==3 ? "rd" : "th";
            int[] prog = new int[4];
            prog[0]=a;
            askColumns.Add(qb.ToColumnFraction(new Fraction(1, prog[0])));
            askColumns.Add(qb.CommaColumn());
            for (int i = 1; i<4; i++) {
                prog[i]=prog[i-1]+d;
                askColumns.Add(qb.ToColumnFraction(new Fraction(1, prog[i])));
                askColumns.Add(qb.CommaColumn());
            }
            askColumns.Add(qb.OpColumn("..."));

            // -- ask
            askBuilder.AddTextDraw($@"Find the {n}{postFix} term in the ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"following harmonic  progression:", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddColumns(askColumns, new Point(0, 3*qb.lineSpace));

            var ans = a+(n-1)*d;
            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(new Fraction(1,ans)));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints = "as reciprocal AP";
        }
    }

    public class qSeqGPFindnthTerm1 : Question {
        public qSeqGPFindnthTerm1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(5, 10);
            var r = Utils.R(2, 5);
            var n = Utils.R(6, 15);
            string postFix = n==1 ? "st" : n==2 ? "nd" : n==3 ? "rd" : "th";
            double[] prog = new double[3];
            prog[0]=a;
            for (double i = 1; i<3; i++)
                prog[(int)i]=prog[(int)i-1]*r;

            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"Find the {n}{postFix} term in the ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"geometric progression", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"{String.Join(", ", prog)}, ...", qb.alphaFont, new Point(15, 3*qb.lineSpace));

            // -- answer
            var ans =a*Math.Pow(r,n-1);
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{ans}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints=$@"ar{GraphicsUtils.ToSuper("n-1")}";
        }
    }
    public class qSeqGPFindnmthTerms : Question {
        public qSeqGPFindnmthTerms(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(2, 5);
            var r = Utils.R(2, 5);
            var n = Utils.R(5, 10);
            var m = n + Utils.R(3, 7);
            string nPostfix = GraphicsUtils.PostFix(n);
            string mPostfix = GraphicsUtils.PostFix(m);
            int[] prog = new int[5];
            prog[0]=a;
            for (int i = 1; i<5; i++)
                prog[i]=prog[i-1]*r;

            // -- ask
            askBuilder.AddTextDraw($@"Find the {n}{nPostfix} and {m}{mPostfix} terms ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"in the geometric progression", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"{String.Join(",", prog)}", qb.alphaFont, new Point(15, 3*qb.lineSpace));

            var nAns = a*Math.Pow(r,n-1);;
            var mAns = a*Math.Pow(r,m-1); ;

            // -- answer
            var ansCols = new List<qColumn> { };
            ansCols.Add(qb.ToSingleInteger($@"{nAns}"));
            ansCols.Add(qb.CommaColumn());
            ansCols.Add(qb.ToSingleInteger($@"{mAns}"));
            qb.possibleAnswerFromColumns(this, ansCols);

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints=$@"ar{GraphicsUtils.ToSuper("n-1")}";
        }
    }
    public class qSeqGPFind3rdTerm1 : Question {
        public qSeqGPFind3rdTerm1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(1, 5);
            var r = Utils.R(2, 6);
            var n = 1;// Utils.r(3, 4);
            var m = n + Utils.R(3, 5);
            var o = m + Utils.R(3, 4);
            string nPostfix = GraphicsUtils.PostFix(n);
            string mPostfix = GraphicsUtils.PostFix(m);
            string oPostfix = GraphicsUtils.PostFix(o);
            int[] prog = new int[o];
            prog[0]=a;
            for (int i = 1; i<o; i++)
                prog[i]=prog[i-1]*r;

            // -- ask
            askBuilder.AddTextDraw($@"The {n}{nPostfix} term of a geometric progression", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"is {prog[n-1]} and the {m}{mPostfix} term is {prog[m-1]}.", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"What is the {o}{oPostfix} term?", qb.alphaFont, new Point(0, 2*qb.lineSpace));

            var ans = prog[o-1];

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(ans));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintBitmap = askBuilder.Reset();
            hintBitmap.AddTextDraw($@"{prog[0]}r{GraphicsUtils.ToSuper($@"{m-1}")} = {prog[m-1]} ...", qb.alphaFont, new Point(0, 0));
            hintBitmap.AddTextDraw($@"r{GraphicsUtils.ToSuper($@"{m-1}")} = ", qb.alphaFont, new Point(0, 2*qb.lineSpace));
            hintBitmap.AddColumn(qb.ToColumnFraction($@"{prog[m-1]}",$@"{prog[n-1]}",true,true,true),new Point(50,2*qb.lineSpace-qb.lineSpace/2));
            hintBitmap.AddTextDraw(" ...", qb.alphaFont,new Point(100, 2*qb.lineSpace));
            var ac = qb.ToColumnFraction($@"{prog[m-1]}", $@"{prog[n-1]}", true, true, true);
            hintBitmap.AddColumn(ac, new Point(50, 3*qb.lineSpace-qb.lineSpace/2));
            hintsBitmap =hintBitmap.Commit();
        }
    }
    public class qSeqGPFind3rdTerm2 : Question {
        public qSeqGPFind3rdTerm2(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var a = Utils.R(1, 5);
            var r = Utils.R(2, 6);
            var n = Utils.R(3, 4);
            var m = n + Utils.R(3, 5);
            var o = m + Utils.R(3, 4);
            string nPostfix = GraphicsUtils.PostFix(n);
            string mPostfix = GraphicsUtils.PostFix(m);
            string oPostfix = GraphicsUtils.PostFix(o);
            int[] prog = new int[o];
            prog[0]=a;
            for (int i = 1; i<o; i++)
                prog[i]=prog[i-1]*r;

            // -- ask
            askBuilder.AddTextDraw($@"The {n}{nPostfix} term of a geometric progression", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"is {prog[n-1]} and the {m}{mPostfix} term is {prog[m-1]}.", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"What is the {o}{oPostfix} term?", qb.alphaFont, new Point(0, 2*qb.lineSpace));

            var ans = prog[o-1];

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(ans));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            Hints="Treat as 1..n, find arithmetic means.";
        }
    }
    public class qSeqGPGeometicMeans : Question {
        public qSeqGPGeometicMeans(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(1, 5);
            decimal r = Utils.R(11, 50)/10m;
            decimal n = Utils.R(3, 5);
            decimal end;

            decimal[] prog = new decimal[(int)n+2];
            prog[0]=a;
            for (int i = 1; i<n+2; i++)
                prog[i]=prog[i-1]*r;

            // -- ask
            askBuilder.AddTextDraw($@"Insert {n} geometric means ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"between {prog.First()} and {prog.Last()}", qb.alphaFont, new Point(0, qb.lineSpace));

            var ansCols = new List<qColumn> { };
            ansCols.Add(qb.ToSingleInteger(""+prog[0], true));
            foreach (var i in prog.Skip(1).Take((int)n)) 
                ansCols.Add(qb.ToSingleInteger(i));
            ansCols.Add(qb.ToSingleInteger(""+prog.Last(), true));

            // -- answer
            qb.possibleAnswerFromColumns(this, ansCols);

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintBitmap = askBuilder.Reset();
            hintBitmap.AddTextDraw($@"{prog.First()}r{GraphicsUtils.ToSuper($@"{n+1}")} = {prog.Last()} ...", qb.alphaFont, new Point(0, 0));
            hintBitmap.AddTextDraw($@"r{GraphicsUtils.ToSuper($@"{n+1}")} = ", qb.alphaFont, new Point(0, 2*qb.lineSpace));
            hintBitmap.AddColumn(qb.ToColumnFraction($@"{prog.Last()}", $@"{prog.First()}", true, true, true), new Point(50, 2*qb.lineSpace-qb.lineSpace/2));
            hintBitmap.AddTextDraw("...", qb.alphaFont, new Point(100, 3*qb.lineSpace));
            //var ac = qb.ToColumnFraction($@"{prog[(int)n-1]}", $@"{prog[(int)n-1]}", true, true, true);
            //hintBitmap.AddColumn(ac, new Point(50, 2*qb.lineSpace-qb.lineSpace/2));
            hintsBitmap =hintBitmap.Commit();
        }
    }
    public class qSeqGPSum1 : Question {
        public qSeqGPSum1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal a = Utils.R(1, 5);
            decimal r = Utils.R(2, 6);
            decimal n = Utils.R(5, 9);

            decimal[] prog = new decimal[(int)n];
            prog[0]=a;
            for (int i = 1; i<n; i++)
                prog[i]=prog[i-1]*r;

            var sum  = ((double)a*(1-Math.Pow((double)r,(double)n)))/(double)(1-r);
            var sum2 = ((double)a*(Math.Pow((double)r, (double)n)-1))/(double)(r-1);

            // -- ask
            askBuilder.AddTextDraw($@"Find the sum of the series", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"{prog[0]}, {prog[1]}, {prog[2]}, ...", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"if there are {n} terms in the series.", qb.alphaFont, new Point(0, 2*qb.lineSpace));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(
                //Utils.MyDecimal(sum, DP.specify|DP.prompt, qParams.mantissa).answer
                ""+sum
            ));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
            var hintsBuilder = askBuilder.Reset();
            var hintColumns = new List<qColumn> { };
            hintColumns.Add(qb.ToSingleInteger("S", true));
            hintColumns.Add(qb.ToSingleInteger("=", true));
            if (qb.Maybe()) {
                hintColumns.Add(qb.ToColumnFraction($@"a(r{GraphicsUtils.ToSuper("n")} - 1)", "r - 1", true, true, true));
            } else {
                hintColumns.Add(qb.ToColumnFraction($@"a(1 - r{GraphicsUtils.ToSuper("n")})", "1 - r", true, true, true));
            }
            hintsBuilder.AddColumns(hintColumns, new Point(0, 0));
            hintsBitmap= hintsBuilder.Commit();
        }
    }


    //sum2Infinity
    //fractions
    //alternators

}
