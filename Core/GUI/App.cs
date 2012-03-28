using System;
using System.Collections;

namespace LifeSimulation.Core.GUI
{
	public class App
	{
		
		#region Application Entry Point
		
		[STAThread]
		static void Main(string[] args) {
			if(args.Length > 0 && args[0] == "autostart") {
				
				// Start simulation
				ArrayList races = World.GetAvailableRaces();
				World world = new World(races,races.Count * Config.WorldInitialLifeletsPerRace,Config.WorldInitialSpawnMethod);
				Viewport viewport = new Viewport(world);
				System.Windows.Forms.Application.Run(viewport);
				
			} else {
				
				// Show startup screen
				System.Windows.Forms.Application.Run(new StartScreen());
			}
		}
		
		#endregion
		
	}
}

