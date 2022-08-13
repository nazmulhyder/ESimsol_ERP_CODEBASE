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
    [Serializable]
    public class FabricExecutionOrderNoteService : MarshalByRefObject, IFabricExecutionOrderNoteService
    {
        #region Private functions and declaration
        private FabricExecutionOrderNote MapObject(NullHandler oReader)
        {
            FabricExecutionOrderNote oFEONote = new FabricExecutionOrderNote();
            oFEONote.FEONID = oReader.GetInt32("FEONID");
            oFEONote.FEOID = oReader.GetInt32("FEOID");
            oFEONote.Note = oReader.GetString("Note");
            return oFEONote;
        }

        public static FabricExecutionOrderNote CreateObject(NullHandler oReader)
        {
            FabricExecutionOrderNote oFabricExecutionOrderNote = new FabricExecutionOrderNote();
            FabricExecutionOrderNoteService oFEONoteService = new FabricExecutionOrderNoteService();
            oFabricExecutionOrderNote = oFEONoteService.MapObject(oReader);
            return oFabricExecutionOrderNote;
        }

        private List<FabricExecutionOrderNote> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrderNote> oFabricExecutionOrderNotes = new List<FabricExecutionOrderNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrderNote oItem = CreateObject(oHandler);
                oFabricExecutionOrderNotes.Add(oItem);
            }
            return oFabricExecutionOrderNotes;
        }
        #endregion

        #region Interface implementatio
        public FabricExecutionOrderNoteService() { }

        public FabricExecutionOrderNote IUD(FabricExecutionOrderNote oFEONote, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = FabricExecutionOrderNoteDA.IUD(tc, oFEONote, nDBOperation, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEONote = new FabricExecutionOrderNote();
                    oFEONote = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFEONote = new FabricExecutionOrderNote();
                    oFEONote.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEONote = new FabricExecutionOrderNote();
                oFEONote.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            return oFEONote;
        }

        public FabricExecutionOrderNote Get(int nFEODID, Int64 nUserId)
        {
            FabricExecutionOrderNote oFEONote = new FabricExecutionOrderNote();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderNoteDA.Get(tc, nFEODID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEONote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEONote = new FabricExecutionOrderNote();
                oFEONote.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFEONote;
        }
        public List<FabricExecutionOrderNote> Gets(int nFEOID, Int64 nUserId)
        {
            List<FabricExecutionOrderNote> oFEONotes = new List<FabricExecutionOrderNote>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderNoteDA.Gets(tc, nFEOID, nUserId);
                oFEONotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEONotes = new List<FabricExecutionOrderNote>();
                #endregion
            }

            return oFEONotes;
        }
        public List<FabricExecutionOrderNote> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricExecutionOrderNote> oFEONotes = new List<FabricExecutionOrderNote>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderNoteDA.Gets(tc, sSQL, nUserId);
                oFEONotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEONotes = new List<FabricExecutionOrderNote>();
                #endregion
            }

            return oFEONotes;
        }
        #endregion
    }
}