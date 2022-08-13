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
    public class ClaimReasonService : MarshalByRefObject, IClaimReasonService
    {
        #region Private functions and declaration
        private ClaimReason MapObject(NullHandler oReader)
        {
            ClaimReason oClaimReason = new ClaimReason();
            oClaimReason.ClaimReasonID = oReader.GetInt32("ClaimReasonID");
            oClaimReason.BUID = oReader.GetInt32("BUID");
            oClaimReason.OperationType = (EnumClaimOperationType)(oReader.GetInt32("OperationType"));
            oClaimReason.Note = oReader.GetString("Note");
            oClaimReason.Name = oReader.GetString("Name");
            oClaimReason.Activity = oReader.GetBoolean("Activity");
            return oClaimReason;
        }

        private ClaimReason CreateObject(NullHandler oReader)
        {
            ClaimReason oClaimReason = new ClaimReason();
            oClaimReason = MapObject(oReader);
            return oClaimReason;
        }

        private List<ClaimReason> CreateObjects(IDataReader oReader)
        {
            List<ClaimReason> oClaimReasons = new List<ClaimReason>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ClaimReason oItem = CreateObject(oHandler);
                oClaimReasons.Add(oItem);
            }
            return oClaimReasons;
        }

        #endregion

        #region Interface implementation
        public ClaimReasonService() { }


        public ClaimReason Save(ClaimReason oClaimReason, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region ClaimReason
                IDataReader reader;
                if (oClaimReason.ClaimReasonID <= 0)
                {
                    reader = ClaimReasonDA.InsertUpdate(tc, oClaimReason, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ClaimReasonDA.InsertUpdate(tc, oClaimReason, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClaimReason = new ClaimReason();
                    oClaimReason = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oClaimReason = new ClaimReason();
                oClaimReason.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oClaimReason;
        }

        public String Delete(ClaimReason oClaimReason, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ClaimReasonDA.Delete(tc, oClaimReason, EnumDBOperation.Delete, nUserID);
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
        public ClaimReason Get(int id, Int64 nUserId)
        {
            ClaimReason oClaimReason = new ClaimReason();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ClaimReasonDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClaimReason = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oClaimReason;
        }

        public List<ClaimReason> Gets(string sSQL, Int64 nUserID)
        {
            List<ClaimReason> oClaimReason = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ClaimReasonDA.Gets(sSQL, tc);
                oClaimReason = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ClaimReason", e);
                #endregion
            }
            return oClaimReason;
        }
        public ClaimReason GetByType(int nOperationType, Int64 nUserId)
        {
            ClaimReason oClaimReason = new ClaimReason();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ClaimReasonDA.GetByType(tc, nOperationType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClaimReason = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oClaimReason;
        }


        public List<ClaimReason> Gets(Int64 nUserId)
        {
            List<ClaimReason> oClaimReasons = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ClaimReasonDA.Gets(tc);
                oClaimReasons = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oClaimReasons;
        }
        public List<ClaimReason> Gets(int nBUID, Int64 nUserId)
        {
            List<ClaimReason> oClaimReasons = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ClaimReasonDA.Gets(tc, nBUID);
                oClaimReasons = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oClaimReasons;
        }

        public ClaimReason Activate(ClaimReason oClaimReason, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ClaimReasonDA.Activate(tc, oClaimReason);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oClaimReason = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oClaimReason = new ClaimReason();
                oClaimReason.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oClaimReason;
        }


        #endregion
    }
}