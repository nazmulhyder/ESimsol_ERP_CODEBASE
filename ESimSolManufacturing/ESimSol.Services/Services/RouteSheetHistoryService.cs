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
    public class RouteSheetHistoryService : MarshalByRefObject, IRouteSheetHistoryService
    {
        #region Private functions and declaration
        private RouteSheetHistory MapObject(NullHandler oReader)
        {
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            oRouteSheetHistory.RouteSheetHistoryID = oReader.GetInt32("RouteSheetHistoryID");
            oRouteSheetHistory.RouteSheetID = oReader.GetInt32("RouteSheetID");
            oRouteSheetHistory.EventTime = oReader.GetDateTime("EventTime");
            oRouteSheetHistory.EventEmpID = oReader.GetInt32("EventEmpID");
            oRouteSheetHistory.CurrentStatus = (EnumRSState)oReader.GetInt16("CurrentStatus");
            oRouteSheetHistory.PreviousState = (EnumRSState)oReader.GetInt16("PreviousState");
            oRouteSheetHistory.Note = oReader.GetString("Note");
            oRouteSheetHistory.RouteSheetNo = oReader.GetString("RouteSheetNo");
            oRouteSheetHistory.EventEmpName = oReader.GetString("EventEmpName");
            oRouteSheetHistory.UserName = oReader.GetString("UserName");

            oRouteSheetHistory.ShadePercentage = oReader.GetDouble("ShadePercentage");
            oRouteSheetHistory.MachineID_Hydro = oReader.GetInt32("MachineID_Hydro");
            oRouteSheetHistory.MachineID_Dryer = oReader.GetInt32("MachineID_Dryer");
            oRouteSheetHistory.Value_Dyes = oReader.GetDouble("Value_Dyes");
            oRouteSheetHistory.Value_Chemcial = oReader.GetDouble("Value_Chemcial");
            oRouteSheetHistory.Value_Yarn = oReader.GetDouble("Value_Yarn");
            oRouteSheetHistory.MachineSpeed = oReader.GetDouble("MachineSpeed");
            oRouteSheetHistory.RBSpeed = oReader.GetDouble("RBSpeed");
            oRouteSheetHistory.MachineName_Hydro = oReader.GetString("MachineName_Hydro");
            oRouteSheetHistory.MachineName_Dryer = oReader.GetString("MachineName_Dryer");

            return oRouteSheetHistory;
        }

        public static RouteSheetHistory CreateObject(NullHandler oReader)
        {
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            RouteSheetHistoryService oService = new RouteSheetHistoryService();
            oRouteSheetHistory = oService.MapObject(oReader);
            return oRouteSheetHistory;
        }
        private List<RouteSheetHistory> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetHistory oItem = CreateObject(oHandler);
                oRouteSheetHistorys.Add(oItem);
            }
            return oRouteSheetHistorys;
        }

        #endregion

        #region Interface implementation
        public RouteSheetHistoryService() { }

        public RouteSheetHistory Get(int nRouteSheetHistoryID, long nUserID)
        {
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetHistoryDA.Get(tc, nRouteSheetHistoryID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheetHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetHistory;
        }
        public RouteSheetHistory GetBy(int nRSID, int nRSStatus, long nUserID)
        {
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetHistoryDA.GetBy(tc, nRSID, nRSStatus);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheetHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetHistory;
        }
        public RouteSheetHistory GetRSDyeingProgress(RouteSheetHistory oRSH, long nUserID)
        {
            RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RouteSheetHistoryDA.GetRSDyeingProgress(tc,oRSH);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetHistory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();

                oRouteSheetHistory.ErrorMessage = e.Message;
                #endregion
            }

            return oRouteSheetHistory;
        }

        public List<RouteSheetHistory> Gets(string sSQL, long nUserID)
        {
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetHistoryDA.Gets(tc, sSQL);
                oRouteSheetHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
                oRouteSheetHistory.ErrorMessage = e.Message;
                oRouteSheetHistorys.Add(oRouteSheetHistory);
                #endregion
            }

            return oRouteSheetHistorys;
        }
        public List<RouteSheetHistory> Gets(int nRSID, long nUserID)
        {
            List<RouteSheetHistory> oRouteSheetHistorys = new List<RouteSheetHistory>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetHistoryDA.Gets(tc, nRSID);
                oRouteSheetHistorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                RouteSheetHistory oRouteSheetHistory = new RouteSheetHistory();
                oRouteSheetHistory.ErrorMessage = e.Message;
                oRouteSheetHistorys.Add(oRouteSheetHistory);
                #endregion
            }

            return oRouteSheetHistorys;
        }

        public RouteSheetHistory ChangeRSStatus(RouteSheetHistory oRSH, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                NullHandler oReader;

                reader = RouteSheetHistoryDA.IUD(tc, oRSH, (int)EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSH = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRSH = new RouteSheetHistory();
                oRSH.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oRSH;
        }
        public RouteSheetHistory UpdateEventTime(RouteSheetHistory oRSH, long nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                NullHandler oReader;

                reader = RouteSheetHistoryDA.UpdateEventTime(tc, oRSH);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSH = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRSH = new RouteSheetHistory();
                oRSH.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oRSH;
        }
        public RouteSheetHistory ChangeRSStatus_Process(RouteSheetHistory oRSH, long nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                NullHandler oReader;

                reader = RouteSheetHistoryDA.IUD_Process(tc, oRSH, (int)EnumDBOperation.Insert, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSH = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oRSH = new RouteSheetHistory();
                oRSH.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oRSH;
        }
      
        #endregion
    }
}