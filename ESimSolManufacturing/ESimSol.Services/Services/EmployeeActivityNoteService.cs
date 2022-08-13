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
    public class EmployeeActivityNoteService : MarshalByRefObject, ESimSol.BusinessObjects.EmployeeActivityNote.IEmployeeActivityNoteService
    {
        #region Private functions and declaration
        private EmployeeActivityNote MapObject(NullHandler oReader)
        {
            EmployeeActivityNote oEmployeeActivityNote = new EmployeeActivityNote();


            oEmployeeActivityNote.EANID = oReader.GetInt32("EANID");
            oEmployeeActivityNote.EACID = oReader.GetInt32("EACID");
            oEmployeeActivityNote.DRPID = oReader.GetInt32("DRPID");
            oEmployeeActivityNote.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeActivityNote.Note = oReader.GetString("Note");
            oEmployeeActivityNote.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeActivityNote.ApproveByDate = oReader.GetDateTime("ApproveByDate");

            oEmployeeActivityNote.Code = oReader.GetString("Code");
            oEmployeeActivityNote.Name = oReader.GetString("Name");
            oEmployeeActivityNote.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeActivityNote.DesignationName = oReader.GetString("DesignationName");
            oEmployeeActivityNote.ActivityCategory = oReader.GetString("ActivityCategory");
            oEmployeeActivityNote.ActivityDate = oReader.GetDateTime("ActivityDate");
            oEmployeeActivityNote.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeActivityNote.LocationName = oReader.GetString("LocationName");

            return oEmployeeActivityNote;
        }
        private EmployeeActivityNote CreateObject(NullHandler oReader)
        {
            EmployeeActivityNote oEmployeeActivityNote = new EmployeeActivityNote();
            oEmployeeActivityNote  = MapObject(oReader);
            return oEmployeeActivityNote;
        }
        private List<EmployeeActivityNote> CreateObjects(IDataReader oReader)
        {
            List<EmployeeActivityNote> oEmployeeActivityNote = new List<EmployeeActivityNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeActivityNote oItem = CreateObject(oHandler);
                oEmployeeActivityNote.Add(oItem);
            }
            return oEmployeeActivityNote;
        }
        #endregion

        #region Interface implementation
        public List<EmployeeActivityNote> Gets(Int64 nUserID)
        {
            List<EmployeeActivityNote> oEmployeeActivityNote = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeActivityNoteDA.Gets(tc);
                oEmployeeActivityNote = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Error", e);
                #endregion
            }

            return oEmployeeActivityNote;
        }


        public List<EmployeeActivityNote> Gets(string sSQL, Int64 nUserId)
        {
            List<EmployeeActivityNote> oEmployeeActivityNote = new List<EmployeeActivityNote>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeActivityNoteDA.Gets(sSQL, tc);
                oEmployeeActivityNote = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Note", e);
                #endregion
            }

            return oEmployeeActivityNote;
        }



        public EmployeeActivityNote Get(int id, Int64 nUserId)
        {
            EmployeeActivityNote oEmployeeActivityNote = new EmployeeActivityNote();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeActivityNoteDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeActivityNote = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Note", e);
                #endregion
            }

            return oEmployeeActivityNote;
        }
        public EmployeeActivityNote Save(EmployeeActivityNote oEmployeeActivityNote, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeActivityNote.EANID <= 0)
                {
                    reader = EmployeeActivityNoteDA.InsertUpdate(tc, oEmployeeActivityNote, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = EmployeeActivityNoteDA.InsertUpdate(tc, oEmployeeActivityNote, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeActivityNote = new EmployeeActivityNote();
                    oEmployeeActivityNote = CreateObject(oReader);
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
                throw new ServiceException("Failed to save note " + e.Message, e);
                #endregion
            }
            return oEmployeeActivityNote;
        }


        public string Delete(int id, Int64 nUserId)
        {
            string message = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeActivityNote oEmployeeActivityNote = new EmployeeActivityNote();
                oEmployeeActivityNote.EANID = id;
                EmployeeActivityNoteDA.Delete(tc, oEmployeeActivityNote, EnumDBOperation.Delete, nUserId);
                tc.End();
                message = Global.DeleteMessage;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                message = e.Message;
                #endregion
            }
            return message;
        }

        public EmployeeActivityNote Approve(EmployeeActivityNote oEmployeeActivityNote, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeActivityNote.ApproveBy <= 0)
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = EmployeeActivityNoteDA.InsertUpdate(tc, oEmployeeActivityNote, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = EmployeeActivityNoteDA.InsertUpdate(tc, oEmployeeActivityNote, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeActivityNote = new EmployeeActivityNote();
                    oEmployeeActivityNote = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oEmployeeActivityNote = new EmployeeActivityNote();
                oEmployeeActivityNote.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oEmployeeActivityNote;
        }


        #endregion
    }
}
