using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region CapitalResource
    public class CapitalResource : BusinessObject
    {
        public CapitalResource()
        {
            CRID = 0;
            CRGID = 1;
            Code = "";
            Name = "";
            ParentID = 0;
            IsLastLayer = false;
            Model = "";
            Brand = "";
            MadeIn = "";
            MadeBy = "";
            MachineCapacity = "";
            Warranty = 0;
            WarrantyOn = "";
            WarrantyStart = DateTime.MinValue;
            WarrantyEnd = DateTime.MinValue;
            SerialNumberOnProduct = "";
            TagNo = "";
            ActualAssetValue = 0;
            ValueAfterEvaluation = 0;
            CNF_FOBValue_Foreign = 0;
            CNF_FOBValue_Local = 0;
            TotalLandedCost = 0;
            InstallationCost = 0;
            OtherCost = 0;
            CurrencyID = 0;
            Note = "";
            IsActive = false;
            SupplierID = 0;
            SupplierAddress = "";
            SupplierContactPerson = "";
            SupplierContactPersonContact = "";
            SupplierNote = "";
            LAName = "";
            LAContactPerson = "";
            LAAddress = "";
            LAWorkshop = "";
            LANote = "";
            InstallationDate = DateTime.MinValue;
            InstallationNote = "";
            InstallationLocationID = 0;
            BasicFunction = "";
            MachineLifeTime = "";
            PowerConsumption = "";
            TechnicalSpecification = "";
            PerformanceSpecification = "";
            PortOfShipment = "";
            LCNo = "";
            HSCode = "";
            SupplierEmail = "";
            SupplierPhone = "";
            SupplierFax = "";
            LAEmail = "";
            LAPhone = "";
            LAFax = "";
            CommissioningDate = DateTime.MinValue;
            CommissioningBy = "";
            InsuranceCost = 0;
            CustomDutyCost = 0;
            ContractorName = "";
            LocationName = "";
            OperationUnitName = "";
            CurrencySymbol = "";
            BUID = 0;
            BusinessUnitName = "";
            RackID = 0;
            ShelfID = 0;
            RackNo = "";
            FinishGoodWeight = 0;
            ConsumptionAmount = 0;
            NaliWeight = 0;
            FGWeightUnit = 0;
            FGWeightUnitSymbol = "";
            FGWeightUnitName = "";
            Cavity = 0;
            LocationID = 0;
            ResourcesType = EnumResourcesType.None;
            ErrorMessage = "";
            Params = "";
        }

        #region Properties
        public int CRID { get; set; }
        public int CRGID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public bool IsLastLayer { get; set; }
        public string Model { get; set; }
        public string Brand { get; set; }
        public string MadeIn { get; set; }
        public string MadeBy { get; set; }
        public string MachineCapacity { get; set; }
        public int Warranty { get; set; }
        public string WarrantyOn { get; set; }
        public DateTime WarrantyStart { get; set; }
        public DateTime WarrantyEnd { get; set; }
        public string SerialNumberOnProduct { get; set; }
        public string TagNo { get; set; }
        public double ActualAssetValue { get; set; }
        public double ValueAfterEvaluation { get; set; }
        public double CNF_FOBValue_Foreign { get; set; }
        public double CNF_FOBValue_Local { get; set; }
        public double TotalLandedCost { get; set; }
        public double InstallationCost { get; set; }
        public double OtherCost { get; set; }
        public int CurrencyID { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public int SupplierID { get; set; }
        public string SupplierAddress { get; set; }
        public string SupplierContactPerson { get; set; }
        public string SupplierContactPersonContact { get; set; }
        public string SupplierNote { get; set; }
        public string LAName { get; set; }
        public string LAContactPerson { get; set; }
        public string LAAddress { get; set; }
        public string LAWorkshop { get; set; }
        public string LANote { get; set; }
        public DateTime InstallationDate { get; set; }
        public string InstallationNote { get; set; }
        public int InstallationLocationID { get; set; }
        public string BasicFunction { get; set; }
        public string MachineLifeTime { get; set; }
        public string PowerConsumption { get; set; }
        public string TechnicalSpecification { get; set; }
        public string PerformanceSpecification { get; set; }
        public string PortOfShipment { get; set; }
        public string LCNo { get; set; }
        public string HSCode { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierPhone { get; set; }
        public string SupplierFax { get; set; }
        public string LAEmail { get; set; }
        public string LAPhone { get; set; }
        public string LAFax { get; set; }
        public DateTime CommissioningDate { get; set; }
        public string CommissioningBy { get; set; }
        public double InsuranceCost { get; set; }
        public double CustomDutyCost { get; set; }
        public int RackID { get; set; }
        public int ShelfID { get; set; }
        public string RackNo { get; set; }
        public int BUID { get; set; }
        public string BusinessUnitName { get; set; }
        public int Cavity { get; set; }
        public EnumResourcesType ResourcesType { get; set; }
        public double FinishGoodWeight { get; set; }
        public double  NaliWeight { get; set; }
        public int FGWeightUnit { get; set; }
        public string FGWeightUnitName { get; set; }
        public double ConsumptionAmount { get; set; }
        public string FGWeightUnitSymbol { get; set; }
        public int LocationID { get; set; }
        public string ParentName { get; set; }
        public string Params { get; set; }
        
        #endregion

        #region Derived Property
        public int ResourcesTypeInInt { get; set; }
        public string ContractorName { get; set; }
        public string LocationName { get; set; }
        public string OperationUnitName { get; set; }
        public string CurrencySymbol { get; set; }
        public string MachineNoWithCapacityAndTotalSchedule { get; set; }
        public string InstallationLocationName
        {
            get
            {
                return this.LocationName + "[" + this.OperationUnitName + "]";
            }
        }
        public string ConsumptionAmountStr
        {
            get
            {
                return Math.Round(this.ConsumptionAmount, 2).ToString();
            }
        }
        public string ResourcesTypeName
        {
            get
            {
                return this.ResourcesType.ToString();
            }
        }
        public double TotalWeight
        {
            get
            {
                return this.FinishGoodWeight + this.NaliWeight;
            }
        }
        public string MachineNoWithCapacity
        {
            get
            {
                return "["+this.Code +"]"+ this.MachineCapacity;
            }
        }
        public string MachineNameWithCapacity
        {
            get
            {
                return "[" + this.Name + "]" + this.MachineCapacity;
            }
        }
        
        #endregion
        #region Non DB Property
        public string ErrorMessage { get; set; }
        public string WarrantyStartDateInString
        {
            get
            {
                if (this.WarrantyStart == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.WarrantyStart.ToString("dd MMM yyyy");
                }
            }
        }
        public string WarrantyEndDateInString
        {
            get
            {
                if (this.WarrantyEnd == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.WarrantyEnd.ToString("dd MMM yyyy");
                }
            }
        }
        public string InstallationDateInString
        {
            get
            {
                if (this.InstallationDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.InstallationDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string CommissioningDateInString
        {
            get
            {
                if (this.CommissioningDate == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return this.CommissioningDate.ToString("dd MMM yyyy");
                }
            }
        }
        public string ActivityInStr
        {
            get
            {
                if (this.IsActive) { return "Active"; }
                else { return "Inactive"; }
            }
        }
        #endregion

        #region Functions
        public CapitalResource IUD(int nDBOperation, long nUserID)
        {
            return CapitalResource.Service.IUD(this, nDBOperation, nUserID);
        }
        public static CapitalResource Get(int nCRID, long nUserID)
        {
            return CapitalResource.Service.Get(nCRID, nUserID);
        }
        public static List<CapitalResource> Gets(string sSQL, long nUserID)
        {
            return CapitalResource.Service.Gets(sSQL, nUserID);
        }
        public static List<CapitalResource> GetsResourceType(long nUserID)
        {
            return CapitalResource.Service.GetsResourceType(nUserID);
        }
        public static List<CapitalResource> GetsResourceTypeWithBU(int buid, long nUserID)
        {
            return CapitalResource.Service.GetsResourceTypeWithBU(buid, nUserID);
        }
        public CapitalResource CopyCR(int nCapitalResourceID, long nUserID)
        {
            return CapitalResource.Service.CopyCR(nCapitalResourceID, nUserID);
        }
        public CapitalResource CopyCapitalResource(CapitalResource oCapitalResource, long nUserID)
        {
            return CapitalResource.Service.CopyCapitalResource(oCapitalResource, nUserID);
        }
        public static List<CapitalResource> BUWiseGets(int nBUID, long nUserID)
        {
            return CapitalResource.Service.BUWiseGets(nBUID, nUserID);
        }
        public static List<CapitalResource> BUWiseResourceTypeGets(int nBUID, long nUserID)
        {
            return CapitalResource.Service.BUWiseResourceTypeGets(nBUID, nUserID);
        }
        public static List<CapitalResource> GetsByBUandResourceTypeWithName(int nBUID, int nResourceType, string sName, long nUserID)
        {
            return CapitalResource.Service.GetsByBUandResourceTypeWithName(nBUID,nResourceType,sName, nUserID);
        }
        #endregion

        #region ServiceFloor
        internal static ICapitalResourceService Service
        {
            get { return (ICapitalResourceService)Services.Factory.CreateService(typeof(ICapitalResourceService)); }
        }
        #endregion
    }
    #endregion

    #region ICapitalResource interface
    public interface ICapitalResourceService
    {
        CapitalResource IUD(CapitalResource oCapitalResource, int nDBOperation, Int64 nUserID);
        CapitalResource Get(int nCRID, Int64 nUserID);
        List<CapitalResource> Gets(string sSQL, Int64 nUserID);
        List<CapitalResource> GetsResourceType(Int64 nUserID);
        List<CapitalResource> GetsResourceTypeWithBU(int buid, Int64 nUserID);
        List<CapitalResource> BUWiseGets(int nBUID, Int64 nUserID);
        List<CapitalResource> BUWiseResourceTypeGets(int nBUID, Int64 nUserID);
        List<CapitalResource> GetsByBUandResourceTypeWithName(int nBUID, int nResourceType, string Name, Int64 nUserID);
        CapitalResource CopyCR(int nCapitalResourceID, Int64 nUserID);
        CapitalResource CopyCapitalResource(CapitalResource oCapitalResource, Int64 nUserID);

    }
    #endregion

    #region TCapitalResource
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TCapitalResource
    {
        public TCapitalResource()
        {
            id = 0;
            text = "";
            state = "";
            IsLastLayer = false;
            activity = "";
            parentid = 0;
            code = "";
            Note = "";
            CRGID = 0;
            //AccountTypeInString = "";
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public bool IsLastLayer { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public string activity { get; set; }        //: define activity
        public int parentid { get; set; }
        public string code { get; set; }
        //public string AccountTypeInString { get; set; }
        public string Note { get; set; }
        public int CRGID { get; set; }
        public IEnumerable<TCapitalResource> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}
