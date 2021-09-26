using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Models
{


    public class Synonym
    {
        public Response[] response { get; set; }
    }

    public class Response
    {
        public List list { get; set; }
    }

    public class List
    {
        public string category { get; set; }
        public string synonyms { get; set; }
    }


}
