using System;
using System.Drawing;
using MathUtils;

namespace Polish.OLevel.Numbers {
    //BS for more
    public class qLogsIndex2Log : Question {
        public qLogsIndex2Log(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int logBase = Utils.R(2, 10);
            int logPower = Utils.R(2, 6);
            int num = (int)Math.Pow((double)logBase, logPower);

            // -- ask 
            askBuilder.AddTextDraw($@"Express {logBase}{GraphicsUtils.ToSuper(""+logPower)} in log notation", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"log{GraphicsUtils.ToSub(""+logBase)}{num}={logPower}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints = $@"log{GraphicsUtils.ToSub("a")} b=c   ==   a{GraphicsUtils.ToSuper("c")}=b";
        }
    }
    //BS for more
    public class qLogsLog2Index : Question {
        public qLogsLog2Index(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int logBase = Utils.R(2, 10);
            int logPower = Utils.R(2, 6);
            int num = (int)Math.Pow((double)logBase, logPower);

            // -- ask 
            askBuilder.AddTextDraw($@"Express log{GraphicsUtils.ToSub(""+logBase)}{num}={logPower} in index notation", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"{logBase}{GraphicsUtils.ToSuper(""+logPower)}={num}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints = $@"log{GraphicsUtils.ToSub("a")} b=c    ==    a{GraphicsUtils.ToSuper("c")}=b";
        }
    }
    public class qLogsAddition : Question {
        public qLogsAddition(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {

            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int logBase = Utils.R(2, 3);
            int logPower1 = Utils.R(2, 5);
            int num1 = (int)Math.Pow((double)logBase, logPower1);
            int logPower2 = Utils.R(2, 6);
            int num2 = (int)Math.Pow((double)logBase, logPower2);

            // -- ask 
            askBuilder.AddTextDraw($@"Evaluate log{GraphicsUtils.ToSub(""+logBase)}{num1} + log{GraphicsUtils.ToSub(""+logBase)}{num2}", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"log{GraphicsUtils.ToSub(""+logBase)}{num1*num2}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints = $@"log{GraphicsUtils.ToSub("a")}x + log{GraphicsUtils.ToSub("a")}y = log{GraphicsUtils.ToSub("a")} (xy)";
        }
    }
    public class qLogsSubtraction : Question {
        public qLogsSubtraction(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {

            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            //var sets = new int[,] { {2,4 },{2,8  },{2,16  },{2,32  },
            //                        {3,9 },{3,27 },{3, 81 },{3,243 },
            //                        {4,16},{4,64 },{4,256 },{4,1024},
            //                        {5,25},{5,125},{5, 625},{5,3125},
            //                        {6,36},{6,216},{6,1296},{6,7776}
            //};
            //int ix = Utils.r(1, 18)-1;
            //int num1 = sets[ix, Utils.r(1,5)-1];
            //int num2 = sets[ix, 0];

            int logBase = Utils.R(2, 3);
            int logPower1 = Utils.R(2, 5);
            int num1 = (int)Math.Pow((double)logBase, logPower1);
            int logPower2 = Utils.R(2, 6);
            int num2 = (int)Math.Pow((double)logBase, logPower2);

            // -- ask 
            askBuilder.AddTextDraw($@"Evaluate log{GraphicsUtils.ToSub(""+logBase)}{num1} - log{GraphicsUtils.ToSub(""+logBase)}{num2}", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@"log{GraphicsUtils.ToSub(""+logBase)}{(double)num1/(double)num2}"));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            var hintBuilder = askBuilder.Reset();
            hintBuilder.backColor=Color.White;
            hintBuilder.AddTextDraw($@"log{GraphicsUtils.ToSub("a")}x-log{GraphicsUtils.ToSub("a")}y = log{GraphicsUtils.ToSub("a")} ", qb.alphaFont, new Point(15, 15));
            hintBuilder.AddColumn(qb.ToColumnFraction("x", "y", true, true, true), new Point(180, 0));
            hintsBitmap =hintBuilder.Commit();
        }
    }

    //AS
    public class qLogsTakeLogs : Question {
        public qLogsTakeLogs(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@""));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
        }
    }
    //AS
    public class qLogsTakExponents : Question {
        public qLogsTakExponents(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            // -- ask
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@""));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- Hints
        }
    }
    //BC
    public class qLogsEvaluate : Question {
        public qLogsEvaluate(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int logBase = Utils.R(2, 10);
            int logPower = Utils.R(2, 6);
            int num = (int)Math.Pow((double)logBase, logPower);

            // -- ask 
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@""));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints = $@"";
        }
    }
    //BC
    public class qLogsExpressTerms : Question {
        public qLogsExpressTerms(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int logBase = Utils.R(2, 10);
            int logPower = Utils.R(2, 6);
            int num = (int)Math.Pow((double)logBase, logPower);

            // -- ask 
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@""));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints = $@"";

        }
    }
    //BC
    public class qLogsSimplify : Question {
        public qLogsSimplify(int id, qParameters qParams) : base(id, qParams) { }
        public override void GenerateQuestion() {
            var qb = new QuestionBuilder(qParams, queFont);
            var askBuilder = new BitmapBuilder();
            possibleAnswer<qColumn> possAnswer = new possibleAnswer<qColumn>();

            int logBase = Utils.R(2, 10);
            int logPower = Utils.R(2, 6);
            int num = (int)Math.Pow((double)logBase, logPower);

            // -- ask 
            askBuilder.AddTextDraw($@"", qb.alphaFont, new Point(0, 0));

            // -- answer
            qb.possibleAnswerFromColumn(this, qb.ToSingleInteger($@""));

            // -- return
            askBitmap=askBuilder.Commit();

            // -- hints
            Hints = $@"";

        }
    }
}
