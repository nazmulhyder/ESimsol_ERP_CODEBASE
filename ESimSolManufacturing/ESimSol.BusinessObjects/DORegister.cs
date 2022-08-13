using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DORegister
    public class DORegister : BusinessObject
    {
        public DORegister()
        {
            BUID = 0;
            ExportPIID = 0;
            BuyerID = 0;
            PINo ="";
            PIDate = DateTime.MinValue;
            CustomerName = "";
            BuyerName = "";
            LCFileNo="";
            ProductName = "";
            ProductCategoryName = "";
            ColorName="";
            StyleNo="";
            PIQty=0.0;
            Qty = 0.0;
            DOQty=0.0;
            YetToDO	=0.0;
            ChallanQty = 0.0;
            YetToChallan = 0.0;
            ReportLayout = 0;
            ErrorMessage = "";
        }

        #region Properties
        public string PINo { get; set; }
        public string BuyerName { get; set; }
        public string ProductName { get; set; }
        public string ProductCategoryName { get; set; }
        public string MUnit { get; set; }
        public string StyleNo { get; set; }
        public int BUID { get; set; }
        public DateTime PIDate { get; set; }
        public int ExportPIID { get; set; }
        public int BuyerID { get; set; }
        public string CustomerName { get; set; }
        public string LCFileNo { get; set; }
        public string ColorName { get; set; }
        public double PIQty { get; set; }
        public double Qty { get; set; }
        public double DOQty { get; set; }
        public double YetToDO { get; set; }
        public double ChallanQty { get; set; }
        public double YetToChallan { get; set; }
        public string ErrorMessage { get; set; }
        public int ReportLayout { get; set; }
        public string PIDateSt
        {
            get
            {
                return DateObject.GetDate(this.PIDate);
            }
        }
      
        #endregion

        #region Functions
        public static List<DORegister> Gets(string sSql, int nReprotLayout, int nUserID)
        {
            return DORegister.Service.Gets(sSql, nReprotLayout, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IDORegisterService Service
        {
            get { return (IDORegisterService)Services.Factory.CreateService(typeof(IDORegisterService)); }
        }
        #endregion


    }
    #endregion

    #region IDORegister interface
    public interface IDORegisterService
    {
        List<DORegister> Gets(string sSql, int nReprotLayout, Int64 nUserID);
    }
    #endregion
}
