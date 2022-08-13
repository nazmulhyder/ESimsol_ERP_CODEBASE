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
    public class EmployeeSettlementClearanceSetupService : MarshalByRefObject, IEmployeeSettlementClearanceSetupService
    {
        #region Private functions and declaration
        private EmployeeSettlementClearanceSetup MapObject(NullHandler oReader)
        {
            EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = new EmployeeSettlementClearanceSetup();

            oEmployeeSettlementClearanceSetup.ESCSetupID = oReader.GetInt32("ESCSetupID");
            oEmployeeSettlementClearanceSetup.ESCSID = oReader.GetInt32("ESCSID");
            oEmployeeSettlementClearanceSetup.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSettlementClearanceSetup.InActiveDate = oReader.GetDateTime("InActiveDate");
            oEmployeeSettlementClearanceSetup.SectionName = oReader.GetString("SectionName");
            oEmployeeSettlementClearanceSetup.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeSettlementClearanceSetup.EmployeeCode = oReader.GetString("EmployeeCode");

            return oEmployeeSettlementClearanceSetup;

        }

        private EmployeeSettlementClearanceSetup CreateObject(NullHandler oReader)
        {
            EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = MapObject(oReader);
            return oEmployeeSettlementClearanceSetup;
        }

        private List<EmployeeSettlementClearanceSetup> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlementClearanceSetup> oEmployeeSettlementClearanceSetups = new List<EmployeeSettlementClearanceSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlementClearanceSetup oItem = CreateObject(oHandler);
                oEmployeeSettlementClearanceSetups.Add(oItem);
            }
            return oEmployeeSettlementClearanceSetups;
        }

        #endregion

        #region Interface implementation
        public EmployeeSettlementClearanceSetupService() { }

        public EmployeeSettlementClearanceSetup IUD(EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSettlementClearanceSetupDA.IUD(tc, oEmployeeSettlementClearanceSetup, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSettlementClearanceSetup.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSettlementClearanceSetup.ESCSetupID = 0;
                #endregion
            }
            return oEmployeeSettlementClearanceSetup;
        }


        public EmployeeSettlementClearanceSetup Get(int nESCSetupID, Int64 nUserId)
        {
            EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = new EmployeeSettlementClearanceSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementClearanceSetupDA.Get(nESCSetupID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);

                oEmployeeSettlementClearanceSetup.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlementClearanceSetup;
        }

        public EmployeeSettlementClearanceSetup Get(string sSQL, Int64 nUserId)
        {
            EmployeeSettlementClearanceSetup oEmployeeSettlementClearanceSetup = new EmployeeSettlementClearanceSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementClearanceSetupDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);

                oEmployeeSettlementClearanceSetup.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlementClearanceSetup;
        }

        public List<EmployeeSettlementClearanceSetup> Gets(Int64 nUserID)
        {
            List<EmployeeSettlementClearanceSetup> oEmployeeSettlementClearanceSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementClearanceSetupDA.Gets(tc);
                oEmployeeSettlementClearanceSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearanceSetup", e);
                #endregion
            }
            return oEmployeeSettlementClearanceSetup;
        }

        public List<EmployeeSettlementClearanceSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlementClearanceSetup> oEmployeeSettlementClearanceSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementClearanceSetupDA.Gets(sSQL, tc);
                oEmployeeSettlementClearanceSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearanceSetup", e);
                #endregion
            }
            return oEmployeeSettlementClearanceSetup;
        }

        #endregion


    }
}
