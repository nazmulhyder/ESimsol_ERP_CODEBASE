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
    public class RouteSheetService : MarshalByRefObject, IRouteSheetService
    {
        #region Private functions and declaration
        private RouteSheet MapObject(NullHandler oReader)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            oRouteSheet.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheet.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheet.RouteSheetDate = oReader.GetDateTime("RouteSheetDate");
            oRouteSheet.MachineID = oReader.GetInt32("MachineID");
            oRouteSheet.ProductID_Raw = oReader.GetInt32("ProductID_Raw");
            oRouteSheet.LotID = oReader.GetInt32("LotID");
            oRouteSheet.Qty = oReader.GetDouble("Qty");
            oRouteSheet.QtyDye = oReader.GetDouble("QtyDye");
            oRouteSheet.QtyOmit = oReader.GetDouble("QtyOmit");
            oRouteSheet.Label = oReader.GetInt32("Label");
            oRouteSheet.RSState = (EnumRSState)oReader.GetInt16("RSState");
            oRouteSheet.LocationID = oReader.GetInt32("LocationID");
            oRouteSheet.PTUID = oReader.GetInt32("PTUID");
            oRouteSheet.DUPScheduleID = oReader.GetInt32("DUPScheduleID");
            oRouteSheet.Note = oReader.GetString("Note");
            oRouteSheet.DyeingType = oReader.GetString("DyeingType");
            oRouteSheet.TtlLiquire = oReader.GetDouble("TtlLiquire");
            oRouteSheet.TtlCotton = oReader.GetDouble("TtlCotton");
            oRouteSheet.HanksCone = oReader.GetInt16("HanksCone");
            oRouteSheet.NoOfHanksCone = oReader.GetInt32("NoOfHanksCone");
            oRouteSheet.CopiedFrom = oReader.GetInt32("CopiedFrom");
            oRouteSheet.PrepareBy = oReader.GetInt32("PrepareBy");
            oRouteSheet.ApproveBy = oReader.GetInt32("ApproveBy");
            oRouteSheet.ContractorName = oReader.GetString("ContractorName");
            oRouteSheet.MachineName = oReader.GetString("MachineName");
            oRouteSheet.ProductID = oReader.GetInt32("ProductID");
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
            //oRouteSheet.EventTime = oReader.GetDateTime("EventTime");
            oRouteSheet.OrderNo = oReader.GetString("OrderNo");
            oRouteSheet.ColorName = oReader.GetString("ColorName");
            oRouteSheet.OrderType = oReader.GetInt32("OrderType");
            oRouteSheet.NoCode = oReader.GetString("NoCode");
            oRouteSheet.RSShiftID = oReader.GetInt32("RSShiftID");
            oRouteSheet.RecipeByName = oReader.GetString("RecipeByName");
            oRouteSheet.IsReDyeing = (EnumReDyeingStatus)oReader.GetInt16("IsReDyeing");
            oRouteSheet.MName = oReader.GetString("MName");

            return oRouteSheet;
        }

        public static RouteSheet CreateObject(NullHandler oReader)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            RouteSheetService oService = new RouteSheetService();
            oRouteSheet = oService.MapObject(oReader);
            return oRouteSheet;
        }
        private List<RouteSheet> CreateObjects(IDataReader oReader)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheet oItem = CreateObject(oHandler);
                oRouteSheets.Add(oItem);
            }
            return oRouteSheets;
        }

        #endregion

        #region Interface implementation
        public RouteSheetService() { }
        
        public RouteSheet IUD(RouteSheet oRouteSheet, int nDBOperation, long nUserID)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            TransactionContext tc = null;
            try
            {
                oRouteSheetDOs = oRouteSheet.RouteSheetDOs;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RouteSheetDA.IUD(tc, oRouteSheet, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);

                }
                reader.Close();
                #region oRouteSheetDO Part
                if (oRouteSheetDOs != null)
                {
                    foreach (RouteSheetDO oItem in oRouteSheetDOs)
                    {
                        if (oItem.Qty_RS > 0)
                        {
                            IDataReader readerRSDO;
                            oItem.RouteSheetID = oRouteSheet.RouteSheetID;
                            if (oItem.RouteSheetDOID <= 0)
                            {
                                readerRSDO = RouteSheetDODA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerRSDO = RouteSheetDODA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oreaderRSDO = new NullHandler(readerRSDO);

                         
                            readerRSDO.Close();
                        }
                    }
                  
                }
                #endregion
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete) { oRouteSheet = new RouteSheet(); oRouteSheet.ErrorMessage = Global.DeleteMessage; }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }
        public RouteSheet UpdateMachine(RouteSheet oRouteSheet, int nDBOperation, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RouteSheetDA.UpdateMachine(tc, oRouteSheet, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }

        public RouteSheet Get(int nRouteSheetID, long nUserID)
        {
            RouteSheet oRouteSheet = new RouteSheet();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetDA.Get(tc, nRouteSheetID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheet.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheet;
        }
        public RouteSheet GetByPS(int nDUPScheduleID, long nUserID)
        {
            RouteSheet oRouteSheet = new RouteSheet();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetDA.GetByPS(tc, nDUPScheduleID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheet.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheet;
        }

        public List<RouteSheet> Gets(string sSQL, long nUserID)
        {
            List<RouteSheet> oRouteSheets = new List<RouteSheet>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetDA.Gets(tc, sSQL);
                oRouteSheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                RouteSheet oRouteSheet = new RouteSheet();
                oRouteSheet.ErrorMessage = e.Message;
                oRouteSheets.Add(oRouteSheet);
                #endregion
            }

            return oRouteSheets;
        }

        public RouteSheet CopyRouteSheet(RouteSheet oRouteSheet, long nUserID)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            oRouteSheetDOs = oRouteSheet.RouteSheetDOs;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = RouteSheetDA.IUD(tc, oRouteSheet, (int)EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                #region oRouteSheetDO Part
                if (oRouteSheetDOs != null)
                {
                    foreach (RouteSheetDO oItem in oRouteSheetDOs)
                    {
                        if (oItem.Qty_RS > 0)
                        {
                            IDataReader readerRSDO;
                            oItem.RouteSheetID = oRouteSheet.RouteSheetID;
                            if (oItem.RouteSheetDOID <= 0)
                            {
                                readerRSDO = RouteSheetDODA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                            }
                            else
                            {
                                readerRSDO = RouteSheetDODA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            }
                            NullHandler oreaderRSDO = new NullHandler(readerRSDO);


                            readerRSDO.Close();
                        }
                    }

                }
                #endregion

                reader = RouteSheetDetailDA.IUDTemplateCopyFromRS(tc, oRouteSheet.CopiedFrom, oRouteSheet.RouteSheetID, nUserID);
                reader.Close();
               
                tc.End();
         
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }
        public RouteSheet SaveRouteSheetDO(RouteSheet oRouteSheet, long nUserID)
        {
            List<RouteSheetDO> oRouteSheetDOs = new List<RouteSheetDO>();
            oRouteSheetDOs = oRouteSheet.RouteSheetDOs;
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Route Sheet DO Part
                if (oRouteSheetDOs != null)
                {
                    foreach (RouteSheetDO oItem in oRouteSheetDOs)
                    {
                        if (oItem.Qty_RS > 0)
                        {
                            IDataReader readerRSDO;
                            readerRSDO = RouteSheetDODA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                            readerRSDO.Close();
                            RouteSheetDODA.UpdateRouteSheet(tc, oItem.RouteSheetID);
                        }
                    }
                }
                

                #endregion
               
                tc.End();

            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }

        public RouteSheet RouteSheetEditSave(RouteSheet oRouteSheet, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                if (oRouteSheet.RouteSheetID > 0)
                {
                    reader = RouteSheetDA.RouteSheetEditSave(tc, oRouteSheet, (int)EnumDBOperation.Update, nUserID);      
                }                      
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oRouteSheet = new RouteSheet();
                    oRouteSheet.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oRouteSheet;
        }
        public RouteSheet YarnOut(RouteSheet oRouteSheet, int nEventEmpID, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = RouteSheetDA.YarnOut(tc, oRouteSheet, nEventEmpID, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
         
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }

        public RouteSheet RSQCDOneByForce(RouteSheet oRouteSheet, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = RouteSheetDA.RSQCDOneByForce(tc, oRouteSheet, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
         
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }
        public RouteSheet RSQCDOne(RouteSheetDO oRouteSheetDo, long nUserID)
        {
            RouteSheet oRouteSheet = new RouteSheet();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = RouteSheetDA.RSQCDOne(tc, oRouteSheetDo, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }

        public RouteSheet RSInRSInSubFinishing(RouteSheet oRouteSheet, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                reader = RouteSheetDA.RSInRSInSubFinishing(tc, oRouteSheet, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheet = new RouteSheet();
                    oRouteSheet = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRouteSheet.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oRouteSheet;
        }
        #endregion
    }
}