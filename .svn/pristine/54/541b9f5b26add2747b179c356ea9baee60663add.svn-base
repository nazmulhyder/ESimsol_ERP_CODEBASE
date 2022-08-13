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
    public class EmployeeLoanDisbursementPolicyService : MarshalByRefObject, IEmployeeLoanDisbursementPolicyService
    {
        #region Private functions and declaration
        private EmployeeLoanDisbursementPolicy MapObject(NullHandler oReader)
        {
            EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy = new EmployeeLoanDisbursementPolicy();

            oEmployeeLoanDisbursementPolicy.ELDPID = oReader.GetInt32("ELDPID");
            oEmployeeLoanDisbursementPolicy.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoanDisbursementPolicy.Amount = oReader.GetDouble("Amount");
            oEmployeeLoanDisbursementPolicy.ReceivableAmount = oReader.GetDouble("ReceivableAmount");
            oEmployeeLoanDisbursementPolicy.IsDisbursed = oReader.GetBoolean("IsDisbursed");
            oEmployeeLoanDisbursementPolicy.ExpectedDisburseDate = oReader.GetDateTime("ExpectedDisburseDate");
            oEmployeeLoanDisbursementPolicy.ActualDisburseDate = oReader.GetDateTime("ActualDisburseDate");
            return oEmployeeLoanDisbursementPolicy;

        }

        private EmployeeLoanDisbursementPolicy CreateObject(NullHandler oReader)
        {
            EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy = MapObject(oReader);
            return oEmployeeLoanDisbursementPolicy;
        }

        private List<EmployeeLoanDisbursementPolicy> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanDisbursementPolicy> oEmployeeLoanDisbursementPolicys = new List<EmployeeLoanDisbursementPolicy>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanDisbursementPolicy oItem = CreateObject(oHandler);
                oEmployeeLoanDisbursementPolicys.Add(oItem);
            }
            return oEmployeeLoanDisbursementPolicys;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanDisbursementPolicyService() { }

        public EmployeeLoanDisbursementPolicy IUD(EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeLoanDisbursementPolicyDA.IUD(tc, oEmployeeLoanDisbursementPolicy, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeLoanDisbursementPolicy = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeLoanDisbursementPolicy.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeLoanDisbursementPolicy.ELDPID = 0;
                #endregion
            }
            return oEmployeeLoanDisbursementPolicy;
        }


        public EmployeeLoanDisbursementPolicy Get(int nELDPID, Int64 nUserId)
        {
            EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy = new EmployeeLoanDisbursementPolicy();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanDisbursementPolicyDA.Get(nELDPID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanDisbursementPolicy = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeLoanDisbursementPolicy", e);
                oEmployeeLoanDisbursementPolicy.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanDisbursementPolicy;
        }

        public EmployeeLoanDisbursementPolicy Get(string sSQL, Int64 nUserId)
        {
            EmployeeLoanDisbursementPolicy oEmployeeLoanDisbursementPolicy = new EmployeeLoanDisbursementPolicy();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanDisbursementPolicyDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanDisbursementPolicy = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeLoanDisbursementPolicy", e);
                oEmployeeLoanDisbursementPolicy.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanDisbursementPolicy;
        }

        public List<EmployeeLoanDisbursementPolicy> Gets(Int64 nUserID)
        {
            List<EmployeeLoanDisbursementPolicy> oEmployeeLoanDisbursementPolicy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanDisbursementPolicyDA.Gets(tc);
                oEmployeeLoanDisbursementPolicy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLoanDisbursementPolicy", e);
                #endregion
            }
            return oEmployeeLoanDisbursementPolicy;
        }

        public List<EmployeeLoanDisbursementPolicy> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeLoanDisbursementPolicy> oEmployeeLoanDisbursementPolicy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanDisbursementPolicyDA.Gets(sSQL, tc);
                oEmployeeLoanDisbursementPolicy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLoanDisbursementPolicy", e);
                #endregion
            }
            return oEmployeeLoanDisbursementPolicy;
        }

        #endregion


    }
}
