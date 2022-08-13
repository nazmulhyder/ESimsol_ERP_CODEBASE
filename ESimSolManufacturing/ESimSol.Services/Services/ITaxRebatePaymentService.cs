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
    public class ITaxRebatePaymentService : MarshalByRefObject, IITaxRebatePaymentService
    {
        #region Private functions and declaration
        private ITaxRebatePayment MapObject(NullHandler oReader)
        {
            ITaxRebatePayment oITaxRebatePayment = new ITaxRebatePayment();

            oITaxRebatePayment.ITaxRebatePaymentID = oReader.GetInt32("ITaxRebatePaymentID");
            oITaxRebatePayment.ITaxAssessmentYearID = oReader.GetInt32("ITaxAssessmentYearID");
            oITaxRebatePayment.EmployeeID = oReader.GetInt32("EmployeeID");
            oITaxRebatePayment.ITaxRebateItemID = oReader.GetInt32("ITaxRebateItemID");
            oITaxRebatePayment.Amount = oReader.GetDouble("Amount");
            oITaxRebatePayment.Note = oReader.GetString("Note");
            oITaxRebatePayment.AssessmentYear = oReader.GetString("AssessmentYear");
            oITaxRebatePayment.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oITaxRebatePayment.EmployeeOfficial = oReader.GetString("EmployeeOfficial");
            oITaxRebatePayment.Description = oReader.GetString("Description");
            oITaxRebatePayment.ITaxRebateType = (EnumITaxRebateType)oReader.GetInt16("ITaxRebateType");

            return oITaxRebatePayment;

        }

        private ITaxRebatePayment CreateObject(NullHandler oReader)
        {
            ITaxRebatePayment oITaxRebatePayment = MapObject(oReader);
            return oITaxRebatePayment;
        }

        private List<ITaxRebatePayment> CreateObjects(IDataReader oReader)
        {
            List<ITaxRebatePayment> oITaxRebatePayments = new List<ITaxRebatePayment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxRebatePayment oItem = CreateObject(oHandler);
                oITaxRebatePayments.Add(oItem);
            }
            return oITaxRebatePayments;
        }

        #endregion

        #region Interface implementation
        public ITaxRebatePaymentService() { }

        public ITaxRebatePayment IUD(ITaxRebatePayment oITaxRebatePayment, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxRebatePaymentDA.IUD(tc, oITaxRebatePayment, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebatePayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oITaxRebatePayment = new ITaxRebatePayment();
                    oITaxRebatePayment.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebatePayment = new ITaxRebatePayment();
                oITaxRebatePayment.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oITaxRebatePayment;
        }


        public ITaxRebatePayment Get(int nITaxRebatePaymentID, Int64 nUserId)
        {
            ITaxRebatePayment oITaxRebatePayment = new ITaxRebatePayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebatePaymentDA.Get(nITaxRebatePaymentID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebatePayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxRebatePayment.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebatePayment;
        }

        public ITaxRebatePayment Get(string sSQL, Int64 nUserId)
        {
            ITaxRebatePayment oITaxRebatePayment = new ITaxRebatePayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxRebatePaymentDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxRebatePayment = CreateObject(oReader);
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

                oITaxRebatePayment.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxRebatePayment;
        }

        public List<ITaxRebatePayment> Gets(Int64 nUserID)
        {
            List<ITaxRebatePayment> oITaxRebatePayments = new List<ITaxRebatePayment>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebatePaymentDA.Gets(tc);
                oITaxRebatePayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxRebatePayment oITaxRebatePayment = new ITaxRebatePayment();
                oITaxRebatePayment.ErrorMessage = e.Message;
                oITaxRebatePayments.Add(oITaxRebatePayment);
                #endregion
            }
            return oITaxRebatePayments;
        }

        public List<ITaxRebatePayment> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxRebatePayment> oITaxRebatePayments = new List<ITaxRebatePayment>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxRebatePaymentDA.Gets(sSQL, tc);
                oITaxRebatePayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxRebatePayment oITaxRebatePayment = new ITaxRebatePayment();
                oITaxRebatePayment.ErrorMessage = e.Message;
                oITaxRebatePayments.Add(oITaxRebatePayment);
                #endregion
            }
            return oITaxRebatePayments;
        }

        #endregion


    }
}
