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
    public class DUSoftWindingService : MarshalByRefObject, IDUSoftWindingService
    {
        #region Private functions and declaration
        private DUSoftWinding MapObject(NullHandler oReader)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            oDUSoftWinding.DUSoftWindingID = oReader.GetInt32("DUSoftWindingID");
            oDUSoftWinding.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oDUSoftWinding.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUSoftWinding.DURequisitionID = oReader.GetInt32("DURequisitionID");
            oDUSoftWinding.ProductID = oReader.GetInt32("ProductID");
            oDUSoftWinding.NumOfCone = oReader.GetInt32("NumOfCone");
            oDUSoftWinding.LotID = oReader.GetInt32("LotID");
            oDUSoftWinding.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDUSoftWinding.Qty = oReader.GetDouble("Qty");
            oDUSoftWinding.MUnitID = oReader.GetInt32("MUnitID");
            oDUSoftWinding.MachineID = oReader.GetInt32("MachineID");
            oDUSoftWinding.RSShiftID = oReader.GetInt32("RSShiftID");
            oDUSoftWinding.BagNo = oReader.GetDouble("BagNo");
            oDUSoftWinding.Qty_Order = oReader.GetDouble("Qty_Order");
            oDUSoftWinding.Qty_RSOut = oReader.GetDouble("Qty_RSOut");
            oDUSoftWinding.Qty_In = oReader.GetDouble("Qty_In");
            oDUSoftWinding.Balance = oReader.GetDouble("Balance");
            oDUSoftWinding.Note = oReader.GetString("Note");
            oDUSoftWinding.Status = (EnumWindingStatus)oReader.GetInt32("Status");
            oDUSoftWinding.StartDate = oReader.GetDateTime("StartDate");
            oDUSoftWinding.EndDate = oReader.GetDateTime("EndDate");
            oDUSoftWinding.OrderDate = oReader.GetDateTime("OrderDate");
            oDUSoftWinding.MachineName = oReader.GetString("MachineName");
            oDUSoftWinding.Operator = oReader.GetString("Operator");
            oDUSoftWinding.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oDUSoftWinding.ProductCode = oReader.GetString("ProductCode");
            oDUSoftWinding.ProductName = oReader.GetString("ProductName");
            oDUSoftWinding.MUnit = oReader.GetString("MUnit");
            oDUSoftWinding.ContractorName = oReader.GetString("ContractorName");
            oDUSoftWinding.OrderType = oReader.GetString("OrderType");
            oDUSoftWinding.LotNo = oReader.GetString("LotNo");

            oDUSoftWinding.Qty_SW = oReader.GetDouble("Qty_SW");
            oDUSoftWinding.Qty_Req = oReader.GetDouble("Qty_Req");
            oDUSoftWinding.QtySRS = oReader.GetDouble("QtySRS");
            oDUSoftWinding.QtySRM = oReader.GetDouble("QtySRM");
            oDUSoftWinding.ContractorID = oReader.GetInt32("ContractorID");
            oDUSoftWinding.DestinationLotID = oReader.GetInt32("DestinationLotID");
            oDUSoftWinding.DestinationLotNo = oReader.GetString("DestinationLotNo");
            oDUSoftWinding.QtyGainLoss = oReader.GetDouble("QtyGainLoss");
            oDUSoftWinding.GainLossType = (EnumInOutType)oReader.GetInt32("GainLossType");
            oDUSoftWinding.GainLossTypeInt = oReader.GetInt32("GainLossType");
            
            return oDUSoftWinding;
        }

        private DUSoftWinding CreateObject(NullHandler oReader)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            oDUSoftWinding = MapObject(oReader);
            return oDUSoftWinding;
        }

        private List<DUSoftWinding> CreateObjects(IDataReader oReader)
        {
            List<DUSoftWinding> oDUSoftWinding = new List<DUSoftWinding>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUSoftWinding oItem = CreateObject(oHandler);
                oDUSoftWinding.Add(oItem);
            }
            return oDUSoftWinding;
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
            DUSoftWindingService oService = new DUSoftWindingService();
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
        public DUSoftWindingService() { }
        public DUSoftWinding Save(DUSoftWinding oDUSoftWinding, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDUSoftWinding.DUSoftWindingID <= 0)
                {
                    reader = DUSoftWindingDA.InsertUpdate(tc, oDUSoftWinding, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DUSoftWindingDA.InsertUpdate(tc, oDUSoftWinding, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUSoftWinding = new DUSoftWinding();
                    oDUSoftWinding = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DUSoftWinding. Because of " + e.Message.Split('~')[0], e);
                #endregion
            }
            return oDUSoftWinding;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUSoftWinding oDUSoftWinding = new DUSoftWinding();
                oDUSoftWinding.DUSoftWindingID = id;
                DUSoftWindingDA.Delete(tc, oDUSoftWinding, EnumDBOperation.Delete, nUserId);
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
        public string UpdateRSLot(int nDUSoftWindingID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                DUSoftWindingDA.UpdateRSLot(tc, nDUSoftWindingID, nUserId);
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
            return "";
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
                    string sRSID = sID.Split('~')[0];
                    string sCone = sID.Split('~')[1]; //Number of cone to out

                    #region Get RouteSheet
                    tc = TransactionContext.Begin(true);
                    reader = RouteSheetDA.Get(tc, Convert.ToInt32(sRSID));
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheet = new RouteSheet();
                        DUSoftWindingService oService = new DUSoftWindingService();
                        oRouteSheet = oService.CreateRouteSheetObject(oReader);

                        oRouteSheet.NoOfHanksCone = Convert.ToInt32(sCone);
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
                        DUSoftWindingService oService = new DUSoftWindingService();
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
        public List<RouteSheet> YarnOut_Multi(string sRSIDs, long nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            NullHandler oReader;

            RouteSheet oRouteSheet = new RouteSheet();
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            try
            {
                foreach (string sID in sRSIDs.Split(','))
                {
                    //string sRSID = sID.Split('~')[0];
                    //string sCone = sID.Split('~')[1]; //Number of cone to out

                    #region Get RouteSheet
                    tc = TransactionContext.Begin(true);
                    reader = RouteSheetDA.Get(tc, Convert.ToInt32(sID));
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheet = new RouteSheet();
                        DUSoftWindingService oService = new DUSoftWindingService();
                        oRouteSheet = oService.CreateRouteSheetObject(oReader);
                    }
                    reader.Close();
                    tc.End();
                    #endregion

                    #region YarnOut
                    tc = TransactionContext.Begin(true);
                    reader = RouteSheetDA.YarnOut_Multi(tc, oRouteSheet, 0, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRouteSheet = new RouteSheet();
                        DUSoftWindingService oService = new DUSoftWindingService();
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

        public DUSoftWinding Get(int id, Int64 nUserId)
        {
            DUSoftWinding oDUSoftWinding = new DUSoftWinding();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUSoftWindingDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUSoftWinding = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUSoftWinding", e);
                #endregion
            }
            return oDUSoftWinding;
        }
        public List<DUSoftWinding> Gets(Int64 nUserID)
        {
            List<DUSoftWinding> oDUSoftWindings = new List<DUSoftWinding>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUSoftWindingDA.Gets(tc);
                oDUSoftWindings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUSoftWinding", e);
                #endregion
            }
            return oDUSoftWindings;
        }
        public List<DUSoftWinding> Gets(string sSQL, Int64 nUserID)
        {
            List<DUSoftWinding> oDUSoftWindings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUSoftWindingDA.Gets(tc, sSQL);
                oDUSoftWindings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUSoftWinding", e);
                #endregion
            }
            return oDUSoftWindings;
        }

        public List<DUSoftWinding> Gets_Report(string sSQL, Int64 nUserID)
        {
            List<DUSoftWinding> oDUSoftWindings = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUSoftWindingDA.Gets_Report(tc, sSQL);
                oDUSoftWindings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUSoftWinding Report", e);
                #endregion
            }
            return oDUSoftWindings;
        }
        #endregion
    }
}