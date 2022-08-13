using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using System.Linq;
namespace ESimSol.Services.Services
{
    public class SalesComPaymentService : MarshalByRefObject, ISalesComPaymentService
    {
        #region Private functions and declaration
        private static SalesComPayment MapObject(NullHandler oReader)
        {
            SalesComPayment oSalesComPayment = new SalesComPayment();
            oSalesComPayment.SalesComPaymentID = oReader.GetInt32("SalesComPaymentID");
            oSalesComPayment.MRNo = oReader.GetString("MRNo");
            oSalesComPayment.MRDate = oReader.GetDateTime("MRDate");
            oSalesComPayment.PaymentMode = (EnumPaymentMethod) oReader.GetInt16("PaymentMode");
            oSalesComPayment.PaymentType = (EnumPayment_CommissionType)oReader.GetInt16("PaymentType");
            oSalesComPayment.Amount = oReader.GetDouble("Amount");
            oSalesComPayment.Currency = oReader.GetString("Currency");
            oSalesComPayment.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oSalesComPayment.ApproveBy = oReader.GetInt32("ApproveBy");
            oSalesComPayment.ApproveDate = oReader.GetDateTime("ApproveDate");
            oSalesComPayment.CRate = oReader.GetDouble("CRate");
            oSalesComPayment.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesComPayment.DocNo = oReader.GetString("DocNo");
            oSalesComPayment.DocDate = oReader.GetDateTime("DocDate");
            oSalesComPayment.BankAccountID = oReader.GetInt32("BankAccountID");
            oSalesComPayment.BankID = oReader.GetInt32("BankID");
            oSalesComPayment.BankBranchID = oReader.GetInt32("BankBranchID");
            oSalesComPayment.BUID = oReader.GetInt32("BUID");
            oSalesComPayment.CPName = oReader.GetString("CPName");
            oSalesComPayment.ContractorName = oReader.GetString("ContractorName");
            oSalesComPayment.AccountName = oReader.GetString("AccountName");
            oSalesComPayment.ApproveByName = oReader.GetString("ApproveByName");
            oSalesComPayment.PreparedByName = oReader.GetString("PreparedByName");
            oSalesComPayment.BankName = oReader.GetString("BankName");
            oSalesComPayment.BranchName = oReader.GetString("BranchName");
            oSalesComPayment.AccountNo = oReader.GetString("AccountNo");
            oSalesComPayment.Note = oReader.GetString("Note");
            oSalesComPayment.SampleInvoiceID = oReader.GetInt32("SampleInvoiceID");
            oSalesComPayment.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oSalesComPayment.SampleInvoiceDate = oReader.GetDateTime("SampleInvoiceDate");
            oSalesComPayment.SampleInvoice_Amount = oReader.GetDouble("SampleInvoice_Amount");
            return oSalesComPayment;
        }


        public static SalesComPayment CreateObject(NullHandler oReader)
        {
            SalesComPayment oSalesComPayment = MapObject(oReader);
            return oSalesComPayment;
        }

        private List<SalesComPayment> CreateObjects(IDataReader oReader)
        {
            List<SalesComPayment> oSalesComPayments = new List<SalesComPayment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesComPayment oItem = CreateObject(oHandler);
                oSalesComPayments.Add(oItem);
            }
            return oSalesComPayments;
        }

        #endregion

        #region Interface implementation
        public SalesComPaymentService() { }

        public SalesComPayment IUD(SalesComPayment oSalesComPayment, int nDBOperation, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    List<SalesComPaymentDetail> _oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
                    List<SalesComPaymentDetail> _oTempSalesComPaymentDetails = new List<SalesComPaymentDetail>();
                    SalesComPaymentDetail _oSalesComPaymentDetail = new SalesComPaymentDetail();
                    _oSalesComPaymentDetails = oSalesComPayment.SalesComPaymentDetails;
                    #region SalesComPayment
                    reader = SalesComPaymentDA.IUD(tc, oSalesComPayment, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oSalesComPayment = new SalesComPayment();
                        oSalesComPayment = CreateObject(oReader);
                    }
                    reader.Close();

                    
                    #endregion
                    #region  Sales commission Detail Part
                    if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                    {
                        if (_oSalesComPaymentDetails.Any())
                        {
                            foreach (SalesComPaymentDetail oItem in _oSalesComPaymentDetails)
                            {
                                IDataReader readerdetail;
                                oItem.SalesComPaymentID = oSalesComPayment.SalesComPaymentID;
                                if (oItem.SalesComPaymentDetailID <= 0)
                                {
                                    readerdetail = SalesComPaymentDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                                }
                                else
                                {
                                    readerdetail = SalesComPaymentDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID);
                                }
                                NullHandler oReaderDetail = new NullHandler(readerdetail);
                                if (readerdetail.Read())
                                {
                                    _oSalesComPaymentDetail.SalesComPaymentDetailID = oReaderDetail.GetInt32("SalesComPaymentDetailID");
                                    _oSalesComPaymentDetail.SalesComPaymentID = oReaderDetail.GetInt32("SalesComPaymentID");
                                    _oSalesComPaymentDetail.SalesCommissionPayableID = oReaderDetail.GetInt32("SalesCommissionPayableID");
                                    _oSalesComPaymentDetail.Amount = oReaderDetail.GetDouble("Amount");
                                    _oSalesComPaymentDetail.Note = oReaderDetail.GetString("Note");
                                    _oSalesComPaymentDetail.CPName = oReaderDetail.GetString("CPName");
                                    _oSalesComPaymentDetail.Currency = oReaderDetail.GetString("Currency");
                                    _oSalesComPaymentDetail.CurrencyID = oReaderDetail.GetInt32("CurrencyID");
                                    _oSalesComPaymentDetail.PINo = oReaderDetail.GetString("PINo");
                                    _oSalesComPaymentDetail.Amount_Bill = oReaderDetail.GetDouble("Amount_Bill");
                                    _oSalesComPaymentDetail.ContactPersonnelID = oReaderDetail.GetInt32("ContactPersonnelID");
                                    _oSalesComPaymentDetail.MaturityAmount = oReaderDetail.GetDouble("MaturityAmount");
                                    _oSalesComPaymentDetail.RealizeAmount = oReaderDetail.GetDouble("RealizeAmount");
                                    _oSalesComPaymentDetail.ExportPIID = oReaderDetail.GetInt32("ExportPIID");
                                    _oSalesComPaymentDetail.ExportBillID = oReaderDetail.GetInt32("ExportBillID");
                                    _oSalesComPaymentDetail.CommissionAmount = oReaderDetail.GetDouble("CommissionAmount");
                                    _oSalesComPaymentDetail.ExportLCNo = oReaderDetail.GetString("ExportLCNo");
                                    _oSalesComPaymentDetail.LDBCNo = oReaderDetail.GetString("LDBCNo");
                                    _oSalesComPaymentDetail.Amount_Paid = oReaderDetail.GetDouble("Amount_Paid");
                                    _oSalesComPaymentDetail.AdjDeduct = oReaderDetail.GetDouble("AdjDeduct");
                                    _oSalesComPaymentDetail.AdjPayable = oReaderDetail.GetDouble("AdjPayable");
                                    _oSalesComPaymentDetail.AdjAdd = oReaderDetail.GetDouble("AdjAdd");

                                }
                                readerdetail.Close();
                                _oTempSalesComPaymentDetails.Add(_oSalesComPaymentDetail);
                                _oSalesComPaymentDetail = new SalesComPaymentDetail();
                            }


                        }
                        oSalesComPayment.SalesComPaymentDetails = _oTempSalesComPaymentDetails;
                        oSalesComPayment.Amount = _oTempSalesComPaymentDetails.Sum(x => x.Amount);
                    }
                    #endregion
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = SalesComPaymentDA.IUD(tc, oSalesComPayment, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oSalesComPayment.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalesComPayment = new SalesComPayment();
                oSalesComPayment.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oSalesComPayment;
        }
        
        public List<SalesComPayment> Gets(string sSQL, Int64 nUserID)
        {
            List<SalesComPayment> oSalesComPayments = new List<SalesComPayment>();
            SalesComPayment oSCLC = new SalesComPayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesComPaymentDA.Gets(tc, sSQL);
                oSalesComPayments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSCLC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oSalesComPayments.Add(oSCLC);
                #endregion
            }

            return oSalesComPayments;
        }
        public SalesComPayment Get(int nSalesComPaymentID, Int64 nUserId)
        {
            SalesComPayment oSalesComPayment = new SalesComPayment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesComPaymentDA.Get(tc, nSalesComPaymentID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesComPayment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalesComPayment = new SalesComPayment();
                oSalesComPayment.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oSalesComPayment;
        }

        #endregion
    }
    
}
