using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class UserWiseStyleConfigureService : MarshalByRefObject, IUserWiseStyleConfigureService
    {
        #region Private functions and declaration
        private UserWiseStyleConfigure MapObject(NullHandler oReader)
        {
            UserWiseStyleConfigure oUserWiseStyleConfigure = new UserWiseStyleConfigure();
            oUserWiseStyleConfigure.UserWiseStyleConfigureID = oReader.GetInt32("UserWiseStyleConfigureID");
            oUserWiseStyleConfigure.UserID = oReader.GetInt32("UserID");
            oUserWiseStyleConfigure.UserName = oReader.GetString("UserName");
            oUserWiseStyleConfigure.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oUserWiseStyleConfigure.StyleNo = oReader.GetString("StyleNo");
            oUserWiseStyleConfigure.BuyerName = oReader.GetString("BuyerName");
            oUserWiseStyleConfigure.BuyerID = oReader.GetInt32("BuyerID");
            oUserWiseStyleConfigure.SessionName = oReader.GetString("SessionName");
            oUserWiseStyleConfigure.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oUserWiseStyleConfigure.GarmmentsProductName = oReader.GetString("GarmmentsProductName");
            oUserWiseStyleConfigure.DevelopmentStatus = (EnumDevelopmentStatus)oReader.GetInt32("DevelopmentStatus");
            oUserWiseStyleConfigure.YarnCategoryName = oReader.GetString("YarnCategoryName");

            return oUserWiseStyleConfigure;
        }

        private UserWiseStyleConfigure CreateObject(NullHandler oReader)
        {
            UserWiseStyleConfigure oUserWiseStyleConfigure = new UserWiseStyleConfigure();
            oUserWiseStyleConfigure = MapObject(oReader);
            return oUserWiseStyleConfigure;
        }

        private List<UserWiseStyleConfigure> CreateObjects(IDataReader oReader)
        {
            List<UserWiseStyleConfigure> oUserWiseStyleConfigure = new List<UserWiseStyleConfigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                UserWiseStyleConfigure oItem = CreateObject(oHandler);
                oUserWiseStyleConfigure.Add(oItem);
            }
            return oUserWiseStyleConfigure;
        }

        #endregion

        #region Interface implementation
        public UserWiseStyleConfigureService() { }

        public UserWiseStyleConfigure Save(UserWiseStyleConfigure oUserWiseStyleConfigure, Int64 nUserID)
        {
            TransactionContext tc = null; string sUserWiseStyleConfigureIDs = "";
            List<UserWiseStyleConfigure> oUserWiseStyleConfigures = new List<UserWiseStyleConfigure>();
            oUserWiseStyleConfigures = oUserWiseStyleConfigure.UserWiseStyleConfigures;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.UserWiseStyleConfigure, EnumRoleOperationType.Add);

                foreach (UserWiseStyleConfigure oItem in oUserWiseStyleConfigures)
                {
                    IDataReader reader;
                    reader = UserWiseStyleConfigureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oUserWiseStyleConfigure = new UserWiseStyleConfigure();
                        oUserWiseStyleConfigure = CreateObject(oReader);
                        sUserWiseStyleConfigureIDs = sUserWiseStyleConfigureIDs + oUserWiseStyleConfigure.UserWiseStyleConfigureID + ",";
                    }
                    reader.Close();                        
                }
                if (sUserWiseStyleConfigureIDs.Length > 0)
                {
                    sUserWiseStyleConfigureIDs = sUserWiseStyleConfigureIDs.Remove(sUserWiseStyleConfigureIDs.Length - 1, 1);
                    string sSQL = "SELECT * FROM View_UserWiseStyleConfigure WHERE UserWiseStyleConfigureID IN (" + sUserWiseStyleConfigureIDs + ")";
                    IDataReader readers = null;
                    readers = UserWiseStyleConfigureDA.Gets(tc, sSQL);
                    oUserWiseStyleConfigures = new List<UserWiseStyleConfigure>();
                    oUserWiseStyleConfigures = CreateObjects(readers);
                    oUserWiseStyleConfigure = new UserWiseStyleConfigure();
                    oUserWiseStyleConfigure.UserWiseStyleConfigures = oUserWiseStyleConfigures;
                    readers.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)tc.HandleError();
                oUserWiseStyleConfigure = new UserWiseStyleConfigure();
                oUserWiseStyleConfigure.ErrorMessage = e.Message.Split('!')[0];
                return oUserWiseStyleConfigure;
                #endregion
            }

            return oUserWiseStyleConfigure;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                UserWiseStyleConfigure oUserWiseStyleConfigure = new UserWiseStyleConfigure();
                oUserWiseStyleConfigure.UserWiseStyleConfigureID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.UserWiseStyleConfigure, EnumRoleOperationType.Delete);
                UserWiseStyleConfigureDA.Delete(tc, oUserWiseStyleConfigure, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete UserWiseStyleConfigure. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public UserWiseStyleConfigure Get(int id, Int64 nUserId)
        {
            UserWiseStyleConfigure oUserWiseStyleConfigure = new UserWiseStyleConfigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserWiseStyleConfigureDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUserWiseStyleConfigure = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get UserWiseStyleConfigure", e);
                #endregion
            }

            return oUserWiseStyleConfigure;
        }


        public List<UserWiseStyleConfigure> GetsByUser(int id, Int64 nUserID)
        {
            List<UserWiseStyleConfigure> oUserWiseStyleConfigure = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseStyleConfigureDA.GetsByUser(id, tc);
                oUserWiseStyleConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseStyleConfigure", e);
                #endregion
            }

            return oUserWiseStyleConfigure;
        }
        public List<UserWiseStyleConfigure> Gets(Int64 nUserID)
        {
            List<UserWiseStyleConfigure> oUserWiseStyleConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseStyleConfigureDA.Gets(tc);
                oUserWiseStyleConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseStyleConfigure", e);
                #endregion
            }

            return oUserWiseStyleConfigure;
        }

        public List<UserWiseStyleConfigure> Gets(string sSQL, Int64 nUserID)
        {
            List<UserWiseStyleConfigure> oUserWiseStyleConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseStyleConfigureDA.Gets(tc, sSQL);
                oUserWiseStyleConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseStyleConfigure", e);
                #endregion
            }

            return oUserWiseStyleConfigure;
        }

        #endregion
    }
    

}
