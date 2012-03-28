using System;
using System.Drawing;

namespace LifeSimulation.Core
{
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

