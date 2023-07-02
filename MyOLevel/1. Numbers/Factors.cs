using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MathUtils;

namespace Polish.OLevel.Numbers {
    public class qFactors : Question {
        public qFactors(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            List<int> answers = new List<int>();

            // -- Good prefabs --
            int[] Prefabs = { 6, 8, 10, 12, 13, 15, 16, 17, 18, 20, 21, 22 };
            int number = 0;

            if (Utils.R(1, 5)<=3) {
                number = Prefabs[Utils.R(1, Prefabs.Count())-1];
            } else {

                int mults = qParams.terms;
                qb.rangeInt=qb.RangeSampleInt();

                number=qb.rangeInt[0];
                for (int n = 1; n<mults; n++) {
                    if (number*qb.rangeInt[n]<100) {
                        number*=qb.rangeInt[n];
                    } else {
                        break;
                    }
                }
            }
            //topText = "List the factors of " + number.ToString();

            // -- extract factors --
            for (int f = 2; f<=number/2; f++) // for(i=2 to 81/2)                
                if (number-(number/f)*f==0)          //    if(81 % i == 0)                
                    answers.Add(f);      //       add
            answers.Add(number); // add 81 //ignore 1?

            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            for (int i = 0; i<answers.Count-1; i++) {
                possAnswer.answer.Add(qb.ToSingleInteger($@"{answers[i]}"));
                possAnswer.answer.Add(qb.CommaColumn());
            }
            possAnswer.answer.Add(qb.ToSingleInteger($@"{answers[answers.Count-1]}"));
            possAnswer.IsSequence = true;

            possibleAnswers.Add(possAnswer);

            var askBuilder = new BitmapBuilder();
            //var len = askBuilder.AddTextLine("List the factors of ", qb.alphaFont, new Point(0, 3));
            //askBuilder.NewAddTextLine($@"{number}", qb.numericFont, new Point(len, 0));

            //askBuilder.AddTextDraw("List the factors of", qb.alphaFont, new Point(0, 20));
            //askBuilder.AddTextDraw("List the factors of", qb.alphaFont, new Point(0, 40));
            var mptd = askBuilder.NewMultiPartTextDraw();
            mptd.Add(askBuilder.NewTextDraw("List the factors of ", qb.alphaFont, new Point(0, 0)));
            mptd.Add(askBuilder.NewTextDraw($@"{number}", qb.numericFont, new Point(0, 0)));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qPrimeFactorProducts : Question {
        public qPrimeFactorProducts(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            List<int> primes = new List<int>();
            List<int> answers = new List<int>();

            //number
            int number = Utils.R(10, 50);
            //topText = $@"Express{qb.br( )}{number}{qb.br( )}as a product of prime factors";

            //prime factors
            //botText.Add(new qChunk(id, 0, "(1), ", true));
            if (Utils.IsPrime(number)) {
                //botText.Add(new qChunk(id, 1, number.ToString(), false));
                possAnswer.answer.Add(qb.ToSingleInteger($@"{number}"));
            } else {
                answers=Utils.PrimeFactors(number).ToList();

                int j = 0;
                //for(int i=0;i<answers.Count;i++){
                //    botText.Add(new answerChunk(id, j++, $@"{answers[i]}",false));
                //    botText.Add(new answerChunk(id, j++, ", ",true));
                //}
                //botText.RemoveAt(botText.Count-1); // comma
                for (int i = 0; i<answers.Count-1; i++) {
                    possAnswer.answer.Add(qb.ToSingleInteger($@"{answers[i]}"));
                    possAnswer.answer.Add(qb.CommaColumn());
                }
                possAnswer.answer.Add(qb.ToSingleInteger($@"{answers[answers.Count-1]}"));
            }

            //qb.ChunkRowToSingleInteger( this );
            possAnswer.IsSequence = true;
            possibleAnswers.Add(possAnswer);

            //int len = askBuilder.OldAddTextLine("Express", qb.alphaFont, new Point(0, 0));
            //len += askBuilder.OldAddTextLine($@"{number}", qb.numericFont, new Point(len, 0));
            //askBuilder.OldAddTextLine("as a product of prime factors", qb.alphaFont, new Point(len, 0));
            var mptd = askBuilder.NewMultiPartTextDraw();
            mptd.Add(askBuilder.NewTextDraw("Express", qb.alphaFont, new Point(0, 0)));
            mptd.Add(askBuilder.NewTextDraw($@"{number}", qb.numericFont, new Point(0, 0)));
            mptd.Add(askBuilder.NewTextDraw("as a product", qb.alphaFont, new Point(0, 0)));
            askBuilder.AddTextDraw("of prime factors", qb.alphaFont, new Point(0, qb.lineSpace));

            askBitmap=askBuilder.Commit();
        }
    }
    public class qHighestCommonFactor : Question {
        public qHighestCommonFactor(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();
            // -- generate --
            int[] srcs = new int[qParams.terms];
            //TODO: Allow failures (HCF==1)

            srcs=qb.RangeSampleInt();

            // -- create set --
            int[] trgs = new int[srcs.Count()];
            int src = srcs[1];
            int max = 0;
            for (int i = 0; i<srcs.Count(); i++) {
                trgs[i]=src*srcs[i];
                if (trgs[i]>max) max=trgs[i];
            }

            // -- calculate HCF --
            int hcf = Utils.hcf(trgs);

            // -- store --
            //qb.sbTop.Append($@"The highest common factor of ");
            askBuilder.AddTextDraw("The highest common factor of ", qb.alphaFont, new Point(0, 0));
            for (int i = 0; i<trgs.Count(); i++)
                qb.sbTop.Append($@"{trgs[i]}, ");
            string tmp = $@"{qb.sbTop}";
            topText = tmp.Substring(0, tmp.Length-2);
            askBuilder.AddTextDraw($@"{qb.sbTop}", qb.alphaFont, new Point(0, qb.lineSpace));

            //botText.Add(new qChunk(id, 0, $@"{hcf}", false));
            //qb.ChunkRowToSingleInteger( this );

            //askBuilder.AddTextDraw(topText, qb.alphaFont, new Point(0,0));
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger(hcf));

            askBitmap=askBuilder.Commit();
        }
    }
}
