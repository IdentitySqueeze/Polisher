using System.Collections.Generic;
using System.Drawing;
using Fangorn;

namespace Polish {
    public interface IQuestionFactory { }
    public interface IQuestion
    {
        List<possibleAnswer<qColumn>> possibleAnswers { get; set; } 
        string topText { get; set; }
        //List<qChunk> botText { get; set; }
        qParameters qParams { get; set; }
        string Hints{ get; set; }
        void GenerateQuestion();
        //QuestionType questionType { get; set; }
        Bitmap askBitmap {get;set; }
        Bitmap hintsBitmap { get; set; }
        string qTitle {get;set; }
        int id { get; set; }
        Genus genus { get; set; }

        //int[] rangeInt {get; set; }
    }
}
