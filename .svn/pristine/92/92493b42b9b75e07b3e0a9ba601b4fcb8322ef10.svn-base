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
    public class LocationService : MarshalByRefObject, ILocationService
    {
        #region Private functions and declaration
        private Location MapObject(NullHandler oReader)
        {
            Location oLocation = new Location();
            oLocation.LocationID = oReader.GetInt32("LocationID");
            oLocation.LocCode = oReader.GetString("LocCode");
            oLocation.NameInBangla = oReader.GetString("NameInBangla");
            oLocation.Name = oReader.GetString("Name");
            oLocation.ShortName = oReader.GetString("ShortName");
            oLocation.Description = oReader.GetString("Description");
            oLocation.ParentID = oReader.GetInt32("ParentID");
            oLocation.IsActive = oReader.GetBoolean("IsActive");
            oLocation.LocationType = (EnumLocationType)oReader.GetInt16("LocationType");
            oLocation.LocationNameCode = oReader.GetString("LocationNameCode");
            oLocation.ParentName = oReader.GetString("ParentName");
            oLocation.AreaID = oReader.GetInt32("AreaID");
            oLocation.AreaName = oReader.GetString("AreaName");
            oLocation.LocationName = oReader.GetString("LocationName");
            
            return oLocation;
        }

        private Location CreateObject(NullHandler oReader)
        {
            Location oLocation = new Location();
            oLocation = MapObject(oReader);
            return oLocation;
        }

        private List<Location> CreateObjects(IDataReader oReader)
        {
            List<Location> oLocation = new List<Location>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Location oItem = CreateObject(oHandler);
                oLocation.Add(oItem);
            }
            return oLocation;
        }

        #endregion

        #region Interface implementation
        public LocationService() { }

        public Location Save(Location oLocation, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLocation.LocationID <= 0)
                {
                    reader = LocationDA.InsertUpdate(tc, oLocation, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = LocationDA.InsertUpdate(tc, oLocation, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLocation = new Location();
                    oLocation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLocation = new Location();
                oLocation.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Location. Because of " + e.Message, e);
                #endregion
            }
            return oLocation;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Location oLocation = new Location();
                oLocation.LocationID = id;
                LocationDA.Delete(tc, oLocation, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Location. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Location Get(int id, int nUserId)
        {
            Location oAccountHead = new Location();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LocationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Location> Gets(int nUserId)
        {
            List<Location> oLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LocationDA.Gets(tc);
                oLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocation;
        }
        public List<Location> GetsByType(EnumLocationType eLocationType, int nUserId)
        {
            List<Location> oLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LocationDA.GetsByType(tc,eLocationType);
                oLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocation;
        }
        public List<Location> GetsByCodeOrName(Location oLocation, int nUserID)
        {
            List<Location> oLocations = new List<Location>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LocationDA.GetsByCodeOrName(tc, oLocation);
                oLocations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLocations = new List<Location>();
                oLocation = new Location();
                oLocation.ErrorMessage = e.Message;
                oLocations.Add(oLocation);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocations;
        }

        public List<Location> GetsByCodeOrNamePick(Location oLocation, int nUserID)
        {
            List<Location> oLocations = new List<Location>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LocationDA.GetsByCodeOrNamePick(tc, oLocation);
                oLocations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLocations = new List<Location>();
                oLocation = new Location();
                oLocation.ErrorMessage = e.Message;
                oLocations.Add(oLocation);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocations;
        }
        public List<Location> GetsByCode(Location oLocation, int nUserID)
        {
            List<Location> oLocations = new List<Location>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LocationDA.GetsByCode(tc, oLocation);
                oLocations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLocations = new List<Location>();
                oLocation = new Location();
                oLocation.ErrorMessage = e.Message;
                oLocations.Add(oLocation);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocations;
        }
        public List<Location> Gets(string sSQL, int nUserId)
        {
            List<Location> oLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LocationDA.Gets(tc, sSQL);
                oLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocation;
        }


        public List<Location> GetsAll(int nUserID)
        {
            List<Location> oLocations = new List<Location>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LocationDA.GetsAll(tc);
                oLocations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Locationss", e);
                #endregion
            }

            return oLocations;
        }
        //Added by Fauzul on 4 June 2013

        public List<Location> GetsIncludingStore(int nUserId)
        {
            List<Location> oLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LocationDA.GetsIncludingStore(tc);
                oLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Location", e);
                #endregion
            }

            return oLocation;
        }

        #endregion
    }
}