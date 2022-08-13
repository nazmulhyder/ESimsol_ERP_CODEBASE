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
    public class KnittingYarnChallanDetailService : MarshalByRefObject, IKnittingYarnChallanDetailService
    {
        #region Private functions and declaration

        private KnittingYarnChallanDetail MapObject(NullHandler oReader)
        {
            KnittingYarnChallanDetail oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
            oKnittingYarnChallanDetail.KnittingYarnChallanDetailID = oReader.GetInt32("KnittingYarnChallanDetailID");
            oKnittingYarnChallanDetail.KnittingYarnChallanID = oReader.GetInt32("KnittingYarnChallanID");
            oKnittingYarnChallanDetail.KnittingCompositionID = oReader.GetInt32("KnittingCompositionID");
            oKnittingYarnChallanDetail.IssueStoreID = oReader.GetInt32("IssueStoreID");
            oKnittingYarnChallanDetail.YarnID = oReader.GetInt32("YarnID");
            oKnittingYarnChallanDetail.LotID = oReader.GetInt32("LotID");
            oKnittingYarnChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oKnittingYarnChallanDetail.Qty = oReader.GetDouble("Qty");
            oKnittingYarnChallanDetail.Remarks = oReader.GetString("Remarks");
            oKnittingYarnChallanDetail.OperationUnitName = oReader.GetString("OperationUnitName");
            oKnittingYarnChallanDetail.YarnName = oReader.GetString("YarnName");
            oKnittingYarnChallanDetail.YarnCode = oReader.GetString("YarnCode");
            oKnittingYarnChallanDetail.LotNo = oReader.GetString("LotNo");
            oKnittingYarnChallanDetail.LotBalance = oReader.GetDouble("LotBalance");
            oKnittingYarnChallanDetail.MUnitName = oReader.GetString("MUnitName");
            oKnittingYarnChallanDetail.ChallanNo = oReader.GetString("ChallanNo");
            oKnittingYarnChallanDetail.ChallanDate = oReader.GetDateTime("ChallanDate");
            oKnittingYarnChallanDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oKnittingYarnChallanDetail.ReturnBalance = oReader.GetDouble("ReturnBalance");
            oKnittingYarnChallanDetail.ChallanBalance = oReader.GetDouble("ChallanBalance");
            oKnittingYarnChallanDetail.BrandName = oReader.GetString("BrandName");
            oKnittingYarnChallanDetail.ColorID = oReader.GetInt32("ColorID");
            oKnittingYarnChallanDetail.ColorName = oReader.GetString("ColorName");
            oKnittingYarnChallanDetail.BagQty = oReader.GetDouble("BagQty");
            oKnittingYarnChallanDetail.KODColorName = oReader.GetString("KODColorName");
            oKnittingYarnChallanDetail.KODStyleNo = oReader.GetString("KODStyleNo");
            oKnittingYarnChallanDetail.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oKnittingYarnChallanDetail.BrandShortName = oReader.GetString("BrandShortName");
            oKnittingYarnChallanDetail.BuyerName = oReader.GetString("BuyerName");
            oKnittingYarnChallanDetail.PAM = oReader.GetInt32("PAM");
            oKnittingYarnChallanDetail.YetToChallanQty = oReader.GetDouble("YetToChallanQty");
            oKnittingYarnChallanDetail.KODQty = oReader.GetDouble("KODQty");
            oKnittingYarnChallanDetail.KODMUShortName = oReader.GetString("KODMUShortName");
            oKnittingYarnChallanDetail.KODOrderQty = oReader.GetDouble("KODOrderQty");

            return oKnittingYarnChallanDetail;
        }

        private KnittingYarnChallanDetail CreateObject(NullHandler oReader)
        {
            KnittingYarnChallanDetail oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
            oKnittingYarnChallanDetail = MapObject(oReader);
            return oKnittingYarnChallanDetail;
        }

        private List<KnittingYarnChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<KnittingYarnChallanDetail> oKnittingYarnChallanDetail = new List<KnittingYarnChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingYarnChallanDetail oItem = CreateObject(oHandler);
                oKnittingYarnChallanDetail.Add(oItem);
            }
            return oKnittingYarnChallanDetail;
        }

        #endregion

        #region Interface implementation
        public KnittingYarnChallanDetail Save(KnittingYarnChallanDetail oKnittingYarnChallanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnittingYarnChallanDetail.KnittingYarnChallanDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarnChallanDetail", EnumRoleOperationType.Add);
                    reader = KnittingYarnChallanDetailDA.InsertUpdate(tc, oKnittingYarnChallanDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarnChallanDetail", EnumRoleOperationType.Edit);
                    reader = KnittingYarnChallanDetailDA.InsertUpdate(tc, oKnittingYarnChallanDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
                    oKnittingYarnChallanDetail = CreateObject(oReader);
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
                    oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
                    oKnittingYarnChallanDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingYarnChallanDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingYarnChallanDetail oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
                oKnittingYarnChallanDetail.KnittingYarnChallanDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingYarnChallanDetail", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingYarnChallanDetail", id);
                KnittingYarnChallanDetailDA.Delete(tc, oKnittingYarnChallanDetail, EnumDBOperation.Delete, nUserId,"");
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

        public KnittingYarnChallanDetail Get(int id, Int64 nUserId)
        {
            KnittingYarnChallanDetail oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingYarnChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarnChallanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingYarnChallanDetail", e);
                #endregion
            }
            return oKnittingYarnChallanDetail;
        }

        public List<KnittingYarnChallanDetail> Gets(Int64 nUserID)
        {
            List<KnittingYarnChallanDetail> oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnChallanDetailDA.Gets(tc);
                oKnittingYarnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingYarnChallanDetail oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
                oKnittingYarnChallanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingYarnChallanDetails;
        }

        public List<KnittingYarnChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingYarnChallanDetail> oKnittingYarnChallanDetails = new List<KnittingYarnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnChallanDetailDA.Gets(tc, sSQL);
                oKnittingYarnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingYarnChallanDetail", e);
                #endregion
            }
            return oKnittingYarnChallanDetails;
        }

        #endregion
    }

}
