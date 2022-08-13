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
    public class VehicleColorService : MarshalByRefObject, IVehicleColorService
    {
        #region Private functions and declaration
        private VehicleColor MapObject(NullHandler oReader)
        {
            VehicleColor oVehicleColor = new VehicleColor();
            oVehicleColor.VehicleColorID = oReader.GetInt32("VehicleColorID");
            oVehicleColor.ColorCode = oReader.GetString("ColorCode");
            oVehicleColor.ColorName = oReader.GetString("ColorName");
            oVehicleColor.ColorType = oReader.GetInt32("ColorType");
            oVehicleColor.Remarks = oReader.GetString("Remarks");
            return oVehicleColor;
        }

        private VehicleColor CreateObject(NullHandler oReader)
        {
            VehicleColor oVehicleColor = new VehicleColor();
            oVehicleColor = MapObject(oReader);
            return oVehicleColor;
        }

        private List<VehicleColor> CreateObjects(IDataReader oReader)
        {
            List<VehicleColor> oVehicleColor = new List<VehicleColor>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleColor oItem = CreateObject(oHandler);
                oVehicleColor.Add(oItem);
            }
            return oVehicleColor;
        }

        #endregion

        #region Interface implementation
        public VehicleColorService() { }

        public VehicleColor Save(VehicleColor oVehicleColor, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleColor.VehicleColorID <= 0)
                {
                    reader = VehicleColorDA.InsertUpdate(tc, oVehicleColor, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleColorDA.InsertUpdate(tc, oVehicleColor, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleColor = new VehicleColor();
                    oVehicleColor = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleColor. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleColor;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleColor oVehicleColor = new VehicleColor();
                oVehicleColor.VehicleColorID = id;
                VehicleColorDA.Delete(tc, oVehicleColor, EnumDBOperation.Delete, nUserId);
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

        public VehicleColor Get(int id, Int64 nUserId)
        {
            VehicleColor oVehicleColor = new VehicleColor();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = VehicleColorDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleColor = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleColor", e);
                #endregion
            }
            return oVehicleColor;
        }

        public List<VehicleColor> GetByColorCode(string sColorCode, Int64 nUserID)
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleColorDA.GetsByColorCode(tc, sColorCode);
                oVehicleColors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleColors", e);
                #endregion
            }
            return oVehicleColors;
        }

        public List<VehicleColor> Gets(Int64 nUserID)
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleColorDA.Gets(tc);
                oVehicleColors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleColor", e);
                #endregion
            }
            return oVehicleColors;
        }
        public List<VehicleColor> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleColor> oVehicleColors = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleColorDA.Gets(tc, sSQL);
                oVehicleColors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleColor", e);
                #endregion
            }
            return oVehicleColors;
        }


        public List<VehicleColor> GetsByColorCode(string sColorCode, Int64 nUserID)
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehicleColorDA.GetsByColorCode(tc, sColorCode);
                oVehicleColors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleColors", e);
                #endregion
            }
            return oVehicleColors;
        }
        public List<VehicleColor> GetsByColorNameWithType(string sColorName,int nColorType, Int64 nUserID)
        {
            List<VehicleColor> oVehicleColors = new List<VehicleColor>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehicleColorDA.GetsByColorNameWithType(tc, sColorName, nColorType);
                oVehicleColors = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleColors", e);
                #endregion
            }
            return oVehicleColors;
        }
       
        #endregion
    }
}