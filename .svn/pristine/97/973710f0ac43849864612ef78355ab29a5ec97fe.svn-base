using System;
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
    public class MaxOTConfigurationService : MarshalByRefObject, IMaxOTConfigurationService
    {
        #region Private functions and declaration
        private MaxOTConfiguration MapObject(NullHandler oReader)
        {
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            oMaxOTConfiguration.MOCID = oReader.GetInt32("MOCID");
            oMaxOTConfiguration.MaxOTInMin = oReader.GetInt32("MaxOTInMin");
            oMaxOTConfiguration.MaxBeforeInMin = oReader.GetInt32("MaxBeforeInMin");
            oMaxOTConfiguration.Sequence = oReader.GetInt32("Sequence");
            oMaxOTConfiguration.TimeCardName = oReader.GetString("TimeCardName");

            oMaxOTConfiguration.IsPresentOnDayOff = oReader.GetBoolean("IsPresentOnDayOff");
            oMaxOTConfiguration.IsPresentOnHoliday = oReader.GetBoolean("IsPresentOnHoliday");
            oMaxOTConfiguration.IsActive = oReader.GetBoolean("IsActive");
            oMaxOTConfiguration.UserName = oReader.GetString("UserName");
            oMaxOTConfiguration.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oMaxOTConfiguration.SourceTimeCardID = oReader.GetInt32("SourceTimeCardID");
            oMaxOTConfiguration.SourceTimeCardName = oReader.GetString("SourceTimeCardName");

            return oMaxOTConfiguration;

        }


        private MaxOTConfiguration CreateObject(NullHandler oReader)
        {
            MaxOTConfiguration oMaxOTConfiguration = MapObject(oReader);
            return oMaxOTConfiguration;
        }

        private List<MaxOTConfiguration> CreateObjects(IDataReader oReader)
        {
            List<MaxOTConfiguration> oMaxOTConfiguration = new List<MaxOTConfiguration>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MaxOTConfiguration oItem = CreateObject(oHandler);
                oMaxOTConfiguration.Add(oItem);
            }
            return oMaxOTConfiguration;
        }

        #endregion

        #region Interface implementation
        public MaxOTConfigurationService() { }

        public MaxOTConfiguration Save(MaxOTConfiguration oMaxOTConfiguration, long nUserID)
        {
            TransactionContext tc = null;
            string sMaxOTCEmployeeTypeIDs = "";
            List<MaxOTCEmployeeType> oMaxOTCEmployeeTypes = new List<MaxOTCEmployeeType>();
            MaxOTCEmployeeType oMaxOTCEmployeeType = new MaxOTCEmployeeType();
            oMaxOTCEmployeeTypes = oMaxOTConfiguration.MaxOTCEmployeeTypes;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMaxOTConfiguration.MOCID <= 0)
                {
                    reader = MaxOTConfigurationDA.IUD(tc, oMaxOTConfiguration, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MaxOTConfigurationDA.IUD(tc, oMaxOTConfiguration, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaxOTConfiguration = new MaxOTConfiguration();
                    oMaxOTConfiguration = CreateObject(oReader);
                }
                reader.Close();
                #region MaxOTCEmployeeType  Part
                foreach (MaxOTCEmployeeType oItem in oMaxOTCEmployeeTypes)
                {
                    IDataReader readerdetail;
                    oItem.MaxOTConfigurationID = oMaxOTConfiguration.MOCID;
                    if (oItem.MaxOTCEmployeeTypeID <= 0)
                    {
                        readerdetail = MaxOTCEmployeeTypeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = MaxOTCEmployeeTypeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sMaxOTCEmployeeTypeIDs = sMaxOTCEmployeeTypeIDs + oReaderDetail.GetString("MaxOTCEmployeeTypeID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sMaxOTCEmployeeTypeIDs.Length > 0)
                {
                    sMaxOTCEmployeeTypeIDs = sMaxOTCEmployeeTypeIDs.Remove(sMaxOTCEmployeeTypeIDs.Length - 1, 1);
                }
                oMaxOTCEmployeeType = new MaxOTCEmployeeType();
                oMaxOTCEmployeeType.MaxOTConfigurationID = oMaxOTConfiguration.MOCID;
                MaxOTCEmployeeTypeDA.Delete(tc, oMaxOTCEmployeeType, EnumDBOperation.Delete, nUserID, sMaxOTCEmployeeTypeIDs);
                #endregion
                #region Get MaxOTConfiguration
                reader = MaxOTConfigurationDA.Get(tc, oMaxOTConfiguration.MOCID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaxOTConfiguration = new MaxOTConfiguration();
                    oMaxOTConfiguration = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMaxOTConfiguration = new MaxOTConfiguration();
                oMaxOTConfiguration.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMaxOTConfiguration;
        }


        public string Delete(MaxOTConfiguration oMaxOTConfiguration, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MaxOTConfigurationDA.Delete(tc, oMaxOTConfiguration, EnumDBOperation.Delete, nUserId);
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


        public List<MaxOTConfiguration> GetsDayoff(long nUserID)
        {
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaxOTConfigurationDA.GetsDayoff(tc);
                oMaxOTConfigurations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaxOTConfiguration", e);
                #endregion
            }
            return oMaxOTConfigurations;
        }

        public MaxOTConfiguration Get(int id, Int64 nUserId)
        {
            MaxOTConfiguration oMaxOTConfiguration = new MaxOTConfiguration();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MaxOTConfigurationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaxOTConfiguration = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MaxOTConfiguration", e);
                #endregion
            }

            return oMaxOTConfiguration;
        }
        public List<MaxOTConfiguration> Gets(long nUserID)
        {
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaxOTConfigurationDA.Gets(tc);
                oMaxOTConfigurations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaxOTConfiguration", e);
                #endregion
            }
            return oMaxOTConfigurations;
        }

        public List<MaxOTConfiguration> Gets(string sSQL, Int64 nUserID)
        {
            List<MaxOTConfiguration> oMaxOTConfiguration = new List<MaxOTConfiguration>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MaxOTConfigurationDA.Gets(tc, sSQL);
                oMaxOTConfiguration = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaxOTConfiguration", e);
                #endregion
            }
            return oMaxOTConfiguration;
        }
        public List<MaxOTConfiguration> Gets(bool bIsActive, long nUserID)
        {
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaxOTConfigurationDA.Gets(tc, bIsActive);
                oMaxOTConfigurations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaxOTConfiguration", e);
                #endregion
            }
            return oMaxOTConfigurations;
        }
        public MaxOTConfiguration Activity(MaxOTConfiguration oMaxOTConfiguration, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MaxOTConfigurationDA.Activity(tc, oMaxOTConfiguration, nUserID);

                IDataReader reader;
                reader = MaxOTConfigurationDA.Get(tc, oMaxOTConfiguration.MOCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaxOTConfiguration = new MaxOTConfiguration();
                    oMaxOTConfiguration = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oMaxOTConfiguration = new MaxOTConfiguration();
                    oMaxOTConfiguration.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oMaxOTConfiguration;
        }

        public List<MaxOTConfiguration> GetsByUser(long nUserID)
        {
            List<MaxOTConfiguration> oMaxOTConfigurations = new List<MaxOTConfiguration>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaxOTConfigurationDA.GetsByUser(tc, nUserID);
                oMaxOTConfigurations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaxOTConfiguration User", e);
                #endregion
            }
            return oMaxOTConfigurations;
        }

        #endregion
    }
}

