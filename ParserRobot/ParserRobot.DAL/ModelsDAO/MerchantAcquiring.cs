using System;

namespace ParserRobot.DAL.ModelsDAO
{
    public class MerchantAcquiring
    {
        public Guid Id { get; set; }
        public int DayLimit { get; set; } = 0;
        public decimal AmountPerDay { get; set; } = 0;
        public int MonthLimit { get; set; } = 0;
        public decimal AmountPerMonth { get; set; } = 0;
        public string Adress { get; set; } = string.Empty;
        public DateTime? InstallationDate { get; set; } = null;
        public string License { get; set; } = string.Empty;
        public DateTime? LicenseExpirationDate { get; set; } = null;
    }
}
