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
    public class SparePartsChallanDetailService : MarshalByRefObject, ISparePartsChallanDetailService
    {
        #region Private functions and declaration
        private SparePartsChallanDetail MapObject(NullHandler oReader)
        {
            SparePartsChallanDetail oSparePartsChallanDetail = new SparePartsChallanDetail();
            oSparePartsChallanDetail.SparePartsChallanDetailID = oReader.GetInt32("SparePartsChallanDetailID");
            oSparePartsChallanDetail.SparePartsChallanID = oReader.GetInt32("SparePartsChallanID");
            oSparePartsChallanDetail.SparePartsRequisitionDetailID = oReader.GetInt32("SparePartsRequisitionDetailID");
            oSparePartsChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oSparePartsChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oSparePartsChallanDetail.ProductName = oReader.GetString("ProductName");
            oSparePartsChallanDetail.LotID = oReader.GetInt32("LotID");
            oSparePartsChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oSparePartsChallanDetail.MUnitName = oReader.GetString("MUnitName");
            oSparePartsChallanDetail.MUnitSymbol = oReader.GetString("MUnitSymbol");
            oSparePartsChallanDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oSparePartsChallanDetail.Remarks = oReader.GetString("Remarks");
            oSparePartsChallanDetail.LotNo = oReader.GetString("LotNo");
            oSparePartsChallanDetail.CurrentStockQty = oReader.GetDouble("CurrentStockQty");
            oSparePartsChallanDetail.RequisitionQty = oReader.GetDouble("RequisitionQty");
            return oSparePartsChallanDetail;
        }

        private SparePartsChallanDetail CreateObject(NullHandler oReader)
        {
            SparePartsChallanDetail oSparePartsChallanDetail = new SparePartsChallanDetail();
            oSparePartsChallanDetail = MapObject(oReader);
            return oSparePartsChallanDetail;
        }

        private List<SparePartsChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<SparePartsChallanDetail> oSparePartsChallanDetail = new List<SparePartsChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SparePartsChallanDetail oItem = CreateObject(oHandler);
                oSparePartsChallanDetail.Add(oItem);
            }
            return oSparePartsChallanDetail;
        }

        #endregion

        #region Interface implementation
        public SparePartsChallanDetailService() { }

        public SparePartsChallanDetail Save(SparePartsChallanDetail oSparePartsChallanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSparePartsChallanDetail.SparePartsChallanDetailID <= 0)
                {
                    reader = SparePartsChallanDetailDA.InsertUpdate(tc, oSparePartsChallanDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    reader = SparePartsChallanDetailDA.InsertUpdate(tc, oSparePartsChallanDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsChallanDetail = new SparePartsChallanDetail();
                    oSparePartsChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save SparePartsChallanDetail. Because of " + e.Message, e);
                #endregion
            }
            return oSparePartsChallanDetail;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SparePartsChallanDetail oSparePartsChallanDetail = new SparePartsChallanDetail();
                oSparePartsChallanDetail.SparePartsChallanDetailID = id;
                SparePartsChallanDetailDA.Delete(tc, oSparePartsChallanDetail, EnumDBOperation.Delete, nUserId, "");
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
        public SparePartsChallanDetail Get(int id, Int64 nUserId)
        {
            SparePartsChallanDetail oSparePartsChallanDetail = new SparePartsChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SparePartsChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SparePartsChallanDetail", e);
                #endregion
            }
            return oSparePartsChallanDetail;
        }
        public List<SparePartsChallanDetail> Gets(Int64 nUserID)
        {
            List<SparePartsChallanDetail> oSparePartsChallanDetails = new List<SparePartsChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsChallanDetailDA.Gets(tc);
                oSparePartsChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsChallanDetail", e);
                #endregion
            }
            return oSparePartsChallanDetails;
        }
        public List<SparePartsChallanDetail> GetsBySparePartsChallanID(int nSparePartsChallanID, Int64 nUserID)
        {
            List<SparePartsChallanDetail> oSparePartsChallanDetails = new List<SparePartsChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsChallanDetailDA.GetsBySparePartsChallanID(tc, nSparePartsChallanID);
                oSparePartsChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SpareParts ChallanDetail", e);
                #endregion
            }
            return oSparePartsChallanDetails;
        }
        public List<SparePartsChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<SparePartsChallanDetail> oSparePartsChallanDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsChallanDetailDA.Gets(tc, sSQL);
                oSparePartsChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsChallanDetail", e);
                #endregion
            }
            return oSparePartsChallanDetails;
        }
        #endregion
    }
}