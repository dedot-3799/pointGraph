using System;

namespace PointGraph.Math
{
    public struct Fraction : IEquatable<Fraction>, IComparable<Fraction>
    {
        public long Numerator { get; }
        public long Denominator { get; }

        public Fraction(long numerator, long denominator = 1)
        {
            if (denominator == 0)
            {
                throw new DivideByZeroException("Denominator cannot be zero.");
            }

            if (denominator < 0)
            {
                numerator = -numerator;
                denominator = -denominator;
            }

            long gcd = Gcd(numerator, denominator);
            Numerator = numerator / gcd;
            Denominator = denominator / gcd;
        }

        public static Fraction Zero => new Fraction(0, 1);
        public static Fraction One => new Fraction(1, 1);

        public bool IsInteger => Denominator == 1;

        // 有限小数（分母の素因数が2と5のみ）であるかどうかを判定する
        public bool CanBeFiniteDecimal()
        {
            if (Denominator == 0) return false;
            
            long tempDenom = Denominator;
            while (tempDenom % 2 == 0)
            {
                tempDenom /= 2;
            }
            while (tempDenom % 5 == 0)
            {
                tempDenom /= 5;
            }
            return tempDenom == 1;
        }

        public double ToDouble() => (double)Numerator / Denominator;

        // 浮動小数点数（double）から分数への変換（連分数法を使用し高い精度で変換）
        public static Fraction FromDouble(double value, double tolerance = 1e-9, int maxIterations = 20)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                throw new ArgumentException("Value must be a valid finite number.", nameof(value));
            }

            // 0に極めて近い場合は0として扱う
            if (System.Math.Abs(value) < 1e-12)
            {
                return Zero;
            }

            // 整数値であれば直接返す
            if (System.Math.Abs(value - System.Math.Round(value)) < 1e-11)
            {
                return new Fraction((long)System.Math.Round(value));
            }

            bool isNegative = value < 0;
            double val = System.Math.Abs(value);

            double h1 = 1, h2 = 0;
            double k1 = 0, k2 = 1;
            double b = val;

            int iterations = 0;
            do
            {
                double a = System.Math.Floor(b);
                double nextH = a * h1 + h2;
                double nextK = a * k1 + k2;

                // 巨大な分母になりオーバーフローするのを防ぐため、上限を設定する
                if (nextK > 10000000)
                {
                    break;
                }

                h2 = h1;
                h1 = nextH;

                k2 = k1;
                k1 = nextK;
                
                if (System.Math.Abs(b - a) < 1e-12)
                    break;

                b = 1.0 / (b - a);
                iterations++;
            } while (System.Math.Abs(val - h1 / k1) > val * tolerance && iterations < maxIterations);

            long num = (long)h1;
            long denom = (long)k1;

            if (denom == 0) denom = 1;

            return new Fraction(isNegative ? -num : num, denom);
        }

        private static long Gcd(long a, long b)
        {
            a = System.Math.Abs(a);
            b = System.Math.Abs(b);
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a == 0 ? 1 : a;
        }

        // 演算子オーバーロード
        public static Fraction operator +(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);
        }

        public static Fraction operator -(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);
        }

        public static Fraction operator *(Fraction a, Fraction b)
        {
            return new Fraction(a.Numerator * b.Numerator, a.Denominator * b.Denominator);
        }

        public static Fraction operator /(Fraction a, Fraction b)
        {
            if (b.Numerator == 0)
            {
                throw new DivideByZeroException("Cannot divide by a fraction with a numerator of zero.");
            }
            return new Fraction(a.Numerator * b.Denominator, a.Denominator * b.Numerator);
        }

        public static Fraction operator -(Fraction a)
        {
            return new Fraction(-a.Numerator, a.Denominator);
        }

        // 比較演算子・等価判定
        public bool Equals(Fraction other)
        {
            return Numerator == other.Numerator && Denominator == other.Denominator;
        }

        public override bool Equals(object? obj)
        {
            return obj is Fraction other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Numerator, Denominator);
        }

        public int CompareTo(Fraction other)
        {
            return (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);
        }

        public static bool operator ==(Fraction left, Fraction right) => left.Equals(right);
        public static bool operator !=(Fraction left, Fraction right) => !left.Equals(right);

        public override string ToString()
        {
            if (Denominator == 1)
            {
                return Numerator.ToString();
            }
            return $"{Numerator}/{Denominator}";
        }

        // MathML の文字リストとして綺麗に出力する
        public string ToMathML(bool includeSign = false)
        {
            long num = Numerator;
            bool isNeg = num < 0;
            if (isNeg) num = -num;

            string signHtml = isNeg ? "<mo>―</mo>" : (includeSign ? "<mo>+</mo>" : "");

            if (Denominator == 1)
            {
                return $"{signHtml}<mn>{num}</mn>";
            }

            if (CanBeFiniteDecimal())
            {
                // 有限小数の場合はそのまま小数表示にする
                double doubleVal = (double)Numerator / Denominator;
                double absVal = System.Math.Abs(doubleVal);
                // 小数第3位までに丸める
                string formatted = absVal.ToString("0.###");
                return $"{signHtml}<mn>{formatted}</mn>";
            }
            else
            {
                // 有限小数で表せない場合は分数タグで囲う
                return $"{signHtml}<mfrac><mn>{num}</mn><mn>{Denominator}</mn></mfrac>";
            }
        }
    }
}
