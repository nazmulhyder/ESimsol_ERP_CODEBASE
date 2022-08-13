using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class SampleInvoiceCharge
    {
        public SampleInvoiceCharge()
        {
            SampleInvoiceChargeID = 0;
            SampleInvoiceID = 0;
            InoutType = 0;
            Name = "";
            Amount = 0;
            LastUpdateBy = 0;
            LastUpdateDateTime = DateTime.Now;
            ErrorMessage = "";
            LastUpdateByName = "";

        }

        #region Property
        public int SampleInvoiceChargeID { get; set; }
        public int SampleInvoiceID { get; set; }
        public int InoutType { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int LastUpdateBy { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
        public string ErrorMessage { get; set; }
        public string LastUpdateByName { get; set; }

        #endregion

        #region Derived Property
        public string InoutTypeST
        {
            get
            {
                if (this.InoutType == 1)
                {
                    return "Add";
                }
                else
                {
                    return "Deduct";
                }
               
            }
        }
        public string AmountST
        {
            get
            {
                return this.Amount.ToString("#,##0.00;(#,##0.00)");
            }
        }
        #endregion

        #region Functions
        public static List<SampleInvoiceCharge> Gets(int nSampleInvoiceID,long nUserID)
        {
            return SampleInvoiceCharge.Service.Gets(nSampleInvoiceID,nUserID);
        }
        public static List<SampleInvoiceCharge> Gets(string sSQL, long nUserID)
        {
            return SampleInvoiceCharge.Service.Gets(sSQL, nUserID);
        }
        public SampleInvoiceCharge Get(int id, long nUserID)
        {
            return SampleInvoiceCharge.Service.Get(id, nUserID);
        }
        public SampleInvoiceCharge Save(long nUserID)
        {
            return SampleInvoiceCharge.Service.Save(this, nUserID);
        }
        public List<SampleInvoiceCharge> SaveList(List<SampleInvoiceCharge> oSampleInvoiceCharges, long nUserID)
        {
            return SampleInvoiceCharge.Service.SaveList(oSampleInvoiceCharges, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return SampleInvoiceCharge.Service.Delete(id, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static ISampleInvoiceChargeService Service
        {
            get { return (ISampleInvoiceChargeService)Services.Factory.CreateService(typeof(ISampleInvoiceChargeService)); }
        }
        #endregion
    }

    #region IFabricQCParName interface
    public interface ISampleInvoiceChargeService
    {
        SampleInvoiceCharge Get(int id, Int64 nUserID);
        List<SampleInvoiceCharge> Gets(int nSampleInvoiceID,Int64 nUserID);
        List<SampleInvoiceCharge> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        SampleInvoiceCharge Save(SampleInvoiceCharge oSampleInvoiceCharge, Int64 nUserID);
        List<SampleInvoiceCharge> SaveList(List<SampleInvoiceCharge> oSampleInvoiceCharges, Int64 nUserID);
    }
    #endregion
}
