using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RobsonROX.Snake.Web.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RobsonROX.Snake.Web.Controllers
{
    public class LeaderboardsController : Controller
    {
        public static LeaderboardsContext Db => LeaderboardsContext.Db;

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Leaderboard> entries = Db.Leaderboards.OrderByDescending(l => l.Score).Take(10).ToList();
            return View(entries);
        }
    }
}
