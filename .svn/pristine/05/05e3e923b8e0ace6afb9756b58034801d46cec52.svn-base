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
    #region BusinessUnit
    public class BusinessUnit : BusinessObject
    {
        public BusinessUnit()
        {
            BusinessUnitID = 0;
            Code = "";
            Name = "";
            ShortName = "";
            RegistrationNo = "";
            TINNo = "";
            VatNo = "";
            BusinessNature = EnumBusinessNature.None;
            LegalFormat = EnumLegalFormat.None;
            Address = "";
            Phone = "";
            Email = "";
            WebAddress = "";
            Note = "";
            IsAreaEffect = false;
            IsZoneEffect = false;
            IsSiteEffect = false;
            NameCode = "";
            BusinessUnitType = EnumBusinessUnitType.None;
            BUImage = null;
            ErrorMessage = "";
            BUImageSt = "";
            BULogo = null;

            NameInBangla = "";
            AddressInBangla = "";
            FaxNo = "";

            //PrintName = "";
        }
        #region Properties
        public int BusinessUnitID { get; set; }
        public string Code { get; set; }
        public string NameInBangla { get; set; }
        public string AddressInBangla { get; set; }
        public string FaxNo { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }        
        public string RegistrationNo { get; set; }
        public string TINNo { get; set; }
        public string VatNo { get; set; }
        public EnumBusinessNature BusinessNature { get; set; }
        public EnumLegalFormat LegalFormat { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WebAddress { get; set; }
        public string Note { get; set; }
        public bool IsAreaEffect { get; set; }
        public bool IsZoneEffect { get; set; }
        public bool IsSiteEffect { get; set; }
        public bool IsPrintHeader { get; set; }
        public string NameCode { get; set; }
        public TLocation TLocation { get; set; }
        public EnumBusinessUnitType BusinessUnitType { get; set; }
        public int BusinessUnitTypeInInt { get; set; }
        public byte[] BUImage { get; set; }
        public string BUImageSt { get; set; }
        public string ErrorMessage { get; set; }
        public System.Drawing.Image BULogo { get; set; }
        #endregion

        #region Derived Properties
        
        public List<EnumObject> BusinessNatures { get; set; }
        public List<EnumObject> LegalFormats { get; set; }
        public List<EnumObject> BusinessUnitTypeObjs { get; set; }
        public int BusinessNatureInInt { get { return (int)this.BusinessNature; } }
        public int LegalFormatInInt { get { return (int)this.LegalFormat; } }
        public string BusinessNatureInString { get { return EnumObject.jGet(this.BusinessNature); } }
        public string LegalFormatInString { get { return EnumObject.jGet(this.LegalFormat); } }
        public string IsAreaEffectSt { get { return this.IsAreaEffect ? "Area Included" : "Area Excluded"; } }
        public string IsZoneEffectSt { get { return this.IsZoneEffect ? "Zone Included" : "Zone Excluded"; } }
        public string IsSiteEffectSt { get { return this.IsSiteEffect ? "Site Included" : "Site Excluded"; } }
        public string BUTypeSt
        {
            get
            {
                return this.ShortName+" ("+EnumObject.jGet(this.BusinessUnitType)+")";
            }
        }
        public string ShortNameCode
        {
            get
            {
                return this.ShortName + " [" + this.Code + "]";
            }
        }
        private string _sPringReportHead = "";
        public string PringReportHead
        {
            get
            {
                _sPringReportHead = "";
                if (this.Address==null)
                {
                    return _sPringReportHead;
                }

                if (this.Address.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + "" + this.Address;
                }
                if (this.Phone.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + "\nTel: " + this.Phone;
                }
               
                if (this.Email != null && this.Email.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + "\ne-mail: " + this.Email;
                }
                if (this.WebAddress != null && this.WebAddress.Length > 0)
                {
                    _sPringReportHead = _sPringReportHead + " " + this.WebAddress;
                }

                return _sPringReportHead;
            }

        }

        #endregion

        #region Functions
        public BusinessUnit Get(int id, int nUserID)
        {
            return BusinessUnit.Service.Get(id, nUserID);
        }
        public BusinessUnit GetWithImage(int id, int nUserID)
        {
            return BusinessUnit.Service.GetWithImage(id, nUserID);
        }
        public BusinessUnit GetByType(int nBUType, int nUserID)
        {
            return BusinessUnit.Service.GetByType(nBUType, nUserID);
        }
        public BusinessUnit Save(int nUserID)
        {
            return BusinessUnit.Service.Save(this, nUserID);
        }
        public BusinessUnit UpdateImage(int nUserID)
        {
            return BusinessUnit.Service.UpdateImage(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return BusinessUnit.Service.Delete(id, nUserID);
        }
        public static List<BusinessUnit> Gets(int nUserID)
        {
            return BusinessUnit.Service.Gets(nUserID);
        }        
        public static List<BusinessUnit> GetsBUByCodeOrName(string sCodeOrName, int nUserID)
        {
            return BusinessUnit.Service.GetsBUByCodeOrName(sCodeOrName, nUserID);
        }
        public static List<BusinessUnit> GetsBUByCodeOrNameAndAccountHead(string sCodeOrName, int nLedgerID, int nUserID)
        {
            return BusinessUnit.Service.GetsBUByCodeOrNameAndAccountHead(sCodeOrName, nLedgerID, nUserID);
        }
        public static List<BusinessUnit> Gets(string sSQL, int nUserID)
        {
            return BusinessUnit.Service.Gets(sSQL, nUserID);
        }
        public static List<BusinessUnit> GetsPermittedBU(User oUser, int nUserID)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            if (oUser.FinancialUserType == EnumFinancialUserType.GroupAccounts || oUser.UserID == -9)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                List<BusinessUnit> oTempBusinessUnits = new List<BusinessUnit>();
                oBusinessUnit.BusinessUnitID = 0;
                oBusinessUnit.Code = "00";
                oBusinessUnit.Name = "Group Accounts";
                oBusinessUnit.ShortName = "Group Accounts";
                oBusinessUnits.Add(oBusinessUnit);
                oTempBusinessUnits = BusinessUnit.Service.Gets(nUserID);
                oBusinessUnits.AddRange(oTempBusinessUnits);
            }
            else if (oUser.FinancialUserType == EnumFinancialUserType.IndividualAccounts)
            {
                oBusinessUnits = BusinessUnit.Service.GetsPermittedBU(nUserID);
            }
            return oBusinessUnits;
        }
        public static bool IsPermittedBU(int nBUID, int nUserID)
        {
            if (nUserID == -9)
            {
                return true;
            }
            else
            {
                return BusinessUnit.Service.IsPermittedBU(nBUID, nUserID);
            }
        }
        #endregion

        #region Non DB Function
        public static string IDInString(List<BusinessUnit> oBusinessUnit)
        {
            string sReturn = "";
            if (oBusinessUnit != null)
            {
                foreach (BusinessUnit oItem in oBusinessUnit)
                {
                    sReturn = sReturn + oItem.BusinessUnitID.ToString() + ",";
                }
                if (sReturn == "") return "";
                sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            }
            return sReturn;
        }
        public static int GetIndex(List<BusinessUnit> oBusinessUnits, int nBusinessUnitID)
        {
            int index = -1, i = 0;

            foreach (BusinessUnit oItem in oBusinessUnits)
            {
                if (oItem.BusinessUnitID == nBusinessUnitID)
                {
                    index = i; break;
                }
                i++;
            }
            return index;
        }
        #endregion

        #region ServiceFactory
        internal static IBusinessUnitService Service
        {
            get { return (IBusinessUnitService)Services.Factory.CreateService(typeof(IBusinessUnitService)); }
        }
        #endregion
    }
    #endregion


    #region IBusinessUnit interface
    public interface IBusinessUnitService
    {
        BusinessUnit Get(int id, int nUserID);
        BusinessUnit GetWithImage(int id, int nUserID);
        BusinessUnit GetByType(int id, int nBUType);
        List<BusinessUnit> Gets(int nUserID);
        List<BusinessUnit> GetsPermittedBU(int nUserID);
        List<BusinessUnit> GetsBUByCodeOrNameAndAccountHead(string sCodeOrName, int nAccountHeadID, int nUserID);
        List<BusinessUnit> GetsBUByCodeOrName(string sCodeOrName, int nUserID);
        string Delete(int id, int nUserID);
        BusinessUnit Save(BusinessUnit oBusinessUnit, int nUserID);
        BusinessUnit UpdateImage(BusinessUnit oBusinessUnit, int nUserID);
        List<BusinessUnit> Gets(string sSQL, int nUserID);
        bool IsPermittedBU(int nBUID, int nUserID);
    }
    #endregion
}