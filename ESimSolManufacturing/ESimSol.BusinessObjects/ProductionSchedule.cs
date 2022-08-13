using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    
    #region ProductionSchedule
    
    public class ProductionSchedule : BusinessObject
    {


        #region  Constructor
        public ProductionSchedule()
        {

            ProductionScheduleID = 0;
            BatchGroup = 0;
            ScheduleStatus = EnumProductionScheduleStatus.Hold;
            ProductionScheduleNo = "";
            ScheduleStability = "";
            MachineID = 0;
            DBUserID = 0;
            BatchNo = EnumNumericOrder.First;
            LocationID = 0;
            ProductionQty = 0;
            ScheduleType = true;
            StartTime = DateTime.Now;
            EndTime = DateTime.Now;
            DBUserID = 0;
            ChangeGrid = 0;
            DBServerdateTime = DateTime.Now;
            ErrorMessage = "";
            MachineNo = "";
            MachineName = "";
            UsesWeight  ="";
            //UsesSpindleCount=0;
            //UsesLiquorCapacity =0;
            LocationName = "";
            MachineIDs = "";
            bIncreaseTime = false;
            SwapScheduleID = 0;
            CheckTime = new DateTime(1900, 1, 1, 0, 0, 0);
            IncDecTime = new DateTime(1900,1,1,0,0,0);
            IncDecMachineID = 0;
            bEffectIncDec = false;
            SwapIncDecTimeFirst = new DateTime(1900, 1, 1, 0, 0, 0);
            SwapIncDecTimeSecond = new DateTime(1900, 1, 1, 0, 0, 0);

        }
        #endregion

        #region Properties
                
        public int ProductionScheduleID { get; set; }
        
        public int BatchGroup {get; set;}

        public string ProductionScheduleNo { get; set; }
        
        public string ScheduleStability { get; set; }

        public int MachineID { get; set; }

        public EnumNumericOrder BatchNo { get; set; }
        
        public int LocationID { get; set; }
        public int BUID { get; set; }
        
        public double ProductionQty { get; set; }
        
        public bool ScheduleType { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        
        public int DBUserID { get; set; }
        
        public int ChangeGrid { get; set; }
        
        public DateTime DBServerdateTime { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public string MachineIDs { get; set; }
        
        public bool bIncreaseTime { get; set; }


        
        public EnumProductionScheduleStatus ScheduleStatus { get; set; }
        
        public string OrderDetail { get; set; }
        
        public int MaxValue { get; set; }
        
        public int PSID1 { get; set; }
        
        public int PSID2 { get; set; }
        
        public double ProductionQtyFirst { get; set; }
        
        public double ProductionQtySecond { get; set; }
        
        public string stableContents { get; set; }
        
        public string sDay { get; set; }
        
        public string sWeek { get; set; }
        
        public string sMonth { get; set; }
        
        public string sPrintView { get; set; }
        
        public string sDateSelection { get; set; }
        
        public string ProductionScheduleOf { get; set; }
        
        public int SwapScheduleID { get; set; }

        #region Property For Undo Redo for Time Changes
        
        public DateTime CheckTime { get; set; }
        
        public DateTime IncDecTime { get; set; }
        
        public bool bEffectIncDec { get; set; }
        
        public int IncDecMachineID { get; set; }
        
        public DateTime SwapIncDecTimeFirst { get; set; }
        
        public DateTime SwapIncDecTimeSecond { get; set; }

        public string CheckTimeInString { get { return this.CheckTime.ToString("dd MMM yyyy HH:mm"); } }
        public string IncDecTimeInString { get { return this.IncDecTime.ToString("dd MMM yyyy HH:mm"); } }
        #endregion

        #region Production 'Schedule Status' and 'Batch No' In String


        public string ScheduleStatusInString
        {
            get
            {
                return ScheduleStatus.ToString();
            }
        }

       

        #endregion


        #region Group By

        
        public string sProductionScheduleIds { get; set; }

        #endregion



        public string StartTimeInString
        {
            get
            {
                return StartTime.ToString("MM'/'dd'/'yyyy HH:mm");
            }
        }

        public string EndTimeInString
        {
            get
            {
                return EndTime.ToString("MM'/'dd'/'yyyy HH:mm");
            }
        }


        public string StartTimeInPerfect
        {
            get
            {
                return StartTime.ToString("dd' 'MMM' 'yyyy HH:mm");
            }
        }

        public string EndTimeInPerfect
        {
            get
            {
                return EndTime.ToString("dd' 'MMM' 'yyyy HH:mm");
            }
        }


        public string ScheduleTypeName
        {

            get
            {
                if (ScheduleType)
                {
                    return "Regular";
                }
                else
                {
                    return "Booking";
                }
            }


        }

        public int TimeDiffInHours
        {
            get {
                TimeSpan oTS = new TimeSpan();
                oTS = this.EndTime - this.StartTime;
                int nDay = oTS.Days;
                return oTS.Hours + (nDay > 0 ? (nDay*24) : 0);
            }
        }


        //public string BatchCardNo
        //{

        //    get
        //    {
        //        if (ProductionScheduleNo != "" && ProductionScheduleNo != null)
        //        {
        //            string sPSNumber = ProductionScheduleNo.Split('-')[1];
        //            string sYear = ProductionScheduleNo.Split('-')[2];
        //            sYear = sYear.Substring(2, 2);
        //            string sZero = "";
        //            if (sPSNumber.Length < 5)
        //            {
        //                for (int i = 0; i < (5 - sPSNumber.Length); i++)
        //                {
        //                    sZero = sZero + "0";
        //                }
        //            }
        //            sPSNumber = "B# " + sZero + sPSNumber + "-" + sYear;
        //            return sPSNumber;
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}

        #region Schedule Detail/ order wise / party wise /product wise

        
        public string sActionLink { get; set; }

        #endregion

        #region  number of job for Job No
        public int TotalJobCount { set; get; }
        public List<JobPerMonth> JobPerMonths { get; set; }

        #endregion

       

        #endregion
        #region derived Properties
        public string MachineNo { get; set; }

        public string MachineName { get; set; }

        public string UsesWeight { get; set; }
        public string BatchNoInString
        {
            get
            {
                return BatchNo.ToString();
            }
        }
        public string LocationName { get; set; }



        public Company Company { get; set; }

        public List<int> UniqueDyeMachineId { get; set; }

        public List<Location> LocationList { get; set; }

        public List<CapitalResource> CapitalResources { get; set; }

        public List<ProductionScheduleDetail> ProductionScheduleDetailList { get; set; }

        public List<ProductionScheduleDetail> ProductionScheduleDetails { get; set; }

        public List<ProductionSchedule> ProductionScheduleList { get; set; }

        public string MachineNoWithCapacity { get { return this.MachineName + "(" + this.UsesWeight + ")"; } }

        #endregion

        #region Functions

        public ProductionSchedule Get(int nId, long nUserID)
        {
            return ProductionSchedule.Service.Get(nId,nUserID);
        }
        public ProductionSchedule Save(long nUserID)
        {
            return ProductionSchedule.Service.Save(this, nUserID);
        }

        public String Update(int nId1, int nId2, double ProductionQtyFirst, double ProductionQtySecond, long nUserID)
        {
            return ProductionSchedule.Service.Update(nId1, nId2, ProductionQtyFirst, ProductionQtySecond, nUserID);
        }

        public static int GetsMax(string sSql, long nUserID)
        {
            return ProductionSchedule.Service.GetsMax(sSql, nUserID);
        }

        public string Delete(long nUserID)
        {
            return ProductionSchedule.Service.Delete(this, nUserID);
        }

        public static List<ProductionSchedule> Gets(string sSql, long nUserID)
        {
            return ProductionSchedule.Service.Gets(sSql, nUserID);
        }

        public static List<ProductionSchedule> Gets(long nUserID)
        {
            return ProductionSchedule.Service.Gets(nUserID);
        }

        public static List<ProductionSchedule> Gets(DateTime dStartDate, DateTime dEndDate, string sLocationIDs,long nUserID)
        {
            return ProductionSchedule.Service.Gets(dStartDate, dEndDate, sLocationIDs, nUserID);
        }

        public static DataSet GetsGroupBy(string sSql, long nUserID)
        {
            return ProductionSchedule.Service.GetsGroupBy(sSql, nUserID);
        }

        public Double GetWaitingProductionQuantity(string sSql, long nUserID)
        {
            return ProductionSchedule.Service.GetWaitingProductionQuantity(sSql, nUserID);
        }

        public int GetUnpublishProductionSchedule(string sSql, long nUserID)
        {
            return ProductionSchedule.Service.GetUnpublishProductionSchedule(sSql, nUserID);
        }


        #endregion

        #region NonDB Functions

        public static string IDInString(List<ProductionSchedule> oProductionSchedules)
        {
           
            string sReturn = "";
            foreach (ProductionSchedule oItem in oProductionSchedules)
            {
                sReturn = sReturn + oItem.ProductionScheduleID.ToString() + ",";

            }
            if (sReturn == "") return "";
            sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;

        }

        #endregion

        #region ServiceFactory
        internal static IProductionScheduleService Service
        {
            get { return (IProductionScheduleService)Services.Factory.CreateService(typeof(IProductionScheduleService)); }
        }
        #endregion
    }


    #region IProductionSchedule interface
    public interface IProductionScheduleService
    {
        
        ProductionSchedule Get(int nID, long nUserID);

        
        List<ProductionSchedule> Gets(long nUserID);

        
        List<ProductionSchedule> Gets(DateTime dStartDate, DateTime dEndDate, string sLocationIDs,long nUserID);

        
        int GetsMax(string sSql, long nUserID);

        
        List<ProductionSchedule> Gets(string sSql, long nUserID);

        
        string Delete(ProductionSchedule oProductionSchedule, long nUserID);

        
        ProductionSchedule Save(ProductionSchedule oProductionSchedule, long nUserID);

        
        String Update(int nId1, int nId2, double ProductionQtyFirst, double ProductionQtySecond, long nUserID);

        
        DataSet GetsGroupBy(string sSql, long nUserID);

        
        Double GetWaitingProductionQuantity(string sSql, long nUserID);

        
        int GetUnpublishProductionSchedule(string sSql, long nUserID);





    }        
    #endregion


    #region Class for Jobs per Month
    public class JobPerMonth
    {
        public JobPerMonth()
        {
            MonthOfYear = "";
            JobCount = 0;
        }

        public string MonthOfYear { get; set; }
        public int JobCount { get; set; }
    }
    #endregion
     #endregion

}
