using System;

namespace Mandelbrot
{
    public struct ComplexDouble
    {
        public ComplexDouble(double real, double imaginary)
        {
            Real = real;
            Imaginary = imaginary;
        }

        public double Real 
        {
            get; set;
        }

        public double Imaginary
        {
            get; set;
        }

        public double Magnitude() 
        {
            return Math.Sqrt(Math.Pow(Real,2)+Math.Pow(Imaginary,2));
        }

        public override string ToString()
        {
            return $"{Real}+{Imaginary}i";
        }

        public static ComplexDouble operator+(ComplexDouble a, ComplexDouble b)
        {
            return new ComplexDouble(a.Real+b.Real,a.Imaginary+b.Imaginary);
        }

        public static ComplexDouble operator-(ComplexDouble a, ComplexDouble b)
        {
            return new ComplexDouble(a.Real-b.Real,a.Imaginary-b.Imaginary);
        }

        public static ComplexDouble operator*(ComplexDouble a, ComplexDouble b)
        {
            return new ComplexDouble((a.Real*b.Real-a.Imaginary*b.Imaginary),(a.Real*b.Imaginary+a.Imaginary*b.Real));
        }

        public static bool operator<(ComplexDouble a, Double b)
        {
            return Math.Pow(a.Real,2)+Math.Pow(a.Imaginary,2) < Math.Pow(b,2);
        }

        public static bool operator>(ComplexDouble a, Double b)
        {
            return Math.Pow(a.Real,2)+Math.Pow(a.Imaginary,2) > Math.Pow(b,2);
        }

        public static bool operator<=(ComplexDouble a, Double b)
        {
            return Math.Pow(a.Real,2)+Math.Pow(a.Imaginary,2) <= Math.Pow(b,2);
        }

        public static bool operator>=(ComplexDouble a, Double b)
        {
            return Math.Pow(a.Real,2)+Math.Pow(a.Imaginary,2) >= Math.Pow(b,2);
        }

        public override bool Equals(object obj)
        {
            var cd = obj as ComplexDouble?;
            if (this.Real == cd?.Real && this.Imaginary == cd?.Imaginary)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Real.GetHashCode() + Imaginary.GetHashCode();
        }

        public static ComplexDouble Zero
        {
            get => new ComplexDouble(0,0);
        }
    }
}