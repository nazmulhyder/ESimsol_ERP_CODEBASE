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
    public class ShelfService : MarshalByRefObject, IShelfService
    {
        #region Private functions and declaration
        private Shelf MapObject(NullHandler oReader)
        {
            Shelf oShelf = new Shelf();
            oShelf.ShelfID = oReader.GetInt32("ShelfID");
            oShelf.ShelfNo = oReader.GetString("ShelfNo");
            oShelf.ShelfName = oReader.GetString("ShelfName");
            oShelf.BUID = oReader.GetInt32("BUID");
            oShelf.Remarks = oReader.GetString("Remarks");
            return oShelf;
        }

        private Shelf CreateObject(NullHandler oReader)
        {
            Shelf oShelf = new Shelf();
            oShelf = MapObject(oReader);
            return oShelf;
        }

        private List<Shelf> CreateObjects(IDataReader oReader)
        {
            List<Shelf> oShelf = new List<Shelf>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Shelf oItem = CreateObject(oHandler);
                oShelf.Add(oItem);
            }
            return oShelf;
        }

        #endregion

        #region Interface implementation
        public ShelfService() { }

        public Shelf Save(Shelf oShelf, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oShelf.ShelfID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shelf, EnumRoleOperationType.Add);
                    reader = ShelfDA.InsertUpdate(tc, oShelf, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shelf, EnumRoleOperationType.Edit);
                    reader = ShelfDA.InsertUpdate(tc, oShelf, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShelf = new Shelf();
                    oShelf = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Shelf. Because of " + e.Message, e);
                #endregion
            }
            return oShelf;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Shelf oShelf = new Shelf();
                oShelf.ShelfID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Shelf, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Shelf", id);
                ShelfDA.Delete(tc, oShelf, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Shelf. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Shelf Get(int id, int nUserId)
        {
            Shelf oAccountHead = new Shelf();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShelfDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Shelf", e);
                #endregion
            }

            return oAccountHead;
        }

       

        public List<Shelf> Gets(int nUserID)
        {
            List<Shelf> oShelf = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShelfDA.Gets(tc);
                oShelf = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Shelf", e);
                #endregion
            }

            return oShelf;
        }
        public List<Shelf> Gets(string sSQL,int nUserID)
        {
            List<Shelf> oShelf = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                    sSQL = "Select * from View_Shelf where ShelfID in (1,2,80,272,347,370,60,45)";
                    }
                reader = ShelfDA.Gets(tc, sSQL);
                oShelf = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Shelf", e);
                #endregion
            }

            return oShelf;
        }

        public List<Shelf> GetsByNoOrName(Shelf oShelf, int nUserID)
        {
            List<Shelf> oShelfs = new List<Shelf>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShelfDA.GetsByNoOrName(tc, oShelf);
                oShelfs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oShelfs = new List<Shelf>();
                oShelf = new Shelf();
                oShelf.ErrorMessage = e.Message;
                oShelfs.Add(oShelf);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get Shelf", e);
                #endregion
            }

            return oShelfs;
        }
        #endregion
    }   
}