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
    public class SalaryHeadService : MarshalByRefObject, ISalaryHeadService
    {
        #region Private functions and declaration
        private SalaryHead MapObject(NullHandler oReader)
        {
            SalaryHead oSalaryHead = new SalaryHead();
            oSalaryHead.SalaryHeadID = oReader.GetInt32("SalaryHeadID");
            oSalaryHead.Sequence = oReader.GetInt32("Sequence");
            oSalaryHead.Name = oReader.GetString("Name");
            oSalaryHead.NameInBangla = oReader.GetString("NameInBangla");
            oSalaryHead.Description = oReader.GetString("Description");
            oSalaryHead.SalaryHeadType = (EnumSalaryHeadType)oReader.GetInt16("SalaryHeadType");
            oSalaryHead.IsActive = oReader.GetBoolean("IsActive");
            oSalaryHead.IsProcessDependent = oReader.GetBoolean("IsProcessDependent");
            
            return oSalaryHead;
        }

        private SalaryHead CreateObject(NullHandler oReader)
        {
            SalaryHead oSalaryHead = MapObject(oReader);
            return oSalaryHead;
        }

        private List<SalaryHead> CreateObjects(IDataReader oReader)
        {
            List<SalaryHead> oSalaryHead = new List<SalaryHead>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalaryHead oItem = CreateObject(oHandler);
                oSalaryHead.Add(oItem);
            }
            return oSalaryHead;
        }

        #endregion

        #region Interface implementation
        public SalaryHeadService() { }

        public SalaryHead Save(SalaryHead oSalaryHead, Int64 nUserID)
       {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSalaryHead.SalaryHeadID <= 0)
                {
                    reader = SalaryHeadDA.InsertUpdate(tc, oSalaryHead, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = SalaryHeadDA.InsertUpdate(tc, oSalaryHead, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryHead = new SalaryHead();
                    oSalaryHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save SalaryHead. Because of " + e.Message, e);
                #endregion
            }
            return oSalaryHead;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalaryHead oSalaryHead = new SalaryHead();
                oSalaryHead.SalaryHeadID = id;
                SalaryHeadDA.Delete(tc, oSalaryHead, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SalaryHead. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public SalaryHead Get(int id, Int64 nUserId)
        {
            SalaryHead aSalaryHead = new SalaryHead();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SalaryHeadDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aSalaryHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SalaryHead", e);
                #endregion
            }

            return aSalaryHead;
        }

        public List<SalaryHead> Gets(Int64 nUserID)
        {
            List<SalaryHead> oSalaryHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalaryHeadDA.Gets(tc);
                oSalaryHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalaryHead", e);
                #endregion
            }

            return oSalaryHead;
        }

        public List<SalaryHead> Gets(string sSQL, Int64 nUserID)
        {
            List<SalaryHead> oSalaryHead = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SalaryHeadDA.Gets(tc, sSQL);
                oSalaryHead = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalaryHead", e);
                #endregion
            }
            return oSalaryHead;
        }
        public SalaryHead UpDown(SalaryHead oSalaryHead, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = SalaryHeadDA.UpDown(tc, oSalaryHead, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSalaryHead = new SalaryHead();
                    oSalaryHead = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oSalaryHead = new SalaryHead();
                oSalaryHead.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oSalaryHead;
        }
        public string ChangeActiveStatus(SalaryHead oSalaryHead, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SalaryHeadDA.ChangeActiveStatus(tc, oSalaryHead);
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
            return "Update sucessfully";
        }
        #endregion
    }
}
