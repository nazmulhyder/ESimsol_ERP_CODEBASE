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
    public class FabricSpecificationNoteService : MarshalByRefObject, IFabricSpecificationNoteService
    {
        #region Private functions and declaration
        private FabricSpecificationNote MapObject(NullHandler oReader)
        {
            FabricSpecificationNote oFabricSpecificationNote = new FabricSpecificationNote();
            oFabricSpecificationNote.FabricSpecificationNoteID = oReader.GetInt32("FabricSpecificationNoteID");
            oFabricSpecificationNote.FEOSID = oReader.GetInt32("FEOSID");
            oFabricSpecificationNote.Note = oReader.GetString("Note");

            return oFabricSpecificationNote;
        }

        private FabricSpecificationNote CreateObject(NullHandler oReader)
        {
            FabricSpecificationNote oFabricSpecificationNote = new FabricSpecificationNote();
            oFabricSpecificationNote = MapObject(oReader);
            return oFabricSpecificationNote;
        }

        private List<FabricSpecificationNote> CreateObjects(IDataReader oReader)
        {
            List<FabricSpecificationNote> oFabricSpecificationNote = new List<FabricSpecificationNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSpecificationNote oItem = CreateObject(oHandler);
                oFabricSpecificationNote.Add(oItem);
            }
            return oFabricSpecificationNote;
        }

        #endregion

        #region Interface implementation
        public FabricSpecificationNoteService() { }

        public FabricSpecificationNote Save(FabricSpecificationNote oFabricSpecificationNote, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricSpecificationNote.FabricSpecificationNoteID <= 0)
                {
                    reader = FabricSpecificationNoteDA.InsertUpdate(tc, oFabricSpecificationNote, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricSpecificationNoteDA.InsertUpdate(tc, oFabricSpecificationNote, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSpecificationNote = new FabricSpecificationNote();
                    oFabricSpecificationNote = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save FabricSpecificationNote. Because of " + e.Message, e);
                #endregion
            }
            return oFabricSpecificationNote;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricSpecificationNote oFabricSpecificationNote = new FabricSpecificationNote();
                oFabricSpecificationNote.FabricSpecificationNoteID = id;
                FabricSpecificationNoteDA.Delete(tc, oFabricSpecificationNote, EnumDBOperation.Delete, nUserId);
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

        public List<FabricSpecificationNote> Gets(Int64 nUserID)
        {
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSpecificationNoteDA.Gets(tc);
                oFabricSpecificationNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSpecificationNote", e);
                #endregion
            }
            return oFabricSpecificationNotes;
        }
        public List<FabricSpecificationNote> Gets(int nFEOSID, Int64 nUserID)
        {
            List<FabricSpecificationNote> oFabricSpecificationNotes = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSpecificationNoteDA.Gets(tc, nFEOSID);
                oFabricSpecificationNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricSpecificationNote", e);
                #endregion
            }
            return oFabricSpecificationNotes;
        }
        public FabricSpecificationNote Get(int nId, Int64 nUserId)
        {
            FabricSpecificationNote oFabricSpecificationNote = new FabricSpecificationNote();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricSpecificationNoteDA.Get(tc, nId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSpecificationNote = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricSpecificationNote", e);
                #endregion
            }
            return oFabricSpecificationNote;
        }

        #endregion
    }
}
