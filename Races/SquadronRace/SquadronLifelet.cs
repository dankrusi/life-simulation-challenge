
using System;
using LifeSimulation.Core;

namespace LifeSimulation.Races.SquadronRace
{


	public class SquadronLifelet : Lifelet
	{
		
		#region Private Variables
		
		private double _speed; 						// Our speed we want
		private double _formationPosition;
		
		
		private long _leaderUID = -1; 				// Squadron leader
		
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
			_formationPosition = this.RandomGen.Next(0,2) * 2*Math.PI; // angle
			
			// Initial message to find each other
			talk('!');
			
		}
		
		// Formation Circle around leader
		public Vector formCircle(Double radius, Vector center) {
			double phi = _formationPosition;
			return new Vector(radius * Math.Cos(phi) - center.X, radius * Math.Cos(phi) - center.Y);
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Find the squadron leader
			foreach(Message message in audibleMessages()) { 
				if(message.Contents == '!') {
					if(message.Sender.UID < this.UID && (_leaderUID == -1 || message.Sender.UID < _leaderUID)) {
						_leaderUID = message.Sender.UID;
					}
				} 
			}
			
			// The squadron leader
			ShelledLifelet leader = this.getLifeletByUID(_leaderUID);
			
			// Guard the squadron leader
			if(leader != null) {
				this.moveToDestination(formCircle(10,leader.Position),_speed); // this doesnt seem to be working...
				this.moveToDestination(leader.Position,_speed); // just to see the leader selection working, this works
			}
			
			
			
			// See other race Attack!
			
		}
		
		#endregion
	}
}
