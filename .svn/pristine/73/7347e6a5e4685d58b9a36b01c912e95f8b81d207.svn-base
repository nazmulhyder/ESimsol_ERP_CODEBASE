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
    public class AdjustmentRegisterService : MarshalByRefObject, IAdjustmentRegisterService
    {
        #region Private functions and declaration
        private AdjustmentRegister MapObject(NullHandler oReader)
        {
            AdjustmentRegister oAdjustmentRegister = new AdjustmentRegister();
            oAdjustmentRegister.AdjustmentRequisitionSlipDetailID= oReader.GetInt32("DeliveryOrderDetailID");
            oAdjustmentRegister.AdjustmentRequisitionSlipID = oReader.GetInt32("DeliveryOrderID");
            oAdjustmentRegister.ProductID = oReader.GetInt32("ProductID");
            oAdjustmentRegister.StyleID = oReader.GetInt32("MUnitID");
            oAdjustmentRegister.Qty = oReader.GetDouble("Qty");
            oAdjustmentRegister.SupplierID = oReader.GetInt32("SupplierID");
            oAdjustmentRegister.BUID = oReader.GetInt32("BUID");
            oAdjustmentRegister.ExecutionDate = oReader.GetDateTime("ExecutionDate");
            oAdjustmentRegister.LotID = oReader.GetInt32("LotID");
            oAdjustmentRegister.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oAdjustmentRegister.Note = oReader.GetString("Note");
            oAdjustmentRegister.LotNo = oReader.GetString("LotNo");
            oAdjustmentRegister.ProductCode = oReader.GetString("ProductCode");
            oAdjustmentRegister.ProductName = oReader.GetString("ProductName");
            oAdjustmentRegister.ColorName = oReader.GetString("ColorName");
            oAdjustmentRegister.MUName = oReader.GetString("MUName");
            oAdjustmentRegister.LogNo = oReader.GetString("LogNo");
            oAdjustmentRegister.StyleNo = oReader.GetString("StyleNo");
            oAdjustmentRegister.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oAdjustmentRegister.SupplierName = oReader.GetString("SupplierName");
            oAdjustmentRegister.ARSlipNo = oReader.GetString("ARSlipNo");
            oAdjustmentRegister.Date = oReader.GetDateTime("Date");
            oAdjustmentRegister.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oAdjustmentRegister.WUName = oReader.GetString("WUName");
            return oAdjustmentRegister;
        }

        private AdjustmentRegister CreateObject(NullHandler oReader)
        {
            AdjustmentRegister oAdjustmentRegister = new AdjustmentRegister();
            oAdjustmentRegister = MapObject(oReader);
            return oAdjustmentRegister;
        }

        private List<AdjustmentRegister> CreateObjects(IDataReader oReader)
        {
            List<AdjustmentRegister> oAdjustmentRegister = new List<AdjustmentRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AdjustmentRegister oItem = CreateObject(oHandler);
                oAdjustmentRegister.Add(oItem);
            }
            return oAdjustmentRegister;
        }

        #endregion

        #region Interface implementation
        public AdjustmentRegisterService() { }        
        public List<AdjustmentRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<AdjustmentRegister> oAdjustmentRegister = new List<AdjustmentRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = AdjustmentRegisterDA.Gets(tc, sSQL);
                oAdjustmentRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AdjustmentRegister", e);
                #endregion
            }

            return oAdjustmentRegister;
        }
        #endregion
    }
}
