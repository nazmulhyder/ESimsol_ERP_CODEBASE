using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ESimSol.BusinessObjects;
using System.Drawing;
using System.Runtime.Serialization;
using System.ServiceModel;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region ProductionScheduleDetail
    
    public class ProductionScheduleDetail : BusinessObject
    {

        #region  Constructor
        public ProductionScheduleDetail()
        {

            ProductionScheduleDetailID = 0;
            ProductionScheduleID = 0;
            ProductionTracingUnitID = 0;
            ProductionQty = 0;
            Remarks = "";
            DBUserID = 0;
            ErrorMessage = "";
            OrderNo = "";
            ProductName="";
            BuyerName="";
            ColorName="";
            YetToProductionQty = 0;
            PSBatchNo = "";
            RSState = EnumRSState.Initialized;
            UsesWeight = 0;
            RouteSheetID = 0;
            PRRemarks = "";
            Loading = 0;
            RedyingForRSNo = "";
            CombineRSNo = "";
            CRSID = 0;
            DyeChemicalCost = 0;
            DODID = 0;
            DyeLoadTime = DateTime.MinValue;
            DyeUnLoadTime = DateTime.MinValue;
            RouteSheetQty = 0;
            ExpDeliveryDateByFactory = DateTime.MinValue;
            LabDipDetailID = 0;
        }
        #endregion

        #region Properties
        public int ProductionScheduleDetailID { get; set; }
        public int ProductionScheduleID { get; set; }
        public int ProductionTracingUnitID { get; set; }
        public int DODID { get; set; }
        public int DUClaimOrderDetailID { get; set; }
        public double ProductionQty { get; set; }
        public string PSBatchNo { get; set; }
        
        public string Remarks { get; set; }
        
        public int DBUserID { get; set; }
        
        public string ErrorMessage { get; set; }


        #region derived Properties
        public string LocationName { get; set; }
        
        public string MachineName { get; set; }
        
        public string MachineNo { get; set; }
        
        public DateTime StartTime { get; set; }
        
        public DateTime EndTime { get; set; }
        public int OrderType { get; set; }
        public int ProductID { get; set; }
        public int LabDipDetailID { get; set; }
        
        public string FactoryName { get; set; }

        public string ColorNo { get; set; }
        public string OrderNo { get; set; }
        
        public string ProductName { get; set; }
        
        public string BuyerName { get; set; }
        
        public string ColorName { get; set; }
        
        public double YetToProductionQty { get; set; }

        
        public double WaitingForProductionQty { get; set; }
        
        public double TotalScheduledQuantity { get; set; }
        
        public double RemainingScheduleQuantity { get; set; }
        
        public string ProductionScheduleNo { get; set; }
        
        public string RouteSheetNo { get; set; }
        
        public string RedyingForRSNo { get; set; }
        
        public string CombineRSNo { get; set; }
        
        public int CRSID { get; set; }

        
        public double DyeChemicalCost { get; set; } // Added By Sagor on 17 Sep 2014
        
        public string DurationInString { get; set; } // Added By Sagor on 18 Sep 2014  Purpose=> set time interval from RHE dyeload and dyeunlaod state

        
        public double RouteSheetQty { get; set; }
        

        public string PSNoWithYTP { get { return this.ProductionScheduleNo + "[YTP-" + this.WaitingForProductionQty.ToString() + "]"; } }
        public string OrderDetail { get; set; }
        
        public string BuyerRef { get; set; }

        
        public bool IsInHouse { get; set; }   // Added By Sagor on 22 Sep 2014

        
        public DateTime ExpDeliveryDateByFactory { get; set; }

        public string StartTimeInString
        {
            get
            {
                return StartTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string EndTimeInString
        {
            get
            {
                return EndTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        #region Schedule Detail/ order wise / party wise /product wise

        
        public string sActionLink { get; set; }

        #endregion

        public string BatchCardNo
        {
            
            get
            {
                //if (ProductionScheduleNo != "" && ProductionScheduleNo!= null)
                //{
                //    string sPSNumber = ProductionScheduleNo.Split('-')[1];
                //    string sYear = ProductionScheduleNo.Split('-')[2];
                //    sYear = sYear.Substring(2,2);
                //    string sZero = "";
                //    if (sPSNumber.Length < 5)
                //    {
                //        for (int i = 0; i < (5 - sPSNumber.Length); i++)
                //        {
                //            sZero = sZero + "0";
                //        }
                //    }
                //    sPSNumber = "B# " + sZero + sPSNumber + "-" + sYear;
                //    return sPSNumber;
                //}
                //else
                //{
                //    return "";
                //}
                if (this.RouteSheetNo != "") { return "B# " + this.RouteSheetNo; }
                else { return ""; }
            }
        }

        
        public EnumRSState RSState { get; set; }

        public string RSStateInString
        {
            get
            {
                return RSState.ToString();
            }
        }


        #region Production Report
        
        public int RouteSheetID  { get; set; }
        
        public double UsesWeight { get; set; }
        public string Shade
        {
            get
            {
                return ColorName + " " + PSBatchNo;
            }
        }
        public string PRRemarks { get; set; }
        public double Loading { get; set; }
        public string Count
        {
            get
            {
                string sCount = this.ProductName;
                if (ProductName != "")
                {
                    int nSpace = sCount.Split(' ').Length;
                    sCount = sCount.Split(' ')[nSpace - 1];
                    if (!sCount.Contains("/"))
                    {
                        return sCount = "";
                    }
                    else
                    {
                        return sCount;
                    }

                }
                else
                {
                    return "";
                }
            }
            
        }
        #endregion

        #region Used For Schedule By DEO

        
        public DateTime DyeLoadTime { get; set; }
        
        public DateTime DyeUnLoadTime { get; set; }

        public string DyeLoadTimeInString
        {
            get
            {
                return (DyeLoadTime == DateTime.MinValue) ? "" : DyeLoadTime.ToString("dd MMM yyyy HH:mm");
            }
        }

        public string DyeUnLoadTimeInString
        {
            get
            {
                return (DyeUnLoadTime==DateTime.MinValue)? "":DyeUnLoadTime.ToString("dd MMM yyyy HH:mm");
            }
        }
        #endregion

        public string OrderNoWithoutExe
        {
            get
            {
                //if (this.OrderNo.Contains("]"))
                //{
                    return this.OrderNo;
                //}
                //else { return ""; }
            }
        }

        public double DEOScheduleQty
        {
          get
          {
              if(this.ProductionQty!=this.RouteSheetQty && this.RouteSheetQty==0)
              {
                  return this.ProductionQty;
              }
              else
              {
                  return this.RouteSheetQty;
              }
          }
        }

        public string ExpDeliveryDateByFactoryInString
        {
            get
            {
                if (this.ExpDeliveryDateByFactory == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.ExpDeliveryDateByFactory.ToString("dd MMM yyyy");
                }
            }
        }

        public double ShadePercentage { get; set; }
        public double DyePerCentage { get; set; }

        public double DyesCost { get; set; }
        public double ChemicalCost { get; set; }
        
        #endregion
        #region Functions
        public static List<ProductionScheduleDetail> Gets(int nId, long nUserID)
        {
            return ProductionScheduleDetail.Service.Gets(nId, nUserID);
        }
        public static List<ProductionScheduleDetail> Gets( long nUserID)
        {
            return ProductionScheduleDetail.Service.Gets(nUserID);
        }
        public static List<ProductionScheduleDetail> Gets(string sPSIDs,long nUserID)
        {
            return ProductionScheduleDetail.Service.Gets(sPSIDs, nUserID);
        }
        public static List<ProductionScheduleDetail> GetsSqL(string sSQL, long nUserID)
        {
            return ProductionScheduleDetail.Service.GetsSqL(sSQL, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IProductionScheduleDetailService Service
        {
            get { return (IProductionScheduleDetailService)Services.Factory.CreateService(typeof(IProductionScheduleDetailService)); }
        }
        #endregion
    }
        #endregion

        #region IProductionScheduleDetail interface
    public interface IProductionScheduleDetailService
    {

        
        List<ProductionScheduleDetail> Gets(int id, Int64 nUserID);
        
        List<ProductionScheduleDetail> Gets(Int64 nUserID);
        
        List<ProductionScheduleDetail> Gets(string sPSIDs,Int64 nUserID);
        
        List<ProductionScheduleDetail> GetsSqL(string sSQL, Int64 nUserID);

    }
    #endregion

    #endregion
}
