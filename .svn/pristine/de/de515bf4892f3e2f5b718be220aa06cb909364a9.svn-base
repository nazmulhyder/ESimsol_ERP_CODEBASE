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
   public class FabricYarnDeliveryOrder :BusinessObject
   {
        #region FabricYarnDeliveryOrder
        public FabricYarnDeliveryOrder()
        {
            FYDOID =0;
            FEOID =0;
            FYDNo =String.Empty;
            DeliveryUnit = EnumTextileUnit.None;
            ApproveBy =0;
            ApproveDate =DateTime.Today;
            ExpectedDeliveryDate =DateTime.Today;
            ErrorMessage = string.Empty;
            Params = string.Empty;
            FYDODetails = new List<FabricYarnDeliveryOrderDetail>();
            ReviseNo = 0;
            LCNo = string.Empty;
            BuyerName = string.Empty;
            Construction = string.Empty;
            Remark = string.Empty;
        }
        #endregion

        #region Properties
        public int FYDOID {get; set;}
        public int FEOID {get; set;}
        public string FYDNo {get; set;}
        public EnumTextileUnit DeliveryUnit {get; set;}
        public int  ApproveBy {get; set;}
        public DateTime ApproveDate {get; set;}
        public DateTime ExpectedDeliveryDate {get; set;}
        public string ErrorMessage {get; set;}
        public string Params {get; set;}
        public int ReviseNo { get; set; }
        public string Construction { get; set; }
        public string Remark { get; set; }
        #endregion

        #region Deriverd Properties
        public double DEOQty { get; set; }
        public double OrderQty {get; set;}
        public double ChallanQty { get; set; }
        public string FEONo {get; set;}
        public string ApproveByName { get; set; }
        public DateTime ReviseDate { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string DBServerDateTimeStr { get { return (this.DBServerDateTime == DateTime.MinValue) ? "" : this.DBServerDateTime.ToString("dd MMM yyyy"); } }
        public string ReviseDateStr { get { return (this.ReviseDate == DateTime.MinValue) ? "" : this.ReviseDate.ToString("dd MMM yyyy"); } }
        public string ApproveDateStr { get { return (this.ApproveDate == DateTime.MinValue) ? "" : this.ApproveDate.ToString("dd MMM yyyy"); } }
        public string ExpectedDeliveryDateStr { get { return (this.ExpectedDeliveryDate == DateTime.MinValue) ? "" : this.ExpectedDeliveryDate.ToString("dd MMM yyyy"); } }
        public string DeliveryUnitStr { get { return this.DeliveryUnit.ToString();  } }
        public List<FabricYarnDeliveryOrderDetail> FYDODetails { get; set; }
        public string LCNo { get; set; }
        public string BuyerName { get; set; }
        #endregion

        #region Functions

        public static FabricYarnDeliveryOrder Get(int nFYDOID, long nUserID)
        {
            return FabricYarnDeliveryOrder.Service.Get(nFYDOID, nUserID);
        }
        public static List<FabricYarnDeliveryOrder> Gets(string sSQL, long nUserID)
        {
            return FabricYarnDeliveryOrder.Service.Gets(sSQL, nUserID);
        }
        public FabricYarnDeliveryOrder IUD(int nDBOperation, long nUserID)
        {
            return FabricYarnDeliveryOrder.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IFabricYarnDeliveryOrderService Service
        {
            get { return (IFabricYarnDeliveryOrderService)Services.Factory.CreateService(typeof(IFabricYarnDeliveryOrderService)); }
        }

        #endregion
   }
   #region IFabricYarnDeliveryOrderinterface

   public interface IFabricYarnDeliveryOrderService
   {

       FabricYarnDeliveryOrder Get(int nFYDOID, Int64 nUserID);
       List<FabricYarnDeliveryOrder> Gets(string sSQL, Int64 nUserID);
       FabricYarnDeliveryOrder IUD(FabricYarnDeliveryOrder oFabricYarnDeliveryOrder, int nDBOperation, Int64 nUserID);

   }
   #endregion
}
