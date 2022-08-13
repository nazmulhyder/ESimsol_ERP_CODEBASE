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

    public class PartsRequisitionDetailService : MarshalByRefObject, IPartsRequisitionDetailService
    {
        #region Private functions and declaration
        private PartsRequisitionDetail MapObject(NullHandler oReader)
        {
            PartsRequisitionDetail oPartsRequisitionDetail = new PartsRequisitionDetail();
            oPartsRequisitionDetail.PartsRequisitionDetailID = oReader.GetInt32("PartsRequisitionDetailID");
            oPartsRequisitionDetail.PartsRequisitionID = oReader.GetInt32("PartsRequisitionID");
            oPartsRequisitionDetail.ProductID = oReader.GetInt32("ProductID");
            oPartsRequisitionDetail.LotID = oReader.GetInt32("LotID");
            oPartsRequisitionDetail.UnitID = oReader.GetInt32("UnitID");
            oPartsRequisitionDetail.Quantity = oReader.GetDouble("Quantity");
            oPartsRequisitionDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oPartsRequisitionDetail.Amount = oReader.GetDouble("Amount");
            oPartsRequisitionDetail.ProductCode = oReader.GetString("ProductCode");
            oPartsRequisitionDetail.ProductName = oReader.GetString("ProductName");
            oPartsRequisitionDetail.LotNo = oReader.GetString("LotNo");
            oPartsRequisitionDetail.Balance = oReader.GetDouble("Balance");
            oPartsRequisitionDetail.LotUnitPrice = oReader.GetDouble("LotUnitPrice");
            oPartsRequisitionDetail.LotUnitID = oReader.GetInt32("LotUnitID");
            oPartsRequisitionDetail.StyleID = oReader.GetInt32("StyleID");
            oPartsRequisitionDetail.ColorID = oReader.GetInt32("ColorID");
            oPartsRequisitionDetail.SizeID = oReader.GetInt32("SizeID");
            oPartsRequisitionDetail.StyleNo = oReader.GetString("StyleNo");
            oPartsRequisitionDetail.BuyerName = oReader.GetString("BuyerName");
            oPartsRequisitionDetail.ColorName = oReader.GetString("ColorName");
            oPartsRequisitionDetail.SizeName = oReader.GetString("SizeName");
            oPartsRequisitionDetail.UnitName = oReader.GetString("UnitName");
            oPartsRequisitionDetail.Symbol = oReader.GetString("Symbol");
            oPartsRequisitionDetail.ProductGroupName = oReader.GetString("ProductGroupName");
            oPartsRequisitionDetail.YetToReturnQty = oReader.GetDouble("YetToReturnQty");
            oPartsRequisitionDetail.PartsRequisitionDetailLogID = oReader.GetInt32("PartsRequisitionDetailLogID");
            oPartsRequisitionDetail.PartsRequisitionLogID = oReader.GetInt32("PartsRequisitionLogID");
            oPartsRequisitionDetail.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oPartsRequisitionDetail.LocationName = oReader.GetString("LocationName");
            oPartsRequisitionDetail.OperationUnitName = oReader.GetString("OperationUnitName");
            oPartsRequisitionDetail.OUShortName = oReader.GetString("OUShortName");
            oPartsRequisitionDetail.LocationShortName = oReader.GetString("LocationShortName");
            oPartsRequisitionDetail.ShelfName = oReader.GetString("ShelfName");
            oPartsRequisitionDetail.RackNo = oReader.GetString("RackNo");
            oPartsRequisitionDetail.ChargeType = (EnumServiceILaborChargeType)oReader.GetInt16("ChargeType");
            return oPartsRequisitionDetail;
        }

        private PartsRequisitionDetail CreateObject(NullHandler oReader)
        {
            PartsRequisitionDetail oPartsRequisitionDetail = new PartsRequisitionDetail();
            oPartsRequisitionDetail = MapObject(oReader);
            return oPartsRequisitionDetail;
        }

        private List<PartsRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<PartsRequisitionDetail> oPartsRequisitionDetail = new List<PartsRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PartsRequisitionDetail oItem = CreateObject(oHandler);
                oPartsRequisitionDetail.Add(oItem);
            }
            return oPartsRequisitionDetail;
        }

        #endregion

        #region Interface implementation
        public PartsRequisitionDetailService() { }

        public PartsRequisitionDetail Save(PartsRequisitionDetail oPartsRequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<PartsRequisitionDetail> _oPartsRequisitionDetails = new List<PartsRequisitionDetail>();
            oPartsRequisitionDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PartsRequisitionDetailDA.InsertUpdate(tc, oPartsRequisitionDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisitionDetail = new PartsRequisitionDetail();
                    oPartsRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPartsRequisitionDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oPartsRequisitionDetail;
        }


        public PartsRequisitionDetail Get(int PartsRequisitionDetailID, Int64 nUserId)
        {
            PartsRequisitionDetail oAccountHead = new PartsRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PartsRequisitionDetailDA.Get(tc, PartsRequisitionDetailID);
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
                throw new ServiceException("Failed to Get PartsRequisitionDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PartsRequisitionDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<PartsRequisitionDetail> oPartsRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartsRequisitionDetailDA.Gets(LabDipOrderID, tc);
                oPartsRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartsRequisitionDetail", e);
                #endregion
            }

            return oPartsRequisitionDetail;
        }

        public List<PartsRequisitionDetail> GetsLog(int id, Int64 nUserID)
        {
            List<PartsRequisitionDetail> oPartsRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartsRequisitionDetailDA.GetsLog(id, tc);
                oPartsRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartsRequisitionDetail", e);
                #endregion
            }

            return oPartsRequisitionDetail;
        }

        public List<PartsRequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<PartsRequisitionDetail> oPartsRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PartsRequisitionDetailDA.Gets(tc, sSQL);
                oPartsRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartsRequisitionDetail", e);
                #endregion
            }

            return oPartsRequisitionDetail;
        }
        #endregion
    }
  
    
   
}
