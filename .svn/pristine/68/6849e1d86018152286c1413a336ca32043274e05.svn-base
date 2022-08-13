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
    public class PLineConfigureService : MarshalByRefObject, IPLineConfigureService
    {
        #region Private functions and declaration
        private PLineConfigure MapObject(NullHandler oReader)
        {
            PLineConfigure oPLineConfigure = new PLineConfigure();
            oPLineConfigure.PLineConfigureID = oReader.GetInt32("PLineConfigureID");
            oPLineConfigure.ProductionUnitID = oReader.GetInt32("ProductionUnitID");
            oPLineConfigure.RefID = oReader.GetInt32("RefID");
            oPLineConfigure.LineNo = oReader.GetString("LineNo");
            oPLineConfigure.Remarks = oReader.GetString("Remarks");
            oPLineConfigure.RefName = oReader.GetString("RefName");
            oPLineConfigure.RefShortName = oReader.GetString("RefShortName");
            oPLineConfigure.PUShortName = oReader.GetString("PUShortName");
            oPLineConfigure.PUName = oReader.GetString("PUName");
            oPLineConfigure.MachineQty = oReader.GetInt32("MachineQty");
            oPLineConfigure.ApproxPlanStartDate = oReader.GetDateTime("ApproxPlanStartDate");
            return oPLineConfigure;
        }

        private PLineConfigure CreateObject(NullHandler oReader)
        {
            PLineConfigure oPLineConfigure = new PLineConfigure();
            oPLineConfigure = MapObject(oReader);
            return oPLineConfigure;
        }

        private List<PLineConfigure> CreateObjects(IDataReader oReader)
        {
            List<PLineConfigure> oPLineConfigures = new List<PLineConfigure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PLineConfigure oItem = CreateObject(oHandler);
                oPLineConfigures.Add(oItem);
            }
            return oPLineConfigures;
        }

        #endregion

        #region Interface implementation
        public PLineConfigureService() { }
        public PLineConfigure Save(PLineConfigure oPLineConfigure, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region PLineConfigure
                IDataReader reader;
                if (oPLineConfigure.PLineConfigureID <= 0)
                {
                    reader = PLineConfigureDA.InsertUpdate(tc, oPLineConfigure, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = PLineConfigureDA.InsertUpdate(tc, oPLineConfigure, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLineConfigure = new PLineConfigure();
                    oPLineConfigure = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPLineConfigure = new PLineConfigure();
                oPLineConfigure.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oPLineConfigure;
        }
        public String Delete(PLineConfigure oPLineConfigure, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PLineConfigureDA.Delete(tc, oPLineConfigure, EnumDBOperation.Delete, nUserID);
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
        public PLineConfigure Get(int id, Int64 nUserId)
        {
            PLineConfigure oPLineConfigure = new PLineConfigure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PLineConfigureDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLineConfigure = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oPLineConfigure;
        }
        public List<PLineConfigure> Gets(string sSQL, Int64 nUserID)
        {
            List<PLineConfigure> oPLineConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PLineConfigureDA.Gets(sSQL, tc);
                oPLineConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PLineConfigure", e);
                #endregion
            }
            return oPLineConfigure;
        }

        public List<PLineConfigure> Gets(int nPUnitID, Int64 nUserID)
        {
            List<PLineConfigure> oPLineConfigure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PLineConfigureDA.Gets(tc, nPUnitID);
                oPLineConfigure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PLineConfigure", e);
                #endregion
            }
            return oPLineConfigure;
        }
        public List<PLineConfigure> Gets(Int64 nUserId)
        {
            List<PLineConfigure> oPLineConfigures = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PLineConfigureDA.Gets(tc);
                oPLineConfigures = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oPLineConfigures;
        }
        #endregion
    }
}