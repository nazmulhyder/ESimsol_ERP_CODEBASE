using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region PaymentPI
    public class PaymentPI : BusinessObject
    {
        public PaymentPI()
        {
            PaymentPIID = 0;
            IsImport = false;
            PaymentID = 0;
            PIID = 0;
            Amount = 0;
            Received = 0;
            PINo = "";
            ExportPIDate = DateTime.Today;
            ExportPIAmount = 0;
            ExportPIs = new List<ExportPI>();
        }

        #region Properties
        public int PaymentPIID { get; set; }
        public bool IsImport { get; set; }
        public int PaymentID { get; set; }
        public int PIID { get; set; }
        public double Amount { get; set; }
        public double Received { get; set; }
        public string PINo { get; set; }
        public DateTime ExportPIDate { get; set; }
        public double ExportPIAmount { get; set; }
        #endregion

        #region
        public List<ExportPI> ExportPIs { get; set; }
        public string ExportPIDateSt
        {
            get
            {
                return this.ExportPIDate.ToString("dd MMM yyyy");
            }
        }

        public double Remaining
        {
            get
            {
                double nRemaining = this.ExportPIAmount - this.Received;
                return System.Math.Round(nRemaining, 2);
            }
        }
        #endregion

        #region Functions
        public static List<PaymentPI> Gets(long nUserID)
        {
            return PaymentPI.Service.Gets(nUserID);
        }
        public static List<PaymentPI> Gets(string sSQL, long nUserID)
        {
            return PaymentPI.Service.Gets(sSQL, nUserID);
        }
        public static List<PaymentPI> GetsByPaymentID(int nPaymentID, long nUserID)
        {
            return PaymentPI.Service.GetsByPaymentID(nPaymentID, nUserID);
        }
        public PaymentPI Get(int nId, long nUserID)
        {
            return PaymentPI.Service.Get(nId, nUserID);
        }
        public PaymentPI Save(long nUserID)
        {
            return PaymentPI.Service.Save(this, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return PaymentPI.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IPaymentPIService Service
        {
            get { return (IPaymentPIService)Services.Factory.CreateService(typeof(IPaymentPIService)); }
        }
        #endregion
    }
    #endregion

    #region IPaymentPI interface
    public interface IPaymentPIService
    {
        PaymentPI Get(int id, long nUserID);
        List<PaymentPI> Gets(long nUserID); //
        List<PaymentPI> Gets(string sSQL, long nUserID);
        List<PaymentPI> GetsByPaymentID(int nPaymentID, long nUserID);
        string Delete(int id, long nUserID);
        PaymentPI Save(PaymentPI oPaymentPI, long nUserID);
    }
    #endregion
}
