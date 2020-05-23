using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NumberFactsApi.Models
{
    public class Commands
    {
        public string FindTriviaFact(UserInfo item)
        {
            item.link = $"http://numbersapi.com/{item.Number}/trivia";
            return item.link;
        }
        public string FindMathFact(UserInfo item)
        {
            item.link = $"http://numbersapi.com/{item.Number}/math";
            return item.link;
        }
        public string FindDateFact(UserInfo item)
        {
            item.link = $"http://numbersapi.com/{item.Month}/{item.Day}/date";
            return item.link;
        }
    }
}
