using System;
using System.Collections.Generic;
using System.Drawing;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qFraction2Percent : Question {
        public qFraction2Percent(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(200, 50);

            qb.rangeInt=qb.RangeSampleInt();
            long num = Utils.R(1, 10);
            long denom = Utils.R(1, 10)*5;
            Fraction f = new Fraction(num, denom);
            decimal ans = Math.Round(f.ToDecimal()*100, qParams.decimalPoints);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            qb.sbTop.Append($@"Convert {f} to a{qb.br()}percentage")
                 .Append(qb.br()).Append(qa.dp);
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id, 0, qa.answer, false));
            //botText.Add(new answerChunk(id, 0, "%", true));
            Hints="(100 / denom) * num";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw("Convert", qb.alphaFont, new Point(0, 10));
            askBuilder.AddColumn(qb.ToColumnFraction(f), new Point(75, 0));
            askBuilder.AddTextDraw("to a percentage", qb.alphaFont, new Point(105, 10));

            ansColumns.Add(qb.ToColumnFraction(qa.answer, ""));
            ansColumns.Add(qb.ToColumnFraction("%", "", true));

            //possAnswer = new possibleAnswer();
            //possAnswer.answer.AddRange(ansList);
            //possibleAnswers.Add(possAnswer);
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercent2Fraction : Question {
        public qPercent2Fraction(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(200, 50);

            qb.rangeInt=qb.RangeSampleInt();
            decimal num = 0m;
            if (qb.Maybe(1, 0)==1) {
                if (qb.Maybe(1, 0)==1) {
                    num=Utils.Dr(1, 9, 1);
                } else {
                    num=Utils.R(1, 9)+(Utils.Dr(0, 0, 1)/10);
                }
            } else {
                num=Utils.R(1, 9);
            }
            topText=$@"Convert {num}% to a fraction";
            Fraction fr = new Fraction(num/100);
            //botText.Add(new answerChunk(id,0,$@"{fr.numerator}/{fr.denominator}",false)); //new code
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(fr));

            //possAnswer = new possibleAnswer();
            //possAnswer.answer.Add(qb.ToColumnFraction(fr));
            //possibleAnswers.Add(possAnswer);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qDecimal2Percent : Question {
        public qDecimal2Percent(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            qb.rangeInt=qb.RangeSampleInt();
            decimal num = 0m;
            if (qb.Maybe(1, 0)==0) {
                num=Utils.Dr(0, 0, 2);
            } else {
                num=(Utils.Dr(0, 0, 1)/10);
            }
            decimal ans = Math.Round(num*100, qParams.decimalPoints);
            //qb.sbTop.Append($@"Convert {num} to a percentage");
            askBuilder.AddTextDraw($@"Convert {num} to a percentage", qb.alphaFont, new Point(0, 0));
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,qa.answer,false));
            //botText.Add(new answerChunk(id,0,"%",true));
            ansColumns.Add(qb.ToSingleInteger(qa.answer));
            ansColumns.Add(qb.ToSingleChar("%"));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercent2Decimal : Question {
        public qPercent2Decimal(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            qb.rangeInt=qb.RangeSampleInt();
            decimal ans = 0m;
            if (qb.Maybe(1, 0)==1) {
                ans=Utils.Dr(0, 0, 2);
            } else {
                ans=Utils.R(1, 9);
                if (qb.Maybe(1, 0)==1) {
                    if (qb.Maybe(1, 0)==1) {
                        ans+=Utils.Dr(0, 0, 1);
                    } else {
                        ans+=Utils.Dr(0, 0, 1)/10;
                    }
                }
            }
            //qb.sbTop.Append($@"Convert{qb.br( )}{ans*100}% to{qb.br( )}decimal");
            askBuilder.AddTextDraw($@"Convert {ans}% to decimal", qb.alphaFont, new Point(0, 0));
            MyFormat qa = Utils.MyDecimal(ans, DP.none|DP.prompt);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,qa.answer,false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentNoResult : Question {
        public qPercentNoResult(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal num = qb.RangeSampleInt(0)[0]*Utils.R(1, 10);
            decimal percent = qb.RangeSampleInt(0)[0];
            //qb.sbTop.Append($@"What's{qb.br( )}{percent}% of{qb.br( )}{num} ?");
            askBuilder.AddTextDraw($@"What's {percent}% of {num} ?", qb.alphaFont, new Point(0, 0));
            decimal ans = Math.Round(percent/100*num, qParams.decimalPoints);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id, 0, qa.answer,true));
            Hints="num / 100 * percent";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentNoNumber : Question {
        public qPercentNoNumber(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal percent = qb.RangeSampleInt(0)[0]*Utils.R(1, 10);
            decimal rslt = Utils.R(1, 20)*Utils.R(1, 4);
            //qb.sbTop.Append($@"{percent}% of{qb.br( )}what is{qb.br( )}{rslt} ?");
            askBuilder.AddTextDraw($@"{percent}% of what is {rslt} ?", qb.alphaFont, new Point(0, 0));
            decimal ans = Math.Round(100/percent*rslt, qParams.decimalPoints);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id, 0, qa.answer,true));
            Hints="percent * result";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentNoPercentage : Question {
        public qPercentNoPercentage(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal num = qb.RangeSampleInt(0)[0]*Utils.R(1, 5);
            decimal rslt = Utils.R(1, 20)*20;
            //qb.sbTop.Append($@"What percentage{qb.br( )}of {rslt}{qb.br( )}is {num} ?");
            askBuilder.AddTextDraw($@"What percentage of {rslt} is {num} ?", qb.alphaFont, new Point(0, 0));
            decimal ans = Math.Round(num/rslt*100, qParams.decimalPoints);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id, 0 , qa.answer,true));
            //botText.Add(new answerChunk(id, 1 , "%",true));
            ansColumns.Add(qb.ToSingleInteger(qa.answer));
            ansColumns.Add(qb.ToSingleChar("%"));
            Hints=qb.Maybe(1, 0)==0 ? "percent / num * 100" : "percent * 100 / num";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentProfitLoss : Question {
        public qPercentProfitLoss(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal buys = qb.RangeSampleInt(0)[0];
            decimal sells = qb.RangeSampleInt(0)[0];
            string outcome = "profit";
            if (buys>sells)
                outcome="loss";
            decimal diff = Math.Max(buys, sells)-Math.Min(buys, sells);
            //qb.sbTop.Append($@"A dealer buys for{qb.br( )}{buys.ToString("c")
            //             } and sells{qb.br( )}for {sells.ToString("c")
            //             }.{qb.br( )}What is the % {outcome} ?");
            askBuilder.AddTextDraw($@"A dealer buys for {buys.ToString("c")}", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"and sells for {sells.ToString("c")}", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"What is the % {outcome} ?", qb.alphaFont, new Point(0, qb.lineSpace*2));
            decimal ans = Math.Round(diff/buys, qParams.decimalPoints+2)*100m;
            MyFormat qa = Utils.MyDecimal(ans, DP.nice|DP.prompt);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*3));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,qa.answer,true));
            //botText.Add(new answerChunk(id,0,"%",true));
            ansColumns.Add(qb.ToSingleInteger(qa.answer));
            ansColumns.Add(qb.ToSingleChar("%"));

            Hints="diff / buys";
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentDiscount : Question {
        public qPercentDiscount(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal price = qb.RangeSampleInt(0)[0]*Utils.R(5, 50);
            decimal discount = qb.RangeSampleInt(0)[0]+5;
            //qb.sbTop.Append($@"A {price.ToString("c")}{qb.br( )}item is on sale at a 
            //            {discount}% discount. How much is it?");
            askBuilder.AddTextDraw($@"A {price.ToString("c")} item is on sale at a", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"{discount}% discount. How much is it?", qb.alphaFont, new Point(0, qb.lineSpace));

            decimal ans = Math.Round(price-(discount/100*price), qParams.decimalPoints);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*2));
            topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,"£",true));
            //botText.Add(new answerChunk(id,0,qa.answer,true));
            ansColumns.Add(qb.ToSingleChar("£"));
            ansColumns.Add(qb.ToSingleInteger(qa.answer));

            Hints="price-(discount*price)";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentChangeNoCost : Question {
        public qPercentChangeNoCost(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal sellPrice = qb.RangeSampleInt(0)[0]*Utils.R(1, 50);
            decimal pl = qb.RangeSampleInt(0)[0]*(qb.Maybe(1, 0)==1 ? -1 : 1);
            string outcome = "profit";
            if (pl<0)
                outcome="loss";
            //qb.sbTop.Append($@"What was the wholesale cost if the sale price{
            //            qb.br( )}of {sellPrice.ToString("c")}{qb.br( )}is a {outcome}{
            //            qb.br( )}of {Math.Abs(pl)}%");
            askBuilder.AddTextDraw($@"What was the wholesale cost if the sale price", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"of {sellPrice.ToString("c")} is a {outcome} of {Math.Abs(pl)}% ?", qb.alphaFont, new Point(0, qb.lineSpace));
            decimal ans = 100/(100+pl)*sellPrice;
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*2));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,"£",true));
            //botText.Add(new answerChunk(id,0,qa.answer, true));
            ansColumns.Add(qb.ToSingleChar("£"));
            ansColumns.Add(qb.ToSingleInteger(qa.answer));

            Hints="100 / ( 100 + pl ) * sellPrice";
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentChangeNoSell : Question {
        public qPercentChangeNoSell(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal costPrice = qb.RangeSampleInt(0)[0];
            decimal pl = qb.RangeSampleInt(0)[0]*(qb.Maybe(1, 0)==1 ? -1 : 1);
            string outcome = "profit";
            if (pl<0)
                outcome="loss";
            //qb.sbTop.Append($@"What was the sell price if an item wholesaled at{
            //              qb.br( )}{costPrice.ToString("c")}{
            //              qb.br( )}and the {outcome} was {Math.Abs(pl)}%");
            askBuilder.AddTextDraw($@"What was the sell price if an item wholesaled at", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"{costPrice.ToString("c")} and the {outcome} was {Math.Abs(pl)}%", qb.alphaFont, new Point(0, qb.lineSpace));
            decimal ans = Math.Round((100+pl)/100*costPrice, qParams.decimalPoints);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, qParams.decimalPoints);
            //qb.sbTop.Append(qa.dp);
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*2));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,"£",true));
            //botText.Add(new answerChunk(id,0,qa.answer,true));
            ansColumns.Add(qb.ToSingleChar("£"));
            ansColumns.Add(qb.ToSingleInteger(qa.answer));

            Hints=" ( 100 + pl ) / 100 * cost";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPercentChangeNoPL : Question {
        public qPercentChangeNoPL(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            decimal sellPrice = qb.RangeSampleInt(0)[0];
            decimal costPrice = qb.RangeSampleInt(0)[0];
            while (costPrice==sellPrice)
                costPrice=qb.RangeSampleInt(0)[0];
            string outcome = "profit";
            if (costPrice>sellPrice)
                outcome="loss";
            decimal diff = sellPrice-costPrice;
            //qb.sbTop.Append($@"What is the % {outcome} of an item wholesaling{
            //             qb.br( )}at {costPrice.ToString("c")}{qb.br( )}and selling{
            //             qb.br( )}for {sellPrice.ToString("c")} ?");
            askBuilder.AddTextDraw($@"What is the % {outcome} of an item wholesaling", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"at {costPrice.ToString("c")} and selling", qb.alphaFont, new Point(0, qb.lineSpace));
            askBuilder.AddTextDraw($@"for {sellPrice.ToString("c")} ?", qb.alphaFont, new Point(0, qb.lineSpace*2));

            decimal ans = Math.Abs(Math.Round(diff/costPrice, qParams.decimalPoints+2));
            MyFormat qa = Utils.MyDecimal(ans, DP.nice|DP.prompt);
            //qb.sbTop.Append(qb.br( )).Append(qa.dp);
            askBuilder.AddTextDraw(qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*3));
            //topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,qa.answer.Substring(0,qa.answer.Length-1),true));
            //botText.Add(new answerChunk(id,0,"%",true));
            ansColumns.Add(qb.ToSingleInteger(qa.answer.Substring(0, qa.answer.Length-1)));
            ansColumns.Add(qb.ToSingleChar("%"));

            Hints="diff/cost";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }

    public class qInterest : Question {
        public qInterest(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            var amount = 0;
            //$@"You invest £{amount} at {percent}% simple interest per annum.";
            //$@"How much interest would you earn in {years} years?";
            Hints = $@"(percent / 100) * amount * years";
            // -- 
            // -- 
            // -- ask
            // -- answer
            // -- return
            askBitmap=askBuilder.Commit();
        }
    }
    public class qCompounding : Question {
        // interest, growth rates
        public qCompounding(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            Hints = "start * (1 + percent / 100 ) ^ years";

            //$@"You invest £{amount} at {percent}% compound interest per annum.";
            //$@"How much will you have in {years} years?";

            //var change = iif(qb.Maybe(1,0)==1?"appreciates":"depreciates";
            //$@"You bought something for £{amount}.";
            //$@"If it {change} by {percentage} each year,";
            //$@"How much will it be worth in {years} years?";

            //$@"{startingNumber} bacteria will increase by {percentage}% per day.";
            //$@"How large will the colony be in {years} years?";

            // -- 
            // -- 
            // -- ask
            // -- answer
            // -- return
            askBitmap=askBuilder.Commit();
        }
    }

}
