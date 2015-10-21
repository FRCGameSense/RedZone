using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace RedZoneHelper
{
    public partial class Form1 : Form
    {
        FRCApi api = new FRCApi();

        List<FRCApi.MatchResult> archimedesMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> galileoMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> teslaMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> newtonMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> hopperMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> curieMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> carverMatches = new List<FRCApi.MatchResult>();
        List<FRCApi.MatchResult> carsonMatches = new List<FRCApi.MatchResult>();
                
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Holder for all pertinent subdivision info.
        /// TODO: add TBA data
        /// </summary>
        private class Subdivision
        {
            public FRCApi.Event Event { get; set; }
            public List<FRCApi.HybridScheduleMatch> Matches { get; set; }
            public List<SubdivisionMatchView> DisplayMatches { get; set; }

            public Subdivision()
            {
                this.Event = new FRCApi.Event();
                this.Matches = new List<FRCApi.HybridScheduleMatch>();
            }

            public Subdivision(FRCApi.Event evt, List<FRCApi.HybridScheduleMatch> matchList)
            {
                this.Event = evt;
                this.Matches = matchList;
                /*
                if (this.Matches[0].autoStartTime.Hour != 8)
                {
                    foreach (FRCApi.MatchResult m in this.Matches)
                    {
                        m.autoStartTime = m.autoStartTime.AddHours(5);
                    }
                }*/
                this.DisplayMatches = new List<SubdivisionMatchView>();
                foreach (FRCApi.HybridScheduleMatch m in this.Matches)
                {                    
                    this.DisplayMatches.Add(new SubdivisionMatchView(m));
                }

            }

        }

        private class SubdivisionMatchView
        {
            public string Match { get; set; }
            public DateTime Time { get; set; }
            public string RedAlliance { get; set; }
            public string BlueAlliance { get; set; }
            public double RedZoneScore { get; set; }

            public SubdivisionMatchView() { }

            public SubdivisionMatchView(FRCApi.HybridScheduleMatch frcMatch)
            {
                if (frcMatch.tournamentLevel.ToLower() == "qualification")
                {
                    this.Match = frcMatch.description.Substring(0, 1) + frcMatch.matchNumber;
                }
                else
                {
                    this.Match = frcMatch.description.Substring(0, 1)+ "F" + frcMatch.matchNumber;
                }                
                this.Time = frcMatch.actualStartTime;
                //Console.WriteLine(frcMatch.autoStartTime.ToUniversalTime());
                this.RedAlliance = frcMatch.RedAllianceString;
                this.BlueAlliance = frcMatch.BlueAllianceString;
                this.RedZoneScore = 1.0;
                
            }
        }

        private void goButton_Click(object sender, EventArgs e)
        {
            int year = Convert.ToInt16(yearBox.Value);

            List<FRCApi.Event> events = api.getEvents(year);
            List<FRCApi.Event> cmpEvents = events.FindAll(i => i.name.IndexOf("Subdivision") != -1);
            List<Subdivision> subdivisions = new List<Subdivision>();

            foreach (FRCApi.Event evt in cmpEvents)
            {
                subdivisions.Add(new Subdivision(evt, api.getHybridSchedule(year,evt.code,"qual")));
            }

            dataGridView0.DataSource = subdivisions.Find(i => i.Event.code == "CMP-ARCHIMEDES").DisplayMatches;
            dataGridView1.DataSource = subdivisions.Find(i => i.Event.code == "CMP-CARSON").DisplayMatches;
            dataGridView2.DataSource = subdivisions.Find(i => i.Event.code == "CMP-CARVER").DisplayMatches;
            dataGridView3.DataSource = subdivisions.Find(i => i.Event.code == "CMP-CURIE").DisplayMatches;
            dataGridView4.DataSource = subdivisions.Find(i => i.Event.code == "CMP-GALILEO").DisplayMatches;
            dataGridView5.DataSource = subdivisions.Find(i => i.Event.code == "CMP-HOPPER").DisplayMatches;
            dataGridView6.DataSource = subdivisions.Find(i => i.Event.code == "CMP-NEWTON").DisplayMatches;
            dataGridView7.DataSource = subdivisions.Find(i => i.Event.code == "CMP-TESLA").DisplayMatches;

            label0.Text = subdivisions.Find(i => i.Event.code == "CMP-ARCHIMEDES").Event.code.Substring(4);
            label1.Text = subdivisions.Find(i => i.Event.code == "CMP-CARSON").Event.code.Substring(4);
            label2.Text = subdivisions.Find(i => i.Event.code == "CMP-CARVER").Event.code.Substring(4);
            label3.Text = subdivisions.Find(i => i.Event.code == "CMP-CURIE").Event.code.Substring(4);
            label4.Text = subdivisions.Find(i => i.Event.code == "CMP-GALILEO").Event.code.Substring(4);
            label5.Text = subdivisions.Find(i => i.Event.code == "CMP-HOPPER").Event.code.Substring(4);
            label6.Text = subdivisions.Find(i => i.Event.code == "CMP-NEWTON").Event.code.Substring(4);
            label7.Text = subdivisions.Find(i => i.Event.code == "CMP-TESLA").Event.code.Substring(4);

            dataGridView0.AutoResizeColumns();
            dataGridView1.AutoResizeColumns();
            dataGridView2.AutoResizeColumns();
            dataGridView3.AutoResizeColumns();
            dataGridView4.AutoResizeColumns();
            dataGridView5.AutoResizeColumns();
            dataGridView6.AutoResizeColumns();
            dataGridView7.AutoResizeColumns();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            ResizeDataGridViews();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ResizeDataGridViews();

        }

        private void ResizeDataGridViews()
        {
            int dgvSpacing = 5; //pixels
            Point startingLocation = new Point(5, 50);
            int dgvWidth = (this.Size.Width - startingLocation.X - dgvSpacing * 7) / 4;
            int dgVHeight = (this.Size.Height - startingLocation.Y - dgvSpacing * 10 - label0.Height) / 2;

            dataGridView0.Width = dgvWidth;
            dataGridView0.Height = dgVHeight;
            dataGridView0.Location = startingLocation;

            dataGridView1.Width = dgvWidth;
            dataGridView1.Height = dgVHeight;
            dataGridView1.Location = new Point(dataGridView0.Location.X + dgvWidth + dgvSpacing, dataGridView0.Location.Y);

            dataGridView2.Width = dgvWidth;
            dataGridView2.Height = dgVHeight;
            dataGridView2.Location = new Point(dataGridView1.Location.X + dgvWidth + dgvSpacing, dataGridView0.Location.Y);

            dataGridView3.Width = dgvWidth;
            dataGridView3.Height = dgVHeight;
            dataGridView3.Location = new Point(dataGridView2.Location.X + dgvWidth + dgvSpacing, dataGridView0.Location.Y);

            dataGridView4.Width = dgvWidth;
            dataGridView4.Height = dgVHeight;
            dataGridView4.Location = new Point(dataGridView0.Location.X, dataGridView0.Location.Y + dgvSpacing + dgVHeight + label0.Height);

            dataGridView5.Width = dgvWidth;
            dataGridView5.Height = dgVHeight;
            dataGridView5.Location = new Point(dataGridView4.Location.X + dgvWidth + dgvSpacing, dataGridView4.Location.Y);

            dataGridView6.Width = dgvWidth;
            dataGridView6.Height = dgVHeight;
            dataGridView6.Location = new Point(dataGridView5.Location.X + dgvWidth + dgvSpacing, dataGridView4.Location.Y);

            dataGridView7.Width = dgvWidth;
            dataGridView7.Height = dgVHeight;
            dataGridView7.Location = new Point(dataGridView6.Location.X + dgvWidth + dgvSpacing, dataGridView4.Location.Y);

            label0.Location = new Point(dataGridView0.Location.X + dgvWidth / 2 - (label0.Width / 2), dataGridView0.Location.Y - label0.Height);
            label1.Location = new Point(dataGridView1.Location.X + dgvWidth / 2 - (label1.Width / 2), dataGridView1.Location.Y - label1.Height);
            label2.Location = new Point(dataGridView2.Location.X + dgvWidth / 2 - (label2.Width / 2), dataGridView2.Location.Y - label2.Height);
            label3.Location = new Point(dataGridView3.Location.X + dgvWidth / 2 - (label3.Width / 2), dataGridView3.Location.Y - label3.Height);
            label4.Location = new Point(dataGridView4.Location.X + dgvWidth / 2 - (label4.Width / 2), dataGridView4.Location.Y - label4.Height);
            label5.Location = new Point(dataGridView5.Location.X + dgvWidth / 2 - (label5.Width / 2), dataGridView5.Location.Y - label5.Height);
            label6.Location = new Point(dataGridView6.Location.X + dgvWidth / 2 - (label6.Width / 2), dataGridView6.Location.Y - label6.Height);
            label7.Location = new Point(dataGridView7.Location.X + dgvWidth / 2 - (label7.Width / 2), dataGridView7.Location.Y - label7.Height);

        }
        
    }
}
