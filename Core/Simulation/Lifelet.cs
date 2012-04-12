/* 
 * The Life Simulation Challenge
 * https://github.com/dankrusi/life-simulation-challenge
 * 
 * Contributor(s):
 *   Dan Krusi <dan.krusi@nerves.ch> (original author)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in 
 * the Software without restriction, including without limitation the rights to 
 * use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
 * of the Software, and to permit persons to whom the Software is furnished to do 
 * so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 * THE SOFTWARE.
 */

using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;


namespace LifeSimulation.Core
{
	

	/// <summary>
	/// The Lifelet class represents a single lifeform in the simulation. 
	/// </summary>
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
		
		private bool _didSimulate = false; // This is used to make sure implementing classes call base.Simulate()
		
		private long _lastTalk = long.MinValue; 
		private long _lastAttack = long.MinValue; 
		private long _lastGiveEnergy = long.MinValue; 
		
		private Color _color;
		
		#endregion
		
		

		
		
		
		#region Protected Variables - these can be changed by contestents
		
		
		#endregion
		
		
		
		
		#region Properties - these can be accessed by anyone and are visible on the debug display
		
		/// <summary>
		/// Gets the race name for this lifelet.
		/// </summary>
		public virtual string Race {
			get { return null; }
		}
		
		/// <summary>
		/// Gets the full type name for this lifelet.
		/// </summary>
		public string Type {
			get { return this.GetType().FullName; }
		}
		
		/// <summary>
		/// Gets the author's name for this lifelet.
		/// </summary>
		public virtual string Author {
			get { return null; }
		}
		
		/// <summary>
		/// Gets the current build version of this lifelet.
		/// </summary>
		public Version Version {
			get { return System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version; }
		}
		
		/// <summary>
		/// Returns the lifelets unique ID.
		/// </summary>
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
		
		/// <summary>
		/// Provides a handle to a seeded random generator.
		/// </summary>
		protected Random RandomGen {
			get { return _world.RandomGen; }
		}
		
		public double Energy {
			get { return _energy; }
		}
		
		public double Health {
			get { return _health; }
		}
		
		/// <summary>
		/// Gets the current visiblity for the lifelet. Lifelets which have bad health have also bad visibility.
		/// </summary>
		public double Visibility {
			get { return (_health/Config.LifeletInitialHealth) * Config.LifeletVisionRadius; } //TODO
		}
		
		/// <summary>
		/// Returns a calculated health supply expectancy in simulation steps.
		/// </summary>
		public long HealthExpectancy {
			get {
				double delta = _lastHealth - _health;
				if(delta == 0) return long.MaxValue;
				return (long)(_health / delta); 
			}
		}
		
		/// <summary>
		/// Returns a calculated energy supply expectancy in simulation steps.
		/// </summary>
		public long EnergyExpectancy {
			get {
				double delta = _lastEnergy - _energy;
				if(delta == 0) return long.MaxValue;
				return (long)(_energy / delta); 
			}
		}
		
		/// <summary>
		/// Returns a calculated life expectancy in simulation steps based on the health and energy expectancy.
		/// </summary>
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
		
		/// <summary>
		/// Returns true if the lifelet is currently running on critical energy.
		/// </summary>
		public bool CriticalEnergy {
			get { return _energy < Config.LifeletInitialEnergy * Config.LifeletLowEnergyHealthBounds; }
		}
		
		/// <summary>
		/// Returns the number of currently visible lifelets.
		/// </summary>
		public int VisibleLifelets {
			get { return visibleLifelets().Count; }
		}
		
		/// <summary>
		/// Returns the number of currently visible food.
		/// </summary>
		public int VisibleFood {
			get { return visibleFood().Count; }
		}
		
		/// <summary>
		/// Returns the number of currently audible messages.
		/// </summary>
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
		
		/// <summary>
		/// Initializes a new instance of the <see cref="LifeSimulation.Core.Lifelet"/> class.
		/// </summary>
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
		
		/// <summary>
		/// Prepares a lifelet for simulation by an extending class.
		/// </summary>
		public void PrepareSimulate() {
			_didSimulate = false;
		}
		
		/// <summary>
		/// Simulate this instance for one timestep. Extending classes must call base.Simulate() otherwise they are disqualified.
		/// </summary>
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
		
		/// <summary>
		/// Draws the current state of the lifelet to the graphics context.
		/// </summary>
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
		
		/// <summary>
		/// Measures the distance from this lifelet to another.
		/// </summary>
		public double Distance(Lifelet other) {
			return this.Position.Distance(other.Position);
		}
		
		/// <summary>
		/// Measures the distance from this lifelet to another.
		/// </summary>
		public double Distance(ShelledLifelet other) {
			return this.Position.Distance(other.Position);
		}
		
		/// <summary>
		/// Measures the distance from this lifelet to a specific point.
		/// </summary>
		public double Distance(Vector point) {
			return this.Position.Distance(point);
		}
		
		public ShelledLifelet Shell() {
			return new ShelledLifelet(this,_world);
		}
		
		#endregion
		
		
		
		
		
		#region Protected Methods
		
		/// <summary>
		/// Returns the closest visible lifelet.
		/// </summary>
		protected ShelledLifelet closestLifelet() {
			double smallestDist = Double.MaxValue;
			ShelledLifelet closestLife = null;
			foreach(ShelledLifelet life in visibleLifelets()) {
				double dist = life.Distance(this);
				if(life.UID != this.UID && dist < smallestDist) {
					smallestDist = dist;
					closestLife = life;
				}
			}
			return closestLife;
		}
		
		/// <summary>
		/// Returns the visible lifelet by UID.
		/// </summary>
		protected ShelledLifelet getLifeletByUID(long uid) {
			foreach(ShelledLifelet lifelet in visibleLifelets()) {
				if(lifelet.UID == uid) return lifelet;
			}
			return null;
		}
		
		/// <summary>
		/// Returns all visible lifelets.
		/// </summary>
		protected List<ShelledLifelet> visibleLifelets() {
			//TODO: cache this per simulation round
			
			// Loop all and find those in range
			List<ShelledLifelet> ret = new List<ShelledLifelet>();
			foreach(Lifelet lifelet in _world.Lifelets) {
				if(lifelet != this && lifelet.Distance(this) < this.Visibility) {
					ret.Add(lifelet.Shell());
				}
			}
			return ret;
		}
		
		/// <summary>
		/// Returns all visible food.
		/// </summary>
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
		
		/// <summary>
		/// Returns all audible messages.
		/// </summary>
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
		
		/// <summary>
		/// Sets a request to move the lifelet with the given velocity.
		/// </summary>
		protected void move(Vector v) {
			// Put a cap on vel
			double n = v.Norm();
			if(n > Config.LifeletMaximumVelocityRequest) {
				v = v.Normalize() * Config.LifeletMaximumVelocityRequest;
			}
			// Update requested velocity for next simulate
			_velReq = v;
		}
		
		/// <summary>
		/// Sets a request to move the lifelet in the direction of destination with the given speed.
		/// </summary>
		protected void moveToDestination(Vector destination, double speed) {
			Vector direction = destination - this.Position;
			this.move(direction.Normalize() * speed);
		}
		
		/// <summary>
		/// Sets a request to move the lifelet randomly with the given speed.
		/// </summary>
		protected void moveRandomly(double speed) {
			this.move(new Vector(	this.RandomGen.Next((int)-speed,(int)speed+1),
			          				this.RandomGen.Next((int)-speed,(int)speed+1) ));
		}
		
		/// <summary>
		/// Creates a new message originating from this lifelet.
		/// </summary>
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
		
		/// <summary>
		/// Gives a specific amount of energy to another lifelet.
		/// </summary>
		protected void giveEnergy(ShelledLifelet other, double amount) {
			
			// Check delay
			if(!this.CanGiveEnergy) return;
			
			// Make sure we have this energy to give and we are in distance
			if(this.Energy >= amount && this.Distance(other) < this.Visibility) {
				_energy -= amount;
				_world.UnshellLifelet(other).recieveEnergy(this,amount);
			}
			
		}
		
		/// <summary>
		/// Attacks another lifelet with the given amount of energy.
		/// </summary>
		protected void attack(ShelledLifelet other, double amount) {
			
			// Check delay
			if(!this.CanAttack) return;
			
			// Make sure we have this energy to give and we are in distance
			if(this.Energy >= amount && this.Distance(other) < Config.LifeletMaximumAttackRange) {
				_energy -= amount;
				_world.UnshellLifelet(other).recieveAttack(this,amount);
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

