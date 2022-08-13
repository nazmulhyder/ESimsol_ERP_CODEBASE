using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Data;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region LabDip

    public class LabDip : BusinessObject
    {
        public LabDip()
        {
            LabDipID = 0;
            LabdipNo = string.Empty;
            ContractorID = 0;
            ContactPersonnelID = 0;
            DeliveryToID = 0;
            DeliveryToContactPersonnelID = 0;
            DeliveryZoneID = 0;
            RelabOn = 0;
            PH = string.Empty;
            LightSourceID = 0;
            BuyerRefNo = string.Empty;
            PriorityLevel = EnumPriorityLevel.None;
            Note = string.Empty;
            OrderStatus = EnumLabdipOrderStatus.Initialized;
            LabDipFormat = EnumLabdipFormat.None;
            OrderReferenceType = EnumOrderType.LabDipOrder;
            OrderReferenceID = 0;
            SeekingDate = DateTime.Today;
            OrderDate = DateTime.Today;
            DBUserID = 0;
            MktPersonID = 0;
            ISTwisted = false;
            FabricID = 0;
            ErrorMessage = "";
            Params = "";
            ChallanNo = "";
            LabDipDetails = new List<LabDipDetail>();
            LabdipHistorys = new List<LabdipHistory>();
            LabDipDetailFabrics = new List<LabDipDetailFabric>();
            OrderReferenceTypeSt = "";
            OrderNo = "";
            FabricNo = "";
            ProductName = "";
            IsInHouse = false;
            LDTwistType = EnumLDTwistType.Generale;
            DeliveryNote = "";
            RelabCount = 0;
        }
      
        #region Properties
        public int LabDipID { get; set; }
        public string LabdipNo { get; set; }
        public int ContractorID { get; set; }
        public int ContactPersonnelID { get; set; }
        public int DeliveryToID { get; set; }
        public int DeliveryToContactPersonnelID { get; set; }
        public Int16 DeliveryZoneID { get; set; }
        public int RelabOn { get; set; }
        public string PH { get; set; }
        public int LightSourceID { get; set; }
        public string BuyerRefNo { get; set; }
        public EnumPriorityLevel PriorityLevel { get; set; }
        public string Note { get; set; }
        public EnumLabdipOrderStatus OrderStatus { get; set; }
        public EnumLabdipFormat LabDipFormat { get; set; }
        public EnumOrderType OrderReferenceType { get; set; }
        public int OrderReferenceID { get; set; }
        public DateTime SeekingDate { get; set; }
        public DateTime OrderDate { get; set; }
        public int DBUserID { get; set; }
        public int MktPersonID { get; set; }
        public bool ISTwisted { get; set; }
        public string ChallanNo { get; set; }
        public int FabricID { get; set; }
        public string OrderNo { get; set; }
        public string FabricNo { get; set; }
        public string ProductName { get; set; }
        public int FSCID { get; set; }
        public int FSCDetailID { get; set; }
        public string SCNo { get; set; }
        public string ActualConstruction { get; set; }
        public string Construction { get; set; }
        public int OrderType  { get; set; }
        public bool IsInHouse { get; set; }
        public int RelabCount { get; set; }
        public EnumLDTwistType LDTwistType { get; set; }
        public string DeliveryNote { get; set; }
        #endregion

        #region Derive
        public List<LabDipDetailFabric> LabDipDetailFabrics { get; set; }
        public string MktPerson { get; set; }
        public string ContractorName { get; set; }
        public string DeliveryToName { get; set; }
        public string LightSourceName { get; set; }
        public string ContractorCPName { get; set; }
        public string DeliveryToCPName { get; set; }
        public int NoOfColor { get; set; }
        public int LabStatus { get; set; }
        public string OrderReferenceTypeSt { get; set; }
        public string ProcessTypeName { get; set; }
        public string FabricWeaveName { get; set; }
        public string FinishTypeName { get; set; }
        public string ExeNo { get; set; }
        public string ErrorMessage { get; set; }
        public string Params { get; set; }
        public List<LabDipDetail> LabDipDetails { get; set; }
        public List<LabdipHistory> LabdipHistorys { get; set; }
        public string LDTwistTypeStr
        {
            get
            {
                return this.LDTwistType.ToString();
            }
        }
        public string PriorityLevelStr
        {
            get
            {
                return this.PriorityLevel.ToString();
            }
        }
        public string OrderStatusStr
        {
            get
            {
                return this.OrderStatus.ToString();
            }
        }
        public string LabDipFormatStr
        {
            get
            {
                return EnumObject.jGet(this.LabDipFormat);
            }
        }
        public string LabStatusSt
        {
            get
            {
                return EnumObject.jGet((EnumFabricLabStatus)this.LabStatus);
            }
        }
        public string SeekingDateStr
        {
            get
            {
                if (this.SeekingDate == DateTime.MinValue) return "--";
                else return this.SeekingDate.ToString("dd MMM yyyy");
            }
        }
        public string OrderDateStr
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
            set{}
        }

        public string TwistedStr
        {
            get
            {
                return (this.ISTwisted) ? "Yes" : "No";
            }
        }


        #endregion


        #region Functions

        public LabDip IUD(int nDBOperation, int nUserID)
        {
            return LabDip.Service.IUD(this, nDBOperation, nUserID);
        }
        public static LabDip Get(int nLabDipID, int nUserID)
        {
            return LabDip.Service.Get(nLabDipID, nUserID);
        }
        public static LabDip GetByFSD(int nFSCDetailID, int nUserID)
        {
            return LabDip.Service.GetByFSD(nFSCDetailID, nUserID);
        }
        public static LabDip GetByFSDMap(string nFSCDetailIDs, int nUserID)
        {
            return LabDip.Service.GetByFSDMap(nFSCDetailIDs, nUserID);
        }
        public static List<LabDip> Gets(string sSQL, int nUserID)
        {
            return LabDip.Service.Gets(sSQL, nUserID);
        }
        public static List<LabDip> Gets( int nUserID)
        {
            return LabDip.Service.Gets( nUserID);
        }
        public LabDip ChangeOrderStatus(short nNextStatus, int nUserID)
        {
            return LabDip.Service.ChangeOrderStatus(this, nNextStatus, nUserID);
        }
        public LabDip DirectApproval(short nNextStatus, int nUserID)
        {
            return LabDip.Service.DirectApproval(this, nNextStatus, nUserID);
        }

        public static LabDip Relab(int nLabDipID, int nUserID)
        {
            return LabDip.Service.Relab(nLabDipID, nUserID);
        }
        public static LabDip CreateLabdipByDO(int nDyeingOrderID, int nUserID)
        {
            return LabDip.Service.CreateLabdipByDO(nDyeingOrderID, nUserID);
        }
        public LabDip Save_Challan(int nUserID)
        {
            return LabDip.Service.Save_Challan(this, nUserID);
        }
        public LabDip IUD_LD_Fabric(int nUserID)
        {
            return LabDip.Service.IUD_LD_Fabric(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILabDipService Service
        {
            get { return (ILabDipService)Services.Factory.CreateService(typeof(ILabDipService)); }
        }
        #endregion


        public LabDip IUD_Fabric(int enumOperation, int nUser)
        {
            return LabDip.Service.IUD_Fabric(this, enumOperation, nUser);
        }

    }

    #endregion

    #region ILabDipOrder interface
    public interface ILabDipService
    {
        LabDip IUD(LabDip oLabDip, int nDBOperation, int nUserID);
        LabDip Get(int nLabDipID, int nUserID);
        List<LabDip> Gets(string sSQL, int nUserID);
        List<LabDip> Gets( int nUserID);
        LabDip IUD_LD_Fabric(LabDip oLabDip,  int nUserID);
        LabDip ChangeOrderStatus(LabDip oLabDip, short nNextStatus, int nUserID);
        LabDip DirectApproval(LabDip oLabDip, short nNextStatus, int nUserID);
        LabDip Relab(int nLabDipID, int nUserID);
        LabDip Save_Challan(LabDip oLabDip, int nUserID);
        LabDip CreateLabdipByDO(int nDyeingOrderID, int nUserID);
        LabDip GetByFSD(int nFSCDetailID, int nUserID);
        LabDip GetByFSDMap(string nFSCDetailIDs, int nUserID);

        LabDip IUD_Fabric(LabDip oLabDip, int enumOperation, int nUserid);
    }
    #endregion

}