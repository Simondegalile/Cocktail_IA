using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_API.Service
{
  
        public class Cocktail
        {
            public List<string> ingredients { get; set; }
            public string instructions { get; set; }
            public string name { get; set; }
        }
        public class ListCocktails
        {
            public List<Cocktail> cocktails { get; set; }
        }

    public class Userdata
    {
        public string Drink { get; set; }
        public string Rating { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }


    }


}
