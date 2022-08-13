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

    public class AHOpeningBreakdownService : MarshalByRefObject, IAHOpeningBreakdownService
    {
        #region Private functions and declaration
        private AHOpeningBreakdown MapObject(NullHandler oReader)
        {
            AHOpeningBreakdown oAHOpeningBreakdown = new AHOpeningBreakdown();
            oAHOpeningBreakdown.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oAHOpeningBreakdown.IsDebit = oReader.GetBoolean("IsDebit");
            oAHOpeningBreakdown.IsDrClosing = oReader.GetBoolean("IsDrClosing");
            oAHOpeningBreakdown.ComponentID = oReader.GetInt32("ComponentID");
            oAHOpeningBreakdown.CCID = oReader.GetInt32("CCID");
            oAHOpeningBreakdown.VoucherBillID = oReader.GetInt32("VoucherBillID");
            oAHOpeningBreakdown.ProductID = oReader.GetInt32("ProductID");
            oAHOpeningBreakdown.OrderID = oReader.GetInt32("OrderID");
            oAHOpeningBreakdown.BreakdownType = (EnumBreakdownType)oReader.GetInt16("BreakdownType");
            oAHOpeningBreakdown.BreakodwnID = oReader.GetInt32("BreakodwnID");
            oAHOpeningBreakdown.BreakdownName = oReader.GetString("BreakdownName");
            oAHOpeningBreakdown.AccountHeadName = oReader.GetString("AccountHeadName");
            oAHOpeningBreakdown.AccountHeadCode = oReader.GetString("AccountHeadCode");

            oAHOpeningBreakdown.OpeningAmount = oReader.GetDouble("OpeningAmount");
            oAHOpeningBreakdown.ClosingAmount = oReader.GetDouble("ClosingAmount");
            oAHOpeningBreakdown.DebitOpeningAmount = oReader.GetDouble("DebitOpeningAmount");
            oAHOpeningBreakdown.CreditOpeningAmount = oReader.GetDouble("CreditOpeningAmount");
            oAHOpeningBreakdown.DebitAmount = oReader.GetDouble("DebitAmount");
            oAHOpeningBreakdown.CreditAmount = oReader.GetDouble("CreditAmount");
            return oAHOpeningBreakdown;
        }

        private AHOpeningBreakdown CreateObject(NullHandler oReader)
        {
            AHOpeningBreakdown oAHOpeningBreakdown = new AHOpeningBreakdown();
            oAHOpeningBreakdown = MapObject(oReader);
            return oAHOpeningBreakdown;
        }

        private List<AHOpeningBreakdown> CreateObjects(IDataReader oReader)
        {
            List<AHOpeningBreakdown> oAHOpeningBreakdown = new List<AHOpeningBreakdown>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AHOpeningBreakdown oItem = CreateObject(oHandler);
                oAHOpeningBreakdown.Add(oItem);
            }
            return oAHOpeningBreakdown;
        }

        #endregion

        #region Interface implementation
        public AHOpeningBreakdownService() { }
        public List<AHOpeningBreakdown> Gets(int nBUID, int nAccountHeadID, int nCurrencyID, DateTime StartDate, bool bIsApproved, int nUserId)
        {
            List<AHOpeningBreakdown> oAHOpeningBreakdowns = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AHOpeningBreakdownDA.Gets(tc, nBUID, nAccountHeadID, nCurrencyID, StartDate, bIsApproved);
                oAHOpeningBreakdowns = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AHOpeningBreakdown", e);
                #endregion
            }

            return oAHOpeningBreakdowns;
        }

        #endregion
    } 
    
    
}
