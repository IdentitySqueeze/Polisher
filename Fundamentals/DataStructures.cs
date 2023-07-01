using System.Collections.Generic;
using System.Drawing;
using Fangorn;

namespace Polish {
    #region notes
    //   Column (empty rows, always bracketted )
    //       Column (bracketted)
    //          Rows (2)
    //                  op
    //                     Column
    //                         Rows (1)
    //                            op
    //                              Column
    //                               Rows (1)
    //                                           op
    //                                              Column
    //                                                Rows (2)
    //    /  -                -            \
    //   |   |   ab3c         |             |        3h4n
    //   /   |                |              \
    //  |    |  ------  *  4  |  -  2b3c     |   *  ------
    //   \   |                |              /
    //   |   |   2xyz         |              |        4mf
    //    \  |_              _|             /
    //    
    // 
    //                                  answer
    //                          /                      \
    //                     Column { }      *        Column
    //                    /            \             /      \   
    //               Column [ ]  -   Column   Row  /  Row
    //              /        \            |
    //         Column  *  Column   Row 
    //        /       \         |
    //    Row   /    Row   Row
    // 
    //
    #endregion
    public class possibleAnswer {
        public List<qColumn> answer { get; set; } = new List<qColumn> { };
        public bool IsSequence { get; set; } = false;
        public bool uniformSize { get; set; } = false;
    }

    public class newNode : INode<newNode>, IRenderNode<newNode> {
        public List<newNode> columns { get; set; } = new List<newNode> { };
        public List<newNode> rows { get; set; } = new List<newNode> { };
        public string nodeValue { get; set; }
        public bool IsColumn => throw new System.NotImplementedException();
        //public bool IsLeaf => (columns.Count==0);
        public bool IsLeaf => nodeValue != null;
        public char? op { get; set; }
        public bool answered { get; set; }
        public ansType ansType { get; set; }
        public ColTyp colType { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public bool Infinity { get; set; }
        public newNode parent { get; set; }
        public bool showDiv { get; set; }
        public int rowLen { get; set; }
        public int rowExpLen { get; set; }
        public Rectangle rowRect { get; set; }
        public int charCount { get; set; }
        public List<CharacterRange> toBlock { get; set; } = new List<CharacterRange> { };
        public Rectangle blockRect { get; set; }
        public Rectangle boundsRect { get; set; }
        public Region[] blockRegions { get; set; }

        public bool decorated => throw new System.NotImplementedException();
    }
    public class qColumn : INode<qColumn>, IRenderNode<qColumn> {
        public ColTyp colType { get; set; } = ColTyp.fraction;

        #region -- sigma, product, integrals, choose ∞
        public int from { get; set; }
        public int to { get; set; }
        public bool Infinity { get; set; } = false;
        #endregion

        public string Hint { get; set; } = "Be more happy.";
        public bool showDiv { get; set; } = true;
        public int outerMax { get; set; }
        public int maxRows { get; set; }
        public int rootDepth { get; set; }
        public int rootsInsideMe { get; set; }
        public int consecs { get; set; }

        public List<qColumn> columns { get; set; } = new List<qColumn>();     // LC.
        public List<qColumn> rows { get; set; } = new List<qColumn>();        // RS.  If I'm a column, count for height.

        public string nodeValue { get; set; }
        public string Leaf { get; set; }
        public bool answered { get; set; }
        // -- Both --
        //public bool IsColumn => (colType!=ColTyp.exponent) && (rows.Count==0 )|| 
        //                        (colType==ColTyp.bracket && rows.Count==1);
        public bool IsColumn => (rows.Count==0 );
        public bool IsLeaf => (columns.Count==0) && (colType & (ColTyp.bracket | ColTyp.rooted))==0;
        public bool decorated => (colType & (ColTyp.bracket | ColTyp.rooted))>0;
        public bool InSequence { get; set; }
        public char? op { get; set; }
        public ansType ansType { get; set; } = ansType.Num;
        public qColumn parent { get; set; }

        public int rowLen { get; set; }                                  //If I'm a row, use wholeRow.
        public int rowExpLen { get; set; }
        public Rectangle rowRect { get; set; } = Rectangle.Empty;
        public int charCount { get; set; } = 0;

        public List<CharacterRange> toBlock { get; set; } = new List<CharacterRange> { };
        public Rectangle blockRect { get; set; } //= Rectangle.Empty;
        public Rectangle boundsRect { get; set; } //= Rectangle.Empty;
        public Region[] blockRegions { get; set; }
    }

    public class bracketColumn : qColumn {
        public bracketColumn() {
            colType |= ColTyp.bracket;
        }
    }
}
