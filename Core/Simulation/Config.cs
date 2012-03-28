using System;
using System.Drawing;

namespace LifeSimulation.Core
{
	public class Config
	{
		
		
		#region Debug Settings
		
		public static bool 	Debug = false;
		
		#endregion
		
		
		
		#region Display Settings
		
		public const int 	DisplayInitialWidth = 800;
		public const int 	DisplayInitialHeight = 600;
		public const int 	DisplaySimulateHerz = 10;
		public const int 	DisplayMouseSensitivity = 10;
		public const int 	DisplayCrosshairSize = 5;
		public const int 	DisplayPseudoPercentBarSize = 20;
		public static Font 	DisplayTextFont = new Font("Lucida Console", 8);
		public static Brush DisplayTextBrush = new SolidBrush(Color.White);
		
		#endregion
		
		
		
		#region Food Settings
		
		public const int	FoodMaxAge = 10000;
		public const int	FoodMaxAgeJitter = 3000;
		public const int	FoodEnergy = 15;
		public const int	FoodEnergyJitter = 10;
		
		#endregion
		
		
		
		#region Message Settings
		
		public const int	MessageMaxAge = 20;
		public const int	MessageMaxAgeJitter = 5;
		public const string MessageLanguage = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?#";
		public const double MessageAgeToRadiusRatio = 6.0;
		
		#endregion
		
		
		
		#region World Settings
		
		
		public const int				WorldRadius = 1000;
		public const int				WorldInitialLifeletsPerRace = 10;
		public const double				WorldInitialLifeletsToRaceDispersionRatio = 1.0;
		public const double				WorldInitialLifeletsDispersionAmount = 50;
		public const World.SpawnMethod	WorldInitialSpawnMethod = World.SpawnMethod.GroupedRacesOnRing;
		public const int				WorldInitialSpawnRadius = 300;
		public const int				WorldInitialFoodAmount = 100;
		
		#endregion
		
		
		
		
		
		#region Lifelet Settings
		
		// Age
		public const int 	LifeletAgeIncrement = 1;
		
		// Health
		public const double LifeletInitialHealth = 100.0; // Pseudo-percent
		public const double LifeletMinimumHealth = 0.001;
		public const double LifeletNaturalHealthDecrement = 0.0001;
		public const double LifeletLowEnergyHealthBounds = 0.10; // Percent
		public const double LifeletLowEnergyHealthDecrement = 0.1; 
		
		// Energy
		public const double LifeletInitialEnergy = 100.0; // Pseudo-percent
		public const double LifeletMinumumEnergy = 0.001;
		public const double LifeletNaturalEnergyDecrement = 0.001;
		public const double LifeletMovementEnergyMultiplier = 0.1;
		
		// Movement
		public const double LifeletMaximumVelocityRequest = 3.0;
		
		// Vision
		public const double LifeletVisionRadius = 40.0;
		
		// Attack
		public const double LifeletMaximumAttackRange = 10.0;
		
		// Delays
		public const long	LifeletMinimumTalkDelay = 30;
		public const long	LifeletMinimumAttackDelay = 1;
		public const long	LifeletMinimumGiveEnergyDelay = 3;
		
		#endregion
		
		
		
		
		
	}
}

