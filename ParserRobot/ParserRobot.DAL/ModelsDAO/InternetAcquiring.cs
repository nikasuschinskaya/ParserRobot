using System;

namespace ParserRobot.DAL.ModelsDAO
{
    public class InternetAcquiring
    {
        public string FullName { get; set; }
        public string PayerAccountNumber { get; set; }
        public DateTime CreationDate { get; set; } 
        public decimal AmountPerDay { get; set; } = 0;
        public int CountPerDay { get; set; } = 0;
        public decimal AmountPerMonth { get; set; } = 0;
        public int CountPerMonth { get; set; } = 0;
    }
}
