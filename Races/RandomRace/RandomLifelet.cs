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
using LifeSimulation.Core;

namespace LifeSimulation.Races.RandomRace
{
	/// <summary>
	/// The Random Lifelet just randomly walks around the world and randomly performs various actions.
	/// This class is meant as an example.
	/// </summary>
	public class RandomLifelet : Lifelet
	{
		
		#region Private Variables
		
		private Vector _direction; 
		private double _speed; 
		
		#endregion
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		public override string Race {
			get { return "Random"; }
		}
		
		public override string Author {
			get { return "Dan Krusi"; }
		}
		
		#endregion
		
		
		
		
		
		#region Public Methods
		
		public RandomLifelet(World world, Vector pos) : base(world,pos)
		{
			// Init
			_direction = new Vector(this.RandomGen.Next(-1,2),this.RandomGen.Next(-1,2));
			_speed = 2.0;
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Change direction randomly
			if(this.RandomGen.Next(200) == 0) {
				_direction = new Vector(this.RandomGen.Next(-1,2),this.RandomGen.Next(-1,2));
			}
			
			// Change speed randomly
			if(this.RandomGen.Next(150) == 0) {
				_speed = this.RandomGen.Next(0,3);
			}
			
			// Random chatter...
			if(this.RandomGen.Next(250) == 0) {
				this.talk(Config.MessageLanguage[this.RandomGen.Next(Config.MessageLanguage.Length)]);
			}
			
			// Move
			this.move(_direction);
			
			
		}
		
		#endregion
	}
}

