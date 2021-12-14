using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class Match
    {
        private int gameId;
        private bool homeGame;
        private int goalsFor;
        private int goalsAgainst;
        private int yellowCards;
        private int redCards;
        private string rivalTeamName;
        private string result;

        public Match(int gameId, bool homeGame, int goalsFor, int goalsAgainst, int yellowCards, int redCards, string rivalTeamName, string result)
        {
            this.GameId = gameId;
            this.HomeGame = homeGame;
            this.GoalsFor = goalsFor;
            this.GoalsAgainst = goalsAgainst;
            this.YellowCards = yellowCards;
            this.RedCards = redCards;
            this.RivalTeamName = rivalTeamName;
            this.Result = result;
        }
        public Match()
        {

        }
        public int GameId { get => GameId1; set => GameId1 = value; }
        public bool HomeGame { get => homeGame; set => homeGame = value; }
        public int GoalsFor { get => goalsFor; set => goalsFor = value; }
        public int GoalsAgainst { get => goalsAgainst; set => goalsAgainst = value; }
        public int YellowCards { get => yellowCards; set => yellowCards = value; }
        public int RedCards { get => redCards; set => redCards = value; }
        public string RivalTeamName { get => rivalTeamName; set => rivalTeamName = value; }
        public string Result { get => result; set => result = value; }
        public int GameId1 { get => gameId; set => gameId = value; }
    }
}