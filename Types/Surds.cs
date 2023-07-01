namespace Polish {
    public class Surd {

        #region// -- Fields -- //
        public int prefix { get; set; } = 1;
        public int rooted { get; set; }
        public char sign { get; set; } = '+';
        public bool IsInt { get; set; } = false;
        #endregion

        #region// -- Constructors -- //
        public Surd() { }
        public Surd(int pRooted) => rooted = pRooted;
        public Surd(int pRooted, bool pSign) {
            rooted = pRooted;
            if (pSign) sign='-';
        }
        public Surd(int pPrefix, int pRooted) {
            prefix = pPrefix;
            rooted = pRooted;
        }
        public Surd(int pPrefix, int pRooted, bool pSign) {
            prefix = pPrefix;
            rooted = pRooted;
            if (pSign) sign='-';
        }

        #endregion

        #region// -- Utilities -- //
        #endregion

        #region// -- Output -- //
        public override string ToString() => $@"{(sign=='-' ? ""+sign : "")}{(prefix!=1 ? ""+prefix : "")}{(IsInt ? "" : "√")}{rooted}";
        #endregion

        #region// -- Operators -- //
        public static Surd operator +(Surd a, Surd b) => Add(a, b);
        public static Surd operator +(Surd a, int b) => Add(a, b);
        public static Surd operator +(int a, Surd b) => Add(a, b);
        public static Surd operator +(Surd a, double b) => Add(a, b);
        public static Surd operator +(double a, Surd b) => Add(a, b);
        public static Surd operator +(Surd a, decimal b) => Add(a, b);
        public static Surd operator +(decimal a, Surd b) => Add(a, b);

        public static Surd operator -(Surd a, Surd b) => Subtract(a, b);
        public static Surd operator /(Surd a, Surd b) => Divide(a, b);

        public static Surd operator *(Surd a, Surd b) => Multiply(a, b);
        public static Surd operator *(Surd a, int b) => Multiply(a, b);
        public static Surd operator *(int a, Surd b) => Multiply(a, b);
        public static Surd operator *(Surd a, double b) => Multiply(a, b);
        public static Surd operator *(double a, Surd b) => Multiply(a, b);
        public static Surd operator *(Surd a, decimal b) => Multiply(a, b);
        public static Surd operator *(decimal a, Surd b) => Multiply(a, b);


        public static Surd Add(Surd a, Surd b) {
            Surd rtn = new Surd();
            if (a.IsInt==b.IsInt==false && a.rooted==b.rooted) {
                rtn.prefix=a.prefix+b.prefix;
                rtn.rooted=a.rooted;
            } else if (a.IsInt && b.IsInt) {
                rtn.rooted=a.rooted+b.rooted;
            } else {
                // return a + b
            }
            return rtn;
        }
        public static Surd Add(int a, Surd b) { return new Surd(); }
        public static Surd Add(Surd a, int b) { return new Surd(); }
        public static Surd Add(double a, Surd b) { return new Surd(); }
        public static Surd Add(Surd a, double b) { return new Surd(); }
        public static Surd Add(decimal a, Surd b) { return new Surd(); }
        public static Surd Add(Surd a, decimal b) { return new Surd(); }


        public static Surd Subtract(Surd a, Surd b) { return new Surd(); }
        public static Surd Divide(Surd a, Surd b) { return new Surd(); }

        public static Surd Multiply(Surd a, Surd b) {
            Surd rtn = new Surd();
            if (a.IsInt==b.IsInt // √3 x √3     3√5 x 2√5
               && a.rooted==b.rooted
               //&& a.prefix==b.prefix
               ) {
                rtn.IsInt = true;
                //rtn.prefix = a.prefix * b.prefix;
                rtn.rooted = a.rooted * a.prefix * b.prefix;
            } else {
                rtn.rooted = a.rooted * b.rooted;
            }
            return rtn;
        }
        public static Surd Multiply(int a, Surd b) {
            Surd rtn = new Surd();
            rtn.prefix = a*b.prefix;
            rtn.rooted=b.rooted;
            return rtn;
        }
        public static Surd Multiply(Surd a, int b) {
            Surd rtn = new Surd();
            rtn.prefix = b*a.prefix;
            rtn.rooted=a.rooted;
            return rtn;
        }
        public static Surd Multiply(double a, Surd b) { return new Surd(); }
        public static Surd Multiply(Surd a, double b) { return new Surd(); }
        public static Surd Multiply(decimal a, Surd b) { return new Surd(); }
        public static Surd Multiply(Surd a, decimal b) { return new Surd(); }
        #endregion
    }
    public class SurdUtils {
        public Surd Simplify(Surd a) {
            var rtn = new Surd();

            return a;
            return rtn;
        }
    }
}
