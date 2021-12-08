using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using TodoApi.Models;
using TodoApi.Repository;
using TodoApi.Interface;
using TodoApi.Utility;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ITeam _service;
        private readonly IUser _serviceUser;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITeam service, IUser serviceUser)
        {
            _logger = logger;
            _service = service;
            _serviceUser = serviceUser;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //string constr = "Data Source=.;Initial Catalog=DemoApp;";
        string constr = "Data Source=AT-2021\\SQLEXPRESS;Initial Catalog=DemoApp;Trusted_Connection=True";
        // GET: api/Teacher
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDetails>>> Index()
        {
            List<TeamDetails> teams = new List<TeamDetails>();
            string query = "select * from Team";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            teams.Add(new TeamDetails
                            {
                                TeamId = Convert.ToInt32(sdr["TeamId"]),
                                TeamName = Convert.ToString(sdr["TeamName"]),
                            });
                        }
                    }
                    con.Close();
                }
            }

            return teams;
        }

        //[InvalidToken]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDetails>>> GetAllTeams()
        {
            string Token = Request.Headers["request-key"];

            return Ok(Token);
        }

        [HttpPost]
        [InvalidToken]
        public IActionResult AddUser([FromBody] User aUser)
        {
            Guid id = Guid.NewGuid();
            string myString = id.ToString().Replace("-", string.Empty);

            string Token = Request.Headers["request-key"];

            string dbName = "SaasDb" + myString;

            if (aUser.userType == "Paid")
            {
                _service.AddUser(dbName, aUser);
                var dbCreate = _service.CreateDb(dbName, aUser);
            }
            else
            {
                _service.AddUser("", aUser);
            }

            return Ok(aUser);
        }

        [HttpPost]
        [InvalidToken]
        public IActionResult ChangePayType(User aUser)
        {
            _service.ChangePayType(aUser);
            return Ok("Done!");
        }

        [HttpGet]
        [InvalidToken]
        public IActionResult GetUsers()
        {
            var users = _serviceUser.GetUsers();
            return Ok(users);
        }
    }
}
