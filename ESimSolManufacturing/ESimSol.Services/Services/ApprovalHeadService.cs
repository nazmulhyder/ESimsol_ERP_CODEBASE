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
    public class ApprovalHeadService : MarshalByRefObject, IApprovalHeadService
    {
        #region Private functions and declaration
        private ApprovalHead MapObject(NullHandler oReader)
        {
            ApprovalHead oApprovalHead = new ApprovalHead();
            oApprovalHead.ApprovalHeadID = oReader.GetInt32("ApprovalHeadID");
            oApprovalHead.ModuleID = (EnumModuleName)oReader.GetInt32("ModuleID");
            oApprovalHead.Name = oReader.GetString("Name");
            oApprovalHead.RefColName = oReader.GetString("RefColName");
            oApprovalHead.Sequence = oReader.GetInt32("Sequence");
            oApprovalHead.BUID = oReader.GetInt32("BUID");
            return oApprovalHead;

        }

        private ApprovalHead CreateObject(NullHandler oReader)
        {
            ApprovalHead oApprovalHead = MapObject(oReader);
            return oApprovalHead;
        }

        private List<ApprovalHead> CreateObjects(IDataReader oReader)
        {
            List<ApprovalHead> oApprovalHead = new List<ApprovalHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ApprovalHead oItem = CreateObject(oHandler);
                oApprovalHead.Add(oItem);
            }
            return oApprovalHead;
        }


        #endregion

        #region Interface implementation
        public ApprovalHeadService() { }
        public ApprovalHead IUD(ApprovalHead oApprovalHead, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = ApprovalHeadDA.IUD(tc, oApprovalHead, nUserID,nDBOperation );
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oApprovalHead = new ApprovalHead();
                        oApprovalHead = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = ApprovalHeadDA.IUD(tc, oApprovalHead, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oApprovalHead.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oApprovalHead = new ApprovalHead();
                oApprovalHead.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oApprovalHead;
        }



        public ApprovalHead UpDown(ApprovalHead oApprovalHead, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = ApprovalHeadDA.UpDown(tc, oApprovalHead, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oApprovalHead = new ApprovalHead();
                    oApprovalHead = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oApprovalHead = new ApprovalHead();
                oApprovalHead.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oApprovalHead;
        }


        public ApprovalHead Get(string sSQL, Int64 nUserId)
        {
            ApprovalHead oApprovalHead = new ApprovalHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ApprovalHeadDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oApprovalHead = CreateObject(oReader);
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

            return oApprovalHead;
        }

        public List<ApprovalHead> Gets(string sSQL, Int64 nUserID)
        {
            List<ApprovalHead> oApprovalHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ApprovalHeadDA.Gets(sSQL, tc);
                oApprovalHead = CreateObjects(reader);
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
            return oApprovalHead;
        }
        #endregion
    }
}

