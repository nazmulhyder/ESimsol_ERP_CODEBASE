using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{


    public class ProformaInvoiceDetailService : MarshalByRefObject, IProformaInvoiceDetailService
    {
        #region Private functions and declaration
        private ProformaInvoiceDetail MapObject(NullHandler oReader)
        {
            ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();

            oProformaInvoiceDetail.ProformaInvoiceDetailID = oReader.GetInt32("ProformaInvoiceDetailID");
            oProformaInvoiceDetail.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
            oProformaInvoiceDetail.ProformaInvoiceDetailLogID = oReader.GetInt32("ProformaInvoiceDetailLogID");
            oProformaInvoiceDetail.ProformaInvoiceLogID = oReader.GetInt32("ProformaInvoiceLogID");
            oProformaInvoiceDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oProformaInvoiceDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oProformaInvoiceDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oProformaInvoiceDetail.Quantity = oReader.GetDouble("Quantity");
            oProformaInvoiceDetail.FOB = oReader.GetDouble("FOB");
            oProformaInvoiceDetail.BuyerCommissionInPercent = oReader.GetDouble("BuyerCommissionInPercent");
            oProformaInvoiceDetail.BuyerCommission = oReader.GetDouble("BuyerCommission");
            oProformaInvoiceDetail.AdjustAdditon = oReader.GetDouble("AdjustAdditon");
            oProformaInvoiceDetail.AdjustDeduction = oReader.GetDouble("AdjustDeduction");
            oProformaInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oProformaInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oProformaInvoiceDetail.CMValue = oReader.GetDouble("CMValue");
            oProformaInvoiceDetail.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            oProformaInvoiceDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oProformaInvoiceDetail.ShipmentQty = oReader.GetDouble("ShipmentQty");
            oProformaInvoiceDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oProformaInvoiceDetail.StyleNo = oReader.GetString("StyleNo");
            oProformaInvoiceDetail.ProductName = oReader.GetString("ProductName");
            oProformaInvoiceDetail.FabricName = oReader.GetString("FabricName");
            oProformaInvoiceDetail.OrderRecapQty = oReader.GetDouble("OrderRecapQty");
            oProformaInvoiceDetail.DEPT = oReader.GetInt32("DEPT");
            oProformaInvoiceDetail.DeptName = oReader.GetString("DeptName");
            oProformaInvoiceDetail.PINo = oReader.GetString("PINo");
            oProformaInvoiceDetail.SessionName = oReader.GetString("SessionName");
            oProformaInvoiceDetail.YetToTransfer = oReader.GetDouble("YetToTransfer");
            oProformaInvoiceDetail.DiscountInPercent = oReader.GetDouble("DiscountInPercent");
            oProformaInvoiceDetail.AdditionInPercent = oReader.GetDouble("AdditionInPercent");
                        

            return oProformaInvoiceDetail;
        }

        private ProformaInvoiceDetail CreateObject(NullHandler oReader)
        {
            ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();
            oProformaInvoiceDetail = MapObject(oReader);
            return oProformaInvoiceDetail;
        }

        private List<ProformaInvoiceDetail> CreateObjects(IDataReader oReader)
        {
            List<ProformaInvoiceDetail> oProformaInvoiceDetail = new List<ProformaInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProformaInvoiceDetail oItem = CreateObject(oHandler);
                oProformaInvoiceDetail.Add(oItem);
            }
            return oProformaInvoiceDetail;
        }

        #endregion

        #region Interface implementation
        public ProformaInvoiceDetailService() { }

        public ProformaInvoiceDetail Save(ProformaInvoiceDetail oProformaInvoiceDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ProformaInvoiceDetail> _oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();
            oProformaInvoiceDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ProformaInvoiceDetailDA.InsertUpdate(tc, oProformaInvoiceDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoiceDetail = new ProformaInvoiceDetail();
                    oProformaInvoiceDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProformaInvoiceDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oProformaInvoiceDetail;
        }


        public ProformaInvoiceDetail Get(int ProformaInvoiceDetailID, Int64 nUserId)
        {
            ProformaInvoiceDetail oAccountHead = new ProformaInvoiceDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProformaInvoiceDetailDA.Get(tc, ProformaInvoiceDetailID);
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
                throw new ServiceException("Failed to Get ProformaInvoiceDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ProformaInvoiceDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<ProformaInvoiceDetail> oProformaInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceDetailDA.Gets(LabDipOrderID, tc);
                oProformaInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceDetail", e);
                #endregion
            }

            return oProformaInvoiceDetail;
        }

        public List<ProformaInvoiceDetail> GetsPILog(int ProformaInvoiceLogID, Int64 nUserID)
        {
            List<ProformaInvoiceDetail> oProformaInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceDetailDA.GetsPILog(ProformaInvoiceLogID, tc);
                oProformaInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceDetail", e);
                #endregion
            }

            return oProformaInvoiceDetail;
        }
        

        public List<ProformaInvoiceDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ProformaInvoiceDetail> oProformaInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProformaInvoiceDetailDA.Gets(tc, sSQL);
                oProformaInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoiceDetail", e);
                #endregion
            }

            return oProformaInvoiceDetail;
        }
        #endregion
    }
    

}
