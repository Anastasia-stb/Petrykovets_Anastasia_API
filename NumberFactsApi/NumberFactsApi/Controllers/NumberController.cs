using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NumberFactsApi.Models;
using System.Net;
using System.IO;
using NumberFactsApi.Models.DB;
using Newtonsoft;
using Newtonsoft.Json;
using System.Net.Http;

namespace NumberFactsApi.Controllers
{
    [Route("api/[controller]")]
    public class NumberController : Controller
    {

        Commands commands = new Commands();

        [HttpPost]
        [Route("trivia")]
        public async Task<ObjectResult> FindTrivia([FromBody] UserInfo item)
        {
            string item1 = commands.FindTriviaFact(item);

            using var client = new HttpClient();
            var content = await client.GetStringAsync(item1);

            UserItem useritem = new UserItem();
            useritem.Fact = (content);
            useritem.Id = item.Id;

            return new ObjectResult(useritem);
        }


        [HttpPost]
        [Route("math")]
        public async Task<ObjectResult> FindMath([FromBody] UserInfo item)
        {
            string item1 = commands.FindMathFact(item);

            using var client = new HttpClient();
            var content = await client.GetStringAsync(item1);

            UserItem useritem = new UserItem();
            useritem.Fact = (content);
            useritem.Id = item.Id;

            return new ObjectResult(useritem);
        }

        [HttpPost]
        [Route("date")]
        public async Task<ObjectResult> FindDate([FromBody] UserInfo item)
        {
            string item1 = commands.FindDateFact(item);

            using var client = new HttpClient();
            var content = await client.GetStringAsync(item1);

            UserItem useritem = new UserItem();
            useritem.Fact = (content);
            useritem.Id = item.Id;

            return new ObjectResult(useritem);
        }
        [HttpPost]
        [Route("AddFavorite")]
        public async Task<ObjectResult> AddFavorite([FromBody] UserInfo item)
        {



            UserDB CurrentUser = new UserDB { Id = item.Id };
            UserListDB CurrentUserListDB = JsonConvert.DeserializeObject<UserListDB>(System.IO.File.ReadAllText(@"JSDB.json"));

            bool ContainsUser = false;
            int CurrentUserIndex = 0;

            for (int i = 0; i < CurrentUserListDB.AllUsersDB.Count; i++)
            {
                if (CurrentUserListDB.AllUsersDB[i].Id==CurrentUser.Id)
                {
                    ContainsUser = true;
                    CurrentUserIndex = i;
                    break;
                }
            }

            if (ContainsUser == true)
            {
                if(CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers.Contains(item.Number))
                {
                    return (new ObjectResult("Number already added!"));
                }
                else
                {
                    CurrentUser.FavoriteNumbers.Add(item.Number);
                }
            }
            else
            {
                CurrentUser.FavoriteNumbers.Add(item.Number);
            }


            if (ContainsUser == false)
            {
                CurrentUserListDB.AllUsersDB.Add(CurrentUser);
                await System.IO.File.WriteAllTextAsync(@"JSDB.json", JsonConvert.SerializeObject(CurrentUserListDB));
                return (new ObjectResult("Ok"));
            }
            else
            {
                CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers.Add(item.Number);
                await System.IO.File.WriteAllTextAsync(@"JSDB.json", JsonConvert.SerializeObject(CurrentUserListDB));
                return (new ObjectResult("Ok"));
            }
        }

        [HttpPost]
        [Route("GetFavorite")]
        public async Task<ObjectResult> GetFavorite([FromBody] UserInfo item)
        {

            UserListDB CurrentUserListDB = JsonConvert.DeserializeObject<UserListDB>(await System.IO.File.ReadAllTextAsync(@"JSDB.json"));

            bool ContainsUser = false;
            int CurrentUserIndex = 0;

            for (int i = 0; i < CurrentUserListDB.AllUsersDB.Count; i++)
            {
                if (CurrentUserListDB.AllUsersDB[i].Id == item.Id)
                {
                    ContainsUser = true;
                    CurrentUserIndex = i;
                    break;
                }
            }

            if (ContainsUser == false)
            {
                return (new ObjectResult("User not found!"));
            }
            else
            {

                if (CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers != null)
                {
                    List<int> UsersNumbers = new List<int>();
                    for (int i = 0; i < CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers.Count; i++)
                    {
                        UsersNumbers.Add(CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers[i]);
                    }
                    return (new ObjectResult(UsersNumbers));
                }
                else
                {
                    return (new ObjectResult("Number not found!"));
                }
            }
        }

        [HttpPut]
        [Route("DeleteFavorite")]
        public async Task<ObjectResult> DeleteFavorite([FromBody] UserInfo item)
        {

            UserListDB CurrentUserListDB = JsonConvert.DeserializeObject<UserListDB>(await System.IO.File.ReadAllTextAsync(@"JSDB.json"));

            bool ContainsUser = false;
            int CurrentUserIndex = 0;

            for (int i = 0; i < CurrentUserListDB.AllUsersDB.Count; i++)
            {
                if (CurrentUserListDB.AllUsersDB[i].Id == item.Id)
                {
                    ContainsUser = true;
                    CurrentUserIndex = i;
                    break;
                }
            }

            if (ContainsUser == false)
            {
                return (new ObjectResult("User not found!"));
            }
            else
            {

                if (CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers.Contains(item.Number))
                {
                    CurrentUserListDB.AllUsersDB[CurrentUserIndex].FavoriteNumbers.Remove(item.Number);
                    await System.IO.File.WriteAllTextAsync(@"JSDB.json", JsonConvert.SerializeObject(CurrentUserListDB));
                    return (new ObjectResult("Ok"));
                }
                else
                {
                    return (new ObjectResult("Number not found!"));
                }
            }
        }

    }
}
