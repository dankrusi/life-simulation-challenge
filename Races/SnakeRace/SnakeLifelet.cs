using System;
using System.Drawing;
using LifeSimulation.Core;

namespace LifeSimulation.Races.SnakeRace
{
	public class SnakeLifelet : Lifelet
	{
		
		
		
		#region Private Variables
		
		private Vector _direction; 			// The current direction of our lifelet
		private double _speed; 				// Our speed we want
		private Lifelet _lifeletToFollow;	// The lifelet to follow, forming a snake. If we have none, then we are the leader!
		
		#endregion
		
		
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		public override string Race {
			get { return "Snake"; }
		}
		
		public override string Author {
			get { return "Dan Krusi"; }
		}
		
		public bool IsFollowing {
			get { return _lifeletToFollow != null; }
		}
		
		#endregion
		
		
		
		
		
		#region Public Methods
		
		public SnakeLifelet(World world, Vector pos) : base(world,pos)
		{
			// Init
			_direction = new Vector(this.RandomGen.Next(-1,2),this.RandomGen.Next(-1,2));
			_speed = 2.0;
			
			// Initial message to find each other
			talk('!');
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Init
			Message weHeardTalkAboutFood = null;
			
			// Recieving messages?
			foreach(Message message in audibleMessages()) { 
				if(message.Contents == '!') {
					if(message.Sender.UID < this.UID && (_lifeletToFollow == null || message.Sender.UID > _lifeletToFollow.UID)) {
						// Follow this lifelet
						_lifeletToFollow = message.Sender;
					}
				} else if(message.Contents == 'F') {
					if(_lifeletToFollow == null) {
						// Register this message
						weHeardTalkAboutFood = message;
					} else {
						// Pass along the message
						//talk('F');
					}
					
				}
			}
			
			// Are we following the chain?
			if(_lifeletToFollow != null) {
				
				// Set direction
				_direction = _lifeletToFollow.Position - this.Position;
				
				// Do we need to catch up?
				if(_lifeletToFollow.Distance(this) > this.Visibility-4)	_speed = 2.0;
				else 													_speed = 0.0;
				
				// Do we see food?
				foreach(Food food in visibleFood()) {
					talk('F');
					break;
				}

			} else {
			
				// Do we see food?
				bool foodFound = false;
				foreach(Food food in visibleFood()) {
					_direction = food.Position - this.Position;
					foodFound = true;
					break;
				}
				
				// Change direction randomly
				if(!foodFound) {
					
					// Did we hear some talk about food?
					if(weHeardTalkAboutFood != null) {
						// Go there
						_direction = weHeardTalkAboutFood.Position - this.Position;
					} else {
						// Randomly change direction
						if(this.RandomGen.Next(80) == 0) {
							_direction = new Vector(this.RandomGen.Next(-1,2),this.RandomGen.Next(-1,2));
							if(_direction.X == 0 && _direction.Y == 0) _direction = new Vector(1,1);
						}
					}
				}
				
			}
			
			// Redistribute energy
			if(this.Energy > 100) {
				foreach(Lifelet lifelet in visibleLifelets()) {
					// Only pass down the chain and only to our race
					if(lifelet != _lifeletToFollow && lifelet.Type == this.Type) {
						giveEnergy(lifelet,this.Energy-100);
						break;
					}
				}
			}
			
			// Move in the current direction
			this.move(_direction.Normalize() * _speed);
		}
		
		#endregion
		
		
		
		
		
		
		#region Protected Methods
		
		protected override void debugDraw(Graphics g) {
			// Draw follow chain
			if(_lifeletToFollow != null) {
				g.DrawLine(Pens.White,(int)this.X,(int)this.Y,(int)_lifeletToFollow.X,(int)_lifeletToFollow.Y);	
			}
		}
		
		#endregion
		
		
	}
}

