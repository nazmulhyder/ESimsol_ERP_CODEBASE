using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class InventoryTrakingService : MarshalByRefObject, IInventoryTrakingService
    {
       
        #region Private functions and declaration
        private InventoryTraking MapObject(NullHandler oReader)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oInventoryTraking.StartDate = oReader.GetDateTime("StartDate");
            oInventoryTraking.EndDate = oReader.GetDateTime("EndDate");
            oInventoryTraking.LotNo = oReader.GetString("LotNo");
            oInventoryTraking.OpeningQty = oReader.GetDouble("OpeningQty");
            oInventoryTraking.InQty = oReader.GetDouble("InQty");

            oInventoryTraking.RefObjectID = oReader.GetInt32("RefObjectID");
            oInventoryTraking.ContractorName = oReader.GetString("ContractorName");
            oInventoryTraking.Origin = oReader.GetString("Origin");
            oInventoryTraking.OpeningAmount = oReader.GetDouble("OpeningAmount");
            oInventoryTraking.InAmount = oReader.GetDouble("InAmount");
            oInventoryTraking.OutAmount = oReader.GetDouble("OutAmount");
            oInventoryTraking.ClosingAmount = oReader.GetDouble("ClosingAmount");

            oInventoryTraking.OutQty = oReader.GetDouble("OutQty");
            oInventoryTraking.ClosingQty = oReader.GetDouble("ClosingQty");
            oInventoryTraking.OpeningValue = oReader.GetDouble("OpeningValue");
            oInventoryTraking.InValue = oReader.GetDouble("InValue");
            oInventoryTraking.OutValue = oReader.GetDouble("OutValue");
            oInventoryTraking.ClosingValue = oReader.GetDouble("ClosingValue");
            oInventoryTraking.CurrencyID = oReader.GetInt32("CurrencyID");
            oInventoryTraking.Currency = oReader.GetString("Currency");

            //--Item Movement
            oInventoryTraking.MUnit = oReader.GetString("MUnit");
            oInventoryTraking.ProductCode = oReader.GetString("ProductCode");
            oInventoryTraking.ProductName = oReader.GetString("ProductName");
            oInventoryTraking.BUName = oReader.GetString("BUName");
            oInventoryTraking.LocName = oReader.GetString("LocName");
            oInventoryTraking.OPUName = oReader.GetString("OPUName");
            oInventoryTraking.Measurement = oReader.GetString("Measurement");
            oInventoryTraking.StyleNo = oReader.GetString("StyleNo");
            oInventoryTraking.ParentTypeSt = oReader.GetString("ParentTypeSt");
            oInventoryTraking.LotID = oReader.GetInt32("LotID");
            oInventoryTraking.ParentID = oReader.GetInt32("ParentID");
            oInventoryTraking.ProductID = oReader.GetInt32("ProductID");
            oInventoryTraking.ColorID = oReader.GetInt32("ColorID");
            oInventoryTraking.ColorName = oReader.GetString("ColorName");
            oInventoryTraking.Remarks = oReader.GetString("Remarks");
            oInventoryTraking.UnitPrice = oReader.GetDouble("UnitPrice");
            oInventoryTraking.FinishDia = oReader.GetString("FinishDia");
            oInventoryTraking.MCDia = oReader.GetString("MCDia");
            oInventoryTraking.GSM = oReader.GetString("GSM");
            oInventoryTraking.UserName = oReader.GetString("UserName");
            
            
            //Item Movement details
            oInventoryTraking.Balance = oReader.GetInt32("Balance");
            oInventoryTraking.ParentType =(EnumTriggerParentsType) oReader.GetInt32("ParentType");
            oInventoryTraking.RefType = (EnumGRNType)oReader.GetInt32("RefType");
            oInventoryTraking.RefNo = oReader.GetString("RefNo");
            oInventoryTraking.TransactionDate = oReader.GetDateTime("TransactionDate");
            oInventoryTraking.EntryDateTime = oReader.GetDateTime("EntryDateTime");

            if (oInventoryTraking.CurrencyID > 0)
            {
                oInventoryTraking.MUnit = oReader.GetString("Currency");
            }
            oInventoryTraking.MUnitID = oReader.GetInt32("MUnitID");
            if (oInventoryTraking.MUnitID > 0)
            {
                oInventoryTraking.MUnit = oReader.GetString("MUnit");
            }
            oInventoryTraking.URL = oReader.GetString("URL");
            return oInventoryTraking;
        }
        private InventoryTraking CreateObject(NullHandler oReader)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking=MapObject(oReader);
            return oInventoryTraking;
        }
        private List<InventoryTraking> CreateObjects(IDataReader oReader)
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InventoryTraking oItem = CreateObject(oHandler);
                oInventoryTrakings.Add(oItem);
            }
            return oInventoryTrakings;
        }
        #endregion

        #region WorkingUnit
        private InventoryTraking MapObject_WU(NullHandler oReader)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oInventoryTraking.StartDate = oReader.GetDateTime("StartDate");
            oInventoryTraking.EndDate = oReader.GetDateTime("EndDate");
            oInventoryTraking.WUName = oReader.GetString("WorkingUnitName");
            oInventoryTraking.OpeningQty = oReader.GetDouble("OpeningQty");
            oInventoryTraking.InQty = oReader.GetDouble("InQty");
            oInventoryTraking.OutQty = oReader.GetDouble("OutQty");
            oInventoryTraking.ClosingQty = oReader.GetDouble("ClosingQty");
            oInventoryTraking.CurrencyID = oReader.GetInt32("CurrencyID");

            oInventoryTraking.InGRN = oReader.GetDouble("InGRN");
            oInventoryTraking.InAdj = oReader.GetDouble("InAdj");
            oInventoryTraking.InRS = oReader.GetDouble("InRS");
            oInventoryTraking.InTr = oReader.GetDouble("InTr");
            oInventoryTraking.InRet = oReader.GetDouble("InRet");
          
            oInventoryTraking.OutAdj = oReader.GetDouble("OutAdj");
            oInventoryTraking.OutRS = oReader.GetDouble("OutRS");
            oInventoryTraking.OutTr = oReader.GetDouble("OutTr");
            oInventoryTraking.OutRet = oReader.GetDouble("OutRet");
            oInventoryTraking.OutCon = oReader.GetDouble("OutCon");

            oInventoryTraking.InTrSW = oReader.GetDouble("InTrSW");
            oInventoryTraking.OutTrSW = oReader.GetDouble("OutTrSW");
            oInventoryTraking.OutDC = oReader.GetDouble("OutDC");

            if (oInventoryTraking.CurrencyID > 0)
            {
                oInventoryTraking.MUnit = oReader.GetString("Currency");
            }
            oInventoryTraking.MUnitID = oReader.GetInt32("MUnitID");
            if (oInventoryTraking.MUnitID > 0)
            {
                oInventoryTraking.MUnit = oReader.GetString("MUnit");
            }
            return oInventoryTraking;
        }
        private InventoryTraking CreateObject_WU(NullHandler oReader)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking = MapObject_WU(oReader);
            return oInventoryTraking;
        }
        private List<InventoryTraking> CreateObjects_WU(IDataReader oReader)
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InventoryTraking oItem = CreateObject_WU(oHandler);
                oInventoryTrakings.Add(oItem);
            }
            return oInventoryTrakings;
        }
        #endregion
         #region Product
        private InventoryTraking MapObject_Product(NullHandler oReader)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oInventoryTraking.StartDate = oReader.GetDateTime("StartDate");
            oInventoryTraking.EndDate = oReader.GetDateTime("EndDate");
            oInventoryTraking.ProductID = oReader.GetInt32("ProductID");
       
            oInventoryTraking.CurrencyID = oReader.GetInt32("CurrencyID");
            oInventoryTraking.ProductCode = oReader.GetString("ProductCode");
            oInventoryTraking.ProductName = oReader.GetString("ProductName");
            oInventoryTraking.OpeningQty = oReader.GetDouble("OpeningQty");
            oInventoryTraking.InQty = oReader.GetDouble("InQty");
            oInventoryTraking.OutQty = oReader.GetDouble("OutQty");
            oInventoryTraking.ClosingQty = oReader.GetDouble("ClosingQty");
            oInventoryTraking.CurrencyID = oReader.GetInt32("CurrencyID");

            oInventoryTraking.OpeningValue = oReader.GetDouble("OpeningValue");
            oInventoryTraking.InValue = oReader.GetDouble("InValue");
            oInventoryTraking.OutValue = oReader.GetDouble("OutValue");
            oInventoryTraking.ClosingValue = oReader.GetDouble("ClosingValue");
            oInventoryTraking.Currency = oReader.GetString("Currency");
            oInventoryTraking.WUName = oReader.GetString("WUName");
            oInventoryTraking.PCategoryName = oReader.GetString("PCategoryName");

            oInventoryTraking.InGRN = oReader.GetDouble("InGRN");
            oInventoryTraking.InAdj = oReader.GetDouble("InAdj");
            oInventoryTraking.InRS = oReader.GetDouble("InRS");
            oInventoryTraking.InTr = oReader.GetDouble("InTr");
            oInventoryTraking.InTrSW = oReader.GetDouble("InTrSW");
            oInventoryTraking.InRet = oReader.GetDouble("InRet");

            oInventoryTraking.OutAdj = oReader.GetDouble("OutAdj");
            oInventoryTraking.OutRS = oReader.GetDouble("OutRS");
            oInventoryTraking.OutTr = oReader.GetDouble("OutTr");
            oInventoryTraking.OutRet = oReader.GetDouble("OutRet");
            oInventoryTraking.OutCon = oReader.GetDouble("OutCon");
            oInventoryTraking.OutDC = oReader.GetDouble("OutDC");

            oInventoryTraking.InWastage = oReader.GetDouble("InWastage");
            oInventoryTraking.InRecycle = oReader.GetDouble("InRecycle");
            oInventoryTraking.InWIP = oReader.GetDouble("InWIP");
            oInventoryTraking.OutWastage = oReader.GetDouble("OutWastage");
            oInventoryTraking.OutRecycle = oReader.GetDouble("OutRecycle");
            oInventoryTraking.OutWIP = oReader.GetDouble("OutWIP");

            oInventoryTraking.OutTrSW = oReader.GetDouble("OutTrSW");

            if (oInventoryTraking.CurrencyID > 0)
            {
                oInventoryTraking.MUnit = oReader.GetString("Currency");
            }
            oInventoryTraking.MUnitID = oReader.GetInt32("MUnitID");
            if (oInventoryTraking.MUnitID > 0)
            {
                oInventoryTraking.MUnit = oReader.GetString("MUnit");
            }

            return oInventoryTraking;
        }
        private InventoryTraking CreateObject_Product(NullHandler oReader)
        {
            InventoryTraking oInventoryTraking = new InventoryTraking();
            oInventoryTraking = MapObject_Product(oReader);
            return oInventoryTraking;
        }
        private List<InventoryTraking> CreateObjects_Product(IDataReader oReader)
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                InventoryTraking oItem = CreateObject_Product(oHandler);
                oInventoryTrakings.Add(oItem);
            }
            return oInventoryTrakings;
        }
        #endregion

        #region Interface implementation
        public InventoryTrakingService() { }

        public List<InventoryTraking> Gets_WU(DateTime dStartDate, DateTime dEndDate, int nBUID, int nTParentType, int nValueType, Int64 nUserId)
        {
            List<InventoryTraking> oInventoryTrakings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_WU(tc, dStartDate, dEndDate, nBUID, nTParentType, nValueType);
                oInventoryTrakings = CreateObjects_WU(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oInventoryTrakings;
        }
        public List<InventoryTraking> Gets_Product(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, Int64 nUserId)
        {
            List<InventoryTraking> oInventoryTrakings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_Product(tc, dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType, nValueType, nMUnitID, nCurrencyID);
                oInventoryTrakings = CreateObjects_Product(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oInventoryTrakings;
        }
        public List<InventoryTraking> Gets_ProductDyeing(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, Int64 nUserId)
        {
            List<InventoryTraking> oInventoryTrakings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_ProductDyeing(tc, dStartDate, dEndDate, nBUID, nWorkingUnitID, nTParentType, nValueType, nMUnitID, nCurrencyID);
                oInventoryTrakings = CreateObjects_Product(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oInventoryTrakings;
        }
        public List<InventoryTraking> Gets_Qty_Value(DateTime dStartDate, DateTime dEndDate, int nBUID, string nWorkingUnitIDs, int nCurrencyID, int nProductCategoryID, Int64 nUserId)
        {
            List<InventoryTraking> oInventoryTrakings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_Qty_Value(tc, dStartDate, dEndDate, nBUID, nWorkingUnitIDs, nCurrencyID, nProductCategoryID);
                oInventoryTrakings = CreateObjects_Product(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oInventoryTrakings;
        }

        public List<InventoryTraking> Gets_ITransactions(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, Int64 nUserID)   
        {
            List<InventoryTraking> oInventoryTrakings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_ITransactions(tc, dStartDate, dEndDate, sSQL, nReportLayOut);
                oInventoryTrakings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oInventoryTrakings;
        }
       
        public List<InventoryTraking> Gets_Lot(DateTime dStartDate, DateTime dEndDate, int nBUID, int nWorkingUnitID, int nProductID, int nTParentType, int nValueType, int nMUnitID, int nCurrencyID, Int64 nUserId)
        {
            List<InventoryTraking> oInventoryTrakings = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_Lot(tc, dStartDate, dEndDate, nBUID,  nWorkingUnitID, nProductID,nTParentType , nValueType,  nMUnitID, nCurrencyID);
                oInventoryTrakings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oInventoryTrakings;
        }


        public List<InventoryTraking> Gets_ForInventorySummary(DateTime dStartDate, DateTime dEndDate, string sSQL, int nReportLayOut, Int64 nUserID)
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_ForInventorySummary(tc, dStartDate, dEndDate, sSQL, nReportLayOut);
                oInventoryTrakings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oInventoryTrakings;
        }
        public List<InventoryTraking> Gets_ForItemMovement(DateTime dStartDate, DateTime dEndDate, string sSQL,  int nReportLayOut, Int64 nUserID)
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_ForItemMovement(tc, dStartDate, dEndDate,  sSQL, nReportLayOut);
                oInventoryTrakings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oInventoryTrakings;
        }
        //Gets_ForItemMovementDetails
        public List<InventoryTraking> Gets_ForItemMovementDetails(DateTime dStartDate, DateTime dEndDate, string sSQL, string sSQL_FindLot,  Int64 nUserID)
        {
            List<InventoryTraking> oInventoryTrakings = new List<InventoryTraking>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_ForItemMovementDetails(tc, dStartDate, dEndDate, sSQL, sSQL_FindLot);
                oInventoryTrakings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                InventoryTraking oInventoryTraking = new InventoryTraking();
                oInventoryTrakings = new List<InventoryTraking>();
                oInventoryTraking.ErrorMessage = e.Message;
                oInventoryTrakings.Add(oInventoryTraking);
                #endregion
            }
            return oInventoryTrakings;
        }

        public List<InventoryTraking> Gets_ProductWise(string sProductIDs, int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserId)
        {
            List<InventoryTraking> oInventoryTrakings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = InventoryTrakingDA.Gets_ProductWise(tc, sProductIDs, nBUID,dStartDate, dEndDate);
                oInventoryTrakings = CreateObjects_Product(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oInventoryTrakings;
        }
        #endregion
    }
}