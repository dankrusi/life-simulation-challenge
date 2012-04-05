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
	/// Piece of food containing energy.
	/// </summary>
	public class Food : Obstacle
	{
		
		
		#region Private Variables
		
		private double _energy;
		
		#endregion
		
		
		
		
		#region Properties
		
		public double Energy {
			get { return _energy; }
		}
		
		#endregion
		
		
		
		#region Public Methods
		
		public Food (World world, Vector pos, double energy) : base(world,pos)
		{
			// Init
			_energy = energy;
			_maxAge = Math2.JitteredValue(Config.FoodMaxAge,Config.FoodMaxAgeJitter);
			
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Die of old age...
			if(this.Age > _maxAge) _world.Food.Remove(this);
			
		}
		
		public override void Draw(Graphics g) {
			base.Draw(g);
			
			g.FillRectangle(Brushes.Yellow,(int)this.X-1,(int)this.Y-1,2,2);
			
		}
		
		#endregion
		
	}
}

