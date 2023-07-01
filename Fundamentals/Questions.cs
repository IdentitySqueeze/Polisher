using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using Fangorn;
using MathUtils;
using Polish.OLevel.Numbers;

namespace Polish {
    //AXIOM: Exponents won't have exponents


    public struct qParameters {
        public string ops { get; set; }
        public int[,] numberRanges { get; set; }
        public bool brackets { get; set; }
        public int terms { get; set; } // >2
        public bool fractions { get; set; }
        public bool decimals { get; set; }
        public bool integers { get; set; }
        public bool negatives { get; set; }
        public int limit { get; set; }
        public bool includeFailure { get; set; }
        public bool unique { get; set; }
        public bool mixed { get; set; }
        public int mantissa { get; set; }
        public int characteristic { get; set; }
        public bool brevity { get; set; }
        public int decimalPoints { get; set; }
        public int numerator { get; set; }
        public int denominator { get; set; }
        public qParameters(string pOps, int[,] pNumberRanges, bool pBrackets, int pTerms, bool pFractions, bool pDecimals, bool pIntegers,
                           bool pNegatives, int pLimit, bool pIncludeFailure, bool pUnique, bool pMixed, int pMantissa, int pCharacteristic,
                           bool pBrevity, int pDecimalPoints, int pNumerator, int pDenominator) {
            ops = pOps;
            numberRanges = pNumberRanges;
            brackets = pBrackets;
            terms = pTerms;
            fractions = pFractions;
            decimals = pDecimals;
            integers = pIntegers;
            negatives = pNegatives;
            limit = pLimit;
            includeFailure = pIncludeFailure;
            unique = pUnique;
            mixed=pMixed;
            mantissa = pMantissa;
            characteristic = pCharacteristic;
            brevity = pBrevity;
            decimalPoints = pDecimalPoints;
            numerator = pNumerator;
            denominator = pDenominator;
        }
    }
    public abstract class Question : IQuestion
    {
        //Ctor
        public Question(int id, qParameters qparams) => qParams = qparams;  
        public List<possibleAnswer> possibleAnswers { get; set; } = new List<possibleAnswer> { };
        //public List<qChunk> botText { get; set; } = new List<qChunk>(); 
        public string topText { get; set; }=string.Empty;
        public string Hints {get; set;}="Be more happy.";
        public qParameters qParams { get; set; }
        public abstract void GenerateQuestion();
        //public QuestionType questionType { get; set; }
        public Bitmap askBitmap{ get; set; }
        public Bitmap hintsBitmap { get; set; }
        public Font queFont { get; set; }
        public string qTitle{ get; set; }
        public int id { get; set; }
        public Genus genus { get; set; } = Genus.OriginalClient;

    }
    public class QuestionBuilder {
        public QuestionBuilder(qParameters qparams, Font pFont) {
            qParams = qparams;
            font = pFont;
            MeasureLetterHeight();
        }

        #region// -- GetStringLength stuff --
        private Bitmap gBmp = new Bitmap(1, 1);
        private Graphics g { get; set; }
        #endregion

        #region // -- fields --
        public StringBuilder sbTop { get; set; } = new StringBuilder();
        qParameters qParams { get; set; }
        public Font font { get; set; }
        public Font alphaFont { get; set; } = new Font("Consolas", 10);
        public Font numericFont { get; set; } = new Font("Consolas", 10);
        public int letterHeight { get; set; } = 15;
        public int lineSpace { get; set; } = 20;
        private Pen blackPen { get; set; } = new Pen(Color.Black, 3);
        private Pen divisionPen { get; set; } = new Pen(Color.Black, 2);
        public string answer { get; set; }
        private bool returns { get; set; } = true; //cr vs 32
        public int brCount { get; set; }
        #endregion

        #region // -- methods --
        public void MeasureLetterHeight() {
            FontFamily fontFamily = new FontFamily(alphaFont.Name);
            int ascent = fontFamily.GetCellAscent(FontStyle.Regular);
            int descent = fontFamily.GetCellDescent(FontStyle.Regular);
            int emHeight = fontFamily.GetEmHeight(FontStyle.Regular);
            //int lineSpacing = fontFamily.GetLineSpacing(FontStyle.Regular);

            int ascentPixel = (int)alphaFont.Size * ascent / emHeight;
            int descentPixel = (int)alphaFont.Size * descent / emHeight;

            letterHeight = ascentPixel + descentPixel + 3; // 3 needed else it chops the bottom
            lineSpace = letterHeight ;// (int)alphaFont.Size * lineSpacing / emHeight;
        }
        public string br() {
            brCount++;
            return returns ? "\n" : " ";
        }
        public char ChooseOp() => qParams.ops[Utils.R(1, qParams.ops.Length)-1];
        public qColumn ToSingleChar(string str, bool ans = true) => ToSingleNumerator(str, ans);
        public qColumn ToSingleInteger(string str, bool ans = false) => ToSingleNumerator(str, ans);
        public qColumn ToSingleNumerator(string str, bool ans) {


            var rtn = new qColumn();
            var num = new qColumn();

            num.nodeValue = str;
            num.answered= ans;

            rtn.rows.Add(num);
            rtn.showDiv = false;
            return rtn;
        }
        public qColumn ToDoubleNumerator(string str, bool ans) => ToColumnFraction(str, "", ans, true, false);
        //public answerColumn ToSingleDenominator ( string str ){ }
        //public answerColumn ToDoubleDenominator ( string str, bool ans ){
        //    return ToColumnFraction("", str, true, ans, false);
        //}
        public qColumn CommaColumn() => ToSingleChar(",", true);
        public qColumn OpColumn(string op) => ToSingleChar(op, true);
        public qColumn ToSingleInteger(decimal i) => ToSingleInteger(""+i, false);
        public qColumn ToColumnFraction(qColumn num, qColumn den, bool showDiv = false) {
            var rtn = new qColumn();
            rtn.rows.Add(num);
            rtn.rows.Add(den);
            rtn.showDiv = showDiv;
            return rtn;
        }

        public qColumn ToColumnFraction(string txtNum, string txtDen, bool numAns = false, bool denAns = true, bool showDiv = false) {
            var rtn = new qColumn();
            var num = new qColumn();
            var den = new qColumn();

            num.nodeValue = txtNum;
            den.nodeValue = txtDen;
            num.answered = numAns;
            den.answered = denAns;

            rtn.rows.Add(num);
            rtn.rows.Add(den);

            rtn.showDiv = showDiv;
            return rtn;
        }
        public qColumn ToBracketColumn(string txtNum, string txtDen, bool numAns = false, bool denAns = true, bool showDiv = false) {
            var rtn = new bracketColumn();
            rtn.colType |= ColTyp.fraction;
            var num = new qColumn();
            var den = new qColumn();

            num.nodeValue = txtNum;
            den.nodeValue = txtDen;
            num.answered = numAns;
            den.answered = denAns;

            rtn.rows.Add(num);
            rtn.rows.Add(den);

            rtn.showDiv = showDiv;
            return rtn;
        }

        public qColumn ToColumnFraction(Fraction fr, bool showDiv = true) => ToColumnFraction(new FractionLite(fr.numerator, fr.denominator));
        public qColumn ToColumnFraction(FractionLite fr, bool numAns = false, bool denAns = false) => ToColumnFraction($@"{fr.numerator}", $@"{fr.denominator}", numAns, denAns, true);
        public qColumn ToBracketColumn(FractionLite fr, bool numAns = false, bool denAns = false) => ToBracketColumn($@"{fr.numerator}", $@"{fr.denominator}", numAns, denAns, true);
        public qColumn BracketWrap(qColumn ac) => Wrap(ac, ColTyp.bracket); 
        public qColumn BraceWrap(qColumn ac) => Wrap(ac, ColTyp.brace);
        public qColumn SquareBracketWrap(qColumn ac) => Wrap(ac, ColTyp.squareBracket);
        public qColumn RootWrap(qColumn ac) => Wrap(ac, ColTyp.rooted);
        private qColumn Wrap(qColumn ac, ColTyp c) {
            var rtn = new qColumn();
            rtn.colType = c;
            rtn.columns.Add(ac);
            return rtn;
        }

        public qColumn Sigma(int pFrom, int pTo) {
            var rtn = new qColumn();
            rtn.from = pFrom;
            rtn.to=pTo;
            rtn.colType=ColTyp.sigma;
            return rtn;
            //"∞"
        }
        public Bitmap ImageFromText(string topText) {
            Bitmap rtn;
            // 200 is about 18 wide...
            // 20 is about 1 row...
            //int rows = 1+ brCount;
            int rows = Math.Max(3, topText.Length / 18) + brCount;
            //System.Windows.Forms.MessageBox.Show($@"{topText}, {"\n"}length: {topText.Length}, {"\n"}rows@ {rows}, {"\n"}brCount: {brCount}");
            rtn = new Bitmap(200, (int)(rows * 20));
            Graphics g = Graphics.FromImage(rtn);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            g.DrawString(topText, font, new SolidBrush(Color.Black), new Rectangle(0, 0, rtn.Width, rtn.Height));
            return rtn;
        }
        public void possibleAnswerFromColumns(Question q, List<qColumn> cols) {
            possibleAnswer possAnswer = new possibleAnswer { }; // I'm ( one ) answer...
            possAnswer.answer.AddRange(cols);
            q.possibleAnswers.Add(possAnswer);
        }
        public void possibleAnswerFromColumn(Question q, qColumn col) {
            possibleAnswer possAnswer = new possibleAnswer { }; // I'm ( one ) answer...
            possAnswer.answer.Add(col);
            q.possibleAnswers.Add(possAnswer);
        }
        public int[] rangeInt { get; set; }
        public int[] RangeSampleInt(int rng = -1) {
            int[] rtn = new int[qParams.terms];
            int num, rngStart, rngEnd = 0, failureCount = 0;
            for (int i = 0; i<qParams.terms; i++) {
                do {
                    if (rng==-1)
                        rng = Utils.R(1, qParams.numberRanges.Length/2)-1; //Which range?

                    rngStart = qParams.numberRanges[rng, 0]; //start
                    rngEnd = qParams.numberRanges[rng, 1]+1; //end
                    num = Utils.R(rngStart, rngEnd);
                    if (qParams.negatives && Utils.R(1, 10)>7)
                        num*=-1;
                } while (qParams.unique && rtn.Contains(num) && failureCount++ != 100);
                if (failureCount==100) {
                    //scrap it all and start again
                    rtn = new int[qParams.terms];
                    i=-1;
                } else {
                    rtn[i]=num;
                }
                failureCount=0;
            }
            return rtn;
        }
        public decimal[] rangeDec { get; set; }
        public decimal[] RangeSampleDec(int rng = -1) {
            // mantissa & characteristics are length values not value values
            decimal[] rtn = new decimal[qParams.terms];
            decimal sumRtn = 0m, num = 0m;
            int rngStart, rngEnd = 0, failureCount = 0;
            for (int i = 0; i<qParams.terms; i++) {
                do {
                    if (rng==-1)
                        rng = Utils.R(1, qParams.numberRanges.Length/2)-1; //Which range?
                    rngStart = qParams.numberRanges[rng, 0]; //start
                    rngEnd = (qParams.numberRanges[rng, 1]+1)*(int)(Math.Pow(10, qParams.characteristic-1));
                    num = Utils.Dr(rngStart, rngEnd, qParams.mantissa);
                    if (sumRtn==0)
                        sumRtn=num;
                    // -- checks --
                } while (qParams.unique && rtn.Contains(num) && failureCount++ != 100);
                if (failureCount==100) {
                    //scrap it all and start again
                    rtn = new decimal[qParams.terms];
                    i=-1;
                    sumRtn=0m;
                } else {
                    rtn[i]=num;
                }
                failureCount=0;
            }
            //return rtn;
            var tmp = rtn.ToList();
            tmp.Reverse();
            return tmp.ToArray();
        }
        public bool Maybe() => Utils.R(1, 10)>5;
        public string Maybe<T>(T value) => Utils.R(1, 10)>5 ? value.ToString() : string.Empty;
        public T Maybe<T>(T value, byte nothingToSeeHere = 0) where T : new() => Utils.R(1, 10)>5 ? value : new T();
        public T Definitely<T>(T value) => value; //:D  conceptual lube
        #endregion
    }










    public class qTestQuestionNew : Question {
        public qTestQuestionNew(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();

            genus = Genus.NewStructure;
            askBuilder.genus= Genus.NewStructure;

            possibleAnswer possAnswer = new possibleAnswer();

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

            // -- pose ask in shuffled order --
            var shuffled = new List<qColumn>();
            for (i=0, j=1, k=0; i<seq.Count(); i++) {
                while (ords[k]!=j)
                    k++;
                shuffled.Add(qb.ToBracketColumn(new FractionLite(fTerms[k].numerator, fTerms[k].denominator)));
                k=0;
                j++;
            }
            
            // -- arrange answer in sequence order --
            possAnswer = new possibleAnswer();
            for (i=0; i<seq.Count()-1; i++) {
                possAnswer.answer.Add(qb.ToBracketColumn(fTerms[i]));
                possAnswer.answer.Add(qb.CommaColumn());
            }
            possAnswer.answer.Add(qb.ToBracketColumn(fTerms[seq.Count()-1]));

            topText = $@"Arrange low to high:{qb.br()}";
            topText = topText.Substring(0, topText.Length);


            possAnswer.IsSequence = true;
            possAnswer.uniformSize = true;
            possibleAnswers.Add(possAnswer);

            //askBuilder = new BitmapBuilder(200, 100);
            //askBuilder.OldAddTextLine(topText, qb.alphaFont, new Point(0, 0));
            askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0, 0));

            askBuilder.AddColumns(shuffled, new Point(10, 40));

            askBitmap=askBuilder.Commit();
        }
    }




    public class qTestQuestion : Question {
        public qTestQuestion(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();

            // -- ask
            //askBuilder.AddTextDraw($@"What is da fing?", qb.alphaFont, new Point(0, 0));

            // ==== 1

            var askColumns = new List<qColumn> { };
            askColumns.Add(qb.BracketWrap(qb.ToSingleInteger("A question: ")));
            askColumns.Add(qb.OpColumn("+"));

            // ==== 2

            var sf = new qColumn();
            var sfnum = new qColumn();
            var sfden = new qColumn();

            // BLOW CHUNKS -----------------
            //sfnum.rowChunks.Add(new qChunk(0, 0, "some", true));
            //sfden.rowChunks.Add(new qChunk(0, 0, "fraction", true));

            //new bits
            sfnum.nodeValue= "some";
            sfden.nodeValue= "fraction";
            sfnum.answered=true;
            sfden.answered=true;


            var wrappedNum = qb.BracketWrap(sfnum);

            //var someBracNum = new qColumn();
            //var someBracDen = new qColumn();
            //someBracNum.colType = ColTyp.bracket;
            //someBracNum.rows.Add(sfnum);
            //someBracDen.colType = ColTyp.bracket;
            //someBracDen.rows.Add(sfden);

            sf.rows.Add(wrappedNum);
            //sf.rows.Add(sfnum);
            sf.rows.Add(sfden);
            sf.showDiv = true;
            askColumns.Add(qb.BracketWrap(sf));

        //askColumns.Add(qb.BracketWrap(qb.ToColumnFraction("some", "fraction", true, true, true)));

            //askColumns.Add(sf);

            // ==== 3

            askColumns.Add(qb.OpColumn("+"));
            var doubleWithExp = new qColumn();
            var doubleWithExpNum = new qColumn();
            var doubleWithExpDenom = new qColumn();
            var doubleWithExpNumExp = new qColumn();

            // BLOW CHUNKS -----------------
            //doubleWithExpNum.rowChunks.Add(new qChunk(0, 0, "double", true));
            //doubleWithExpDenom.rowChunks.Add(new qChunk(0, 0, "h", true));

            //new bits
            doubleWithExpNum.nodeValue= "double";
            doubleWithExpDenom.nodeValue= "h";
            doubleWithExpNum.answered=true;
            doubleWithExpDenom.answered=true;


            doubleWithExpNumExp.colType = ColTyp.exponent;
            doubleWithExpNumExp.columns.Add(qb.ToSingleChar("expexpexp"));
            doubleWithExpDenom.columns.Add(doubleWithExpNumExp);
            doubleWithExp.rows.Add(doubleWithExpNum);
            doubleWithExp.rows.Add(doubleWithExpDenom);
            askColumns.Add(qb.BracketWrap(qb.BracketWrap(doubleWithExp)));
            askColumns.Add(qb.OpColumn("+"));

            // ==== 4

            var singWithExp = new qColumn();
            var singRow = new qColumn(); // S
            var singExp = new qColumn(); // sing
            singWithExp.rows.Add(singRow);
            // BLOW CHUNKS -----------------
            //singRow.rowChunks.Add(new qChunk(0, 0, "S", true));

            //new bits
            singRow.nodeValue= "S";
            singRow.answered = true;

            singRow.columns.Add(singExp);
            //singExp.colType = ColTyp.exponent;
            singExp.columns.Add(qb.ToSingleChar("sing"));
            var singBrac = new qColumn();
            singBrac.colType=ColTyp.bracket;
            singBrac.columns.Add(singWithExp);
            //askColumns.Add(singBrac);
            var singBracExp = new qColumn();
            singBrac.rows.Add(singBracExp);
            singBracExp.columns.Add(qb.ToSingleChar("a"));
            singBracExp.columns.Add(qb.ToColumnFraction("1", "2", true, true, true));
            singBracExp.columns.Add(qb.ToSingleChar("b"));
            singBracExp.columns.Add(qb.ToColumnFraction("1", "2", true, true, true));
            singBracExp.columns.Add(qb.ToSingleChar("c"));
            singBracExp.columns.Add(qb.ToColumnFraction("1", "2", true, true, true));
            singBracExp.columns.Add(qb.ToSingleChar("d"));
            singBracExp.columns.Add(qb.ToColumnFraction("1", "2", true, true, true));

            var yabrac = new qColumn();
            yabrac.colType=ColTyp.bracket;
            yabrac.columns.Add(singBrac);

            var bigA = new qColumn();
            var bigARow = new qColumn();
            bigA.rows.Add(bigARow);
            // BLOW CHUNKS -----------------
            //bigARow.rowChunks.Add(new qChunk(0, 0, "A", true));

            //new bits
            bigARow.nodeValue = "A";
            bigARow.answered = true;

            var bigAExp = new qColumn();
            bigAExp.colType=ColTyp.exponent;
            bigAExp.columns.Add(qb.ToSingleChar("bigAExp"));
            bigARow.columns.Add(bigAExp);

            //            yabrac.columns.Add(qb.OpColumn("/"));
            //            yabrac.columns.Add(bigA);

            askColumns.Add(yabrac);
            var yabracExp = new qColumn();
            yabrac.rows.Add(yabracExp);
            yabracExp.columns.Add(qb.ToSingleChar("e"));
            yabracExp.columns.Add(qb.ToColumnFraction("1", "2", true, true, true));
            yabracExp.columns.Add(qb.ToSingleChar("f"));
            yabracExp.columns.Add(qb.ToColumnFraction("1", "2", true, true, true));

            //askColumns.Add(qb.BracketWrap(singWithExp));

            askColumns.Add(qb.OpColumn("+"));

            // ==== 5

            var frac = new qColumn();   // 
            var num = new qColumn();    // a
            var den = new qColumn();    // b
            var fenom = new qColumn();    // f

            var numExp = new qColumn(); // 2
            var denExp = new qColumn(); // 3
            var fenExp = new qColumn(); // 3

            // fraction
            frac.rows.Add(num);
            frac.rows.Add(den);
            frac.rows.Add(fenom);

            // num, denom, fenom
            // BLOW CHUNKS -----------------
            //num.rowChunks.Add(new qChunk(0, 0, "AAAAAA", true));
            //den.rowChunks.Add(new qChunk(0, 0, "b", true));
            //fenom.rowChunks.Add(new qChunk(0, 0, "F", true));

            //new bits
            num.nodeValue="AAAAAA";
            den.nodeValue="b";
            fenom.nodeValue="F";
            num.  answered = true;
            den.  answered = true;
            fenom.answered = true;

            num.columns.Add(numExp);
            den.columns.Add(denExp);
            fenom.columns.Add(fenExp);

            numExp.colType = ColTyp.exponent;
            denExp.colType = ColTyp.exponent;
            fenExp.colType = ColTyp.exponent;

            numExp.columns.Add(qb.ToSingleChar("w"));
            var xFrac = qb.ToColumnFraction("x", "y", true, true, true);
        //xFrac.rows[0].colType=ColTyp.exponent;
        //xFrac.rows[1].colType=ColTyp.exponent;
            numExp.columns.Add(xFrac);
            numExp.columns.Add(qb.ToSingleChar("z"));
            numExp.columns.Add(qb.ToSingleChar("M"));
            numExp.columns.Add(qb.ToSingleChar("z"));

            denExp.columns.Add(qb.ToSingleChar("b"));
            var cFrac = qb.ToColumnFraction("c", "dd", true, true, true);
        //cFrac.rows[0].colType=ColTyp.exponent;
        //cFrac.rows[1].colType=ColTyp.exponent;
            denExp.columns.Add(cFrac);

            fenExp.columns.Add(qb.ToSingleChar("f"));
            var fFrac = qb.ToColumnFraction("FF", "GGG", true, true, true);
        //fFrac.rows[0].colType=ColTyp.exponent;
        //fFrac.rows[1].colType=ColTyp.exponent;
            fenExp.columns.Add(fFrac);
            fenExp.columns.Add(qb.ToSingleChar("g"));
            fenExp.columns.Add(qb.ToSingleChar("73"));
            fenExp.columns.Add(qb.ToSingleChar("234"));
            //fenExp.columns.Add(qb.ToSingleChar("34"));
            //fenExp.columns.Add(qb.ToSingleChar("FFF"));


            var brac = new qColumn();
            brac.colType = ColTyp.bracket;
            brac.columns.Add(frac);
            brac.columns.Add(qb.OpColumn("+"));
            brac.columns.Add(qb.ToColumnFraction("some", "fraction", true, true, true));
            //brac.columns.Add(qb.OpColumn("/"));
            //brac.columns.Add(bigA);

            var bracExp = new qColumn(); // 2
            brac.rows.Add(bracExp);
            bracExp.colType = ColTyp.exponent;
            var exy1 = qb.ToColumnFraction("i", "j", true, true, true);
            exy1.rows[0].colType=ColTyp.exponent;
            exy1.rows[1].colType=ColTyp.exponent;
            var exy2 = qb.ToColumnFraction("i", "j", true, true, true);
            exy2.rows[0].colType=ColTyp.exponent;
            exy2.rows[1].colType=ColTyp.exponent;
            var exy3 = qb.ToColumnFraction("i", "j", true, true, true);
            exy3.rows[0].colType=ColTyp.exponent;
            exy3.rows[1].colType=ColTyp.exponent;
            bracExp.columns.Add(exy1);
            bracExp.columns.Add(exy2);
            bracExp.columns.Add(exy3);
            //askColumns.Add(brac);

            // ==== 6

            var brac2 = new qColumn();
            brac2.colType = ColTyp.bracket;
            brac2.columns.Add(brac);

            brac2.columns.Add(qb.OpColumn("/"));
            brac2.columns.Add(bigA);

            var bracExp2 = new qColumn(); // 2
            brac2.rows.Add(bracExp2);
            bracExp2.colType = ColTyp.exponent;
            var ex2 = qb.ToColumnFraction("ppp", "q", true, true, true);
            ex2.rows[0].colType=ColTyp.exponent;
            ex2.rows[1].colType=ColTyp.exponent;
            bracExp2.columns.Add(ex2);
            askColumns.Add(brac2);


            askBuilder.AddColumns(askColumns, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, frac);

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
        }
    }

    //public class q :Question{
    //public q(qid id, qParameters qParams) : base(id, qParams){ }
    //public override void GenerateQuestion() {
    //    var qb = new QuestionBuilder(qParams, queFont);
    //    var askBuilder = new BitmapBuilder();
    //    possibleAnswer possAnswer = new possibleAnswer();
    //
    //    // -- ask
    //    askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));
    //
    //    // -- answer
    //    qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@""));
    //
    //    // -- return
    //    askBitmap=askBuilder.Commit();
    //
    //    // -- Hints
    //}
    //}

    public class qAlgebraTest : Question {
        public qAlgebraTest(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder( qParams, queFont );
            var askBuilder = new BitmapBuilder();
            possibleAnswer possAnswer = new possibleAnswer();


            //var fr1 = qb.ToColumnFraction("a", "b", true, true, true);
            var fr2 = qb.ToColumnFraction("a", "b", true, true, true);
            //var fr3 = qb.ToColumnFraction("a", "b", true, true, true);
            var fr4 = qb.ToColumnFraction("a", "b", true, true, true);
            //var fr5 = qb.ToColumnFraction("a", "b", true, true, true);
            var fr6 = qb.ToColumnFraction("a", "b", true, true, true);
            var fr7 = qb.ToColumnFraction("a", "b", true, true, true);
            //var fr8 = qb.ToColumnFraction("a", "b", true, true, true);
            var fr9 = qb.ToColumnFraction("a", "b", true, true, true);
            //var fr10 = qb.ToColumnFraction("a", "b", true, true, true);

            var fr1 = new qColumn();
            var ar1 = new qColumn();
            var ar2 = new qColumn();
            var ar3 = new qColumn();
            var ar4 = new qColumn();
            // BLOW CHUNKS -----------------
            //ar1.rowChunks.Add(new qChunk(0, 0, "x", true));
            //ar2.rowChunks.Add(new qChunk(0, 0, "y", true));
            //ar3.rowChunks.Add(new qChunk(0, 0, "z", true));

            //new bits
            ar1.nodeValue = "x";
            ar2.nodeValue = "y";
            ar3.nodeValue = "z";
            ar1.answered = true;
            ar2.answered = true;
            ar3.answered = true;

            fr1.rows.Add(ar1);
            fr1.rows.Add(ar2);
            fr1.rows.Add(ar3);
            fr1.showDiv=false;


            var rtn = new List<qColumn>();
            rtn.Add(qb.BracketWrap(fr1));
            rtn.Add(qb.OpColumn("+"));

            var root2 = new qColumn();
            root2.colType=ColTyp.rooted;
            root2.Hint = "Second";
            var fr3 = new qColumn();
                ar1 = new qColumn();
                ar2 = new qColumn();
                ar3 = new qColumn();
            // BLOW CHUNKS -----------------
            //ar1.rowChunks.Add(new qChunk(0, 0, "w", true));
            //ar2.rowChunks.Add(new qChunk(0, 0, "x", true));
            //ar3.rowChunks.Add(new qChunk(0, 0, "y", true));
            //ar4.rowChunks.Add(new qChunk(0, 0, "z", true));

            //new bits
            ar1.nodeValue = "w";
            ar2.nodeValue = "x";
            ar3.nodeValue = "y";
            ar4.nodeValue = "z";
            ar1.answered =true;
            ar2.answered =true;
            ar3.answered =true;
            ar4.answered =true;

            fr3.rows.Add(ar1);
            fr3.rows.Add(ar2);
            fr3.rows.Add(ar3);
            fr3.rows.Add(ar4);
            fr3.showDiv=false;
            var brcol = new qColumn();
            brcol.colType=ColTyp.bracket;
            brcol.columns.Add(fr3);
            brcol.columns.Add(qb.OpColumn("+"));
            //brcol.answerColumns.Add(fr4);

            var FourRootCol = new qColumn();
            FourRootCol.colType=ColTyp.rooted;
            FourRootCol.columns.Add(fr4);
            FourRootCol.Hint="Fourth";
            brcol.columns.Add(FourRootCol);


            root2.columns.Add(brcol);


            var root1 = new qColumn();
            root1.colType=ColTyp.rooted;
            root1.Hint="First";
            var brcol2 = new qColumn();
            brcol2.colType=ColTyp.bracket;
            brcol2.columns.Add(fr2);
            brcol2.columns.Add(qb.OpColumn("+"));
            brcol2.columns.Add(root2);
            root1.columns.Add(brcol2);

            //root1.answerColumns.Add(fr2);
            //root1.answerColumns.Add(qb.OpColumn("+"));
            //root1.answerColumns.Add(root2);
            root1.columns.Add(qb.OpColumn("+"));
            var fr5 = new qColumn();
                ar1 = new qColumn();
                ar2 = new qColumn();
                ar3 = new qColumn();
                ar4 = new qColumn();
            // BLOW CHUNKS -----------------
            //ar1.rowChunks.Add(new qChunk(0, 0, "w", true));
            //ar2.rowChunks.Add(new qChunk(0, 0, "x", true));
            //ar3.rowChunks.Add(new qChunk(0, 0, "y", true));
            //ar4.rowChunks.Add(new qChunk(0, 0, "z", true));

            //new bits
            ar1.nodeValue="w";
            ar2.nodeValue="x";
            ar3.nodeValue="y";
            ar4.nodeValue="z";
            ar1.answered =true;
            ar2.answered =true;
            ar3.answered =true;
            ar4.answered =true;

            fr5.rows.Add(ar1);
            fr5.rows.Add(ar2);
            fr5.rows.Add(ar3);
            fr5.rows.Add(ar4);
            fr5.showDiv=false;
            root1.columns.Add(qb.BracketWrap(fr5));
            root1.columns.Add(qb.OpColumn("+"));
            root1.columns.Add(fr6);

            rtn.Add(root1);
            rtn.Add(qb.OpColumn("+"));
            rtn.Add(qb.BracketWrap(fr7));
            rtn.Add(qb.OpColumn("+"));
            var fr8 = new qColumn();
            ar1 = new qColumn();
            ar2 = new qColumn();
            ar3 = new qColumn();
            // BLOW CHUNKS -----------------
            //ar1.rowChunks.Add(new qChunk(0, 0, "x", true));
            //ar2.rowChunks.Add(new qChunk(0, 0, "y", true));
            //ar3.rowChunks.Add(new qChunk(0, 0, "z", true));

            //new bits
            ar1.nodeValue="x";
            ar2.nodeValue="y";
            ar3.nodeValue="z";
            ar1.answered =true;
            ar2.answered =true;
            ar3.answered =true;

            fr8.rows.Add(ar1);
            fr8.rows.Add(ar2);
            fr8.rows.Add(ar3);
            fr8.showDiv=false;
            rtn.Add(qb.BracketWrap(fr8));
            rtn.Add(qb.OpColumn("+"));

            var root3 = new qColumn();
            root3.colType=ColTyp.rooted;
            root3.Hint="Fourth";
            root3.columns.Add(qb.BracketWrap(fr9));
            rtn.Add(root3);

            askBuilder.AddColumns(rtn, new Point(0, 0));

            //askBuilder.AddColumn(qb.ToColumnFraction("a","b"), new Point(0, 0));
            qb.possibleAnswerFromColumn(this, qb.ToColumnFraction("a","b"));
            //possibleAnswers.Add(possAnswer);

            askBitmap=askBuilder.Commit();
        }
    }
}

//    ...
// interest
//    ...
// compound interest
//    ...
// Int + Decimal + Fraction + brackets + powers TermOpEvaluator
//    ...

