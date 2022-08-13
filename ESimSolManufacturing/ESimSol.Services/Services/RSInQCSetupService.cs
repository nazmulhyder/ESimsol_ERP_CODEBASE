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
    public class RSInQCSetupService : MarshalByRefObject, IRSInQCSetupService
    {
        #region Private functions and declaration
        private RSInQCSetup MapObject(NullHandler oReader)
        {
            RSInQCSetup oRSInQCSetup = new RSInQCSetup();
            oRSInQCSetup.RSInQCSetupID = oReader.GetInt32("RSInQCSetupID");
            oRSInQCSetup.Name = oReader.GetString("Name");
            oRSInQCSetup.YarnType = (EnumYarnType)oReader.GetInt16("YarnType");
            oRSInQCSetup.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oRSInQCSetup.LocationID = oReader.GetInt32("LocationID");
            oRSInQCSetup.Activity = oReader.GetBoolean("Activity");
            oRSInQCSetup.WUName = oReader.GetString("WUName");
            oRSInQCSetup.LocationName = oReader.GetString("LocationName");
            return oRSInQCSetup;
        }

        private RSInQCSetup CreateObject(NullHandler oReader)
        {
            RSInQCSetup oRSInQCSetup = MapObject(oReader);
            return oRSInQCSetup;
        }

        private List<RSInQCSetup> CreateObjects(IDataReader oReader)
        {
            List<RSInQCSetup> oRSInQCSetup = new List<RSInQCSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RSInQCSetup oItem = CreateObject(oHandler);
                oRSInQCSetup.Add(oItem);
            }
            return oRSInQCSetup;
        }

        #endregion

        #region Interface implementation
        public RSInQCSetupService() { }

        public RSInQCSetup IUD(RSInQCSetup oRSInQCSetup, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RSInQCSetupDA.IUD(tc, oRSInQCSetup, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSInQCSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oRSInQCSetup = new RSInQCSetup();
                    oRSInQCSetup.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRSInQCSetup.ErrorMessage = e.Message.Split('~')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRSInQCSetup;
        }
        public RSInQCSetup Get(int nRSInQCSetupID, Int64 nUserId)
        {
            RSInQCSetup oRSInQCSetup = new RSInQCSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RSInQCSetupDA.Get(nRSInQCSetupID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRSInQCSetup = CreateObject(oReader);
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
                oRSInQCSetup.ErrorMessage = e.Message;
                #endregion
            }

            return oRSInQCSetup;
        }
        public List<RSInQCSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<RSInQCSetup> oRSInQCSetups = new List<RSInQCSetup>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSInQCSetupDA.Gets(sSQL, tc);
                oRSInQCSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RSInQCSetup oRSInQCSetup = new RSInQCSetup();
                oRSInQCSetup.ErrorMessage = e.Message;
                oRSInQCSetups = new List<RSInQCSetup>();
                oRSInQCSetups.Add(oRSInQCSetup);
                #endregion
            }
            return oRSInQCSetups;
        }
        public List<RSInQCSetup> GetsBy(int nYarnType, Int64 nUserID)
        {
            List<RSInQCSetup> oRSInQCSetups = new List<RSInQCSetup>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RSInQCSetupDA.GetsBy(nYarnType, tc);
                oRSInQCSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RSInQCSetup oRSInQCSetup = new RSInQCSetup();
                oRSInQCSetup.ErrorMessage = e.Message;
                oRSInQCSetups = new List<RSInQCSetup>();
                oRSInQCSetups.Add(oRSInQCSetup);
                #endregion
            }
            return oRSInQCSetups;
        }


        #endregion
    }
}
