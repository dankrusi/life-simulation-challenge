using System;
using System.Drawing;
using LifeSimulation.Core;

namespace LifeSimulation.Races.CannibalRace
{
	
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

