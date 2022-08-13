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
    public class RackService : MarshalByRefObject, IRackService
    {
        #region Private functions and declaration
        private Rack MapObject(NullHandler oReader)
        {
            Rack oRack = new Rack();
            oRack.RackID = oReader.GetInt32("RackID");
            oRack.RackNo = oReader.GetString("RackNo");
            oRack.ShelfID = oReader.GetInt32("ShelfID");
            oRack.Remarks = oReader.GetString("Remarks");
            oRack.ShelfNo = oReader.GetString("ShelfNo");
            oRack.ShelfName = oReader.GetString("ShelfName");
            return oRack;
        }

        private Rack CreateObject(NullHandler oReader)
        {
            Rack oRack = new Rack();
            oRack = MapObject(oReader);
            return oRack;
        }

        private List<Rack> CreateObjects(IDataReader oReader)
        {
            List<Rack> oRack = new List<Rack>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Rack oItem = CreateObject(oHandler);
                oRack.Add(oItem);
            }
            return oRack;
        }

        #endregion

        #region Interface implementation
        public RackService() { }

        public Rack Save(Rack oRack, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oRack.RackID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shelf, EnumRoleOperationType.Add);
                    reader = RackDA.InsertUpdate(tc, oRack, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Shelf, EnumRoleOperationType.Edit);
                    reader = RackDA.InsertUpdate(tc, oRack, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRack = new Rack();
                    oRack = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRack = new Rack();
                oRack.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Rack. Because of " + e.Message, e);
                #endregion
            }
            return oRack;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Rack oRack = new Rack();
                oRack.RackID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Shelf, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Rack", id);
                RackDA.Delete(tc, oRack, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Rack. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Rack Get(int id, int nUserId)
        {
            Rack oAccountHead = new Rack();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RackDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Rack", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<Rack> Gets(int nUserID)
        {
            List<Rack> oRack = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RackDA.Gets(tc);
                oRack = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Rack", e);
                #endregion
            }

            return oRack;
        }

        public List<Rack> Gets(int nShelfID, int nUserID)
        {
            List<Rack> oRack = new List<Rack>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RackDA.Gets(tc, nShelfID);
                oRack = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Rack", e);
                #endregion
            }

            return oRack;
        }
        //
        public List<Rack> BUWiseGets(int nBUID, int nUserID)
        {
            List<Rack> oRack = new List<Rack>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RackDA.BUWiseGets(tc, nBUID);
                oRack = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Rack", e);
                #endregion
            }

            return oRack;
        }
        public List<Rack> Gets(string sSQL,int nUserID)
        {
            List<Rack> oRack = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                    sSQL = "Select * from View_Rack where RackID in (1,2,80,272,347,370,60,45)";
                    }
                reader = RackDA.Gets(tc, sSQL);
                oRack = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Rack", e);
                #endregion
            }

            return oRack;
        }

     
        #endregion
    }   
}