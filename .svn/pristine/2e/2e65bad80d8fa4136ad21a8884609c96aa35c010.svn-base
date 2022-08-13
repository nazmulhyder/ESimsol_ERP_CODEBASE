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
    public class FabricPlanningDetailService : MarshalByRefObject, IFabricPlanningDetailService
    {
        #region Private functions and declaration

        private FabricPlanningDetail MapObject(NullHandler oReader)
        {
            FabricPlanningDetail oFabricPlanningDetail = new FabricPlanningDetail();
            oFabricPlanningDetail.FabricPlanningDetailID = oReader.GetInt32("FabricPlanningDetailID");
            oFabricPlanningDetail.FabricPlanningID = oReader.GetInt32("FabricPlanningID");
            oFabricPlanningDetail.ColNo = oReader.GetInt32("ColNo");
            oFabricPlanningDetail.EndsCount = oReader.GetInt32("EndsCount");
            oFabricPlanningDetail.SLNo = oReader.GetInt32("SLNo");
            oFabricPlanningDetail.RepeatNo = oReader.GetInt32("RepeatNo");
            return oFabricPlanningDetail;
        }

        private FabricPlanningDetail CreateObject(NullHandler oReader)
        {
            FabricPlanningDetail oFabricPlanningDetail = new FabricPlanningDetail();
            oFabricPlanningDetail = MapObject(oReader);
            return oFabricPlanningDetail;
        }

        private List<FabricPlanningDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricPlanningDetail> oFabricPlanningDetail = new List<FabricPlanningDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPlanningDetail oItem = CreateObject(oHandler);
                oFabricPlanningDetail.Add(oItem);
            }
            return oFabricPlanningDetail;
        }

        #endregion

        #region Interface implementation
        public FabricPlanningDetail Save(FabricPlanningDetail oFabricPlanningDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricPlanningDetail.FabricPlanningDetailID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricPlanningDetail", EnumRoleOperationType.Add);
                    reader = FabricPlanningDetailDA.InsertUpdate(tc, oFabricPlanningDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "FabricPlanningDetail", EnumRoleOperationType.Edit);
                    reader = FabricPlanningDetailDA.InsertUpdate(tc, oFabricPlanningDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanningDetail = new FabricPlanningDetail();
                    oFabricPlanningDetail = CreateObject(oReader);
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
                    oFabricPlanningDetail = new FabricPlanningDetail();
                    oFabricPlanningDetail.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricPlanningDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricPlanningDetail oFabricPlanningDetail = new FabricPlanningDetail();
                oFabricPlanningDetail.FabricPlanningDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricPlanningDetail", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricPlanningDetail", id);
                FabricPlanningDetailDA.Delete(tc, oFabricPlanningDetail, EnumDBOperation.Delete, nUserId);
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

        public FabricPlanningDetail Get(int id, Int64 nUserId)
        {
            FabricPlanningDetail oFabricPlanningDetail = new FabricPlanningDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricPlanningDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanningDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricPlanningDetail", e);
                #endregion
            }
            return oFabricPlanningDetail;
        }

        public List<FabricPlanningDetail> Gets(Int64 nUserID)
        {
            List<FabricPlanningDetail> oFabricPlanningDetails = new List<FabricPlanningDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanningDetailDA.Gets(tc);
                oFabricPlanningDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlanningDetail oFabricPlanningDetail = new FabricPlanningDetail();
                oFabricPlanningDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricPlanningDetails;
        }

        public List<FabricPlanningDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricPlanningDetail> oFabricPlanningDetails = new List<FabricPlanningDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanningDetailDA.Gets(tc, sSQL);
                oFabricPlanningDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricPlanningDetail", e);
                #endregion
            }
            return oFabricPlanningDetails;
        }

        #endregion
    }

}
