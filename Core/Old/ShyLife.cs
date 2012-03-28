using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	public class ShyLife : SocialiteLife
	{
		public ShyLife (World world, Vector pos) : base(world,pos)
		{
			
			
			attractivity *= 2.0;
		}
		
		public override void Simulate() {
			
			base.Simulate();
			
			this.move(-Velocity);
		}
	}
}

