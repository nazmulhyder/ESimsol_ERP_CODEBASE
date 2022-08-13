using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;

namespace ESimSol.Services.Services
{
    public class ITaxLedgerSalaryHeadService : MarshalByRefObject, IITaxLedgerSalaryHeadService
    {
        #region Private functions and declaration
        private ITaxLedgerSalaryHead MapObject(NullHandler oReader)
        {
            ITaxLedgerSalaryHead oITaxLedgerSalaryHead = new ITaxLedgerSalaryHead();

            oITaxLedgerSalaryHead.ITaxLSHID = oReader.GetInt32("ITaxLSHID");
            oITaxLedgerSalaryHead.ITaxLedgerID = oReader.GetInt32("ITaxLedgerID");
            oITaxLedgerSalaryHead.ITaxHeadConfigureID = oReader.GetInt32("ITaxHeadConfigureID");
            oITaxLedgerSalaryHead.TaxableAmount = oReader.GetDouble("TaxableAmount");
            oITaxLedgerSalaryHead.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oITaxLedgerSalaryHead.SalaryHeadAmount = oReader.GetDouble("SalaryHeadAmount");
            oITaxLedgerSalaryHead.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            return oITaxLedgerSalaryHead;

        }

        private ITaxLedgerSalaryHead CreateObject(NullHandler oReader)
        {
            ITaxLedgerSalaryHead oITaxLedgerSalaryHead = MapObject(oReader);
            return oITaxLedgerSalaryHead;
        }

        private List<ITaxLedgerSalaryHead> CreateObjects(IDataReader oReader)
        {
            List<ITaxLedgerSalaryHead> oITaxLedgerSalaryHeads = new List<ITaxLedgerSalaryHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxLedgerSalaryHead oItem = CreateObject(oHandler);
                oITaxLedgerSalaryHeads.Add(oItem);
            }
            return oITaxLedgerSalaryHeads;
        }

        #endregion

        #region Interface implementation
        public ITaxLedgerSalaryHeadService() { }

        public ITaxLedgerSalaryHead Get(int nITaxLSHID, Int64 nUserId)
        {
            ITaxLedgerSalaryHead oITaxLedgerSalaryHead = new ITaxLedgerSalaryHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxLedgerSalaryHeadDA.Get(nITaxLSHID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxLedgerSalaryHead = CreateObject(oReader);
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

                oITaxLedgerSalaryHead.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxLedgerSalaryHead;
        }

        public ITaxLedgerSalaryHead Get(string sSQL, Int64 nUserId)
        {
            ITaxLedgerSalaryHead oITaxLedgerSalaryHead = new ITaxLedgerSalaryHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxLedgerSalaryHeadDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxLedgerSalaryHead = CreateObject(oReader);
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

                oITaxLedgerSalaryHead.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxLedgerSalaryHead;
        }

        public List<ITaxLedgerSalaryHead> Gets(Int64 nUserID)
        {
            List<ITaxLedgerSalaryHead> oITaxLedgerSalaryHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxLedgerSalaryHeadDA.Gets(tc);
                oITaxLedgerSalaryHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxLedgerSalaryHead", e);
                #endregion
            }
            return oITaxLedgerSalaryHead;
        }

        public List<ITaxLedgerSalaryHead> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxLedgerSalaryHead> oITaxLedgerSalaryHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxLedgerSalaryHeadDA.Gets(sSQL, tc);
                oITaxLedgerSalaryHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ITaxLedgerSalaryHead", e);
                #endregion
            }
            return oITaxLedgerSalaryHead;
        }

        #endregion


    }
}
