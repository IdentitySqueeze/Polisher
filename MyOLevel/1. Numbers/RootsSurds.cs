using System;
using System.Collections.Generic;
using System.Drawing;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qRootsSquaresCubes : Question {
        public qRootsSquaresCubes(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            double power = Utils.R(2, 3);
            double _base = Utils.R(2, 10);
            qb.sbTop.Append(GraphicsUtils.ToSuper(""+power))
                 .Append($@"√")
                 .Append(Math.Pow(_base, power));
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, $@"{_base}", false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{_base}"));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRootsNthRoots : Question {
        public qRootsNthRoots(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            double power = Utils.R(2, 5);
            double _base = Utils.Not(power, () => Utils.R(2, 5));
            qb.sbTop.Append(GraphicsUtils.ToSuper(""+power))
                 .Append($@"√")
                 .Append(Math.Pow(_base, power));
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, $@"{_base}", false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{_base}"));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRootsRootXʸ : Question {
        public qRootsRootXʸ(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            double y = Utils.R(3, 4);
            double x = Utils.R(2, 9);
            decimal intermediate = (decimal)Math.Pow(x, y);
            decimal ans = (decimal)Math.Sqrt((double)intermediate);
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, 2);
            qb.sbTop.Append("√"+x)
                 .Append(GraphicsUtils.ToSuper(""+y))
                 .Append(qb.br()).Append(qb.br())
                 .Append(qa.dp);
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRootsⁿᵗʰRootXʸ : Question {
        public qRootsⁿᵗʰRootXʸ(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            decimal n = Utils.R(3, 5);
            double x = Utils.R(2, 10);
            double y = Utils.R(2, 5);
            double intermediate = Math.Pow(x, y);
            decimal ans = (decimal)Math.Pow(intermediate, (double)(1m/n));
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, 2);
            qb.sbTop.Append(GraphicsUtils.ToSuper(""+n))
                 .Append("√"+x)
                 .Append(GraphicsUtils.ToSuper(""+y))
                 .Append(qb.br()).Append(qb.br())
                 .Append(qa.dp);
            topText = $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //GOTCHA:
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRootsOfFraction : Question {
        public qRootsOfFraction(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            var askColumns = new List<qColumn>();

            decimal x = Utils.Sq(2, 10);
            decimal y = Utils.Gt(x, () => Utils.Sq(3, 15));
            decimal ans = (decimal)Math.Sqrt((double)(x/y));
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, 2);
            qb.sbTop.Append("√")
                 .Append(x)
                 .Append("/")
                 .Append(y)
                 .Append(qb.br()).Append(qb.br())
                 .Append(qa.dp);
            topText= $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));
            Hints="√x/y = √x / √y";
            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            askColumns.Add(qb.ToSingleChar("√"));
            askColumns.Add(qb.ToColumnFraction(new FractionLite((long)x, (long)y)));
            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            askBuilder.AddColumns(askColumns, new Point(0, 0));
            //GOTCHA:
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qRootsProduct : Question {
        public qRootsProduct(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            decimal x = Utils.Sq(2, 10);
            decimal y = Utils.Gt(x, () => Utils.Sq(3, 15));
            decimal ans = (decimal)Math.Sqrt((double)(x*y));
            MyFormat qa = Utils.MyDecimal(ans, DP.specify|DP.prompt, 2);
            qb.sbTop.Append("√(")
                 .Append(x)
                 .Append(" * ")
                 .Append(y)
                 .Append(")")
                 .Append(qb.br()).Append(qb.br())
                 .Append(qa.dp);
            topText= $@"{qb.sbTop}";
            //botText.Add(new qChunk(id, 0, qa.answer, false));
            Hints="√xy = √x*√y";

            // -- return --
            //qb.ChunkRowToSingleInteger( this );
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));
            //GOTCHA:
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qa.answer));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qSurdsSimplify : Question {
        public qSurdsSimplify(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            decimal squared = 0m;
            decimal num = 4m;
            int[] intRange = new int[] { };
            int somethingSquared = 0;
            int rooted = 0;

            while (Utils.IsSquare((int)num)) { // No rationals
                intRange = qb.RangeSampleInt();
                somethingSquared = intRange[0];
                rooted = intRange[1];

                squared = (decimal)Math.Pow(somethingSquared, 2);//   9
                num = (decimal)(squared*rooted);      //   72
            }

            qb.answer=$@"{somethingSquared}√{rooted}";
            qb.sbTop.Append("Simplify √").Append(num);
            topText= $@"{qb.sbTop}";

            // -- ask
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(qb.answer));

            // -- return --
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints="simplify √(something squared * something)..";

        }
    }
    public class qSurdsMultiply : Question {
        public qSurdsMultiply(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            int[] intRange = new int[] { };
            int surd1 = 4, surd2 = 4;

            while (Utils.IsSquare(surd1) || Utils.IsSquare(surd2)) { // No rationals
                intRange = qb.RangeSampleInt(0);
                surd1 = intRange[0];
                surd2 = intRange[1];
            }
            // -- ask
            askBuilder.AddTextDraw($@"√{surd1} * √{surd2}", qb.alphaFont, new Point(0, 0));

            // -- answer
            double ans = surd1 * surd2;
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger("√"+ans));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hint --
            var hintBuilder = askBuilder.Reset();
            hintBuilder.backColor=Color.White;
            hintBuilder.AddLineDraw(Pens.Black, new Point(0, 10), new Point(5, 20));
            hintBuilder.AddLineDraw(Pens.Black, new Point(5, 20), new Point(10, 0));
            hintBuilder.AddLineDraw(Pens.Black, new Point(10, 0), new Point(70, 0));
            hintBuilder.AddTextDraw($@"a x b", qb.alphaFont, new Point(15, 2));
            hintsBitmap=hintBuilder.Commit();
        }
    }
    public class qSurdsFractional : Question {
        public qSurdsFractional(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            double num = 4, denom = 4;
            qb.rangeInt=new int[] { };
            while (Utils.IsSquare((int)num) ||Utils.IsSquare((int)denom)) { // No rationals
                qb.rangeInt=qb.RangeSampleInt();
                //num = Math.Pow((double)Utils.r(1, 10), 2);
                //denom = Math.Pow((double)Utils.r(1, 10), 2);
                num = qb.rangeInt[0];
                denom = qb.rangeInt[1];
            }
            Fraction f = new Fraction((long)num, (long)denom);

            // -- ask
            askBuilder.AddColumn(qb.ToColumnFraction($@"√{num}", $@"√{denom}", true, true, true), new Point(0, 0));
            var ans = Math.Sqrt(num/denom);

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(""+ans));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hint --
            var hintBuilder = askBuilder.Reset();
            hintBuilder.backColor=Color.White;
            hintBuilder.AddLineDraw(Pens.Black, new Point(0, 27), new Point(5, 37));
            hintBuilder.AddLineDraw(Pens.Black, new Point(5, 37), new Point(10, 0));
            hintBuilder.AddLineDraw(Pens.Black, new Point(10, 0), new Point(30, 0));
            hintBuilder.AddTextDraw($@"a", qb.alphaFont, new Point(15, 0));
            hintBuilder.AddLineDraw(Pens.Black, new Point(15, 20), new Point(28, 20));
            hintBuilder.AddTextDraw($@"b", qb.alphaFont, new Point(15, 20));
            hintsBitmap=hintBuilder.Commit();
        }
    }
    public class qSurdsRationaliseDenom : Question {
        public qSurdsRationaliseDenom(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            double num = 4, denom = 4;
            qb.rangeInt=new int[] { };
            while (Utils.IsSquare((int)num) ||Utils.IsSquare((int)denom)) { // No rationals
                qb.rangeInt=qb.RangeSampleInt();
                num =   qb.rangeInt[0];
                denom = qb.rangeInt[1];
            }

            // -- ask
            askBuilder.AddTextDraw("Rationalise:", qb.alphaFont, new Point(0, 0));
            askBuilder.AddColumn(qb.ToColumnFraction(""+num, "√"+denom, true, true, true), new Point(0, qb.lineSpace*2));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction(""+num+"√"+denom, ""+denom, false, false, true));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            var hintBuilder = askBuilder.Reset();
            hintBuilder.backColor=Color.White;
            hintBuilder.AddColumn(qb.ToColumnFraction("a", "√b", true, true, true), new Point(0, 0));
            hintBuilder.AddColumn(qb.OpColumn("x"), new Point(30, 15));
            hintBuilder.AddColumn(qb.ToColumnFraction("√b", "√b", true, true, true), new Point(45, 0));
            hintsBitmap =hintBuilder.Commit();
        }
    }



    // √a(  b +/- √c )
    // √a( √b +/- √c )
    public class qSurdsExpandSimplify1 : Question {
        public qSurdsExpandSimplify1(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            int[] intRange = qb.RangeSampleInt();
            int a = intRange[0];
            int b = intRange[1];
            int c = intRange[2];
            char sign = qb.Maybe(1, 0)==1 ? '+' : '-';

            // -- ask
            askBuilder.AddTextDraw($@"Expand and simplify:", qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw($@"√{a}( {b} {sign} √{c} )", qb.alphaFont, new Point(0, qb.lineSpace));

            // -- answer
            if (sign=='-') {
            } else {
            }
            qb.possibleAnswerFromColumn(this, qb.CommaColumn());

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
        }
    }

    // (  √a +/- b )( √c +/- d )
    // ( a√b +/- c )( √d +/- e )
    // (  √a +/- b )( c√d +/- e )
    public class qSurdsExpandSimplify2 : Question {
        public qSurdsExpandSimplify2(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));
            // -- answer
            qb.possibleAnswerFromColumn(this, qb.CommaColumn());
            // -- return
            askBitmap=askBuilder.Commit();
            // -- Hints
        }
    }

    // ( √a -   b )^2
    // (  a + b√c )^2
    public class qSurdsExpandSimplify3 : Question {
        public qSurdsExpandSimplify3(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));
            // -- answer
            qb.possibleAnswerFromColumn(this, qb.CommaColumn());
            // -- return
            askBitmap=askBuilder.Commit();
            // -- Hints
        }
    }

    // ( a - √b ) which bracket will make it rational?  (diff 2 square)
    // ( a + √b ) which bracket will make it rational?  (diff 2 square)
    public class qSurdsExpandSimplify4 : Question {
        public qSurdsExpandSimplify4(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));
            // -- answer
            qb.possibleAnswerFromColumn(this, qb.CommaColumn());
            // -- return
            askBitmap=askBuilder.Commit();
            // -- Hints
        }
    }

    // (a + √b)^2         = (a+√b)(a+√b) = a^2 + 2a√b + b
    public class qSurdsBracketSquared : Question {
        public qSurdsBracketSquared(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            // -- ask
            // -- answer
            // -- return
            askBitmap=askBuilder.Commit();
        }
    }

    // (a + √b)(a - √b)      surd diff 2 squares
    public class qSurdsDiff2Squares : Question {
        public qSurdsDiff2Squares(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();
            // -- ask
            // -- answer
            // -- return
            askBitmap=askBuilder.Commit();
        }
    }



}
