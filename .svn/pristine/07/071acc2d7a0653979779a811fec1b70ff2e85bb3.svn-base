using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ESimSol.Services.Services
{
    public class NonNegativeLedgerService : MarshalByRefObject, INonNegativeLedgerService
    {
        #region Private functions and declaration

        private NonNegativeLedger MapObject(NullHandler oReader)
        {
            NonNegativeLedger oNonNegativeLedger = new NonNegativeLedger();
            oNonNegativeLedger.NonNegativeLedgerID = oReader.GetInt32("NonNegativeLedgerID");
            oNonNegativeLedger.BUID = oReader.GetInt32("BUID");
            oNonNegativeLedger.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oNonNegativeLedger.Remarks = oReader.GetString("Remarks");
            oNonNegativeLedger.DBUserID = oReader.GetInt32("DBUserID");           
            oNonNegativeLedger.BUName = oReader.GetString("BUName");
            oNonNegativeLedger.AccountCode = oReader.GetString("AccountCode");
            oNonNegativeLedger.AccountHeadName = oReader.GetString("AccountHeadName");
            oNonNegativeLedger.CategoryName = oReader.GetString("ParentHeadName");
            oNonNegativeLedger.UserName = oReader.GetString("EntryUserName");
            return oNonNegativeLedger;
        }
        private NonNegativeLedger CreateObject(NullHandler oReader)
        {
            NonNegativeLedger oNonNegativeLedger = new NonNegativeLedger();
            oNonNegativeLedger = MapObject(oReader);
            return oNonNegativeLedger;
        }

        private List<NonNegativeLedger> CreateObjects(IDataReader oReader)
        {
            List<NonNegativeLedger> oNonNegativeLedger = new List<NonNegativeLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                NonNegativeLedger oItem = CreateObject(oHandler);
                oNonNegativeLedger.Add(oItem);
            }
            return oNonNegativeLedger;
        }

        #endregion

        #region Interface implementation

        public NonNegativeLedgerService() { }

        public NonNegativeLedger Save(NonNegativeLedger oNonNegativeLedger, Int64 nUserID)
        {
            TransactionContext tc = null;
            oNonNegativeLedger.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oNonNegativeLedger.NonNegativeLedgerID <= 0)
                {
                    reader = NonNegativeLedgerDA.InsertUpdate(tc, oNonNegativeLedger, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = NonNegativeLedgerDA.InsertUpdate(tc, oNonNegativeLedger, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNonNegativeLedger = new NonNegativeLedger();
                    oNonNegativeLedger = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oNonNegativeLedger = new NonNegativeLedger();
                oNonNegativeLedger.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oNonNegativeLedger;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                NonNegativeLedger oNonNegativeLedger = new NonNegativeLedger();
                oNonNegativeLedger.NonNegativeLedgerID = id;
                NonNegativeLedgerDA.Delete(tc, oNonNegativeLedger, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data deleted successfully";
        }

        
        public List<NonNegativeLedger> Gets(string sSQL, Int64 nUserID)
        {
            List<NonNegativeLedger> oNonNegativeLedger = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = NonNegativeLedgerDA.Gets(tc, sSQL);
                oNonNegativeLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Non Negative Ledger", e);
                #endregion
            }
            return oNonNegativeLedger;
        }

        #endregion
    }
}
