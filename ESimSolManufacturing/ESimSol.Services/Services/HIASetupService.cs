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
 
using System.Net.Mail;

namespace ESimSol.Services.Services
{
    public class HIASetupService : MarshalByRefObject, IHIASetupService
    {
        #region Private functions and declaration
        private HIASetup MapObject(NullHandler oReader)
        {
            HIASetup oHIASetup = new HIASetup();
            oHIASetup.HIASetupID = oReader.GetInt32("HIASetupID");
            oHIASetup.HIASetupType = (EnumHIASetupType)oReader.GetInt32("HIASetupType");
            oHIASetup.HIASetupTypeInInt = oReader.GetInt32("HIASetupType");
            oHIASetup.SetupName = oReader.GetString("SetupName");
            oHIASetup.DBTable = oReader.GetString("DBTable");
            oHIASetup.KeyColumn = oReader.GetString("KeyColumn");
            oHIASetup.FileNumberColumn = oReader.GetString("FileNumberColumn");
            oHIASetup.SenderColumn = oReader.GetString("SenderColumn");
            oHIASetup.ReceiverColumn = oReader.GetString("ReceiverColumn");
            oHIASetup.WhereClause = oReader.GetString("WhereClause");
            oHIASetup.MessageBodyText = oReader.GetString("MessageBodyText");
            oHIASetup.Activity = oReader.GetBoolean("Activity");
            oHIASetup.LinkReference = oReader.GetString("LinkReference");
            oHIASetup.Parameter = oReader.GetString("Parameter");
            oHIASetup.OrderStepID = oReader.GetInt32("OrderStepID");
            oHIASetup.TimeEventType = (EnumTimeEventType)oReader.GetInt32("TimeEventType");
            oHIASetup.TimeEventTypeInInt = oReader.GetInt32("TimeEventType");
            oHIASetup.BUID = oReader.GetInt32("BUID");
            oHIASetup.TimeCounter = oReader.GetInt32("TimeCounter");
            oHIASetup.OperationName = oReader.GetString("OperationName");
            oHIASetup.OperationValue = oReader.GetString("OperationValue");
            return oHIASetup;
        }

        private HIASetup CreateObject(NullHandler oReader)
        {
            HIASetup oHIASetup = new HIASetup();
            oHIASetup = MapObject(oReader);
            return oHIASetup;
        }

        private List<HIASetup> CreateObjects(IDataReader oReader)
        {
            List<HIASetup> oHIASetups = new List<HIASetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HIASetup oItem = CreateObject(oHandler);
                oHIASetups.Add(oItem);
            }
            return oHIASetups;
        }

        #endregion

        #region Interface implementation
        public HIASetupService() { }


        public HIASetup Save(HIASetup oHIASetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<HIAUserAssign> oHIAUserAssigns = new List<HIAUserAssign>();
                HIAUserAssign oHIAUserAssign = new HIAUserAssign();
                oHIAUserAssigns = oHIASetup.HIAUserAssigns;
                string sHIAUserAssignsIDs = "";
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oHIASetup.HIASetupID <= 0)
                {
                    reader = HIASetupDA.InsertUpdate(tc, oHIASetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = HIASetupDA.InsertUpdate(tc, oHIASetup, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHIASetup = new HIASetup();
                    oHIASetup = CreateObject(oReader);
                }
                reader.Close();

                #region HIA User Assign  
                if (oHIAUserAssigns != null)
                {
                    foreach (HIAUserAssign oItem in oHIAUserAssigns)
                    {
                        IDataReader readerdetail;
                        oItem.HIASetupID = oHIASetup.HIASetupID;
                        if (oItem.HIAUserAssignID <= 0)
                        {
                            readerdetail = HIAUserAssignDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = HIAUserAssignDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sHIAUserAssignsIDs = sHIAUserAssignsIDs + oReaderDetail.GetString("HIAUserAssignID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sHIAUserAssignsIDs.Length > 0)
                    {
                        sHIAUserAssignsIDs = sHIAUserAssignsIDs.Remove(sHIAUserAssignsIDs.Length - 1, 1);
                    }
                    oHIAUserAssign = new HIAUserAssign();
                    oHIAUserAssign.HIASetupID = oHIASetup.HIASetupID;
                    HIAUserAssignDA.Delete(tc, oHIAUserAssign, EnumDBOperation.Delete, nUserID, sHIAUserAssignsIDs);
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oHIASetup.ErrorMessage = Message;
                #endregion
            }
            return oHIASetup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                HIASetup oHIASetup = new HIASetup();
                oHIASetup.HIASetupID = id;
                HIASetupDA.Delete(tc, oHIASetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return "Delete sucessfully";
        }

        public HIASetup Get(int id, Int64 nUserId)
        {
            HIASetup oHIASetup = new HIASetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HIASetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHIASetup = CreateObject(oReader);
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

            return oHIASetup;
        }

        public List<HIASetup> Gets(int buid, Int64 nUserId)
        {
            List<HIASetup> oHIASetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HIASetupDA.GetsBy(tc, nUserId, buid);
                oHIASetups = CreateObjects(reader);
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

            return oHIASetups;
        }

        public List<HIASetup> GetsByOrderStep(int id, Int64 nUserID)
        {
            List<HIASetup> oHIASetups = new List<HIASetup>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HIASetupDA.GetsByOrderStep(tc, id, nUserID);
                oHIASetups = CreateObjects(reader);
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

            return oHIASetups;
        }

        public List<HIASetup> GetsByOrderStepBUWise(int id, int buid, Int64 nUserID)
        {
            List<HIASetup> oHIASetups = new List<HIASetup>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HIASetupDA.GetsByOrderStepBUWise(tc, id, buid);
                oHIASetups = CreateObjects(reader);
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

            return oHIASetups;
        }
        #endregion
    }
}
