using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreServiceContracts.Hub.Sale
{
    public class SaleDO
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        public SaleDO()
        { }

        public SaleDO(DataRow dr)
        {
            Id = dr.Field<int>("id");
            Region = dr.Field<string>("region");
            Country = dr.Field<string>("country");
            City = dr.Field<string>("city");
            Amount = dr.Field<double>("amount");
            Date = dr.Field<DateTime>("date");

        }
    }
}
