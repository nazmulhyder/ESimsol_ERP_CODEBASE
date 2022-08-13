using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{

    public class KnittingService : MarshalByRefObject, IKnittingService
    {
        #region Private functions and declaration
        private Knitting MapObject(NullHandler oReader)
        {
            Knitting oKnitting = new Knitting();
            oKnitting.KnittingID = oReader.GetInt32("KnittingID");
            oKnitting.Name = oReader.GetString("Name");
            oKnitting.Note = oReader.GetString("Note");
            return oKnitting;
        }

        private Knitting CreateObject(NullHandler oReader)
        {
            Knitting oKnitting = new Knitting();
            oKnitting = MapObject(oReader);
            return oKnitting;
        }

        private List<Knitting> CreateObjects(IDataReader oReader)
        {
            List<Knitting> oKnitting = new List<Knitting>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Knitting oItem = CreateObject(oHandler);
                oKnitting.Add(oItem);
            }
            return oKnitting;
        }

        #endregion

        #region Interface implementation
        public KnittingService() { }

        public Knitting Save(Knitting oKnitting, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnitting.KnittingID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Knitting, EnumRoleOperationType.Add);
                    reader = KnittingDA.InsertUpdate(tc, oKnitting, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Knitting, EnumRoleOperationType.Edit);
                    reader = KnittingDA.InsertUpdate(tc, oKnitting, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitting = new Knitting();
                    oKnitting = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oKnitting = new Knitting();
                oKnitting.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitting;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Knitting oKnitting = new Knitting();
                oKnitting.KnittingID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Knitting, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Knitting", id);
                KnittingDA.Delete(tc, oKnitting, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Knitting. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public Knitting Get(int id, Int64 nUserId)
        {
            Knitting oAccountHead = new Knitting();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = KnittingDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Knitting", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<Knitting> Gets(Int64 nUserID)
        {
            List<Knitting> oKnitting = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = KnittingDA.Gets(tc);
                oKnitting = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Knitting", e);
                #endregion
            }

            return oKnitting;
        }

        public List<Knitting> Gets(string sSQL, Int64 nUserID)
        {
            List<Knitting> oKnitting = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = KnittingDA.Gets(tc, sSQL);
                oKnitting = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Knitting", e);
                #endregion
            }

            return oKnitting;
        }

        #endregion
    }   

}
