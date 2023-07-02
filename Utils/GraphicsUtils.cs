using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using MathUtils;
using System.Threading.Tasks;
using Fangorn;
using Polish;

namespace Polish {
    public class BitmapBuilder {
        public BitmapBuilder() { g=Graphics.FromImage(gBitmap); MeasureLetterHeights(); }
        ~BitmapBuilder() => g.Dispose();
        public int width { get; set; }
        public int height { get; set; }

        #region // -- Draw queuing stuff --
        List<PathDraw> PathDraws = new List<PathDraw>() { };
        List<BezierDraw> BezierDraws = new List<BezierDraw>() { };
        List<CurveDraw> CurveDraws = new List<CurveDraw>() { };
        List<ClosedCurveDraw> ClosedCurveDraws = new List<ClosedCurveDraw>() { };
        List<LineDraw> LineDraws = new List<LineDraw>() { };
        List<TextDraw> TextDraws = new List<TextDraw>() { };
        List<ImageDraw> ImageDraws = new List<ImageDraw>() { };
        List<MultiPartTextDraw> MultiPartTextDraws = new List<MultiPartTextDraw>();
        #endregion

        #region vars
        public Bitmap gBitmap { get; set; } = new Bitmap(500, 500);
        public Rectangle nullRect = new Rectangle(0, 0, 500, 500);
        public Color backColor { get; set; } = Color.White;
        public Graphics g { get; set; }
        public Genus genus { get; set; } = Genus.OriginalService;
        public int letterHeight { get; set; } = 15;
        public int lineSpace { get; set; } = 10;
        public int expLetterHeight { get; set; } = 8;
        public int expLineSpace { get; set; } = 5;
        public Font ConsolasFont { get; set; } = new Font("Consolas", 11);
        public Font ExponentFont { get; set; } = new Font("Consolas", 8);
        public MeasureArgsRtns measureRtn { get; set; }
        public SizeArgsRtns sizeRtn { get; set; }
        public DrawArgsRtns drawRtn { get; set; }

        public ITreeWalker<qColumn, MeasureArgsRtns> measureWalker { get; set; }
        public ITreeWalker<qColumn, SizeArgsRtns> layoutWalker { get; set; }
        public ITreeWalker<qColumn, DrawArgsRtns> drawWalker { get; set; }
        public decimal maxBracket { get; set; } = 0m;
        public int rootDepth { get; set; }
        #endregion

        public void StackIncrement(Stack<rootStacker> stack) {
            var tmp = new Stack<rootStacker> { };
            var rs = new rootStacker();
            while (stack.Count>0) {
                rs=stack.Pop();
                rs.innersCount++;
                rs.consecs=Math.Max(rs.consecs, rs.innersCount);
                tmp.Push(rs);
            }
            while (tmp.Count>0) stack.Push(tmp.Pop());
        }
        public void StackDecrement(Stack<rootStacker> stack) {
            var tmp = new Stack<rootStacker> { };
            var rs = new rootStacker();
            while (stack.Count>0) {
                rs=stack.Pop();
                rs.innersCount--;
                tmp.Push(rs);
            }
            while (tmp.Count>0) stack.Push(tmp.Pop());
        }

        public int GetStringLength(string txt, Font font) {
            return GraphicsUtils.GetStringLength(txt, g, font, nullRect, null, null)+10;
        }

        #region // -- Add queued ops --
        public void NewPathDraw(Point[] points, Pen pen) {
            PathDraws.Add(new PathDraw(points, pen));
        }
        public PathDraw AddPathDraw(Point[] points, Pen pen) {
            return new PathDraw(points, pen);
        }
        public void NewBezierDraw(Point[] points, Pen pen) {
            BezierDraws.Add(new BezierDraw(pen, points));
        }
        public BezierDraw AddBezierDraw(Point[] points, Pen pen) {
            return new BezierDraw(pen, points);
        }
        public void NewCurveDraw(Point[] points, Pen pen, float tension) {
            CurveDraws.Add(new CurveDraw(pen, points, tension));
        }
        public CurveDraw AddCurveDraw(Point[] points, Pen pen, float tension) {
            return new CurveDraw(pen, points, tension);
        }
        public void NewClosedCurveDraw(Pen pen, Point[] points) {
            ClosedCurveDraws.Add(new ClosedCurveDraw(pen, points));
        }
        public ClosedCurveDraw AddClosedCurveDraw(Pen pen, Point[] points) {
            return new ClosedCurveDraw(pen, points);
        }
        public LineDraw NewLineDraw(Pen pen, Point start, Point finish) {
            return new LineDraw(pen, start, finish);
        }
        public void AddLineDraw(Pen pen, Point start, Point finish) {
            LineDraws.Add(new LineDraw(pen, start, finish));
        }
        public TextDraw NewTextDraw(string txt, Font font, Point p) {
            return new TextDraw(txt, p, font, Brushes.Black, GetStringLength(txt, font));
        }
        public void AddTextDraw(string txt, Font font, Point p) {
            TextDraws.Add(NewTextDraw(txt, font, p));
        }
        public MultiPartTextDraw NewMultiPartTextDraw() {
            var mptd = new MultiPartTextDraw();
            MultiPartTextDraws.Add(mptd);
            return mptd;
        }
        #endregion

        public void AddImage(Bitmap bmp, Point p) { }
        //
        // How many blah blah blah does it take to blah blah blah 
        //                                                                  y             
        //      6                                    /|  C       |          |     |          
        // 1)   -                   c          /    \_|           |         |    |         
        //      4                         /           |            |        |   |          
        //                           /                |  b          \       |  /              
        //     22               /                   --|         ------------0----------- x
        // 2)  --      A   /  \                    |  |  B             \____|/               
        //     16         -----------------------------                     |                          
        //                                                                  |    y=x^3                        
        //     23                      a                                    |                              
        // 3) ---                                                           |                            
        //    100         blah blah blah question                     how blah blah
        //                             
        // anticipate every math question I could ask myself, into a wieldable Bitmap creator
        //

        public void AddColumn(qColumn ans, Point p) {
            var ansList = new List<qColumn>();
            ansList.Add(ans);
            AddColumns(ansList, p);
        }
        public void AddColumnAsNode(qColumn ansNode, Point p, bool uniformSize = false) {
            //These have Point()'s threaded into them, the front end doesn't

            #region -- measureWalker
            var measureFactory = new Treebeard<qColumn, qColumn>.MeasureFactory<qColumn, MeasureArgsRtns>();
            measureWalker = (ITreeWalker<qColumn, MeasureArgsRtns>)measureFactory.GetWalker(genus);
            measureRtn = new MeasureArgsRtns();
            measureFactory.GetWalkerOps(genus, measureWalker);
            measureRtn.Height = letterHeight;
            measureRtn = measureWalker.Traverse(ansNode, measureRtn); //<-- assigns rowLens
            #endregion

            #region -- sizeWalker
            var layoutFactory = new Treebeard<qColumn, qColumn>.LayoutFactory<qColumn, SizeArgsRtns>();
            layoutWalker =  (ITreeWalker<qColumn, SizeArgsRtns>)layoutFactory.GetWalker(genus);
            layoutFactory.GetWalkerOps(genus, layoutWalker);
            sizeRtn = new SizeArgsRtns {
                Height = measureRtn.Height, // PROVISIONAL
                maxRootDepth = measureRtn.maxRootDepth,
                TopLeft=p,
            };
            if (uniformSize) {
                sizeRtn.uniformSize = uniformSize;
                sizeRtn.uniformedWidth = 50;// ansList.Max(ansCol => ansCol.rows.Max(ansRow => ansRow.rowLen)); // PROVISIONAL
            }
            sizeRtn = layoutWalker.Traverse(ansNode, sizeRtn); //<-- creates rowRects, does cum width & uniforming
                                                               //<-- rowRects not used as absolute positions: modified with TopLeft offsets in draw
            #endregion

            #region -- drawWalker
            var drawFactory = new Treebeard<qColumn, qColumn>.DrawFactory<qColumn, DrawArgsRtns>();
            drawWalker = (ITreeWalker<qColumn, DrawArgsRtns>)drawFactory.GetWalker(genus);
            drawRtn = new DrawArgsRtns {
                Selected = false,
                Width = sizeRtn.Width,    // cum?
                Height = sizeRtn.Height,  // ?
                maxRootDepth=sizeRtn.maxRootDepth
            };
            //drawWalker.PreColumnOp = DrawPreColumnOp;
            drawWalker.RowOp = DrawProcessRowsGraphicsUtils;
            //drawWalker.PostColumnOp = DrawPostColumnOp;
            drawRtn.ansBackBuffer = g;
            drawRtn = drawWalker.Traverse(ansNode, drawRtn);
            #endregion
            // Draw    : Don't calculate, just draw
        }

        public void AddColumns(List<qColumn> ansList, Point p, bool uniformSize = false) {
            //These have Point()'s threaded into them, the front end doesn't

            // Measure : Individual Widths, Heights, stringLengths, rowLengths, letterHeights.
            //           Overall Width, Height..?
            // Size    : Layout - X, Y start coordinates for everything
            //           Overall Width, Height..?
            // Draw    : Don't think, draw

            #region -- measureWalker
            var measureFactory = new Treebeard<qColumn, qColumn>.MeasureFactory<qColumn, MeasureArgsRtns>();
            //measureWalker = new TreeWalker<qColumn, MeasureArgsRtns>();
            measureWalker = (ITreeWalker<qColumn, MeasureArgsRtns>)measureFactory.GetWalker(genus);
            measureRtn = new MeasureArgsRtns();
            measureFactory.GetWalkerOps(genus, measureWalker);

            //measureWalker.RowOp = MeasureProcessRows;
            //measureWalker.PreColumnOp = MeasurePreColumn;
            //measureWalker.PostColumnOp =MeasurePostColumn;
            measureRtn.Height = letterHeight;
            measureRtn = measureWalker.Traverse(ansList, measureRtn); //<-- assigns rowLens
            #endregion
            // Measure : Widths, Heights, stringLengths, rowLengths, letterHeights

            #region -- sizeWalker
            var layoutFactory = new Treebeard<qColumn, qColumn>.LayoutFactory<qColumn, SizeArgsRtns>();
            //layoutWalker = new OriginalTreeWalker<qColumn, SizeArgsRtns>();
            layoutWalker =  (ITreeWalker<qColumn, SizeArgsRtns>)layoutFactory.GetWalker(genus);

            layoutFactory.GetWalkerOps(genus, layoutWalker);

            //layoutWalker.PreTraversalOp = SizePreTraversalOp;
            //layoutWalker.PreColumnOp = SizePreColumnOp;
            //layoutWalker.RowOp = SizeProcessRows;
            //layoutWalker.PostColumnOp = SizePostColumnOp;

            sizeRtn = new SizeArgsRtns {
                Height = measureRtn.Height, // PROVISIONAL
                maxRootDepth = measureRtn.maxRootDepth,
                TopLeft=p,
            };
            if (uniformSize) {
                sizeRtn.uniformSize = uniformSize;
                sizeRtn.uniformedWidth = 50;// ansList.Max(ansCol => ansCol.rows.Max(ansRow => ansRow.rowLen)); // PROVISIONAL
            }
            sizeRtn = layoutWalker.Traverse(ansList, sizeRtn); //<-- creates answerColumns' rowRects, does cum width & any ex post uniforming
                                                               //<-- and is going to have to create bracket columns' rowRects
                                                               //<-- rowRects not used as absolute positions: modified with TopLeft offsets in draw
            #endregion
            // Size    : Layout - X, Y start coordinates for everything

            #region -- drawWalker
            //var drawFactory = new Original<qColumn, qColumn>.DrawFactoryUnanswerable<qColumn, DrawArgsRtns>();
            var drawFactory = new Treebeard<qColumn, qColumn>.DrawFactory<qColumn, DrawArgsRtns>();
            drawWalker = (ITreeWalker<qColumn, DrawArgsRtns>)drawFactory.GetWalker(genus);
            //drawFactory.GetWalkerOps(Genus.OriginalClient, drawWalker);

            //drawWalker = new TreeWalker<qColumn, DrawArgsRtns>();

            drawRtn = new DrawArgsRtns {
                Selected = false,
                Width = sizeRtn.Width,    // cum?
                Height = sizeRtn.Height,  // ?
                maxRootDepth=sizeRtn.maxRootDepth
            };
            //drawWalker.PreColumnOp = DrawPreColumnOp;
            drawWalker.RowOp = DrawProcessRowsGraphicsUtils;
            //drawWalker.PostColumnOp = DrawPostColumnOp;
            drawRtn.ansBackBuffer = g;
            drawRtn = drawWalker.Traverse(ansList, drawRtn);
            #endregion
            // Draw    : Don't calculate, just draw
        }

        public void MeasureLetterHeights() {
            FontFamily fontFamily = new FontFamily(ConsolasFont.Name);
            int ascent = fontFamily.GetCellAscent(FontStyle.Regular);
            int descent = fontFamily.GetCellDescent(FontStyle.Regular);
            int emHeight = fontFamily.GetEmHeight(FontStyle.Regular);

            int ascentPixel = (int)ConsolasFont.Size * ascent / emHeight;
            int descentPixel = (int)ConsolasFont.Size * descent / emHeight;

            letterHeight = ascentPixel + descentPixel + 5;
            lineSpace = letterHeight;// (int)alphaFont.Size * lineSpacing / emHeight;

            fontFamily = new FontFamily(ExponentFont.Name);
            ascent = fontFamily.GetCellAscent(FontStyle.Regular);
            descent = fontFamily.GetCellDescent(FontStyle.Regular);
            emHeight = fontFamily.GetEmHeight(FontStyle.Regular);

            ascentPixel = (int)ExponentFont.Size * ascent / emHeight;
            descentPixel = (int)ExponentFont.Size * descent / emHeight;

            expLetterHeight = ascentPixel + descentPixel + 5;
            expLineSpace = expLetterHeight;// (int)alphaFont.Size * lineSpacing / emHeight;
        }

        #region -- measure stuff
        public MeasureArgsRtns MeasurePreColumn(qColumn ac, MeasureArgsRtns rtn) {
            if (rtn.exponent)
                return rtn;
            //------------------
            // bracket heights
            //------------------
            ac.outerMax = rtn.currentMax;
            rtn.currentMax=0;
            //------------------
            if (ac.colType==ColTyp.rooted) {
                rootDepth++; //TODO: Be better at this.
                ac.rootDepth=rootDepth;
                rtn.maxRootDepth=Math.Max(rtn.maxRootDepth, rootDepth);
                // rootDepth offsets top-down across all columns' rows
                // rootsInsideMe is (root) column-specific.
                // root tops are middle-up from the centres plus a gap * rootsInsideMe

                // -- new root column --------------
                StackIncrement(rtn.rootStack);
                rtn.rootStack.Push(new rootStacker());
                // ---------------------------------
            }
            return rtn;
        }

        public MeasureArgsRtns MeasureProcessRows(qColumn qc, MeasureArgsRtns rtn) {
            var ourHeight = 0;
            foreach (var row in qc.rows) { // nums, denoms
                if (rtn.exponent) {
                    //---------------------------------
                    // Am I recursing into an exponent now?
                    //---------------------------------
                    row.rowLen = GraphicsUtils.GetStringLength(row.nodeValue, g, ExponentFont, new Rectangle(0, 0, 200, 500), null, null);
                    row.rowRect = new Rectangle(0, 0, row.rowLen, expLetterHeight);
                    ourHeight += expLetterHeight * qc.rows.Count;
                //} else if (row.colType== ColTyp.bracket) {
                //    rtn = measureWalker.Traverse(row.columns, rtn, rtn.depth);

                } else {
                    rtn.currentMax=Math.Max(rtn.currentMax, qc.rows.Count); // bracket verticals
                    row.rowLen = GraphicsUtils.GetStringLength(row.nodeValue, g, ConsolasFont, new Rectangle(0, 0, 200, 500), null, null);
                    row.rowRect = new Rectangle(0, 0, row.rowLen, letterHeight);
                    //-------------------------------
                    //Does this row have an exponent?
                    //-------------------------------
                    if (row.columns.Count == 1 && row.columns[0].colType == ColTyp.exponent && row.columns[0].columns.Count > 0) {
                        var exponentGroup = row.columns[0]; //Grouping column, no drawn parts. It's never processed.
                        rtn.exponent = true;
                        rtn = measureWalker.Traverse(exponentGroup.columns, rtn, rtn.depth);
                        rtn.exponent = false;
                        ourHeight += expLetterHeight+2;
                    }
                    //}
                }
            }
            if (!rtn.exponent) {
                switch (qc.colType) {

                    case ColTyp.brace:
                        break;
                    case ColTyp.bracket:
                        ourHeight += letterHeight* qc.rows.Count;
                        break;
                    case ColTyp.squareBracket:
                        break;

                    case ColTyp.fraction: //ASSUMPTION ABOUT ROW HEIGHTS
                        ourHeight += letterHeight* qc.rows.Count;
                        break;
                    case ColTyp.integer:
                        ourHeight += letterHeight;
                        break;
                    case ColTyp.exponent:
                        break;

                    case ColTyp.rooted:
                        ourHeight += 8;
                        break;

                    case ColTyp.sigma:
                        break;
                    case ColTyp.product:
                        break;
                    case ColTyp.integral:
                        break;

                    case ColTyp.matrix:
                        break;

                    case ColTyp.factorial:
                        break;
                    case ColTyp.choose:
                        break;
                    case ColTyp.complex:
                        break;
                    case ColTyp.real:
                        ourHeight += letterHeight;
                        break;
                    default:
                        break;
                }
            }
            rtn.Height = Math.Max(rtn.Height, ourHeight);
            return rtn;
        }
        public MeasureArgsRtns MeasurePostColumn(qColumn ac, MeasureArgsRtns rtn) {
            if (rtn.exponent)
                return rtn;
            //------------------
            // bracket heights
            //------------------
            ac.maxRows = rtn.currentMax;
            rtn.currentMax =Math.Max(rtn.currentMax, ac.outerMax);
            //------------------
            if (ac.colType==ColTyp.rooted) {
                rootDepth--;

                // -- root column closing ----------
                ac.rootsInsideMe = rtn.rootStack.Pop().consecs; //Any zero's popped here
                StackDecrement(rtn.rootStack);
                // ---------------------------------
            }
            if (ac.colType==ColTyp.bracket && ac.rows.Count>0) {
                // Do I have an exponent?
                if (ac.rows.Count==1) {
                    rtn.exponent=true;
                    rtn = measureWalker.Traverse(ac.rows[0].columns, rtn, rtn.depth);
                    rtn.exponent=false;
                }
            }
            if (ac.colType==ColTyp.sigma) { }
            return rtn;
        }
        #endregion

        #region -- size stuff
        public SizeArgsRtns SizePreTraversalOp(List<qColumn> acs, SizeArgsRtns rtn) {
            if (acs[0].colType == ColTyp.brace
             || acs[0].colType == ColTyp.bracket
             || acs[0].colType == ColTyp.squareBracket
             || acs[0].colType == ColTyp.matrix) {
                rtn.Width=5;
            } else if (acs[0].colType == ColTyp.rooted) {
                //rtn.Width=5;
            }

            return rtn;
        }
        public SizeArgsRtns SizeProcessRows(qColumn qc, SizeArgsRtns rtn) {
            // X's and Y's (rowRects)
            float mid = (float)rtn.Height/2f; // PROVISIONAL
            float dynY = 0;
            Point offset = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);

            if (!rtn.exponent) {
                foreach (var ansRow in qc.rows) {
                    //if (ansRow.rowChunks[0].elementChunk.Trim() != ",") { // Standard spacing
                    ansRow.rowLen+=5;
                    //}
                    if (rtn.uniformSize) {
                        ansRow.rowLen= rtn.uniformedWidth+5;
                    }
                } //Widths only
            }
            int maxRowLen = qc.rows.Max(r => r.rowLen);

            if (rtn.exponent) {
                //---------------------------------
                // Am I processing an exponent now?
                //---------------------------------
                var maxRowWidth = 0; var myWidth = 0;
                // -- centering num/denom --
                maxRowWidth = qc.rows.Max(r => r.rowLen); //qc.rows.Max(r => r.rowRect.Width);
                var midX = maxRowWidth/2;
                // -------------------------
                for (int i = 0; i<qc.rows.Count; i++) {  // 1..n rows    (singles/fractionals)
                    var row = qc.rows[i];
                    dynY = (rtn.exponentPoint.Y - (((float)qc.rows.Count/2f)*(expLetterHeight-5)))+((expLetterHeight-5)*i); //Hug the division line closer
                    dynY += expLetterHeight/3;
                    myWidth = row.rowLen;                 //row.rowRect.Width;                    
                    row.rowRect = new Rectangle(offset.X + rtn.exponentPoint.X+midX-myWidth/2, offset.Y + (int)dynY,
                                                 row.rowRect.Width, row.rowRect.Height);
                }
                rtn.exponentPoint = new Rectangle(rtn.exponentPoint.X+maxRowWidth+1, rtn.exponentPoint.Y,
                                                  maxRowWidth+5, qc.rows.Sum(r => r.rowRect.Height));
            } else {
                //------------
                // Normal size
                //------------
                var exGWidth = 0; var exGStart = 0; var exGEnd = 0; var exponents = false;
                for (int i = 0; i<qc.rows.Count; i++) {  // 1..n rows    (singles/fractionals)
                    var row = qc.rows[i];
                    //if (row.colType==ColTyp.bracket) {
                    //    // Brackets around num/denom/row. Which means I'm a COLUMN around the row.
                    //    rtn = SizePreColumnOp(row, rtn);
                    //    rtn = SizeProcessRows(row, rtn);
                    //    rtn = SizePostColumnOp(row, rtn);
                    //    return rtn;
                    //}
                    dynY = (mid- (((float)qc.rows.Count/2f)*letterHeight))+(letterHeight*i);
                    row.rowRect = new Rectangle(offset.X + rtn.Width + maxRowLen/2-row.rowLen/2, offset.Y + (int)dynY,
                                                row.rowRect.Width+2, row.rowRect.Height);
                    //-------------------------------
                    //Does this row have an exponent?
                    //-------------------------------
                    if (row.columns.Count == 1 && row.columns[0].colType == ColTyp.exponent && row.columns[0].columns.Count > 0) {
                        exponents=true;
                        rtn.exponent = true;
                        var exponentGroup = row.columns[0]; //Grouping column, no drawn parts. Stores exponent group width.
                        rtn.exponentPoint = new Rectangle(row.rowRect.X+row.rowRect.Width, row.rowRect.Y,
                                                          row.rowRect.Width+2, row.rowRect.Height);
                        exGStart = rtn.exponentPoint.X;
                        rtn = layoutWalker.Traverse(exponentGroup.columns, rtn, rtn.depth);
                        exGEnd = rtn.exponentPoint.X;
                        exGWidth=exGEnd-exGStart;
                        row.rowExpLen = exGWidth;
                        rtn.exponent = false;
                    }
                }
                int colStart = Utils.min(qc.rows.Select(r => r.rowRect.X).ToArray())+2;
                int colEnd = Utils.max(qc.rows.Select(r => r.rowRect.Width).ToArray())-1 + colStart;
                var colBasesMidX = colStart+(colEnd-colStart)/2;
                //If there's multiple rows & one+ has an exponent, growth of overall width (rtn.Width) won't be straightforward (see draw Division line)
                var calculatedWidths = new List<int> { };
                qc.rows.ForEach(row => calculatedWidths.Add(colBasesMidX + row.rowRect.Width/2 + row.rowExpLen)); // allBasesMid + (halfMyBaseWidth + myExpWidth)
                colEnd = Utils.max(calculatedWidths.ToArray());
                rtn.Width += (colEnd-colStart)+10;
            }
            return rtn;
        }
        public SizeArgsRtns SizePreColumnOp(qColumn ac, SizeArgsRtns rtn) {
            switch (ac.colType) {

                case ColTyp.brace:
                    break;
                case ColTyp.bracket:
                    break;
                case ColTyp.squareBracket:
                    break;

                case ColTyp.fraction:
                    break;
                case ColTyp.integer:
                    break;

                case ColTyp.rooted:
                    break;

                case ColTyp.sigma:
                    break;
                case ColTyp.product:
                    break;
                case ColTyp.integral:
                    break;

                case ColTyp.matrix:
                    break;

                case ColTyp.factorial:
                    break;
                case ColTyp.choose:
                    break;
                case ColTyp.complex:
                    break;
                case ColTyp.real:
                    break;
                default:
                    break;
            }
            if (ac.colType==ColTyp.bracket || ac.colType==ColTyp.brace || ac.colType==ColTyp.squareBracket) {
                rtn.Width +=5;
                ac.rowRect=new Rectangle(rtn.Width, 0, rtn.Width+1, 1); //rowRect's horizontals used as X coord's for left & right brackets
            }
            if (ac.colType==ColTyp.rooted) {
                ac.rowRect=new Rectangle(rtn.Width, 0, 1, 1); //Using rects' left & right as X coord's for left & right brackets
                rtn.Width +=20;
            }
            if (ac.colType==ColTyp.sigma) {
                int len = GetStringLength(ac.from+"__", ConsolasFont);
                ac.rowRect=new Rectangle(rtn.Width, 0, len, 1);
                rtn.Width+=len;
            }
            return rtn;
        }
        public SizeArgsRtns SizePostColumnOp(qColumn ac, SizeArgsRtns rtn) {
            switch (ac.colType) {

                case ColTyp.brace:
                    break;
                case ColTyp.bracket:
                    break;
                case ColTyp.squareBracket:
                    break;

                case ColTyp.fraction:
                    break;
                case ColTyp.integer:
                    break;

                case ColTyp.rooted:
                    break;

                case ColTyp.sigma:
                    break;
                case ColTyp.product:
                    break;
                case ColTyp.integral:
                    break;

                case ColTyp.matrix:
                    break;

                case ColTyp.factorial:
                    break;
                case ColTyp.choose:
                    break;
                case ColTyp.complex:
                    break;
                case ColTyp.real:
                    break;
                default:
                    break;
            }
            float mid = (float)rtn.Height/2f;
            float dynY = (mid- ((float)ac.maxRows/2f*letterHeight));// +rtn.maxRootDepth*8;

            if (ac.colType==ColTyp.bracket || ac.colType==ColTyp.brace || ac.colType==ColTyp.squareBracket) {
                //For closing bracket:
                ac.rowRect=new Rectangle(ac.rowRect.X, (int)dynY, rtn.Width, ac.maxRows*letterHeight);
                //ROWRECT horizontals ARE FOR BETWEEN THE BRACKETS ONLY
                // Do I have an exponent?
                // .. Gently does it.. \\

                if (ac.rows.Count==1) {
                    var exGWidth = 0; var exGStart = 0; var exGEnd = 0;
                    rtn.exponent = true;
                    var exponentGroup = ac.rows; //Grouping column, no drawn parts. Stores exponent group width.
                    rtn.exponentPoint = new Rectangle(rtn.Width, (int)dynY,
                                                      rtn.Width, ac.maxRows*letterHeight);
                    //Bracket exponents are in rows not columns like normal exponents
                    exGStart = rtn.exponentPoint.X;
                    rtn = layoutWalker.Traverse(exponentGroup, rtn, rtn.depth);
                    exGEnd = rtn.exponentPoint.X;
                    exGWidth=exGEnd-exGStart;
                    ac.rowExpLen = exGWidth;
                    rtn.exponent=false;
                    rtn.Width+=ac.rowExpLen;
                }
                rtn.Width +=5;
            }
            if (ac.colType==ColTyp.rooted) {
                // Using rects' left & right as X coord's for left & right brackets
                // Root tops are middle-up from the centres plus a gap * rootsInsideMe
                // Like to treat root tops as rows
                ac.rowRect=new Rectangle(ac.rowRect.X,
                                         (int)(dynY)-((ac.rootsInsideMe+1)*8),
                                         rtn.Width,
                                         (((ac.maxRows*letterHeight)+rtn.maxRootDepth*8)+(ac.rootsInsideMe-2)*8));
            }
            if (ac.colType==ColTyp.sigma) {
                ac.rowRect=new Rectangle(ac.rowRect.X, (int)mid-25, rtn.Width, 3*letterHeight);
            }
            return rtn;
        }
        #endregion

        #region -- draw stuff
        public DrawArgsRtns DrawProcessRowsGraphicsUtils(qColumn qc, DrawArgsRtns rtn) {
            int midY = 0;
            Pen bracketPen = new Pen(Color.Black, 2);
            //if (qc.colType!=ColTyp.bracket) {
            if (rtn.exponent) {
                //---------------------------------
                // Am I processing an exponent now?
                //---------------------------------
                for (int i = 0; i<qc.rows.Count; i++) {  //Multi-rows
                    //AddTextDraw(qc.rows[i].wholeRow, ExponentFont, new Point(qc.rows[i].rowRect.X, qc.rows[i].rowRect.Y));

                    //new bits
                    AddTextDraw(qc.rows[i].nodeValue, ExponentFont, new Point(qc.rows[i].rowRect.X, qc.rows[i].rowRect.Y));
                }
            } else {
                //------------
                // Normal size
                //------------
                if ((qc.colType & (ColTyp.bracket))>0) {
                    var minX = qc.rows.Min(row => row.rowRect.X);
                    var minY = qc.rows.Min(row => row.rowRect.Y);
                    var bracketHeight = qc.rows.Sum(row => row.rowRect.Height);
                    var bracketWidth = qc.rows.Sum(row => row.rowRect.Width);
                    //rtn.ansBackBuffer.DrawLine(bracketPen,
                    //    new Point(minX-8, minY),
                    //    new Point(minX-8, minY + height));

                    //rtn.ansBackBuffer.DrawLine(bracketPen,
                    //    new Point(minX + rtn.Width+8, minY),
                    //    new Point(minX + rtn.Width+8, minY + height));
                    AddLineDraw(Pens.Black,
                        new Point(minX-8, minY),
                        new Point(minX-8, minY + bracketHeight));
                    AddLineDraw(Pens.Black,
                        new Point(minX + bracketWidth+8, minY),
                        new Point(minX + bracketWidth+8, minY + bracketHeight));
                }

                for (int i = 0; i<qc.rows.Count; i++) {  //Multi-rows
                                                         //var row = qc.rows[i];
                                                         //if (row.colType==ColTyp.bracket) {
                                                         //// Brackets around num/denom/row. Which means I'm a COLUMN around the row.
                                                         //    rtn = DrawPreColumnOp(row, rtn);
                                                         //    rtn = DrawProcessRows(row.rows[0], rtn);
                                                         //    rtn = DrawPostColumnOp(row, rtn);
                                                         //    return rtn;
                                                         //} else {

                    //}
                    //AddTextDraw(qc.rows[i].wholeRow, ConsolasFont, new Point(qc.rows[i].rowRect.X, qc.rows[i].rowRect.Y));
                    //if (qc.rows[i].colType==ColTyp.bracket) {
                    //    rtn = drawWalker.Traverse(qc.columns, rtn, rtn.depth);
                    //} else {
                        //new bit
                        AddTextDraw(qc.rows[i].nodeValue, ConsolasFont, new Point(qc.rows[i].rowRect.X, qc.rows[i].rowRect.Y));

                        //-------------------------------
                        //Does this row have an exponent?
                        //-------------------------------
                        if (qc.rows[i].columns.Count == 1 && qc.rows[i].columns[0].colType == ColTyp.exponent && qc.rows[i].columns[0].columns.Count > 0) {
                            var exponentGroup = qc.rows[i].columns[0]; //Grouping column, no drawn parts
                            rtn.exponent = true;
                            rtn = drawWalker.Traverse(qc.rows[i].columns[0].columns, rtn, rtn.depth);
                            rtn.exponent = false;
                        }
                    //}
                }
            }

            // -- division line --
            if (qc.rows.Count == 2 && qc.showDiv && (qc.colType & (ColTyp.fraction))>0) {
                int xStart = Utils.min(qc.rows.Select(r => r.rowRect.X).ToArray())+2;
                int xEnd = xStart + Utils.max(qc.rows.Select(r => r.rowLen).ToArray())-1;
                //int xEnd = xStart + Utils.max(qc.rows.Select(r => r.rowRect.Width).ToArray())-1;
                midY = (qc.rows[0].rowRect.Y+ qc.rows[1].rowRect.Y+qc.rows[1].rowRect.Height)/2;

                // We have exponents ----------------------------
                if ((qc.rows[0].columns.Count==1 && qc.rows[0].columns[0].colType==ColTyp.exponent) ||
                     (qc.rows[1].columns.Count==1 && qc.rows[1].columns[0].colType==ColTyp.exponent) ||
                      qc.colType == ColTyp.exponent) {
                    midY-=2;
                    var midX = xStart+(xEnd-xStart)/2;
                    var x0 = midX + qc.rows[0].rowRect.Width/2 + qc.rows[0].rowExpLen; // bothBasesMid + (halfMyBase plus exp)
                    var x1 = midX + qc.rows[1].rowRect.Width/2 + qc.rows[1].rowExpLen; // bothBasesMid + (halfMyBase plus exp)
                    xEnd = Math.Max(x0, x1); //Extends div line under exp
                }

                // We are an exponent ----------------------------
                //if (qc.rows.Any(r => r.colType==ColTyp.exponent)) {
                //xEnd = xStart + Utils.max(qc.rows.Select(r => r.rowRect.Width).ToArray())-1;
                //xEnd-=4;
                //midY-=2;
                //}
                AddLineDraw(Pens.Black, new Point(xStart, midY),
                                        new Point(xEnd, midY));
            }
            // } else {
            //     var a = "here";
            // }
            return rtn;
        }
        public DrawArgsRtns DrawPreColumnOp(qColumn ac, DrawArgsRtns rtn) {
            Pen p = new Pen(Color.Black, 2);
            int columnLeft = ac.rowRect.X;
            if ( (ac.colType & (ColTyp.bracket))>0) {
                #region brackets
                Point[] points = {
                    new Point( columnLeft,    ac.rowRect.Y),
                    new Point((columnLeft)-4, ac.rowRect.Y+   (ac.rowRect.Height/3)),
                    new Point((columnLeft)-4, ac.rowRect.Y+(2*(ac.rowRect.Height/3))),
                    new Point( columnLeft,    ac.rowRect.Y+    ac.rowRect.Height),
                };
                CurveDraws.Add(new CurveDraw(p, points, 2f));
                #endregion

            } else if (ac.colType == ColTyp.brace) {
                #region braces
                Point[] points = {
                    new Point(  columnLeft+2,       rtn.TopLeft.Y),
                    new Point( (columnLeft+2)-10,   rtn.TopLeft.Y + 5),
                    new Point( (columnLeft+2),     (rtn.TopLeft.Y + (rtn.Height/2))-5),
                    new Point( (columnLeft+2)-7,    rtn.TopLeft.Y + (rtn.Height/2)),
                    new Point( (columnLeft+2),     (rtn.TopLeft.Y + (rtn.Height/2))+5),
                    new Point( (columnLeft+2)-10,  (rtn.TopLeft.Y +  rtn.Height)- 5),
                    new Point(  columnLeft+2,      (rtn.TopLeft.Y +  rtn.Height))
                };
                BezierDraws.Add(new BezierDraw(p, points));
                rtn.TopLeft = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);
                #endregion

            } else if (ac.colType == ColTyp.squareBracket) {
                #region square brackets
                LineDraws.Add(new LineDraw(p, new Point(columnLeft, rtn.TopLeft.Y),
                                              new Point(columnLeft+4, rtn.TopLeft.Y)));

                LineDraws.Add(new LineDraw(p, new Point(columnLeft, rtn.TopLeft.Y),
                                              new Point(columnLeft, rtn.TopLeft.Y+rtn.Height)));

                LineDraws.Add(new LineDraw(p, new Point(columnLeft, rtn.TopLeft.Y+rtn.Height),
                                              new Point(columnLeft+4, rtn.TopLeft.Y+rtn.Height)));
                rtn.TopLeft = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);
                #endregion

            } else if (ac.colType == ColTyp.rooted) {
                #region rooted
                Point[] points = new Point[]{
                    new Point(columnLeft,    ac.rowRect.Y+ac.rowRect.Height-6 ),
                    new Point(columnLeft+2,  ac.rowRect.Y+ac.rowRect.Height-8 ),
                    new Point(columnLeft+6,  ac.rowRect.Y+ac.rowRect.Height ),
                    new Point(columnLeft+15, ac.rowRect.Y)
                };
                PathDraws.Add(new PathDraw(points, p));
                rtn.TopLeft = new Point(rtn.TopLeft.X, rtn.TopLeft.Y);
                #endregion
            }
            return rtn;
        }
        public DrawArgsRtns DrawPostColumnOp(qColumn ac, DrawArgsRtns rtn) {
            Pen p = new Pen(Color.Black, 2);
            int columnRight = ac.rowRect.Width-5;

            if (ac.colType == ColTyp.bracket) {
                #region brackets
                Point[] points = {
                    new Point( columnRight,    ac.rowRect.Y),
                    new Point((columnRight)+4, ac.rowRect.Y+   (ac.rowRect.Height/3)),
                    new Point((columnRight)+4, ac.rowRect.Y+(2*(ac.rowRect.Height/3))),
                    new Point( columnRight,    ac.rowRect.Y+    ac.rowRect.Height),
                };
                CurveDraws.Add(new CurveDraw(p, points, 2f));
                // Legacy ---------------------------------
                //if (ac.rows.Count==1) { // This bracket (its inner column) has an exponent
                //    AddTextDraw(ac.rows[0].wholeRow, ConsolasFont, new Point(columnRight+5,   rtn.TopLeft.Y));
                //}//----------------------------------------

                if (ac.rows.Count==1) { // Exponent on the bracket
                    rtn.exponent=true;
                    rtn = drawWalker.Traverse(ac.rows[0].columns, rtn, rtn.depth);
                    rtn.exponent=false;
                }
                #endregion

            } else if (ac.colType == ColTyp.brace) {
                #region braces
                Point[] points = {
                    new Point(  columnRight-2,      rtn.TopLeft.Y),
                    new Point( (columnRight-2)+10,  rtn.TopLeft.Y + 5),
                    new Point( (columnRight-2),    (rtn.TopLeft.Y + (rtn.Height/2))-5),
                    new Point( (columnRight-2)+7,   rtn.TopLeft.Y + (rtn.Height/2)),
                    new Point( (columnRight-2),    (rtn.TopLeft.Y + (rtn.Height/2))+5),
                    new Point( (columnRight-2)+10, (rtn.TopLeft.Y +  rtn.Height)- 5),
                    new Point(  columnRight-2,     (rtn.TopLeft.Y +  rtn.Height))
                };
                BezierDraws.Add(new BezierDraw(p, points));
                #endregion

            } else if (ac.colType == ColTyp.squareBracket) {
                #region square brackets
                LineDraws.Add(new LineDraw(p, new Point(columnRight, rtn.TopLeft.Y),
                                              new Point(columnRight-4, rtn.TopLeft.Y)));

                LineDraws.Add(new LineDraw(p, new Point(columnRight, rtn.TopLeft.Y),
                                              new Point(columnRight, rtn.TopLeft.Y+rtn.Height)));

                LineDraws.Add(new LineDraw(p, new Point(columnRight, rtn.TopLeft.Y+rtn.Height),
                                              new Point(columnRight-4, rtn.TopLeft.Y+rtn.Height)));
                #endregion

            }
            if (ac.colType == ColTyp.rooted) {
                #region rooted
                Point[] points = new Point[]{
                    new Point(ac.rowRect.X+15, ac.rowRect.Y), //15 is symbol width
                    new Point(columnRight,     ac.rowRect.Y),
                };
                PathDraws.Add(new PathDraw(points, p));
                #endregion

            }
            if (ac.colType == ColTyp.sigma) {
                #region sigma
                int row = rtn.TopLeft.Y;// letterHeight;
                //Bollocks. Rubbish.
                Point[] points = new Point[]{
                    new Point(ac.rowRect.Width/2+17,row+2),
                    new Point(ac.rowRect.Width/2+15,row),
                    new Point(ac.rowRect.Width/2-15,row),
                    new Point(ac.rowRect.Width/2,   row/2+ac.rowRect.Height/2-1),
                    new Point(ac.rowRect.Width/2-15,ac.rowRect.Height-1),
                    new Point(ac.rowRect.Width/2+15,ac.rowRect.Height-1),
                    new Point(ac.rowRect.Width/2+17,ac.rowRect.Height-3),
                };
                PathDraws.Add(new PathDraw(points, p));
                AddTextDraw($@"n={ac.from}", ConsolasFont,
                    new Point(2+ac.rowRect.Width/2-GetStringLength("n="+ac.from, ConsolasFont)/2, ac.rowRect.Height));
                if (ac.Infinity) {
                    AddTextDraw($@"∞", ConsolasFont, new Point(ac.rowRect.Width/2-3, 0));
                } else {
                    AddTextDraw($@"{ac.to}", ConsolasFont,
                        new Point(2+ac.rowRect.Width/2-GetStringLength(""+ac.to, ConsolasFont)/2, 0));
                }
                #endregion

            }

            if (ac.rowRect.Width > maxBracket)
                maxBracket=ac.rowRect.Width;

            return rtn;
        }
        #endregion

        public Bitmap Commit() {
            Bitmap rtn;

            // Columns are transient, drawing ops persist
            // Size the bitmap to contents
            int maxWidth = 0, maxHeight = 0, mptdWidth = 0;

            #region  -- Get width 
            //    TextDraws
            if (TextDraws.Count>0)
                maxWidth = TextDraws.Max(td => td.point.X+td.len);
            //    MultiPartTextDraws
            foreach (var mptd in MultiPartTextDraws) {
                mptdWidth= mptd.TextDraws[0].point.X+mptd.TextDraws[0].len;
                foreach (var td in mptd.TextDraws.Skip(1)) {
                    mptdWidth+=td.len;
                }
                maxWidth = Math.Max(maxWidth, mptdWidth);
            }
            //    LineDraws
            if (LineDraws.Count>0)
                maxWidth = Math.Max(maxWidth, LineDraws.Max(ld => Math.Max(ld.start.X, ld.finish.X)));
            //    ImageDraws
            #endregion

            maxWidth = (int)Math.Max(maxWidth, maxBracket+5);
            width =maxWidth+5;

            #region -- Get height 
            //    TextDraws
            if (TextDraws.Count>0)
                maxHeight = TextDraws.Max(td => td.point.Y+letterHeight);
            //    MultiPartTextDraws
            foreach (var mptd in MultiPartTextDraws) {
                maxHeight = Math.Max(maxHeight, mptd.TextDraws.Max(td => td.point.Y+letterHeight));
            }
            //    LineDraws
            if (LineDraws.Count>0)
                maxHeight = Math.Max(maxHeight, LineDraws.Max(ld => Math.Max(ld.start.Y, ld.finish.Y)));
            //    ImageDraws
            #endregion

            height = maxHeight+5;

            rtn= new Bitmap(width, height);

            #region -- Draw everything 
            int len = 0, cumLen = 0;
            Graphics gr;
            using (gr= Graphics.FromImage(rtn)) {
                gr.Clear(backColor);
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                //Draw rooteds
                foreach (var rt in PathDraws) {
                    var gp = new GraphicsPath();
                    for (int i = 1; i<rt.points.Count(); i++) {
                        gp.AddLine(rt.points[i-1], rt.points[i]);
                    }
                    gr.DrawPath(rt.pen, gp);
                }

                // Draw closed curves
                foreach (var cv in ClosedCurveDraws)
                    gr.DrawClosedCurve(cv.pen, cv.points);

                // Draw curves
                foreach (var cv in CurveDraws)
                    gr.DrawCurve(cv.pen, cv.points);

                // Draw Beziers
                foreach (var bz in BezierDraws)
                    gr.DrawBeziers(bz.pen, bz.points);

                // Draw lines
                foreach (var ld in LineDraws)
                    gr.DrawLine(ld.pen, ld.start, ld.finish);

                // Draw Text lines
                foreach (var td in TextDraws) {
                    gr.DrawString(td.txt, td.font, td.brush, new Rectangle(td.point.X, td.point.Y, td.len+10, letterHeight));
                }
                // Draw Multi-part Text lines
                foreach (var mptd in MultiPartTextDraws) {
                    len=0;
                    foreach (var td in mptd.TextDraws) {
                        //letterHeight =  ((int)td.font.Size+5);
                        gr.DrawString(td.txt, td.font, td.brush, new Rectangle(td.point.X+cumLen, td.point.Y, td.len+10, letterHeight));
                        cumLen+=td.len;
                    }
                }
                // Draw Images
            }
            #endregion

            Bitmap marginedRtn = new Bitmap(width+4, height+4);
            using (gr=Graphics.FromImage(marginedRtn)) {
                gr.Clear(backColor);
                gr.DrawImage(rtn, new PointF(2, 2));
            }
            return marginedRtn;
        }
        public BitmapBuilder Reset() {
            PathDraws.Clear();
            TextDraws.Clear();
            MultiPartTextDraws.Clear();
            LineDraws.Clear();
            CurveDraws.Clear();
            ClosedCurveDraws.Clear();
            BezierDraws.Clear();
            return this;
        }
    }

    public class PathDraw {
        public PathDraw(Point[] ppoints, Pen ppen) { //lines only
            points=ppoints;
            pen=ppen;
        }
        public Point[] points { get; set; }
        public Pen pen { get; set; }

    }
    public class BezierDraw {
        public BezierDraw(Pen ppen, Point[] ppoints) {
            pen=ppen;
            points=ppoints;
        }
        public Pen pen { get; set; }
        public Point[] points { get; set; }
    }
    public class CurveDraw {
        public CurveDraw(Pen ppen, Point[] ppoints, float ttension) {
            pen=ppen;
            points=ppoints;
            tension=ttension;
        }
        public Pen pen { get; set; }
        public Point[] points { get; set; }
        public float tension { get; set; }
    }
    public class ClosedCurveDraw {
        public ClosedCurveDraw(Pen ppen, Point[] ppoints) {
            points=ppoints;
            pen=ppen;
        }
        public Point[] points { get; set; }
        public Pen pen { get; set; }
    }
    public class LineDraw {
        public LineDraw(Pen ppen, Point pstart, Point pfinish) {
            pen=ppen;
            start=pstart;
            finish=pfinish;
        }
        public int len { get; set; }
        public int width { get; set; }
        public Pen pen { get; set; }
        public Point start { get; set; }
        public Point finish { get; set; }

    }
    public class TextDraw {
        public TextDraw(string ptxt, Point ppoint, Font pfont, Brush pbrush, int plen) {
            txt =ptxt;
            font =pfont;
            brush =pbrush;
            point = ppoint;
            len=plen;
        }
        public int len { get; set; }
        public int width { get; set; }
        public string txt { get; set; }
        public Font font { get; set; }
        public Brush brush { get; set; }
        public Point point { get; set; }
    }
    public class ImageDraw {
        public int len { get; set; }
        public int width { get; set; }

    }
    public class MultiPartTextDraw {
        public MultiPartTextDraw() { }
        public MultiPartTextDraw(List<TextDraw> ptextdraws) {
            TextDraws=ptextdraws;
        }
        public List<TextDraw> TextDraws { get; set; } = new List<TextDraw>();
        public void Add(TextDraw ptextDraw) => TextDraws.Add(ptextDraw);
    }

    public static class GraphicsUtils {
        //TODO: This is a string formatting function.
        /// <summary>
        /// Ordinal postfix applier.
        /// </summary>
        /// <param name="n">The number to modify.</param>
        /// <returns>Returns a string ordinal representation.</returns>
        public static string PostFix(int n) => n==1 ? "st" : n==2 ? "nd" : n==3 ? "rd" : "th";

        //TODO: This is a number/string formatting function.
        /// <summary>
        /// Converts a number string to superscript.
        /// </summary>
        /// <param name="str">String representation of a number.</param>
        /// <returns>The input number string converted to superscript.</returns>
        public static string ToSuper(string str) {//  ³√27    x²
            StringBuilder sb = new StringBuilder();
            foreach (char s in str) {
                switch (s) {
                    case '1': sb.Append("¹"); break;
                    case '2': sb.Append("²"); break;
                    case '3': sb.Append("³"); break;
                    case '4': sb.Append("⁴"); break;
                    case '5': sb.Append("⁵"); break;
                    case '6': sb.Append("⁶"); break;
                    case '7': sb.Append("⁷"); break;
                    case '8': sb.Append("⁸"); break;
                    case '9': sb.Append("⁹"); break;
                    case '0': sb.Append("⁰"); break;
                    case '-': sb.Append("⁻"); break;
                    case '+': sb.Append("⁺"); break;
                    case '*': sb.Append("*"); break;
                    case '/': sb.Append("ᐟ"); break;
                    case '\\': sb.Append("ᐠ"); break;
                    case 'a': sb.Append("ᵃ"); break;
                    case 'b': sb.Append("ᵇ"); break;
                    case 'c': sb.Append("ᶜ"); break;
                    case 'd': sb.Append("ᵈ"); break;
                    case 'e': sb.Append("ᵉ"); break;
                    case 'f': sb.Append("ᶠ"); break;
                    case 'g': sb.Append("ᵍ"); break;
                    case 'h': sb.Append("ʰ"); break;
                    case 'i': sb.Append("ⁱ"); break;
                    case 'j': sb.Append("ʲ"); break;
                    case 'k': sb.Append("ᵏ"); break;
                    case 'l': sb.Append("ˡ"); break;
                    case 'm': sb.Append("ᵐ"); break;
                    case 'n': sb.Append("ⁿ"); break;
                    case 'o': sb.Append("ᵒ"); break;
                    case 'p': sb.Append("ᵖ"); break;
                    case 'r': sb.Append("ʳ"); break;
                    case 's': sb.Append("ˢ"); break;
                    case 't': sb.Append("ᵗ"); break;
                    case 'u': sb.Append("ᵘ"); break;
                    case 'v': sb.Append("ᵛ"); break;
                    case 'w': sb.Append("ʷ"); break;
                    case 'x': sb.Append("ˣ"); break;
                    case 'y': sb.Append("ʸ"); break;
                    case 'z': sb.Append("ᶻ"); break;
                    case 'A': sb.Append("ᴬ"); break;
                    case 'B': sb.Append("ᴮ"); break;
                    case 'D': sb.Append("ᴰ"); break;
                    case 'E': sb.Append("ᴱ"); break;
                    case 'G': sb.Append("ᴳ"); break;
                    case 'H': sb.Append("ᴴ"); break;
                    case 'I': sb.Append("ᴵ"); break;
                    case 'J': sb.Append("ᴶ"); break;
                    case 'K': sb.Append("ᴷ"); break;
                    case 'L': sb.Append("ᴸ"); break;
                    case 'M': sb.Append("ᴹ"); break;
                    case 'N': sb.Append("ᴺ"); break;
                    case 'O': sb.Append("ᴼ"); break;
                    case 'P': sb.Append("ᴾ"); break;
                    case 'R': sb.Append("ᴿ"); break;
                    case 'T': sb.Append("ᵀ"); break;
                    case 'U': sb.Append("ᵁ"); break;
                    case 'V': sb.Append("ⱽ"); break;
                    case 'W': sb.Append("ᵂ"); break;
                    case '=': sb.Append("₌"); break;
                    case '(': sb.Append("⁽"); break;
                    case ')': sb.Append("⁾"); break;
                    default: sb.Append(s); break;
                }
            }
            return sb.ToString();
        }

        //TODO: This is a number/string formatting function.
        /// <summary>
        /// Converts a number to subscript.
        /// </summary>
        /// <param name="str">String representation of a number.</param>
        /// <returns>The input number string converted to subscript.</returns>
        public static string ToSub(string str) {
            StringBuilder sb = new StringBuilder();
            foreach (char s in str) {
                switch (s) {
                    case '1': sb.Append("₁"); break;
                    case '2': sb.Append("₂"); break;
                    case '3': sb.Append("₃"); break;
                    case '4': sb.Append("₄"); break;
                    case '5': sb.Append("₅"); break;
                    case '6': sb.Append("₆"); break;
                    case '7': sb.Append("₇"); break;
                    case '8': sb.Append("₈"); break;
                    case '9': sb.Append("₉"); break;
                    case '0': sb.Append("₀"); break;
                    case '+': sb.Append("₊"); break;
                    case '-': sb.Append("₋"); break;
                    case '=': sb.Append("₌"); break;
                    case '(': sb.Append("₍"); break;
                    case ')': sb.Append("₎"); break;
                    case 'a': sb.Append("ₐ"); break;
                    case 'e': sb.Append("ₑ"); break;
                    case 'h': sb.Append("ₕ"); break;
                    case 'i': sb.Append("ᵢ"); break;
                    case 'j': sb.Append("ⱼ"); break;
                    case 'k': sb.Append("ₖ"); break;
                    case 'l': sb.Append("ₗ"); break;
                    case 'm': sb.Append("ₘ"); break;
                    case 'n': sb.Append("ₙ"); break;
                    case 'o': sb.Append("ₒ"); break;
                    case 'p': sb.Append("ₚ"); break;
                    case 'r': sb.Append("ᵣ"); break;
                    case 's': sb.Append("ₛ"); break;
                    case 't': sb.Append("ₜ"); break;
                    case 'u': sb.Append("ᵤ"); break;
                    case 'v': sb.Append("ᵥ"); break;
                    case 'x': sb.Append("ₓ"); break;
                    default: sb.Append(s); break;
                }
            }
            return sb.ToString();
        }

        //TODO: Dependency on font system.
        //            Abstract out. Say what not how.
        /// <summary>
        /// Convert a regular number into superscripted standard form.
        /// </summary>
        /// <param name="originalNum">A number, normally formatted.</param>
        /// <returns>A string representation of a superscripted Standard Form number.</returns>
        public static string ToStandardForm(decimal originalNum) {
            StringBuilder sb = new StringBuilder();
            int ops = 0;
            bool sign = originalNum>0m;
            decimal num = Math.Abs(originalNum);
            if (num<1m) {
                while (num<1m) {
                    num*=10m;
                    ops++;
                }
                sb.Append(((double)num*(sign ? 1 : -1))).Append("*10").Append(GraphicsUtils.ToSuper($"-{ops}"));
            } else {
                while (num>10m) {
                    num/=10m;
                    ops++;
                }
                sb.Append((num*(sign ? 1 : -1))).Append("*10").Append(GraphicsUtils.ToSuper($"{ops}"));
            }
            return sb.ToString();
        }

        //TODO: These are Winforms/System.Drawing helper functions.        
        public static StringFormat GetStringFormat(string str) {
            StringFormat strFmt = new StringFormat();
            strFmt.SetMeasurableCharacterRanges(new CharacterRange[] { new CharacterRange(0, str.Length) });
            return strFmt;
        }
        public static StringFormat GetStringFormat(CharacterRange[] crArray) {
            StringFormat strFmt = new StringFormat();
            strFmt.SetMeasurableCharacterRanges(crArray);
            return strFmt;
        }
        public static Region[] GetRegionArray(string str, Font font, Rectangle rect, StringFormat strFmt, Graphics g) {
            if (strFmt is null)
                strFmt = GetStringFormat(str);
            return g.MeasureCharacterRanges(str, font, rect, strFmt);
        }
        public static int GetStringLength(string str, Graphics g, Font font, Rectangle rect, StringFormat strFmt, Region[] region) {
            if (region is null) // Get whole string
                region = GetRegionArray(str, font, rect, strFmt, g);
            return (int)region.Select(r => r.GetBounds(g).Width).Sum();
        }
        public static Region[] GetRegionArray(string str, Font font, Rectangle rect, CharacterRange[] crArray, Graphics g) {
            StringFormat strFmt = GetStringFormat(crArray);
            return g.MeasureCharacterRanges(str, font, rect, strFmt);
        }

    }
}
