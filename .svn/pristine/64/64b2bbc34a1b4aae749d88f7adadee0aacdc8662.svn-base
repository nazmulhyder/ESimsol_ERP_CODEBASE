using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using System.Net.Mail;

namespace ESimSol.Services.Services
{
    public class HumanInteractionAgentService : MarshalByRefObject, IHumanInteractionAgentService
    {
        #region Private functions and declaration
        int nSLNo = 0;
        private HumanInteractionAgent MapObject(NullHandler oReader)
        {
            nSLNo = nSLNo + 1;
            HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
            oHumanInteractionAgent.HIAID = oReader.GetInt32("HIAID");
            oHumanInteractionAgent.HIASetupID = oReader.GetInt32("HIASetupID");
            oHumanInteractionAgent.MessageBodyText = oReader.GetString("MessageBodyText");
            oHumanInteractionAgent.ProcessDateTime = oReader.GetDateTime("ProcessDateTime");
            oHumanInteractionAgent.OrginType = oReader.GetString("OrginType");
            oHumanInteractionAgent.UserID = oReader.GetInt32("UserID");
            oHumanInteractionAgent.IsRead = oReader.GetBoolean("IsRead");
            oHumanInteractionAgent.ReadDatetime = oReader.GetDateTime("ReadDatetime");
            oHumanInteractionAgent.OperationObjectID = oReader.GetInt32("OperationObjectID");
            oHumanInteractionAgent.LinkReference = oReader.GetString("LinkReference");
            oHumanInteractionAgent.View_Hight = oReader.GetInt32("View_Hight");
            oHumanInteractionAgent.View_Wight = oReader.GetInt32("View_Wight");
            oHumanInteractionAgent.Parameter = oReader.GetString("Parameter");
            oHumanInteractionAgent.SetupName = oReader.GetString("SetupName");
            return oHumanInteractionAgent;
        }

        private HumanInteractionAgent CreateObject(NullHandler oReader)
        {
            HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
            oHumanInteractionAgent = MapObject(oReader);
            return oHumanInteractionAgent;
        }

        private List<HumanInteractionAgent> CreateObjects(IDataReader oReader)
        {
            List<HumanInteractionAgent> oHumanInteractionAgents = new List<HumanInteractionAgent>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HumanInteractionAgent oItem = CreateObject(oHandler);
                oHumanInteractionAgents.Add(oItem);
            }
            return oHumanInteractionAgents;
        }

        #endregion

        #region Interface implementation
        public HumanInteractionAgentService() { }

        public HumanInteractionAgent Save(HumanInteractionAgent oHumanInteractionAgent, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oHumanInteractionAgent.HIAID <= 0)
                {
                    reader = HumanInteractionAgentDA.InsertUpdate(tc, oHumanInteractionAgent, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = HumanInteractionAgentDA.InsertUpdate(tc, oHumanInteractionAgent, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHumanInteractionAgent = new HumanInteractionAgent();
                    oHumanInteractionAgent = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oHumanInteractionAgent.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oHumanInteractionAgent;
        }

        public HumanInteractionAgent UpdateRead(HumanInteractionAgent oHumanInteractionAgent, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                HumanInteractionAgentDA.UpdateRead(tc, oHumanInteractionAgent, nUserID);
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
            return oHumanInteractionAgent;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
                HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
                oHumanInteractionAgent.HIAID = id;
                HumanInteractionAgentDA.Delete(tc, oHumanInteractionAgent, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public int GetHIA_NotificationCount(Int64 nUserId)
       {
            int nGetHIA_Noty = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                nGetHIA_Noty = HumanInteractionAgentDA.GetHIA_NotificationCount(tc, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to get . Because of " + e.Message, e);
                #endregion
            }
            return nGetHIA_Noty;
        }

        public HumanInteractionAgent Get(int id, Int64 nUserId)
        {
            HumanInteractionAgent oHumanInteractionAgent = new HumanInteractionAgent();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HumanInteractionAgentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHumanInteractionAgent = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oHumanInteractionAgent;
        }


        public List<HumanInteractionAgent> Gets(bool bIsAll, Int64 nUserId)
        {
            List<HumanInteractionAgent> oHumanInteractionAgents = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HumanInteractionAgentDA.GetsBy(tc, nUserId, bIsAll);
                oHumanInteractionAgents = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oHumanInteractionAgents;
        }


        #endregion
    }
}
