using System;
using System.ComponentModel.DataAnnotations;

namespace RobsonROX.Snake.Web.Models
{
    public class Leaderboard
    {
        [Key]
        public string Player { get; set; }
        public int Score { get; set; }
        public DateTime Date { get; set; }
    }
}