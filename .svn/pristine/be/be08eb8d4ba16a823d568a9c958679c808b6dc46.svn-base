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
    public class BusinessLocationService : MarshalByRefObject, IBusinessLocationService
    {
        #region Private functions and declaration
        private BusinessLocation MapObject(NullHandler oReader)
        {
            BusinessLocation oBusinessLocation = new BusinessLocation();
            oBusinessLocation.BusinessLocationID = oReader.GetInt32("BusinessLocationID");
            oBusinessLocation.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oBusinessLocation.LocationID = oReader.GetInt32("LocationID");
            //oBusinessLocation.LocationCode = oReader.GetString("LocationCode");
            //oBusinessLocation.LocationName = oReader.GetString("LocationName");
            //oBusinessLocation.LocationDescription = oReader.GetString("LocationDescription");
            //oBusinessLocation.Locationparentid = oReader.GetInt32("LocationParentID");
            //oBusinessLocation.LocationIsActive = oReader.GetBoolean("LocationIsActive");
            //oBusinessLocation.LocationType = (EnumLocationType)oReader.GetInt32("LocationType");
            //oBusinessLocation.BusinessUnitCode = oReader.GetString("BusinessUnitCode");
            //oBusinessLocation.BusinessUnitName = oReader.GetString("BusinessUnitName");
            //oBusinessLocation.BusinessUnitShortName = oReader.GetString("BusinessUnitShortName");
            
            
            return oBusinessLocation;
        }

        private BusinessLocation CreateObject(NullHandler oReader)
        {
            BusinessLocation oBusinessLocation = new BusinessLocation();
            oBusinessLocation = MapObject(oReader);
            return oBusinessLocation;
        }

        private List<BusinessLocation> CreateObjects(IDataReader oReader)
        {
            List<BusinessLocation> oBusinessLocation = new List<BusinessLocation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BusinessLocation oItem = CreateObject(oHandler);
                oBusinessLocation.Add(oItem);
            }
            return oBusinessLocation;
        }

        #endregion

        #region Interface implementation
        public BusinessLocationService() { }

        public BusinessLocation Save(BusinessLocation oBusinessLocation, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Business Location
                List<BusinessLocation> oBusinessLocations = new List<BusinessLocation>();
                oBusinessLocations = oBusinessLocation.BusinessLocations;


                if (oBusinessLocations != null)
                {
                    string sBusinessLocationIDs = "";
                    foreach (BusinessLocation oItem in oBusinessLocations)
                    {
                        IDataReader readertnc;
                        
                        if (oItem.BusinessLocationID <= 0)
                        {
                            readertnc = BusinessLocationDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = BusinessLocationDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sBusinessLocationIDs = sBusinessLocationIDs + oReaderTNC.GetString("BusinessLocationID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sBusinessLocationIDs.Length > 0)
                    {
                        sBusinessLocationIDs = sBusinessLocationIDs.Remove(sBusinessLocationIDs.Length - 1, 1);
                    }
                    BusinessLocation otempBusinessLocation = new BusinessLocation();
                    otempBusinessLocation.BusinessUnitID = oBusinessLocation.BusinessUnitID;
                    BusinessLocationDA.Delete(tc, otempBusinessLocation, EnumDBOperation.Delete, nUserID, sBusinessLocationIDs);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BusinessLocation. Because of " + e.Message, e);
                #endregion
            }
            return oBusinessLocation;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BusinessLocation oBusinessLocation = new BusinessLocation();
                oBusinessLocation.BusinessLocationID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BusinessLocation, EnumRoleOperationType.Delete);
                BusinessLocationDA.Delete(tc, oBusinessLocation, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BusinessLocation. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public BusinessLocation Get(int id, int nUserId)
        {
            BusinessLocation oAccountHead = new BusinessLocation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BusinessLocationDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BusinessLocation", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<BusinessLocation> Gets(int nUserID)
        {
            List<BusinessLocation> oBusinessLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessLocationDA.Gets(tc);
                oBusinessLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessLocation", e);
                #endregion
            }

            return oBusinessLocation;
        }

        public List<BusinessLocation> Gets(int nBUID, int nUserID)
        {
            List<BusinessLocation> oBusinessLocation = new List<BusinessLocation>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BusinessLocationDA.Gets(tc, nBUID);
                oBusinessLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessLocation", e);
                #endregion
            }

            return oBusinessLocation;
        }
        public List<BusinessLocation> Gets(string sSQL,int nUserID)
        {
            List<BusinessLocation> oBusinessLocation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from BusinessLocation where BusinessLocationID in (1,2,80,272,347,370,60,45)";
                    }
                reader = BusinessLocationDA.Gets(tc, sSQL);
                oBusinessLocation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessLocation", e);
                #endregion
            }

            return oBusinessLocation;
        }

       
        #endregion
    }   
}