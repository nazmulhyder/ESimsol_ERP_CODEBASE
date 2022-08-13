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
    public class SourcingConfigHeadService : MarshalByRefObject, ISourcingConfigHeadService
    {
        #region Private functions and declaration
        private SourcingConfigHead MapObject(NullHandler oReader)
        {
            SourcingConfigHead oSourcingConfigHead = new SourcingConfigHead();
            oSourcingConfigHead.SourcingConfigHeadID = oReader.GetInt32("SourcingConfigHeadID");
            oSourcingConfigHead.SourcingConfigHeadType = (EnumSourcingConfigHeadType)oReader.GetInt32("SourcingConfigHeadType");
            oSourcingConfigHead.SourcingConfigHeadName = oReader.GetString("SourcingConfigHeadName");
            oSourcingConfigHead.Remarks = oReader.GetString("Remarks");

            return oSourcingConfigHead;
        }

        private SourcingConfigHead CreateObject(NullHandler oReader)
        {
            SourcingConfigHead oSourcingConfigHead = new SourcingConfigHead();
            oSourcingConfigHead = MapObject(oReader);
            return oSourcingConfigHead;
        }

        private List<SourcingConfigHead> CreateObjects(IDataReader oReader)
        {
            List<SourcingConfigHead> oSourcingConfigHead = new List<SourcingConfigHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SourcingConfigHead oItem = CreateObject(oHandler);
                oSourcingConfigHead.Add(oItem);
            }
            return oSourcingConfigHead;
        }

        #endregion

        #region Interface implementation
        public SourcingConfigHeadService() { }

        public SourcingConfigHead Save(SourcingConfigHead oSourcingConfigHead, Int64 nUserID)
        {
            TransactionContext tc = null;
            oSourcingConfigHead.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSourcingConfigHead.SourcingConfigHeadID <= 0)
                {
                    reader = SourcingConfigHeadDA.InsertUpdate(tc, oSourcingConfigHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SourcingConfigHeadDA.InsertUpdate(tc, oSourcingConfigHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSourcingConfigHead = new SourcingConfigHead();
                    oSourcingConfigHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSourcingConfigHead.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save SourcingConfigHead. Because of " + e.Message, e);
                #endregion
            }
            return oSourcingConfigHead;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SourcingConfigHead oSourcingConfigHead = new SourcingConfigHead();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SourcingConfigHead, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "SourcingConfigHead", id);
                oSourcingConfigHead.SourcingConfigHeadID = id;
                SourcingConfigHeadDA.Delete(tc, oSourcingConfigHead, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SourcingConfigHead. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public SourcingConfigHead Get(int id, Int64 nUserId)
        {
            SourcingConfigHead oAccountHead = new SourcingConfigHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SourcingConfigHeadDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SourcingConfigHead", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<SourcingConfigHead> Gets(Int64 nUserID)
        {
            List<SourcingConfigHead> oSourcingConfigHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SourcingConfigHeadDA.Gets(tc);
                oSourcingConfigHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SourcingConfigHead", e);
                #endregion
            }

            return oSourcingConfigHead;
        }

        public List<SourcingConfigHead> Gets(string sSQL, Int64 nUserID)
        {
            List<SourcingConfigHead> oSourcingConfigHead = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SourcingConfigHeadDA.Gets(tc, sSQL);
                oSourcingConfigHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SourcingConfigHead", e);
                #endregion
            }

            return oSourcingConfigHead;
        }

        #endregion
    }
}
