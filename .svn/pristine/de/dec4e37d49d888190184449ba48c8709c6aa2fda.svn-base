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
    public class FabricPlanDetailService : MarshalByRefObject, IFabricPlanDetailService
    {
        #region Private functions and declaration

        private FabricPlanDetail MapObject(NullHandler oReader)
        {
            FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
            oFabricPlanDetail.FabricPlanDetailID = oReader.GetInt32("FabricPlanDetailID");
            oFabricPlanDetail.FabricPlanID = oReader.GetInt32("FabricPlanID");
            oFabricPlanDetail.ColNo = oReader.GetInt32("ColNo");
            oFabricPlanDetail.EndsCount = oReader.GetInt32("EndsCount");
            oFabricPlanDetail.SLNo = oReader.GetInt32("SLNo");
            oFabricPlanDetail.RepeatNo = oReader.GetInt32("RepeatNo");
            return oFabricPlanDetail;
        }

        private FabricPlanDetail CreateObject(NullHandler oReader)
        {
            FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
            oFabricPlanDetail = MapObject(oReader);
            return oFabricPlanDetail;
        }

        private List<FabricPlanDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricPlanDetail> oFabricPlanDetail = new List<FabricPlanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPlanDetail oItem = CreateObject(oHandler);
                oFabricPlanDetail.Add(oItem);
            }
            return oFabricPlanDetail;
        }

        #endregion

        #region Interface implementation
        public List<FabricPlanDetail> SaveAll(List<FabricPlanDetail> oFabricPlanDetails, Int64 nUserID)
        {
            FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
            List<FabricPlanDetail> oFabricPlanDetails_Return = new List<FabricPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;

                /// For Delete Set Ends=0
                if (oFabricPlanDetails.Count > 0)
                {
                    FabricPlanDetailDA.UpdateEnds(tc, oFabricPlanDetails[0].FabricPlanID);
                }

                foreach (FabricPlanDetail oItem in oFabricPlanDetails)
                {

                    if (oItem.FabricPlanDetailID <= 0)
                    {
                        reader = FabricPlanDetailDA.SaveAll(tc, oItem,EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {

                        reader = FabricPlanDetailDA.SaveAll(tc, oItem, EnumDBOperation.Update,nUserID);
                    }

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricPlanDetail = new FabricPlanDetail();
                        oFabricPlanDetail = CreateObject(oReader);
                        oFabricPlanDetails_Return.Add(oFabricPlanDetail);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricPlanDetails_Return = new List<FabricPlanDetail>();
                oFabricPlanDetail = new FabricPlanDetail();
                oFabricPlanDetail.ErrorMessage = e.Message.Split('~')[0];
                oFabricPlanDetails_Return.Add(oFabricPlanDetail);

                #endregion
            }
            return oFabricPlanDetails_Return;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
                oFabricPlanDetail.FabricPlanDetailID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricPlanDetail", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricPlanDetail", id);
                FabricPlanDetailDA.Delete(tc, oFabricPlanDetail, EnumDBOperation.Delete, nUserId);
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

        public FabricPlanDetail Get(int id, Int64 nUserId)
        {
            FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricPlanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricPlanDetail", e);
                #endregion
            }
            return oFabricPlanDetail;
        }

        public List<FabricPlanDetail> Gets(int nFabricPlanID, Int64 nUserID)
        {
            List<FabricPlanDetail> oFabricPlanDetails = new List<FabricPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanDetailDA.Gets(tc, nFabricPlanID);
                oFabricPlanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlanDetail oFabricPlanDetail = new FabricPlanDetail();
                oFabricPlanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricPlanDetails;
        }

        public List<FabricPlanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricPlanDetail> oFabricPlanDetails = new List<FabricPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanDetailDA.Gets(tc, sSQL);
                oFabricPlanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricPlanDetail", e);
                #endregion
            }
            return oFabricPlanDetails;
        }


        #endregion
    }

}
