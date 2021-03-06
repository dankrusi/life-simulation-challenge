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
using System.Timers;
using System.Drawing;
using System.Windows.Forms;

namespace LifeSimulation.Core.GUI
{
	public class Viewport : Form
	{
		
		
		#region Private Variables
		
        // GUI
		private System.Timers.Timer _timer;
        private Point _offset;
        private Point _mousePressLocation;
        private bool _mousePressed = false;

        // Data
		private World _world;
		
		#endregion
		
		
		
		#region Public Methods 
		
		public Viewport (World world) 
		{
			// GUI
			this.Width = Config.DisplayInitialWidth;
			this.Height = Config.DisplayInitialHeight;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true); // Enable double buffering

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
			// Make sure the simulation doesnt overtake our timer...
			_timer.Stop();
			
			// Just tell the world to simulate one step
			try{
				_world.Simulate();
			} catch(Exception error) {
				// Mono doesnt invoke the error all the way through the call stack of the timer thread so we need to crash the application ourselves...
				#if __MonoCS__
					System.Console.WriteLine(error);
					Application.Exit();
				#else 
					throw error;
				#endif
			}
			
			// Tell GUI backend to repaint
			//BUG: Cocoa always draws in a different thread so this will cause problems on Intel Macs...
            this.Invalidate();
			//this.Refresh();
			
			_timer.Start();
	    }
		
		#endregion
		
		
		
		
		
	}
}

