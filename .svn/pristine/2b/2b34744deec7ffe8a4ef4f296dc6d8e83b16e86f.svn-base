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
    public class RMRequisitionMaterialService : MarshalByRefObject, IRMRequisitionMaterialService
    {
        #region Private functions and declaration

        private static RMRequisitionMaterial MapObject(NullHandler oReader)
        {
            RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
            oRMRequisitionMaterial.RMRequisitionMaterialID = oReader.GetInt32("RMRequisitionMaterialID");
            oRMRequisitionMaterial.RMRequisitionID = oReader.GetInt32("RMRequisitionID");
            oRMRequisitionMaterial.ProductionRecipeID = oReader.GetInt32("ProductionRecipeID");
            oRMRequisitionMaterial.ProductID = oReader.GetInt32("ProductID");
            oRMRequisitionMaterial.RequiredQty = oReader.GetDouble("RequiredQty");
            oRMRequisitionMaterial.OutQty = oReader.GetDouble("OutQty");
            oRMRequisitionMaterial.ProductionRecipeID = oReader.GetDouble("ProductionRecipeID");
            oRMRequisitionMaterial.Remarks = oReader.GetString("Remarks");           
            oRMRequisitionMaterial.ProductCode = oReader.GetString("ProductCode");
            oRMRequisitionMaterial.ProductName = oReader.GetString("ProductName");
            oRMRequisitionMaterial.MUnitID = oReader.GetInt32("MUnitID");
            oRMRequisitionMaterial.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oRMRequisitionMaterial.MUName = oReader.GetString("MUName");           
            oRMRequisitionMaterial.QtyType = (EnumQtyType)oReader.GetInt32("QtyType");
            oRMRequisitionMaterial.QtyTypeInt = oReader.GetInt32("QtyType");
            oRMRequisitionMaterial.MUSymbol = oReader.GetString("MUSymbol");
            oRMRequisitionMaterial.SheetNoWithDescription = oReader.GetString("SheetNoWithDescription");
            oRMRequisitionMaterial.SheetNo = oReader.GetString("SheetNo");
            oRMRequisitionMaterial.YetToRequisitionQty = oReader.GetDouble("YetToRequisitionQty");
            oRMRequisitionMaterial.Qty = oReader.GetDouble("Qty");
            oRMRequisitionMaterial.MaterialOutQty = oReader.GetDouble("MaterialOutQty");
            oRMRequisitionMaterial.ReportingUnit = oReader.GetString("ReportingUnit");
            oRMRequisitionMaterial.ReportingQty = oReader.GetDouble("ReportingQty");
            oRMRequisitionMaterial.RequisitionNo = oReader.GetString("RequisitionNo");
            oRMRequisitionMaterial.YetToOutQty = oReader.GetDouble("YetToOutQty");
            return oRMRequisitionMaterial;
        }

        public static RMRequisitionMaterial CreateObject(NullHandler oReader)
        {
            RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
            oRMRequisitionMaterial = MapObject(oReader);
            return oRMRequisitionMaterial;
        }

        private List<RMRequisitionMaterial> CreateObjects(IDataReader oReader)
        {
            List<RMRequisitionMaterial> oRMRequisitionMaterial = new List<RMRequisitionMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMRequisitionMaterial oItem = CreateObject(oHandler);
                oRMRequisitionMaterial.Add(oItem);
            }
            return oRMRequisitionMaterial;
        }

        #endregion

        #region Interface implementation

        public RMRequisitionMaterial Get(int id, Int64 nUserId)
        {
            RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RMRequisitionMaterialDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisitionMaterial = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RMRequisitionMaterial", e);
                #endregion
            }
            return oRMRequisitionMaterial;
        }

        public List<RMRequisitionMaterial> Gets(int nRMRequisitionID, Int64 nUserID)
        {
            List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMRequisitionMaterialDA.Gets(nRMRequisitionID, tc);
                oRMRequisitionMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oRMRequisitionMaterials;
        }
        public List<RMRequisitionMaterial> Gets(string sSQL, Int64 nUserID)
        {
            List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMRequisitionMaterialDA.Gets(tc, sSQL);
                oRMRequisitionMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMRequisitionMaterial", e);
                #endregion
            }
            return oRMRequisitionMaterials;
        }
        public RMRequisitionMaterial CommitRawMaterialOut(RMRequisitionMaterial oRMRequisitionMaterial, EnumTriggerParentsType eTriggerParentType, Int64 nUserID)
        {
            TransactionContext tc = null;
            RMRequisitionMaterial oTempRMRequisitionMaterial = new RMRequisitionMaterial();
            List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;

                foreach (RMRequisitionMaterial oItem in oRMRequisitionMaterial.RMRequisitionMaterials)
                {
                    oItem.WUID = oRMRequisitionMaterial.WUID;
                    reader = RMRequisitionMaterialDA.CommitRawMaterialOut(tc, oItem, eTriggerParentType, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempRMRequisitionMaterial = new RMRequisitionMaterial();
                        oTempRMRequisitionMaterial = RMRequisitionMaterialService.CreateObject(oReader);
                        oRMRequisitionMaterials.Add(oTempRMRequisitionMaterial);
                    }
                    reader.Close();
                }
                oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = "";
                oRMRequisitionMaterial.RMRequisitionMaterials = oRMRequisitionMaterials;
                tc.End();
            }
            catch (Exception e)
            {
                oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = e.Message;
            }
            return oRMRequisitionMaterial;
        }

        public RMRequisitionMaterial ReceiveReturnQty(RMRequisitionMaterial oRMRequisitionMaterial, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true); 
                foreach (ITransaction oItem in oRMRequisitionMaterial.ITransactions)
                {
                    RMRequisitionMaterialDA.ReceiveReturnQty(tc, oItem, (int)EnumTriggerParentsType.RawMaterial_Return, nUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                oRMRequisitionMaterial = new RMRequisitionMaterial();
                oRMRequisitionMaterial.ErrorMessage = e.Message;
            }
            return oRMRequisitionMaterial;
        }
        #endregion
    }
}
