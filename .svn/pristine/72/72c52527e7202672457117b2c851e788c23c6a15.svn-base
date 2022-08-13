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
    public class FabricTransferNoteService : MarshalByRefObject, IFabricTransferNoteService
    {
        #region Private functions and declaration
        private FabricTransferNote MapObject(NullHandler oReader)
        {
            FabricTransferNote oFabricTransferNote = new FabricTransferNote();
            oFabricTransferNote.FTNID = oReader.GetInt32("FTNID");
            oFabricTransferNote.FTNNo = oReader.GetString("FTNNo");
            oFabricTransferNote.Note = oReader.GetString("Note");
            oFabricTransferNote.NoteDate = oReader.GetDateTime("NoteDate");
            oFabricTransferNote.DisburseBy = oReader.GetInt32("DisburseBy");
            oFabricTransferNote.DisburseByDate = oReader.GetDateTime("DisburseByDate");
            oFabricTransferNote.DisburseByName = oReader.GetString("DisburseByName");
            oFabricTransferNote.NoOfPackingList = oReader.GetInt32("NoOfPackingList");
            oFabricTransferNote.CountDetail = oReader.GetInt32("CountDetail");
            oFabricTransferNote.CountReceive = oReader.GetInt32("CountReceive");
            return oFabricTransferNote;
        }
        private FabricTransferNote CreateObject(NullHandler oReader)
        {
            FabricTransferNote oFabricTransferNote = new FabricTransferNote();
            oFabricTransferNote = MapObject(oReader);
            return oFabricTransferNote;
        }
        private List<FabricTransferNote> CreateObjects(IDataReader oReader)
        {
            List<FabricTransferNote> oFabricTransferNote = new List<FabricTransferNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricTransferNote oItem = CreateObject(oHandler);
                oFabricTransferNote.Add(oItem);
            }
            return oFabricTransferNote;
        }

        #endregion

        #region Interface implementation
        public FabricTransferNote Save(FabricTransferNote oFabricTransferNote, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricTransferNote.FTNID <= 0)
                {
                    reader = FabricTransferNoteDA.InsertUpdate(tc, oFabricTransferNote, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricTransferNoteDA.InsertUpdate(tc, oFabricTransferNote, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferNote = new FabricTransferNote();
                    oFabricTransferNote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferNote;
        }
        public FabricTransferNote Disburse(FabricTransferNote oFabricTransferNote, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricTransferNoteDA.Disburse(tc, oFabricTransferNote, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferNote = new FabricTransferNote();
                    oFabricTransferNote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferNote;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricTransferNote oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.FTNID = id;
                DBTableReferenceDA.HasReference(tc, "FabricTransferNote", id);
                FabricTransferNoteDA.Delete(tc, oFabricTransferNote, EnumDBOperation.Delete, nUserId);
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
            return Global.DeleteMessage;
        }
        public List<FabricTransferNote> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricTransferNote> oFabricTransferNotes = new List<FabricTransferNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferNoteDA.Gets(tc, sSQL);
                oFabricTransferNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferNotes = new List<FabricTransferNote>();
                FabricTransferNote oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.ErrorMessage = e.Message.Split('!')[0];
                oFabricTransferNotes.Add(oFabricTransferNote);
                #endregion
            }
            return oFabricTransferNotes;
        }
        public List<FabricTransferNote> Gets(Int64 nUserID)
        {
            List<FabricTransferNote> oFabricTransferNotes = new List<FabricTransferNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferNoteDA.Gets(tc);
                oFabricTransferNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferNotes = new List<FabricTransferNote>();
                FabricTransferNote oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.ErrorMessage = e.Message.Split('!')[0];
                oFabricTransferNotes.Add(oFabricTransferNote);
                #endregion
            }
            return oFabricTransferNotes;
        }
        public FabricTransferNote Get(int id, Int64 nUserId)
        {
            FabricTransferNote oFabricTransferNote = new FabricTransferNote();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricTransferNoteDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferNote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferNote;
        }
        public FabricTransferNote Receive(FabricTransferNote oFabricTransferNote, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                bool bIsFTPLD = oFabricTransferNote.IsFTPLD;
                int nFTPLDetailID = 0;
                if (bIsFTPLD)
                {
                    nFTPLDetailID = oFabricTransferNote.FTPLDetailID;
                }
                if (!string.IsNullOrEmpty(oFabricTransferNote.FTPListIDs))
                {
                    IDataReader reader;
                    reader = FabricTransferNoteDA.Receive(tc, oFabricTransferNote, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricTransferNote = new FabricTransferNote();
                        oFabricTransferNote = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else
                {
                    oFabricTransferNote = new FabricTransferNote();
                    oFabricTransferNote.ErrorMessage = "No fabric transfer packing list found.";
                }
                oFabricTransferNote.IsFTPLD = bIsFTPLD;
                if (bIsFTPLD)
                {
                    oFabricTransferNote.FTPLD.FTPLDetailID = nFTPLDetailID;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricTransferNote = new FabricTransferNote();
                oFabricTransferNote.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferNote;
        }

        #endregion
    }
}
