using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;

namespace LifeSimulation.Core
{
	public class World
	{
		
		#region Private Variables
		
		// Data
		private ArrayList _races;				// List of all currently competing races (contains System.Type)
		private LoopList<Lifelet> _lifelets;	// List of living lifelets
		private LoopList<Message> _messages;	// List of active messages
		private LoopList<Food> _food;			// List of avaialable food
		private Hashtable _stats;				// Hashtable of all tracked stats. We use a hashtable for flexibility and automatic export/display.
		
		// World
		private int _initialLifelets;
		private SpawnMethod _spawnMethod;
		
		// Util
		private Random _randomGen;
		
		// GUI
		private Lifelet _highlightedLifelet = null;
		private Lifelet _selectedLifelet = null;
		private Point _cursor;
		
		#endregion
		
		
		
		
		#region Enumerations
		
		public enum SpawnMethod {
			GroupedRacesOnRing,
			RandomOnRing,
			Random
		}
		
		#endregion
		
		
		
		
		
		#region Properties
		
		public Lifelet HighlightedLifelet {
			get { return _highlightedLifelet; }	
			set { _highlightedLifelet = value; }
		}
		
		public Lifelet SelectedLifelet {
			get { return _selectedLifelet; }	
			set { _selectedLifelet = value; }	
		}
		
		public Random RandomGen {
			get { return _randomGen; }	
		}
		
		public LoopList<Lifelet> Lifelets {
			get { return _lifelets; }	
		}
		
		public LoopList<Message> Messages {
			get { return _messages; }	
		}
		
		public LoopList<Food> Food {
			get { return _food; }	
		}
		
		public Hashtable Statistics {
			get { return _stats; }	
		}
		
		#endregion
		
		
		
		
		#region Public Methods
		
		public World (ArrayList races, int initialLifelets, SpawnMethod spawnMethod)
		{
			// Init
			_stats = new Hashtable();
			_randomGen = new Random((int)DateTime.Now.Ticks);
			_initialLifelets = initialLifelets;
			_races = races;
			_spawnMethod = spawnMethod;
			
			// Assign a color for each race
			ArrayList colors = new ArrayList(new Color[]{Color.Red,Color.Blue,Color.Green,Color.Aqua,Color.Lime,Color.Orange});
			foreach(Type t in races) {
				Color c = Color.FromArgb(_randomGen.Next(256),_randomGen.Next(256),_randomGen.Next(256)); // Random color, if we run out
				if(colors.Count > 0) {
					int i = _randomGen.Next(colors.Count);
					c = (Color)colors[i];
					colors.RemoveAt(i);
				}
				_stats[t.FullName + "_Color"] = c;
			}
			
			// Reset
			this.Reset();
			
		}
		
		public void Reset() {
			
			// Setup data
			_lifelets = new LoopList<Lifelet>();
			_messages = new LoopList<Message>();
			_food = new LoopList<Food>();
			
			// Create the initial lifelets... 
			spawnInitialLifelets();	
		}
		
		public void CreateLifelet(int x, int y) {
			
		}
		
		public void RemoveLifelet(Lifelet life) {
			_lifelets.Remove(life);
		}
		
		public void Simulate() {
			
			// Init
			_highlightedLifelet = null;
			
			// Food
			foreach(Food food in _food) {
				food.Simulate();
			}
			
			// Messages
			foreach(Message message in _messages) {
				message.Simulate();
			}
			
			// Loop life forms
			foreach(Lifelet life in _lifelets) {
				
				// Simulate
				life.PrepareSimulate();
				life.Simulate();
				
				// Check that the simulation was performed properly
				if(life.DidSimulate != true) {
					throw new Exception("Implementing class did not call base Simulate()!");
				}
				
				// Highlighted?
				if(life.Distance(new Vector(_cursor.X,_cursor.Y)) < Config.DisplayMouseSensitivity) _highlightedLifelet = life;
			}
			
			
			
		}
		
		public void Draw(Graphics g) {
			
			// Init
			Lifelet lifeToHighlight = _highlightedLifelet;
			if(_selectedLifelet != null) lifeToHighlight = _selectedLifelet;
			
			
			// Background
			g.Clear(Color.Black);
			
			// Food
			foreach(Food food in _food) {
				food.Draw(g);
			}
			
			// Loop life forms
			foreach(Lifelet life in _lifelets) {
				// Tell life to draw
				life.Draw(g);
				
				// Debugging?
				if(Config.Debug) {
						
				}
				
				// Highlight / debugging infos
				if(life == lifeToHighlight || Config.Debug) {
					
					System.Drawing.Drawing2D.GraphicsContainer gc1 = g.BeginContainer(); {
						
						g.TranslateTransform((int)life.X,(int)life.Y); 
						
						// Draw a crosshair
						Pen pen = new Pen( life == lifeToHighlight ? Color.White : life.Color );
						int crosshairSize = life == lifeToHighlight ? (int)(Config.DisplayCrosshairSize*1.5) : Config.DisplayCrosshairSize;
						g.DrawLine(pen,0,-crosshairSize,0,crosshairSize);
						g.DrawLine(pen,-crosshairSize,0,crosshairSize,0);
							
						// Focus Circle
						g.DrawEllipse(new Pen(Color.White),
						              (int)(-life.Visibility),
						              (int)(-life.Visibility),
						              (int)(life.Visibility*2),
						              (int)(life.Visibility*2) );
						
						// Health bar
						g.FillRectangle(Brushes.Red,
						                0,
						                -(Config.DisplayCrosshairSize*3),
						                (int)((life.Health/100.0) * Config.DisplayPseudoPercentBarSize),
						                2 );
						
						// Energy bar
						g.FillRectangle(Brushes.Yellow,
						                0,
						                -(Config.DisplayCrosshairSize*3) - 4,
						                (int)((life.Energy/100.0) * Config.DisplayPseudoPercentBarSize),
						                2 );
						
					} g.EndContainer(gc1);
				}
			}
			
			// Message
			foreach(Message message in _messages) {
				message.Draw(g);
			}
			
			// Highlight infos
			string infos = "";
			if(lifeToHighlight != null) {
				
				// Infos via reflection. We only show information for properties. Note that
				// we do this operation on the virtual type, not the Life type, so any 
				// properties in an implemented life are also shown.
				Type t = lifeToHighlight.GetType();
				foreach(System.Reflection.PropertyInfo p in t.GetProperties()) {
					object val = p.GetValue(lifeToHighlight,null);
					if(val != null) infos += p.Name + ": " + val.ToString() + "\n";
				}
				infos += "\n";
			}
			
			// Show debug infos
			if(Config.Debug) {
				// World...
				//TODO
				
				// Statistics
				foreach(object k in _stats.Keys) {
					infos += k + ": " + _stats[k] + "\n";
				}
				infos += "\n";
			}

			// Draw world boundaries
			if(Config.Debug) {
				Pen pen = new Pen(Color.White);
				g.DrawLine(pen,0,-Config.DisplayCrosshairSize*3,0,Config.DisplayCrosshairSize*3);
				g.DrawLine(pen,-Config.DisplayCrosshairSize*3,0,Config.DisplayCrosshairSize*3,0);
				g.DrawEllipse(pen,-Config.WorldRadius,-Config.WorldRadius,Config.WorldRadius*2,Config.WorldRadius*2);
			}

            // Draw debug infos - no world drawing after this
            if (infos != "") {
                g.ResetTransform(); //NOTE: On Windows this doesnt reset through all the containers, so we do it in the current container and restrict any drawing after
                System.Drawing.Drawing2D.GraphicsContainer gc2 = g.BeginContainer();
                {
                    // g.ResetTransform();
                    g.DrawString(infos, Config.DisplayTextFont, Config.DisplayTextBrush, new PointF(30, 30));
                } g.EndContainer(gc2);
            }
			
		}
		
		public void SetCursor(int x, int y) {
			_cursor.X = x;
			_cursor.Y = y;
		}
		
		public void DoEnergyTransaction() {
			
		}
		
		#endregion 
		
		
		
		
		
		
		#region Public Static Methods
		
		public static ArrayList GetAvailableRaces() {
			// Init
			ArrayList races = new ArrayList();
			
			// Get from bin - this has prio
			races.AddRange(getAvailableRacesFromFolder("bin"));
			
			// Get from plugins - only add if new
			ArrayList plugins = getAvailableRacesFromFolder("plugins");
			foreach(System.Type type in plugins) {
				if(races.Contains(type) == false) races.Add(type);
			}
			
			return races;
		}
		
		#endregion
		
		
		
		
		
		#region Private Methods
		
		public static ArrayList getAvailableRacesFromFolder(string folder) {
			
			// Init
			ArrayList races = new ArrayList();
			Type lifeletType = Type.GetType("LifeSimulation.Core.Lifelet");
			
			// Loop all dll files
			System.IO.DirectoryInfo binDir = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath);
			foreach(System.IO.DirectoryInfo dir in binDir.Parent.GetDirectories(folder)) {
				foreach(string file in System.IO.Directory.GetFiles(dir.ToString(),"*.dll")) {
					
					// Load the dll
					System.Reflection.Assembly dll = System.Reflection.Assembly.LoadFrom(file);
					
					// Loop all contained types
					foreach (Type type in dll.GetTypes()) {
						
						// Is this a lifelet?
						if(type.IsSubclassOf(lifeletType)) races.Add(type);
	
					}
				}
			}
			return races;
		}
		
		private void spawnInitialLifelets() {
			
			// Init
			int lifeletsPerRace = _initialLifelets / _races.Count;
			
			// We spawn races around a outside ring
			if(_spawnMethod == SpawnMethod.GroupedRacesOnRing) {
				double radiansPerRace = Math.PI / (_races.Count-1);
				int lifeletRandomDispersion = (int)(Config.WorldInitialLifeletsDispersionAmount * (_races.Count/(_races.Count*Config.WorldInitialLifeletsToRaceDispersionRatio)));
				for(int i = 0; i < _races.Count; i++) {
					
					// Get the actual type and figure out center point
					Type lifeletType = (Type)_races[i];
					Vector raceCenter = Vector.FromAngle(Config.WorldInitialSpawnRadius,radiansPerRace*i);
					
					// Loop lifelets for race
					for(int ii = 0; ii < lifeletsPerRace; ii++) {
						
						// New position
						double x = raceCenter.X + _randomGen.Next(-lifeletRandomDispersion,lifeletRandomDispersion);
						double y = raceCenter.Y + _randomGen.Next(-lifeletRandomDispersion,lifeletRandomDispersion);
						
						// Create and register life
						Lifelet newLife = (Lifelet)Activator.CreateInstance(lifeletType,this,new Vector(x,y));
						_lifelets.Add(newLife);
					}
				}
			}
			
			// We spawn randomly on outside ring
			else if (_spawnMethod == SpawnMethod.RandomOnRing) {
				for(int i = 0; i < _races.Count; i++) {
					
					// Get the actual type and figure out center point
					Type lifeletType = (Type)_races[i];
					
					// Loop lifelets for race
					for(int ii = 0; ii < lifeletsPerRace; ii++) {
						
						// Position
						int angle = _randomGen.Next(0,360);
						Vector pos = Vector.FromAngle(Config.WorldInitialSpawnRadius,Math2.DegreeToRadian(angle));
						
						// Create and register life
						Lifelet newLife = (Lifelet)Activator.CreateInstance(lifeletType,this,pos);
						_lifelets.Add(newLife);
					}
				}
			}
			
			// Spawn food
			for(int i = 0; i < Config.WorldInitialFoodAmount; i++) {
				
				// Position
				int angle = _randomGen.Next(0,360);
				int length = _randomGen.Next(0,Config.WorldRadius);
				Vector pos = Vector.FromAngle(length,Math2.DegreeToRadian(angle));
				
				// Create and register food
				Food newFood = new Food(this,pos,Math2.JitteredValue(Config.FoodEnergy,Config.FoodEnergyJitter)); 
				_food.Add(newFood);
			}
			
		}
		
		
		
		#endregion
		
		
	
		
	}
}

