using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class FDONoteService : MarshalByRefObject, IFDONoteService
    {
        #region Private functions and declaration
        private FDONote MapObject(NullHandler oReader)
        {
            FDONote oFDONote = new FDONote();
            oFDONote.FDONoteID = oReader.GetInt32("FDONoteID");
            oFDONote.FDOID = oReader.GetInt32("FDOID");
            oFDONote.Note = oReader.GetString("Note");
            return oFDONote;
        }
        private FDONote CreateObject(NullHandler oReader)
        {
            FDONote oFDONote = new FDONote();
            oFDONote = MapObject(oReader);
            return oFDONote;
        }
        private List<FDONote> CreateObjects(IDataReader oReader)
        {
            List<FDONote> oFDONote = new List<FDONote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FDONote oItem = CreateObject(oHandler);
                oFDONote.Add(oItem);
            }
            return oFDONote;
        }

        #endregion

        #region Interface implementation
        public FDONote Save(FDONote oFDONote, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFDONote.FDONoteID <= 0)
                {
                    reader = FDONoteDA.InsertUpdate(tc, oFDONote, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FDONoteDA.InsertUpdate(tc, oFDONote, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDONote = new FDONote();
                    oFDONote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDONote.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFDONote;
        }
        public string SaveAll(FDONote oFDONote, Int64 nUserId)
        {
            TransactionContext tc = null;

            List<FDONote> oFDONotes = new List<FDONote>();
            oFDONotes = oFDONote.FDONotes;
          
            try
            {
                tc = TransactionContext.Begin(true);
                if (oFDONotes != null)
                {
                    foreach (FDONote oItem in oFDONotes)
                    {
                        IDataReader reader;
                        if (oItem.FDONoteID <= 0)
                        {
                            reader = FDONoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            reader = FDONoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFDONote = new FDONote();
                            oFDONote = CreateObject(oReader);
                        }
                        reader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PITandCClause. Because of " + e.Message, e);
                #endregion
            }
            return "";
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FDONote oFDONote = new FDONote();
                oFDONote.FDONoteID = id;
                FDONoteDA.Delete(tc, oFDONote, EnumDBOperation.Delete, nUserId);
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
        public List<FDONote> Gets(string sSQL, int nUserID)
        {
            List<FDONote> oFDONotes = new List<FDONote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDONoteDA.Gets(tc, sSQL);
                oFDONotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDONotes = new List<FDONote>();
                FDONote oFDONote = new FDONote();
                oFDONote.ErrorMessage = e.Message.Split('~')[0];
                oFDONotes.Add(oFDONote);
                #endregion
            }
            return oFDONotes;
        }
        public List<FDONote> GetByOrderID(int nOrderID, int nUserID)
        {
            List<FDONote> oFDONotes = new List<FDONote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDONoteDA.GetByOrderID(tc, nOrderID);
                oFDONotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFDONotes = new List<FDONote>();
                FDONote oFDONote = new FDONote();
                oFDONote.ErrorMessage = e.Message.Split('~')[0];
                oFDONotes.Add(oFDONote);
                #endregion
            }
            return oFDONotes;
        }
        #endregion
    }
}
