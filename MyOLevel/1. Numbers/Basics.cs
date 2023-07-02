using System;
using System.Drawing;
using System.Linq;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qBasicAddSubtract : Question {
        public qBasicAddSubtract(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();

            qb.rangeInt = qb.RangeSampleInt();
            char op = qb.ChooseOp();
            int padLen = qb.rangeInt.Select(x => (""+x).Length).Max()+3;
            int i = 0;

            qb.sbTop.Append((""+qb.rangeInt.First()).PadLeft(padLen, ' '));
            askBuilder.AddTextDraw((""+qb.rangeInt.First()).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, 0));
            for (i = 1; i<qParams.terms-1; i++) {
                qb.sbTop.Append(qb.br()+(""+qb.rangeInt[i]).PadLeft(padLen, ' '));
                askBuilder.AddTextDraw((" "+qb.rangeInt[i]).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, qb.lineSpace*i));
            }
            qb.sbTop.Append(qb.br()+op+(""+qb.rangeInt.Last()).PadLeft(padLen-1, ' '));
            askBuilder.AddTextDraw(op+(""+qb.rangeInt.Last()).PadLeft(padLen-1, ' '), qb.alphaFont, new Point(0, qb.lineSpace*i));

            int rslt = qb.rangeInt.First();
            if (op=='+') {
                qb.rangeInt.Skip(1).ToList().ForEach(x => rslt+=x);
            } else {
                qb.rangeInt.Skip(1).ToList().ForEach(x => rslt-=x);
            }
            qb.answer = $@"{rslt}";

            Hints = "Working?";
            //topText=$@"{qb.sbTop}";

            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(""+rslt, ""));
            //possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            //possAnswer.answer.Add(qb.ToColumnFraction(""+rslt,""));
            //possibleAnswers.Add(possAnswer);

            //askBuilder.AddColumn(qb.ToColumnFraction(topText, "", false, true), new Point(0, 0));
            askBitmap=askBuilder.Commit();

        }
    }
    public class qBasicMultiplyDivide : Question {
        public qBasicMultiplyDivide(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            qb.rangeInt = qb.RangeSampleInt(); //terms = 2
            char op = qb.ChooseOp();
            int padLen = qb.rangeInt.Select(x => (""+x).Length).Max()+2;

            int first = qb.rangeInt.First();

            if (op=='*') {
                //qb.sbTop.Append((""+first).PadLeft(padLen,' '));
                //qb.sbTop.Append("\n"+op + (""+qb.rangeInt.Last()).PadLeft(padLen-1, ' '));
                askBuilder.AddTextDraw((""+first).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, 0));
                askBuilder.AddTextDraw(""+op + (""+qb.rangeInt.Last()).PadLeft(padLen-1, ' '), qb.alphaFont, new Point(0, qb.lineSpace));
                qb.answer=""+first*qb.rangeInt.Last();
                askBitmap=askBuilder.Commit();

            } else {
                int mult = Utils.R(2, 12);
                if (qb.Maybe(1, 0)==1)
                    mult*=-1;
                int rslt = first * mult;
                qb.answer = $@"{mult}";
                //qb.sbTop.Append(rslt);
                //qb.sbTop.Append($@" {op} {first}");
                var mptd = askBuilder.NewMultiPartTextDraw();
                mptd.Add(askBuilder.NewTextDraw(""+rslt, qb.alphaFont, new Point(0, 0)));
                mptd.Add(askBuilder.NewTextDraw(""+op+" "+first, qb.alphaFont, new Point(0, 0)));
                askBitmap=askBuilder.Commit();

                // -- hint --
                var hintBuilder = askBuilder.Reset();
                hintBuilder.backColor=Color.White;
                hintBuilder.AddTextDraw(""+first, qb.alphaFont, new Point(0, 3));
                hintBuilder.AddLineDraw(Pens.Black, new Point((""+first).Length*((int)qb.alphaFont.Size)+5, 0),
                                                    new Point((""+first).Length*((int)qb.alphaFont.Size)+5, 20));
                hintBuilder.AddTextDraw(""+rslt, qb.alphaFont, new Point((""+first).Length*((int)qb.alphaFont.Size)+10, 3));
                hintBuilder.AddLineDraw(Pens.Black, new Point((""+first).Length*((int)qb.alphaFont.Size)+5, 0),
                                                    new Point((""+first).Length*((int)qb.alphaFont.Size)+
                                                               (""+rslt).Length*((int)qb.alphaFont.Size)+10, 0));
                //       _______________
                //  rslt |   first
                //       |
                //
                hintsBitmap=hintBuilder.Commit();
            }
            //topText = $@"{qb.sbTop}";


            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(""+qb.answer, ""));
            //possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            //possAnswer.answer.Add(qb.ToColumnFraction(""+qb.answer, ""));
            //possibleAnswers.Add(possAnswer);

            //askBuilder.AddColumn(qb.ToColumnFraction(topText, "", false, true), new Point(0, 0));
        }
    }
    public class qLowestCommonMultiple : Question {
        public qLowestCommonMultiple(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            qb.rangeInt=qb.RangeSampleInt();
            int lcm = Utils.lcm(qb.rangeInt);

            //qb.sbTop.Append($@"Lowest Common Multiple of ");
            askBuilder.AddTextDraw($@"Lowest Common Multiple of", qb.alphaFont, new Point(0, 0));
            foreach (int t in qb.rangeInt.Take(qParams.terms-1))
                qb.sbTop.Append($@"{t}, ");
            qb.sbTop.Append(qb.rangeInt.Last());

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, $@"{lcm}", false));

            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, qb.lineSpace));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(lcm));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimalPlaces : Question {
        public qDecimalPlaces(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- generate number --
            decimal num = Utils.Dr(1, 100, qParams.terms);
            int places = Utils.R(1, qParams.terms-1);

            qb.answer=$@"{Math.Round(num, places)}";
            topText = $@"{num} correct to {places} decimal places";
            //botText.Add(new qChunk(id, 0, qb.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qSignificantFigures : Question {
        public qSignificantFigures(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- generate number --
            int figures = Utils.R(1, qParams.decimalPoints-1);
            decimal num = 0m;
            if (!qParams.decimals || Utils.R(1, 5)==5) {                // int
                num=(decimal)(Math.Pow(10, figures)+Utils.R(1, 1000));
            } else if (qParams.decimals && Utils.R(1, 5)==5) {           // dec<0
                num = Utils.Dr(0, 0, qParams.decimalPoints);
            } else {                                                   // dec>0
                num = Utils.Dr(1, 100, qParams.decimalPoints);
            }

            topText = $@"{num} correct to {figures} significant figures";

            // -- calculate --
            qb.answer=Utils.significantFigures(num, figures);

            //botText.Add(new qChunk(id, 0, qb.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qStandardFormX : Question {
        public qStandardFormX(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal num;
            int numZeros = Utils.R(2, 5);
            if (qb.Maybe(1, 0)==0) {
                //big
                num = Utils.Dr(10, 10000, Utils.R(1, 3));
            } else {
                //small
                string strNum = "0." + string.Join("", Enumerable.Repeat("0", numZeros))+Utils.R(2, 999).ToString();
                decimal.TryParse(strNum, out num);
            }
            int sign = qb.Maybe(1, 0)==1 ? 1 : -1;
            topText = $"What is {num} in standard form?";
            //botText.Add(new qChunk(id, 1, Utils.ToStandardForm(num), false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(GraphicsUtils.ToStandardForm(num)));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qStandardFormSF : Question {
        public qStandardFormSF(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal num;
            int numZeros = Utils.R(2, 5);
            if (qb.Maybe(1, 0)==0) {
                //big
                num = Utils.Dr(10, 10000, Utils.R(1, 3));
            } else {
                //small
                string strNum = "0." + string.Join("", Enumerable.Repeat("0", numZeros))+Utils.R(2, 999).ToString();
                decimal.TryParse(strNum, out num);
            }
            int sign = qb.Maybe(1, 0)==1 ? 1 : -1;
            topText = $"What is {GraphicsUtils.ToStandardForm(num)}?";
            //botText.Add(new qChunk(id, 1, $@"{((double)num)}", false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(""+(double)num));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qStandardFormMixed : Question {
        public qStandardFormMixed(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal sfNum;
            int numZeros = Utils.R(2, 3);
            if (qb.Maybe(1, 0)==0) {
                //big
                sfNum = Utils.Dr(10, 10000, Utils.R(1, 3));
            } else {
                //small
                string strNum = "0." + string.Join("", Enumerable.Repeat("0", numZeros))+Utils.R(2, 999).ToString();
                decimal.TryParse(strNum, out sfNum);
            }
            int sign = qb.Maybe(1, 0)==1 ? 1 : -1;
            decimal num;
            numZeros = Utils.R(2, 4);
            if (qb.Maybe(1, 0)==0) {
                //big
                num = Utils.Dr(10, 10000, Utils.R(1, 3));
            } else {
                //small
                string strNum = "0." + string.Join("", Enumerable.Repeat("0", numZeros))+Utils.R(2, 999).ToString();
                decimal.TryParse(strNum, out num);
            }
            sign =qb.Maybe(1, 0)==1 ? 1 : -1;

            char op = qb.ChooseOp();
            decimal ans = 0m;
            switch (op) {
                case '+': ans=sfNum+num; break;
                case '-': ans=sfNum-num; break;
                case '*': ans=sfNum*num; break;
                case '\\': ans=sfNum/num; break;
                default: break;
            }
            topText = $"What is {GraphicsUtils.ToStandardForm(sfNum)} {op} {num}?";
            //botText.Add(new qChunk(id, 1, $@"{ans}", false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(""+ans));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qStandardFormArithmetic : Question {
        public qStandardFormArithmetic(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            decimal num1, num2;
            string strNum1, strNum2;
            int sign1, sign2, numZeros1, numZeros2;
            char op = qb.ChooseOp();
            decimal[] rangeDec = qb.RangeSampleDec();

            // -- number one
            numZeros1 = Utils.R(2, 3);
            sign1 = qb.Maybe(1, 0)==1 ? 1 : -1;
            if (qb.Maybe(1, 0)==0) {
                //big
                //num1 = Utils.dr(100, 3, Utils.r(1, 3));
                num1= rangeDec.First()*Utils.R(10, 50);
            } else {
                //small
                strNum1 = "0." + string.Join("", Enumerable.Repeat("0", numZeros1))+Utils.R(10, 20).ToString();
                decimal.TryParse(strNum1, out num1);
            }

            // -- number two
            numZeros2 = Utils.R(2, 3);
            sign2 = qb.Maybe(1, 0)==1 ? 1 : -1;
            if (qb.Maybe(1, 0)==0) {
                //big
                num2 = Utils.Dr(100, 3, Utils.R(1, 3));
                num2=rangeDec[1]*Utils.R(10, 50);
            } else {
                //small
                strNum2 = "0." + string.Join("", Enumerable.Repeat("0", numZeros2))+Utils.R(10, 20).ToString();
                decimal.TryParse(strNum2, out num2);
            }

            // -- ask
            askBuilder.AddTextDraw(GraphicsUtils.ToStandardForm(num1), qb.numericFont, new Point(15, 0));
            askBuilder.AddTextDraw(""+op, qb.alphaFont, new Point(0, qb.lineSpace+2));
            askBuilder.AddTextDraw(GraphicsUtils.ToStandardForm(num2), qb.numericFont, new Point(15, qb.lineSpace));

            // -- answer
            decimal ans = 0.0M;
            switch (op) {
                case '/':
                    ans=num1/num2;
                    Hints="Separately";
                    break;
                case '*':
                    ans=num1*num2;
                    Hints="Separately";
                    break;
                case '-':
                    ans=num1-num2;
                    Hints="Align, but not separately";
                    break;
                case '+':
                    ans=num1+num2;
                    Hints="Align, but not separately";
                    break;
            }
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(GraphicsUtils.ToStandardForm(ans)));

            // -- return
            askBitmap=askBuilder.Commit();
        }
    }
    public class qAveragesMean : Question {
        public qAveragesMean(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int[] nums = new int[Utils.R(3, 6)];
            decimal ans = 0m;
            int i = 0;
            for (i=0; i<nums.Length; nums[i]=qb.RangeSampleInt()[0], i++) ;

            //qb.sbTop.Append($@"Find the average of:{qb.br( )}");
            askBuilder.AddTextDraw($@"Find the average of:", qb.alphaFont, new Point(0, 0));
            i=1;
            foreach (var s in nums.Take(nums.Count()-1)) {
                qb.sbTop.Append(s).Append(", ");
                //askBuilder.AddTextDraw($@"{s},", qb.alphaFont, new Point(0, i*17));
                i++;
            }
            qb.sbTop.Append(nums.Last());
            askBuilder.AddTextDraw($@"{qb.sbTop}", qb.alphaFont, new Point(0, qb.lineSpace));

            ans = (decimal)Math.Round((decimal)nums.Sum()/(decimal)nums.Count(), 2);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, 2);
            qb.answer=qa.answer;
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            askBuilder.AddTextDraw($@"{qa.dp}", qb.alphaFont, new Point(0, qb.lineSpace*2));

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }


}
