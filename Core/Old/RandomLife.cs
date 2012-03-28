using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	public class RandomLife : Lifelet
	{
		
	
		
		public RandomLife (World world, Vector pos) : base(world,pos)
		{
			
		}
		
		public override void Simulate() {
			
			base.Simulate();
			
			this.move(new Vector(this.RandomGen.Next(-1,2),this.RandomGen.Next(-1,2)));
		}
		
	}
}

