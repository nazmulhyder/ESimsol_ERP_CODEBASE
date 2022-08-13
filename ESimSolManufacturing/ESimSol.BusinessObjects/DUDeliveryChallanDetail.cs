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
    #region DUDeliveryChallanDetail
    
    public class DUDeliveryChallanDetail : BusinessObject
    {
        public DUDeliveryChallanDetail()
        {

            DUDeliveryChallanID = 0;
            DUDeliveryChallanDetailID = 0;
            ProductID = 0;
            OrderID = 0;
            DODetailID = 0;
            DOID = 0;
            Qty=0.0;
            PTUID = 0;
            MUnit = "";
            ChallanNo = "";
            Note = "";
            Shade = 0;
            ProductName = "";
            ColorName = "";
            ColorNo = "";
            ChallanDate = "";
            LotNo = "";
            ErrorMessage = "";
            PartyName = "";
            DONo = "";
            DyeingOrderDetailID = 0;
            RefNo = "";
            DeliveryPoint = "";
            StockInHand = 0;
            GYLotNo = "";
        }



        #region Properties
        public int DUDeliveryChallanDetailID { get; set; }
        public int DUDeliveryChallanID { get; set; }
        public int ProductID { get; set; }
        public int LotID { get; set; }
        public int OrderID { get; set; }
        public int DyeingOrderID { get; set; }
        public int OrderType { get; set; }
        public int DODetailID { get; set; }
        public int DOID { get; set; }
        public int PTUID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int Shade { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public double BagQty { get; set; }
        public int HanksCone { get; set; }
        public string Note { get; set; }
        public string MUnit { get; set; }
        public string PartyName { get; set; }
        public string RefNo { get; set; }
        public string DeliveryPoint { get; set; }
        
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string DONo { get; set; }
        public string OrderNo { get; set; }
        public string PI_SampleNo { get; set; }
        public string ChallanNo { get; set; }
        public string ChallanDate { get; set; }
        public string LotNo { get; set; }
        public string GYLotNo { get; set; }
        public string ColorNo { get; set; }
        public double StockInHand { get; set; }
        public double OrderQty { get; set; }
        public double DeliveryQty { get; set; }
        public string ShadeSt
        {
            get
            {
                if (this.Shade == (int)EnumShade.AVL)
                {
                    return "ANY";
                }
                else if (this.Shade == (int)EnumShade.DTM)
                {
                    return "DTM";
                }
                else if (this.Shade == (int)EnumShade.NA)
                {
                    return "";
                }
                else
                {
                    return ((EnumShade)this.Shade).ToString();
                }
            }
        }
       
        public int WorkingUnitID { get; set; }
        #endregion

        #region Functions
        public static DUDeliveryChallanDetail Get(int nId, long nUserID)
        {
            return DUDeliveryChallanDetail.Service.Get(nId, nUserID);
        }
        public static List<DUDeliveryChallanDetail> Gets(int nDUDeliveryChallanID, long nUserID)
        {
            return DUDeliveryChallanDetail.Service.Gets(nDUDeliveryChallanID, nUserID);
        }
        public static List<DUDeliveryChallanDetail> Gets_Lot(int nWorkingUnitID, int nDODetailID, int nPTUID, int nDyeingOrderDetailID, int nLotID, long nUserID)
        {
            return DUDeliveryChallanDetail.Service.Gets_Lot( nWorkingUnitID,  nDODetailID,  nPTUID,nDyeingOrderDetailID,  nLotID, nUserID);
        }
        public static List<DUDeliveryChallanDetail> Gets(string sSQL, long nUserID)
        {
            return DUDeliveryChallanDetail.Service.Gets(sSQL, nUserID);
        }
        public DUDeliveryChallanDetail Save(long nUserID)
        {
            return DUDeliveryChallanDetail.Service.Save(this, nUserID);
        }
   
        public string Delete(long nUserID)
        {
            return DUDeliveryChallanDetail.Service.Delete(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDUDeliveryChallanDetailService Service
        {
            get { return (IDUDeliveryChallanDetailService)Services.Factory.CreateService(typeof(IDUDeliveryChallanDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IDUDeliveryChallanDetail interface
    
    public interface IDUDeliveryChallanDetailService
    {
        
        DUDeliveryChallanDetail Get(int id, long nUserID);
        List<DUDeliveryChallanDetail> Gets(int nDUDeliveryChallanID, long nUserID);
        List<DUDeliveryChallanDetail> Gets_Lot(int nWorkingUnitID, int nDODetailID, int nPTUID, int nDyeingOrderDetailID, int nLotID, long nUserID);
        List<DUDeliveryChallanDetail> Gets(string sSQL, long nUserID);
        string Delete(DUDeliveryChallanDetail oDUDeliveryChallanDetail, long nUserID);
        DUDeliveryChallanDetail Save(DUDeliveryChallanDetail oDUDeliveryChallanDetail, long nUserID);
      

      


    }
    #endregion
}
