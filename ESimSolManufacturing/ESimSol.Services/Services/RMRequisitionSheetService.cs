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

    public class RMRequisitionSheetService : MarshalByRefObject, IRMRequisitionSheetService
    {
        #region Private functions and declaration

        private static RMRequisitionSheet MapObject(NullHandler oReader)
        {
            RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
            oRMRequisitionSheet.RMRequisitionSheetID = oReader.GetInt32("RMRequisitionSheetID");
            oRMRequisitionSheet.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oRMRequisitionSheet.RMRequisitionID = oReader.GetInt32("RMRequisitionID");
            oRMRequisitionSheet.YetToPPQty = oReader.GetDouble("YetToPPQty");
            oRMRequisitionSheet.SheetQty = oReader.GetDouble("SheetQty");
            oRMRequisitionSheet.PPQty = oReader.GetDouble("PPQty");
            oRMRequisitionSheet.Remarks = oReader.GetString("Remarks");           
            oRMRequisitionSheet.ProductCode = oReader.GetString("ProductCode");
            oRMRequisitionSheet.ProductName = oReader.GetString("ProductName");
            oRMRequisitionSheet.UnitID = oReader.GetInt32("UnitID");
            oRMRequisitionSheet.UnitSymbol = oReader.GetString("UnitSymbol");
            oRMRequisitionSheet.SheetNo = oReader.GetString("SheetNo");
            oRMRequisitionSheet.BuyerName = oReader.GetString("BuyerName");
            oRMRequisitionSheet.ApprovedByName = oReader.GetString("ApprovedByName");
            oRMRequisitionSheet.ExportPINo = oReader.GetString("ExportPINo");
            
            return oRMRequisitionSheet;
        }

        public static RMRequisitionSheet CreateObject(NullHandler oReader)
        {
            RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
            oRMRequisitionSheet = MapObject(oReader);
            return oRMRequisitionSheet;
        }

        private List<RMRequisitionSheet> CreateObjects(IDataReader oReader)
        {
            List<RMRequisitionSheet> oRMRequisitionSheet = new List<RMRequisitionSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMRequisitionSheet oItem = CreateObject(oHandler);
                oRMRequisitionSheet.Add(oItem);
            }
            return oRMRequisitionSheet;
        }

        #endregion

        #region Interface implementation
        public RMRequisitionSheet Get(int id, Int64 nUserId)
        {
            RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RMRequisitionSheetDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisitionSheet = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RMRequisitionSheet", e);
                #endregion
            }
            return oRMRequisitionSheet;
        }
        public List<RMRequisitionSheet> Gets(int nRMRequisitionID, Int64 nUserID)
        {
            List<RMRequisitionSheet> oRMRequisitionSheets = new List<RMRequisitionSheet>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMRequisitionSheetDA.Gets(nRMRequisitionID, tc);
                oRMRequisitionSheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
                oRMRequisitionSheet.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oRMRequisitionSheets;
        }
        public List<RMRequisitionSheet> Gets(string sSQL, Int64 nUserID)
        {
            List<RMRequisitionSheet> oRMRequisitionSheets = new List<RMRequisitionSheet>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMRequisitionSheetDA.Gets(tc, sSQL);
                oRMRequisitionSheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMRequisitionSheet", e);
                #endregion
            }
            return oRMRequisitionSheets;
        }


        #endregion
    }
    
   
}
