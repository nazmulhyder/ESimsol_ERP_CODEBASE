using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RouteSheetCombineService : MarshalByRefObject, IRouteSheetCombineService
    {
        #region Private functions and declaration
        private RouteSheetCombine MapObject(NullHandler oReader)
        {
            RouteSheetCombine oRouteSheetCombine = new RouteSheetCombine();
            oRouteSheetCombine.RouteSheetCombineID = oReader.GetInt32("RouteSheetCombineID");
            oRouteSheetCombine.RSNo_Combine = oReader.GetString("RSNo_Combine");
            oRouteSheetCombine.CombineRSDate = oReader.GetDateTime("CombineRSDate");
            oRouteSheetCombine.TotalQty = oReader.GetDouble("TotalQty");
            //oRouteSheetCombine.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetCombine.TotalLiquor = oReader.GetDouble("TotalLiquor");
            oRouteSheetCombine.TtlCotton = oReader.GetDouble("TtlCotton");
            oRouteSheetCombine.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetCombine.ContractorName = oReader.GetString("ContractorName");
            oRouteSheetCombine.OrderNo = oReader.GetString("OrderNo");
            oRouteSheetCombine.Note = oReader.GetString("Note");
            oRouteSheetCombine.OrderType = oReader.GetInt32("OrderType");
            oRouteSheetCombine.ApproveBy = oReader.GetInt32("ApproveBy");
            oRouteSheetCombine.ApproveByName = oReader.GetString("ApproveByName");
            return oRouteSheetCombine;

        }
        private RouteSheetCombine CreateObject(NullHandler oReader)
        {
            RouteSheetCombine oRouteSheetCombine = MapObject(oReader);
            return oRouteSheetCombine;
        }
        private List<RouteSheetCombine> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetCombine> oRouteSheetCombines = new List<RouteSheetCombine>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetCombine oItem = CreateObject(oHandler);
                oRouteSheetCombines.Add(oItem);
            }
            return oRouteSheetCombines;
        }

   
        #endregion

        #region Interface implementation
        public RouteSheetCombineService() { }

        public RouteSheetCombine Save(RouteSheetCombine oRouteSheetCombine, Int64 nUserID)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            RouteSheet oRouteSheet = new RouteSheet();
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            TransactionContext tc = null;
            try
            {
                oRouteSheetCombineDetails = oRouteSheetCombine.RouteSheetCombineDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteSheetCombine.RouteSheetCombineID <= 0)
                {
                    reader = RouteSheetCombineDA.InsertUpdate(tc, oRouteSheetCombine, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = RouteSheetCombineDA.InsertUpdate(tc, oRouteSheetCombine, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombine = new RouteSheetCombine();
                    oRouteSheetCombine = CreateObject(oReader);
                }
                reader.Close();
                #region Detail Part
                if (oRouteSheetCombineDetails != null)
                {
                    foreach (RouteSheetCombineDetail oItem in oRouteSheetCombineDetails)
                    {
                       
                        ///insert/Update Route Sheer
                        if (oItem.RSState <= EnumRSState.InFloor)
                        {
                            //IDataReader readerRS;
                            //oRouteSheet = new RouteSheet();
                            //oRouteSheet.RouteSheetID = oItem.RouteSheetID;
                            //oRouteSheet.DUPScheduleID = oItem.DUPScheduleID;
                            //oRouteSheet.TtlLiquire = oItem.TtlLiquire;
                            ////oRouteSheet.ParentID = PTUDA.GetOrder(tc, oItem.PTUID);
                            ////oRouteSheet.ParentType = EnumOrderType.Bulk;
                            //oRouteSheet.LotID = oItem.LotID;
                            //oRouteSheet.Qty = oItem.Qty;
                            //oRouteSheet.PTUID = oItem.PTUID;
                            //oRouteSheet.ProductID_Raw = oItem.ProductID_Raw;
                            //oRouteSheet.TtlCotton = oItem.TtlLiquire;
                            //oRouteSheet.Note = oItem.Note;
                            //oRouteSheet.RouteSheetNo = oItem.RouteSheetNo;
                            //oRouteSheet.OrderType = oItem.OrderType;
                            //oRouteSheet.LocationID = oItem.LocationID;
                            //oRouteSheet.MachineID = oItem.MachineID;
                            ////oRouteSheet.LotID = oItem.LocationID;
                            ////double nLotBalance = LotDA.GetLotBalance(tc, oRouteSheet.LotID, oRouteSheet.ProductionYarnID);
                            ////if (nLotBalance < oRouteSheet.ReqYarnQty)
                            ////{
                            ////    throw new Exception("Insufficient Lot Balance [" + nLotBalance.ToString("0.00") + "KG].");
                            ////}

                            ////if (oRouteSheet.RouteSheetID <= 0) // Add
                            ////{
                            ////    readerRS = RouteSheetDA.IUD(tc, oRouteSheet, (int)EnumDBOperation.Insert, nUserID);
                            ////}
                            ////else
                            ////{
                            ////    readerRS = RouteSheetDA.IUD(tc, oRouteSheet, (int)EnumDBOperation.Update, nUserID);
                            ////}
                            ////NullHandler oreaderRS = new NullHandler(readerRS);
                            ////if (readerRS.Read())
                            ////{
                            ////    oRouteSheet.RouteSheetID = oreaderRS.GetInt32("RouteSheetID");
                            ////}
                            //readerRS.Close();
                            ///// end Route Sheet
                            /////insert/Update Route Sheer DO
                            //IDataReader readerDO;
                            //oRouteSheetDO = new RouteSheetDO();
                            //oRouteSheetDO.RouteSheetID = oRouteSheet.RouteSheetID;
                            //oRouteSheetDO.DyeingOrderDetailID = oItem.DyeingOrderDetailID;
                            //oRouteSheetDO.RouteSheetDOID = oItem.RouteSheetDOID;
                            //oRouteSheetDO.Qty_RS = oItem.Qty;
                            //if (oItem.DyeingOrderDetailID > 0)
                            //{
                            //    if (oRouteSheetDO.RouteSheetDOID <= 0) // Add
                            //    {
                            //        readerDO = RouteSheetDODA.InsertUpdate(tc, oRouteSheetDO, EnumDBOperation.Insert, nUserID);
                            //    }
                            //    else
                            //    {
                            //        readerDO = RouteSheetDODA.InsertUpdate(tc, oRouteSheetDO, EnumDBOperation.Update, nUserID);
                            //    }
                            //    readerDO.Close();
                            //}

                            /// //


                            IDataReader readerRSCD;
                            //oItem.RouteSheetID = oRouteSheet.RouteSheetID;
                            oItem.RouteSheetCombineID = oRouteSheetCombine.RouteSheetCombineID;
                            oRouteSheetCombine.TotalQty = oRouteSheetCombine.TotalQty + oItem.Qty;
                            oRouteSheetCombine.TotalLiquor = oRouteSheetCombine.TotalLiquor + oItem.TtlLiquire;
                            oRouteSheetCombine.TtlCotton = oRouteSheetCombine.TtlCotton + oItem.TtlCotton;
                            if (oItem.RouteSheetCombineDetailID <= 0)
                            {
                                readerRSCD = RouteSheetCombineDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerRSCD = RouteSheetCombineDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oreaderRSCD = new NullHandler(readerRSCD);
                            readerRSCD.Close();
                        }
                        
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
                oRouteSheetCombine = new RouteSheetCombine();
                oRouteSheetCombine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRouteSheetCombine;
        }
        public RouteSheetCombine Approve(RouteSheetCombine oRouteSheetCombine, Int64 nUserID)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            RouteSheet oRouteSheet = new RouteSheet();
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            TransactionContext tc = null;
            try
            {
                oRouteSheetCombineDetails = oRouteSheetCombine.RouteSheetCombineDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteSheetCombine.RouteSheetCombineID <= 0)
                {
                    reader = RouteSheetCombineDA.InsertUpdate(tc, oRouteSheetCombine, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    reader = RouteSheetCombineDA.InsertUpdate(tc, oRouteSheetCombine, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombine = new RouteSheetCombine();
                    oRouteSheetCombine = CreateObject(oReader);
                }
                reader.Close();
                

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oRouteSheetCombine = new RouteSheetCombine();
                oRouteSheetCombine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRouteSheetCombine;
        }

        public RouteSheetCombine UndoApprove(RouteSheetCombine oRouteSheetCombine, Int64 nUserID)
        {
            List<RouteSheetCombineDetail> oRouteSheetCombineDetails = new List<RouteSheetCombineDetail>();
            RouteSheet oRouteSheet = new RouteSheet();
            RouteSheetDO oRouteSheetDO = new RouteSheetDO();
            TransactionContext tc = null;
            try
            {
                oRouteSheetCombineDetails = oRouteSheetCombine.RouteSheetCombineDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = RouteSheetCombineDA.InsertUpdate(tc, oRouteSheetCombine, EnumDBOperation.UnApproval, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombine = new RouteSheetCombine();
                    oRouteSheetCombine = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oRouteSheetCombine = new RouteSheetCombine();
                oRouteSheetCombine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRouteSheetCombine;
        }
        public RouteSheetCombine Get(int nID, Int64 nUserId)
        {
            RouteSheetCombine oRouteSheetCombine = new RouteSheetCombine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetCombineDA.Get(nID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombine = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get RouteSheetCombine", e);
                oRouteSheetCombine.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetCombine;
        }
        public RouteSheetCombine GetBy(int nRSID, Int64 nUserId)
        {
            RouteSheetCombine oRouteSheetCombine = new RouteSheetCombine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetCombineDA.GetBy(nRSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetCombine = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get RouteSheetCombine", e);
                oRouteSheetCombine.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetCombine;
        }

        public List<RouteSheetCombine> GetsBy(int nRouteSheetID, Int64 nUserID)
        {
            List<RouteSheetCombine> oRouteSheetCombine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCombineDA.GetsBy(tc, nRouteSheetID);
                oRouteSheetCombine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCombine", e);
                #endregion
            }
            return oRouteSheetCombine;
        }
        public List<RouteSheetCombine> Gets(int nPTUID, Int64 nUserID)
        {
            List<RouteSheetCombine> oRouteSheetCombine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCombineDA.Gets(tc, nPTUID);
                oRouteSheetCombine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCombine", e);
                #endregion
            }
            return oRouteSheetCombine;
        }
    
        public string Delete(RouteSheetCombine oRouteSheetCombine, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                RouteSheetCombineDA.Delete(tc, oRouteSheetCombine, EnumDBOperation.Delete, nUserId);
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
        public List<RouteSheetCombine> Gets(string sSQL, Int64 nUserID)
        {
            List<RouteSheetCombine> oRouteSheetCombine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetCombineDA.Gets(sSQL, tc);
                oRouteSheetCombine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteSheetCombine", e);
                #endregion
            }
            return oRouteSheetCombine;
        }
    
        
        #endregion
    }
}
