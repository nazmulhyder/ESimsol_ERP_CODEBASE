using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class AuthorizationUserOEDOService : MarshalByRefObject, IAuthorizationUserOEDOService
    {
        #region Private functions and declaration
        private AuthorizationUserOEDO MapObject(NullHandler oReader)
        {
            AuthorizationUserOEDO oAuthorizationUserOEDO = new AuthorizationUserOEDO();
            oAuthorizationUserOEDO.AUOEDOID = oReader.GetInt32("AUOEDOID");
            oAuthorizationUserOEDO.UserID = oReader.GetInt32("UserID");
            oAuthorizationUserOEDO.AWUOEDBID = oReader.GetInt32("AWUOEDBID");
            oAuthorizationUserOEDO.IsActive = oReader.GetBoolean("IsActive");
            oAuthorizationUserOEDO.DBObjectName = oReader.GetString("DBObjectName");
            oAuthorizationUserOEDO.OEName = oReader.GetString("OEName");
            oAuthorizationUserOEDO.OEValue = (EnumOperationFunctionality)oReader.GetInt16("OEValue");
            oAuthorizationUserOEDO.IsMTRApply = oReader.GetBoolean("IsMTRApply");
            oAuthorizationUserOEDO.TriggerParentType = (EnumTriggerParentsType)oReader.GetInt16("TriggerParentType");
            oAuthorizationUserOEDO.WorkingUnitName = oReader.GetString("WorkingUnitName");
            oAuthorizationUserOEDO.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            return oAuthorizationUserOEDO;
        }

        private AuthorizationUserOEDO CreateObject(NullHandler oReader)
        {
            AuthorizationUserOEDO oAuthorizationUserOEDO = new AuthorizationUserOEDO();
            oAuthorizationUserOEDO = MapObject(oReader);
            return oAuthorizationUserOEDO;
        }

        private List<AuthorizationUserOEDO> CreateObjects(IDataReader oReader)
        {
            List<AuthorizationUserOEDO> oAuthorizationUserOEDOs = new List<AuthorizationUserOEDO>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AuthorizationUserOEDO oItem = CreateObject(oHandler);
                oAuthorizationUserOEDOs.Add(oItem);
            }
            return oAuthorizationUserOEDOs;
        }

        #endregion

        #region Interface implementation
        public AuthorizationUserOEDOService() { }

        public List<AuthorizationUserOEDO> Save(AuthorizationUserOEDO oAuthorizationUserOEDO, Int64 nUserId)
        {
            TransactionContext tc = null;
            List<AuthorizationUserOEDO> oAuthorizationUserOEDOs = new List<AuthorizationUserOEDO>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oAuthorizationUserOEDO.AUOEDOID > 0)
                {
                    reader = AuthorizationUserOEDODA.InsertUpdate(tc, oAuthorizationUserOEDO, EnumDBOperation.Update, nUserId);
                }
                else
                {
                    reader = AuthorizationUserOEDODA.InsertUpdate(tc, oAuthorizationUserOEDO, EnumDBOperation.Insert, nUserId);
                }
                oAuthorizationUserOEDOs = CreateObjects(reader);
                reader.Close();
                tc.End();               
            }
            catch (Exception e)
            {
                #region Handle Exception
                oAuthorizationUserOEDOs = new List<AuthorizationUserOEDO>();
                oAuthorizationUserOEDO = new AuthorizationUserOEDO();
                oAuthorizationUserOEDO.ErrorMessage = e.Message.Split('!')[0];
                oAuthorizationUserOEDOs.Add(oAuthorizationUserOEDO);
                #endregion
            }
            return oAuthorizationUserOEDOs;
        }

        public string Delete(AuthorizationUserOEDO oAuthorizationUserOEDO, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);



                AuthorizationUserOEDODA.Delete(tc, oAuthorizationUserOEDO, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAuthorizationUserOEDO.ErrorMessage = e.Message.Split('!')[0];
                return "Deletion not possible please try again";
                #endregion
            }

        }

        public AuthorizationUserOEDO Get(int id, Int64 nUserId)
        {
            AuthorizationUserOEDO oAuthorizationUserOEDO = new AuthorizationUserOEDO();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AuthorizationUserOEDODA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAuthorizationUserOEDO = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Authorization User Operational Event", e);
                #endregion
            }

            return oAuthorizationUserOEDO;
        }


        public List<AuthorizationUserOEDO> Gets(Int64 nUserId)
        {
            List<AuthorizationUserOEDO> oAuthorizationUserOEDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationUserOEDODA.Gets(tc);
                oAuthorizationUserOEDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Authorization User Operational Event", e);
                #endregion
            }

            return oAuthorizationUserOEDO;
        }


        public List<AuthorizationUserOEDO> Gets(string sSQL, Int64 nUserId)
        {
            List<AuthorizationUserOEDO> oAuthorizationUserOEDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationUserOEDODA.Gets(tc, sSQL);
                oAuthorizationUserOEDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Authorization User Operational Event", e);
                #endregion
            }

            return oAuthorizationUserOEDO;
        }

        public List<AuthorizationUserOEDO> GetsByUser(int ID, Int64 nUserID)
        {
            List<AuthorizationUserOEDO> oAuthorizationUserOEDO = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AuthorizationUserOEDODA.GetsByUser(tc, ID);
                oAuthorizationUserOEDO = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Authorization User Operational Event", e);
                #endregion
            }

            return oAuthorizationUserOEDO;
        }
        #endregion
    }
   
  
}
