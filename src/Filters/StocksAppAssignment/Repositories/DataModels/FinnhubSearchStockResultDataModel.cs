using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.DataModels
{
    public class FinnhubSearchStockResultDataModel
    {

        public class Rootobject
        {
            public int count { get; set; }
            public Result[] result { get; set; }
        }

        public class Result
        {
            public string description { get; set; }
            public string displaySymbol { get; set; }
            public string symbol { get; set; }
            public string type { get; set; }
            public string[] primary { get; set; }
        }

    }
}
