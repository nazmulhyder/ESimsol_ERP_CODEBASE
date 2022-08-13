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
    public class FabricClaimDetailService : MarshalByRefObject, IFabricClaimDetailService
    {
        #region Private functions and declaration

        private FabricClaimDetail MapObject(NullHandler oReader)
        {
            FabricClaimDetail oFabricClaimDetail = new FabricClaimDetail();
            oFabricClaimDetail.FabricClaimDetailID = oReader.GetInt32("FabricClaimDetailID");
            oFabricClaimDetail.FabricClaimID = oReader.GetInt32("FabricClaimID");
            oFabricClaimDetail.ClaimSettlementType = (EnumImportClaimSettleType)oReader.GetInt32("ClaimSettlementType");
            oFabricClaimDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFabricClaimDetail.ParentFSCDID = oReader.GetInt32("ParentFSCDID");
            oFabricClaimDetail.ParentExeNo = oReader.GetString("ParentExeNo");
            oFabricClaimDetail.QtyInPercent = oReader.GetDouble("QtyInPercent");
            oFabricClaimDetail.Remarks = oReader.GetString("Remarks");
            oFabricClaimDetail.Amount = oReader.GetDouble("Amount");
            oFabricClaimDetail.ProductCode = oReader.GetString("ProductCode");
            oFabricClaimDetail.ProductName = oReader.GetString("ProductName");
            oFabricClaimDetail.ProductCount = oReader.GetString("ProductCount");
            oFabricClaimDetail.ExeNoFull = oReader.GetString("ExeNoFull");
            oFabricClaimDetail.MUName = oReader.GetString("MUName");
            oFabricClaimDetail.FabricNo = oReader.GetString("FabricNo");
            oFabricClaimDetail.FabricNum = oReader.GetString("FabricNum");
            oFabricClaimDetail.ConstructionPI = oReader.GetString("ConstructionPI");
            oFabricClaimDetail.OptionNo = oReader.GetString("OptionNo");
            oFabricClaimDetail.Qty_PI = oReader.GetDouble("Qty_PI");
            oFabricClaimDetail.FabricDesignName = oReader.GetString("FabricDesignName");
            oFabricClaimDetail.FabricProductName = oReader.GetString("FabricProductName");
            oFabricClaimDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFabricClaimDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFabricClaimDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oFabricClaimDetail.LDNo = oReader.GetString("LDNo");
            oFabricClaimDetail.OwnLDNo = oReader.GetString("OwnLDNo");
            oFabricClaimDetail.ShadeCount = oReader.GetInt32("ShadeCount");
            oFabricClaimDetail.Code = oReader.GetString("Code");
            return oFabricClaimDetail;
        }

        private FabricClaimDetail CreateObject(NullHandler oReader)
        {
            FabricClaimDetail oFabricClaimDetail = new FabricClaimDetail();
            oFabricClaimDetail = MapObject(oReader);
            return oFabricClaimDetail;
        }

        private List<FabricClaimDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricClaimDetail> oFabricClaimDetail = new List<FabricClaimDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricClaimDetail oItem = CreateObject(oHandler);
                oFabricClaimDetail.Add(oItem);
            }
            return oFabricClaimDetail;
        }

        #endregion

        #region Interface implementation
        public FabricClaimDetail Save(FabricClaimDetail oFabricClaimDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricClaimDetail.FabricClaimDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricClaimDetail", EnumRoleOperationType.Add);
                    reader = FabricClaimDetailDA.InsertUpdate(tc, oFabricClaimDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricClaimDetail", EnumRoleOperationType.Edit);
                    reader = FabricClaimDetailDA.InsertUpdate(tc, oFabricClaimDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricClaimDetail = new FabricClaimDetail();
                    oFabricClaimDetail = CreateObject(oReader);
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
                    oFabricClaimDetail = new FabricClaimDetail();
                    oFabricClaimDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricClaimDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricClaimDetail oFabricClaimDetail = new FabricClaimDetail();
                oFabricClaimDetail.FabricClaimDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricClaimDetail", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricClaimDetail", id);
                FabricClaimDetailDA.Delete(tc, oFabricClaimDetail, EnumDBOperation.Delete, nUserId, "");
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

        public FabricClaimDetail Get(int id, Int64 nUserId)
        {
            FabricClaimDetail oFabricClaimDetail = new FabricClaimDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricClaimDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricClaimDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricClaimDetail", e);
                #endregion
            }
            return oFabricClaimDetail;
        }

        public List<FabricClaimDetail> Gets(Int64 nUserID)
        {
            List<FabricClaimDetail> oFabricClaimDetails = new List<FabricClaimDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricClaimDetailDA.Gets(tc);
                oFabricClaimDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricClaimDetail oFabricClaimDetail = new FabricClaimDetail();
                oFabricClaimDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricClaimDetails;
        }

        public List<FabricClaimDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricClaimDetail> oFabricClaimDetails = new List<FabricClaimDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricClaimDetailDA.Gets(tc, sSQL);
                oFabricClaimDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricClaimDetail", e);
                #endregion
            }
            return oFabricClaimDetails;
        }

        #endregion
    }

}
