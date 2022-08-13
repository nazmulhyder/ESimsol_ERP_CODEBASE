using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region LCContact
    public class LCContact : BusinessObject
    {
        public LCContact()
        {
            LCContactID = 0;
            BUID = 0;
            BalanceDate = DateTime.Now;
            LCInHand = 0;
            ContactInHand = 0;
            Remarks = "";
            ErrorMessage = "";
            BUName = "";
            BUSName = "";
            BusinessUnitID = 0;
            LCContacts = new List<LCContact>();
        }

        #region Properties
        public int LCContactID { get; set; }
        public int BUID { get; set; }   
        public DateTime BalanceDate { get; set; }
        public double LCInHand { get; set; }
        public double ContactInHand { get; set; }
        public double Total {
            get { return LCInHand + ContactInHand; }
        }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public string BUName { get; set; }
        public string BUSName { get; set; }
        public int BusinessUnitID { get; set; }
        public Company Company { get; set; }
        public string BalanceDateSt
        {
            get
            {
                if (BalanceDate.ToString("dd MMM yyyy") == "01 Jan 0001") 
                {
                    return "";
                }
                return this.BalanceDate.ToString("dd MMM yyyy");
            }
        }
        public List<LCContact> LCContacts { get; set; }

        #endregion

        #region Functions
        public LCContact Get(int id, int nUserID)
        {
            return LCContact.Service.Get(id, nUserID);
        }
        public LCContact Save(int nUserID)
        {
            return LCContact.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return LCContact.Service.Delete(id, nUserID);
        }
        public static List<LCContact> Gets(int nUserID)
        {
            return LCContact.Service.Gets(nUserID);
        }
        public List<LCContact> GetsLCContacts(int nUserID)
        {
            return LCContact.Service.GetsLCContacts(this,nUserID);
        }
        public static List<LCContact> Gets(string sSQL, int nUserID)
        {
            return LCContact.Service.Gets(sSQL, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILCContactService Service
        {
            get { return (ILCContactService)Services.Factory.CreateService(typeof(ILCContactService)); }
        }
        #endregion
    }
    #endregion

    #region ILCContact interface
    public interface ILCContactService
    {
        LCContact Get(int id, int nUserID);
        List<LCContact> Gets(int nUserID);
        string Delete(int id, int nUserID);
        LCContact Save(LCContact oLCContact, int nUserID);
        List<LCContact> Gets(string sSQL, int nUserID);
        List<LCContact> GetsLCContacts(LCContact oLCContact, int nUserID);
    }
    #endregion
}
