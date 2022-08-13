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
    public class SelfServiceClearanceService : MarshalByRefObject, ISelfServiceClearanceService
    {
        #region Private functions and declaration
        private SelfServiceClearance MapObject(NullHandler oReader)
        {
            SelfServiceClearance oSelfServiceClearance = new SelfServiceClearance();

            oSelfServiceClearance.EmployeeSettlementID = oReader.GetInt32("EmployeeSettlementID");
            oSelfServiceClearance.EmployeeID = oReader.GetInt32("EmployeeID");
            oSelfServiceClearance.ESCID = oReader.GetInt32("ESCID");
            oSelfServiceClearance.EmployeeCode = oReader.GetString("EmployeeCode");
            oSelfServiceClearance.EmployeeName = oReader.GetString("EmployeeName");
            oSelfServiceClearance.Reason = oReader.GetString("Reason");
            oSelfServiceClearance.SubmissionDate = oReader.GetDateTime("SubmissionDate");
            oSelfServiceClearance.LastWorkingDate = oReader.GetDateTime("LastWorkingDate");
            oSelfServiceClearance.SettlementType = (EnumSettleMentType)oReader.GetInt16("SettlementType");
            oSelfServiceClearance.CurrentStatus = (EnumESCrearance)oReader.GetInt16("CurrentStatus");
            oSelfServiceClearance.Note = oReader.GetString("Note");

            return oSelfServiceClearance;

        }

        private SelfServiceClearance CreateObject(NullHandler oReader)
        {
            SelfServiceClearance oSelfServiceClearance = MapObject(oReader);
            return oSelfServiceClearance;
        }

        private List<SelfServiceClearance> CreateObjects(IDataReader oReader)
        {
            List<SelfServiceClearance> oSelfServiceClearances = new List<SelfServiceClearance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SelfServiceClearance oItem = CreateObject(oHandler);
                oSelfServiceClearances.Add(oItem);
            }
            return oSelfServiceClearances;
        }

        #endregion

        #region Interface implementation
        public SelfServiceClearanceService() { }
        public List<SelfServiceClearance> Gets(int nEmployeeID, Int64 nUserID)
        {
            List<SelfServiceClearance> oSelfServiceClearances = new List<SelfServiceClearance>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SelfServiceClearanceDA.Gets(nEmployeeID, tc);
                oSelfServiceClearances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                 oSelfServiceClearances = new List<SelfServiceClearance>();
                SelfServiceClearance oSelfServiceClearance = new SelfServiceClearance();
                oSelfServiceClearance.ErrorMessage = e.Message;
                #endregion
            }
            return oSelfServiceClearances;
        }

        #endregion
    }
}
