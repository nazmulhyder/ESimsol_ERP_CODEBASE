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
    public class ShiftBULocConfigureService : MarshalByRefObject, IShiftBULocConfigureService
    {
        #region Private functions and declaration
        private ShiftBULocConfigure MapObject(NullHandler oReader)
        {
            ShiftBULocConfigure oShiftBULocConfigure = new ShiftBULocConfigure();
            oShiftBULocConfigure.ShiftBULocID = oReader.GetInt32("ApprovalHeadID");
            oShiftBULocConfigure.BUID = oReader.GetInt32("BUID");
            oShiftBULocConfigure.LocationID = oReader.GetInt32("LocationID");
            oShiftBULocConfigure.ShiftID = oReader.GetInt32("ShiftID");
            oShiftBULocConfigure.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oShiftBULocConfigure.LocationName = oReader.GetString("LocationName");
            oShiftBULocConfigure.Name = oReader.GetString("Name");
            return oShiftBULocConfigure;

        }

        private ShiftBULocConfigure CreateObject(NullHandler oReader)
        {
            ShiftBULocConfigure oShiftBULocConfigure = MapObject(oReader);
            return oShiftBULocConfigure;
        }

        private List<ShiftBULocConfigure> CreateObjects(IDataReader oReader)
        {
            List<ShiftBULocConfigure> oShiftBULocConfigure = new List<ShiftBULocConfigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShiftBULocConfigure oItem = CreateObject(oHandler);
                oShiftBULocConfigure.Add(oItem);
            }
            return oShiftBULocConfigure;
        }


        #endregion

        #region Interface implementation
        public ShiftBULocConfigureService() { }
        public ShiftBULocConfigure IUD(ShiftBULocConfigure oShiftBULocConfigure, Int64 nUserID)
        {
            

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                ShiftBULocConfigureDA.Delete(tc, oShiftBULocConfigure.BUID, oShiftBULocConfigure.LocationID, nUserID);
                if (oShiftBULocConfigure.ErrorMessage.Length > 0)
                {
                    foreach (var oItem in oShiftBULocConfigure.ErrorMessage.Split(','))
                    {
                        ShiftBULocConfigure oShiftBULocConfigure_Temp = new ShiftBULocConfigure()
                        {
                            ShiftBULocID = oShiftBULocConfigure.ShiftBULocID,
                            BUID = oShiftBULocConfigure.BUID,
                            LocationID = oShiftBULocConfigure.LocationID,
                            ShiftID = Convert.ToInt32(oItem)
                        };
                        reader = ShiftBULocConfigureDA.IUD(tc, oShiftBULocConfigure_Temp, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oShiftBULocConfigure = new ShiftBULocConfigure();
                            oShiftBULocConfigure = CreateObject(oReader);
                        }
                        reader.Close();
                    }

                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oShiftBULocConfigure = new ShiftBULocConfigure();
                oShiftBULocConfigure.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oShiftBULocConfigure;
        }



        public ShiftBULocConfigure Get(string sSQL, Int64 nUserId)
        {
            ShiftBULocConfigure oShiftBULocConfigure = new ShiftBULocConfigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShiftBULocConfigureDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShiftBULocConfigure = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oShiftBULocConfigure;
        }

        public List<ShiftBULocConfigure> Gets(string sSQL, Int64 nUserID)
        {
            List<ShiftBULocConfigure> oShiftBULocConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShiftBULocConfigureDA.Gets(sSQL, tc);
                oShiftBULocConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oShiftBULocConfigure;
        }
        #endregion
    }
}


