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
    public class DUHardWindingService : MarshalByRefObject, IDUHardWindingService
    {
        #region Private functions and declaration
        private DUHardWinding MapObject(NullHandler oReader)
        {
            DUHardWinding oDUHardWinding = new DUHardWinding();
            oDUHardWinding.DUHardWindingID = oReader.GetInt32("DUHardWindingID");
            oDUHardWinding.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oDUHardWinding.QCDate = oReader.GetDateTime("QCDate");
            oDUHardWinding.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUHardWinding.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDUHardWinding.DURequisitionID = oReader.GetInt32("DURequisitionID");
            oDUHardWinding.ProductID = oReader.GetInt32("ProductID");
            oDUHardWinding.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oDUHardWinding.LotID = oReader.GetInt32("LotID");
            oDUHardWinding.MachineID = oReader.GetInt32("MachineID");
            oDUHardWinding.RSShiftID = oReader.GetInt32("RSShiftID");
            oDUHardWinding.NumOfCone = oReader.GetInt32("NumOfCone");
            oDUHardWinding.Dia = oReader.GetDouble("Dia");
            oDUHardWinding.Qty = oReader.GetDouble("Qty");
            oDUHardWinding.MUnitID = oReader.GetInt32("MUnitID");
            oDUHardWinding.BagNo = oReader.GetDouble("BagNo");
            oDUHardWinding.Qty_Order = Math.Round(oReader.GetDouble("Qty_Order"),2);
            oDUHardWinding.Qty_RSOut = oReader.GetDouble("Qty_RSOut");
           
            oDUHardWinding.Balance = oReader.GetDouble("Balance");
            oDUHardWinding.Note = oReader.GetString("Note");
            oDUHardWinding.Status = (EnumWindingStatus)oReader.GetInt32("Status");
            oDUHardWinding.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            oDUHardWinding.WarpWeftTypeInt = oReader.GetInt32("WarpWeftType");
            oDUHardWinding.RSCone = oReader.GetInt32("RSCone");
            oDUHardWinding.StartDate = oReader.GetDateTime("StartDate");
            oDUHardWinding.EndDate = oReader.GetDateTime("EndDate");
            oDUHardWinding.OrderDate = oReader.GetDateTime("OrderDate");
            oDUHardWinding.MachineDes = oReader.GetString("MachineDes");
            oDUHardWinding.ColorName = oReader.GetString("ColorName");
            oDUHardWinding.Operator = oReader.GetString("Operator");
            oDUHardWinding.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oDUHardWinding.ProductCode = oReader.GetString("ProductCode");
            oDUHardWinding.ProductName = oReader.GetString("ProductName");
            oDUHardWinding.MUnit = oReader.GetString("MUnit");
            oDUHardWinding.ContractorName = oReader.GetString("ContractorName");
            oDUHardWinding.BuyerName = oReader.GetString("BuyerName");
            oDUHardWinding.OrderType = oReader.GetString("OrderType");
            oDUHardWinding.LotNo = oReader.GetString("LotNo");
            oDUHardWinding.MachineName = oReader.GetString("MachineName");
            oDUHardWinding.RSShitName = oReader.GetString("RSShitName");
            oDUHardWinding.EntryByName = oReader.GetString("EntryByName");
            oDUHardWinding.RSState = (EnumRSState)oReader.GetInt32("RSState");
            oDUHardWinding.IsInHouse = oReader.GetBoolean("IsInHouse");
            oDUHardWinding.IsQCDone = oReader.GetBoolean("IsQCDone");
            oDUHardWinding.IsRewinded = oReader.GetBoolean("IsRewinded");
            oDUHardWinding.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oDUHardWinding.EntryByName = oReader.GetString("EntryByName");
            oDUHardWinding.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            return oDUHardWinding;
        }

        private DUHardWinding CreateObject(NullHandler oReader)
        {
            DUHardWinding oDUHardWinding = new DUHardWinding();
            oDUHardWinding = MapObject(oReader);
            return oDUHardWinding;
        }

        private List<DUHardWinding> CreateObjects(IDataReader oReader)
        {
            List<DUHardWinding> oDUHardWinding = new List<DUHardWinding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUHardWinding oItem = CreateObject(oHandler);
                oDUHardWinding.Add(oItem);
            }
            return oDUHardWinding;
        }

        #endregion

        #region  Private functions and declaration For 'RouteSheet'
        private RouteSheet MapRouteSheetObject(NullHandler oReader)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            oRouteSheet.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheet.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheet.RouteSheetDate = oReader.GetDateTime("RouteSheetDate");
            oRouteSheet.MachineID = oReader.GetInt32("MachineID");
            oRouteSheet.ProductID_Raw = oReader.GetInt32("ProductID_Raw");
            oRouteSheet.LotID = oReader.GetInt32("LotID");
            oRouteSheet.Qty = oReader.GetDouble("Qty");
            oRouteSheet.RSState = (EnumRSState)oReader.GetInt16("RSState");
            oRouteSheet.LocationID = oReader.GetInt32("LocationID");
            oRouteSheet.PTUID = oReader.GetInt32("PTUID");
            oRouteSheet.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oRouteSheet.Note = oReader.GetString("Note");
            oRouteSheet.TtlLiquire = oReader.GetDouble("TtlLiquire");
            oRouteSheet.TtlCotton = oReader.GetDouble("TtlCotton");
            oRouteSheet.HanksCone = oReader.GetInt16("HanksCone");
            oRouteSheet.NoOfHanksCone = oReader.GetInt32("NoOfHanksCone");
            oRouteSheet.CopiedFrom = oReader.GetInt32("CopiedFrom");
            oRouteSheet.PrepareBy = oReader.GetInt32("PrepareBy");
            oRouteSheet.ApproveBy = oReader.GetInt32("ApproveBy");
            oRouteSheet.MachineName = oReader.GetString("MachineName");
            oRouteSheet.ProductCode = oReader.GetString("ProductCode");
            oRouteSheet.ProductName = oReader.GetString("ProductName");
            oRouteSheet.ProductName_Raw = oReader.GetString("ProductName_Raw");
            oRouteSheet.LotNo = oReader.GetString("LotNo");
            oRouteSheet.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRouteSheet.OperationUnitName = oReader.GetString("OperationUnitName");
            oRouteSheet.LocationName = oReader.GetString("LocationName");
            //oRouteSheet.CopiedFromRouteSheetNo = oReader.GetString("CopiedFromRouteSheetNo");
            oRouteSheet.PrepareByName = oReader.GetString("PrepareByName");
            oRouteSheet.ApproveByName = oReader.GetString("ApproveByName");
            oRouteSheet.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oRouteSheet.OrderNo = oReader.GetString("OrderNo");
            oRouteSheet.ColorName = oReader.GetString("ColorName");
            oRouteSheet.OrderType = oReader.GetInt32("OrderType");
            return oRouteSheet;
        }

        private RouteSheet CreateRouteSheetObject(NullHandler oReader)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            DUHardWindingService oService = new DUHardWindingService();
            oRouteSheet = oService.MapRouteSheetObject(oReader);
            return oRouteSheet;
        }
        private List<RouteSheet> CreateRouteSheetObjects(IDataReader oReader)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheet oItem = CreateRouteSheetObject(oHandler);
                oRouteSheets.Add(oItem);
            }
            return oRouteSheets;
        }

        #endregion

        #region Interface implementation
        public DUHardWindingService() { }
        public DUHardWinding Save(DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUHardWinding.DUHardWindingID <= 0)
                {
                    reader = DUHardWindingDA.InsertUpdate(tc, oDUHardWinding, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DUHardWindingDA.InsertUpdate(tc, oDUHardWinding, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DUHardWinding. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oDUHardWinding;
        }
        public DUHardWinding Rewinding(DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
             
                reader = DUHardWindingDA.InsertUpdate(tc, oDUHardWinding, EnumDBOperation.Insert, nUserID);
               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DUHardWinding. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oDUHardWinding;
        }
        public DUHardWinding RSQCDOne(DUHardWinding oDUHardWinding, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = DUHardWindingDA.QCDOne(tc, oDUHardWinding, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDUHardWinding.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oDUHardWinding;
        }
        public DUHardWinding SendToDelivery(DUHardWinding oDUHardWinding, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = DUHardWindingDA.SendToDelivery(tc, oDUHardWinding, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDUHardWinding.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oDUHardWinding;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUHardWinding oDUHardWinding = new DUHardWinding();
                oDUHardWinding.DUHardWindingID = id;
                DUHardWindingDA.Delete(tc, oDUHardWinding, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public DUHardWinding Receive(DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DUHardWindingDA.Receive(tc, oDUHardWinding, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DUHardWinding. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oDUHardWinding;
        }
        public DUHardWinding DODAssign(DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DUHardWindingDA.DODAssign(tc, oDUHardWinding, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDUHardWinding.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion

            }
            return oDUHardWinding;
        }
        public DUHardWinding UpdateReceivedate(DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DUHardWindingDA.UpdateReceivedate(tc, oDUHardWinding, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDUHardWinding.ErrorMessage = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion

            }
            return oDUHardWinding;
        }
        public DUHardWinding SendToHWStore(DUHardWinding oDUHardWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = DUHardWindingDA.SendToHWStore(tc, oDUHardWinding, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = new DUHardWinding();
                    oDUHardWinding = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                //DUHardWindingDA.SendToHWStore(tc, oDUHardWinding, nUserId);
                //tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oDUHardWinding.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oDUHardWinding;
        }
        public List<RouteSheet> YarnOut(string sRSIDs, long nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader;

            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                foreach(string sID in sRSIDs.Split(',')) 
                {
                    #region Get RouteSheet
                    tc = TransactionContext.Begin(true);
                    reader = RouteSheetDA.Get(tc, Convert.ToInt32(sID));
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheet = new RouteSheet();
                        DUHardWindingService oService = new DUHardWindingService();
                        oRouteSheet = oService.CreateRouteSheetObject(oReader);
                    }
                    reader.Close();
                    tc.End();
                    #endregion

                    #region YarnOut
                    tc = TransactionContext.Begin(true);
                    reader = RouteSheetDA.YarnOut(tc, oRouteSheet, 0, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheet = new RouteSheet();
                        DUHardWindingService oService = new DUHardWindingService();
                        oRouteSheet = oService.CreateRouteSheetObject(oReader);
                        oRouteSheets.Add(oRouteSheet);
                    }
                    reader.Close();
                    tc.End();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oRouteSheets = new List<RouteSheet>();
                oRouteSheet = new RouteSheet();
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oRouteSheets.Add(oRouteSheet);
                #endregion
            }
            return oRouteSheets;
        }

        public DUHardWinding Get(int id, Int64 nUserId)
        {
            DUHardWinding oDUHardWinding = new DUHardWinding();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUHardWindingDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUHardWinding = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUHardWinding", e);
                #endregion
            }
            return oDUHardWinding;
        }
        public List<DUHardWinding> Gets(Int64 nUserID)
        {
            List<DUHardWinding> oDUHardWindings = new List<DUHardWinding>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUHardWindingDA.Gets(tc);
                oDUHardWindings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUHardWinding", e);
                #endregion
            }
            return oDUHardWindings;
        }
        public List<DUHardWinding> Gets(string sSQL, Int64 nUserID)
        {
            List<DUHardWinding> oDUHardWindings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUHardWindingDA.Gets(tc, sSQL);
                oDUHardWindings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUHardWinding", e);
                #endregion
            }
            return oDUHardWindings;
        }
        #endregion
    }
}