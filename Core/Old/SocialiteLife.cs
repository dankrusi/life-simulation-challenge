using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	public class SocialiteLife : Lifelet
	{
		public double attractivity;
		
		public SocialiteLife(World world, Vector pos) : base(world,pos)
		{
			
			
			attractivity = 0.3;
		}
		
		public override void Simulate ()
		{
			base.Simulate();
			
			// Assume no motion
			this.move(new Vector(0,0));
			
			Lifelet life = Closest();
			if(life != null && life.Distance(this) < 100) {
				// Someone is close
				double dx = this.X - life.X;
				double dy = this.Y - life.Y;
				this.move(new Vector(Math.Sign(-dx)*attractivity, Math.Sign(-dy)*attractivity));
			}
		}
		
		
	}
}

