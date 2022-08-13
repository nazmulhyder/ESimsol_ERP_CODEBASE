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
    public class ITaxAdvancePaymentService : MarshalByRefObject, IITaxAdvancePaymentService
    {
        #region Private functions and declaration
        private ITaxAdvancePayment MapObject(NullHandler oReader)
        {
            ITaxAdvancePayment oITaxAdvancePayment = new ITaxAdvancePayment();

            oITaxAdvancePayment.ITaxAdvancePaymentID = oReader.GetInt32("ITaxAdvancePaymentID");
            oITaxAdvancePayment.ITaxAssessmentYearID = oReader.GetInt32("ITaxAssessmentYearID");
            oITaxAdvancePayment.EmployeeID = oReader.GetInt32("EmployeeID");
            oITaxAdvancePayment.Amount = oReader.GetDouble("Amount");
            oITaxAdvancePayment.Note = oReader.GetString("Note");
            oITaxAdvancePayment.AssessmentYear = oReader.GetString("AssessmentYear");
            oITaxAdvancePayment.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            oITaxAdvancePayment.EmployeeOfficial = oReader.GetString("EmployeeOfficial");

            return oITaxAdvancePayment;

        }

        private ITaxAdvancePayment CreateObject(NullHandler oReader)
        {
            ITaxAdvancePayment oITaxAdvancePayment = MapObject(oReader);
            return oITaxAdvancePayment;
        }

        private List<ITaxAdvancePayment> CreateObjects(IDataReader oReader)
        {
            List<ITaxAdvancePayment> oITaxAdvancePayments = new List<ITaxAdvancePayment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ITaxAdvancePayment oItem = CreateObject(oHandler);
                oITaxAdvancePayments.Add(oItem);
            }
            return oITaxAdvancePayments;
        }

        #endregion

        #region Interface implementation
        public ITaxAdvancePaymentService() { }

        public ITaxAdvancePayment IUD(ITaxAdvancePayment oITaxAdvancePayment, int nDBOperation, Int64 nUserID)
        {


            TransactionContext tc = null;

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ITaxAdvancePaymentDA.IUD(tc, oITaxAdvancePayment, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oITaxAdvancePayment = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oITaxAdvancePayment = new ITaxAdvancePayment();
                    oITaxAdvancePayment.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxAdvancePayment = new ITaxAdvancePayment();
                oITaxAdvancePayment.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oITaxAdvancePayment;
        }


        public ITaxAdvancePayment Get(int nITaxAdvancePaymentID, Int64 nUserId)
        {
            ITaxAdvancePayment oITaxAdvancePayment = new ITaxAdvancePayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxAdvancePaymentDA.Get(nITaxAdvancePaymentID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxAdvancePayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxAdvancePayment.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxAdvancePayment;
        }

        public ITaxAdvancePayment Get(string sSQL, Int64 nUserId)
        {
            ITaxAdvancePayment oITaxAdvancePayment = new ITaxAdvancePayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ITaxAdvancePaymentDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oITaxAdvancePayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oITaxAdvancePayment.ErrorMessage = e.Message;
                #endregion
            }

            return oITaxAdvancePayment;
        }

        public List<ITaxAdvancePayment> Gets(Int64 nUserID)
        {
            List<ITaxAdvancePayment> oITaxAdvancePayments = new List<ITaxAdvancePayment>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ITaxAdvancePaymentDA.Gets(tc);
                oITaxAdvancePayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxAdvancePayment oITaxAdvancePayment = new ITaxAdvancePayment();
                oITaxAdvancePayment.ErrorMessage = e.Message;
                oITaxAdvancePayments.Add(oITaxAdvancePayment);
                #endregion
            }
            return oITaxAdvancePayments;
        }

        public List<ITaxAdvancePayment> Gets(string sSQL, Int64 nUserID)
        {
            List<ITaxAdvancePayment> oITaxAdvancePayments = new List<ITaxAdvancePayment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ITaxAdvancePaymentDA.Gets(sSQL, tc);
                oITaxAdvancePayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ITaxAdvancePayment oITaxAdvancePayment = new ITaxAdvancePayment();
                oITaxAdvancePayment.ErrorMessage = e.Message;
                oITaxAdvancePayments.Add(oITaxAdvancePayment);
                #endregion
            }
            return oITaxAdvancePayments;
        }

        #endregion


    }
}
