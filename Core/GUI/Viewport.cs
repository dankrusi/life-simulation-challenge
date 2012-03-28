using System;
using System.Timers;
using System.Drawing;
using System.Windows.Forms;

namespace LifeSimulation.Core.GUI
{
	public class Viewport : Form
	{
		
		
		#region Private Variables
		
		private System.Timers.Timer _timer;
		private World _world;
		private Point _offset;
		private Point _mousePressLocation;
		private bool _mousePressed = false;
		
		#endregion
		
		
		
		#region Public Methods 
		
		public Viewport (World world) 
		{
			// GUI
			this.Width = Config.DisplayInitialWidth;
			this.Height = Config.DisplayInitialHeight; 
			
			// Show version
			Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			this.Text = "Life Simulation - Version " + v.Major + "." + v.Minor + " Build " + v.Build;
			 
			// World
			_world = world;
			
			// Simulate Timer
			_timer = new System.Timers.Timer(1000/Config.DisplaySimulateHerz);
			_timer.Elapsed += new ElapsedEventHandler(onTimer);
			_timer.Start();
		}
		
		#endregion
		
		
		
		#region Protected Methods
		
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			
			// Translate for viewport
			e.Graphics.TranslateTransform(this.ClientSize.Width/2 + _offset.X, this.ClientSize.Height/2 + _offset.Y); {
			
				// Tell world to draw onto viewport
				_world.Draw(e.Graphics);
				
			} e.Graphics.ResetTransform();
			
		}
		
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
			_mousePressLocation = new Point(e.X-_offset.X,e.Y-_offset.Y);
			_mousePressed = true;
			this.Cursor = Cursors.Hand;
		}
		
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			
			_mousePressed = false;
			this.Cursor = Cursors.Default;
		}
		
		protected override void OnMouseClick(MouseEventArgs e)
		{
			base.OnMouseClick(e);
			
			if(_world.HighlightedLifelet != null) {
				_world.SelectedLifelet = _world.HighlightedLifelet;
			} else if(_world.SelectedLifelet != null) {
				_world.SelectedLifelet = null;
			} else {
				//_world.CreateLifelet(e.X - (this.ClientSize.Width/2) + 0,e.Y - (this.ClientSize.Height/2) + 0);
			}
		}
		
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			
			if(_mousePressed) {
				// Drag world
				_offset = new Point(e.Location.X-_mousePressLocation.X,e.Location.Y-_mousePressLocation.Y);
				//_mousePressLocation = _offset;
			} else {
				// Update world cursor for highlight...
				_world.SetCursor(e.X - (this.ClientSize.Width/2) - _offset.X,e.Y - (this.ClientSize.Height/2) - _offset.Y);
			}
		}
		
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			if(e.KeyChar == 'd') 		Config.Debug = !Config.Debug;
			else if(e.KeyChar == 'r') 	_world.Reset();
		}
	
		#endregion
		
		
		
		#region Private Methods
		
		private void onTimer(object sender, ElapsedEventArgs e)
	    {
			// Just tell the world to simulate one step
			_world.Simulate();
			
			// Tell GUI backend to repaint
			//BUG: Cocoa always draws in a different thread so this will cause problems on Intel Macs...
			this.Refresh();
	    }
		
		#endregion
		
		
		
		
		
	}
}

