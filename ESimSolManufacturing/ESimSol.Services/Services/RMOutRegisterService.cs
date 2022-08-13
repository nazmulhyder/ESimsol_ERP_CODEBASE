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
    public class RMOutRegisterService : MarshalByRefObject, IRMOutRegisterService
    {
        #region Private functions and declaration
        private RMOutRegister MapObject(NullHandler oReader)
        {
            RMOutRegister oRMOutRegister = new RMOutRegister();
            oRMOutRegister.BUID = oReader.GetInt32("BUID");
            oRMOutRegister.RMRequisitionID = oReader.GetInt32("RMRequisitionID");
            oRMOutRegister.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oRMOutRegister.ProductionRecipeID = oReader.GetInt32("ProductionRecipeID");
            oRMOutRegister.RMProductID = oReader.GetInt32("RMProductID");
            oRMOutRegister.RMProductCode = oReader.GetString("RMProductCode");
            oRMOutRegister.RMProductName = oReader.GetString("RMProductName");
            oRMOutRegister.RequisitionQty = oReader.GetDouble("RequisitionQty");
            oRMOutRegister.RMOutQty = oReader.GetDouble("RMOutQty");
            oRMOutRegister.RemainingQty = oReader.GetDouble("RemainingQty");
            oRMOutRegister.RMUnitSymbol = oReader.GetString("RMUnitSymbol");
            oRMOutRegister.ProductionSheetNo = oReader.GetString("ProductionSheetNo");
            oRMOutRegister.ExportPIID = oReader.GetInt32("ExportPIID");
            oRMOutRegister.ExportPINo = oReader.GetString("ExportPINo");
            oRMOutRegister.ContractorName = oReader.GetString("ContractorName");
            oRMOutRegister.FinishGoodsID = oReader.GetInt32("FinishGoodsID");
            oRMOutRegister.FinishGoodsName = oReader.GetString("FinishGoodsName");
            oRMOutRegister.FinisgGoodsQty = oReader.GetDouble("FinisgGoodsQty");
            oRMOutRegister.RMRequisitionNo = oReader.GetString("RMRequisitionNo");
            oRMOutRegister.RMRequisitionDate = oReader.GetDateTime("RMRequisitionDate");
            oRMOutRegister.FGUnitSymbol = oReader.GetString("FGUnitSymbol");
            return oRMOutRegister;
        }

        private RMOutRegister CreateObject(NullHandler oReader)
        {
            RMOutRegister oRMOutRegister = new RMOutRegister();
            oRMOutRegister = MapObject(oReader);
            return oRMOutRegister;
        }

        private List<RMOutRegister> CreateObjects(IDataReader oReader)
        {
            List<RMOutRegister> oRMOutRegister = new List<RMOutRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMOutRegister oItem = CreateObject(oHandler);
                oRMOutRegister.Add(oItem);
            }
            return oRMOutRegister;
        }

        #endregion

        #region Interface implementation
        public RMOutRegisterService() { }        
        public List<RMOutRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<RMOutRegister> oRMOutRegister = new List<RMOutRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMOutRegisterDA.Gets(tc, sSQL);
                oRMOutRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMOutRegister", e);
                #endregion
            }

            return oRMOutRegister;
        }
        #endregion
    }
}
