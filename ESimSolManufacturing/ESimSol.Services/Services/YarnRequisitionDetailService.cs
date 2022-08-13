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
    public class YarnRequisitionDetailService : MarshalByRefObject, IYarnRequisitionDetailService
    {
        #region Private functions and declaration

        private YarnRequisitionDetail MapObject(NullHandler oReader)
        {
            YarnRequisitionDetail oYarnRequisitionDetail = new YarnRequisitionDetail();
            oYarnRequisitionDetail.YarnRequisitionDetailID = oReader.GetInt32("YarnRequisitionDetailID");
            oYarnRequisitionDetail.YarnRequisitionID = oReader.GetInt32("YarnRequisitionID");
            oYarnRequisitionDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oYarnRequisitionDetail.ColorID = oReader.GetInt32("ColorID");
            oYarnRequisitionDetail.PentonNo = oReader.GetString("PentonNo");
            oYarnRequisitionDetail.GARQty = oReader.GetDouble("GARQty");
            oYarnRequisitionDetail.FabricID = oReader.GetInt32("FabricID");
            oYarnRequisitionDetail.GSM = oReader.GetString("GSM");
            oYarnRequisitionDetail.YarnID = oReader.GetInt32("YarnID");
            oYarnRequisitionDetail.YarnCount = oReader.GetString("YarnCount");
            oYarnRequisitionDetail.YarnPercent = oReader.GetDouble("YarnPercent");
            oYarnRequisitionDetail.ActualConsumption = oReader.GetDouble("ActualConsumption");
            oYarnRequisitionDetail.CostingConsumption = oReader.GetDouble("CostingConsumption");
            oYarnRequisitionDetail.RequisitionQty = oReader.GetDouble("RequisitionQty");
            oYarnRequisitionDetail.ApproxQty = oReader.GetDouble("ApproxQty");
            oYarnRequisitionDetail.MUnitID = oReader.GetInt32("MUnitID");
            oYarnRequisitionDetail.Remarks = oReader.GetString("Remarks");
            oYarnRequisitionDetail.FabricName = oReader.GetString("FabricName");
            oYarnRequisitionDetail.FabricCode = oReader.GetString("FabricCode");
            oYarnRequisitionDetail.YarnName = oReader.GetString("YarnName");
            oYarnRequisitionDetail.YarnCode = oReader.GetString("YarnCode");
            oYarnRequisitionDetail.MUSymbol = oReader.GetString("MUSymbol");
            oYarnRequisitionDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oYarnRequisitionDetail.StyleNo = oReader.GetString("StyleNo");
            oYarnRequisitionDetail.BuyerName = oReader.GetString("BuyerName");
            oYarnRequisitionDetail.ColorName = oReader.GetString("ColorName");
            return oYarnRequisitionDetail;
        }

        private YarnRequisitionDetail CreateObject(NullHandler oReader)
        {
            YarnRequisitionDetail oYarnRequisitionDetail = new YarnRequisitionDetail();
            oYarnRequisitionDetail = MapObject(oReader);
            return oYarnRequisitionDetail;
        }

        private List<YarnRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<YarnRequisitionDetail> oYarnRequisitionDetail = new List<YarnRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                YarnRequisitionDetail oItem = CreateObject(oHandler);
                oYarnRequisitionDetail.Add(oItem);
            }
            return oYarnRequisitionDetail;
        }

        #endregion

        #region Interface implementation
        public YarnRequisitionDetail Save(YarnRequisitionDetail oYarnRequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region YarnRequisitionDetail
                IDataReader reader;
                if (oYarnRequisitionDetail.YarnRequisitionDetailID <= 0)
                {
                    reader = YarnRequisitionDetailDA.InsertUpdate(tc, oYarnRequisitionDetail, EnumDBOperation.Insert, "", nUserID);
                }
                else
                {
                    reader = YarnRequisitionDetailDA.InsertUpdate(tc, oYarnRequisitionDetail, EnumDBOperation.Update, "", nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisitionDetail = new YarnRequisitionDetail();
                    oYarnRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oYarnRequisitionDetail = new YarnRequisitionDetail();
                    oYarnRequisitionDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oYarnRequisitionDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                YarnRequisitionDetail oYarnRequisitionDetail = new YarnRequisitionDetail();
                oYarnRequisitionDetail.YarnRequisitionDetailID = id;
                DBTableReferenceDA.HasReference(tc, "YarnRequisitionDetail", id);
                YarnRequisitionDetailDA.Delete(tc, oYarnRequisitionDetail, EnumDBOperation.Delete, "", nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public YarnRequisitionDetail Get(int id, Int64 nUserId)
        {
            YarnRequisitionDetail oYarnRequisitionDetail = new YarnRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = YarnRequisitionDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisitionDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get YarnRequisitionDetail", e);
                #endregion
            }
            return oYarnRequisitionDetail;
        }

        public List<YarnRequisitionDetail> Gets(Int64 nUserID)
        {
            List<YarnRequisitionDetail> oYarnRequisitionDetails = new List<YarnRequisitionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionDetailDA.Gets(tc);
                oYarnRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                YarnRequisitionDetail oYarnRequisitionDetail = new YarnRequisitionDetail();
                oYarnRequisitionDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oYarnRequisitionDetails;
        }

        public List<YarnRequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<YarnRequisitionDetail> oYarnRequisitionDetails = new List<YarnRequisitionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionDetailDA.Gets(tc, sSQL);
                oYarnRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get YarnRequisitionDetail", e);
                #endregion
            }
            return oYarnRequisitionDetails;
        }

        public List<YarnRequisitionDetail> Gets(int nYarnRequisitionID, Int64 nUserID)
        {
            List<YarnRequisitionDetail> oYarnRequisitionDetails = new List<YarnRequisitionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionDetailDA.Gets(tc, nYarnRequisitionID);
                oYarnRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get YarnRequisitionDetail", e);
                #endregion
            }
            return oYarnRequisitionDetails;
        }
        #endregion
    }

}
