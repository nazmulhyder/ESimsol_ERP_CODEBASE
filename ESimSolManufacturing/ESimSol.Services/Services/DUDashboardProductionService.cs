using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using System.Linq;
namespace ESimSol.Services.Services
{
   public class DUDashboardProductionService :MarshalByRefObject ,IDUDashboardProductionService
    {
        #region Private functions and declaration
        private static DUDashboardProduction MapObject(NullHandler oReader)
        {
            DUDashboardProduction oDUDashboardProduction = new DUDashboardProduction();

            oDUDashboardProduction.Count =oReader.GetInt16("Count");
            oDUDashboardProduction.Qty_Out = Math.Round(oReader.GetDouble("Qty_Out"), 2);
            oDUDashboardProduction.Qty_Machine = Math.Round(oReader.GetDouble("Qty_Machine"),2);
            oDUDashboardProduction.Qty_Hydro = Math.Round(oReader.GetDouble("Qty_Hydro"),2);
            oDUDashboardProduction.Qty_Dryer = Math.Round(oReader.GetDouble("Qty_Dryer"),2);
            oDUDashboardProduction.Qty_WQC =Math.Round( oReader.GetDouble("Qty_WQC"),2);
            oDUDashboardProduction.Qty_QCD = Math.Round(oReader.GetDouble("Qty_QCD"),2);
            oDUDashboardProduction.Qty_UnManage =Math.Round( oReader.GetDouble("Qty_UnManage"),2);
            oDUDashboardProduction.Qty_WForStore =Math.Round(oReader.GetDouble("Qty_WForStore"),2);

            oDUDashboardProduction.Qty_Cancel = Math.Round(oReader.GetDouble("Qty_Cancel"), 2);
            oDUDashboardProduction.Qty_Fresh = Math.Round(oReader.GetDouble("Qty_Fresh"), 2);
            oDUDashboardProduction.Qty_Gain = Math.Round(oReader.GetDouble("Qty_Gain"), 2);
            oDUDashboardProduction.Qty_Loss = Math.Round(oReader.GetDouble("Qty_Loss"), 2);
            oDUDashboardProduction.Qty_Recycle = Math.Round(oReader.GetDouble("Qty_Recycle"), 2);
            oDUDashboardProduction.Qty_Wastage = Math.Round(oReader.GetDouble("Qty_Wastage"), 2);
            oDUDashboardProduction.Qty_Received = Math.Round(oReader.GetDouble("Qty_Received"), 2);
            oDUDashboardProduction.Qty_DC = Math.Round(oReader.GetDouble("Qty_DC"), 2);
            oDUDashboardProduction.Qty_Manage = Math.Round(oReader.GetDouble("Qty_Manage"), 2);
            
            return oDUDashboardProduction;
        }
        public static DUDashboardProduction CreateObject(NullHandler oReader)
        {
            DUDashboardProduction oDUDashboardProduction = MapObject(oReader);
            return oDUDashboardProduction;
        }
        private List<DUDashboardProduction> CreateObjects(IDataReader oReader)
        {
            List<DUDashboardProduction> oDUDashboardProductions = new List<DUDashboardProduction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDashboardProduction oItem = CreateObject(oHandler);
                oDUDashboardProductions.Add(oItem);
            }
            return oDUDashboardProductions;
        }
        #endregion
        #region Private functions and declaration
        private static DUDashboardProduction MapObject_Two(NullHandler oReader)
        {
            DUDashboardProduction oDUDashboardProduction = new DUDashboardProduction();

            oDUDashboardProduction.Qty_Out =Math.Round(oReader.GetDouble("Qty"),2);
            oDUDashboardProduction.Count = oReader.GetInt32("Count");
            oDUDashboardProduction.Qty_Fresh =Math.Round(oReader.GetDouble("Qty_Fresh"),2);
            oDUDashboardProduction.StockInHand = Math.Round(oReader.GetDouble("StockInHand"),2);
            oDUDashboardProduction.Qty_DC = Math.Round(oReader.GetDouble("Qty_DC"), 2);
            oDUDashboardProduction.Name = oReader.GetString("Name");
            oDUDashboardProduction.Qty_UnManage = Math.Round(oReader.GetDouble("Qty_UnManage"),2);
            oDUDashboardProduction.ID = oReader.GetInt32("ID");
            oDUDashboardProduction.RouteSheetDate = oReader.GetDateTime("RouteSheetDate");
            oDUDashboardProduction.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oDUDashboardProduction.OrderNo = oReader.GetString("OrderNo");

            return oDUDashboardProduction;
        }
        public static DUDashboardProduction CreateObject_Two(NullHandler oReader)
        {
            DUDashboardProduction oDUDashboardProduction = MapObject_Two(oReader);
            return oDUDashboardProduction;
        }
        private List<DUDashboardProduction> CreateObjects_Two(IDataReader oReader)
        {
            List<DUDashboardProduction> oDUDashboardProductions = new List<DUDashboardProduction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDashboardProduction oItem = CreateObject_Two(oHandler);
                oDUDashboardProductions.Add(oItem);
            }
            return oDUDashboardProductions;
        }
        #endregion
       #region Interface implementation
        public DUDashboardProductionService() { }

    
        public List<DUDashboardProduction> Gets(DUDashboardProduction oDUDashboardProduction, Int64 nUserID)
        {
            List<DUDashboardProduction> oDUDashboardProductions = new List<DUDashboardProduction>();
            DUDashboardProduction oSCLC = new DUDashboardProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDashboardProductionDA.Gets(tc,oDUDashboardProduction);
                oDUDashboardProductions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSCLC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUDashboardProductions.Add(oSCLC);
                #endregion
            }

            return oDUDashboardProductions;
        }
        public List<DUDashboardProduction> Gets_Daily(DUDashboardProduction oDUDashboardProduction, Int64 nUserID)
        {
            List<DUDashboardProduction> oDUDashboardProductions = new List<DUDashboardProduction>();
            DUDashboardProduction oSCLC = new DUDashboardProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDashboardProductionDA.Gets_Daily(tc, oDUDashboardProduction);
                oDUDashboardProductions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSCLC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUDashboardProductions.Add(oSCLC);
                #endregion
            }

            return oDUDashboardProductions;
        }
        public DUDashboardProduction Get(DUDashboardProduction oDUDashboardProduction, Int64 nUserId)
        {
            DUDashboardProduction oDUDP = new DUDashboardProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDashboardProductionDA.Get(tc, oDUDashboardProduction);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDP = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDP = new DUDashboardProduction();
                oDUDP.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oDUDP;
        }
        public List<DUDashboardProduction> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDashboardProduction> oDUDashboardProductions = new List<DUDashboardProduction>();
            DUDashboardProduction oSCLC = new DUDashboardProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDashboardProductionDA.Gets(tc, sSQL);
                oDUDashboardProductions = CreateObjects_Two(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSCLC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUDashboardProductions.Add(oSCLC);
                #endregion
            }

            return oDUDashboardProductions;
        }
        #endregion
    }
}
