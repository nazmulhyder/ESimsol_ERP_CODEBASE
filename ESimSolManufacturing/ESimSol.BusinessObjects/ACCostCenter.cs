using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region ACCostCenter
    public class ACCostCenter : BusinessObject
    {
        public ACCostCenter()
        {
            ACCostCenterID = 0;
            Code = "";
            Name = "";
            Description = "";
            ParentID = 0;
            ReferenceType = EnumReferenceType.None;
            ReferenceTypeInt = 0;
            ReferenceObjectID = 0;
            ActivationDate = DateTime.Today;
            ExpireDate = DateTime.Today;
            IsActive = false;            
            CategoryName = "";
            ErrorMessage = "";
            DisplayMessage = "";
            IsChild = true;
            ACCostCenters = new List<ACCostCenter>();
            AccountHeadID = 0;
            IsBillRefApply = false;
            IsChequeApply = false;
            IsOrderRefApply = false;
            NameCode = "";
            VoucherTypeID = 0;
            IsPaymentCheque = false;
            BUID = 0;
            BUName = "";
            DueAmount = 0;
            OverDueDays = 0;
            AccountHeadConfigures = new List<AccountHeadConfigure>();
            BUWiseSubLedgers = new List<BUWiseSubLedger>();
            SubledgerRefConfigs = new List<SubledgerRefConfig>();
            CurrentBalance = "";
        }

        #region Properties
        public int ACCostCenterID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentID { get; set; }
        public EnumReferenceType ReferenceType { get; set; }
        public int ReferenceTypeInt { get; set; }
        public int ReferenceObjectID { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsBillRefApply { get; set; }
        public bool IsChequeApply { get; set; }
        public bool IsOrderRefApply { get; set; }        
        public string CategoryName { get; set; }
         public string CategoryCode { get; set; }
        public string ErrorMessage { get; set; }
        public string DisplayMessage { get; set; }
        public bool IsChild { get; set; }        
        public List<ACCostCenter> ACCostCenters { get; set; }
        public string ActivationDateInString { get { return ActivationDate.ToString("dd MMM yyyy"); } }
        public string ExpireDateInString { get { return ExpireDate.ToString("dd MMM yyyy"); } }
        public string ReferenceTypeSt { get { return EnumObject.jGet(this.ReferenceType); } }
        public string IsBillRefApplySt { get { return IsBillRefApply ? "Applicable" : "Not Applicable"; } }
        public string IsOrderRefApplySt { get { return IsOrderRefApply ? "Applicable" : "Not Applicable"; } }
        public int AccountHeadID { get; set; }
        public string NameCode { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public int VoucherTypeID { get; set; } // only use for check cheque reference 
        public bool IsPaymentCheque { get; set; } // only use for check cheque reference 
        public double DueAmount { get; set; }
        public int OverDueDays { get; set; }
        public string CurrentBalance { get; set; }
        public List<AccountHeadConfigure> AccountHeadConfigures { get; set; }
        public List<BUWiseSubLedger> BUWiseSubLedgers { get; set; }
        public List<SubledgerRefConfig> SubledgerRefConfigs { get; set; }
        public string DueAmountSt
        {
            get
            {
                return Global.MillionFormat(this.DueAmount);
            }
    
    }
        #endregion

        #region Functions
        public static List<ACCostCenter> Gets(int nUserID)
        {
            return ACCostCenter.Service.Gets(nUserID);
        }
        public static List<ACCostCenter> Gets(int nParentID, int nUserID)
        {
            return ACCostCenter.Service.Gets(nParentID, nUserID);
        }
        public static List<ACCostCenter> GetsByConfigure(int nAccountHeadID, string sCCName, int nBUID, int nUserID)
        {
            return ACCostCenter.Service.GetsByConfigure(nAccountHeadID, sCCName, nBUID, nUserID);
        }
        public ACCostCenter Get(int id, int nUserID)
        {
            return ACCostCenter.Service.Get(id, nUserID);
        }
        public ACCostCenter GetByRef(EnumReferenceType eEnumReferenceType, int nReferenceObjectID, int nUserID)
        {
            return ACCostCenter.Service.GetByRef(eEnumReferenceType, nReferenceObjectID, nUserID);
        }
        public static List<ACCostCenter> Gets(string sSQL, int nUserID)
        {
            return ACCostCenter.Service.Gets(sSQL, nUserID);
        }
        public static List<ACCostCenter> GetsByCodeOrName(ACCostCenter oACCostCenter, int nUserID, int nBUID=0)
        {
            return ACCostCenter.Service.GetsByCodeOrName(oACCostCenter, nBUID, nUserID);
        }
        public static List<ACCostCenter> GetsByCode(ACCostCenter oACCostCenter, int nUserID)
        {
            return ACCostCenter.Service.GetsByCode(oACCostCenter, nUserID);
        }
        public ACCostCenter Save(int nUserID)
        {
            return ACCostCenter.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return ACCostCenter.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IACCostCenterService Service
        {
            get { return (IACCostCenterService)Services.Factory.CreateService(typeof(IACCostCenterService)); }
        }
        #endregion
    }
    #endregion

    #region IACCostCenter interface
     
    public interface IACCostCenterService
    {
        ACCostCenter Get(int id, int nUserID);
        ACCostCenter GetByRef(EnumReferenceType eEnumReferenceType, int nReferenceObjectID, int nUserID);
        string Delete(int id, int nUserID);
        ACCostCenter Save(ACCostCenter oACCostCenter, int nUserID);
        List<ACCostCenter> Gets(string sSQL, int nUserID);
        List<ACCostCenter> Gets(int nUserID);
        List<ACCostCenter> GetsByConfigure(int nAccountHeadID, string sCCName, int nBUID, int nUserID);
        List<ACCostCenter> Gets(int nParentID, int nUserID);
        List<ACCostCenter> GetsByCodeOrName(ACCostCenter oACCostCenter, int nBUID, int nUserID);
        List<ACCostCenter> GetsByCode(ACCostCenter oACCostCenter, int nUserID);
    }
    #endregion

    #region TACCostCentre
    //this is a derive class that use only for display user menu(j tree tree architecture reference : http://www.jeasyui.com/documentation/index.php# )
    public class TACCostCenter
    {
        public TACCostCenter()
        {
            id = 0;
            text = "";
            state = "";
            attributes = "";
            activity = "";
            parentid = 0;
            code = "";
            Description = "";
            IsLastLayer = false;            
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public string activity { get; set; }        //: define activity
        public int parentid { get; set; }
        public string code { get; set; }
        public string Description { get; set; }
        public bool IsLastLayer { get; set; }
        public List<TACCostCenter> children { get; set; }//: an array nodes defines some children nodes
    }
    #endregion
}


