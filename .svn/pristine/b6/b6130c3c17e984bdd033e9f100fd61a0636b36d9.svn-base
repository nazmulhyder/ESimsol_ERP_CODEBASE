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
    public class RouteLocationService : MarshalByRefObject, IRouteLocationService
    {
        #region Private functions and declaration
        private RouteLocation MapObject(NullHandler oReader)
        {
            RouteLocation oRouteLocation = new RouteLocation();
            oRouteLocation.RouteLocationID = oReader.GetInt32("RouteLocationID");
            oRouteLocation.LocCode = oReader.GetString("LocCode");
            oRouteLocation.Name = oReader.GetString("Name");
            oRouteLocation.Description = oReader.GetString("Description");
            oRouteLocation.LocationType = (EnumRouteLocation)oReader.GetInt16("LocationType");
            oRouteLocation.LocationTypeInt = oReader.GetInt16("LocationType");
            oRouteLocation.BUID = oReader.GetInt32("BUID");
            return oRouteLocation;
        }

        private RouteLocation CreateObject(NullHandler oReader)
        {
            RouteLocation oRouteLocation = new RouteLocation();
            oRouteLocation = MapObject(oReader);
            return oRouteLocation;
        }

        private List<RouteLocation> CreateObjects(IDataReader oReader)
        {
            List<RouteLocation> oRouteLocation = new List<RouteLocation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RouteLocation oItem = CreateObject(oHandler);
                oRouteLocation.Add(oItem);
            }
            return oRouteLocation;
        }

        #endregion

        #region Interface implementation
        public RouteLocationService() { }
        public RouteLocation Save(RouteLocation oRouteLocation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRouteLocation.RouteLocationID <= 0)
                {
                    reader = RouteLocationDA.InsertUpdate(tc, oRouteLocation, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = RouteLocationDA.InsertUpdate(tc, oRouteLocation, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRouteLocation = new RouteLocation();
                    oRouteLocation = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oRouteLocation = new RouteLocation();
                oRouteLocation.ErrorMessage = e.Message;

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save RouteLocation. Because of " + e.Message, e);
                #endregion
            }
            return oRouteLocation;
        }       
        public string Delete(RouteLocation oRouteLocation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RouteLocationDA.Delete(tc, oRouteLocation, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRouteLocation = new RouteLocation();
                oRouteLocation.ErrorMessage = e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }

        public List<RouteLocation> Gets(string sSQL, int nUserId)
        {
            List<RouteLocation> oContractor = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteLocationDA.Gets(tc, sSQL);
                oContractor = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteLocation", e);
                #endregion
            }

            return oContractor;
        }
        public RouteLocation Get(int id, Int64 nUserId)
        {
            RouteLocation oAccountHead = new RouteLocation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RouteLocationDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get RouteLocation", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<RouteLocation> Gets(Int64 nUserId)
        {
            List<RouteLocation> oRouteLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteLocationDA.Gets(tc);
                oRouteLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteLocation", e);
                #endregion
            }

            return oRouteLocation;
        }

        //
        public List<RouteLocation> BUWiseGets(int BUID, Int64 nUserId)
        {
            List<RouteLocation> oRouteLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RouteLocationDA.BUWiseGets(BUID, tc);
                oRouteLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteLocation", e);
                #endregion
            }

            return oRouteLocation;
        }
        public List<RouteLocation> Gets(int neEnumRouteLocation, Int64 nUserId)
        {
            List<RouteLocation> oRouteLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RouteLocationDA.Gets(tc, neEnumRouteLocation);
                oRouteLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RouteLocation", e);
                #endregion
            }

            return oRouteLocation;
        }
        #endregion
    }
}