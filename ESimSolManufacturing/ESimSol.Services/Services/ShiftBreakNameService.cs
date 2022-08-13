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
    public class ShiftBreakNameService : MarshalByRefObject, IShiftBreakNameService
    {
        #region Private functions and declaration
        private ShiftBreakName MapObject(NullHandler oReader)
        {
            ShiftBreakName oShiftBreakName = new ShiftBreakName();

            oShiftBreakName.ShiftBNID = oReader.GetInt32("ShiftBNID");
            oShiftBreakName.Name = oReader.GetString("Name");
            oShiftBreakName.IsActive = oReader.GetBoolean("IsActive");

            return oShiftBreakName;

        }

        private ShiftBreakName CreateObject(NullHandler oReader)
        {
            ShiftBreakName oShiftBreakName = MapObject(oReader);
            return oShiftBreakName;
        }

        private List<ShiftBreakName> CreateObjects(IDataReader oReader)
        {
            List<ShiftBreakName> oShiftBreakNames = new List<ShiftBreakName>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShiftBreakName oItem = CreateObject(oHandler);
                oShiftBreakNames.Add(oItem);
            }
            return oShiftBreakNames;
        }

        #endregion

        #region Interface implementation
        public ShiftBreakNameService() { }
        public ShiftBreakName IUD(ShiftBreakName oShiftBreakName, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ShiftBreakNameDA.IUD(tc, oShiftBreakName, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakName = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if(nDBOperation==3)
                {
                    oShiftBreakName.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oShiftBreakName.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oShiftBreakName;
        }

        public ShiftBreakName Get(int nShiftBNID, Int64 nUserId)
        {
            ShiftBreakName oShiftBreakName = new ShiftBreakName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBreakNameDA.Get(nShiftBNID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakName = CreateObject(oReader);
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

                oShiftBreakName.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftBreakName;
        }

        public ShiftBreakName Get(string sSQL, Int64 nUserId)
        {
            ShiftBreakName oShiftBreakName = new ShiftBreakName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBreakNameDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakName = CreateObject(oReader);
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

                oShiftBreakName.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftBreakName;
        }

        public List<ShiftBreakName> Gets(Int64 nUserID)
        {
            List<ShiftBreakName> oShiftBreakName = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftBreakNameDA.Gets(tc);
                oShiftBreakName = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShiftBreakName", e);
                #endregion
            }
            return oShiftBreakName;
        }

        public List<ShiftBreakName> Gets(string sSQL, Int64 nUserID)
        {
            List<ShiftBreakName> oShiftBreakName = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftBreakNameDA.Gets(sSQL, tc);
                oShiftBreakName = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShiftBreakName", e);
                #endregion
            }
            return oShiftBreakName;
        }

        #endregion

        #region Activity
        public ShiftBreakName Activite(int nShiftBNID, Int64 nUserId)
        {
            ShiftBreakName oShiftBreakName = new ShiftBreakName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBreakNameDA.Activity(nShiftBNID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBreakName = CreateObject(oReader);
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
                oShiftBreakName.ErrorMessage = e.Message;
                #endregion
            }

            return oShiftBreakName;
        }


        #endregion

    }
}
