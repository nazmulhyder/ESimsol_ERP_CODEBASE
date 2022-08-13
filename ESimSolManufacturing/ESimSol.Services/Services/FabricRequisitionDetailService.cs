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
    public class FabricRequisitionDetailService : MarshalByRefObject, IFabricRequisitionDetailService
    {
        #region Private functions and declaration

        private FabricRequisitionDetail MapObject(NullHandler oReader)
        {
            FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
            oFabricRequisitionDetail.FabricRequisitionDetailID = oReader.GetInt32("FabricRequisitionDetailID");
            oFabricRequisitionDetail.FabricRequisitionID = oReader.GetInt32("FabricRequisitionID");
            oFabricRequisitionDetail.FEOSID = oReader.GetInt32("FEOSID");
            oFabricRequisitionDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFabricRequisitionDetail.ReqQty = oReader.GetDouble("ReqQty");
            oFabricRequisitionDetail.ExeNo = oReader.GetString("ExeNo");
            return oFabricRequisitionDetail;
        }

        private FabricRequisitionDetail CreateObject(NullHandler oReader)
        {
            FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
            oFabricRequisitionDetail = MapObject(oReader);
            return oFabricRequisitionDetail;
        }

        private List<FabricRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricRequisitionDetail> oFabricRequisitionDetail = new List<FabricRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricRequisitionDetail oItem = CreateObject(oHandler);
                oFabricRequisitionDetail.Add(oItem);
            }
            return oFabricRequisitionDetail;
        }

        #endregion

        #region Interface implementation
        public FabricRequisitionDetail Save(FabricRequisitionDetail oFabricRequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricRequisitionDetail.FabricRequisitionDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisitionDetail", EnumRoleOperationType.Add);
                    reader = FabricRequisitionDetailDA.InsertUpdate(tc, oFabricRequisitionDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricRequisitionDetail", EnumRoleOperationType.Edit);
                    reader = FabricRequisitionDetailDA.InsertUpdate(tc, oFabricRequisitionDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisitionDetail = new FabricRequisitionDetail();
                    oFabricRequisitionDetail = CreateObject(oReader);
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
                    oFabricRequisitionDetail = new FabricRequisitionDetail();
                    oFabricRequisitionDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricRequisitionDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
                oFabricRequisitionDetail.FabricRequisitionDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricRequisitionDetail", EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "FabricRequisitionDetail", id);
                FabricRequisitionDetailDA.Delete(tc, oFabricRequisitionDetail, EnumDBOperation.Delete, nUserId,"");
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

        public FabricRequisitionDetail Get(int id, Int64 nUserId)
        {
            FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricRequisitionDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricRequisitionDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricRequisitionDetail", e);
                #endregion
            }
            return oFabricRequisitionDetail;
        }

        public List<FabricRequisitionDetail> Gets(Int64 nUserID)
        {
            List<FabricRequisitionDetail> oFabricRequisitionDetails = new List<FabricRequisitionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionDetailDA.Gets(tc);
                oFabricRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
                oFabricRequisitionDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRequisitionDetails;
        }

        public List<FabricRequisitionDetail> Gets(int id, Int64 nUserID)
        {
            List<FabricRequisitionDetail> oFabricRequisitionDetails = new List<FabricRequisitionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionDetailDA.Gets(tc, id);
                oFabricRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricRequisitionDetail oFabricRequisitionDetail = new FabricRequisitionDetail();
                oFabricRequisitionDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricRequisitionDetails;
        }

        public List<FabricRequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricRequisitionDetail> oFabricRequisitionDetails = new List<FabricRequisitionDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricRequisitionDetailDA.Gets(tc, sSQL);
                oFabricRequisitionDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricRequisitionDetail", e);
                #endregion
            }
            return oFabricRequisitionDetails;
        }

        #endregion
    }

}
