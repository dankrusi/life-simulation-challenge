using System;
using System.Drawing;

namespace LifeSimulation.Core
{

	public struct Vector
	{
		
		#region Private Variables
		
		private double _x;
		private double _y;
		
		#endregion
		
		
		
		#region Public Static Variables
		
		public static readonly Vector Empty = new Vector();
		
		#endregion
		
		
		
		#region Properties
		
		public double X
		{
			get { return _x; }
			set { _x = value; }
		}

		public double Y
		{
			get { return _y; }
			set { _y = value; }
		}
		
		#endregion
		
		
		
		#region Public Methods
		
		public Vector(double x, double y)
		{
			this._x = x;
			this._y = y;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is Vector)
			{
				Vector v = (Vector)obj;
				if (v._x == _x && v._y == _y)
					return obj.GetType().Equals(this.GetType());
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode ();
		}

		public override string ToString()
		{
			return String.Format("{{X={0}, Y={1}}}", _x, _y);
		}

		public double Dot(Vector other)
		{
			return _x*other.X + _y*other.Y;
		}

		public double Norm()
		{
			return Math.Sqrt(this.Dot(this));
		}
		
		public double Distance(Vector other)
		{
			return Vector.Distance(this,other);
		}

		public Vector Normalize()
		{
			double s = this.Norm();
    		return s == 0 ? this : this / s;
		}

		public double AngleBetween(Vector other)
		{
			return Math.Atan2(this.X - other.X, this.Y - other.Y);
		}
		
		#endregion
		
		
		
		
		#region Static Methods
		
		public static double Distance(Vector v1, Vector v2)
		{
			return Math.Sqrt( (v2.X - v1.X)*(v2.X - v1.X) + (v2.Y - v1.Y)*(v2.Y - v1.Y) );
		}
		
		public static Vector FromAngle(double length, double radians)
		{
			return new Vector( 	Math.Cos(radians) * length,
								Math.Sin(radians) * length  );
		}
		
		#endregion
		
		
		
		
		#region Operators

		public static bool operator==(Vector u, Vector v)
		{
			if (u.X == v._x && u._y == v._y)
				return true;
			else
				return false;
		}

		public static bool operator !=(Vector u, Vector v)
		{
			return u != v;
		}

		public static Vector operator+(Vector u, Vector v)
		{
			return new Vector(u._x + v._x, u._y + v._y);
		}

		public static Vector operator-(Vector u, Vector v)
		{
			return new Vector(u._x - v._x, u._y - v._y);
		}

		public static Vector operator*(Vector u, double a)
		{
			return new Vector(a*u._x, a*u._y);
		}

		public static Vector operator/(Vector u, double a)
		{
			return new Vector(u._x/a, u._y/a);
		}

		public static Vector operator-(Vector u)
		{
			return new Vector(-u._x, -u._y);
		}

		public static explicit operator PointF(Vector u)
		{
			return new PointF((float)u._x, (float)u._y);
		}

		public static implicit operator Vector(PointF p)
		{
			return new Vector(p.X, p.Y);
		}
		
		#endregion
	}

}

