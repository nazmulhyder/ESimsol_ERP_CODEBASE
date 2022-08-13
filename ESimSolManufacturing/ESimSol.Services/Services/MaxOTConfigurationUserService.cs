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
    public class MaxOTConfigurationUserService : MarshalByRefObject, IMaxOTConfigurationUserService
    {
        #region Private functions and declaration
        private MaxOTConfigurationUser MapObject(NullHandler oReader)
        {
            MaxOTConfigurationUser oMaxOTConfigurationUser = new MaxOTConfigurationUser();
            oMaxOTConfigurationUser.MOCUID = oReader.GetInt32("MOCUID");
            oMaxOTConfigurationUser.MOCID = oReader.GetInt32("MOCID");
            oMaxOTConfigurationUser.UserID = oReader.GetInt32("UserID");
            return oMaxOTConfigurationUser;

        }

        private MaxOTConfigurationUser CreateObject(NullHandler oReader)
        {
            MaxOTConfigurationUser oMaxOTConfigurationUser = MapObject(oReader);
            return oMaxOTConfigurationUser;
        }

        private List<MaxOTConfigurationUser> CreateObjects(IDataReader oReader)
        {
            List<MaxOTConfigurationUser> oMaxOTConfigurationUser = new List<MaxOTConfigurationUser>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MaxOTConfigurationUser oItem = CreateObject(oHandler);
                oMaxOTConfigurationUser.Add(oItem);
            }
            return oMaxOTConfigurationUser;
        }


        #endregion

        #region Interface implementation
        public MaxOTConfigurationUserService() { }
        public string IUD(MaxOTConfigurationUser oMaxOTConfigurationUser, bool IsShortList, bool IsUserBased, Int64 nUserID)
        {
            TransactionContext tc = null;
            MaxOTConfigurationUser oNewMaxOTConfigurationUser = new MaxOTConfigurationUser();
            List<MaxOTConfigurationUser> oMaxOTConfigurationUsers = new List<MaxOTConfigurationUser>();
            oMaxOTConfigurationUsers = oMaxOTConfigurationUser.MaxOTConfigurationUsers;
            string sIds = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (IsShortList == true)
                {
                    foreach (MaxOTConfigurationUser oItem in oMaxOTConfigurationUsers)
                    {
                        if (oItem.MOCUID <= 0)
                        {
                            reader = MaxOTConfigurationUserDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = MaxOTConfigurationUserDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    foreach (MaxOTConfigurationUser oItem in oMaxOTConfigurationUsers)
                    {
                        if (oItem.MOCUID <= 0)
                        {
                            reader = MaxOTConfigurationUserDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = MaxOTConfigurationUserDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);

                        if (reader.Read())
                        {
                            oNewMaxOTConfigurationUser = new MaxOTConfigurationUser();
                            oNewMaxOTConfigurationUser = CreateObject(oReader);
                        }
                        reader.Close();
                        if (IsUserBased == true)
                        {
                            sIds = sIds + oNewMaxOTConfigurationUser.MOCID + ",";
                        }
                        else
                        {
                            sIds = sIds + oNewMaxOTConfigurationUser.UserID + ",";
                        }
                    }
                    if (sIds.Length > 0)
                    {
                        sIds = sIds.Remove(sIds.Length - 1, 1);
                    }
                    MaxOTConfigurationUserDA.Delete(tc, oNewMaxOTConfigurationUser, EnumDBOperation.Delete, nUserID, IsUserBased, sIds);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save Time Card. Because of " + e.Message, e);
                #endregion
            }
            return "Succefully Saved";
        }





        public MaxOTConfigurationUser Get(string sSQL, Int64 nUserId)
        {
            MaxOTConfigurationUser oMaxOTConfigurationUser = new MaxOTConfigurationUser();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MaxOTConfigurationUserDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMaxOTConfigurationUser = CreateObject(oReader);
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
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oMaxOTConfigurationUser;
        }
        public List<MaxOTConfigurationUser> GetsUser(long nUserID, int id)
        {
            List<MaxOTConfigurationUser> oMaxOTConfigurationUsers = new List<MaxOTConfigurationUser>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaxOTConfigurationUserDA.GetsUser(tc, nUserID);
                oMaxOTConfigurationUsers = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MaxOTConfigurationUser User", e);
                #endregion
            }
            return oMaxOTConfigurationUsers;
        }
        public List<MaxOTConfigurationUser> Gets(string sSQL, Int64 nUserID)
        {
            List<MaxOTConfigurationUser> oMaxOTConfigurationUser = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MaxOTConfigurationUserDA.Gets(sSQL, tc);
                oMaxOTConfigurationUser = CreateObjects(reader);
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
            return oMaxOTConfigurationUser;
        }
        #endregion
    }
}

