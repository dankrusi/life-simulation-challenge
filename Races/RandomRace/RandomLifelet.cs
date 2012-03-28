using System;
using System.Drawing;
using LifeSimulation.Core;

namespace LifeSimulation.Races.RandomRace
{
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

