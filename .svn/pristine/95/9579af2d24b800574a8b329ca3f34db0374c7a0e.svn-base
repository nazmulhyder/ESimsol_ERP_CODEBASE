using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class RSFreshDyedYarnService : MarshalByRefObject, IRSFreshDyedYarnService
    {
        #region Private functions and declaration
        private RSFreshDyedYarn MapObject(NullHandler oReader)
        {
            RSFreshDyedYarn oRSFreshDyedYarn = new RSFreshDyedYarn();
            oRSFreshDyedYarn.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRSFreshDyedYarn.ProductID = oReader.GetInt32("ProductID");
            oRSFreshDyedYarn.LocationID = oReader.GetInt32("LocationID");
            oRSFreshDyedYarn.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRSFreshDyedYarn.RSShiftID = oReader.GetInt32("RSShiftID");
            oRSFreshDyedYarn.RSShiftName = oReader.GetString("RSShiftName");
            oRSFreshDyedYarn.ProductName = oReader.GetString("ProductName");
            oRSFreshDyedYarn.Qty_RS = oReader.GetDouble("Qty_RS");
            oRSFreshDyedYarn.FreshDyedYarnQty = oReader.GetDouble("FreshDyedYarnQty");
            oRSFreshDyedYarn.WastageQty = oReader.GetDouble("WastageQty");
            oRSFreshDyedYarn.RecycleQty = oReader.GetDouble("RecycleQty");
            oRSFreshDyedYarn.ManagedQty = oReader.GetDouble("ManagedQty");
            oRSFreshDyedYarn.ContractorName = oReader.GetString("ContractorName");
            oRSFreshDyedYarn.OrderNo = oReader.GetString("OrderNo");
          
            oRSFreshDyedYarn.ColorName = oReader.GetString("ColorName");
            oRSFreshDyedYarn.BagCount = oReader.GetInt16("BagCount");
            oRSFreshDyedYarn.OrderType = oReader.GetInt16("OrderType");
            oRSFreshDyedYarn.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oRSFreshDyedYarn.RawLotNo = oReader.GetString("RawLotNo");
            oRSFreshDyedYarn.MachineName = oReader.GetString("MachineName");
            oRSFreshDyedYarn.Note = oReader.GetString("Note");
            oRSFreshDyedYarn.RSState = (EnumRSState)oReader.GetInt16("RSState");
            oRSFreshDyedYarn.RSSubNote = oReader.GetString("RSSubNote");
            oRSFreshDyedYarn.DyeingType = oReader.GetString("DyeingType");
            oRSFreshDyedYarn.NoOfHanksCone = oReader.GetInt32("NoOfHanksCone");
            oRSFreshDyedYarn.RequestByName = oReader.GetString("RequestByName");
            oRSFreshDyedYarn.LoadByName = oReader.GetString("LoadByName");
            oRSFreshDyedYarn.LoadTime = oReader.GetDateTime("LoadTime");
            oRSFreshDyedYarn.UnloadByName = oReader.GetString("UnloadByName");
            oRSFreshDyedYarn.UnloadTime = oReader.GetDateTime("UnloadTime");
            oRSFreshDyedYarn.QCDate = oReader.GetDateTime("QCDate");
            oRSFreshDyedYarn.RSDate = oReader.GetDateTime("RSDate");
            oRSFreshDyedYarn.RSSubStates = (EnumRSSubStates)oReader.GetInt16("RSSubStates");
            oRSFreshDyedYarn.DCAddCount = oReader.GetInt32("DCAddCount");
            oRSFreshDyedYarn.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRSFreshDyedYarn.IsReDyeing = oReader.GetBoolean("IsReDyeing");
            oRSFreshDyedYarn.IsInHouse = oReader.GetBoolean("IsInHouse");
            oRSFreshDyedYarn.WUName = oReader.GetString("WUName");
            oRSFreshDyedYarn.QtyDCAdd = oReader.GetDouble("QtyDCAdd");
            oRSFreshDyedYarn.IsFullQC = oReader.GetBoolean("IsFullQC");
            oRSFreshDyedYarn.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oRSFreshDyedYarn.PackingQty = oReader.GetDouble("PackingQty");
            oRSFreshDyedYarn.Gain = oReader.GetDouble("Gain");
            oRSFreshDyedYarn.Loss = oReader.GetDouble("Loss");
         
            
           
            return oRSFreshDyedYarn;
        }
        private RSFreshDyedYarn CreateObject(NullHandler oReader)
        {
            RSFreshDyedYarn oRSFreshDyedYarn = new RSFreshDyedYarn();
            oRSFreshDyedYarn = MapObject(oReader);
            return oRSFreshDyedYarn;
        }
        private List<RSFreshDyedYarn> CreateObjects(IDataReader oReader)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = new List<RSFreshDyedYarn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSFreshDyedYarn oItem = CreateObject(oHandler);
                oRSFreshDyedYarn.Add(oItem);
            }
            return oRSFreshDyedYarn;
        }
        #region Product
        private RSFreshDyedYarn MapObject_Product(NullHandler oReader)
        {
            RSFreshDyedYarn oRSFreshDyedYarn = new RSFreshDyedYarn();
            oRSFreshDyedYarn.ProductID = oReader.GetInt32("ProductID");
            oRSFreshDyedYarn.ProductName = oReader.GetString("ProductName");
            oRSFreshDyedYarn.Qty_RS = oReader.GetDouble("Qty_RS");
            oRSFreshDyedYarn.FreshDyedYarnQty = oReader.GetDouble("FreshDyedYarnQty");
            oRSFreshDyedYarn.PackingQty = oReader.GetDouble("PackingQty");
            oRSFreshDyedYarn.WastageQty = oReader.GetDouble("WastageQty");
            oRSFreshDyedYarn.RecycleQty = oReader.GetDouble("RecycleQty");
            oRSFreshDyedYarn.ManagedQty = oReader.GetDouble("ManagedQty");
            oRSFreshDyedYarn.BagCount = oReader.GetInt16("BagCount");
            oRSFreshDyedYarn.WIPQtyTwo = oReader.GetDouble("WIPQty");
            oRSFreshDyedYarn.Gain = oReader.GetDouble("Gain");
            oRSFreshDyedYarn.Loss = oReader.GetDouble("Loss");
         
            return oRSFreshDyedYarn;
        }
        private RSFreshDyedYarn CreateObject_Product(NullHandler oReader)
        {
            RSFreshDyedYarn oRSFreshDyedYarn = new RSFreshDyedYarn();
            oRSFreshDyedYarn = MapObject_Product(oReader);
            return oRSFreshDyedYarn;
        }
        private List<RSFreshDyedYarn> CreateObjects_Product(IDataReader oReader)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = new List<RSFreshDyedYarn>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSFreshDyedYarn oItem = CreateObject_Product(oHandler);
                oRSFreshDyedYarn.Add(oItem);
            }
            return oRSFreshDyedYarn;
        }
        #endregion
        #endregion

        #region Interface implementation
        public RSFreshDyedYarnService() { }

        public List<RSFreshDyedYarn> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType, Int64 nUserID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSFreshDyedYarnDA.Gets(tc,  dStartDate,  dEndDate,  nOrderType, nReportType);
                if (nReportType==1)
                { oRSFreshDyedYarn = CreateObjects(reader); }
                if (nReportType == 2)
                { oRSFreshDyedYarn = CreateObjects_Product(reader); }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oRSFreshDyedYarn;
        }
        public List<RSFreshDyedYarn> Gets(string sSQL,  int cboQCdate,DateTime dStartDate, DateTime dEndDate,int nReportType, Int64 nUserID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSFreshDyedYarnDA.Gets(tc, sSQL, cboQCdate, dStartDate, dEndDate,nReportType);
                if (nReportType == 1)
                { 
                    oRSFreshDyedYarn = CreateObjects(reader); 
                }
                if (nReportType == 2)
                { 
                    oRSFreshDyedYarn = CreateObjects_Product(reader); 
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
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oRSFreshDyedYarn;
        }
        public List<RSFreshDyedYarn> Gets(string sSQL, int nReportType, Int64 nUserID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSFreshDyedYarnDA.Gets(tc, sSQL, nReportType);
                if (nReportType == 1)
                {
                    oRSFreshDyedYarn = CreateObjects(reader);
                }
                if (nReportType == 2)
                {
                    oRSFreshDyedYarn = CreateObjects_Product(reader);
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
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oRSFreshDyedYarn;
        }
        public List<RSFreshDyedYarn> Gets_Product(string sSQL, Int64 nUserID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSFreshDyedYarnDA.Gets_Product(tc, sSQL);
                oRSFreshDyedYarn = CreateObjects_Product(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oRSFreshDyedYarn;
        }
        public List<RSFreshDyedYarn> Gets(string sSQL, Int64 nUserID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSFreshDyedYarnDA.Gets(tc, sSQL);
                oRSFreshDyedYarn = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get sp_RPT_RouteSheetQC", e);
                #endregion
            }

            return oRSFreshDyedYarn;
        }
     
        public List<RSFreshDyedYarn> GetsLoadUnload(string sSQL, Int64 nUserID)
        {
            List<RSFreshDyedYarn> oRSFreshDyedYarn = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSFreshDyedYarnDA.GetsLoadUnload(tc, sSQL);
               
                oRSFreshDyedYarn = CreateObjects(reader);
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oRSFreshDyedYarn;
        }
        #endregion
    }
}