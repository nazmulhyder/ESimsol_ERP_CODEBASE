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

    public class ConsumptionRequisitionDetailService : MarshalByRefObject, IConsumptionRequisitionDetailService
    {
        #region Private functions and declaration
        private ConsumptionRequisitionDetail MapObject(NullHandler oReader)
        {
            ConsumptionRequisitionDetail oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
            oConsumptionRequisitionDetail.ConsumptionRequisitionDetailID = oReader.GetInt32("ConsumptionRequisitionDetailID");
            oConsumptionRequisitionDetail.ConsumptionRequisitionID = oReader.GetInt32("ConsumptionRequisitionID");
            oConsumptionRequisitionDetail.ProductID = oReader.GetInt32("ProductID");
            oConsumptionRequisitionDetail.LotID = oReader.GetInt32("LotID");
            oConsumptionRequisitionDetail.UnitID = oReader.GetInt32("UnitID");
            oConsumptionRequisitionDetail.Quantity = oReader.GetDouble("Quantity");
            oConsumptionRequisitionDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oConsumptionRequisitionDetail.Amount = oReader.GetDouble("Amount");
            oConsumptionRequisitionDetail.ProductCode = oReader.GetString("ProductCode");
            oConsumptionRequisitionDetail.ProductName = oReader.GetString("ProductName");
            oConsumptionRequisitionDetail.LotNo = oReader.GetString("LotNo");
            oConsumptionRequisitionDetail.Balance = oReader.GetDouble("Balance");
            oConsumptionRequisitionDetail.LotUnitPrice = oReader.GetDouble("LotUnitPrice");
            oConsumptionRequisitionDetail.LotUnitID = oReader.GetInt32("LotUnitID");
            oConsumptionRequisitionDetail.StyleID = oReader.GetInt32("StyleID");
            oConsumptionRequisitionDetail.ColorID = oReader.GetInt32("ColorID");
            oConsumptionRequisitionDetail.SizeID = oReader.GetInt32("SizeID");
            oConsumptionRequisitionDetail.StyleNo = oReader.GetString("StyleNo");
            oConsumptionRequisitionDetail.BuyerName = oReader.GetString("BuyerName");
            oConsumptionRequisitionDetail.ColorName = oReader.GetString("ColorName");
            oConsumptionRequisitionDetail.SizeName = oReader.GetString("SizeName");
            oConsumptionRequisitionDetail.UnitName = oReader.GetString("UnitName");
            oConsumptionRequisitionDetail.Symbol = oReader.GetString("Symbol");
            oConsumptionRequisitionDetail.ProductGroupName = oReader.GetString("ProductGroupName");
            oConsumptionRequisitionDetail.YetToReturnQty = oReader.GetDouble("YetToReturnQty");
            oConsumptionRequisitionDetail.ConsumptionRequisitionDetailLogID = oReader.GetInt32("ConsumptionRequisitionDetailLogID");
            oConsumptionRequisitionDetail.ConsumptionRequisitionLogID = oReader.GetInt32("ConsumptionRequisitionLogID");
            return oConsumptionRequisitionDetail;
        }

        private ConsumptionRequisitionDetail CreateObject(NullHandler oReader)
        {
            ConsumptionRequisitionDetail oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
            oConsumptionRequisitionDetail = MapObject(oReader);
            return oConsumptionRequisitionDetail;
        }

        private List<ConsumptionRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetail = new List<ConsumptionRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ConsumptionRequisitionDetail oItem = CreateObject(oHandler);
                oConsumptionRequisitionDetail.Add(oItem);
            }
            return oConsumptionRequisitionDetail;
        }

        #endregion

        #region Interface implementation
        public ConsumptionRequisitionDetailService() { }

        public ConsumptionRequisitionDetail Save(ConsumptionRequisitionDetail oConsumptionRequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ConsumptionRequisitionDetail> _oConsumptionRequisitionDetails = new List<ConsumptionRequisitionDetail>();
            oConsumptionRequisitionDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ConsumptionRequisitionDetailDA.InsertUpdate(tc, oConsumptionRequisitionDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oConsumptionRequisitionDetail = new ConsumptionRequisitionDetail();
                    oConsumptionRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oConsumptionRequisitionDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oConsumptionRequisitionDetail;
        }


        public ConsumptionRequisitionDetail Get(int ConsumptionRequisitionDetailID, Int64 nUserId)
        {
            ConsumptionRequisitionDetail oAccountHead = new ConsumptionRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ConsumptionRequisitionDetailDA.Get(tc, ConsumptionRequisitionDetailID);
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
                throw new ServiceException("Failed to Get ConsumptionRequisitionDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ConsumptionRequisitionDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionRequisitionDetailDA.Gets(LabDipOrderID, tc);
                oConsumptionRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionRequisitionDetail", e);
                #endregion
            }

            return oConsumptionRequisitionDetail;
        }

        public List<ConsumptionRequisitionDetail> GetsLog(int id, Int64 nUserID)
        {
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionRequisitionDetailDA.GetsLog(id, tc);
                oConsumptionRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionRequisitionDetail", e);
                #endregion
            }

            return oConsumptionRequisitionDetail;
        }

        public List<ConsumptionRequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ConsumptionRequisitionDetail> oConsumptionRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ConsumptionRequisitionDetailDA.Gets(tc, sSQL);
                oConsumptionRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsumptionRequisitionDetail", e);
                #endregion
            }

            return oConsumptionRequisitionDetail;
        }
        #endregion
    }
  
    
   
}
