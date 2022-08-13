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
    public class EmployeeLeaveTransferService : MarshalByRefObject, IEmployeeLeaveTransferService
    {
        #region Private functions and declaration
        private EmployeeLeaveTransfer MapObject(NullHandler oReader)
        {
            EmployeeLeaveTransfer oELT = new EmployeeLeaveTransfer();
            oELT.LeaveTransferID = oReader.GetInt32("LeaveTransferID");
            oELT.EmployeeID = oReader.GetInt32("EmployeeID");
            oELT.ELLIDFrom = oReader.GetInt32("ELLIDFrom");
            oELT.ELLIDTo = oReader.GetInt32("ELLIDTo");
            oELT.TransferDays = oReader.GetInt32("TransferDays");
            oELT.Note = oReader.GetString("Note");
            oELT.TransferFrom = oReader.GetString("TransferFrom");
            oELT.TransferTo = oReader.GetString("TransferTo");
            return oELT;
        }

        private EmployeeLeaveTransfer CreateObject(NullHandler oReader)
        {
            EmployeeLeaveTransfer oELT = MapObject(oReader);
            return oELT;
        }

        private List<EmployeeLeaveTransfer> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLeaveTransfer> oELT = new List<EmployeeLeaveTransfer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLeaveTransfer oItem = CreateObject(oHandler);
                oELT.Add(oItem);
            }
            return oELT;
        }

        #endregion

        #region Interface implementation
        public EmployeeLeaveTransferService() { }

        public EmployeeLeaveTransfer Get(int nLeaveTransferID, Int64 nUserId)
        {
            EmployeeLeaveTransfer oELT = new EmployeeLeaveTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLeaveTransferDA.Get(tc, nLeaveTransferID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oELT = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELT = new EmployeeLeaveTransfer();
                oELT.ErrorMessage = ex.Message;
                #endregion
            }

            return oELT;
        }

        public List<EmployeeLeaveTransfer> Gets(int nEmpLeaveLedgerID, Int64 nUserID)
        {
            List<EmployeeLeaveTransfer> oELTs = new List<EmployeeLeaveTransfer>();
            EmployeeLeaveTransfer oELT = new EmployeeLeaveTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveTransferDA.Gets(tc, nEmpLeaveLedgerID);
                oELTs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELT.ErrorMessage = ex.Message;
                oELTs.Add(oELT);
                #endregion
            }

            return oELTs;
        }

        public List<EmployeeLeaveTransfer> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeLeaveTransfer> oELTs = new List<EmployeeLeaveTransfer>();
            EmployeeLeaveTransfer oELT = new EmployeeLeaveTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveTransferDA.Gets(tc, sSQL);
                oELTs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oELT.ErrorMessage = ex.Message;
                oELTs.Add(oELT);
                #endregion
            }

            return oELTs;
        }

        #endregion
    }
}
