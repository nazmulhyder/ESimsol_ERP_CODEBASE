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
    public class EmployeeLoanInstallmentPolicyService : MarshalByRefObject, IEmployeeLoanInstallmentPolicyService
    {
        #region Private functions and declaration
        private EmployeeLoanInstallmentPolicy MapObject(NullHandler oReader)
        {
            EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy = new EmployeeLoanInstallmentPolicy();

            oEmployeeLoanInstallmentPolicy.ELIPID = oReader.GetInt32("ELIPID");
            oEmployeeLoanInstallmentPolicy.EmployeeLoanID = oReader.GetInt32("EmployeeLoanID");
            oEmployeeLoanInstallmentPolicy.Amount = oReader.GetDouble("Amount");
            oEmployeeLoanInstallmentPolicy.IsRealized = oReader.GetBoolean("IsRealized");
            oEmployeeLoanInstallmentPolicy.ExpectedRealizeDate = oReader.GetDateTime("ExpectedRealizeDate");
            oEmployeeLoanInstallmentPolicy.ActualRealizeDate = oReader.GetDateTime("ActualRealizeDate");
            oEmployeeLoanInstallmentPolicy.RealizeNote = oReader.GetString("RealizeNote");
            return oEmployeeLoanInstallmentPolicy;

        }

        private EmployeeLoanInstallmentPolicy CreateObject(NullHandler oReader)
        {
            EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy = MapObject(oReader);
            return oEmployeeLoanInstallmentPolicy;
        }

        private List<EmployeeLoanInstallmentPolicy> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanInstallmentPolicy> oEmployeeLoanInstallmentPolicys = new List<EmployeeLoanInstallmentPolicy>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanInstallmentPolicy oItem = CreateObject(oHandler);
                oEmployeeLoanInstallmentPolicys.Add(oItem);
            }
            return oEmployeeLoanInstallmentPolicys;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanInstallmentPolicyService() { }

        public EmployeeLoanInstallmentPolicy IUD(EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeLoanInstallmentPolicyDA.IUD(tc, oEmployeeLoanInstallmentPolicy, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeLoanInstallmentPolicy = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeLoanInstallmentPolicy.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeLoanInstallmentPolicy.ELIPID = 0;
                #endregion
            }
            return oEmployeeLoanInstallmentPolicy;
        }


        public EmployeeLoanInstallmentPolicy Get(int nELIPID, Int64 nUserId)
        {
            EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy = new EmployeeLoanInstallmentPolicy();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanInstallmentPolicyDA.Get(nELIPID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanInstallmentPolicy = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeLoanInstallmentPolicy", e);
                oEmployeeLoanInstallmentPolicy.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanInstallmentPolicy;
        }

        public EmployeeLoanInstallmentPolicy Get(string sSQL, Int64 nUserId)
        {
            EmployeeLoanInstallmentPolicy oEmployeeLoanInstallmentPolicy = new EmployeeLoanInstallmentPolicy();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeLoanInstallmentPolicyDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeLoanInstallmentPolicy = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeLoanInstallmentPolicy", e);
                oEmployeeLoanInstallmentPolicy.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeLoanInstallmentPolicy;
        }

        public List<EmployeeLoanInstallmentPolicy> Gets(Int64 nUserID)
        {
            List<EmployeeLoanInstallmentPolicy> oEmployeeLoanInstallmentPolicy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanInstallmentPolicyDA.Gets(tc);
                oEmployeeLoanInstallmentPolicy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLoanInstallmentPolicy", e);
                #endregion
            }
            return oEmployeeLoanInstallmentPolicy;
        }

        public List<EmployeeLoanInstallmentPolicy> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeLoanInstallmentPolicy> oEmployeeLoanInstallmentPolicy = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanInstallmentPolicyDA.Gets(sSQL, tc);
                oEmployeeLoanInstallmentPolicy = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLoanInstallmentPolicy", e);
                #endregion
            }
            return oEmployeeLoanInstallmentPolicy;
        }

        #endregion


    }
}
