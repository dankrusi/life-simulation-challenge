using System;
using System.Drawing;

namespace LifeSimulation.Core
{
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
		
		public Lifelet Sender {
			get { return _sender; }
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

