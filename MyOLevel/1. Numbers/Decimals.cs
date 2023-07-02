using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qDecimalAdd : Question {
        public qDecimalAdd(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal[] rangeDec = qb.RangeSampleDec();
            int padLen = Math.Max(5, rangeDec.Select(x => (""+x).Length).Max());
            decimal ans = rangeDec.Sum();
            string op = "+";

            qb.sbTop.Append((""+rangeDec.First()).PadLeft(padLen, ' '));
            askBuilder.AddTextDraw((""+rangeDec.First()).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, 0));
            int i = 0;
            for (i = 1; i<qParams.terms-1; i++) {
                qb.sbTop.Append("\n"+(""+rangeDec[i]).PadLeft(padLen, ' '));
                askBuilder.AddTextDraw((""+rangeDec[i]).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, i*qb.lineSpace));
            }
            qb.sbTop.Append("\n"+op+(""+rangeDec.Last()).PadLeft(padLen-1, ' '));
            askBuilder.AddTextDraw(op+(""+rangeDec.Last()).PadLeft(padLen-1, ' '), qb.alphaFont, new Point(0, i*qb.lineSpace));

            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, ++i*qb.lineSpace));
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimalSubtract : Question {
        public qDecimalSubtract(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal[] rangeDec = qb.RangeSampleDec();
            int padLen = Math.Max(5, rangeDec.Select(x => (""+x).Length).Max());
            decimal ans = rangeDec[0];
            string op = "-";

            qb.sbTop.Append((""+rangeDec.First()).PadLeft(padLen, ' '));
            askBuilder.AddTextDraw((""+rangeDec.First()).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, 0));
            int i = 0;
            for (i=1; i<qParams.terms-1; i++) {
                qb.sbTop.Append("\n"+(""+rangeDec[i]).PadLeft(padLen, ' '));
                ans-=rangeDec[i];
                askBuilder.AddTextDraw((""+rangeDec[i]).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, i*qb.lineSpace));
            }
            qb.sbTop.Append("\n"+op+(""+rangeDec.Last()).PadLeft(padLen-1, ' '));
            askBuilder.AddTextDraw(op+(""+rangeDec.Last()).PadLeft(padLen-1, ' '), qb.alphaFont, new Point(0, i*qb.lineSpace));
            ans-=rangeDec.Last();

            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, ++i*qb.lineSpace));
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimalMultiply : Question {
        public qDecimalMultiply(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal dec = qb.RangeSampleDec()[0];
            int integer = qb.RangeSampleInt()[0];
            int padLen = new int[] { 5, (""+dec).Length, (""+(decimal)integer).Length }.Max();
            string op = "*";

            qb.sbTop.Append((""+dec).PadLeft(padLen, ' '))
                 .Append("\n"+op+((""+integer).PadLeft(padLen-1, ' ')));
            askBuilder.AddTextDraw((""+dec).PadLeft(padLen, ' '), qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw(op+((""+integer).PadLeft(padLen-1, ' ')), qb.alphaFont, new Point(0, qb.lineSpace));

            decimal ans = dec*integer;

            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*2));
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimalDivide : Question {
        public qDecimalDivide(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal dec = qb.RangeSampleDec()[0];
            int integer = qb.RangeSampleInt()[0];
            int padLen = new int[] { 5, (""+dec).Length, (""+integer).Length }.Max();
            string op = @"\";

            qb.sbTop.Append($@"{dec} {op} {integer}");
            askBuilder.AddTextDraw($@"{dec} {op} {integer}", qb.alphaFont, new Point(0, 0));
            decimal ans = Math.Round(dec/(decimal)integer, 2);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.sbTop.Append(qb.br()).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace));
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFraction2Decimal : Question {
        public qFraction2Decimal(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(200, 50);
            var ansColumnss = new List<qColumn>();
            bool IsMixed = false;
            var colWhole = new qColumn();

            qb.rangeInt=qb.RangeSampleInt();
            // -- calculate --
            decimal ans = 0.0m;
            if (!qParams.mixed) {
                ans = Utils.Dr(0, 0, qParams.decimalPoints);
            } else {
                ans = Utils.Dr(qb.rangeInt[0], qb.rangeInt[1], qParams.decimalPoints);
                IsMixed=true;
            }
            Fraction fr = new Fraction(ans);
            var colFraction = qb.ToColumnFraction(fr);

            MyFormat qa = Utils.MyDecimal(ans, DP.nice|DP.prompt);
            qb.answer=qa.answer;
            qb.sbTop.Append($@"Convert{qb.br()}{fr.ToMixed()}{qb.br()}to a decimal")
                 .Append(qb.br()).Append(qa.dp);
            topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(1,2,qa.answer, false));

            if (IsMixed) {
                ansColumnss.Add(qb.ToSingleInteger(""+fr.whole));
            }
            ansColumnss.Add(colFraction);
            askBuilder.AddTextDraw("Convert", qb.alphaFont, new Point(0, 10));
            askBuilder.AddColumns(ansColumnss, new Point(70, 0));
            askBuilder.AddTextDraw("to a decimal", qb.alphaFont, new Point(105, 10));

            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(""+qa.answer, ""));
            //possAnswer = new possibleAnswer();
            //possAnswer.answer.Add(qb.ToColumnFraction(""+qa.answer, ""));
            //possibleAnswers.Add(possAnswer);


            //var mptd = askBuilder.NewMultiPartTextDraw();
            //mptd.Add( askBuilder.NewTextDraw( "Convert", qb.alphaFont, new Point(0, 10)));
            //mptd.Add( askBuilder.NewTextDraw( $@"{fr.ToMixed()}", qb.numericFont, new Point(0, 0)));
            //mptd.Add( askBuilder.NewTextDraw( "to a decimal", qb.alphaFont, new Point(0, 10)));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimal2Fraction : Question {
        public qDecimal2Fraction(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(200, 50);

            qb.rangeInt=qb.RangeSampleInt();
            // -- calculate --
            decimal dec = 0.0m;
            if (!qParams.mixed) {
                dec = Utils.Dr(0, 0, qParams.decimalPoints);
            } else {
                dec = Utils.Dr(qb.rangeInt[0], qb.rangeInt[1], qParams.decimalPoints);
            }
            Fraction fr = new Fraction(dec);
            qb.answer = $@"{fr}";//new code
            // -- return --
            topText=$@"Convert {dec} to a fraction";
            //askBuilder.AddColumn(qb.ToColumnFraction(topText, ""), new Point(65, 0));
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(fr));

            //possAnswer = new possibleAnswer();
            //possAnswer.answer.Add(qb.ToColumnFraction(fr));
            //possibleAnswers.Add(possAnswer);

            askBitmap=askBuilder.Commit();
        }
    }
}
