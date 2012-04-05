using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace LifeSimulation.Core.GUI
{
	public class StartScreen : Form
	{
		
		#region Private Variables
		
		private TableLayoutPanel _layout;
		private CheckedListBox _checkedListRaces;
		private Button _buttonSimulate;
		private Label _labelInitialLifelets;
		private TrackBar _trackbarInitialLifelets;
		
		#endregion
		
		
		
		#region Public Method
		
		public StartScreen () : base()
		{
			// Init
			
			// Window
			//this.ClientSize = new Size(300, 500);
			this.Text = "Life Simulation";
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
    		this.StartPosition = FormStartPosition.CenterScreen;
			
			// Layout
			_layout = new TableLayoutPanel();
			_layout.Dock = DockStyle.Fill;
			this.Controls.Add(_layout);
			
			// Suspend layouting
			_layout.SuspendLayout();
			this.SuspendLayout();
			
			// Races list
			_checkedListRaces = new CheckedListBox();
			_checkedListRaces.Dock = DockStyle.Fill;
			_layout.Controls.Add(createLabel("Races to Simulate:"));
			_layout.Controls.Add(_checkedListRaces);
			
			// Initial lifelets
			_labelInitialLifelets = createLabel("Initial Lifelets:");
			_layout.Controls.Add(_labelInitialLifelets);
			_trackbarInitialLifelets = new TrackBar();
			_trackbarInitialLifelets.Dock = DockStyle.Fill;
			_trackbarInitialLifelets.Minimum = 1;
			_trackbarInitialLifelets.Maximum = 1000;
			_trackbarInitialLifelets.TickStyle = TickStyle.None;
			_trackbarInitialLifelets.Scroll += new System.EventHandler(trackbarInitialLifelets_Scroll);
			_layout.Controls.Add(_trackbarInitialLifelets);
			
			// Simulate button
			_buttonSimulate = new Button();
			_buttonSimulate.Dock = DockStyle.Fill;
			_buttonSimulate.Text = "Simulate";
			_buttonSimulate.Click += new System.EventHandler(buttonSimulate_Click);
			_layout.Controls.Add(_buttonSimulate);
			
			// Load races
			ArrayList races = World.GetAvailableRaces();
			foreach(Type type in races) _checkedListRaces.Items.Add(type,true);
			_trackbarInitialLifelets.Value = (races.Count == 0 ? 1 : races.Count * Config.WorldInitialLifeletsPerRace);
			
			// Special cases
			if(races.Count == 0) {
				_buttonSimulate.Enabled = false;
			}
			
			// Resume layouting
			_layout.ResumeLayout(false);
			_layout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();
			
			// Send of some events
			trackbarInitialLifelets_Scroll(null,null);
		}
		
		#endregion
		
	
		
		
		#region UI Events
		
		private void buttonSimulate_Click(object sender, EventArgs e) {
			
			// Compile list of races
			ArrayList races = new ArrayList();
			foreach(Type type in _checkedListRaces.CheckedItems) races.Add(type);
			
			// Create world
			World world = new World(races,_trackbarInitialLifelets.Value,Config.WorldInitialSpawnMethod);
			
			// Create and show viewport
			Viewport viewport = new Viewport(world);
			viewport.Show();
		}
		
		private void trackbarInitialLifelets_Scroll(object sender, EventArgs e) {
			_labelInitialLifelets.Text = "Initial Lifelets: " + _trackbarInitialLifelets.Value;
		}
		
		#endregion
		
		
		
		
		
		
		#region Private Methods
		
		private Label createLabel(string text) {
			Label lbl = new Label();
			lbl.Text = text;
			lbl.Dock = DockStyle.Fill;
			return lbl;
		}
		
		#endregion
		
	}
	
}
