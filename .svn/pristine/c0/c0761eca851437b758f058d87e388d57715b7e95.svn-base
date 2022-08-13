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
    public class FabricSizingPlanDetailService : MarshalByRefObject, IFabricSizingPlanDetailService
    {
        #region Private functions and declaration

        private FabricSizingPlanDetail MapObject(NullHandler oReader)
        {
            FabricSizingPlanDetail oFabricSizingPlanDetail = new FabricSizingPlanDetail();
            oFabricSizingPlanDetail.FSPDID = oReader.GetInt32("FSPDID");
            oFabricSizingPlanDetail.FabricSizingPlanID = oReader.GetInt32("FabricSizingPlanID");
            oFabricSizingPlanDetail.FMID = oReader.GetInt32("FMID");
            oFabricSizingPlanDetail.FabricMachineTypeID = oReader.GetInt32("FabricMachineTypeID");
            oFabricSizingPlanDetail.SizingBeamNo = oReader.GetInt32("SizingBeamNo");
            oFabricSizingPlanDetail.Qty = oReader.GetDouble("Qty");
            oFabricSizingPlanDetail.FabricMachineTypeName = oReader.GetString("FabricMachineTypeName");
            oFabricSizingPlanDetail.FabricModelName = oReader.GetString("FabricModelName");
            if (oFabricSizingPlanDetail.FabricMachineTypeID > 0)
            {
                oFabricSizingPlanDetail.BeamName = "";
            }
            else
            {
                oFabricSizingPlanDetail.BeamName = oReader.GetString("BeamName");
            }

            return oFabricSizingPlanDetail;
        }

        private FabricSizingPlanDetail CreateObject(NullHandler oReader)
        {
            FabricSizingPlanDetail oFabricSizingPlanDetail = new FabricSizingPlanDetail();
            oFabricSizingPlanDetail = MapObject(oReader);
            return oFabricSizingPlanDetail;
        }

        private List<FabricSizingPlanDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricSizingPlanDetail> oFabricSizingPlanDetail = new List<FabricSizingPlanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSizingPlanDetail oItem = CreateObject(oHandler);
                oFabricSizingPlanDetail.Add(oItem);
            }
            return oFabricSizingPlanDetail;
        }

        #endregion

        #region Interface implementation


        public FabricSizingPlanDetail Get(int id, Int64 nUserId)
        {
            FabricSizingPlanDetail oFabricSizingPlanDetail = new FabricSizingPlanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricSizingPlanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSizingPlanDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSizingPlanDetail", e);
                #endregion
            }
            return oFabricSizingPlanDetail;
        }

        public List<FabricSizingPlanDetail> Gets(int nFabricSizingPlanID, Int64 nUserID)
        {
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSizingPlanDetailDA.Gets(tc, nFabricSizingPlanID);
                oFabricSizingPlanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricSizingPlanDetail oFabricSizingPlanDetail = new FabricSizingPlanDetail();
                oFabricSizingPlanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricSizingPlanDetails;
        }

        public List<FabricSizingPlanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricSizingPlanDetail> oFabricSizingPlanDetails = new List<FabricSizingPlanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricSizingPlanDetailDA.Gets(tc, sSQL);
                oFabricSizingPlanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSizingPlanDetail", e);
                #endregion
            }
            return oFabricSizingPlanDetails;
        }
        public string Delete(FabricSizingPlanDetail oFabricSizingPlanDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.FabricSizingPlan, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "FabricSizingPlan", oFabricSizingPlan.FabricSizingPlanID);
                FabricSizingPlanDetailDA.Delete(tc, oFabricSizingPlanDetail, EnumDBOperation.Delete, nUserId);
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
        #endregion
    }

}
