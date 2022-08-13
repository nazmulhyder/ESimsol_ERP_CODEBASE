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
    #region LotBuyer
    public class LotBuyer :BusinessObject
    {
        public LotBuyer()
        {
            LotID = 0;
            LotNo = "";
            Balance = 0;
            LocationName = "";
            OperationUnitName = "";
            ProductCode = "";
            ProductName = "";
            MUName = "";
            ContractorID = 0;
            ContractorName = "";
            ErrorMessage = "";
        }
    
        #region Properties
        public int LotID {get; set;}
        public string LotNo { get; set; }
        public double Balance { get; set; }
        public string LocationName {get; set;}
        public string OperationUnitName {get; set;}
        public string ProductCode {get; set;}
        public string ProductName {get; set;}
        public string MUName {get; set;}
        public int ContractorID {get; set;}
        public string ContractorName {get; set;}
        public string ErrorMessage { get; set; }

        #endregion

        #region Functions


        public static List<LotBuyer> Gets(string sSQL, int ReportType, long nUserID)
        {
            return LotBuyer.Service.Gets(sSQL, ReportType, nUserID);
        }

       
        #endregion

        #region ServiceFactory
        internal static ILotBuyerService Service
        {
            get { return (ILotBuyerService)Services.Factory.CreateService(typeof(ILotBuyerService)); }
        }

        #endregion
    }
    #endregion

    #region ISalaryScheme interface
    public interface ILotBuyerService
    {
        List<LotBuyer> Gets(string sSQL, int ReportType, Int64 nUserID);
       

    }
    #endregion
}
