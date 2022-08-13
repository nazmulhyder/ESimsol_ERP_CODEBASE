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
    public class LCTransferDetailService : MarshalByRefObject, ILCTransferDetailService
    {
        #region Private functions and declaration
        private LCTransferDetail MapObject(NullHandler oReader)
        {
            LCTransferDetail oLCTransferDetail = new LCTransferDetail();
            oLCTransferDetail.LCTransferDetailID = oReader.GetInt32("LCTransferDetailID");
            oLCTransferDetail.LCTransferID = oReader.GetInt32("LCTransferID");
            oLCTransferDetail.LCTransferDetailLogID = oReader.GetInt32("LCTransferDetailLogID");
            oLCTransferDetail.LCTransferLogID = oReader.GetInt32("LCTransferLogID");
            oLCTransferDetail.ProformaInvoiceDetailID = oReader.GetInt32("ProformaInvoiceDetailID");
            oLCTransferDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oLCTransferDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oLCTransferDetail.TransferQty = oReader.GetDouble("TransferQty");
            oLCTransferDetail.FOB = oReader.GetDouble("FOB");
            oLCTransferDetail.Amount = oReader.GetDouble("Amount");
            oLCTransferDetail.CommissionInPercent = oReader.GetDouble("CommissionInPercent");
            oLCTransferDetail.FactoryFOB = oReader.GetDouble("FactoryFOB");
            oLCTransferDetail.CommissionPerPcs = oReader.GetDouble("CommissionPerPcs");
            oLCTransferDetail.CommissionAmount = oReader.GetDouble("CommissionAmount");
            oLCTransferDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oLCTransferDetail.OrderNo = oReader.GetString("OrderNo");
            oLCTransferDetail.StyleNo = oReader.GetString("StyleNo");
            oLCTransferDetail.ProductName = oReader.GetString("ProductName");
            oLCTransferDetail.Fabrication = oReader.GetString("Fabrication");
            oLCTransferDetail.PIDetailQty = oReader.GetDouble("PIDetailQty");
            oLCTransferDetail.YeToTransferQty = oReader.GetDouble("YeToTransferQty");
            oLCTransferDetail.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            oLCTransferDetail.DiscountInPercent = oReader.GetDouble("DiscountInPercent");
            oLCTransferDetail.AdditionInPercent = oReader.GetDouble("AdditionInPercent");
            oLCTransferDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            return oLCTransferDetail;
        }

        private LCTransferDetail CreateObject(NullHandler oReader)
        {
            LCTransferDetail oLCTransferDetail = new LCTransferDetail();
            oLCTransferDetail = MapObject(oReader);
            return oLCTransferDetail;
        }

        private List<LCTransferDetail> CreateObjects(IDataReader oReader)
        {
            List<LCTransferDetail> oLCTransferDetail = new List<LCTransferDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LCTransferDetail oItem = CreateObject(oHandler);
                oLCTransferDetail.Add(oItem);
            }
            return oLCTransferDetail;
        }

        #endregion

        #region Interface implementation
        public LCTransferDetailService() { }

        public LCTransferDetail Save(LCTransferDetail oLCTransferDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<LCTransferDetail> _oLCTransferDetails = new List<LCTransferDetail>();
            oLCTransferDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LCTransferDetailDA.InsertUpdate(tc, oLCTransferDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTransferDetail = new LCTransferDetail();
                    oLCTransferDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLCTransferDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oLCTransferDetail;
        }
        
        public LCTransferDetail Get(int LCTransferDetailID, Int64 nUserId)
        {
            LCTransferDetail oAccountHead = new LCTransferDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LCTransferDetailDA.Get(tc, LCTransferDetailID);
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
                throw new ServiceException("Failed to Get LCTransferDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<LCTransferDetail> Gets(int id, Int64 nUserID)
        {
            List<LCTransferDetail> oLCTransferDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LCTransferDetailDA.Gets(id, tc);
                oLCTransferDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCTransferDetail", e);
                #endregion
            }

            return oLCTransferDetail;
        }
        public List<LCTransferDetail> GetsLog(int id, Int64 nUserID)
        {
            List<LCTransferDetail> oLCTransferDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LCTransferDetailDA.GetsLog(id, tc);
                oLCTransferDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCTransferDetail", e);
                #endregion
            }

            return oLCTransferDetail;
        }

        
        public List<LCTransferDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<LCTransferDetail> oLCTransferDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LCTransferDetailDA.Gets(tc, sSQL);
                oLCTransferDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCTransferDetail", e);
                #endregion
            }

            return oLCTransferDetail;
        }
        #endregion
    }
}
