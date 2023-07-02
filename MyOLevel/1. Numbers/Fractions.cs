using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qFractionMultiplyUp : Question {
        public qFractionMultiplyUp(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var askColumns = new List<qColumn>();
            // -- get fraction --
            qb.rangeInt=qb.RangeSampleInt(); //numberRange==1,10; terms==1
            int numerator = Utils.R(1, qb.rangeInt[0]);
            FractionLite f = FractionUtils.Reduce(numerator, numerator+Utils.R(1, 10)); //Proper

            // -- multiply --
            int mult = Utils.R(2, 9);
            topText = $@"{f.numerator}/{f.denominator} == ?/{f.denominator*mult}";

            askColumns.Add(qb.ToColumnFraction(new FractionLite(f.numerator, f.denominator), false, true));
            askColumns.Add(qb.OpColumn("="));
            askColumns.Add(qb.ToColumnFraction("?", ""+f.denominator*mult, true, true, true));

            var ans = qb.ToColumnFraction(new FractionLite(f.numerator*mult, f.denominator*mult), false, true);

            //askBuilder = new BitmapBuilder(200, 100);
            askBuilder.AddColumns(askColumns, new Point(0, 0));

            qb.possibleAnswerFromColumn(this, ans);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionDivideDown : Question {
        public qFractionDivideDown(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            var queColumns = new List<qColumn>();
            // -- get fraction --
            qb.rangeInt=qb.RangeSampleInt(); //numberRange==1,10; terms==1

            int numerator = Utils.R(1, qb.rangeInt[0]);
            FractionLite f = FractionUtils.Reduce(numerator, numerator+Utils.R(1, 10)); //Proper

            // -- multiply --
            int mult = Utils.R(2, 9);
            topText = $@"{f.numerator*mult}/{f.denominator*mult} == ?/{f.denominator}";

            queColumns.Add(qb.ToColumnFraction(new FractionLite(f.numerator*mult, f.denominator*mult), false, true));
            queColumns.Add(qb.OpColumn("="));
            queColumns.Add(qb.ToColumnFraction("?", ""+f.denominator, true, true, true));

            //askBuilder = new BitmapBuilder(200, 100);
            askBuilder.AddColumns(queColumns, new Point(0, 0));

            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(f, false, true));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionToMixed : Question {
        public qFractionToMixed(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();

            int whole = Utils.R(1, 5);
            int numerator = qb.RangeSampleInt()[0]; //Basic: numberRange==1,10; terms==1
            int denominator = Utils.R(1, 4)+numerator;

            Fraction ask = new Fraction((whole*denominator)+numerator, denominator);

            FractionLite fractionalReduced = FractionUtils.Reduce(numerator, denominator);

            var askColumn = qb.ToColumnFraction(ask);

            ansColumns.Add(qb.ToSingleInteger(""+whole));
            ansColumns.Add(qb.ToColumnFraction(new FractionLite(fractionalReduced.numerator, fractionalReduced.denominator)));
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBuilder.AddTextDraw("Express", qb.alphaFont, new Point(0, 10));
            askBuilder.AddColumn(askColumn, new Point(75, 0));
            askBuilder.AddTextDraw("as a mixed number", qb.alphaFont, new Point(125, 10));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionTopHeavy : Question {
        public qFractionTopHeavy(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(270, 80);
            var ansColumns = new List<qColumn>();

            // -- get fraction --
            qb.rangeInt=qb.RangeSampleInt(); //numberRange==1,10; terms==1

            int whole = Utils.R(1, 5);
            int numerator = Utils.R(1, qb.rangeInt[0]);
            FractionLite fractionalReduced = FractionUtils.Reduce(numerator, numerator+Utils.R(1, 10));
            Fraction f = new Fraction(whole, fractionalReduced.numerator, fractionalReduced.denominator); //whole + proper

            ansColumns.Add(qb.ToSingleInteger(""+whole));
            ansColumns.Add(qb.ToColumnFraction(new FractionLite(fractionalReduced.numerator, f.denominator)));

            var ac = qb.ToColumnFraction(f);

            topText = $@"Express{qb.br()}{f.ToMixed()}{qb.br()}as a top heavy fraction";
            //botText.Add(new qChunk(id, 0, $@"{(whole>0 ? whole : 1)*f.denominator+fractionalReduced.numerator}/{f.denominator}", false));

            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(new FractionLite(f.numerator, f.denominator)));
            //qb.ChunkRowToSingleInteger( this );
            //possAnswer = new possibleAnswer(); // I'm ( one ) answer...
            //possAnswer.answer.Add(qb.ToColumnFraction(new FractionLite(f.numerator, f.denominator)));
            //possibleAnswers.Add(possAnswer);

            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //askBuilder.AddTextLines(topText, qb.brCount+1, qb.font, new Point(0, 0));
            askBuilder.AddTextDraw("Express", qb.alphaFont, new Point(0, 10));
            askBuilder.AddColumns(ansColumns, new Point(75, 0));
            askBuilder.AddTextDraw("as a top heavy fraction", qb.alphaFont, new Point(145, 10));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionLowestCommonDenominator : Question {
        public qFractionLowestCommonDenominator(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int i = 0, j = 0, k = 0;

            // -- generate high denom
            int denom = (Utils.R(1, 5)+qParams.terms) * 10;

            // generate tight group of numerators in sequence (roughly half way onwards)
            int numStart = (int)(denom*0.33)+Utils.R(1, (int)(denom*0.25)); //concentrate
            List<int> seq = new List<int>();
            seq.Add(numStart);
            int next = 0;
            for (i=1; i<qParams.terms; i++) {
                do { // -- generate next numerator --
                    next=seq[i-1]+Utils.R(1, 4); //1..4 apart
                } while (next<seq[i-1]);
                seq.Add(next);
            }

            // -- (create then shuffle some indices) --
            int[] ords = new int[seq.Count()];
            ords[0]=Utils.R(1, seq.Count());
            for (i=1; i<seq.Count(); i++) {
                for (k=ords[i-1];
                    ords.Contains(k);
                    k=Utils.R(1, ords.Count())
                    //,MessageBox.Show($@"ords[0]={ords[0]}{"\n"}ords[i]={ords[i]}{"\n"}ords.Contains(ords[i])={ords.Contains(ords[i])}")
                    ) ;
                ords[i]=k;
            } //3,1,2,4

            // -- fractionify & reduce --
            var fTerms = new List<FractionLite>();
            FractionLite fTerm = new FractionLite();
            for (i=0; i<seq.Count(); i++) {
                fTerm=FractionUtils.Reduce(seq[i], denom);
                fTerms.Add(fTerm);
            }

            // -- pose question in shuffled order --
            var shuffled = new List<qColumn>();
            for (i=0, j=1, k=0; i<seq.Count(); i++) {
                while (ords[k]!=j)
                    k++;
                shuffled.Add(qb.ToColumnFraction(new FractionLite(fTerms[k].numerator, fTerms[k].denominator)));
                k=0;
                j++;
            }

            // -- arrange answer in sequence order --
            possAnswer = new possibleAnswer<qColumn>();
            for (i=0; i<seq.Count()-1; i++) {
                possAnswer.answer.Add(qb.ToColumnFraction(fTerms[i]));
                possAnswer.answer.Add(qb.CommaColumn());
            }
            possAnswer.answer.Add(qb.ToColumnFraction(fTerms[seq.Count()-1]));

            topText = $@"Arrange low to high:{qb.br()}";
            topText = topText.Substring(0, topText.Length);

            possAnswer.IsSequence = true;
            possAnswer.uniformSize = true;
            possibleAnswers.Add(possAnswer);

            //askBuilder = new BitmapBuilder(200, 100);
            //askBuilder.OldAddTextLine(topText, qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            askBuilder.AddColumns(shuffled, new Point(0, 40));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionAddition : Question {
        public qFractionAddition(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();


            //askBuilder = new BitmapBuilder(qParams.terms*55, 50);

            // numberRange:  {1,3},{2,7},{2,4},{2,5} }

            // -- generate --
            int i = 0;
            int num = 1;
            int denom = 0;
            Fraction[] fractions = new Fraction[qParams.terms];
            var askColumns = new List<qColumn>();


            for (i=0; i<qParams.terms; i++) {
                num = Utils.R(qParams.numberRanges[0, 0], qParams.numberRanges[0, 1]);
                while (denom<=num)
                    denom= Utils.R(qParams.numberRanges[1, 0], qParams.numberRanges[1, 1]);
                fractions[i]=new Fraction(num, denom);

                askColumns.Add(qb.ToColumnFraction(new FractionLite(num, denom)));

                //qb.sbTop.Append($@"{num}/{denom}");
                //que.answerColumns.Add(qb.ToColumnFraction(fractions[i]));

                if (i<qParams.terms-1) {
                    //qb.sbTop.Append(" + ");
                    askColumns.Add(qb.OpColumn("+"));
                }
                num=1;
                denom=0;
            }
            //topText = $@"{qb.sbTop}";





            Fraction ans = fractions[0];
            for (i=1; i<fractions.Count(); i++)
                ans+=fractions[i];
            Hints="Multiply, multiply, add, reduce";
            // -- return --
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(ans));

            //possAnswer = new possibleAnswer(); // I'm ( one ) answer...
            //possAnswer.answer.Add(qb.ToColumnFraction(ans));
            //possibleAnswers.Add(possAnswer);

            askBuilder.AddColumns(askColumns, new Point(0, 0));
            

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionSubtract : Question {
        public qFractionSubtract(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(qParams.terms*55, 50);
            // numberRange:  {1,3},{2,7},{2,4},{2,5} }
            // -- generate --
            int i = 0;
            int num = 1;
            int denom = 0;
            Fraction[] fractions = new Fraction[qParams.terms];
            var queColumns = new List<qColumn>();
            for (i=0; i<qParams.terms; i++) {
                num = Utils.R(qParams.numberRanges[0, 0], qParams.numberRanges[0, 1]);
                while (denom<=num)
                    denom= Utils.R(qParams.numberRanges[1, 0], qParams.numberRanges[1, 1]);
                fractions[i]=new Fraction(num, denom);
                queColumns.Add(qb.ToColumnFraction(new FractionLite(num, denom)));
                //qb.sbTop.Append($@"{num}/{denom}");
                if (i<qParams.terms-1) {
                    //qb.sbTop.Append(" - ");
                    queColumns.Add(qb.OpColumn("-"));
                }
                num=1;
                denom=0;
            }
            //topText = $@"{qb.sbTop}";
            Fraction ans = fractions[0];
            for (i=1; i<fractions.Count(); i++)
                ans-=fractions[i];
            if (ans.numerator<0 || ans.whole<0) {
                Hints="It's negative";
            } else {
                Hints="Multiply, multiply, subtract, reduce";
            }
            // -- return --
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(ans));
            //possAnswer = new possibleAnswer(); // I'm ( one ) answer...
            //possAnswer.answer.Add(qb.ToColumnFraction(ans));
            //possibleAnswers.Add(possAnswer);

            askBuilder.AddColumns(queColumns, new Point(0, 0));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionMultiply : Question {
        public qFractionMultiply(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(qParams.terms*55, 50);
            // numberRange:  {1,3},{2,7},{2,4},{2,5} }
            // -- generate --
            int i = 0;
            int num = 1;
            int denom = 0;
            Fraction[] fractions = new Fraction[qParams.terms];
            var queColumns = new List<qColumn>();
            for (i=0; i<qParams.terms; i++) {
                num = Utils.R(qParams.numberRanges[0, 0], qParams.numberRanges[0, 1]);
                while (denom<=num)
                    denom= Utils.R(qParams.numberRanges[1, 0], qParams.numberRanges[1, 1]);
                fractions[i]=new Fraction(num, denom);
                //qb.sbTop.Append($@"{num}/{denom}");
                //TODO:
                queColumns.Add(qb.ToColumnFraction(new FractionLite(num, denom)));

                if (i<qParams.terms-1) {
                    //qb.sbTop.Append(" * ");
                    queColumns.Add(qb.OpColumn("*"));
                }
                num=1;
                denom=0;
            }
            //topText = $@"{qb.sbTop}";
            Fraction ans = fractions[0];
            for (i=1; i<fractions.Count(); i++)
                ans*=fractions[i];

            Hints="Multiply, multiply, reduce";
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(ans));
            //possAnswer = new possibleAnswer(); // I'm ( one ) answer...
            //possAnswer.answer.Add(qb.ToColumnFraction(ans));
            //possibleAnswers.Add(possAnswer);

            //askBuilder.AddTextLines(topText, qb.brCount+1, qb.font, new Point(0, 0));
            askBuilder.AddColumns(queColumns, new Point(0, 0));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionDivision : Question {
        public qFractionDivision(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // numberRange:  {1,3},{2,7},{2,4},{2,5} }
            // -- generate --
            int i = 0;
            int num = 1;
            int denom = 0;
            Fraction[] fractions = new Fraction[qParams.terms];
            var queColumns = new List<qColumn>();
            for (i=0; i<qParams.terms; i++) {
                num = Utils.R(qParams.numberRanges[0, 0], qParams.numberRanges[0, 1]);
                while (denom<=num)
                    denom= Utils.R(qParams.numberRanges[1, 0], qParams.numberRanges[1, 1]);
                fractions[i]=new Fraction(num, denom);
                queColumns.Add(qb.ToColumnFraction(new FractionLite(num, denom)));
                //qb.sbTop.Append($@"{num}/{denom}");
                if (i<qParams.terms-1) {
                    //qb.sbTop.Append(" / ");
                    queColumns.Add(qb.OpColumn("/"));
                }
                num=1;
                denom=0;
            }
            //topText = $@"{qb.sbTop}";
            Fraction ans = fractions[0];
            for (i=1; i<fractions.Count(); i++)
                ans/=fractions[i];
            Hints="Invert, multiply, multiply, reduce";

            // -- return --
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(ans));

            askBuilder.AddColumns(queColumns, new Point(0, 0));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFractionOf : Question {
        public qFractionOf(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- int --
            qb.rangeInt=qb.RangeSampleInt();
            decimal whole = qb.rangeInt[0]*10;

            // -- fraction --
            decimal num = Utils.R(qParams.numberRanges[0, 0], qParams.numberRanges[0, 1]);
            decimal denom = 0;
            while (denom<=num)
                denom= Utils.R(qParams.numberRanges[1, 0], qParams.numberRanges[1, 1]);
            Fraction f = new Fraction((int)num, (int)denom);

            // -- answer --
            decimal ans = Math.Round((whole /  denom) *  num, 2);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.mantissa);
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(ans));

            // -- ask --
            var askColumns = new List<qColumn>();
            askColumns.Add(qb.ToColumnFraction(f));
            askColumns.Add(qb.ToSingleChar("of"));
            askColumns.Add(qb.ToSingleInteger(whole));
            askBuilder.AddColumns(askColumns, new Point(0, 0));
            //askBuilder.AddTextDraw(""+qa.dp,qb.alphaFont,new Point(0,55));

            // -- return --
            Hints = $@"( {whole} / {f.denominator} ) * {f.numerator} )";
            askBitmap=askBuilder.Commit();
        }
    }

}
