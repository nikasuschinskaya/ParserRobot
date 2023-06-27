using System;

namespace ParserRobot.DAL.ModelsDAO
{
    public class MerchantAcquiring
    {
        public Guid Id { get; set; }
        public int DayLimit { get; set; }
        public decimal AmountPerDay { get; set; }
        public int MonthLimit { get; set; }
        public decimal AmountPerMonth { get; set; }
        public string Adress { get; set; }
        public DateTime InstallationDate { get; set; }
        public string License { get; set; }
        public DateTime LicenseExpirationDate { get; set; }
    }
}
