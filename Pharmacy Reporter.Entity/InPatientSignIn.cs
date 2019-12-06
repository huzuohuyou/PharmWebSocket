namespace Pharmacy_Reporter.Entity
{
    /// <summary>
    /// 住院患者发药登记
    /// </summary>
    public class InPatientSignIn
    {
        public string PTNO { get; set; }
        public string WARDCODE { get; set; }
        public string DEPTCODE { get; set; }
        public string DRCODE { get; set; }
    }
}
