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
using System.Collections;
using System.Collections.Generic;


namespace LifeSimulation.Core
{
	/// <summary>
	/// The Lifelet class represents a single lifeform in the simulation. 
	/// </summary>
	public class ShelledLifelet
	{
	
		#region Private Variables - these can't be changed by contestents directly
		
		private Lifelet _lifelet = null;
		private World _world = null;
		private long _validWorldAge = -1;
				
		#endregion
		
		

		
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		public bool IsValid {
			get {
				return _world.Age == _validWorldAge;
			}
		}
		
		public string Race {
			get { 
				checkIfValid();
				return _lifelet.Race; 
			}
		}
		
		public string Type {
			get { 
				checkIfValid();
				return _lifelet.Type; 
			}
		}
		
		public long UID {
			get { 
				checkIfValid();
				return _lifelet.UID; 
			}
		}
		
		public Vector Position {
			get { 
				checkIfValid();
				return _lifelet.Position; 
			}
		}
		
		public double X {
			get { 
				checkIfValid();
				return _lifelet.X; 
			}
		}
		
		public double Y {
			get { 
				checkIfValid();
				return _lifelet.Y; 
			}
		}
		
		public Vector Velocity {
			get { 
				checkIfValid();
				return _lifelet.Velocity; 
			}
		}
		
		public double Energy {
			get { 
				checkIfValid();
				return _lifelet.Energy; 
			}
		}
		
		public double Health {
			get { 
				checkIfValid();
				return _lifelet.Health; 
			}
		}
		
		public double Visibility {
			get { 
				checkIfValid();
				return _lifelet.Visibility; 
			}
		}
		
		public System.Drawing.Color Color {
			get { 
				checkIfValid();
				return _lifelet.Color; 
			}
		}
		
		#endregion
		
		
		
		
		#region Public Methods
		
		public ShelledLifelet (Lifelet lifelet, World world)
		{
			// Init
			_lifelet = lifelet;
			_world = world;
			_validWorldAge = _world.Age;
		}
		
		public double Distance(Lifelet other) {
			checkIfValid();
			return _lifelet.Distance(other);
		}
		
		public double Distance(ShelledLifelet other) {
			checkIfValid();
			return _lifelet.Distance(other);
		}
		
		public double Distance(Vector point) {
			checkIfValid();
			return _lifelet.Distance(point);
		}
	
		
		#endregion
		
		
		
		
		
		
		
		#region Private Methods
		
		private void checkIfValid() {
			if(!this.IsValid) throw new Exception("The shelled lifelet handle is no longer valid. Shelled lifelets may only be accessed for a single simulation step.");
		}
		
		#endregion
	}
}

