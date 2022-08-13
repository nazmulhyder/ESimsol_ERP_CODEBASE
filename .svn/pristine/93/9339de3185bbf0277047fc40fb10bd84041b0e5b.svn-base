using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region YarnChallanRegister
    public class YarnChallanRegister : BusinessObject
    {
        public YarnChallanRegister()
        {
            KnittingYarnChallanDetailID =  0;
            KnittingYarnChallanID =  0;
            KnittingOrderDetailID =  0;
            BUID = 0;
            ChallanNo =  "";
            ChallanDate =  DateTime.Now;
            BuyerID =  0;
            BuyerName =  "";
            FactoryID =  0;
            FactoryName =  "";
            ProductID =  0;
            ProductName =  "";
            ProductCode =  "";
            ColorID =  0;
            ColorName =  "";
            Brand =  "";
            BrandShortName =  "";
            LotID =  0;
            LotNo =  "";
            StoreID =  0;
            StoreName =  "";
            MUnitID =  0;
            MUShortName = "";
            Qty =0;
            BagQty =0;
            StyleID = 0;
            StyleNo = "";
            KnittingOrderNo ="";
            PAM = "";
            ReportLayout = EnumReportLayout.None;
            ErrorMessage = "";
        }

        #region Property
        public int KnittingYarnChallanDetailID { get; set; }
        public int KnittingYarnChallanID { get; set; }
        public int KnittingOrderDetailID { get; set; }
        public int BUID { get; set; }
        public string ChallanNo { get; set; }
        public DateTime ChallanDate { get; set; }
        public int StyleID { get; set; }
        public string KnittingOrderNo {get;set;}
        public string PAM { get; set; }
        public string StyleNo { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }
        public int FactoryID { get; set; }
        public string FactoryName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int ColorID { get; set; }
        public string ColorName { get; set; }
        public string Brand { get; set; }
        public string BrandShortName { get; set; }
        public int LotID { get; set; }
        public string LotNo { get; set; }
        public int StoreID { get; set; }

        public string StoreName { get; set; }
        public int MUnitID { get; set; }
        public string MUShortName { get; set; }
        public double Qty { get; set; }
        public double BagQty { get; set; }
        

        public string ErrorMessage { get; set; }
        public EnumReportLayout ReportLayout { get; set; }
        #endregion

        #region Derived Property
        
        public string ChallanDateInString
        {
            get
            {
                if (ChallanDate == DateTime.MinValue) return "";
                return ChallanDate.ToString("dd MMM yyyy");
            }
        }
        
        #endregion

        #region Functions
        public static List<YarnChallanRegister> Gets(string sSQL, long nUserID)
        {
            return YarnChallanRegister.Service.Gets(sSQL, nUserID);
        }
        
        #endregion

        #region ServiceFactory
        internal static IYarnChallanRegisterService Service
        {
            get { return (IYarnChallanRegisterService)Services.Factory.CreateService(typeof(IYarnChallanRegisterService)); }
        }
        #endregion

        public List<YarnChallanRegister> YarnChallanRegisters { get; set; }
    }
    #endregion

    #region IYarnChallanRegister interface
    public interface IYarnChallanRegisterService
    {
        List<YarnChallanRegister> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
