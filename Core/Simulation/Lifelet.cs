using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;


namespace LifeSimulation.Core
{
	

	
	public abstract class Lifelet
	{
	
		#region Private Variables - these can't be changed by contestents directly
		
		private long _uid = -1;
		private static long _gid = 0;
		
		private World _world;
		
		private Vector _pos;
		private Vector _vel;
		private Vector _velReq;
		
		private int _age;
		
		private double _health;
		private double _lastHealth;
		
		private double _energy;
		private double _lastEnergy;
		
		private bool _didSimulate = false; // This is used to make sure implementing classes call this
		
		private long _lastTalk = long.MinValue; 
		private long _lastAttack = long.MinValue; 
		private long _lastGiveEnergy = long.MinValue; 
		
		private Color _color;
		
		#endregion
		
		

		
		
		
		#region Protected Variables - these can be changed by contestents
		
		
		#endregion
		
		
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		public virtual string Race {
			get { return null; }
		}
		
		public string Type {
			get { return this.GetType().FullName; }
		}
		
		public virtual string Author {
			get { return null; }
		}
		
		public Version Version {
			get { return System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version; }
		}
		
		public long UID {
			get { return _uid; }
		}
		
		public long Age {
			get { return _age; }
		}
		
		public Vector Position {
			get { return _pos; } //TODO: Hide this from calling lifelets which are not in the visibility range...
		}
		
		public double X {
			get { return _pos.X; }
		}
		
		public double Y {
			get { return _pos.Y; }
		}
		
		public Vector Velocity {
			get { return _vel; }
		}
		
		protected Random RandomGen {
			get { return _world.RandomGen; }
		}
		
		public double Energy {
			get { return _energy; }
		}
		
		public double Health {
			get { return _health; }
		}
		
		public double Visibility {
			get { return (_health/Config.LifeletInitialHealth) * Config.LifeletVisionRadius; } //TODO
		}
		
		public long HealthExpectancy {
			get {
				double delta = _lastHealth - _health;
				if(delta == 0) return long.MaxValue;
				return (long)(_health / delta); 
			}
		}
		
		public long EnergyExpectancy {
			get {
				double delta = _lastEnergy - _energy;
				if(delta == 0) return long.MaxValue;
				return (long)(_energy / delta); 
			}
		}
		
		public long LifeExpectancy {
			get {
				long expentancy = this.HealthExpectancy;
				if(this.EnergyExpectancy < expentancy) expentancy = this.EnergyExpectancy;
				return this.Age + expentancy; 
			}
		}
		
		public System.Drawing.Color Color {
			get { return _color; }
		}
		
		public bool DidSimulate {
			get { return _didSimulate; }
		}
		
		public bool CriticalEnergy {
			get { return _energy < Config.LifeletInitialEnergy * Config.LifeletLowEnergyHealthBounds; }
		}
		
		public int VisibleLifelets {
			get { return visibleLifelets().Count; }
		}
		
		public int VisibleFood {
			get { return visibleFood().Count; }
		}
		
		public int AudibleMessages {
			get { return audibleMessages().Count; }
		}
		
		public bool CanTalk {
			get { return this.Age > _lastTalk + Config.LifeletMinimumTalkDelay; }
		}
		
		public bool CanGiveEnergy {
			get { return this.Age > _lastGiveEnergy + Config.LifeletMinimumGiveEnergyDelay; }
		}
		
		public bool CanAttack {
			get { return this.Age > _lastAttack + Config.LifeletMinimumAttackDelay; }
		}
			
		#endregion
		
		
		
		
		#region Public Methods
		
		public Lifelet (World world, Vector pos)
		{
			// Init
			_world = world;
			_uid = _gid++;
			_pos = pos;
			_vel = new Vector(0,0);
			_velReq = new Vector(0,0);
			_color = (Color)world.Statistics[this.Type + "_Color"]; // We assign a fixed color based on the value kept in the statistics
			_age = 0;
			_health = Config.LifeletInitialHealth;
			_energy = Config.LifeletInitialEnergy;
		}
		
		public void PrepareSimulate() {
			_didSimulate = false;
		}
		
		public virtual void Simulate() {
						
			// Age
			_age += Config.LifeletAgeIncrement;
			
			// Energy
			_lastEnergy = _energy;
			_energy -= Config.LifeletNaturalEnergyDecrement;
			if(_energy < 0) _energy = 0;
			
			// Consume food
			foreach(Food food in _world.Food) {
				if(food.Distance(this) < 10) {
					_energy += food.Energy;
					_world.Food.Remove(food);
				}
			}
			
			// Make sure we are moving within world radius
			Vector newPos = _pos + _velReq;
			if(newPos.Distance(Vector.Empty) > Config.WorldRadius) {
				// Can't do this, out of bounds!
				_velReq = new Vector(0,0);
			}
			
			// Calculate energy required for move
			double energyNeeded = _velReq.Norm() * Config.LifeletMovementEnergyMultiplier;
			if(_energy - energyNeeded > 0) {
				
				// Set the velocity and book the energy
				_energy -= energyNeeded;
				_vel = _velReq;
				
			} else {
				
				// Can't do this, no energy!
				_vel = new Vector(0,0);
				
			}
			
			// Process movement
			_pos += _vel;
			
			// Health
			_lastHealth = _health;
			_health -= Config.LifeletNaturalHealthDecrement;
			if(this.CriticalEnergy == true) {
				_health -= Config.LifeletLowEnergyHealthDecrement;
			}
			if ( _health <= Config.LifeletMinimumHealth )	this.die();
			
			// Register that the base simulate was called
			_didSimulate = true;
		}
		
		public void Draw(Graphics g) {
			
			// Draw body
			int size = 4;
			g.FillEllipse(new SolidBrush(this.Color),
			              (float)this.X - size/2,
			              (float)this.Y - size/2,
			              size,
			              size );
			
			// Debug draw
			if(Config.Debug) debugDraw(g);
			
		}
		
		public double Distance(Lifelet other) {
			return this.Position.Distance(other.Position);
		}
		
		public double Distance(Vector point) {
			return this.Position.Distance(point);
		}
		
		public Lifelet Closest() {
			double smallestDist = Double.MaxValue;
			Lifelet closestLife = null;
			foreach(Lifelet life in _world.Lifelets) {
				double dist = life.Distance(this);
				if(life != this && dist < smallestDist) {
					smallestDist = dist;
					closestLife = life;
				}
			}
			return closestLife;
		}
		
		#endregion
		
		
		
		
		
		#region Protected Methods
		
		protected List<Lifelet> visibleLifelets() {
			//TODO: cache this per simulation round
			
			// Loop all and find those in range
			List<Lifelet> ret = new List<Lifelet>();
			foreach(Lifelet lifelet in _world.Lifelets) {
				if(lifelet != this && lifelet.Distance(this) < this.Visibility) {
					ret.Add(lifelet);
				}
			}
			return ret;
		}
		
		protected List<Food> visibleFood() {
			//TODO: cache this per simulation round
			
			// Loop all and find those in range
			List<Food> ret = new List<Food>();
			foreach(Food food in _world.Food) {
				if(food.Distance(this) < this.Visibility) {
					ret.Add(food);
				}
			}
			return ret;
		}
		
		protected List<Message> audibleMessages() {
			//TODO: cache this per simulation round
			
			// Loop all and find those in range
			List<Message> ret = new List<Message>();
			foreach(Message message in _world.Messages) {
				if(message.Distance(this) - message.Radius < this.Visibility) {
					ret.Add(message);
				}
			}
			return ret;
		}
		
		protected void move(Vector v) {
			// Put a cap on vel
			double n = v.Norm();
			if(n > Config.LifeletMaximumVelocityRequest) {
				v = v.Normalize() * Config.LifeletMaximumVelocityRequest;
			}
			// Update requested velocity for next simulate
			_velReq = v;
		}
		
		protected void moveToDestination(Vector destination, double speed) {
			Vector direction = destination - this.Position;
			this.move(direction.Normalize() * speed);
		}
		
		protected void moveRandomly(double speed) {
			this.move(new Vector(	this.RandomGen.Next((int)-speed,(int)speed+1),
			          				this.RandomGen.Next((int)-speed,(int)speed+1) ));
		}
		
		protected void talk(char message) {
			
			// Check delay
			if(!this.CanTalk) return;
			
			// Reformat message
			message = message.ToString().ToUpper()[0];
			
			// Make sure it is valid
			if(!Config.MessageLanguage.Contains(message.ToString())) {
				throw new Exception("Invalid message contents! The message must use the following language: " + Config.MessageLanguage);	
			}
			
			// Send message
			_world.Messages.Add(new Message(_world,message,this));
			_lastTalk = this.Age;
		}
		
		protected void giveEnergy(Lifelet other, double amount) {
			
			// Check delay
			if(!this.CanGiveEnergy) return;
			
			// Make sure we have this energy to give and we are in distance
			if(this.Energy >= amount && this.Distance(other) < this.Visibility) {
				_energy -= amount;
				other.recieveEnergy(this,amount); 
			}
			
		}
		
		protected void attack(Lifelet other, double amount) {
			
			// Check delay
			if(!this.CanAttack) return;
			
			// Make sure we have this energy to give and we are in distance
			if(this.Energy >= amount && this.Distance(other) < Config.LifeletMaximumAttackRange) {
				_energy -= amount;
				other.recieveAttack(this,amount); 
			}
			
		}
		
		#endregion
		
		
		
		
		
		
		#region Private Methods
		
		private void die() {
			// Remove the lifelet
			_world.RemoveLifelet(this);
			
			// Add food here
			_world.Food.Add(new Food(_world,this.Position,this.Energy)); //TODO
		}
		
		private void recieveEnergy(Lifelet sender, double amount) {
			//TODO: should we tax this with a conversion ratio?
			_energy += amount;
		}
		
		private void recieveAttack(Lifelet sender, double amount) {
			//TODO: should we tax this with a conversion ratio?
			_health -= 50.0;//(amount*4); //TODO
		}
		
		protected virtual void debugDraw(Graphics g) {
			
		}
		
		#endregion
		
	}
}

