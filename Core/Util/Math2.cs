using System;

namespace LifeSimulation.Core
{
	public class Math2
	{
		private static Random _randomGen = new Random((int)DateTime.Now.Ticks);
		
		public static double DegreeToRadian(double angle) {
			return Math.PI * angle / 180.0;
		}
		
		public static double RadianToDegree(double angle) {
			return angle * (180.0 / Math.PI);
		}
		
		public static int JitteredValue(int val, int jitter) {
			return val + _randomGen.Next(-jitter,jitter);
		}

	}
}

