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
    public class SUDeliveryChallanDetailService : MarshalByRefObject, ISUDeliveryChallanDetailService
    {
        #region Private functions and declaration
        private static SUDeliveryChallanDetail MapObject(NullHandler oReader)
        {
            SUDeliveryChallanDetail oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
            oSUDeliveryChallanDetail.SUDeliveryChallanDetailID = oReader.GetInt32("SUDeliveryChallanDetailID");
            oSUDeliveryChallanDetail.SUDeliveryChallanID = oReader.GetInt32("SUDeliveryChallanID");
            oSUDeliveryChallanDetail.SUDeliveryOrderDetailID = oReader.GetInt32("SUDeliveryOrderDetailID");
            oSUDeliveryChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oSUDeliveryChallanDetail.LotID = oReader.GetInt32("LotID");
            oSUDeliveryChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oSUDeliveryChallanDetail.Qty = oReader.GetDouble("Qty");
            oSUDeliveryChallanDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oSUDeliveryChallanDetail.ProgramQty = oReader.GetDouble("ProgramQty");
            oSUDeliveryChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oSUDeliveryChallanDetail.ProductName = oReader.GetString("ProductName");
            oSUDeliveryChallanDetail.ProductShortName = oReader.GetString("ProductShortName");
            oSUDeliveryChallanDetail.LotNo = oReader.GetString("LotNo");
            oSUDeliveryChallanDetail.MeasurementUnitName = oReader.GetString("MeasurementUnitName");
            oSUDeliveryChallanDetail.MeasurementUnitSymbol = oReader.GetString("MeasurementUnitSymbol");
            oSUDeliveryChallanDetail.SUDeliveryProgramID = oReader.GetInt32("SUDeliveryProgramID");
            oSUDeliveryChallanDetail.WUName = oReader.GetString("WUName");
            oSUDeliveryChallanDetail.Bags = oReader.GetInt32("Bags");
            oSUDeliveryChallanDetail.DCDRemark = oReader.GetString("DCDRemark");
            oSUDeliveryChallanDetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oSUDeliveryChallanDetail.BuyerID = oReader.GetInt32("BuyerID");
            oSUDeliveryChallanDetail.RemainingQty = oReader.GetDouble("RemainingQty");
            oSUDeliveryChallanDetail.ChallanNo = oReader.GetString("ChallanNo");
            oSUDeliveryChallanDetail.ChallanDate = oReader.GetDateTime("ChallanDate");
            oSUDeliveryChallanDetail.ProgramDate = oReader.GetDateTime("ProgramDate");
            oSUDeliveryChallanDetail.Remarks = oReader.GetString("Remarks");
            oSUDeliveryChallanDetail.YetToChallan = oReader.GetDouble("RemainingQty");
            oSUDeliveryChallanDetail.DEOID = oReader.GetInt32("DEOID");
            return oSUDeliveryChallanDetail;
        }

        private SUDeliveryChallanDetail CreateObject(NullHandler oReader)
        {
            SUDeliveryChallanDetail oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
            oSUDeliveryChallanDetail = MapObject(oReader);
            return oSUDeliveryChallanDetail;
        }
        private List<SUDeliveryChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<SUDeliveryChallanDetail> oSUDeliveryChallanDetail = new List<SUDeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SUDeliveryChallanDetail oItem = CreateObject(oHandler);
                oSUDeliveryChallanDetail.Add(oItem);
            }
            return oSUDeliveryChallanDetail;
        }
        #endregion

        #region Interface implementation
        public SUDeliveryChallanDetail Save(SUDeliveryChallanDetail oSUDeliveryChallanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSUDeliveryChallanDetail.SUDeliveryChallanDetailID <= 0)
                {
                    reader = SUDeliveryChallanDetailDA.InsertUpdate(tc, oSUDeliveryChallanDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = SUDeliveryChallanDetailDA.InsertUpdate(tc, oSUDeliveryChallanDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
                    oSUDeliveryChallanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
                oSUDeliveryChallanDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oSUDeliveryChallanDetail;
        }

        public string Delete(int nSUDeliveryChallanDetailId, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SUDeliveryChallanDetail oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
                oSUDeliveryChallanDetail.SUDeliveryChallanDetailID = nSUDeliveryChallanDetailId;
                SUDeliveryChallanDetailDA.Delete(tc, oSUDeliveryChallanDetail, EnumDBOperation.Delete, nUserId,"");
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

        public SUDeliveryChallanDetail Get(int id, Int64 nUserId)
        {
            SUDeliveryChallanDetail oSUDeliveryChallanDetail = new SUDeliveryChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SUDeliveryChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSUDeliveryChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Delivery Challan Detail.", e);
                #endregion
            }
            return oSUDeliveryChallanDetail;
        }

        public List<SUDeliveryChallanDetail> Gets(int nSUDeliveryChallanID, Int64 nUserID)
        {
            List<SUDeliveryChallanDetail> oSUDeliveryChallanDetails = new List<SUDeliveryChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SUDeliveryChallanDetailDA.Gets(tc, nSUDeliveryChallanID);
                oSUDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Challan Detail.", e);
                #endregion
            }
            return oSUDeliveryChallanDetails;
        }

        public List<SUDeliveryChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<SUDeliveryChallanDetail> oSUDeliveryChallanDetails = new List<SUDeliveryChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SUDeliveryChallanDetailDA.Gets(tc, sSQL);
                oSUDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Challan Detail.", e);
                #endregion
            }
            return oSUDeliveryChallanDetails;
        }
        #endregion
    }
}
