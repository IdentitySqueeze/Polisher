using System;
using System.Drawing;
using Boundary = System.Drawing.Rectangle;
using Side = Polish.Line;

namespace Polish {
    public class Triangle{ 
        public Triangle()        {        }
        public Triangle(decimal a, decimal b, decimal c){
            A.Degrees=a;B.Degrees=b;C.Degrees=c;
        }
        public Angle A {get;set; } = new Angle("A");
        public Angle B {get;set; } = new Angle("B");
        public Angle C {get;set; } = new Angle("C");
        public Side a {get;set; } = new Side("a");
        public Side b {get;set; } = new Side("b");
        public Side c {get;set; } = new Side("c");
        // -- draw --
        public Boundary boundary{get;set;}
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    public class Angle{ // -- Multi-use --
        public Angle(string l){ Label=new Label(l);}
        public Angle(string l, decimal d){ Label=new Label(l);Degrees=d;}
        public decimal Degrees{get; set; }
        public bool Show{ get; set; }
        public bool Acute{ get;set; }=true;
        public bool Arrows{ get;set; }
        public Label Label{ get;set; }
        public int EquateLines { get; set;}=0;
        // -- draw --
        public Side[] Sides{get;set; } // * 2
        public Point Point { get;set;}
    }
    public class Line{ // -- Multi-use --
        public Line(decimal l){Length=l;}
        public Line(string s){LineLabel=s; }
        public string LineLabel{ get; set; }
        public bool StartLabel {get; set; }
        public bool EndLabel {get; set; }
        public decimal Length{ get; set; } // not for drawing
        public int EquateLines { get; set;}=0;
        // -- draw --
        public Point Start{get;set; }
        public Point End{get;set; }
        public decimal LengthDraw{ get;set; } // implied by Points
        public bool LineArrow{ get;set; }
        public bool StartArrows{ get;set; }
        public bool EndArrows{ get;set; }
        public LineStyle style { get; set; } 
    }
    public class Label{ // -- Multi-use --
        public Label(string l){ Text=l;}
        public string Text{ get;set; }
        // -- draw --
        public bool Orient { get;set; }
    }
    [Flags]
    public enum LineStyle{
        none=1,
        solid=2,
        dashed=4,
        bold=8
    }
    public class Circle{
        public Circle(){}
        public decimal Radious{get;set; }
        public decimal Circumference{get; set; }
        // -- draw --
        public Point Centre{get;set; }
        public Boundary boundary{get;set;}
        public LineStyle Style{get;set; }
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    public class Arc{ // of a circle or floating & unclosed
        public Arc(){ }
        public Point[] Points{get;set; } 
        public Label ArcLabel{get;set; }
        public Label AreaLabel{get;set;}
        // -- draw --
        public Point Centre{get;set; }
        public Boundary boundary{get;set;}
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    public class Ellipse{
        public Ellipse(){}
        // -- draw --
        public Point Centre{get;set; }
        public Boundary boundary{get;set;}
        public LineStyle Style{get;set; }
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    public class Square{
        public Square(){ }
        public Angle A {get;set; } = new Angle("A", 90);
        public Angle B {get;set; } = new Angle("B", 90);
        public Angle C {get;set; } = new Angle("C", 90);
        public Angle D {get;set; } = new Angle("D", 90);
        public Side a {get;set; } = new Side("a");
        public Side b {get;set; } = new Side("b");
        public Side c {get;set; } = new Side("c");
        public Side d {get;set; } = new Side("c");
        // -- draw --
        public Boundary boundary{get;set;}
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    public class Rectangular{ 
        public Rectangular(){ }
        public Angle A {get;set; } = new Angle("A", 90);
        public Angle B {get;set; } = new Angle("B", 90);
        public Angle C {get;set; } = new Angle("C", 90);
        public Angle D {get;set; } = new Angle("D", 90);
        public Side a {get;set; } = new Side("a");
        public Side b {get;set; } = new Side("b");
        public Side c {get;set; } = new Side("c");
        public Side d {get;set; } = new Side("c");
        // -- draw --
        public Boundary boundary{get;set;}
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    //Rhombus, Trapezium
    public class Quadrilateral{
        public Quadrilateral(){ }
        public Angle A {get;set; } = new Angle("A");
        public Angle B {get;set; } = new Angle("B");
        public Angle C {get;set; } = new Angle("C");
        public Angle D {get;set; } = new Angle("D");
        public Side a {get;set; } = new Side("a");
        public Side b {get;set; } = new Side("b");
        public Side c {get;set; } = new Side("c");
        public Side d {get;set; } = new Side("c");
        // -- draw --
        public Boundary boundary{get;set;}
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    public class IntersectingCircles{ //Needs to be Shapes
        public IntersectingCircles(){ }
        public Circle Circle1{ get;set;}
        public Circle Circle2{ get;set; }
        public Label Label{ get; set; }
        public Label StartLabel{ get; set; }
        public Label EndLabel{ get; set; }

        // -- draw --
        public bool Shaded{get;set; }
        public bool Unshaded{ get;set; }
    }
    
}
