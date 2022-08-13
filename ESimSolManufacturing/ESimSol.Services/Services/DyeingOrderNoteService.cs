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
    public class DyeingOrderNoteService : MarshalByRefObject, IDyeingOrderNoteService
    {
        #region Private functions and declaration
        private DyeingOrderNote MapObject(NullHandler oReader)
        {
            DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
            oDyeingOrderNote.DyeingOrderNoteID = oReader.GetInt32("DyeingOrderNoteID");
            oDyeingOrderNote.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDyeingOrderNote.OrderNote = oReader.GetString("OrderNote");
            return oDyeingOrderNote;
        }
        private DyeingOrderNote CreateObject(NullHandler oReader)
        {
            DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
            oDyeingOrderNote = MapObject(oReader);
            return oDyeingOrderNote;
        }
        private List<DyeingOrderNote> CreateObjects(IDataReader oReader)
        {
            List<DyeingOrderNote> oDyeingOrderNote = new List<DyeingOrderNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingOrderNote oItem = CreateObject(oHandler);
                oDyeingOrderNote.Add(oItem);
            }
            return oDyeingOrderNote;
        }

        #endregion

        #region Interface implementation
        public DyeingOrderNote Save(DyeingOrderNote oDyeingOrderNote, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDyeingOrderNote.DyeingOrderNoteID <= 0)
                {
                    reader = DyeingOrderNoteDA.InsertUpdate(tc, oDyeingOrderNote, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DyeingOrderNoteDA.InsertUpdate(tc, oDyeingOrderNote, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrderNote = new DyeingOrderNote();
                    oDyeingOrderNote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingOrderNote.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrderNote;
        }
        public string SaveAll(DyeingOrderNote oDyeingOrderNote, Int64 nUserId)
        {
            TransactionContext tc = null;

            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            oDyeingOrderNotes = oDyeingOrderNote.DyeingOrderNotes;
          
            try
            {
                tc = TransactionContext.Begin(true);
                DyeingOrderDA.UpdateDeliveryNote(tc, oDyeingOrderNote.DyeingOrder.DeliveryNote,  oDyeingOrderNote.DyeingOrder.DyeingOrderID);
                if (oDyeingOrderNotes != null)
                {
                    foreach (DyeingOrderNote oItem in oDyeingOrderNotes)
                    {
                        IDataReader reader;
                        if (oItem.DyeingOrderNoteID <= 0)
                        {
                            reader = DyeingOrderNoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            reader = DyeingOrderNoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oDyeingOrderNote = new DyeingOrderNote();
                            oDyeingOrderNote = CreateObject(oReader);
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
                DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.DyeingOrderNoteID = id;
                DyeingOrderNoteDA.Delete(tc, oDyeingOrderNote, EnumDBOperation.Delete, nUserId);
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
        public List<DyeingOrderNote> Gets(int nUserID)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderNoteDA.Gets(tc);
                oDyeingOrderNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingOrderNotes = new List<DyeingOrderNote>();
                DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = e.Message.Split('~')[0];
                oDyeingOrderNotes.Add(oDyeingOrderNote);
                #endregion
            }
            return oDyeingOrderNotes;
        }

        public List<DyeingOrderNote> Gets(string sSQL, int nUserID)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderNoteDA.Gets(tc, sSQL);
                oDyeingOrderNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingOrderNotes = new List<DyeingOrderNote>();
                DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = e.Message.Split('~')[0];
                oDyeingOrderNotes.Add(oDyeingOrderNote);
                #endregion
            }
            return oDyeingOrderNotes;
        }
        public DyeingOrderNote Get(int id, int nUserId)
        {
            DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DyeingOrderNoteDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDyeingOrderNote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingOrderNote.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDyeingOrderNote;
        }
        public List<DyeingOrderNote> GetByOrderID(int nOrderID, int nUserID)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderNoteDA.GetByOrderID(tc, nOrderID);
                oDyeingOrderNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingOrderNotes = new List<DyeingOrderNote>();
                DyeingOrderNote oDyeingOrderNote = new DyeingOrderNote();
                oDyeingOrderNote.ErrorMessage = e.Message.Split('~')[0];
                oDyeingOrderNotes.Add(oDyeingOrderNote);
                #endregion
            }
            return oDyeingOrderNotes;
        }
        public List<DyeingOrderNote> GetByConID(DyeingOrderNote oDyeingOrderNote, int nUserID)
        {
            List<DyeingOrderNote> oDyeingOrderNotes = new List<DyeingOrderNote>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderNoteDA.GetByConID(tc, oDyeingOrderNote);
                oDyeingOrderNotes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDyeingOrderNotes = new List<DyeingOrderNote>();
                #endregion
            }
            return oDyeingOrderNotes;
        }
       

        #endregion
    }
}
