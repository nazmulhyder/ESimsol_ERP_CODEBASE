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
    #region DyeingOrderDetail
    
    public class DyeingOrderDetail : BusinessObject
    {
        public DyeingOrderDetail()
        {
            DyeingOrderID = 0;
            DyeingOrderDetailID = 0;
            LabDipDetailID=0;
            ExportSCDetailID = 0;
            Qty=0.0;
            UnitPrice = 0.0;
            Shade = 0;
            NoOfCone="";
            LengthOfCone = "";
            NoOfCone_Weft = "";
            ReceiveDate = DateTime.MinValue;
            LengthOfCone_Weft = "";
            DeliveryDate = DateTime.MinValue;
            Note = "";
            //DepthofShade = "";
            LabDipType = (int)EnumLabDipType.Normal;
            PTUID = 0;
            JobNo = "";
            Qty_Schedule = 0;
            BuyerName = "";
            ErrorMessage = "";
            Qty_Pro = 0;
            MUnitID = 0;
            OrderDateNew = DateTime.MinValue;
            LDNo = "";
            RGB = "";
            MUnit = "";
            OrderNo = "";
            ProductNameCode = "";
            ProductName = "";
            CellRowSpans = new List<CellRowSpan>();
            SL = 0;
            YetQty = 0;
        }

        #region Properties
        public int DyeingOrderDetailID { get; set; }
        public int DyeingOrderID { get; set; }
        public int LabDipDetailID { get; set; }
        public int ProductID { get; set; }
        public int ExportSCDetailID { get; set; }
        public int LabDipType { get; set; }
        public int Shade { get; set; }
        public string ColorName { get; set; }
        public string ColorNo { get; set; }
        public string LDNo { get; set; }
        public string PantonNo { get; set; }
        public string RGB { get; set; }
        public double Qty { get; set; }
        public double UnitPrice { get; set; }
        public int HankorCone { get; set; }
        public int BuyerCombo { get; set; }
        public string BuyerRef { get; set; }
        public string BuyerName { get; set; }
        public string ApproveLotNo { get; set; }
        public string NoOfCone { get; set; }
        public string LengthOfCone { get; set; }
        public string NoOfCone_Weft { get; set; }
        public string LengthOfCone_Weft { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public string MUnit { get; set; }
        public int MUnitID { get; set; }
        
        //public string DepthofShade { get; set; }
        public int Status { get; set; }
        public string JobNo { get; set; }
        public string ErrorMessage { get; set; }
        public int PTUID { get; set; }
        public int SL { get; set; }
        #endregion

        #region Derived Property
        public double DOQty { get; set; }
        public string ProductNameCode { get; set; }
        public string ProductCodeName { get; set; }
        public string ProductName { get; set; }
        public string LabdipNo { get; set; }
        public string OrderNo { get; set; } // for view DO NO & LotParent Assigning
        public string OrderDate { get; set; }// for view DO Date
        public DateTime OrderDateNew { get; set; }// for view DO Date
        public string ColorDevelopProduct { get; set; }
        public double Qty_Pro { get; set; }
        public double Qty_Schedule { get; set; }
        public double YetQty { get; set; }
        public string ShadeSt
        {
            get
            {
                if (this.LabDipType == (int)EnumShade.AVL)
                {
                    return "ANY";
                }
                else if (this.LabDipType == (int)EnumShade.DTM)
                {
                    return "DTM";
                }
                else
                {
                    return ((EnumShade)this.Shade).ToString();
                }
            }
        }
        public string LabDipTypeSt
        {
            get
            {
                return ((EnumLabDipType)this.LabDipType).ToString();
            }
        }
        public string StatusSt
        {
            get
            {
                return EnumObject.jGet((EnumDyeingOrderState)this.Status);
            }
        }
       
        public double Amount
        {
            get
            {
                return Qty * UnitPrice;
            }
        }
        public string DeliveryDateSt
        {
            get
            {
                if (this.DeliveryDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return DeliveryDate.ToString("dd MMM yyyy");
                }
            }
        }
        public List<CellRowSpan> CellRowSpans { get; set; }
        #endregion

        #region Loan Order
        public int ContractorID { get; set; }
        public string ContractorName { get; set; }
        public string MUnitName { get; set; }
        public int DUProGuideLineID { get; set; }
        public int ReceiveProductID { get; set; }
        public int ReceiveProductMUnitID { get; set; }
        public int DeliveryChallanDetailID { get; set; }
        public int DeliveryChallanProductID { get; set; }
        public int DeliveryChallanMUnitID { get; set; }
        public string ReceiveSLNo { get; set; }
        public string DeliveryChallanProductName { get; set; }
        public string DeliveryChallanMUnitName { get; set; }
        public string ReceiveProductName { get; set; }
        public string DeliveryChallanNo { get; set; }
        public string ReceiveProductMUnitName { get; set; }
        public DateTime ReceiveDate { get; set; }
        public double DeliveryChallanQty{ get; set; }
        public double ReceiveProductQty { get; set; }
        public string ReceiveDateSt
        {
            get
            {
                if(this.ReceiveDate == DateTime.MinValue)
                {
                    return "-";
                }
                else
                {
                    return this.ReceiveDate.ToString("dd MMM yyyy");
                }
                   
            }
        }
        public string OrderDateSt { get; set; }
        public string OrderDateNewSt
        {
            get
            {
                if (this.OrderDateNew == DateTime.MinValue)
                    return "-";
                else
                    return this.OrderDateNew.ToString("dd MMM yyyy");
            }
        }
        
        public string DeliveryChallanQtySt
        {
            get
            {
                return Math.Round(DeliveryChallanQty, 2).ToString();
            }
        }
        public string ReceiveProductQtySt
        {
            get
            {
                return Math.Round(ReceiveProductQty, 2).ToString();
            }
        }
        #endregion

        #region Property For Order Follow Up
        #endregion

        #region Property for History
        public int DyeingOrderHistoryID { get; set; }
        public DateTime OprationDate { get; set; }
        public int CurrentStatus { get; set; }
        public int PreviousStatus { get; set; }
        public string Note_System { get; set; }
        public string HistoryNote { get; set; }
        public string UserName { get; set; }
        public string OprationDateSt
        {
            get
            {
                return OprationDate.ToString("dd MMM yyyy hh:mm tt");
            }
        }
        public string CurrentStatusSt
        {
            get
            {
                return ((EnumDyeingOrderState)CurrentStatus).ToString();
            }
        }
        public string PreviousStatusSt
        {
            get
            {
                return ((EnumDyeingOrderState)PreviousStatus).ToString();
            }
        }
        #endregion

        #region Functions
        public  DyeingOrderDetail Get(int nId, long nUserID)
        {
            return DyeingOrderDetail.Service.Get(nId, nUserID);
        }

        public static List<DyeingOrderDetail> Gets(int nDyeingOrderID, long nUserID)
        {
            return DyeingOrderDetail.Service.Gets(nDyeingOrderID, nUserID);
        }
        public static List<DyeingOrderDetail> GetsBy(int nSampleInvoiceID, long nUserID)
        {
            return DyeingOrderDetail.Service.GetsBy(nSampleInvoiceID, nUserID);
        }
        public static List<DyeingOrderDetail> GetsBy(int nLabDipDetailID, int nOrderID, int nOrderType, long nUserID)
        {
            return DyeingOrderDetail.Service.GetsBy( nLabDipDetailID,  nOrderID,  nOrderType, nUserID);
        }
        public static List<DyeingOrderDetail> Gets(string sSQL, long nUserID)
        {
            return DyeingOrderDetail.Service.Gets(sSQL, nUserID);
        }
        //public static List<DyeingOrderDetail> GetsLoanOrder(string sSQL, long nUserID)
        //{
        //    return DyeingOrderDetail.Service.GetsLoanOrder(sSQL, nUserID);
        //}
        public DyeingOrderDetail Save(long nUserID)
        {
            return DyeingOrderDetail.Service.Save(this, nUserID);
        }
   
        public string Delete(long nUserID)
        {
            return DyeingOrderDetail.Service.Delete(this, nUserID);
        }

        public DyeingOrderDetail OrderHold(Int64 nUserID)
        {
            return DyeingOrderDetail.Service.OrderHold(this, nUserID);
        }
        public static List<DyeingOrderDetail> MakeTwistedGroup(string sDyeingOrderDetailID, int nDyeingOrderID, int nTwistedGroup, int nDBOperation, int nUserID)
        {
            return DyeingOrderDetail.Service.MakeTwistedGroup(sDyeingOrderDetailID, nDyeingOrderID, nTwistedGroup, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDyeingOrderDetailService Service
        {
            get { return (IDyeingOrderDetailService)Services.Factory.CreateService(typeof(IDyeingOrderDetailService)); }
        }
        #endregion
    }
    #endregion

    #region IDyeingOrderDetail interface
    
    public interface IDyeingOrderDetailService
    {
        
        DyeingOrderDetail Get(int id, long nUserID);
        List<DyeingOrderDetail> Gets(int nDyeingOrderID, long nUserID);
        List<DyeingOrderDetail> GetsBy(int nSampleInvoiceID, long nUserID);
        List<DyeingOrderDetail> GetsBy(int nLabDipDetailID, int nOrderID, int nOrderType, long nUserID);
        List<DyeingOrderDetail> Gets(string sSQL, long nUserID);
        //List<DyeingOrderDetail> GetsLoanOrder(string sSQL, long nUserID);
        string Delete(DyeingOrderDetail oDyeingOrder, long nUserID);
        DyeingOrderDetail Save(DyeingOrderDetail oDyeingOrderDetail, long nUserID);
        DyeingOrderDetail OrderHold(DyeingOrderDetail oDyeingOrderDetail, Int64 nUserID);
        List<DyeingOrderDetail> MakeTwistedGroup(string sDyeingOrderDetailID, int nDyeingOrderID, int nTwistedGroup, int nDBOperation, int nUserID);


    }
    #endregion
}
