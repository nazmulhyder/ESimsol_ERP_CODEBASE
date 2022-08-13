using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
 
    [Serializable]
    public class DUPScheduleService : MarshalByRefObject, IDUPScheduleService
    {
        #region Private functions and declaration
        private DUPSchedule MapObject(NullHandler oReader)
        {
            DUPSchedule oDUPSchedule = new DUPSchedule();
            oDUPSchedule.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oDUPSchedule.BatchGroup = oReader.GetInt32("BatchGroup");
            oDUPSchedule.Status = (EnumProductionScheduleStatus)oReader.GetInt16("ScheduleStatus");
            oDUPSchedule.StatusInt = oReader.GetInt16("ScheduleStatus");
            oDUPSchedule.ScheduleNo = oReader.GetString("ScheduleNo");
            oDUPSchedule.MachineID = oReader.GetInt16("MachineID");
            oDUPSchedule.PSBatchNo = (EnumNumericOrder)oReader.GetInt16("PSBatchNo");
            oDUPSchedule.LocationID = oReader.GetInt32("LocationID");
            oDUPSchedule.BUID = oReader.GetInt32("BUID");
            oDUPSchedule.Qty = Math.Round(oReader.GetDouble("Qty"),2);
            oDUPSchedule.ScheduleType = oReader.GetBoolean("ScheduleType");
            oDUPSchedule.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oDUPSchedule.StartTime = oReader.GetDateTime("StartTime");
            oDUPSchedule.EndTime = oReader.GetDateTime("EndTime");
            oDUPSchedule.DBUserID = oReader.GetInt32("DBUserID");
            oDUPSchedule.MachineNo = oReader.GetString("MachineNo");
            oDUPSchedule.ProductName = oReader.GetString("ProductName");
            oDUPSchedule.ScheduleNo = oReader.GetString("ScheduleNo");
            oDUPSchedule.OrderNo = oReader.GetString("OrderNo");
            oDUPSchedule.MachineName = oReader.GetString("MachineName");
            oDUPSchedule.ContractorName = oReader.GetString("ContractorName");
            oDUPSchedule.BuyerRef = oReader.GetString("BuyerRef");
            oDUPSchedule.ColorName = oReader.GetString("ColorName");
            oDUPSchedule.Capacity = Math.Round(oReader.GetDouble("Capacity"), 2);
            oDUPSchedule.LocationName = oReader.GetString("LocationName");
            oDUPSchedule.OrderCount = oReader.GetInt32("OrderCount");
            oDUPSchedule.LotNo = oReader.GetString("LotNo");
            oDUPSchedule.Note = oReader.GetString("Note");
            oDUPSchedule.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDUPSchedule.LotID = oReader.GetInt32("LotID");
            oDUPSchedule.Capacity2 = oReader.GetString("Capacity2");
            return oDUPSchedule;
        }

        private DUPSchedule CreateObject(NullHandler oReader)
        {
            DUPSchedule oDUPSchedule = new DUPSchedule();
            oDUPSchedule = MapObject(oReader);
            return oDUPSchedule;
        }

        private List<DUPSchedule> CreateObjects(IDataReader oReader)
        {
            List<DUPSchedule> oDUPSchedules = new List<DUPSchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUPSchedule oItem = CreateObject(oHandler);
                oDUPSchedules.Add(oItem);
            }
            return oDUPSchedules;
        }
        #endregion

        #region Interface implementation
        public DUPScheduleService() { }
        public DUPSchedule Save(DUPSchedule oDUPSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
                List<DUPSLot> oDUPSLots = new List<DUPSLot>();
                oDUPScheduleDetails = oDUPSchedule.DUPScheduleDetails;
                oDUPSLots = oDUPSchedule.DUPSLots;
            
                IDataReader reader;
                if (oDUPSchedule.DUPScheduleID <= 0)
                {
                    reader = DUPScheduleDA.InsertUpdate(tc, oDUPSchedule, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DUPScheduleDA.InsertUpdate(tc, oDUPSchedule, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUPSchedule = new DUPSchedule();
                    oDUPSchedule = CreateObject(oReader);
                }
                reader.Close();

                #region Production Schedule Detail Part
                foreach (DUPScheduleDetail oItem in oDUPScheduleDetails)
                {
                    IDataReader readerdetail;
                    oItem.DUPScheduleID = oDUPSchedule.DUPScheduleID;
                    if (oItem.DUPScheduleDetailID <= 0)
                    {
                        readerdetail = DUPScheduleDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = DUPScheduleDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    //NullHandler oReaderDetail = new NullHandler(readerdetail);
                    //if (readerdetail.Read())
                    //{
                    //    sDUPScheduleDetailIDs = sDUPScheduleDetailIDs + oReaderDetail.GetString("DUPScheduleDetailID") + ",";
                    //}
                    readerdetail.Close();
                }
               
                #endregion
                #region  Lot Detail Part
                foreach (DUPSLot oItem1 in oDUPSLots)
                {
                    IDataReader readerdetailLot;
                    oItem1.DUPScheduleID = oDUPSchedule.DUPScheduleID;
                    if (oItem1.DUPSLotID <= 0)
                    {
                        readerdetailLot = DUPSLotDA.InsertUpdate(tc, oItem1, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetailLot = DUPSLotDA.InsertUpdate(tc, oItem1, EnumDBOperation.Update, nUserID);
                    }
                   
                    readerdetailLot.Close();
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUPSchedule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDUPSchedule;

        }
        public DUPSchedule SaveRS(DUPSchedule oDUPSchedule, Int64 nUserID)
        {
            List<DUPScheduleDetail> oDUPScheduleDetails = new List<DUPScheduleDetail>();
            RouteSheet oRouteSheet = new RouteSheet();
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            TransactionContext tc = null;
            String sRouteSheetDOIDs = "";
            try
            {
                oDUPScheduleDetails = oDUPSchedule.DUPScheduleDetails;
                tc = TransactionContext.Begin(true);

                if (oDUPScheduleDetails == null || oDUPScheduleDetails.Count <= 0)
                {
                    throw new ServiceException("Order info not found ");
                }
                #region RS Part
              
                IDataReader readerRS;
                oRouteSheet = new RouteSheet();
                oRouteSheet.RouteSheetID = oDUPSchedule.RouteSheetID;
                oRouteSheet.RouteSheetNo = oDUPSchedule.RouteSheetNo;
                oRouteSheet.DUPScheduleID = oDUPSchedule.DUPScheduleID;
                oRouteSheet.NoOfHanksCone = (int)oDUPScheduleDetails.Select(o => o.BagCount).Sum();
                oRouteSheet.LotID = 0;
                oRouteSheet.HanksCone = oDUPScheduleDetails[0].HankorCone;// (int)EumDyeingType.Cone; 
                oRouteSheet.RouteSheetDate = DateTime.Today;
                oRouteSheet.Qty = oDUPScheduleDetails.Select(o => o.Qty).Sum();
                oRouteSheet.TtlLiquire = oRouteSheet.Qty*6;
                oRouteSheet.PTUID = oDUPScheduleDetails[0].PTUID;
                oRouteSheet.ProductID_Raw = oDUPScheduleDetails[0].ProductID;
                oRouteSheet.TtlCotton = oRouteSheet.Qty*6;
                oRouteSheet.Note = "";
                oRouteSheet.OrderType = oDUPScheduleDetails[0].OrderType;
                oRouteSheet.LocationID = oDUPSchedule.LocationID;
                oRouteSheet.MachineID = oDUPSchedule.MachineID;
                //oRouteSheet.LotID = oItem.LocationID;
                //double nLotBalance = LotDA.GetLotBalance(tc, oRouteSheet.LotID, oRouteSheet.ProductionYarnID);
                //if (nLotBalance < oRouteSheet.ReqYarnQty)
                //{
                //    throw new Exception("Insufficient Lot Balance [" + nLotBalance.ToString("0.00") + "KG].");
                //}

                if (oRouteSheet.RouteSheetID <= 0) // Add
                {
                    readerRS = RouteSheetDA.IUD(tc, oRouteSheet, (int)EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    readerRS = RouteSheetDA.IUD(tc, oRouteSheet, (int)EnumDBOperation.Update, nUserID);
                }
                NullHandler oreaderRS = new NullHandler(readerRS);
                if (readerRS.Read())
                {
                    oRouteSheet.RouteSheetID = oreaderRS.GetInt32("RouteSheetID");
                }
                readerRS.Close();
                /// end Route Sheet
                ///insert/Update Route Sheet DO
                IDataReader readerDO;

                if (oDUPScheduleDetails != null)
                {
                    foreach (DUPScheduleDetail oItem in oDUPScheduleDetails)
                    {
                        oRouteSheetDO = new RouteSheetDO();
                        oRouteSheetDO.RouteSheetID = oRouteSheet.RouteSheetID;
                        oRouteSheetDO.DyeingOrderDetailID = oItem.DODID;
                        oRouteSheetDO.RouteSheetDOID = oItem.RouteSheetDOID;
                        oRouteSheetDO.Qty_RS = oItem.Qty;
                        if (oRouteSheetDO.RouteSheetDOID <= 0) // Add
                        {
                            readerDO = RouteSheetDODA.InsertUpdate(tc, oRouteSheetDO, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerDO = RouteSheetDODA.InsertUpdate(tc, oRouteSheetDO, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderTNC = new NullHandler(readerDO);
                        if (readerDO.Read())
                        {
                            sRouteSheetDOIDs = sRouteSheetDOIDs + oReaderTNC.GetString("RouteSheetDOID") + ",";
                        }
                        readerDO.Close();
                    }
                    if (sRouteSheetDOIDs.Length > 0)
                    {
                        sRouteSheetDOIDs = sRouteSheetDOIDs.Remove(sRouteSheetDOIDs.Length - 1, 1);
                    }
                    if (sRouteSheetDOIDs.Length > 0 && oRouteSheet.RouteSheetID>0)
                    {
                        RouteSheetDODA.DeleteByRS(tc, oRouteSheet.RouteSheetID, sRouteSheetDOIDs);
                    }
                }

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oDUPSchedule = new DUPSchedule();
                oDUPSchedule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUPSchedule;
        }
        public int GetsMax(string sSql, Int64 nUserID)
        {
            int maxValue=0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                maxValue=Convert.ToInt32(DUPScheduleDA.GetsMax(tc, sSql));
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return maxValue;
        }
        public List<DUPSchedule> Gets(string sSql, Int64 nUserID)
        {
            List<DUPSchedule> DUPScheduleList = null;
            TransactionContext tc = null;
            IDataReader reader=null;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = DUPScheduleDA.Gets(tc, sSql);
                DUPScheduleList = CreateObjects(reader);
                reader.Close();
                tc.End();



            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
            return DUPScheduleList;
        }
        public String Delete(DUPSchedule oDUPSchedule, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUPScheduleDA.Delete(tc, oDUPSchedule, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<DUPSchedule> Gets(Int64 nUserID)
        {
            List<DUPSchedule> oDUPSchedules = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDA.Gets(tc);
                oDUPSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get DUPSchedules", e);
                #endregion
            }

            return oDUPSchedules;
        }
        public List<DUPSchedule> Gets(DateTime dStartDate, DateTime dEndDate, string sLocationIDs,Int64 nUserID)
        {
            List<DUPSchedule> oDUPSchedules = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUPScheduleDA.Gets(tc, dStartDate,  dEndDate,  sLocationIDs);
                oDUPSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get DUPSchedules", e);
                #endregion
            }

            return oDUPSchedules;
        }
        public DUPSchedule Get(int Id, Int64 nUserID)
        {
            DUPSchedule oDUPSchedule = new DUPSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUPScheduleDA.Get(tc, Id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUPSchedule = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUPSchedule = new DUPSchedule();
                oDUPSchedule.ErrorMessage = e.Message;
                #endregion
            }



            return oDUPSchedule;
        }
        public DUPSchedule Update_Status(DUPSchedule oDUPSchedule, Int64 nUserId)
        {
            TransactionContext tc = null;
            EnumRSState eRSState = EnumRSState.None;
            try
            {
                tc = TransactionContext.Begin();

                eRSState = (EnumRSState)DUPScheduleDA.GetHisEven(tc, oDUPSchedule.DUPScheduleID, EnumRSState.UnloadedFromDyeMachine);
                if (eRSState == EnumRSState.UnloadedFromDyeMachine)
                {
                    throw new Exception("Already Batch UnloadedFromDyeMachine. You Can't Change it .");
                }

                IDataReader reader = DUPScheduleDA.Update_Status(tc, oDUPSchedule);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUPSchedule = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUPSchedule = new DUPSchedule();
                oDUPSchedule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUPSchedule;
        }
        #region Get Waiting Production Quantity


        //public Double GetWaitingProductionQuantity(string sSql, Int64 nUserID)
        //{
        //    double nWaitingTotalQuantity = 0;

        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin();
        //        nWaitingTotalQuantity = Convert.ToDouble(DUPScheduleDA.GetWaitingProductionQuantity(tc, sSql));
        //        tc.End();
        //    }
        //    catch(Exception e)
        //    {

        //        #region Handel Exception
        //        if (tc != null)
        //        {
        //            tc.HandleError();
        //            ExceptionLog.Write(e);
        //        }
        //        #endregion

        //    }

        //    return nWaitingTotalQuantity;
        //}

        #endregion
        #endregion
    }
}
