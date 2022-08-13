using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects.ReportingObject
{
    public class FabricFSStatus
    {

        public FabricFSStatus()
        {
            SCNo = "";
            BuyerID = 0;
            SCDate = DateTime.MinValue;
            ApproveDate = DateTime.MinValue;
            MktAccountID = 0;
            OrderType = 0;
            BuyerName = "";
            MktPersonName = "";
            ReviseNo = 0;
            FabricSalesContractID = 0;
            NoofDispo = 0;
            Qty = 0;
            AppDCDate = DateTime.MinValue;
            QtyDC = 0;
            LastDCDate = DateTime.MinValue;
            ErrorMessage = "";
            OrderName = "";
            Remarks = "";
            CurrentStatus = 0;
            isPrint = false;
        }


        #region Properties
        public string SCNo { get; set; }
        public int BuyerID { get; set; }
        public DateTime SCDate { get; set; }
        public DateTime ApproveDate { get; set; }
        public int MktAccountID { get; set; }
        public int OrderType { get; set; }
        public string BuyerName { get; set; }
        public string OrderName { get; set; }
        public string MktPersonName { get; set; }
        public int ReviseNo { get; set; }
        public int FabricSalesContractID { get; set; }
        public int NoofDispo { get; set; }
        public double Qty { get; set; }
        public DateTime AppDCDate { get; set; }
        public double QtyDC { get; set; }
        public DateTime LastDCDate { get; set; }      
        public string ErrorMessage { get; set; }
        public int CurrentStatus { get; set; }
        public string Remarks { get; set; }
        public bool isPrint { get; set; }
        #endregion

        #region Derived Proeprty
        public string SCDateInString
        {
            get
            {
                if (this.SCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.SCDate.ToString("dd MMM yyyy");
                }              
            }
        }
        public string ApproveDateInString
        {
            get
            {
                {
                    if (this.ApproveDate == DateTime.MinValue)
                    {
                        return "";
                    }
                    else
                    {
                        return this.ApproveDate.ToString("dd MMM yyyy");
                    }

                }              
            }
        }
        public string AppDCDateInString
        {
            get
            {
                if (this.AppDCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.AppDCDate.ToString("dd MMM yyyy");
                }
               
            }
        }
        public string LastDCDateInString
        {
            get
            {
                if (this.LastDCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.LastDCDate.ToString("dd MMM yyyy");
                }
                
            }
        }
        public string TimeSpan
        {
            get
            {
                if (ApproveDate == DateTime.MinValue || AppDCDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    int Diff = (int)(AppDCDate - ApproveDate).TotalDays;
                    return Diff.ToString();
                }
                
            }
        }

        public string RemarksST
        {
            get
            {
                if (this.isPrint == true) return "Printed";
                else return "";
            }
        }

        #endregion
        #region Functions
        public static List<FabricFSStatus> GetsFabricFSStatus(string SQL, int nReportType, long nUserID)
        {
            return FabricFSStatus.Service.GetsFabricFSStatus(SQL,nReportType,nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricFSStatusService Service
        {
            get { return (IFabricFSStatusService)Services.Factory.CreateService(typeof(IFabricFSStatusService)); }
        }

        #endregion
    }
    #region IFabricFSStatus interface

    public interface IFabricFSStatusService
    {
        List<FabricFSStatus> GetsFabricFSStatus(string SQL, int nReportType, long nUserID);
    }
    #endregion
}
