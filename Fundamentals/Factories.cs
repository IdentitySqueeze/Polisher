using System;
using System.Linq;
using Polish.OLevel.Numbers;

namespace Polish {
    public abstract class qQuestionFactory : IQuestionFactory {
        public object[] args;
        public string Title { get; set; }
        public IQuestion Request(int id, qParameters qParams, int option = 0) {
            args = new object[] { id, qParams };
            QuestionList ql = (QuestionList)Attribute.GetCustomAttributes(this.GetType())[0];//TODO: Loop, don't index
            if (option<1 || option>ql.questions.Count()) option=1;
            Type question = ql.questions[option-1];
            return (IQuestion)question.GetConstructor(args.Select(q => q.GetType()).ToArray()).Invoke(args);
        }
    }

    // -- Attribute Classes --
    public class QuestionList : Attribute {
        public Type[] questions { get; set; }
        public QuestionList(params Type[] types) => questions=types;
        public Type[] GetQuestions() => questions;
    }
    public class NaturalName : Attribute {
        public string naturalName { get; set; }
        public NaturalName(string pName) => naturalName=pName;
    }




    //typeof(qBasicArithmetic), 
    [QuestionList(new Type[]{ typeof(qLowestCommonMultiple),
                              typeof(qBasicAddSubtract),  typeof(qBasicMultiplyDivide),
                              typeof(qDecimalPlaces),     typeof(qSignificantFigures),
                              typeof(qStandardFormX),     typeof(qStandardFormSF),
                              typeof(qStandardFormMixed), typeof(qStandardFormArithmetic), typeof(qAveragesMean),
                              typeof(qFractionMultiplyUp), typeof(qFractionDivideDown),

                             })]
    //[NaturalName("Basic Arithmetic")]
    public class qBasicArithmeticFactory : qQuestionFactory { }//(!)

    //[QuestionList(new Type[]{ typeof(qAveragesMean), })]//typeof(qAveragesMeanHowMany), 
    //typeof(qAveragesMeanCompose), typeof(qAveragesMeanSubset), 
    //typeof(qAveragesMeanSubtract), })]//, NaturalName("Averages")]
    //public class qAveragesFactory : qQuestionFactory{}


    [QuestionList(new Type[] { typeof(qFactors), typeof(qPrimeFactorProducts), typeof(qHighestCommonFactor) })]//, NaturalName("Factors")]
    public class qFactorsFactory : qQuestionFactory { }

    //typeof(qFractionBasicArithmetic), typeof(qFractionLowestCommonDenominator), 
    [QuestionList(new Type[]{ //typeof(qFractionReduce), 
                              typeof(qFractionToMixed),    typeof(qFractionTopHeavy ),    //typeof(qFractionCancel),
                              typeof(qFractionLowestCommonDenominator), typeof(qFractionOf),
                              typeof(qFractionAddition),   typeof(qFractionSubtract),
                              typeof(qFractionMultiply),   typeof(qFractionDivision),      })]//, NaturalName("Fractions")]
    public class qFractionsFactory : qQuestionFactory { }

    //typeof(qDecimalLongMultiply), typeof(qDecimalLongDivide),    
    [QuestionList(new Type[]{ typeof(qDecimalAdd),       typeof(qDecimalSubtract),     typeof(qDecimalMultiply),
                              typeof(qDecimalDivide),
                              typeof(qFraction2Decimal), typeof(qDecimal2Fraction), })]//, NaturalName("Decimals")]
    public class qDecimalsFactory : qQuestionFactory { }


    [QuestionList(new Type[]{ typeof(qFraction2Ratio), typeof(qRatio2Fraction), typeof(qRatioPencePound),
                              typeof(qRatioMixed),     typeof(qRatioFind),      //typeof(qRatioDistribute),  
                              typeof(qRatioDivide),    typeof(qProportionBuysCosts), typeof(qProportionThingMoves),
                              typeof(qRatioDecimals),  typeof(qRatioFractions),})]//, NaturalName("Ratios")]
    public class qRatioProportionFactory : qQuestionFactory { }

    [QuestionList(new Type[]{ typeof(qFraction2Percent),    typeof(qPercent2Fraction),    typeof(qDecimal2Percent),
                              typeof(qPercent2Decimal),     typeof(qPercentNoResult),     typeof(qPercentNoNumber),
                              typeof(qPercentNoPercentage), typeof(qPercentProfitLoss),   typeof(qPercentDiscount),
                              typeof(qPercentChangeNoCost), typeof(qPercentChangeNoSell),
                              typeof(qInterest), typeof(qCompounding),})]//typeof(qPercentChangeNoPL),  })]//, NaturalName("Percentages")]
    public class qPercentagesFactory : qQuestionFactory { }    
    
    [QuestionList(new Type[]{ //typeof(qPowersSquaresCubes), 
                              typeof(qPowersNthPowers),// typeof(qPowersPlus), typeof(qPowersMinus),        
                              typeof(qPowersMultiply),     typeof(qPowersDivide),
                              typeof(qPowersNegative),     typeof(qPowersFractionˣ), typeof(qPowersXᶠʳᵃᶜᵗⁱᵒⁿ),
                              typeof(qPowersⁿᵗʰRoot),      typeof(qPowersBrackets), })]
    public class qPowersFactory : qQuestionFactory{ }

    [QuestionList(new Type[]{ typeof(qRootsSquaresCubes),  typeof(qRootsNthRoots),     typeof(qRootsRootXʸ), 
                              typeof(qRootsⁿᵗʰRootXʸ),     typeof(qRootsOfFraction),   typeof(qRootsProduct),
                              typeof(qSurdsSimplify),      typeof(qSurdsMultiply),     typeof(qSurdsFractional),
                              //typeof(qSurdsExpandSimplify1),typeof(qSurdsExpandSimplify2),typeof(qSurdsExpandSimplify3),
                              //typeof(qSurdsExpandSimplify4), typeof(qSurdsBracketSquared),typeof(qSurdsDiff2Squares),
                              typeof(qSurdsRationaliseDenom),})]

    public class qRootsSurds : qQuestionFactory{ }

    [QuestionList(new Type[]{ typeof(qLogsIndex2Log),   typeof(qLogsLog2Index),
                              typeof(qLogsAddition),   typeof(qLogsSubtraction),
                              //typeof(qLogsEvaluate), typeof(qLogsExpressTerms),typeof(qLogsSimplify),
                              })]
    public class qLogs:qQuestionFactory{ }


    [QuestionList(new Type[] { typeof(qSeqAPFindnthTerm1), typeof(qSeqAPFindnthTerm2),
                               typeof(qSeqAPFindnmthTerms), typeof(qSeqAPFind3rdTerm), typeof(qSeqAPWhichTerm),
                               typeof(qSeqAPArithmeticMeans), typeof(qSeqAPSum1), typeof(qSeqAPSum2),
                               typeof(qSeqAPSum3), typeof(qSeqAPSumRange), typeof(qSeqHPFindnthTerm),
                               typeof(qSeqGPFindnthTerm1), typeof(qSeqGPFindnmthTerms), typeof(qSeqGPFind3rdTerm1),
                               typeof(qSeqGPFind3rdTerm2), typeof(qSeqGPGeometicMeans), typeof(qSeqGPSum1),
                               typeof(qSigma1)
    })]
    public class qSequences : qQuestionFactory { }


    [QuestionList(new Type[]{ typeof(qTestQuestion) })]
    public class qTest: qQuestionFactory{ }


    [QuestionList(new Type[] { typeof(qTestQuestionNew) })]
    public class qTestNew : qQuestionFactory { }


    //[QuestionList(new Type[]{ typeof(qTriangleTest) })]
    //public class qPictures: qQuestionFactory{ }


    //[QuestionList(new Type[] { typeof(qAlgebraTest) })]
    //public class qAlgebraFactory : qQuestionFactory { }
}
