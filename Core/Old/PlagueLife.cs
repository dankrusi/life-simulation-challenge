using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	public class PlagueLife : RandomLife
	{
		public PlagueLife (World world, Vector pos) : base(world,pos)
		{
			
			
		}
		
		public override void Simulate() {
			
			base.Simulate();
			
			Lifelet life = Closest();
			if(life != null && life.Distance(this) < 50) {
				//life._health *= 0.5;
			}
		}
	}
}

