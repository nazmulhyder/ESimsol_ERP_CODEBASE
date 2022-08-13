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
    public class AdjustmentRequisitionSlipRegisterService : MarshalByRefObject, IAdjustmentRequisitionSlipRegisterService
    {
        #region Private functions and declaration
        private AdjustmentRequisitionSlipRegister MapObject(NullHandler oReader)
        {
            AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister = new AdjustmentRequisitionSlipRegister();
            oAdjustmentRequisitionSlipRegister.AdjustmentRequisitionSlipDetailID = oReader.GetInt32("AdjustmentRequisitionSlipDetailID");
            oAdjustmentRequisitionSlipRegister.AdjustmentRequisitionSlipID = oReader.GetInt32("AdjustmentRequisitionSlipID");
            oAdjustmentRequisitionSlipRegister.ProductID = oReader.GetInt32("ProductID");
            oAdjustmentRequisitionSlipRegister.MUnitID = oReader.GetInt32("MUnitID");
            oAdjustmentRequisitionSlipRegister.Qty = oReader.GetDouble("Qty");
            oAdjustmentRequisitionSlipRegister.ARSlipNo = oReader.GetString("ARSlipNo");
            oAdjustmentRequisitionSlipRegister.BUID = oReader.GetInt32("BUID");
            oAdjustmentRequisitionSlipRegister.RequestedByID = oReader.GetInt32("RequestedByID");
            oAdjustmentRequisitionSlipRegister.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oAdjustmentRequisitionSlipRegister.AdjustmentType = (EnumAdjustmentType)oReader.GetInt32("AdjustmentType");
            oAdjustmentRequisitionSlipRegister.AprovedByID = oReader.GetInt32("AprovedByID");
            oAdjustmentRequisitionSlipRegister.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oAdjustmentRequisitionSlipRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            oAdjustmentRequisitionSlipRegister.RequestedDate = oReader.GetDateTime("RequestedDate");
            oAdjustmentRequisitionSlipRegister.Date = oReader.GetDateTime("Date");
            oAdjustmentRequisitionSlipRegister.RequestByName = oReader.GetString("RequestByName");
            oAdjustmentRequisitionSlipRegister.Remarks = oReader.GetString("Remarks");
            oAdjustmentRequisitionSlipRegister.ApproveByName = oReader.GetString("ApproveByName");
            oAdjustmentRequisitionSlipRegister.ProductCode = oReader.GetString("ProductCode");
            oAdjustmentRequisitionSlipRegister.ProductName = oReader.GetString("ProductName");
            oAdjustmentRequisitionSlipRegister.MUName = oReader.GetString("MUName");
            oAdjustmentRequisitionSlipRegister.MUSymbol = oReader.GetString("MUSymbol");
            oAdjustmentRequisitionSlipRegister.WUName = oReader.GetString("WUName");
            oAdjustmentRequisitionSlipRegister.LotNo = oReader.GetString("LotNo");
            oAdjustmentRequisitionSlipRegister.InOutType = (EnumInOutType) oReader.GetInt32("InOutType");
            
            return oAdjustmentRequisitionSlipRegister;
        }

        private AdjustmentRequisitionSlipRegister CreateObject(NullHandler oReader)
        {
            AdjustmentRequisitionSlipRegister oAdjustmentRequisitionSlipRegister = new AdjustmentRequisitionSlipRegister();
            oAdjustmentRequisitionSlipRegister = MapObject(oReader);
            return oAdjustmentRequisitionSlipRegister;
        }

        private List<AdjustmentRequisitionSlipRegister> CreateObjects(IDataReader oReader)
        {
            List<AdjustmentRequisitionSlipRegister> oAdjustmentRequisitionSlipRegister = new List<AdjustmentRequisitionSlipRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AdjustmentRequisitionSlipRegister oItem = CreateObject(oHandler);
                oAdjustmentRequisitionSlipRegister.Add(oItem);
            }
            return oAdjustmentRequisitionSlipRegister;
        }

        #endregion

        #region Interface implementation
        public AdjustmentRequisitionSlipRegisterService() { }        
        public List<AdjustmentRequisitionSlipRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<AdjustmentRequisitionSlipRegister> oAdjustmentRequisitionSlipRegister = new List<AdjustmentRequisitionSlipRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AdjustmentRequisitionSlipRegisterDA.Gets(tc, sSQL);
                oAdjustmentRequisitionSlipRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AdjustmentRequisitionSlipRegister", e);
                #endregion
            }

            return oAdjustmentRequisitionSlipRegister;
        }
        #endregion
    }
}
