using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qRatio2Fraction : Question {
        public qRatio2Fraction(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(200, 50);

            int num = qb.RangeSampleInt(0)[0];
            int den = qb.RangeSampleInt(1)[0];
            // -- calculate --
            Fraction fr = new Fraction(num, den);
            qb.answer = $@"{fr}";
            // -- return --
            topText=$@"Express {num}:{den} as a fraction";
            //botText.Add(new qChunk(1, 2, qb.answer, false));
            //qb.ChunkRowToSingleInteger( this );

            //askBuilder.AddColumn(qb.ToColumnFraction(topText, ""), new Point(65, 0));
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));

            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(fr));
            //possAnswer = new possibleAnswer();
            //possAnswer.answer.Add(qb.ToColumnFraction(fr));
            //possibleAnswers.Add(possAnswer);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qFraction2Ratio : Question {
        public qFraction2Ratio(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //askBuilder = new BitmapBuilder(200, 50);

            long num = qb.RangeSampleInt(0)[0];
            long den = qb.RangeSampleInt(1)[0];
            // -- calculate --
            Fraction fr = new Fraction(num, den);
            num=fr.numerator;   // reduced
            den=fr.denominator; // --
            // -- return --
            topText=$@"Express{qb.br()}{fr}{qb.br()}as a ratio";


            askBuilder.AddTextDraw("Express", qb.alphaFont, new Point(0, 10));
            askBuilder.AddColumn(qb.ToColumnFraction(fr), new Point(70, 0));
            askBuilder.AddTextDraw("as a ratio", qb.alphaFont, new Point(105, 10));

            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction($@"{fr.numerator}:{fr.denominator}", ""));
            //possAnswer = new possibleAnswer();
            //possAnswer.answer.Add(qb.ToColumnFraction($@"{fr.numerator}:{fr.denominator}",""));
            //possibleAnswers.Add(possAnswer);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRatioPencePound : Question {
        public qRatioPencePound(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int pence = qb.RangeSampleInt(0)[0]*10;
            int pounds = qb.RangeSampleInt(0)[0];
            // -- calculate --
            int num, den;
            if (qb.Maybe(1, 0)==1) {
                num=pence;
                den=pounds*100;
                Fraction fr = new Fraction(num, den);
                topText=$@"Express{qb.br()}{pence}p to £{pounds}{qb.br()}as a ratio in its lowest terms";
                askBuilder.AddTextDraw($@"Express {pence}p to £{pounds}", qb.numericFont, new Point(0, 0));
                askBuilder.AddTextDraw("as a ratio in its lowest terms.", qb.numericFont, new Point(0, qb.lineSpace));

                //botText.Add(new answerChunk(1,2,$@"{fr.numerator}:{fr.denominator}", false));
                qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{fr.numerator}:{fr.denominator}"));
            } else {
                num=pounds*100;
                den=pence;
                Fraction fr = new Fraction(num, den);
                topText=$@"Express{qb.br()}£{pounds} to {pence}p{qb.br()}as a ratio in its lowest terms";
                askBuilder.AddTextDraw($@"Express £{pounds}p to {pence}p", qb.numericFont, new Point(0, 0));
                askBuilder.AddTextDraw("as a ratio in its lowest terms.", qb.numericFont, new Point(0, qb.lineSpace));
                //botText.Add(new answerChunk(1,2,$@"{fr.numerator}:{fr.denominator}", false));
                qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{fr.numerator}:{fr.denominator}"));
            }

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.numericFont,new Point(0,0));


            askBitmap=askBuilder.Commit();
        }
    }
    public class qRatioMixed : Question {
        public qRatioMixed(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            long num = qb.RangeSampleInt(0)[0];
            var tup = Utils.Fr(1, 2);
            Fraction denom = new Fraction(tup.Item1, tup.Item2);
            if (qParams.mixed && qb.Maybe(1, 0)==1)
                denom+=qb.RangeSampleInt(0)[0];
            // -- calculate --
            Fraction ans = new Fraction(num*denom.denominator, denom.numerator);
            // -- return --
            topText=$@"Express{qb.br()}{num} : {$@"{denom}"}{qb.br()}as a ratio in its lowest terms";
            //botText.Add(new answerChunk(1,2,$@"{ans.numerator}:{ans.denominator}", false));
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw($@"Express {num} : {denom}", qb.numericFont, new Point(0, 0));
            askBuilder.AddTextDraw("as a ratio in its lowest terms.", qb.numericFont, new Point(0, qb.lineSpace));

            //askBuilder.AddTextDraw($@"{ans.numerator}:{ans.denominator}", qb.alphaFont,new Point(0,0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{ans.numerator}:{ans.denominator}"));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRatioFind : Question {
        public qRatioFind(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- calculate --
            int num = qb.RangeSampleInt(0)[0];
            int den = num+qb.RangeSampleInt(0)[0];
            int mult = (qb.RangeSampleInt(1)[0]);
            // -- return --
            if (qb.Maybe(1, 0)==1) {
                //topText=$@"Two lengths are in the ratio {num}:{den}.{qb.br( )}If the 2nd is {mult*den}m, what is the 1st?";
                askBuilder.AddTextDraw($@"Two lengths are in the ratio {num}:{den}", qb.alphaFont, new Point(0, 0));
                askBuilder.AddTextDraw($@"If the 2nd is {mult*den}m, what is the 1st?", qb.alphaFont, new Point(0, qb.lineSpace));
                //botText.Add(new answerChunk(1,2,$@"{mult*num}", false));
                qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{mult*num}"));
            } else {
                //topText=$@"Two lengths are in the ratio {num}:{den}.{qb.br( )}If the 1st is {mult*num}m, what is the 2nd?";
                askBuilder.AddTextDraw($@"Two lengths are in the ratio {num}:{den}", qb.alphaFont, new Point(0, 0));
                askBuilder.AddTextDraw($@"If the 1st is {mult*num}m, what is the 2nd?", qb.alphaFont, new Point(0, qb.lineSpace));
                //botText.Add(new answerChunk(1,2,$@"{mult*den}", false));
                qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{mult*den}"));
            }
            //botText.Add(new qChunk(1, 2, $@"m", true));
            //qb.ChunkRowToSingleInteger( this );            

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRatioDivide : Question {
        public qRatioDivide(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            var ansColumns = new List<qColumn>();

            qb.rangeInt=qb.RangeSampleInt(); //qParams.terms=4

            long divisor = qb.rangeInt.Sum();
            long quotient = qb.RangeSampleInt(0)[0];
            long dividend = divisor*quotient;

            // -- calculate --
            qb.sbTop.Append($@"Divide {dividend} in the ratio ");
            for (int i = 0; i<qb.rangeInt.Count()-1; i++)
                qb.sbTop.Append($@"{qb.rangeInt[i]}:");
            qb.sbTop.Append(qb.rangeInt.Last());

            // -- return --
            topText=$@"{qb.sbTop}";
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            for (int i = 0; i<qb.rangeInt.Count()-1; i++) {
                //botText.Add(new answerChunk(1,2, $@"{qb.rangeInt[i]*quotient}", false));
                //botText.Add(new answerChunk(1,2, ", ", true));
                ansColumns.Add(qb.ToSingleInteger($@"{qb.rangeInt[i]*quotient}"));
                ansColumns.Add(qb.CommaColumn());
            }
            //botText.Add(new answerChunk(1,2, $@"{qb.rangeInt.Last()*quotient}", false));
            ansColumns.Add(qb.ToSingleInteger($@"{qb.rangeInt.Last()*quotient}"));
            //qb.ChunkRowToSingleInteger( this );
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qProportionBuysCosts : Question {
        public qProportionBuysCosts(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var ansColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            string[] semanticStuff ={"articles", "ingredients", "items", "units", "parts", "components", "k", "kg", "litres", "fl. ounces", "hours",
                                     "minutes", "days", "metres", "yards", "Mwh", "packages", "samples", "packs", "crates", "pallets", "boxes", "trays" };
            string stuff = semanticStuff[Utils.R(1, semanticStuff.Count()-1)];
            string buysCosts = "buys";
            if (qb.Maybe(1, 0)==0)
                buysCosts="costs";

            // -- calculate --
            qb.rangeInt=qb.RangeSampleInt();
            decimal more = qb.rangeInt[0]*qb.rangeInt[1];
            decimal less = qb.rangeInt[2];
            decimal unitCost = ((decimal)Utils.R(2, 20))+(qb.Maybe(1, 0)==1 ? 0.5m : 0m);
            string money = string.Empty;
            if (buysCosts=="costs") {
                money=Utils.ToMoney(unitCost*less);
                //topText=$@"if {more} {stuff} costs {Utils.ToMoney(unitCost*more)}, how much do {less} {stuff} cost?";
                askBuilder.AddTextDraw($@"if {more} {stuff} costs {Utils.ToMoney(unitCost*more)},", qb.alphaFont, new Point(0, 0));
                askBuilder.AddTextDraw($@"how much do {less} {stuff} cost?", qb.alphaFont, new Point(0, qb.lineSpace));
                //botText.Add(new answerChunk(1,2,$@"£", true));
                //botText.Add(new answerChunk(1,2,$@"{money.Substring(1,money.Length-1)}", false));
                ansColumns.Add(qb.ToSingleChar("£"));
                ansColumns.Add(qb.ToSingleInteger($@"{money.Substring(1, money.Length-1)}"));

                //Pretending to use fractions like you#re supposed to...
                //decimal moreCost = unitCost*more;
                //botText.Add(new qDets(1,2,"\n"+$@"{Utils.ToMoney((less/more)*moreCost)}", false));
                //or
                //botText.Add(new qDets(1,2,"\n"+$@"{Utils.ToMoney((new Fraction((long)less,(long)more).ToDecimal() * moreCost ) )}", false));
            } else {
                //topText=$@"if {Utils.ToMoney(unitCost*more)} buys {more} {stuff}, how many {stuff} does{qb.br( )}{Utils.ToMoney(unitCost*less)} buy?";
                askBuilder.AddTextDraw($@"if {Utils.ToMoney(unitCost*more)} buys {more} {stuff},", qb.alphaFont, new Point(0, 0));
                askBuilder.AddTextDraw($@"how many {stuff} does {Utils.ToMoney(unitCost*less)} buy?", qb.alphaFont, new Point(0, qb.lineSpace));
                //botText.Add(new answerChunk(1,2,"          ", true));
                //botText.Add(new answerChunk(1,2,$@"{less}", false));
                //botText.Add(new answerChunk(1,2,"          ", true));
                ansColumns.Add(qb.ToSingleInteger($@"{less}"));
            }
            Hints="(less/more)*moreCost";
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            qb.possibleAnswerFromColumns(this, ansColumns);

            askBitmap=askBuilder.Commit();
        }
    }
    public class qProportionThingMoves : Question {
        public qProportionThingMoves(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            flongy[] movers = new flongy[]{
                new flongy( "time",   " hours",      "in",  1, new flingy( "hiker", "walks",  "km"), new flangy("hikers", "walk",  "km") ),
                new flongy( "time",   " hours",      "in",  1, new flingy( "hiker", "walks",  "km"), new flangy("hikers", "walk",  "km") ),
                new flongy( "petrol", " ltr of petrol", "on", 10, new flingy( "car",   "drives", "km"), new flangy("cars",   "drive", "km") ),
                new flongy( "time", " hours", "in", 100, new flingy( "plane", "flies", "km"), new flangy("planes", "fly", "km") ),
                new flongy( "time", " hours", "in", 30, new flingy( "train", "travels", "km"), new flangy("trains", "travel", "km") ),
                new flongy( "time", " hours", "in", 10, new flingy( "bus", "averages", " stops"), new flangy("buses", "average", " stops") ),
                new flongy( "energy", "kj", "using", 1, new flingy( "cyclist", "covers", "km"), new flangy("cyclists", "cover", "km") ),
                new flongy( "time", " hours", "in", 1, new flingy( "pedestrian", "walks", "km"), new flangy("pedestrians", "walk", "km") )
            };
            // A {hiker}      {walks}    {short}{km}     {in}    {stime}  {hours}.       How much {time}   to {walk}    {long}km?
            // A {car}        {drives}   {short}{km}     {on}    {spetrol}{l of petrol}. How much {petrol} to {drive}   {long}km?;
            // A {train}      {travels}  {short}{km}     {in}    {stime}  {hours}.       How much {time}   to {travl}   {long}km?
            // A {plane}      {flies}    {short}{km}     {in}    {stime}  {hours}.       How much {time}   to {fly}     {long}km?
            // A {bus}        {averages} {short}{ stops} {in}    {1}      {hour}.        How much {time}   to {average} {long}km?
            // A {cyclist}    {covers}   {short}{km}     {using} {int}    {kj}.          How much {energY} to {cover}   {long}km? 
            // A {pedestrian} {walks}    {short}{km}     {in}    {stime}  {day}.         How much {time}   to {walk}    {long}km?

            // -- calculate --
            flongy thing = movers[Utils.R(1, movers.Count()-1)];
            qb.rangeInt=qb.RangeSampleInt();
            decimal shortUnits = qb.rangeInt[0] * thing.shrubbery; // 10's of km
            decimal shortMeasure = qb.rangeInt[1];                 //  5's of l of petrol
            decimal longUnits = qb.rangeInt[2] * thing.shrubbery;  // 10's of km
            //decimal longMeasure=Math.Round((longUnits/shortUnits)*shortMeasure+.001m,2);
            decimal longMeasure = Math.Round((longUnits/shortUnits)*shortMeasure, 2);

            //topText= $@"A {thing.eki.doobrey} {thing.eki.wotsit}{qb.br( )}{shortUnits}{thing.eki.thingy}{qb.br( )}{thing.bosh} {shortMeasure}{thing.bash}. ";
            //topText+= $@"How much {thing.bish} to {thing.ftang.wotsits} {longUnits}{thing.eki.thingy} ?";
            askBuilder.AddTextDraw(
                // A {hiker}             {walks}            {short}     {km}               {in}         {stime}       {hours}.       
                // A {car}               {drives}           {short}     {km}               {on}         {spetrol}     {l of petrol}.
                $@"A {thing.eki.doobrey} {thing.eki.wotsit} {shortUnits}{thing.eki.thingy} {thing.bosh} {shortMeasure}{thing.bash}.",
                qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw(
                // How much {time}       to {walk}                {long}     {km}?
                // How much {petrol}     to {drive}               {long}     {km}?;
                $@"How much {thing.bish} to {thing.ftang.wotsits} {longUnits}{thing.eki.thingy} ?",
                qb.alphaFont, new Point(0, 17));
            //and that's quite enough of that.

            // -- return --
            Hints="long/short*shortMeasure";
            //botText.Add(new answerChunk(1,2,"     ", true));
            //botText.Add(new answerChunk(1,2,$@"{longMeasure}", false));
            //botText.Add(new answerChunk(1,2,"     ", true));

            //qb.ChunkRowToSingleInteger( this );
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{longMeasure}"));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRatioDecimals : Question {
        public qRatioDecimals(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int int1, int2;
            int[] intRange = qb.RangeSampleInt(0);
            int1=intRange[0];
            int2=intRange[1];

            int mult = qb.RangeSampleInt(0)[0];

            decimal d1 = (int1*mult)/10m;
            decimal d2 = (int2*mult)/10m;

            // -- ask
            askBuilder.AddTextDraw($@"Simplify {d1}:{d2}", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"as far as possible.", qb.alphaFont, new Point(0, qb.lineSpace));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{int1}:{int2}"));

            // -- return
            askBitmap=askBuilder.Commit();
            Hints = $@"Times ten(s), divide.";
        }
    }
    public class qRatioFractions : Question {
        public qRatioFractions(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            var askColumns = new List<qColumn>();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            FractionLite fl1 = new FractionLite(1, 1);
            FractionLite fl2 = new FractionLite(1, 1);
            int[] intRange;
            long long1 = 1, long2 = 1, commonDenominator = 1;
            FractionLite tmp;

            while (fl1.denominator==fl2.denominator) { //So the denoms aren't the same
                intRange = qb.RangeSampleInt();
                long1 = intRange[0];
                long2 = intRange[1];
                commonDenominator = qb.RangeSampleInt(1)[0];
                tmp = FractionUtils.Reduce(long1, long2);
                long1 = tmp.numerator;
                long2 = tmp.denominator;

                fl1 = FractionUtils.Reduce(long1, commonDenominator);
                fl2 = FractionUtils.Reduce(long2, commonDenominator);
            }

            // -- ask
            askBuilder.AddTextDraw($@"Give the following ratio in its simplest form:", qb.alphaFont, new Point(0, 0));
            askColumns.Add(qb.ToColumnFraction(fl1));
            askColumns.Add(qb.ToSingleInteger(":"));
            askColumns.Add(qb.ToColumnFraction(fl2));
            askBuilder.AddColumns(askColumns, new Point(0, qb.lineSpace));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{long1}:{long2}"));

            // -- return
            askBitmap=askBuilder.Commit();
            Hints = $@"Common denominator, take tops";
        }
    }
}
