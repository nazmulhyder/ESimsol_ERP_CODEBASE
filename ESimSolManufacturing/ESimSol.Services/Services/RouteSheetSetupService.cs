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
    public class RouteSheetSetupService : MarshalByRefObject, IRouteSheetSetupService
    {
        #region Private functions and declaration
        private RouteSheetSetup MapObject(NullHandler oReader)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup.RouteSheetSetupID = oReader.GetInt32("RouteSheetSetupID");
            oRouteSheetSetup.MUnitID_Two = oReader.GetInt32("MUnitID_Two");
            oRouteSheetSetup.MUnitID = oReader.GetInt32("MUnitID");
            oRouteSheetSetup.RSName = oReader.GetString("RSName");
            oRouteSheetSetup.RSName_Print = oReader.GetString("RSName_Print");
            oRouteSheetSetup.RSShortName = oReader.GetString("RSShortName");
            oRouteSheetSetup.Activity = oReader.GetBoolean("Activity");
            oRouteSheetSetup.IsLotMandatory = oReader.GetBoolean("IsLotMandatory");
            oRouteSheetSetup.IsShowBuyer = oReader.GetBoolean("IsShowBuyer");
            oRouteSheetSetup.IsGraceApplicable = oReader.GetBoolean("IsGraceApplicable");
            oRouteSheetSetup.IsApplyHW = oReader.GetBoolean("IsApplyHW");
            oRouteSheetSetup.SmallUnit_Cal = oReader.GetDouble("SmallUnit_Cal");
            oRouteSheetSetup.SMUnitValue = oReader.GetDouble("SMUnitValue");
            oRouteSheetSetup.Activity = oReader.GetBoolean("Activity");
            oRouteSheetSetup.Note = oReader.GetString("Note");
            oRouteSheetSetup.GracePercentage = oReader.GetDouble("GracePercentage");
            oRouteSheetSetup.LossPercentage = oReader.GetDouble("LossPercentage");
            oRouteSheetSetup.GainPercentage = oReader.GetDouble("GainPercentage");
            oRouteSheetSetup.PrintNo = (EnumExcellColumn)oReader.GetInt32("PrintNo");
            oRouteSheetSetup.RestartBy = (EnumRestartPeriod)oReader.GetInt32("RestartBy");
            oRouteSheetSetup.DyesChemicalViewType = (EnumDyesChemicalViewType)oReader.GetInt32("DyesChemicalViewType");
            oRouteSheetSetup.MUnit = oReader.GetString("MUnit");
            oRouteSheetSetup.MUnitTwo = oReader.GetString("MUnitTwo");
            oRouteSheetSetup.BatchCode = oReader.GetString("BatchCode");
            oRouteSheetSetup.MachinePerDoc = oReader.GetInt32("MachinePerDoc");
            oRouteSheetSetup.NumberOfAddition = oReader.GetInt32("NumberOfAddition");
            oRouteSheetSetup.FontSize = oReader.GetDouble("FontSize");
            oRouteSheetSetup.BatchTime = oReader.GetDateTime("BatchTime");
            oRouteSheetSetup.WorkingUnitIDWIP = oReader.GetInt32("WorkingUnitIDWIP");
            oRouteSheetSetup.DCEntryValType = (EnumDCEntryType)oReader.GetInt32("DCEntryType");
            oRouteSheetSetup.DCOutValType = (EnumDCEntryType)oReader.GetInt32("DCOutType");
            oRouteSheetSetup.RSStateForCost = (EnumRSState)oReader.GetInt32("RSStateForCost");
            oRouteSheetSetup.IsRateOnUSD = oReader.GetBoolean("IsRateOnUSD");
            
            return oRouteSheetSetup;
        }

        private RouteSheetSetup CreateObject(NullHandler oReader)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            oRouteSheetSetup = MapObject(oReader);
            return oRouteSheetSetup;
        }

        private List<RouteSheetSetup> CreateObjects(IDataReader oReader)
        {
            List<RouteSheetSetup> oRouteSheetSetups = new List<RouteSheetSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteSheetSetup oItem = CreateObject(oHandler);
                oRouteSheetSetups.Add(oItem);
            }
            return oRouteSheetSetups;
        }

        #endregion

        #region Interface implementation
        public RouteSheetSetupService() { }

        public RouteSheetSetup Save(RouteSheetSetup oRouteSheetSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);

                #region RouteSheetSetup
                IDataReader reader;
                if (oRouteSheetSetup.RouteSheetSetupID <= 0)
                {
                    reader = RouteSheetSetupDA.InsertUpdate(tc, oRouteSheetSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = RouteSheetSetupDA.InsertUpdate(tc, oRouteSheetSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetSetup = new RouteSheetSetup();
                    oRouteSheetSetup = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRouteSheetSetup = new RouteSheetSetup();
                oRouteSheetSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRouteSheetSetup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            
            string sMessage = "Delete sucessfully";
            try
            {
                tc = TransactionContext.Begin(true);
                RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
                oRouteSheetSetup.RouteSheetSetupID = id;
                RouteSheetSetupDA.Delete(tc, oRouteSheetSetup, EnumDBOperation.Delete, nUserId);
               
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = "Delete operation could not complete";
            }

            return sMessage;
        }

        public RouteSheetSetup Get(int id, Int64 nUserId)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ", e);
                #endregion
            }

            return oRouteSheetSetup;
        }
        public RouteSheetSetup GetBy( Int64 nUserId)
        {
            RouteSheetSetup oRouteSheetSetup = new RouteSheetSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteSheetSetupDA.GetBy(tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteSheetSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RouteSheetSetup", e);
                #endregion
            }

            return oRouteSheetSetup;
        }

        public List<RouteSheetSetup> Gets(Int64 nUserId)
        {
            List<RouteSheetSetup> oRouteSheetSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteSheetSetupDA.Gets(tc);
                oRouteSheetSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oRouteSheetSetups;
        }


        #endregion
    }
}