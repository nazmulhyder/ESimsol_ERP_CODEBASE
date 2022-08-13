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
    public class EmployeeGroupService : MarshalByRefObject, IEmployeeGroupService
    {
        #region Private functions and declaration
        private EmployeeGroup MapObject(NullHandler oReader)
        {
            EmployeeGroup oEmployeeGroup = new EmployeeGroup();
            oEmployeeGroup.EGID = oReader.GetInt32("EGID");
            oEmployeeGroup.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oEmployeeGroup.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeGroup.Name = oReader.GetString("Name");
            return oEmployeeGroup;
        }

        private EmployeeGroup CreateObject(NullHandler oReader)
        {
            EmployeeGroup oEmployeeGroup = MapObject(oReader);
            return oEmployeeGroup;
        }

        private List<EmployeeGroup> CreateObjects(IDataReader oReader)
        {
            List<EmployeeGroup> oEmployeeGroup = new List<EmployeeGroup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeGroup oItem = CreateObject(oHandler);
                oEmployeeGroup.Add(oItem);
            }
            return oEmployeeGroup;
        }

        #endregion

        #region Interface implementation
        public EmployeeGroupService() { }

        public EmployeeGroup Save(EmployeeGroup oEmployeeGroup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeGroup.EGID <= 0)
                {
                    reader = EmployeeGroupDA.InsertUpdate(tc, oEmployeeGroup, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = EmployeeGroupDA.InsertUpdate(tc, oEmployeeGroup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeGroup = new EmployeeGroup();
                    oEmployeeGroup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save EmployeeType. Because of " + e.Message, e);
                #endregion
            }
            return oEmployeeGroup;
        }

        public string Upload(EmployeeGroup oEmployeeGroup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                     EmployeeGroupDA.Upload(tc, oEmployeeGroup, EnumDBOperation.Upload, nUserID);
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
            return "";
        }

        public List<EmployeeGroup> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeGroup> oEmployeeGroup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeGroupDA.Gets(sSQL, tc);
                oEmployeeGroup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oEmployeeGroup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeGroup oEmployeeGroup = new EmployeeGroup();
                oEmployeeGroup.EGID = id;
                EmployeeGroupDA.Delete(tc, oEmployeeGroup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete EmployeeType. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public EmployeeGroup Get(int id, Int64 nUserId)
        {
            EmployeeGroup aEmployeeGroup = new EmployeeGroup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeGroupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aEmployeeGroup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeType", e);
                #endregion
            }

            return aEmployeeGroup;
        }

        public List<EmployeeGroup> Gets(Int64 nUserID)
        {
            List<EmployeeGroup> oEmployeeGroup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeGroupDA.Gets(tc);
                oEmployeeGroup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeType", e);
                #endregion
            }

            return oEmployeeGroup;
        }

        #endregion
    }
}

