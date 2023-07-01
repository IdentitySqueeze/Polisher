using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MathUtils;

namespace Polish {
    public class qTriangleTest:Question{
        public void Δ(string s)=>MessageBox.Show(s);
        public qTriangleTest(int id, qParameters qParams):base(id,qParams){ }
        public Point PointFromQuads(Rectangle r, ref List<int> quads, int margin){
            // 'Draw' without replacement
            //         |
            //   00    |   10
            //         |
            //-------------------
            //         |
            //   01    |   11
            //         |
            int quad = Utils.Choose(quads);
            quads.RemoveAll(q=>q==quad);
            int x = Utils.R(margin, r.Width/2-margin) + (((quad-1)&1)==1?r.Width/2:0);
            int y = Utils.R(margin, r.Height/2-margin)+ (((quad-1)>>1)==1?r.Height/2:0);
            return new Point(x,y);
        }
        public override void GenerateQuestion(){
            var qb= new QuestionBuilder( qParams, queFont );
            askBitmap = new Bitmap(200,200);
            Graphics g = Graphics.FromImage(askBitmap);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);
            Pen p=new Pen(Color.Orange);

            int margin=15;
            List<int> quads = Enumerable.Range(1,4).ToList();
            Rectangle r = new Rectangle(0,0,askBitmap.Width,askBitmap.Height);

            Triangle t = new Triangle();
            t.boundary = r;
            //t.A.Point=PointFromQuads(r, ref quads, margin);
            //t.B.Point=PointFromQuads(r, ref quads, margin);
            //t.C.Point=PointFromQuads(r, ref quads, margin);

            t.C.Point=new Point(20,50);
            t.A.Point=new Point(70,90);
            t.B.Point=new Point(130,20);

            p.Width = 2;
            g.DrawLine(p, t.A.Point, t.B.Point);
            g.DrawLine(p, t.A.Point, t.C.Point);
            g.DrawLine(p, t.B.Point, t.C.Point);

            // -- Finger gyp --
            int Ax, Ay, Bx, By, Cx, Cy;
            Ax=t.A.Point.X;Ay=t.A.Point.Y;
            Bx=t.B.Point.X;By=t.B.Point.Y;
            Cx=t.C.Point.X;Cy=t.C.Point.Y; 
            // -- --
            Point madeUpA, madeUpB, madeUpC;
            bool aIsCorner=false, bIsCorner=false, cIsCorner=false, doubleCornered=false;
            int cornerCount=0;
            madeUpA = new Point( Math.Max(Bx,Cx)-Math.Min(Bx,Cx), Math.Max(By,Cy)-Math.Min(By,Cy) );
            madeUpB = new Point( Math.Max(Ax,Cx)-Math.Min(Ax,Cx), Math.Max(Ay,Cy)-Math.Min(Ay,Cy) );
            madeUpC = new Point( Math.Max(Ax,Bx)-Math.Min(Ax,Bx), Math.Max(Ay,By)-Math.Min(Ay,By) );

            int maxX = Utils.max(new int[]{Ax,Bx,Cx});
            int maxY = Utils.max(new int[]{Ay,By,Cy});
            int minX = Utils.min(new int[]{Ax,Bx,Cx});
            int minY = Utils.min(new int[]{Ay,By,Cy});
            if( (Ax==maxX && Ay==maxY) || (Ax==minX && Ay==minY) || (Ax==maxX && Ay==minY) || (Ax==minX && Ay==maxY)){
                aIsCorner=true;
                cornerCount++;
            }
            if( (Bx==maxX && By==maxY) || (Bx==minX && By==minY) || (Bx==maxX && By==minY) || (Bx==minX && By==maxY) ){
                bIsCorner=true;
                cornerCount++;
            }
            if( (Cx==maxX && Cy==maxY) || (Cx==minX && Cy==minY) || (Cx==maxX && Cy==minY) || (Cx==minX && Cy==maxY) ){
                cIsCorner=true;
                cornerCount++;
            }
            if(cornerCount>1) doubleCornered=true;

            double adj=0,hyp=0,opp=0;
            double muB_A_C=0, muC_A_B=0, muC_B_A=0, muB_C_A=0, muA_C_B=0, muA_B_C=0;
 
            //  maxY && not cornered : adj=x,x
            //  minY && not cornered : adj=x,x
            //  maxX && not cornered : adj=y,y
            //  minX && not cornered : adj=y,y
            //  cornered             : adj=x,y
            //  
            //  

            // -- muB_A_C --
            adj= Cx-Ax; //adj=Math.Max(Cx,Ax)-Math.Min(Cx,Ax);
            opp= Ay-Cy; //opp=Math.Max(Ay,Cy)-Math.Min(Ay,Cy);
            hyp= Math.Sqrt( Math.Pow(adj,2) + Math.Pow(opp,2) );
            muB_A_C=90-  180d*Math.Acos(adj/hyp)/Math.PI;
        //if(muB_A_C>90)muB_A_C=90-muB_A_C;

            // -- muB_C_A --
            muB_C_A = 180-(90+muB_A_C);

            // -- muC_A_B --
            adj= By-Ay; //adj=Math.Max(By,Ay)-Math.Min(By,Ay);
            opp= Bx-Ax; //opp=Math.Max(Bx,Ax)-Math.Min(Bx,Ax);
            hyp= Math.Sqrt( Math.Pow(adj,2) + Math.Pow(opp,2) );
            muC_A_B= 180d*Math.Acos(adj/hyp)/Math.PI;

            // -- muC_B_A --
            muC_B_A=  180-(90+muC_A_B);

            // -- muA_B_C --
            adj= Cx-Bx; //adj=Math.Max(Cx,Bx)-Math.Min(Cx,Bx);
            opp= By-Cy; //opp=Math.Max(By,Cy)-Math.Min(By,Cy);
            hyp= Math.Sqrt( Math.Pow(adj,2) + Math.Pow(opp,2) );
            muA_B_C= 180d*Math.Acos(adj/hyp)/Math.PI;
        //if(muA_B_C>90)muA_B_C=90-muA_B_C;

            // -- muA_C_B --
            //muA_C_B = 180-(90+muA_B_C);



            // -- ABC --
            t.A.Degrees = (decimal)((aIsCorner?90:180)-(muB_A_C+muC_A_B));
            t.B.Degrees = (decimal)((bIsCorner?90:180)-(muC_B_A+muA_B_C));
            t.C.Degrees = 180-(t.A.Degrees+t.B.Degrees);//(decimal)((cIsCorner?90:180)-(muA_C_B+muB_C_A));
            if(doubleCornered){
                t.A.Degrees+=aIsCorner?0:90;
                t.B.Degrees+=bIsCorner?0:90;
                t.C.Degrees+=cIsCorner?0:90;
            }


           // -- Calculate lengths -- 
            double lena, lenb, lenc = 0;
            lena = Math.Sqrt( Math.Pow( Math.Max(t.C.Point.X,t.B.Point.X)-Math.Min(t.C.Point.X,t.B.Point.X),2) +
                              Math.Pow( Math.Max(t.C.Point.Y,t.B.Point.Y)-Math.Min(t.C.Point.Y,t.B.Point.Y),2));
            lenb = Math.Sqrt( Math.Pow( Math.Max(t.C.Point.X,t.A.Point.X)-Math.Min(t.C.Point.X,t.A.Point.X),2) +
                              Math.Pow( Math.Max(t.C.Point.Y,t.A.Point.Y)-Math.Min(t.C.Point.Y,t.A.Point.Y),2));
            lenc = Math.Sqrt( Math.Pow( Math.Max(t.A.Point.X,t.B.Point.X)-Math.Min(t.A.Point.X,t.B.Point.X),2) +
                              Math.Pow( Math.Max(t.A.Point.Y,t.B.Point.Y)-Math.Min(t.A.Point.Y,t.B.Point.Y),2));
            lena=Math.Round(lena,2);
            lenb=Math.Round(lenb,2);
            lenc=Math.Round(lenc,2);
            
            // -- Calculate angles --
            double angleA=0, angleB=0, angleC = 0;
           
            // -- Set Label Coords --
            Point A = new Point(t.A.Point.X, t.A.Point.Y-25);
            Point B = new Point(t.B.Point.X-20,  t.B.Point.Y-10);
            Point C = new Point(t.C.Point.X-5,  t.C.Point.Y-10);

            Point a = new Point(t.B.Point.X+2, t.C.Point.Y + ((t.B.Point.Y-t.C.Point.Y)/3)+2);
            Point b = new Point(t.A.Point.X+((t.C.Point.X-t.A.Point.X)/2)-20,
                                t.C.Point.Y+((t.A.Point.Y-t.C.Point.Y)/2)-20);
            Point c = new Point(t.A.Point.X+((t.B.Point.X-t.A.Point.X)/2), t.A.Point.Y);

            Font font = new Font("arial", 8);

//g.DrawString($@"{aIsCorner},{bIsCorner},{cIsCorner},{doubleCornered}", new Font("arial", 6), 
//new SolidBrush(Color.Black), new Rectangle(A.X-50,A.Y,200,20));
            //g.DrawString(""+Math.Round(t.A.Degrees),
//g.DrawString($@"adj: {Math.Round(adj)}, opp: {Math.Round(opp)}, hyp: {Math.Round(hyp)}",

g.DrawString($@"muB_A_C: {Math.Round(muB_A_C)}{"\n"}muC_B_A: {Math.Round(muC_B_A)}{"\n"}muB_C_A: {Math.Round(muB_C_A)}{"\n"}" +
             $@"muC_A_B: {Math.Round(muC_A_B)}{"\n"}muA_C_B: {Math.Round(muA_C_B)}{"\n"}muA_B_C: {Math.Round(muA_B_C)}",
  font, new SolidBrush(Color.Green), new Rectangle(A.X,C.Y+70,200,200));
            g.DrawString(""+Math.Round(t.A.Degrees), font, new SolidBrush(Color.Blue), new Rectangle(A.X,A.Y,180,100));
            g.DrawString(""+Math.Round(t.B.Degrees), font, new SolidBrush(Color.Black), new Rectangle(B.X,B.Y,180,100));
            g.DrawString(""+Math.Round(t.C.Degrees), font, new SolidBrush(Color.Black), new Rectangle(C.X,C.Y,180,100));
            //g.DrawString(""+lena, font, new SolidBrush(Color.Black), new Rectangle(a.X,a.Y,90,20));
            //g.DrawString(""+lenb, font, new SolidBrush(Color.Black), new Rectangle(b.X,b.Y,90,20));
            //g.DrawString(""+lenc, font, new SolidBrush(Color.Black), new Rectangle(c.X,c.Y,90,20));

            // -- draw angle arcs --
            //g.DrawArc(p, t.A.Point.X, t.A.Point.Y-40, 40, 40, 315, 90);
            //g.DrawArc(p, t.C.Point.X-40, t.C.Point.Y, 40, 40, 90, 90);
            // -- Draw right angle --
            //p.Width=1;
            //g.DrawRectangle(p, t.B.Point.X-13, t.B.Point.Y-15,13,15);

            //picture.Save(@"/home/havoc/Desktop/picture");
            topText="";
            //botText.Add(new qChunk(id,1,"Whatever",false));
            //qb.ChunkRowToColumnInteger( this );
        }
    }
}
