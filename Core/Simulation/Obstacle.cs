/* 
 * The Life Simulation Challenge
 * https://github.com/dankrusi/life-simulation-challenge
 * 
 * Contributor(s):
 *   Dan Krusi <dan.krusi@nerves.ch> (original author)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do 
 * so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	/// <summary>
	/// Base class containing standard world obstacle properties.
	/// </summary>
	public class Obstacle
	{
		
		
		
		#region Private / Protected Variables
		
		private long _uid = -1;
		private static long _gid = 0;
		protected World _world;
		
		private Vector _pos;
		
		private long _age = 0;
		protected int _maxAge = 0;
		
		#endregion
		
		
		
		
		#region Properties
		
		public long UID {
			get { return _uid; }
		}
		
		public Vector Position {
			get { return _pos; }
		}
		
		public double X {
			get { return _pos.X; }
		}
		
		public double Y {
			get { return _pos.Y; }
		}
		
		public long Age {
			get { return _age; }
		}
		
		#endregion
		
		
		
		#region Public Methods
		
		public Obstacle (World world, Vector pos)
		{
			// Init
			_world = world;
			_uid = _gid++;
			_pos = pos;
		}
		
		public virtual void Simulate() {
			// Age
			_age++;
		}
		
		public virtual void Draw(Graphics g) {
			// Nothing to do here...	
		}
		
		public double Distance(Lifelet other) {
			return this.Position.Distance(other.Position);
		}
		
		public double Distance(Vector point) {
			return this.Position.Distance(point);
		}
		
		#endregion
		
		
		
	}
}

