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
    public class LabDipSetupService : MarshalByRefObject, ILabDipSetupService
    {
        #region Private functions and declaration
        private LabDipSetup MapObject(NullHandler oReader)
        {
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup.LabDipSetupID = oReader.GetInt32("LabDipSetupID");
            oLabDipSetup.IsApplyCode = oReader.GetBoolean("IsApplyCode");
            oLabDipSetup.IsApplyPO = oReader.GetBoolean("IsApplyPO");
            oLabDipSetup.PrintNo = oReader.GetInt32("PrintNo");
            oLabDipSetup.OrderCode = oReader.GetString("OrderCode");
            oLabDipSetup.ColorNoName = oReader.GetString("ColorNoName");
            oLabDipSetup.OrderName = oReader.GetString("OrderName");
            oLabDipSetup.LDName = oReader.GetString("LDName");
            oLabDipSetup.LDNoCreateBy = oReader.GetInt32("LDNoCreateBy");
            
            return oLabDipSetup;
        }

        private LabDipSetup CreateObject(NullHandler oReader)
        {
            LabDipSetup oLabDipSetup = new LabDipSetup();
            oLabDipSetup = MapObject(oReader);
            return oLabDipSetup;
        }

        private List<LabDipSetup> CreateObjects(IDataReader oReader)
        {
            List<LabDipSetup> oLabDipSetups = new List<LabDipSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabDipSetup oItem = CreateObject(oHandler);
                oLabDipSetups.Add(oItem);
            }
            return oLabDipSetups;
        }

        #endregion

        #region Interface implementation
        public LabDipSetupService() { }


        public LabDipSetup Save(LabDipSetup oLabDipSetup, Int64 nUserId)
        {

            TransactionContext tc = null;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region LabDipSetup
                IDataReader reader;
                if (oLabDipSetup.LabDipSetupID <= 0)
                {
                    reader = LabDipSetupDA.InsertUpdate(tc, oLabDipSetup, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = LabDipSetupDA.InsertUpdate(tc, oLabDipSetup, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipSetup = new LabDipSetup();
                    oLabDipSetup = CreateObject(oReader);
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
                oLabDipSetup = new LabDipSetup();
                oLabDipSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLabDipSetup;
        }
      
        public String Delete(LabDipSetup oLabDipSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LabDipSetupDA.Delete(tc, oLabDipSetup, EnumDBOperation.Delete, nUserID);
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
        public LabDipSetup Get(int id, Int64 nUserId)
        {
            LabDipSetup oLabDipSetup = new LabDipSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipSetup = CreateObject(oReader);
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

            return oLabDipSetup;
        }
        public LabDipSetup GetByType(int nOrderType, Int64 nUserId)
        {
            LabDipSetup oLabDipSetup = new LabDipSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipSetupDA.GetByType(tc, nOrderType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipSetup = CreateObject(oReader);
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

            return oLabDipSetup;
        }
     

        public List<LabDipSetup> Gets(Int64 nUserId)
        {
            List<LabDipSetup> oLabDipSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipSetupDA.Gets(tc);
                oLabDipSetups = CreateObjects(reader);
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

            return oLabDipSetups;
        }
        public List<LabDipSetup> Gets(int nBUID,Int64 nUserId)
        {
            List<LabDipSetup> oLabDipSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipSetupDA.Gets(tc, nBUID);
                oLabDipSetups = CreateObjects(reader);
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

            return oLabDipSetups;
        }
        public LabDipSetup Activate(LabDipSetup oLabDipSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LabDipSetupDA.Activate(tc, oLabDipSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabDipSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLabDipSetup = new LabDipSetup();
                oLabDipSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLabDipSetup;
        }
    

        #endregion
    }
}