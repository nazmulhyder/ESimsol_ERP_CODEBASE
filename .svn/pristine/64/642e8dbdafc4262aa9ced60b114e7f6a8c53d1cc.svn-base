using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
   public class FabricYarnDeliveryChallanDetail: BusinessObject
    {
       #region FabricYarnDeliveryChallanDetail
       public FabricYarnDeliveryChallanDetail()
       {
           FYDCDetailID = 0;
           FYDChallanID = 0;
           FYDODetailID = 0;
           Qty = 0;
           LotID = 0;
           ErrorMessage = string.Empty;
           Params = string.Empty;
           FYDC = null;
           Remarks = string.Empty;
           DisburseDate = DateTime.Now;
           DisburseByName = "";
           WUID = 0;
           Color = string.Empty;
           FEOID = 0;
           UnitPrice = 0;
           FYDOID = 0;
           BagQty = string.Empty;
           LCDate = DateTime.Today;
       }
       #endregion

       #region Properties
        public int  FYDCDetailID {get; set;}
        public int  FYDChallanID {get; set;}
        public int  FYDODetailID {get; set;}
        public double Qty {get; set;}
        public int LotID { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public FabricYarnDeliveryChallan FYDC { get; set; }
        public string Remarks { get; set; }
        public DateTime DisburseDate { get; set; }
        public string DisburseByName { get; set; }
        public int WUID { get; set; }
        public int FEOID  { get; set; }
        public double UnitPrice { get; set; }
        public int FYDOID { get; set; }
        public string BagQty { get; set; }
        public DateTime LCDate { get; set; }
        #endregion

       #region Deriverd Properties
		public string LotNo {get; set;}
        public string MUName { get; set; }
		public double Balance  {get; set;}
		public string ProductCode {get; set;}
        public string ProductName { get; set; }
        public int ProductID { get; set; }
        public string Color { get; set; }
        public DateTime ChallanDate { get; set; }
        public string FYDChallanNo { get; set; }
        public string ChallanDateStr { get { return (this.ChallanDate== DateTime.MinValue)?"": this.ChallanDate.ToString("dd MMM yyyy"); } }
        public string LCDateSt { get { return (this.LCDate == DateTime.MinValue) ? "" : this.LCDate.ToString("dd MMM yyyy"); } }
       #endregion

       #region Functions

        public static FabricYarnDeliveryChallanDetail Get(int nFYDCDetailID, long nUserID)
       {
           return FabricYarnDeliveryChallanDetail.Service.Get(nFYDCDetailID, nUserID);
       }
        public static List<FabricYarnDeliveryChallanDetail> Gets(string sSQL, long nUserID)
       {
           return FabricYarnDeliveryChallanDetail.Service.Gets(sSQL, nUserID);
       }
        public FabricYarnDeliveryChallanDetail IUD(int nDBOperation, long nUserID)
       {
           return FabricYarnDeliveryChallanDetail.Service.IUD(this, nDBOperation, nUserID);
       }

       #endregion

       #region ServiceFactory
        internal static IFabricYarnDeliveryChallanDetailService Service
       {
           get { return (IFabricYarnDeliveryChallanDetailService)Services.Factory.CreateService(typeof(IFabricYarnDeliveryChallanDetailService)); }
       }

       #endregion

    }
      #region IFabricYarnDeliveryChallanDetail

   public interface IFabricYarnDeliveryChallanDetailService
   {

       FabricYarnDeliveryChallanDetail Get(int nFYDCDetailID, Int64 nUserID);
       List<FabricYarnDeliveryChallanDetail> Gets(string sSQL, Int64 nUserID);
       FabricYarnDeliveryChallanDetail IUD(FabricYarnDeliveryChallanDetail oFabricYarnDeliveryChallanDetail, int nDBOperation, Int64 nUserID);

   }
   #endregion
}
