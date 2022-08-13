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
    #region DUClaimOrderDetail
    
    public class DUClaimOrderDetail : BusinessObject
    {
        #region  Constructor
        public DUClaimOrderDetail()
        {
            DUClaimOrderDetailID = 0;
            DUClaimOrderID = 0;
            DyeingOrderDetailID = 0;
            LotID = 0;
            Qty = 0;
            PTUID = 0;
            LabDipDetailID = 0;
            ProductID = 0;
            OrderQty = 0;
            Note = "";
            MUnit = "";
            ProductCode = "";
            ProductName = "";
            ColorNo = "";
            ColorName = "";
            LotNo = "";
            Shade = 0;
            ErrorMessage = "";
            DOQty = 0;
            Qty_RS = 0;
            ExportSCDetailID = 0;
            ParentDODetailID = 0;
            Date = "";
            BatchNo = "";
            ChallanNo = "";
            ClaimReasonID = 0;
            ClaimOrderNo = "";
            OrderDate = DateTime.Now;
            ClaimType = 0;
            OrderNo = "";
            BuyerID = 0;
        }
        #endregion

        #region Properties
        
        public int DUClaimOrderDetailID { get; set; }
        public int DUClaimOrderID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public int ParentDODetailID { get; set; }
        public int ParentDOID { get; set; }/// For carry property
        public int ClaimReasonID { get; set; }
        public int PTUID { get; set; }
        public int LabDipDetailID { get; set; }
        public int ProductID { get; set; }
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
        public string ApproveLotNo { get; set; }
        public string NoOfCone { get; set; }
        public string LengthOfCone { get; set; }
        public string NoOfCone_Weft { get; set; }
        public string LengthOfCone_Weft { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
        public double OrderQty { get; set; }
        public int LotID { get; set; }
        public string MUnit { get; set; }
        public int Status { get; set; }
        public string LabdipNo { get; set; }
        public string ColorDevelopProduct { get; set; }
        public string ChallanNo { get; set; }
        public string BatchNo { get; set; }
        public String ErrorMessage { get; set; }
        #endregion

        #region derived
        public string ClaimOrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int ClaimType { get; set; }
        public string OrderNo { get; set; }
        public int BuyerID { get; set; }
        public string BuyerName { get; set; }

        public string ClaimRegion { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string LotNo { get; set; }
        public double DOQty { get; set; }
        public double Qty_RS { get; set; }
        public int ExportSCDetailID { get; set; }
        public string Date { get; set; }
        public string ClaimTypeSt
        {
            get
            {
                return ((EnumClaimOrderType)this.ClaimType).ToString();
            }
        }
        public string OrderDateSt
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }
        public string StatusSt
        {
            get
            {
                return EnumObject.jGet((EnumDOState)this.Status);
            }
        }
        public string ShadeSt
        {
            get
            {
              return ((EnumShade)this.Shade).ToString();
            }
        }
        public string LabDipTypeSt
        {
            get
            {
                return ((EnumLabDipType)this.LabDipType).ToString();
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
        #endregion

        #region Functions
        public DUClaimOrderDetail Get(int nDUClaimOrderDetailID, long nUserID)
        {
            return DUClaimOrderDetail.Service.Get(nDUClaimOrderDetailID, nUserID);
        }
        public static List<DUClaimOrderDetail> Gets(int nDUClaimOrderID, long nUserID)
        {
            return DUClaimOrderDetail.Service.Gets(nDUClaimOrderID, nUserID);
        }
        public static List<DUClaimOrderDetail> Gets(string sSQL, long nUserID)
        {
            return DUClaimOrderDetail.Service.Gets(sSQL, nUserID);
        }
        public DUClaimOrderDetail Save(long nUserID)
        {
            return DUClaimOrderDetail.Service.Save(this, nUserID);
        }
        public string Delete(long nUserID)
        {
            return DUClaimOrderDetail.Service.Delete(this, nUserID);
        }
      

        #endregion

        #region ServiceFactory
        internal static IDUClaimOrderDetailService Service
        {
            get { return (IDUClaimOrderDetailService)Services.Factory.CreateService(typeof(IDUClaimOrderDetailService)); }
        }
        #endregion
    }
    #endregion



    #region IDUClaimOrderDetail interface
    
    public interface IDUClaimOrderDetailService
    {
        DUClaimOrderDetail Get(int id, long nUserID);
        List<DUClaimOrderDetail> Gets(int nDUClaimOrderID, long nUserID);
        List<DUClaimOrderDetail> Gets(string sSQL, long nUserID);
        string Delete(DUClaimOrderDetail oDUClaimOrderDetail, long nUserID);
        DUClaimOrderDetail Save(DUClaimOrderDetail oDUClaimOrderDetail, long nUserID);
      

    }
    #endregion
}
