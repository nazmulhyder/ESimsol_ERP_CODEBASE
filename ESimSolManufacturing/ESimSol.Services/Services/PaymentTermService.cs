using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class PaymentTermService : MarshalByRefObject, IPaymentTermService
    {
        #region Private functions and declaration
        private PaymentTerm MapObject(NullHandler oReader)
        {
            PaymentTerm oPaymentTerm = new PaymentTerm();
            oPaymentTerm.PaymentTermID = oReader.GetInt32("PaymentTermID");
            oPaymentTerm.Percentage = oReader.GetInt32("Percentage");
            oPaymentTerm.BUID = oReader.GetInt32("BUID");
            oPaymentTerm.TermText = oReader.GetString("TermText");
            oPaymentTerm.DayApplyType = (EnumDayApplyType)oReader.GetInt32("DayApplyType");
            oPaymentTerm.DayApplyTypeint = oReader.GetInt32("DayApplyType");
            oPaymentTerm.Days = oReader.GetInt32("Days");
            oPaymentTerm.DateDisplayPart = (EnumDisplayPart)oReader.GetInt32("DateDisplayPart");
            oPaymentTerm.DateText = oReader.GetString("DateText");
            oPaymentTerm.PaymentTermType = (EnumPaymentTermType)oReader.GetInt16("PaymentTermType");
            oPaymentTerm.PaymentTermTypeInt = oReader.GetInt16("PaymentTermType");
            oPaymentTerm.Days = oReader.GetInt32("Days");
            oPaymentTerm.EndNote = oReader.GetString("EndNote");
            return oPaymentTerm;
        }

        private PaymentTerm CreateObject(NullHandler oReader)
        {
            PaymentTerm oPaymentTerm = new PaymentTerm();
            oPaymentTerm = MapObject(oReader);
            return oPaymentTerm;
        }

        private List<PaymentTerm> CreateObjects(IDataReader oReader)
        {
            List<PaymentTerm> oPaymentTerms = new List<PaymentTerm>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PaymentTerm oItem = CreateObject(oHandler);
                oPaymentTerms.Add(oItem);
            }
            return oPaymentTerms;
        }

        #endregion

        #region Interface implementation
        public PaymentTermService() { }

        public PaymentTerm Save(PaymentTerm oPaymentTerm, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPaymentTerm.PaymentTermID <= 0)
                {
                    reader = PaymentTermDA.InsertUpdate(tc, oPaymentTerm, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = PaymentTermDA.InsertUpdate(tc, oPaymentTerm, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPaymentTerm = new PaymentTerm();
                    oPaymentTerm = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPaymentTerm = new PaymentTerm();
                oPaymentTerm.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPaymentTerm;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PaymentTerm oPaymentTerm = new PaymentTerm();
                oPaymentTerm.PaymentTermID = id;
                PaymentTermDA.Delete(tc, oPaymentTerm, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];                
                #endregion
            }
            return Global.DeleteMessage;
        }
        public PaymentTerm Get(int id, Int64 nUserId)
        {
            PaymentTerm oPaymentTerm = new PaymentTerm();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PaymentTermDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPaymentTerm = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oPaymentTerm;
        }

        public List<PaymentTerm> Gets(Int64 nUserId)
        {
            List<PaymentTerm> oPaymentTerms = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentTermDA.Gets(tc);
               oPaymentTerms = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oPaymentTerms;
        }

        public List<PaymentTerm> GetsByBU(int nBUID, Int64 nUserId)
        {
            List<PaymentTerm> oPaymentTerms = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentTermDA.GetsByBU(nBUID, tc);
                oPaymentTerms = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oPaymentTerms;
        }
        public List<PaymentTerm> Gets(string sSQl,Int64 nUserId)
        {
            List<PaymentTerm> oPaymentTerms = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentTermDA.Gets(tc, sSQl);
                oPaymentTerms = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oPaymentTerms;
        }

        #endregion
    }
}