namespace RobsonROX.Snake.Web.Controllers.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using RobsonROX.Snake.Web.Models;

    [Route("api/[controller]")]
    public class LeaderboardsController : Controller
    {
        public static LeaderboardsContext Db => LeaderboardsContext.Db;

        // GET: api/leaderboards
        [HttpGet]
        public IEnumerable<Leaderboard> Get()
        {
            return Db.Leaderboards.OrderByDescending(l => l.Score).Take(10);
        }

        //GET api/leaderboards/player
        [HttpGet("{player}")]
        public async Task<IActionResult> Get(string player)
        {
            Leaderboard entry = await Db.Leaderboards.FindAsync(player);
            return entry != null ? Json(entry) : (IActionResult)NotFound();
        }

        // POST api/leaderboards/player
        [HttpPost("{player}")]
        public async Task<IActionResult> Post(string player, string score)
        {
            Leaderboard entry = await Db.Leaderboards.FindAsync(player);
            if(entry != null)
            {
                entry.Score = Math.Max(entry.Score, int.Parse(score));
                entry.Date = DateTime.Now;
            }
            else
            {
                entry = new Leaderboard
                {
                    Score = int.Parse(score),
                    Player = player,
                    Date = DateTime.Now
                };
                Db.Leaderboards.Add(entry);
            }
            await Db.SaveChangesAsync();
            return entry != null ? Json(entry) : (IActionResult)NotFound();
        }

        // PUT api/leaderboards/player
        [HttpPut("{player}")]
        public async Task<IActionResult> Put(string player, int score)
        {
            Leaderboard entry = await Db.Leaderboards.FindAsync(player);
            if (entry != null)
            {
                entry.Score = Math.Max(entry.Score, score);
                await Db.SaveChangesAsync();
            }
            return entry != null ? Ok() : (IActionResult)NotFound();
        }

        // DELETE api/leaderboards/player
        [HttpDelete("{player}")]
        public IActionResult Delete(string player)
        {
            return Forbid();
        }
    }
}
