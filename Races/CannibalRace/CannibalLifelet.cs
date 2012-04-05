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

namespace LifeSimulation.Races.CannibalRace
{
	/// <summary>
	/// The Cannibal Lifelet sacrifices the entire race to a chosen leader, creating a super energized single lifelet.
	/// This class is meant as an example.
	/// </summary>
	public class CannibalLifelet : Lifelet
	{
		
		
		
		#region Private Variables
		
		private Lifelet _leader; 		// Cannibal leader
		private double _speed; 			// Our speed we want
		
		#endregion
		
		
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		public override string Race {
			get { return "Cannibal"; }
		}
		
		public override string Author {
			get { return "Dan Krusi"; }
		}
		
		/// <summary>
		/// TODO
		/// </summary>
		public bool IsCannibalLeader {
			get { return _leader == null; }
		}
		
		#endregion
		
		
		
		
		
		#region Public Methods
		
		public CannibalLifelet(World world, Vector pos) : base(world,pos)
		{
			// Init
			_speed = 1.0;
			
			// Initial message to find each other
			talk('!');
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Recieving messages?
			foreach(Message message in audibleMessages()) { 
				if(message.Contents == '!') {
					if(message.Sender.UID < this.UID && (_leader == null || message.Sender.UID < _leader.UID)) {
						_leader = message.Sender;
					}
				} 
			}
			
			// Cannibal leader?
			if(_leader == null) {
				
				// Eat
				bool foundSomethingToEat = false;
				foreach(Lifelet lifelet in visibleLifelets()) {
					attack(lifelet, 10);
					this.moveToDestination(lifelet.Position,_speed*3);
					foundSomethingToEat = true;
					break;
				}
				
				// Move randomly
				if(foundSomethingToEat == false) this.moveRandomly(_speed*3);
				
			} else {
				
				// Move in the sacrifice location
				this.moveToDestination(_leader.Position,_speed);
			}
			
			
		}
		
		#endregion
		
		
		
		
		
		
		
		
	}
}

