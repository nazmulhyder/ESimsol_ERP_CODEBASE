using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ShiftOTSlabService : MarshalByRefObject, IShiftOTSlabService
    {
        #region Private functions and declaration
        private ShiftOTSlab MapObject(NullHandler oReader)
        {
            ShiftOTSlab oShiftOTSlab = new ShiftOTSlab();

            oShiftOTSlab.ShiftOTSlabID = oReader.GetInt32("ShiftOTSlabID");
            oShiftOTSlab.ShiftID = oReader.GetInt32("ShiftID");
            oShiftOTSlab.MinOTInMin = oReader.GetInt32("MinOTInMin");
            oShiftOTSlab.MaxOTInMin = oReader.GetInt32("MaxOTInMin");
            oShiftOTSlab.AchieveOTInMin = oReader.GetInt32("AchieveOTInMin");
            oShiftOTSlab.IsActive = oReader.GetBoolean("IsActive");
            oShiftOTSlab.CompMinOTInMin = oReader.GetInt32("CompMinOTInMin");
            oShiftOTSlab.CompMaxOTInMin = oReader.GetInt32("CompMaxOTInMin");
            oShiftOTSlab.CompAchieveOTInMin = oReader.GetInt32("CompAchieveOTInMin");
            oShiftOTSlab.IsCompActive = oReader.GetBoolean("IsCompActive");
      
            return oShiftOTSlab;

        }

        private ShiftOTSlab CreateObject(NullHandler oReader)
        {
            ShiftOTSlab oShiftOTSlab = MapObject(oReader);
            return oShiftOTSlab;
        }

        private List<ShiftOTSlab> CreateObjects(IDataReader oReader)
        {
            List<ShiftOTSlab> oShiftOTSlabs = new List<ShiftOTSlab>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShiftOTSlab oItem = CreateObject(oHandler);
                oShiftOTSlabs.Add(oItem);
            }
            return oShiftOTSlabs;
        }

        #endregion

        #region Interface implementation
        public ShiftOTSlabService() { }
        public ShiftOTSlab IUD(ShiftOTSlab oShiftOTSlab, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ShiftOTSlabDA.IUD(tc, oShiftOTSlab, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftOTSlab = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oShiftOTSlab.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oShiftOTSlab.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oShiftOTSlab;
        }

        public ShiftOTSlab Get(int nShiftOTSlabID, Int64 nUserId)
        {
            ShiftOTSlab oShiftOTSlab = new ShiftOTSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftOTSlabDA.Get(nShiftOTSlabID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftOTSlab = CreateObject(oReader);
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

                oShiftOTSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftOTSlab;
        }

        public ShiftOTSlab Get(string sSQL, Int64 nUserId)
        {
            ShiftOTSlab oShiftOTSlab = new ShiftOTSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftOTSlabDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftOTSlab = CreateObject(oReader);
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

                oShiftOTSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftOTSlab;
        }

        public List<ShiftOTSlab> Gets(Int64 nUserID)
        {
            List<ShiftOTSlab> oShiftOTSlab = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftOTSlabDA.Gets(tc);
                oShiftOTSlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShiftOTSlab", e);
                #endregion
            }
            return oShiftOTSlab;
        }

        public List<ShiftOTSlab> Gets(string sSQL, Int64 nUserID)
        {
            List<ShiftOTSlab> oShiftOTSlab = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftOTSlabDA.Gets(sSQL, tc);
                oShiftOTSlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShiftOTSlab", e);
                #endregion
            }
            return oShiftOTSlab;
        }

        #endregion

        #region Activity
        public ShiftOTSlab Activite(int nShiftOTSlabID, Int64 nUserId)
        {
            ShiftOTSlab oShiftOTSlab = new ShiftOTSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftOTSlabDA.Activity(nShiftOTSlabID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftOTSlab = CreateObject(oReader);
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
                oShiftOTSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftOTSlab;
        }
        public ShiftOTSlab ActiviteComp(int nShiftOTSlabID, Int64 nUserId)
        {
            ShiftOTSlab oShiftOTSlab = new ShiftOTSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftOTSlabDA.ActivityComp(nShiftOTSlabID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftOTSlab = CreateObject(oReader);
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
                oShiftOTSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftOTSlab;
        }


        #endregion

    }
}
