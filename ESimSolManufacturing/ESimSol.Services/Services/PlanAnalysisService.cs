using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class PlanAnalysisService : MarshalByRefObject, IPlanAnalysisService
    {
        #region Private functions and declaration
        private PlanAnalysis MapObject(NullHandler oReader)
        {
            PlanAnalysis oPlanAnalysis = new PlanAnalysis();
            oPlanAnalysis.TempID = oReader.GetInt32("TempID");
            oPlanAnalysis.PlanDate = oReader.GetDateTime("PlanDate");
            oPlanAnalysis.PlanQty = oReader.GetInt32("PlanQty");
            oPlanAnalysis.TotalPlanQty = oReader.GetInt32("TotalPlanQty");
            oPlanAnalysis.ExecutionStepQty = oReader.GetInt32("ExecutionStepQty");
            oPlanAnalysis.TotalExecutionStepQty = oReader.GetInt32("TotalExecutionStepQty");
            oPlanAnalysis.MaxExecutionDate = oReader.GetDateTime("MaxExecutionDate");
            oPlanAnalysis.TotalQtyOfMaxExecutionDate = oReader.GetDouble("TotalQtyOfMaxExecutionDate");
            return oPlanAnalysis;
        }

        private PlanAnalysis CreateObject(NullHandler oReader)
        {
            PlanAnalysis oPlanAnalysis = new PlanAnalysis();
            oPlanAnalysis = MapObject(oReader);
            return oPlanAnalysis;
        }

        private List<PlanAnalysis> CreateObjects(IDataReader oReader)
        {
            List<PlanAnalysis> oPlanAnalysis = new List<PlanAnalysis>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PlanAnalysis oItem = CreateObject(oHandler);
                oPlanAnalysis.Add(oItem);
            }
            return oPlanAnalysis;
        }

        #endregion

        #region Interface implementation


        public List<PlanAnalysis> Gets(int stepID, int ProductionOrderID, Int64 nUserID)
        {
            List<PlanAnalysis> oPlanAnalysis = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PlanAnalysisDA.Gets(stepID, ProductionOrderID,tc);
                oPlanAnalysis = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PlanAnalysis", e);
                #endregion
            }

            return oPlanAnalysis;
        }
        #endregion
    }   

}
