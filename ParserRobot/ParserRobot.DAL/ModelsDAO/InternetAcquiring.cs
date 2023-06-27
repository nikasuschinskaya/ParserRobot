using System;

namespace ParserRobot.DAL.ModelsDAO
{
    public class InternetAcquiring
    {
        public string FullName { get; set; }
        public string PayerAccountNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal AmountPerDay { get; set; }
        public int CountPerDay { get; set; }
        public decimal AmountPerMonth { get; set; }
        public int CountPerMonth { get; set; }
    }
}
