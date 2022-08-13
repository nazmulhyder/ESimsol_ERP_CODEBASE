using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class RptGreyFabricStock
    {
        public RptGreyFabricStock()
        {
            DispoNo = "";
            CustomerName = "";
            isYD = false;
            Grade = "";
            OpeningQty = 0;
            QtyIn = 0;
            QtyOut = 0;
            ClosingQty = 0;
            ErrorMessage = "";
            WorkingUnitID = 0;
            LotID = 0;
            SCNo = "";
            FabricNo = "";
            ReviseNo = 0;
            OrderType = 0;
            Construction = "";
            ProcessTypeName = "";
            ColorInfo = "";
            ProcessType = 0;
            StyleNo = "";

        }

        #region Properties
        public int WorkingUnitID { get; set; }
        public int LotID { get;set; }
        public string DispoNo { get; set; }
        public string CustomerName { get; set; }
        public bool isYD { get; set; }
        public string Grade { get; set; }
        public double OpeningQty { get; set; }
        public double QtyIn { get; set; }
        public double QtyOut { get; set; }
        public double ClosingQty { get; set; }

        public string SCNo { get; set; }
        public string FabricNo { get; set; }
        public int ReviseNo { get; set; }
        public int OrderType { get; set; }
        public string Construction { get; set; }
        public int ProcessType { get; set; }
        public string ProcessTypeName { get; set; }
        public string ColorInfo { get; set; }
        public string StyleNo { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived Property
        public string IsYDST
        {
            get
            {
                if (this.isYD == true)
                {
                    this.ErrorMessage = "YD";
                    return this.ErrorMessage;
                }
                else
                {
                    this.ErrorMessage = "SD";
                    return this.ErrorMessage;
                }
            }
        }
        public string ProcessTypeST
        {
            get
            {
                if (this.ProcessType == 0)
                {
                    return "Warping";
                }
                if (this.ProcessType == 1)
                {
                    return "Sizing";
                }
                return "";
            }
        }
        public string OrderTypeSt { get; set; }
        #endregion
        #region Functions
        public static List<RptGreyFabricStock> Gets(string sSQL, DateTime StartTime, DateTime EndTime,int ReportType,int StoreID, long nUserID)
        {
            return RptGreyFabricStock.Service.Gets(sSQL,StartTime, EndTime,ReportType,StoreID, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IRptGreyFabricStockService Service
        {
            get { return (IRptGreyFabricStockService)Services.Factory.CreateService(typeof(IRptGreyFabricStockService)); }
        }

        #endregion

    }

    #region IRptGreyFabricStock interface
    public interface IRptGreyFabricStockService
    {
        List<RptGreyFabricStock> Gets(string sSQL, DateTime StartTime, DateTime EndTime,int ReportType,int StoreID, long nUserID);

    }
    #endregion
}
