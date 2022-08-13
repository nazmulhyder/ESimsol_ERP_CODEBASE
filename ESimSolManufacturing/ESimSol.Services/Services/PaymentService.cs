using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{

    public class PaymentService : MarshalByRefObject, IPaymentService
    {
        #region Private functions and declaration
        private Payment MapObject(NullHandler oReader)
        {
            Payment oPayment = new Payment();
            oPayment.PaymentID = oReader.GetInt32("PaymentID");
            oPayment.ContractorID = oReader.GetInt32("ContractorID");          
            oPayment.MRNo = oReader.GetString("MRNo");
            oPayment.Currency = oReader.GetString("Currency");
            oPayment.PaymentMode = (EnumPaymentMethod)oReader.GetInt32("PaymentMode");
            oPayment.PaymentModeInt = oReader.GetInt32("PaymentMode");
            oPayment.Amount = oReader.GetDouble("Amount");
            oPayment.Discount = oReader.GetDouble("Discount");
            oPayment.ApproveBy = oReader.GetInt32("ApprovedBy");
            oPayment.PaymentDocID = oReader.GetInt32("PaymentDocID");
            oPayment.Note = oReader.GetString("Note");
            oPayment.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            oPayment.MRDate = oReader.GetDateTime("MRDate");
            oPayment.ApprovedByName = oReader.GetString("ApprovedByName");
            oPayment.BankID = oReader.GetInt32("BankID");
            oPayment.BankName = oReader.GetString("BankName");            
            oPayment.PaymentStatus = (EnumPaymentStatus) oReader.GetInt32("PaymentStatus");
            oPayment.PaymentStatusInInt = oReader.GetInt32("PaymentStatus");
            oPayment.DocNo = oReader.GetString("DocNo");
            oPayment.DocAmount = oReader.GetDouble("DocAmount");
            oPayment.ConsumedAmount = oReader.GetDouble("ConsumedAmount");
            oPayment.DocDate = oReader.GetDateTime("DocDate");
            oPayment.CRate = oReader.GetDouble("CRate");
            oPayment.ContractorName = oReader.GetString("ContractorName");
            oPayment.PaymentType = (EnumPaymentReceiveType)oReader.GetInt32("PaymentType");
            oPayment.PaymentTypeInInt = oReader.GetInt32("PaymentType");
            oPayment.BUID = oReader.GetInt32("BUID");
            oPayment.CurrencyID = oReader.GetInt32("CurrencyID");
            oPayment.PrepareByName = oReader.GetString("PrepareByName");
            oPayment.BankAccountID_Deposit = oReader.GetInt32("BankAccountID_Deposit");
            oPayment.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            return oPayment;
        }

        private Payment CreateObject(NullHandler oReader)
        {
            Payment oPayment = new Payment();
            oPayment = MapObject(oReader);
            return oPayment;
        }

        private List<Payment> CreateObjects(IDataReader oReader)
        {
            List<Payment> oPayment = new List<Payment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Payment oItem = CreateObject(oHandler);
                oPayment.Add(oItem);
            }
            return oPayment;
        }

        #endregion

        #region Interface implementation
        public PaymentService() { }

        public Payment Save(Payment oPayment, Int64 nUserId)
        {
            TransactionContext tc = null;
            string sPaymentDetaillIDs = "";                
            try
            {
                List<PaymentDetail> oPaymentDetails = new List<PaymentDetail>();
                oPaymentDetails = oPayment.PaymentDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPayment.PaymentID <= 0)
                {
                    reader = PaymentDA.InsertUpdate(tc, oPayment, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    VoucherDA.CheckVoucherReference(tc, "Payment", "PaymentID", oPayment.PaymentID);//Check vourcher Reference
                    reader = PaymentDA.InsertUpdate(tc, oPayment, EnumDBOperation.Update, nUserId);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayment = new Payment();
                    oPayment = CreateObject(oReader);
                }
                reader.Close();
                #region Details Part
                if (oPaymentDetails != null)
                {
                    foreach (PaymentDetail oItem in oPaymentDetails)
                    {
                        IDataReader readertnc;
                        oItem.PaymentID = oPayment.PaymentID;
                        if (oItem.PaymentDetailID <= 0)
                        {
                            readertnc = PaymentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readertnc = PaymentDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sPaymentDetaillIDs = sPaymentDetaillIDs + oReaderTNC.GetString("PaymentDetailID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sPaymentDetaillIDs.Length > 0)
                    {
                        sPaymentDetaillIDs = sPaymentDetaillIDs.Remove(sPaymentDetaillIDs.Length - 1, 1);
                    }
                    PaymentDetail oPaymentDetail = new PaymentDetail();
                    oPaymentDetail.PaymentID = oPayment.PaymentID;
                    PaymentDetailDA.Delete(tc, oPaymentDetail, EnumDBOperation.Delete, nUserId, sPaymentDetaillIDs);
                }


                #endregion

                #region
                reader = PaymentDA.Get(tc, oPayment.PaymentID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayment = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPayment = new Payment();
                oPayment.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Payment. Because of " + e.Message, e);
                #endregion
            }
            return oPayment;
        }
        public string Delete(Payment oPayment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherDA.CheckVoucherReference(tc, "Payment", "PaymentID", oPayment.PaymentID);//Check vourcher Reference
                PaymentDA.Delete(tc, oPayment, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Payment. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Payment Get(int id, Int64 nUserId)
        {
            Payment oPayment = new Payment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PaymentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Payment", e);
                #endregion
            }

            return oPayment;
        }
        public Payment GetBY(int nDebiteID, Int64 nUserId)
        {
            Payment oAccountHead = new Payment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PaymentDA.GetBy(tc, nDebiteID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Payment", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Payment> Gets(Int64 nUserId)
        {
            List<Payment> oPayment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentDA.Gets(tc);
                oPayment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Payment", e);
                #endregion
            }

            return oPayment;
        }

        public List<Payment> Gets(EnumPaymentType ePaymentType, Int64 nUserId)
        {
            List<Payment> oPayment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentDA.Gets(tc, ePaymentType);
                oPayment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Payment", e);
                #endregion
            }

            return oPayment;
        }

        public List<Payment> Gets(string sSQL, Int64 nUserId)
        {
            List<Payment> oPayment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PaymentDA.Gets(tc, sSQL);
                oPayment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Payment", e);
                #endregion
            }

            return oPayment;
        }

     

        public Payment Payment_Approve(Payment oPayment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
               
                    reader = PaymentDA.InsertUpdate(tc, oPayment, EnumDBOperation.Approval, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayment = new Payment();
                    oPayment = CreateObject(oReader);
                }
                reader.Close();
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPayment = new Payment();
                oPayment.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Payment. Because of " + e.Message, e);
                #endregion
            }
            return oPayment;
        }
        public Payment Payment_UndoApprove(Payment oPayment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = PaymentDA.InsertUpdate(tc, oPayment, EnumDBOperation.UnApproval, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayment = new Payment();
                    oPayment = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPayment = new Payment();
                oPayment.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Payment. Because of " + e.Message, e);
                #endregion
            }
            return oPayment;
        }

        public Payment UpdateVoucherEffect(Payment oPayment, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PaymentDA.UpdateVoucherEffect(tc, oPayment);
                IDataReader reader;
                reader = PaymentDA.Get(tc, oPayment.PaymentID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPayment = new Payment();
                    oPayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPayment = new Payment();
                oPayment.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oPayment;

        }
        #endregion
    }    
    
}
