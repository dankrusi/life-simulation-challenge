using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	public class Obstacle
	{
		
		
		
		#region Private / Protected Variables
		
		private long _uid = -1;
		private static long _gid = 0;
		protected World _world;
		
		private Vector _pos;
		
		private long _age = 0;
		protected int _maxAge = 0;
		
		#endregion
		
		
		
		
		#region Properties
		
		public long UID {
			get { return _uid; }
		}
		
		public Vector Position {
			get { return _pos; }
		}
		
		public double X {
			get { return _pos.X; }
		}
		
		public double Y {
			get { return _pos.Y; }
		}
		
		public long Age {
			get { return _age; }
		}
		
		#endregion
		
		
		
		#region Public Methods
		
		public Obstacle (World world, Vector pos)
		{
			// Init
			_world = world;
			_uid = _gid++;
			_pos = pos;
		}
		
		public virtual void Simulate() {
			// Age
			_age++;
		}
		
		public virtual void Draw(Graphics g) {
			// Nothing to do here...	
		}
		
		public double Distance(Lifelet other) {
			return this.Position.Distance(other.Position);
		}
		
		public double Distance(Vector point) {
			return this.Position.Distance(point);
		}
		
		#endregion
		
		
		
	}
}

