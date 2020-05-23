using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumberFactsApi.Models.DB
{
    public class UserDB
    {
        public string Id { get; set; }

        public List<int> FavoriteNumbers = new List<int>(0);
    }
}
