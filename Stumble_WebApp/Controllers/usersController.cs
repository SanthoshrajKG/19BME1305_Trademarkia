using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Stumble_WebApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public usersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("matches")]
        public JsonResult Get()
        {
            string query = @"
                        select users.name as 'person1', 'matches' , (select users.name from users where users.id = likes.who_is_liked) as 'person2' from users, likes where users.id = likes.who_likes and likes.who_likes in (select who_is_liked from likes)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("StumbleAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }

            return new JsonResult(table);
        }

       [HttpGet("{id}/{k}")]
        public JsonResult Kdistance(int id,int k)
        {
            string query = @"select * from users where users.id >= " + id.ToString() + " and " + " users.id < " + (id+k).ToString();

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("StumbleAppCon");
            MySqlDataReader myReader;
            using (MySqlConnection mycon = new MySqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (MySqlCommand myCommand = new MySqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }

    }
}
