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
    public class EmployeeSettlementClearanceSectionService : MarshalByRefObject, IEmployeeSettlementClearanceSectionService
    {
        #region Private functions and declaration
        private EmployeeSettlementClearanceSection MapObject(NullHandler oReader)
        {
            EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection = new EmployeeSettlementClearanceSection();

            oEmployeeSettlementClearanceSection.ESCSID = oReader.GetInt32("ESCSID");
            oEmployeeSettlementClearanceSection.Name = oReader.GetString("Name");
            return oEmployeeSettlementClearanceSection;

        }

        private EmployeeSettlementClearanceSection CreateObject(NullHandler oReader)
        {
            EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection = MapObject(oReader);
            return oEmployeeSettlementClearanceSection;
        }

        private List<EmployeeSettlementClearanceSection> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlementClearanceSection> oEmployeeSettlementClearanceSections = new List<EmployeeSettlementClearanceSection>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlementClearanceSection oItem = CreateObject(oHandler);
                oEmployeeSettlementClearanceSections.Add(oItem);
            }
            return oEmployeeSettlementClearanceSections;
        }

        #endregion

        #region Interface implementation
        public EmployeeSettlementClearanceSectionService() { }

        public EmployeeSettlementClearanceSection IUD(EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeSettlementClearanceSectionDA.IUD(tc, oEmployeeSettlementClearanceSection, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceSection = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeSettlementClearanceSection.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeSettlementClearanceSection.ESCSID = 0;
                #endregion
            }
            return oEmployeeSettlementClearanceSection;
        }

        public EmployeeSettlementClearanceSection Get(int nESCSID, Int64 nUserId)
        {
            EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection = new EmployeeSettlementClearanceSection();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementClearanceSectionDA.Get(nESCSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceSection = CreateObject(oReader);
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

                oEmployeeSettlementClearanceSection.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlementClearanceSection;
        }

        public EmployeeSettlementClearanceSection Get(string sSQL, Int64 nUserId)
        {
            EmployeeSettlementClearanceSection oEmployeeSettlementClearanceSection = new EmployeeSettlementClearanceSection();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementClearanceSectionDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementClearanceSection = CreateObject(oReader);
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

                oEmployeeSettlementClearanceSection.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeSettlementClearanceSection;
        }

        public List<EmployeeSettlementClearanceSection> Gets(Int64 nUserID)
        {
            List<EmployeeSettlementClearanceSection> oEmployeeSettlementClearanceSection = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementClearanceSectionDA.Gets(tc);
                oEmployeeSettlementClearanceSection = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearanceSection", e);
                #endregion
            }
            return oEmployeeSettlementClearanceSection;
        }

        public List<EmployeeSettlementClearanceSection> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlementClearanceSection> oEmployeeSettlementClearanceSection = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementClearanceSectionDA.Gets(sSQL, tc);
                oEmployeeSettlementClearanceSection = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeSettlementClearanceSection", e);
                #endregion
            }
            return oEmployeeSettlementClearanceSection;
        }

        #endregion


    }
}
