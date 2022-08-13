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
    public class KnittingFabricReceiveDetailService : MarshalByRefObject, IKnittingFabricReceiveDetailService
    {
        #region Private functions and declaration

        private KnittingFabricReceiveDetail MapObject(NullHandler oReader)
        {
            KnittingFabricReceiveDetail oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
            oKnittingFabricReceiveDetail.KnittingFabricReceiveDetailID = oReader.GetInt32("KnittingFabricReceiveDetailID");
            oKnittingFabricReceiveDetail.KnittingFabricReceiveID = oReader.GetInt32("KnittingFabricReceiveID");
            oKnittingFabricReceiveDetail.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oKnittingFabricReceiveDetail.FabricID = oReader.GetInt32("FabricID");
            oKnittingFabricReceiveDetail.ReceiveStoreID = oReader.GetInt32("ReceiveStoreID");
            oKnittingFabricReceiveDetail.LotID = oReader.GetInt32("LotID");
            oKnittingFabricReceiveDetail.MUnitID = oReader.GetInt32("MUnitID");
            oKnittingFabricReceiveDetail.Qty = oReader.GetDouble("Qty");
            oKnittingFabricReceiveDetail.NewLotNo = oReader.GetString("NewLotNo");
            oKnittingFabricReceiveDetail.Remarks = oReader.GetString("Remarks");
            oKnittingFabricReceiveDetail.MUnitName = oReader.GetString("MUnitName");
            oKnittingFabricReceiveDetail.OperationUnitName = oReader.GetString("OperationUnitName");
            oKnittingFabricReceiveDetail.FabricName = oReader.GetString("FabricName");
            oKnittingFabricReceiveDetail.FabricCode = oReader.GetString("FabricCode");
            oKnittingFabricReceiveDetail.LotNo = oReader.GetString("LotNo");
            oKnittingFabricReceiveDetail.LotBalance = oReader.GetDouble("LotBalance");
            oKnittingFabricReceiveDetail.LotMUSymbol = oReader.GetString("LotMUSymbol");
            oKnittingFabricReceiveDetail.ProcessLossQty = oReader.GetDouble("ProcessLossQty");
            oKnittingFabricReceiveDetail.PAM = oReader.GetInt32("PAM");
            oKnittingFabricReceiveDetail.GSM = oReader.GetString("GSM");
            oKnittingFabricReceiveDetail.MICDia = oReader.GetString("MICDia");
            oKnittingFabricReceiveDetail.FinishDia = oReader.GetString("FinishDia");
            return oKnittingFabricReceiveDetail;
        }

        private KnittingFabricReceiveDetail CreateObject(NullHandler oReader)
        {
            KnittingFabricReceiveDetail oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
            oKnittingFabricReceiveDetail = MapObject(oReader);
            return oKnittingFabricReceiveDetail;
        }

        private List<KnittingFabricReceiveDetail> CreateObjects(IDataReader oReader)
        {
            List<KnittingFabricReceiveDetail> oKnittingFabricReceiveDetail = new List<KnittingFabricReceiveDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingFabricReceiveDetail oItem = CreateObject(oHandler);
                oKnittingFabricReceiveDetail.Add(oItem);
            }
            return oKnittingFabricReceiveDetail;
        }

        #endregion

        #region Interface implementation
        public KnittingFabricReceiveDetail Save(KnittingFabricReceiveDetail oKnittingFabricReceiveDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnittingFabricReceiveDetail.KnittingFabricReceiveDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingFabricReceiveDetail", EnumRoleOperationType.Add);
                    reader = KnittingFabricReceiveDetailDA.InsertUpdate(tc, oKnittingFabricReceiveDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingFabricReceiveDetail", EnumRoleOperationType.Edit);
                    reader = KnittingFabricReceiveDetailDA.InsertUpdate(tc, oKnittingFabricReceiveDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
                    oKnittingFabricReceiveDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
                    oKnittingFabricReceiveDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingFabricReceiveDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingFabricReceiveDetail oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
                oKnittingFabricReceiveDetail.KnittingFabricReceiveDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingFabricReceiveDetail", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingFabricReceiveDetail", id);
                KnittingFabricReceiveDetailDA.Delete(tc, oKnittingFabricReceiveDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public KnittingFabricReceiveDetail Get(int id, Int64 nUserId)
        {
            KnittingFabricReceiveDetail oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingFabricReceiveDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingFabricReceiveDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingFabricReceiveDetail", e);
                #endregion
            }
            return oKnittingFabricReceiveDetail;
        }

        public List<KnittingFabricReceiveDetail> Gets(Int64 nUserID)
        {
            List<KnittingFabricReceiveDetail> oKnittingFabricReceiveDetails = new List<KnittingFabricReceiveDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingFabricReceiveDetailDA.Gets(tc);
                oKnittingFabricReceiveDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingFabricReceiveDetail oKnittingFabricReceiveDetail = new KnittingFabricReceiveDetail();
                oKnittingFabricReceiveDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingFabricReceiveDetails;
        }

        public List<KnittingFabricReceiveDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingFabricReceiveDetail> oKnittingFabricReceiveDetails = new List<KnittingFabricReceiveDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingFabricReceiveDetailDA.Gets(tc, sSQL);
                oKnittingFabricReceiveDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingFabricReceiveDetail", e);
                #endregion
            }
            return oKnittingFabricReceiveDetails;
        }

        #endregion
    }

}
