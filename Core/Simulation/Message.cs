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

namespace LifeSimulation.Core
{
	/// <summary>
	/// The Message class represents a single message which is broadcasted by a lifelet.
	/// </summary>
	public class Message : Obstacle
	{
		#region Private Variables
		
		private char _contents;
		private Lifelet _sender;
		
		#endregion
		
		
		
		
		#region Properties
		
		public char Contents {
			get { return _contents; }
		}
		
		public int Radius {
			get { return (int)(this.Age * Config.MessageAgeToRadiusRatio); }
		}
		
		public ShelledLifelet Sender {
			get { return _sender.Shell(); }
		}
		
		#endregion
		
		
		
		#region Public Methods
		
		public Message (World world, char contents, Lifelet sender) : base(world,sender.Position)
		{
			// Init
			_contents = contents;
			_maxAge = Math2.JitteredValue(Config.MessageMaxAge,Config.MessageMaxAgeJitter);
			_sender = sender;
		}
		
		public override void Simulate() {
			base.Simulate();
			
			// Die of old age...
			if(this.Age > _maxAge) _world.Messages.Remove(this);
			
		}
		
		public override void Draw(Graphics g) {
			base.Draw(g);
			
			System.Drawing.Drawing2D.GraphicsContainer gc1 = g.BeginContainer(); {
					
				g.TranslateTransform((int)this.X,(int)this.Y); 
				
				// Color
				int alpha = (int)( (1.0 - (double)this.Age/(double)_maxAge) * 255 );
				if(alpha > 255) alpha = 255;
				Color c = Color.FromArgb(alpha,Color.White);
				
				// Focus Circle
				g.DrawEllipse(new Pen(c),
				              (int)(-this.Radius),
				              (int)(-this.Radius),
				              (int)(this.Radius*2),
				              (int)(this.Radius*2) );
				
				// Draw message
				g.DrawString(this.Contents.ToString(), Config.DisplayTextFont, new SolidBrush(c), new PointF(-Config.DisplayTextFont.Size/2,this.Radius-Config.DisplayTextFont.Size*2));
				
			} g.EndContainer(gc1);
			
		}
		
		#endregion
	}
}

