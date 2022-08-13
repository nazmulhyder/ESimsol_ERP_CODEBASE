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

    public class FabricSalesContractNoteService : MarshalByRefObject, IFabricSalesContractNoteService
    {
        #region Private functions and declaration
        private FabricSalesContractNote MapObject(NullHandler oReader)
        {
            FabricSalesContractNote oFabricSalesContractNote = new FabricSalesContractNote();
            oFabricSalesContractNote.FabricSalesContractNoteID = oReader.GetInt32("FabricSalesContractNoteID");
            oFabricSalesContractNote.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFabricSalesContractNote.Note = oReader.GetString("Note");
            oFabricSalesContractNote.Sequence = oReader.GetInt32("Sequence");          
            return oFabricSalesContractNote;
        }

        private FabricSalesContractNote CreateObject(NullHandler oReader)
        {
            FabricSalesContractNote oFabricSalesContractNote = new FabricSalesContractNote();
            oFabricSalesContractNote = MapObject(oReader);
            return oFabricSalesContractNote;
        }

        private List<FabricSalesContractNote> CreateObjects(IDataReader oReader)
        {
            List<FabricSalesContractNote> oFabricSalesContractNote = new List<FabricSalesContractNote>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricSalesContractNote oItem = CreateObject(oHandler);
                oFabricSalesContractNote.Add(oItem);
            }
            return oFabricSalesContractNote;
        }

        #endregion

        #region Interface implementation
        public FabricSalesContractNoteService() { }

        public FabricSalesContractNote Save(FabricSalesContractNote oFabricSalesContractNote, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricSalesContractNote.FabricSalesContractNoteID <= 0)
                {
                    reader = FabricSalesContractNoteDA.InsertUpdate(tc, oFabricSalesContractNote, EnumDBOperation.Insert, nUserId, "");
                }
                else
                {
                    reader = FabricSalesContractNoteDA.InsertUpdate(tc, oFabricSalesContractNote, EnumDBOperation.Update, nUserId, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricSalesContractNote = new FabricSalesContractNote();
                    oFabricSalesContractNote = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContractNote.ErrorMessage = e.Message;
                #endregion
            }
            return oFabricSalesContractNote;
        }

        public string Delete(FabricSalesContractNote oFabricSalesContractNote, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                FabricSalesContractNoteDA.Delete(tc, oFabricSalesContractNote, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public string DeleteAll(FabricSalesContractNote oFabricSalesContractNote, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                FabricSalesContractNoteDA.DeleteAll(tc, oFabricSalesContractNote.FabricSalesContractID, oFabricSalesContractNote.ErrorMessage);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public string SaveAll(List<FabricSalesContractNote> oFabricSalesContractNotes, Int64 nUserId)
        {
            TransactionContext tc = null;

            List<FabricSalesContractNote> _oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            FabricSalesContractNote oFabricSalesContractNote = new FabricSalesContractNote();
            oFabricSalesContractNote.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricSalesContractNote oItem in oFabricSalesContractNotes)
                {
                    IDataReader reader;
                    if (oItem.FabricSalesContractNoteID <= 0)
                    {
                        reader = FabricSalesContractNoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                    }
                    else
                    {
                        reader = FabricSalesContractNoteDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricSalesContractNote = new FabricSalesContractNote();
                        oFabricSalesContractNote = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricSalesContractNote.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PITandCClause. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public FabricSalesContractNote Get(int id, Int64 nUserId)
        {
            FabricSalesContractNote oAccountHead = new FabricSalesContractNote();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricSalesContractNoteDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<FabricSalesContractNote> Gets(int nFSCID, Int64 nUserID)
        {
            List<FabricSalesContractNote> oFabricSalesContractNote = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractNoteDA.Gets(tc, nFSCID);
                oFabricSalesContractNote = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oFabricSalesContractNote;
        }
        public List<FabricSalesContractNote> GetsLog(int nFSCID, Int64 nUserID)
        {
            List<FabricSalesContractNote> oFabricSalesContractNote = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractNoteDA.GetsLog(tc, nFSCID);
                oFabricSalesContractNote = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oFabricSalesContractNote;
        }

    
        public List<FabricSalesContractNote> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricSalesContractNote> oFabricSalesContractNote = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricSalesContractNoteDA.Gets(tc, sSQL);
                oFabricSalesContractNote = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oFabricSalesContractNote;
        }
        #endregion
    }

}
