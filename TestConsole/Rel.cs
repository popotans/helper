using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestConsole
{
    class Rel
    {

    }

    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }


    public class Test
    {
        public int Idx { get; set; }
        public string Title { get; set; }
        public DateTime Das { get; set; }
        public string contEnt { get; set; }

    }

    public class Channel
    {
        public int Idx { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int ShowinNav { get; set; }
        public int OrderIndex { get; set; }
    }

    public class Company
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public string jyfw { get; set; }
        public string Contact { get; set; }
    }

    public class VirtualRole
    {
        public int IDx { get; set; }
        public string ProcCode { get; set; }
        public string RoleGroupCode { get; set; }
        public string SpecialclassName { get; set; }
        public string SpecialPersonName { get; set; }
        public string Type { get; set; }

    }



    public partial class Order
    {
        public System.Int32 OrderId { get; set; }
        public System.DateTime OrderDate { get; set; }
        public System.Decimal SumMoney { get; set; }
        public System.String Comment { get; set; }
        public System.Int32 Finished { get; set; }
        public System.Int32 Customerid { get; set; }
    }



    public partial class Address
    {
        public System.Int32 AddressID { get; set; }
        public System.String AddressLine1 { get; set; }
        public System.String AddressLine2 { get; set; }
        public System.String City { get; set; }
        public System.Int32 StateProvinceID { get; set; }
        public System.String PostalCode { get; set; }
        public System.Guid rowguid { get; set; }
        public System.DateTime ModifiedDate { get; set; }
    }
}
