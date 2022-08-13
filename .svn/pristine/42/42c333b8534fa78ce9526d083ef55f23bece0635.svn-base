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

    public class UserWiseContractorConfigureService : MarshalByRefObject, IUserWiseContractorConfigureService
    {
        #region Private functions and declaration
        private UserWiseContractorConfigure MapObject(NullHandler oReader)
        {
            UserWiseContractorConfigure oUserWiseContractorConfigure = new UserWiseContractorConfigure();
            oUserWiseContractorConfigure.UserWiseContractorConfigureID = oReader.GetInt32("UserWiseContractorConfigureID");
            oUserWiseContractorConfigure.UserID = oReader.GetInt32("UserID");
            oUserWiseContractorConfigure.ContractorName = oReader.GetString("ContractorName");
            oUserWiseContractorConfigure.UserName = oReader.GetString("UserName");
            oUserWiseContractorConfigure.ContractorID = oReader.GetInt32("ContractorID");

            return oUserWiseContractorConfigure;
        }

        private UserWiseContractorConfigure CreateObject(NullHandler oReader)
        {
            UserWiseContractorConfigure oUserWiseContractorConfigure = new UserWiseContractorConfigure();
            oUserWiseContractorConfigure = MapObject(oReader);
            return oUserWiseContractorConfigure;
        }

        private List<UserWiseContractorConfigure> CreateObjects(IDataReader oReader)
        {
            List<UserWiseContractorConfigure> oUserWiseContractorConfigure = new List<UserWiseContractorConfigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                UserWiseContractorConfigure oItem = CreateObject(oHandler);
                oUserWiseContractorConfigure.Add(oItem);
            }
            return oUserWiseContractorConfigure;
        }

        #endregion

        #region Interface implementation
        public UserWiseContractorConfigureService() { }

        public string Save(UserWiseContractorConfigure oUserWiseContractorConfigure, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<UserWiseContractorConfigure> oUserWiseContractorConfigures = new List<UserWiseContractorConfigure>();
                oUserWiseContractorConfigures = oUserWiseContractorConfigure.UserWiseContractorConfigures;
                UserWiseContractorConfigure oNewUserWiseContractorConfigure = new UserWiseContractorConfigure();
                string sContractorIDs = "", sUserWiseContractorConfigureDetailIDs = "" ;
                int nTempUserID = oUserWiseContractorConfigure.UserID;//Set UserID
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oUserWiseContractorConfigures.Count > 0)
                {
                    foreach (UserWiseContractorConfigure oItem in oUserWiseContractorConfigures)
                    {
                        if (oItem.UserWiseContractorConfigureID <= 0)
                        {
                            AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.UserWiseContractorConfigure, EnumRoleOperationType.Add);
                            reader = UserWiseContractorConfigureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.UserWiseContractorConfigure, EnumRoleOperationType.Edit);
                            reader = UserWiseContractorConfigureDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oNewUserWiseContractorConfigure = new UserWiseContractorConfigure();
                            oNewUserWiseContractorConfigure = CreateObject(oReader);
                            sContractorIDs += oReader.GetInt32("ContractorID") + ",";
                        }
                        reader.Close();
                        if(oItem.UserWiseContractorConfigureDetails.Count>0)
                        {
                            List<UserWiseContractorConfigureDetail> oUserWiseContractorConfigureDetails = new List<UserWiseContractorConfigureDetail>();
                            oUserWiseContractorConfigureDetails = oItem.UserWiseContractorConfigureDetails;
                            foreach (UserWiseContractorConfigureDetail oDetailItem in oUserWiseContractorConfigureDetails)
                            {
                                IDataReader readerdetail;
                                oDetailItem.UserWiseContractorConfigureID = oNewUserWiseContractorConfigure.UserWiseContractorConfigureID;
                                if (oDetailItem.UserWiseContractorConfigureDetailID <= 0)
                                {
                                    readerdetail = UserWiseContractorConfigureDetailDA.InsertUpdate(tc, oDetailItem, EnumDBOperation.Insert, nUserID, "");
                                }
                                else
                                {
                                    readerdetail = UserWiseContractorConfigureDetailDA.InsertUpdate(tc, oDetailItem, EnumDBOperation.Update, nUserID, "");
                                }
                                NullHandler oReaderDetail = new NullHandler(readerdetail);
                                if (readerdetail.Read())
                                {
                                    sUserWiseContractorConfigureDetailIDs = sUserWiseContractorConfigureDetailIDs + oReaderDetail.GetString("UserWiseContractorConfigureDetailID") + ",";
                                }
                                readerdetail.Close();
                            }
                            if (sUserWiseContractorConfigureDetailIDs.Length > 0)
                            {
                                sUserWiseContractorConfigureDetailIDs = sUserWiseContractorConfigureDetailIDs.Remove(sUserWiseContractorConfigureDetailIDs.Length - 1, 1);
                            }
                            UserWiseContractorConfigureDetail oUserWiseContractorConfigureDetail = new UserWiseContractorConfigureDetail();
                            oUserWiseContractorConfigureDetail.UserWiseContractorConfigureID = oNewUserWiseContractorConfigure.UserWiseContractorConfigureID;
                            UserWiseContractorConfigureDetailDA.Delete(tc, oUserWiseContractorConfigureDetail, EnumDBOperation.Delete, nUserID, sUserWiseContractorConfigureDetailIDs);
                            sUserWiseContractorConfigureDetailIDs = "";//Reset
                        }
                    }
                }
                if(sContractorIDs.Length>0)
                {
                    sContractorIDs = sContractorIDs.Remove(sContractorIDs.Length - 1, 1);
                }
                oUserWiseContractorConfigure = new UserWiseContractorConfigure();
                oUserWiseContractorConfigure.UserID = nTempUserID;
                UserWiseContractorConfigureDA.Delete(tc, oUserWiseContractorConfigure, EnumDBOperation.Delete, nUserID, sContractorIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Successfully Saved";
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                UserWiseContractorConfigure oUserWiseContractorConfigure = new UserWiseContractorConfigure();
                oUserWiseContractorConfigure.UserWiseContractorConfigureID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.UserWiseContractorConfigure, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "UserWiseContractorConfigure", id);
                UserWiseContractorConfigureDA.Delete(tc, oUserWiseContractorConfigure, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete UserWiseContractorConfigure. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public UserWiseContractorConfigure Get(int id, Int64 nUserId)
        {
            UserWiseContractorConfigure oAccountHead = new UserWiseContractorConfigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserWiseContractorConfigureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get UserWiseContractorConfigure", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<UserWiseContractorConfigure> GetsByUser(int id, Int64 nUserID)
        {
            List<UserWiseContractorConfigure> oUserWiseContractorConfigure = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseContractorConfigureDA.GetsByUser(id,tc);
                oUserWiseContractorConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseContractorConfigure", e);
                #endregion
            }

            return oUserWiseContractorConfigure;
        }
        public List<UserWiseContractorConfigure> Gets(Int64 nUserID)
        {
            List<UserWiseContractorConfigure> oUserWiseContractorConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseContractorConfigureDA.Gets(tc);
                oUserWiseContractorConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseContractorConfigure", e);
                #endregion
            }

            return oUserWiseContractorConfigure;
        }

        public List<UserWiseContractorConfigure> Gets(string sSQL, Int64 nUserID)
        {
            List<UserWiseContractorConfigure> oUserWiseContractorConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserWiseContractorConfigureDA.Gets(tc, sSQL);
                oUserWiseContractorConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserWiseContractorConfigure", e);
                #endregion
            }

            return oUserWiseContractorConfigure;
        }

        #endregion
    }
   
}
