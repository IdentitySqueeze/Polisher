using System;
using System.Drawing;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qPowersNthPowers : Question {
        public qPowersNthPowers(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double _base = Utils.R(2, 10);
            double power = Utils.R(2, 5);
            qb.sbTop.Append(_base);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power}"));

            qb.answer=$@"{Math.Pow(_base, power)}";
            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersMultiply : Question {
        public qPowersMultiply(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double _base1, power1, _base2, power2;
            for (_base1=10, power1=1, _base2=10, power2=1;
                _base1+power1>9  || _base2+power2>9;
                _base1 = Utils.R(2, 5),
                power1 = Utils.R(2, 4),
                _base2 = Utils.R(2, 5),
                power2 = Utils.R(2, 4)
            ) ;

            qb.sbTop.Append(_base1);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power1}"));
            qb.sbTop.Append(" * ");
            qb.sbTop.Append(_base2);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power2}"));

            qb.answer = $@"{(Math.Pow(_base1, power1) * Math.Pow(_base2, power2))}";
            if ((int)_base1==(int)_base2)
                Hints = "Add the powers";

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersDivide : Question {
        public qPowersDivide(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double _base1, power1, _base2, power2;
            decimal num1, num2;
            for (_base1=10, power1=1, _base2=10, power2=1, num1 = (decimal)Math.Pow(_base1, power1), num2= (decimal)Math.Pow(_base2, power2);
                _base1+power1>9  || _base2+power2>9 || num2<num1;
                _base1 = Utils.R(2, 5),
                power1 = Utils.R(2, 4),
                _base2 = Utils.R(2, 5),
                power2 = Utils.R(2, 4)
            ) ;

            qb.sbTop.Append(_base1);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power1}"));
            qb.sbTop.Append(" / ");
            qb.sbTop.Append(_base2);
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power2}"));

            qb.answer = $@"{((decimal)Math.Pow(_base1, power1) / (decimal)Math.Pow(_base2, power2))}";
            if ((int)_base1==(int)_base2)
                Hints = "Subtract the powers";

            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersNegative : Question {
        public qPowersNegative(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double power = Utils.R(2, 3);
            Fraction askFr = new Fraction();
            Fraction ansFr = new Fraction();
            FractionLite fl = new FractionLite();

            double _base = Utils.R(2, 10); //integers

            qb.sbTop.Append(_base);
            qb.sbTop.Append("ᐨ");
            qb.sbTop.Append(GraphicsUtils.ToSuper($@"{power}"));

            //qb.answer="1/" + $@"{Math.Pow(_base, power)}";
            topText=$@"{qb.sbTop}";
            //botText.Add(new answerChunk(id,0,qb.answer,false));


            //if (qb.Maybe(1, 0)==1){
            //    // Fraction
            //    long num = qb.RangeSampleInt(0)[0];
            //    long den = qb.RangeSampleInt(1)[0];
            //    fl = FractionUtils.Reduce((long)Math.Pow(den, power), (long)Math.Pow(num, power));
            //    // -- ask -- 
            //    askFr=new Fraction($@"");
            //    askBuilder.AddColumn(qb.ToColumnFraction("",""), new Point(0, 0));
            //} else {
            //Integer
            //    double _base = Utils.r(2, 10);
            fl=new FractionLite(1, (long)Math.Pow(_base, power));
            // -- ask -- 
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //}
            ansFr=new Fraction(fl.numerator, fl.denominator);


            Hints = "One over x to the y"; //Integers

            // -- answer --
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(ansFr));

            // -- return --
            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersFractionˣ : Question {
        public qPowersFractionˣ(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //TODO: Sign flip
            //      Intermediate level
            double num = 1;//Utils.r(1,9);
            double den = num + Utils.R(1, 9);
            double power = Utils.R(2, 5);

            //qb.sbTop.Append(num).Append("/").Append(den).Append(GraphicsUtils.ToSuper($@"{power}"));
            //topText = $@"{qb.sbTop}";
            qb.answer=$@"{-Math.Pow(den, power)}";
            //botText.Add(new answerChunk(id, 0, qb.answer, false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));


            // -- ask
            var wrap = qb.BracketWrap(qb.ToColumnFraction(""+num, ""+den, true, true, true));
            var bracketPower = new qColumn();
            //TODO:
            //bracketPower.rowChunks.Add(new qChunk(1, 1, GraphicsUtils.ToSuper($@"{power}"), true));
            wrap.rows.Add(bracketPower);
            askBuilder.AddColumn(wrap,new Point(0,0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hint

        }
    }
    public class qPowersXᶠʳᵃᶜᵗⁱᵒⁿ : Question {
        public qPowersXᶠʳᵃᶜᵗⁱᵒⁿ(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- restrict to integer answers only

            decimal _base = (decimal)Math.Pow((double)Utils.R(2, 9), (double)Utils.R(1, 4));
            decimal num = Utils.R(1, 4);
            decimal den = num + Utils.R(1, 9);
            decimal ans = (decimal)Math.Pow((double)_base, (double)(num/den));

            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, 2);

            qb.sbTop.Append(_base)
                 .Append(GraphicsUtils.ToSuper(""+num))
                 .Append("ᐟ")
                 .Append(GraphicsUtils.ToSuper(""+den))
                 .Append(qb.br()).Append(qb.br())
                 .Append(qa.dp);
            askBuilder.AddTextDraw($@"{_base}{GraphicsUtils.ToSuper(""+num)}ᐟ{GraphicsUtils.ToSuper(""+den)}", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw(""+qa.dp, qb.alphaFont, new Point(0, qb.lineSpace*2));

            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //GOTCHA:
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersⁿᵗʰRoot : Question {
        public qPowersⁿᵗʰRoot(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double power = Utils.R(2, 5);
            double _base = Utils.R(2, 10);
            qb.sbTop.Append($@"{GraphicsUtils.ToSuper(""+power)}")
                 .Append("√")
                 .Append($@"{Math.Pow(_base, power)}");
            topText=$@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, $@"{_base}", false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{_base}"));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPowersBrackets : Question {
        public qPowersBrackets(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            double powerIn = Utils.R(2, 3);
            double powerOut = Utils.R(2, 3);
            double _base = Utils.R(2, 5);
            qb.answer = $@"{Math.Pow(Math.Pow(_base, powerIn), powerOut)}";
            qb.sbTop.Append($@"({_base}{GraphicsUtils.ToSuper(""+powerIn)}){GraphicsUtils.ToSuper(""+powerOut)}");
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
