
using System;
using LifeSimulation.Core;

namespace LifeSimulation.Races.SquadronRace
{


	public class SquadronLifelet : Lifelet
	{
		
		#region Private Variables
		
		private Vector _direction; 					// The current direction of our lifelet
		private Vector _randDirection;				// Just a helpful random Vector
		private Vector _toMapCenter;
		private Vector _mapCenter;
		private double _speed; 						// Our speed we want
		
		
		private long _leaderUID = -1; 				// Squadron leader
		private ShelledLifelet leader;
		
		#endregion
		
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		public override string Race {
			get { return "Test";}
		}
		
		public override string Author {
			get { return "stephinity";}
		}
		
		#endregion
		
		
		#region Public Methods


		public SquadronLifelet (World world, Vector pos) : base(world,pos)
		{
			// Init
			_speed = 2.0;
			_randDirection = new Vector(this.RandomGen.Next(-1,2),this.RandomGen.Next(-1,2));
			_toMapCenter = new Vector(0 - this.X, 0 - this.Y);
			_mapCenter = new Vector(0.0, 0.0);
			
			//_direction = _toMapCenter;
			
			// Initial message to find each other
			talk('!');
			
		}
		
		public Vector formCircle( Double radius, Vector center) {
			double phi = this.RandomGen.Next(0,2) * 2*Math.PI;
			return new Vector(radius * Math.Cos(phi) - center.X, radius * Math.Cos(phi) - center.Y);
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Find the squadron leader
			foreach(Message message in audibleMessages()) { 
				if(message.Contents == '!') {
					if(message.Sender.UID < this.UID && (_leaderUID == -1 || message.Sender.UID > _leaderUID)) {
						// Follow this lifelet
						_leaderUID = message.Sender.UID;
					}
				} 
			}
			
			// The squadron leader
			ShelledLifelet leader = this.getLifeletByUID(_leaderUID);
			
			// Guard the squadron leader
			if(leader != null) {
				if (this.UID != _leaderUID) {
					this.move(formCircle(3,leader.Position));
				} else {
					//this.move(_toMapCenter);
					this.moveToDestination(_mapCenter, 1.0);
				}
			}
			
			
			
			// See other race Attack!
			
		}
		
		#endregion
	}
}
