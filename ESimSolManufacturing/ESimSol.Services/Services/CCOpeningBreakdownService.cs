using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{

    public class CCOpeningBreakdownService : MarshalByRefObject, ICCOpeningBreakdownService
    {
        #region Private functions and declaration
        private CCOpeningBreakdown MapObject(NullHandler oReader)
        {
            CCOpeningBreakdown oCCOpeningBreakdown = new CCOpeningBreakdown();
            oCCOpeningBreakdown.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oCCOpeningBreakdown.IsDebit = oReader.GetBoolean("IsDebit");
            oCCOpeningBreakdown.IsDrClosing = oReader.GetBoolean("IsDrClosing");
            oCCOpeningBreakdown.ComponentID = oReader.GetInt32("ComponentID");
            oCCOpeningBreakdown.CCID = oReader.GetInt32("CCID");
            oCCOpeningBreakdown.VoucherBillID = oReader.GetInt32("VoucherBillID");
            oCCOpeningBreakdown.ProductID = oReader.GetInt32("ProductID");
            oCCOpeningBreakdown.OrderID = oReader.GetInt32("OrderID");
            oCCOpeningBreakdown.BreakdownType = (EnumBreakdownType)oReader.GetInt16("BreakdownType");
            oCCOpeningBreakdown.BreakodwnID = oReader.GetInt32("BreakodwnID");
            oCCOpeningBreakdown.BreakdownName = oReader.GetString("BreakdownName");
            oCCOpeningBreakdown.AccountHeadName = oReader.GetString("AccountHeadName");
            oCCOpeningBreakdown.AccountHeadCode = oReader.GetString("AccountHeadCode");
            oCCOpeningBreakdown.CCName = oReader.GetString("CCName");
            oCCOpeningBreakdown.CCCode = oReader.GetString("CCCode");

            oCCOpeningBreakdown.OpeningAmount = oReader.GetDouble("OpeningAmount");
            oCCOpeningBreakdown.ClosingAmount = oReader.GetDouble("ClosingAmount");
            oCCOpeningBreakdown.DebitOpeningAmount = oReader.GetDouble("DebitOpeningAmount");
            oCCOpeningBreakdown.CreditOpeningAmount = oReader.GetDouble("CreditOpeningAmount");
            oCCOpeningBreakdown.DebitAmount = oReader.GetDouble("DebitAmount");
            oCCOpeningBreakdown.CreditAmount = oReader.GetDouble("CreditAmount");
            return oCCOpeningBreakdown;
        }

        private CCOpeningBreakdown CreateObject(NullHandler oReader)
        {
            CCOpeningBreakdown oCCOpeningBreakdown = new CCOpeningBreakdown();
            oCCOpeningBreakdown = MapObject(oReader);
            return oCCOpeningBreakdown;
        }

        private List<CCOpeningBreakdown> CreateObjects(IDataReader oReader)
        {
            List<CCOpeningBreakdown> oCCOpeningBreakdown = new List<CCOpeningBreakdown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CCOpeningBreakdown oItem = CreateObject(oHandler);
                oCCOpeningBreakdown.Add(oItem);
            }
            return oCCOpeningBreakdown;
        }

        #endregion

        #region Interface implementation
        public CCOpeningBreakdownService() { }
        public List<CCOpeningBreakdown> Gets(string BUIDs, int nCCID, int nAccountHeadID, int nCurrencyID, DateTime StartDate, bool bIsApproved, int nUserId)
        {
            List<CCOpeningBreakdown> oCCOpeningBreakdowns = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CCOpeningBreakdownDA.Gets(tc, BUIDs, nCCID, nAccountHeadID, nCurrencyID, StartDate, bIsApproved);
                oCCOpeningBreakdowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CCOpeningBreakdown", e);
                #endregion
            }

            return oCCOpeningBreakdowns;
        }

        #endregion
    } 
    
    
}
