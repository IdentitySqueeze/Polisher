using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Polish.OLevel.Numbers;
using MathUtils;

namespace Polish {
    public class qBasicArithmetic : Question {
        public qBasicArithmetic(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            string question = string.Empty;
            List<string> terms = new List<string>();//So I can do Remove()'s

            // -- Generate term numbers --
            qb.rangeInt=qb.RangeSampleInt();
            foreach (int r in qb.rangeInt)
                terms.Add(r.ToString());
            char op;
            StringBuilder sbTerm = new StringBuilder();
            int t1, t2 = 0;
            string term1, term2;
            bool brackets, bracketted = false;
            bool bCont = true;

            // -- Build terms --
            while (terms.Count>1) {

                //Add at least one set of brackets if bracketting
                brackets = false;

                if (qParams.brackets && (Utils.R(0, 10)>7 || (terms.Count == 2 && !bracketted))) {
                    brackets = true;
                    bracketted = true;
                }

                //Loop until term is good
                bCont=true;
                int failCount = 0;
                while (bCont && terms.Count>1 && failCount <100) {
                    failCount++;
                    bCont=false;
                    sbTerm.Clear();

                    // -- Make pairs (leaves singles) --
                    t1 = Utils.R(0, terms.Count-1);
                    term1 = terms[t1];
                    terms.Remove(terms[t1]);//doesn't matter which

                    t2 = Utils.R(0, terms.Count-1);
                    term2 = terms[t2];
                    terms.Remove(terms[t2]);

                    //  -- Choose op --
                    op = qParams.ops[Utils.R(1, qParams.ops.Length)-1];

                    // -- Add brackets --
                    if (brackets)
                        sbTerm.Append("(");
                    sbTerm.Append(term1).Append(" ").Append(op).Append(" ").Append(term2);
                    if (brackets)
                        sbTerm.Append(")");

                    // -- Checks ---------------------------------------------
                    if (failCount!=100) {
                        BasicTermOpEvaluator tmpToc = new BasicTermOpEvaluator();
                        tmpToc.term = sbTerm.ToString();
                        tmpToc.ParseToList();
                        double dCheck = tmpToc.Evaluate();

                        // -- Decimals --
                        if (!bCont && qParams.ops.Contains('/') && !qParams.decimals && dCheck % 1.0d != 0.0d)
                            bCont=true;

                        // -- Divide by zero 
                        if (!bCont && dCheck ==0.0d) {
                            bCont=true;
                        }

                        // -- Put 'em back in if it's failed --
                        if (bCont) {
                            terms.Add(term1);
                            terms.Add(term2);
                        }
                    } else { //okay scrap and start again
                        // -- Generate term numbers --
                        terms.Clear();
                        qb.rangeInt=qb.RangeSampleInt();
                        foreach (int r in qb.rangeInt)
                            terms.Add(r.ToString());
                        failCount=0;
                        bCont=true;
                        //Console.WriteLine(" (--------- Failed ---------)");
                    }
                }
                terms.Add(sbTerm.ToString());
            }
            question = terms[0].Trim();

            BasicTermOpEvaluator toc = new BasicTermOpEvaluator();
            toc.term = question;
            toc.ParseToList();
            qb.answer = toc.Evaluate().ToString();

            topText = question;

            //botText is 'custom' chars such as '(', '+', '145' ...
            //botText.Add(new qChunk(id, 0, qb.answer, false));

            //qb.ChunkRowToColumnInteger( this );
        }
    }

    public class qAveragesMeanHowMany : Question {
        public qAveragesMeanHowMany(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            qb.rangeInt=qb.RangeSampleInt();
            int average = qb.rangeInt[0];
            int count = qb.rangeInt[1];
            int total = count*average;
            // -- return --
            //topText=$@"Items averaging {average} total{qb.br( )}to {total}.{qb.br( )}How many items?";
            //botText.Add(new answerChunk(id, 0, count.ToString(), false));

            askBuilder.AddTextDraw($@"Items averaging {average}", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"total to {total}.", qb.alphaFont, new Point(0, 17));
            askBuilder.AddTextDraw("How many items are there?", qb.alphaFont, new Point(0, 34));

            //qb.ChunkRowToColumnInteger( this );
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(count));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qAveragesMeanCompose : Question {
        public qAveragesMeanCompose(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            thangy[] things = { new thangy("instrument reading", "mm"),
                               new thangy("weight of boxes in a container","kg"),
                               new thangy("mass of apples in a box","kg"),
                               new thangy("age of offspring","yrs"),
                               new thangy("mark of exam candidates","")};
            thangy thing = things[Utils.R(0, things.Length-1)];

            askBuilder.AddTextDraw($@"Find the average", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"{thing.slarty} if", qb.alphaFont, new Point(0, 17));

            //qb.sbTop.Append($@"Find the average {thing.slarty}{qb.br( )}if ");
            int[] items = qb.RangeSampleInt();       // 2, 3, 4
            int[] measuresCum = qb.RangeSampleInt(); // 5, 6, 7

            int i = 0;
            askBuilder.AddTextDraw($@"{items[i]} are {measuresCum[i]}{thing.bartfast}", qb.alphaFont, new Point(0, 34));
            //qb.sbTop.Append($@"{items[i]} are {measuresCum[i]}{thing.bartfast}" );
            measuresCum[i]*=items[i]; // 2, 10
            i++;
            for (; i<qParams.terms-1; i++) {
                askBuilder.AddTextDraw($@",{items[i]} are {measuresCum[i]}{thing.bartfast}", qb.alphaFont, new Point(0, 34+(i*17)));
                //qb.sbTop.Append($@",{qb.br( )}{items[i]} are {measuresCum[i]}{thing.bartfast}" );
                measuresCum[i]*=items[i]; // 3, 18
            }
            askBuilder.AddTextDraw($@"{qb.br()}and {items[i]}{qb.br()}are {measuresCum[i]}{thing.bartfast}.", qb.alphaFont, new Point(0, 34+(i*17)));
            //qb.sbTop.Append($@"{qb.br( )}and {items[i]}{qb.br( )}are {measuresCum[i]}{thing.bartfast}." );
            measuresCum[i]*=items[i]; // 4, 28

            //topText=$@"{qb.sbTop}";         // 56            /         9
            //botText.Add(new qDets(id,0,(Math.Round((decimal)measuresCum.Sum()/(decimal)items.Sum(),2)).ToString(),false));
            qb.answer=$@"{(Math.Round((decimal)measuresCum.Sum()/items.Sum(), 2))}";
            //botText.Add(new answerChunk(id,0,qb.answer,false));

            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            //qb.possibleAnswerFromColumn(this, qb.ToColumnInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qAveragesMeanSubset : Question {
        public qAveragesMeanSubset(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // The average of {x} numbers is {a} and the average of {y} of them is {b}. What's the average of the remaining {x-y}?"
            qb.rangeInt = qb.RangeSampleInt();

            int x = qParams.terms;    //10
            int y = Utils.R(3, qParams.terms-3);
            int r = x-y;

            List<int> xSet = qb.rangeInt.ToList<int>();
            List<int> ySubset = Utils.Subset(y, qb.rangeInt).ToList<int>();
            List<int> rSubset = xSet.Skip(y).Take(xSet.Count-y).ToList<int>();

            decimal xAverage = Math.Round((decimal)xSet.Sum()/(decimal)x, 2);
            decimal yAverage = Math.Round((decimal)ySubset.Sum()/(decimal)y, 2);
            decimal rAverage = Math.Round((decimal)rSubset.Sum()/(decimal)r, 2);

            askBuilder.AddTextDraw($@"The average of {x} numbers is {xAverage} ", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"and the average of {y} of them is {yAverage}.", qb.alphaFont, new Point(0, 17));
            askBuilder.AddTextDraw($@"What's the average of the remaining {r} ?", qb.alphaFont, new Point(0, 34));
            //topText=$@"The average of {x} numbers is{qb.br( )}{xAverage}{qb.br( )}and the average of {y} of them is {yAverage}.{qb.br( )}What's the average of the remaining {r} ?";
            //botText.Add(new answerChunk(id, 0, $@"{rAverage}", false));

            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(rAverage));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qAveragesMeanSubtract : Question {
        public qAveragesMeanSubtract(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //"The average of {x} numbers is {a}. What would the average be if one number, {y}, was removed?"
            qb.rangeInt=qb.RangeSampleInt();
            List<int> xSet = qb.rangeInt.ToList<int>();
            decimal xAverage = Math.Round((decimal)xSet.Average(), 2);
            int i = Utils.R(0, qb.rangeInt.Length-1);
            int y = xSet[i];
            xSet.RemoveAt(i);
            decimal newAverage = Math.Round((decimal)xSet.Average(), 2);
            askBuilder.AddTextDraw($@"The average of {qParams.terms} numbers is {xAverage}", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"What would the average be ", qb.alphaFont, new Point(0, 17));
            askBuilder.AddTextDraw($@"if one number, {y}, was removed?""", qb.alphaFont, new Point(0, 34));
            //topText = $@"The average of {qParams.terms} numbers is{qb.br( )}{xAverage}.{qb.br( )}What would the average be if one number, {y}, was removed?";
            //botText.Add(new answerChunk(id,0, $@"{newAverage}", false));
            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(newAverage));

            askBitmap=askBuilder.Commit();
        }
    }

    public class qFractionReduce : Question {
        public qFractionReduce(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- get fraction --
            qb.rangeInt=qb.RangeSampleInt(); //numberRange==1,10; terms==1

            int numerator = Utils.R(1, qb.rangeInt[0]);
            FractionLite f = new FractionLite(numerator, numerator+Utils.R(1, 10)); //Proper

            // -- multiply --
            int mult = Utils.R(2, 9); //TODO: Fail option (primes)

            // -- reduce --
            FractionLite a = FractionUtils.Reduce(f.numerator*mult, f.denominator*mult);

            topText = $@"Reduce{qb.br()}{f.numerator*mult}/{f.denominator*mult}{qb.br()}to its lowest terms";
            //botText.Add(new qChunk(id, 0, $@"{a.numerator}/{a.denominator}", false));

            //qb.ChunkRowToColumnInteger( this );
            //qb.possibleAnswerFromColumn(this, );

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionCancel : Question {
    public qFractionCancel(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // numberRange:  {1,3},{2,7},{2,4},{2,5} }

            // -- generate --
            int denom1 = 0, denom2 = 0, num1 = 1, num2 = 1;
            char op = qParams.ops[Utils.R(0, qParams.ops.Count()-1)];
            num1 = Utils.R(qParams.numberRanges[0, 0], qParams.numberRanges[0, 1]);
            while (denom1<=num1)
                denom1= Utils.R(qParams.numberRanges[1, 0], qParams.numberRanges[1, 1]);
            num2 = denom1*Utils.R(qParams.numberRanges[2, 0], qParams.numberRanges[2, 1]);
            denom2= num1*Utils.R(qParams.numberRanges[3, 0], qParams.numberRanges[3, 1]);

            // -- cancel --
            topText = $@"Cancel out{qb.br()}{num1}/{denom1} {op} {num2}/{denom2}";
            FractionLite a = FractionUtils.Reduce(num1, denom2);
            FractionLite b = FractionUtils.Reduce(num2, denom1);
            //botText.Add(new qChunk(id, 0, $@"{a.numerator}", false));
            //botText.Add(new qChunk(id, 1, $@"/", true));
            //botText.Add(new qChunk(id, 2, $@"{b.denominator}", false));
            //botText.Add(new qChunk(id, 3, $@" {op} ", true));
            //botText.Add(new qChunk(id, 4, $@"{b.numerator}", false));
            //botText.Add(new qChunk(id, 5, $@"/", true));
            //botText.Add(new qChunk(id, 6, $@"{a.denominator}", false));

            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            //qb.possibleAnswerFromColumn(this, );

            askBitmap=askBuilder.Commit();
        }
    }

    public class qFractionBasicArithmetic : Question {
        public qFractionBasicArithmetic(int id, qParameters qParams) : base(id, qParams) { }
        public List<string> MakeFractions(QuestionBuilder qb) { //qParams.terms must be even #
            //TODO: in theory I could duoble it..
            List<string> rtn = new List<string>();
            string integer = string.Empty;
            int num = 0;
            int den = 0;
            string sign = string.Empty;
            int[] intRange = qb.RangeSampleInt();
            for (int i = 0; i<qb.rangeInt.Count(); i++) {
                num=qb.rangeInt[i]; //always smaller if proper
                i++;
                den=num+qb.rangeInt[i];
                if (qParams.negatives)
                    sign=qb.Maybe('-');
                if (qParams.mixed)
                    integer=qb.Maybe(intRange[Utils.R(0, intRange.Count()-1)]+" ");
                rtn.Add($@"{sign}{integer}{num}/{den}");
            }
            return rtn;
        }

        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);

            string question = string.Empty;
            List<string> terms = new List<string>();

            // -- Generate term fractions --
            qb.rangeInt=qb.RangeSampleInt(); //qParams.terms = 2*terms
            terms=MakeFractions(qb);

            StringBuilder sbGroup = new StringBuilder();
            string f1 = string.Empty;
            bool brackets, bracketting, bracketted = false;
            bool again = true;
            char op;
            int failCount = 0;

            // -- build groups --
            while (terms.Count>1) {

                failCount=0;
                again=true;
                brackets = false;
                bracketting=false;
                List<string> used = new List<string>();

                int groupCount = Utils.R(2, terms.Count);
                sbGroup.Clear();
                used.Clear();

                // -- build group ---------------------------
                if (qParams.brackets)
                    brackets = qb.Maybe(true, 0);

                while (again && groupCount>0 && failCount <100) {

                    // -- brackets --
                    if (brackets && !bracketting) {
                        sbGroup.Append("(");
                        bracketting=true;
                        bracketted=true;
                    }

                    // -- grab a fraction --
                    f1=terms[0];
                    terms.RemoveAt(0);
                    used.Add(f1);
                    sbGroup.Append(f1).Append(" ");
                    groupCount--;

                    if (groupCount>0) {

                        //  -- Choose op --
                        op = qb.ChooseOp();
                        sbGroup.Append(op).Append(" ");
                    } else {

                        // -- last term --
                        if (brackets)
                            sbGroup.Append(")");

                        // -- Checks ---------------------------------------------
                        if (failCount!=100) {
                            again=false;

                            FractionTermOpEvaluator tmpToc = new FractionTermOpEvaluator();
                            tmpToc.term = sbGroup.ToString();
                            tmpToc.ParseToList();
                            decimal dCheck = tmpToc.Evaluate();
                            // -- Decimals --
                            //if(!again&& !qParams.decimals && dCheck % 1.0m != 0.0m)
                            //again=true;

                            // -- Divide by zero 
                            if (!again && dCheck ==0.0m)
                                again=true;

                            // -- Put 'em back in if it's failed --
                            if (again) {
                                failCount++;
                                terms.AddRange(used);
                                used.Clear();
                                sbGroup.Clear();
                                groupCount = Utils.R(2, terms.Count);
                                brackets=false;
                                bracketted=false;
                            }

                        } else {
                            // -- clear the slate if it's failed --
                            //Console.WriteLine(" (--------- Failed ---------)");
                            terms=MakeFractions(qb);
                            used.Clear();
                            sbGroup.Clear();
                            groupCount = Utils.R(2, terms.Count);
                            brackets=false;
                            bracketted=false;
                        }
                    }
                }
                if (terms.Count==1 && qParams.brackets && !bracketted) {
                    terms.Add("("+sbGroup.ToString()+")");
                } else {
                    terms.Add(sbGroup.ToString());
                }
            }
            question = terms[0].Trim();

            FractionTermOpEvaluator toc = new FractionTermOpEvaluator();
            toc.term = question;
            toc.ParseToList();
            qb.answer = toc.Evaluate().ToString();

            topText = question;
            //botText.Add(new qChunk(id, 0, qb.answer, false));

            //qb.ChunkRowToColumnInteger( this );
        }
    }

    public class qDecimalLongMultiply : Question {
        public qDecimalLongMultiply(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal[] rangeDec = qb.RangeSampleDec();//numberRanges, terms, mantissa
            // -- calculate --
            decimal ans = rangeDec[0];
            qb.sbTop.Append(ans).Append(" * ");

            for (int i = 1; i<rangeDec.Count(); i++) {
                qb.sbTop.Append(rangeDec[i]).Append(" * ");
                ans*=rangeDec[i];
            }

            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            topText = $@"{qb.sbTop}";
            topText = topText.Substring(0, topText.Length-3);
            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //qb.possibleAnswerFromColumn(this, qb.ToColumnInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimalLongDivide : Question {
        public qDecimalLongDivide(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal[] rangeDec = qb.RangeSampleDec();//numberRanges, terms, mantissa

            if (qParams.brevity)
                for (int i = rangeDec.Count()-1; i>0; i--)
                    rangeDec[i-1]*=(Utils.R(2, 5)*rangeDec[i]);

            // -- calculate --
            decimal ans = rangeDec[0];
            qb.sbTop.Append(ans).Append(" / ");
            for (int i = 1; i<rangeDec.Count(); i++) {
                qb.sbTop.Append(rangeDec[i]).Append(" / ");
                ans/=rangeDec[i];
            }

            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            topText = $@"{qb.sbTop}";
            topText = topText.Substring(0, topText.Length-2);

            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //qb.possibleAnswerFromColumn(this, qb.ToColumnInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }


    public class qRatioDistribute : Question {
    public qRatioDistribute(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            qb.rangeInt=qb.RangeSampleInt(); //qParams.terms=4
            int distriband = qb.RangeSampleInt(0)[0];
            int biggest = 0;
            int smallest = Int32.MaxValue;

            // -- calculate --
            qb.sbTop.Append($@"Goods are distributed ");
            for (int i = 0; i<qb.rangeInt.Count()-1; i++) {
                qb.sbTop.Append($@"{qb.rangeInt[i]}:");
                if (qb.rangeInt[i]<smallest)
                    smallest=qb.rangeInt[i];
                if (qb.rangeInt[i]>biggest)
                    biggest=qb.rangeInt[i];
            }
            qb.sbTop.Append(qb.rangeInt.Last());
            askBuilder.AddTextDraw(qb.sbTop.ToString(), qb.alphaFont, new Point(0, 0));

            if (qb.rangeInt.Last()<smallest)
                smallest=qb.rangeInt.Last();
            if (qb.rangeInt.Last()>biggest)
                biggest=qb.rangeInt.Last();

            //Different ways of asking:
            if (qb.Maybe(1, 0)==1) {       // biggest
                bool answered = false;
                //qb.sbTop.Append($@".{qb.br( )}If the biggest gets{qb.br( )}{distriband*biggest}, what do the rest get?");
                askBuilder.AddTextDraw($@".{qb.br()}If the biggest gets{qb.br()}{distriband*biggest},", qb.alphaFont, new Point(0, 17));
                askBuilder.AddTextDraw("what do the rest get?", qb.alphaFont, new Point(0, 34));
                for (int i = 0; i<qb.rangeInt.Count()-1; i++) {
                    if (qb.rangeInt[i]==biggest)
                        answered=true;
                    //botText.Add(new answerChunk(1,2, $@"{qb.rangeInt[i]*distriband}", answered));
                    //botText.Add(new answerChunk(1,2, ", ", true));
                    //ansColumns.Add(qb.ToColumnInteger($@"{qb.rangeInt[i]*distriband}"));
                    ansColumns.Add(qb.CommaColumn());
                    answered=false;
                }
            } else if (qb.Maybe(1, 0)==1) { // smallest
                bool answered = false;
                //qb.sbTop.Append($@".{qb.br( )}If the smallest gets{qb.br( )}{distriband*smallest}, what do the rest get?");
                askBuilder.AddTextDraw($@".{qb.br()}If the smallest gets{qb.br()}{distriband*smallest},", qb.alphaFont, new Point(0, 17));
                askBuilder.AddTextDraw("what do the rest get?", qb.alphaFont, new Point(0, 34));
                for (int i = 0; i<qb.rangeInt.Count()-1; i++) {
                    if (qb.rangeInt[i]==smallest)
                        answered=true;
                    //botText.Add(new answerChunk(1,2, $@"{qb.rangeInt[i]*distriband}", answered));
                    //botText.Add(new answerChunk(1,2, ", ", true));
                    //ansColumns.Add(qb.ToColumnInteger($@"{qb.rangeInt[i]*distriband}"));
                    ansColumns.Add(qb.CommaColumn());
                    answered=false;
                }
            } else {                   // first
                //qb.sbTop.Append($@".{qb.br( )}If the first gets{qb.br( )}{distriband*qb.rangeInt[0]}, what do the rest get?");
                askBuilder.AddTextDraw($@".{qb.br()}If the firstgets{qb.br()}{distriband*qb.rangeInt[0]},", qb.alphaFont, new Point(0, 17));
                askBuilder.AddTextDraw("what do the rest get?", qb.alphaFont, new Point(0, 34));
                //botText.Add(new answerChunk(1,2, $@"{qb.rangeInt[0]*distriband}"+", ", true));
                //ansColumns.Add(qb.ToColumnInteger($@"{qb.rangeInt[0]*distriband}"));
                for (int i = 1; i<qb.rangeInt.Count()-1; i++) {
                    //botText.Add(new answerChunk(1,2, $@"{qb.rangeInt[i]*distriband}", false));
                    //botText.Add(new answerChunk(1,2, ", ", true));
                    //ansColumns.Add(qb.ToColumnInteger($@"{qb.rangeInt[i]*distriband}"));
                    ansColumns.Add(qb.CommaColumn());
                }
            }

            Hints="int * denom : numerator";
            // -- return --
            topText=$@"{qb.sbTop}";
            //qb.ChunkRowToColumnInteger( this );
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qProportionRecipe : Question {
        public qProportionRecipe(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            qb.rangeInt=qb.RangeSampleInt();
            // -- calculate --
            // -- return --
            // 
            // a {recipe} to feed {n} people calls for
            // /n {n}{measure} {ingredient},
            // /n {n}{measure} {ingredient},
            // /n {n}{measure} {ingredient},
            // /n {n}{measure} {ingredient},
            // /n {n}{measure} {ingredient},
            // /n {n}{measure} {ingredient}.
            // /n
            // /nWHow much of each ingredient to feed {m} people?
            //  
            // /n {a}{measure} {ingredient},
            // /n {a}{measure} {ingredient},
            // /n {a}{measure} {ingredient},
            // /n {a}{measure} {ingredient},
            // /n {a}{measure} {ingredient},
            // /n {a}{measure} {ingredient}.
            // 
            // struct ingredient{
            //   string name    //egg teaspooen
            //   int AtomicInit //1   1/2
            // }
            // 
            // Recipe
            //  KVP<string, int> ingredient
            //  List<Ingredient> ingredients
            //  int numPeople
            // 
            // recipe for n
            // how much for (work out m using multiples based off atomics)
            //



            topText="Not done yet";
            //botText.Add(new qChunk(1, 2, "Not done yet", false));
            //qb.ChunkRowToColumnInteger( this );
            //qb.possibleAnswerFromColumn(this, );

            askBitmap=askBuilder.Commit();
        }
    }


    public class qPowersSquaresCubes : Question {
        public qPowersSquaresCubes(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double _base = Utils.R(2, 20);
            double power = Utils.R(2, 3);
            qb.sbTop.Append(_base);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power}"));

            qb.answer=$@"{Math.Pow(_base, power)}";
            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));

            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //qb.possibleAnswerFromColumn(this, qb.ToColumnInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersPlus : Question {
        public qPowersPlus(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //TODO Consolidate into one question
            //     Shrinky-growy the number of terms
            //     Use the parameter class and the qb.rangeInt function
            //char op=qb.ChooseOp( );
            //decimal ans = 0m;
            //switch (op){
            //    case '+': ans=sfNum+num; break;
            //    case '-': ans=sfNum-num; break;
            //    case '*': ans=sfNum*num; break;
            //    case '\\':ans=sfNum/num; break;
            //    default: break;
            //}

            double _base1 = Utils.R(2, 5);
            double power1 = Utils.R(2, 5);
            double _base2 = Utils.R(2, 5);
            double power2 = Utils.R(2, 5);
            qb.sbTop.Append(_base1);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power1}"));
            qb.sbTop.Append(" + ");
            qb.sbTop.Append(_base2);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power2}"));

            qb.answer = $@"{(Math.Pow(_base1, power1) + Math.Pow(_base2, power2))}";

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //qb.possibleAnswerFromColumn(this, qb.ToColumnInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersMinus : Question {
        public qPowersMinus(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double _base1 = Utils.R(2, 5);
            double power1 = Utils.R(2, 5);
            double _base2 = Utils.R(2, 5);
            double power2 = Utils.R(2, 5);
            qb.sbTop.Append(_base1);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power1}"));
            qb.sbTop.Append(" - ");
            qb.sbTop.Append(_base2);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power2}"));

            qb.answer = $@"{(Math.Pow(_base1, power1) - Math.Pow(_base2, power2))}";

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToColumnInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //qb.possibleAnswerFromColumn(this, qb.ToColumnInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }

    public class qSurdsMultiplyOld : Question {
        public qSurdsMultiplyOld(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //TODO: signs
            int x = Utils.NotSq(2, 5);
            int y = Utils.NotSq(2, 10);
            int a = Utils.R(2, 5);
            int b = Utils.R(2, 5);
            char op1 = qb.ChooseOp();
            char op2 = qb.ChooseOp();
            switch (Utils.R(1, 4)) {
                case 1:// √x(a +/- √y)
                    qb.sbTop.Append($@"√{x}({a}{op1}√{y})");
                    Hints="√x*√y = √(x*y)";
                    //√x*a op1 √x*√y
                    //qb.Maybe square a
                    //  √xa op √y
                    int surds = x*y;
                    // simplify surds
                    if (true) {//numeric(surds)){
                        if (op1 =='+') {
                            qb.answer= $@"√{x}{a+surds}";
                        } else if (op1=='-') {
                            qb.answer= $@"√{x}{a-surds}";
                        }
                    } else {
                        if (x==surds) {
                            qb.answer= $@"√{x}{a} op1 √{surds}";
                            if (op1 =='+') {
                            } else if (op1=='-') {
                            }
                        } else {
                        }
                    }

                    break;
                case 2:// (√x +/- a)(√y +/- b)
                    qb.sbTop.Append($@"(√{x}{op1}{a})(√{y}{op2}{b})");
                    if (x==y) {
                        Hints="√";
                    } else {
                        Hints="√";
                    }
                    break;
                case 3:// (√x +/- a)²
                    qb.sbTop.Append($@"(√{x}{op1}{a})²");
                    Hints="√";
                    break;
                case 4:// (a +/- √x)(b +/- √x)
                    qb.sbTop.Append($@"({a}{op1}√{x})({b}{op2}√{y})");
                    if (x==y) {
                        Hints="√";
                    } else {
                        Hints="√";
                    }
                    break;
                default:
                    break;
            }
            qb.answer="yeah anyway";

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }


}
