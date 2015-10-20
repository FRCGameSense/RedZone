using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            public List<FRCApi.MatchResult> Matches { get; set; }
            public List<SubdivisionMatchView> DisplayMatches { get; set; }

            public Subdivision()
            {
                this.Event = new FRCApi.Event();
                this.Matches = new List<FRCApi.MatchResult>();
            }

            public Subdivision(FRCApi.Event evt, List<FRCApi.MatchResult> matchList)
            {
                this.Event = evt;
                this.Matches = matchList;
                this.DisplayMatches = new List<SubdivisionMatchView>();
                foreach (FRCApi.MatchResult m in this.Matches)
                {
                    this.DisplayMatches.Add(new SubdivisionMatchView(m));
                }
            }
        }

        private class SubdivisionMatchView
        {
            public string Match { get; set; }
            public string RedAlliance { get; set; }
            public string BlueAlliance { get; set; }
            public double RedZoneScore { get; set; }

            public SubdivisionMatchView() { }

            public SubdivisionMatchView(FRCApi.MatchResult frcMatch)
            {
                this.Match = frcMatch.description.Substring(0, 1) + frcMatch.matchNumber;
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
                subdivisions.Add(new Subdivision(evt, api.getMatchResults(year, evt.code)));
            }

            dataGridView1.DataSource = subdivisions.First().DisplayMatches;
        }
    }
}
