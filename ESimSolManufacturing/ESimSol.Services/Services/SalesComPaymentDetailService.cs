using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class SalesComPaymentDetailService : MarshalByRefObject ,ISalesComPaymentDetailService
    {
        #region Private functions and declaration
        private SalesComPaymentDetail MapObject(NullHandler oReader)
        {
            SalesComPaymentDetail oSalesComPaymentDetail = new SalesComPaymentDetail();
            oSalesComPaymentDetail.SalesComPaymentDetailID = oReader.GetInt32("SalesComPaymentDetailID");
            oSalesComPaymentDetail.SalesComPaymentID = oReader.GetInt32("SalesComPaymentID");
            oSalesComPaymentDetail.SalesCommissionPayableID = oReader.GetInt32("SalesCommissionPayableID");
            oSalesComPaymentDetail.Amount = oReader.GetDouble("Amount");
            oSalesComPaymentDetail.PayableAmountBC = oReader.GetDouble("PayableAmountBC");
            oSalesComPaymentDetail.ActualPayable = oReader.GetDouble("ActualPayable");
            oSalesComPaymentDetail.AmountBC = oReader.GetDouble("AmountBC");
            oSalesComPaymentDetail.Note = oReader.GetString("Note");
            oSalesComPaymentDetail.CPName = oReader.GetString("CPName");
            oSalesComPaymentDetail.Currency = oReader.GetString("Currency");
            oSalesComPaymentDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oSalesComPaymentDetail.PINo = oReader.GetString("PINo");
            oSalesComPaymentDetail.Amount_Bill = oReader.GetDouble("Amount_Bill");
            oSalesComPaymentDetail.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oSalesComPaymentDetail.MaturityAmount = oReader.GetDouble("MaturityAmount");
            oSalesComPaymentDetail.RealizeAmount = oReader.GetDouble("RealizeAmount");
            oSalesComPaymentDetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oSalesComPaymentDetail.ExportBillID = oReader.GetInt32("ExportBillID");
            oSalesComPaymentDetail.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oSalesComPaymentDetail.ExportLCNo = oReader.GetString("ExportLCNo");
            oSalesComPaymentDetail.LDBCNo = oReader.GetString("LDBCNo");
            oSalesComPaymentDetail.Amount_Paid = oReader.GetDouble("Amount_Paid");
            oSalesComPaymentDetail.AdjDeduct = oReader.GetDouble("AdjDeduct");
            oSalesComPaymentDetail.AdjPayable = oReader.GetDouble("AdjPayable");
            oSalesComPaymentDetail.AdjAdd = oReader.GetDouble("AdjAdd");
            return oSalesComPaymentDetail;
        }

        private SalesComPaymentDetail CreateObject(NullHandler oReader)
        {
            SalesComPaymentDetail oSalesComPaymentDetail = MapObject(oReader);
            return oSalesComPaymentDetail;
        }

        private List<SalesComPaymentDetail> CreateObjects(IDataReader oReader)
        {
            List<SalesComPaymentDetail> oSalesComPaymentDetail = new List<SalesComPaymentDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesComPaymentDetail oItem = CreateObject(oHandler);
                oSalesComPaymentDetail.Add(oItem);
            }
            return oSalesComPaymentDetail;
        }

        #endregion

        #region Interface implementation
        public SalesComPaymentDetailService() { }

        public SalesComPaymentDetail IUD(SalesComPaymentDetail oSalesComPaymentDetail, int nDBOperation, Int64 nUserID)
        {
            SalesComPayment oSalesComPayment = new SalesComPayment();
            List<SalesComPaymentDetail> _oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                //if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                //{

                //    if (oSalesComPaymentDetail.SalesComPaymentID <= 0)
                //    {
                //        reader = SalesComPaymentDA.IUD(tc, oSalesComPaymentDetail.TempSalesComPayment, nDBOperation, nUserID);
                //        oReader = new NullHandler(reader);
                //        if (reader.Read())
                //        {
                //            oSalesComPayment = new SalesComPayment();
                //            oSalesComPayment = SalesComPaymentService.CreateObject(oReader);
                //        }
                //        reader.Close();
                //    }
                //   // oSalesComPaymentDetail.SalesComPaymentID = (oSalesComPaymentDetail.SalesComPaymentID > 0) ? oSalesComPaymentDetail.SalesComPaymentID : oSalesComPayment.SalesComPaymentID;
                //    foreach (SalesComPaymentDetail obj in oSalesComPaymentDetail.SalesComPaymentDetails)
                //    {

                //        obj.SalesComPaymentID = oSalesComPaymentDetail.SalesComPaymentID;
                //        reader = SalesComPaymentDetailDA.IUD(tc, obj, nDBOperation, nUserID);
                //        oReader = new NullHandler(reader);
                //        if (reader.Read())
                //        {
                //            var oSCPD = new SalesComPaymentDetail();
                //            oSCPD = CreateObject(oReader);
                //            _oSalesComPaymentDetails.Add(oSCPD);
                           
                //        }
                //        reader.Close();

                //    }
                //    oSalesComPaymentDetail = new SalesComPaymentDetail();
                //    oSalesComPaymentDetail.SalesComPaymentDetails = _oSalesComPaymentDetails;
                // }
                 if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = SalesComPaymentDetailDA.IUD(tc, oSalesComPaymentDetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    reader.Close();
                    oSalesComPaymentDetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                _oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
                oSalesComPaymentDetail = new SalesComPaymentDetail();
                oSalesComPaymentDetail.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            //oSalesComPaymentDetail.TempSalesComPayment = oSalesComPayment;
            return oSalesComPaymentDetail;
        }

        public SalesComPaymentDetail Get(int nSULDDetailID, Int64 nUserId)
        {
            SalesComPaymentDetail oSalesComPaymentDetail = new SalesComPaymentDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalesComPaymentDetailDA.Get(tc, nSULDDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalesComPaymentDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalesComPaymentDetail = new SalesComPaymentDetail();
                oSalesComPaymentDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oSalesComPaymentDetail;
        }

        public List<SalesComPaymentDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<SalesComPaymentDetail> oSalesComPaymentDetails = new List<SalesComPaymentDetail>();
            SalesComPaymentDetail oSalesComPaymentDetail = new SalesComPaymentDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalesComPaymentDetailDA.Gets(tc, sSQL);
                oSalesComPaymentDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalesComPaymentDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oSalesComPaymentDetails.Add(oSalesComPaymentDetail);
                #endregion
            }

            return oSalesComPaymentDetails;
        }

      
        
        #endregion
    }
}
