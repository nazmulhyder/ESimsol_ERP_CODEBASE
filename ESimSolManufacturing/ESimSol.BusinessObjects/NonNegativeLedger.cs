using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    public class NonNegativeLedger : BusinessObject
    {
        public NonNegativeLedger()
        {
            NonNegativeLedgerID = 0;
            BUID = 0;
            AccountHeadID = 0;           
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            UserName = "";
            ErrorMessage = "";
            BUName = "";
            AccountCode = "";
            AccountHeadName = "";
            CategoryName = "";
        }

        #region Properties

        public int NonNegativeLedgerID { get; set; }
        public int BUID { get; set; }
        public int AccountHeadID { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        public string ErrorMessage { get; set; }
        public string BUName { get; set; }
        public string AccountCode { get; set; }
        public string AccountHeadName { get; set; }
        public string CategoryName { get; set; }

        #endregion

        #region Functions

        public NonNegativeLedger Save(long nUserID)
        {
            return NonNegativeLedger.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return NonNegativeLedger.Service.Delete(id, nUserID);
        }
        public static List<NonNegativeLedger> Gets(string sSQL, long nUserID)
        {
            return NonNegativeLedger.Service.Gets(sSQL, nUserID);
        }      

        #endregion

        #region NonNegativeLedger Service Factory

        internal static INonNegativeLedgerService Service
        {
            get { return (INonNegativeLedgerService)Services.Factory.CreateService(typeof(INonNegativeLedgerService)); }
        }

        #endregion
    }

    #region IFood interface

    public interface INonNegativeLedgerService
    {
        NonNegativeLedger Save(NonNegativeLedger oNonNegativeLedger, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        List<NonNegativeLedger> Gets(string sSQL, Int64 nUserID);
    }

    #endregion
}
