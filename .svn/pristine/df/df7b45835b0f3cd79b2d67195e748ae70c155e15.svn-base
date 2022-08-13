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
    
    public class ExportPartyInfoService : MarshalByRefObject, IExportPartyInfoService
    {
        #region Private functions and declaration
        private static ExportPartyInfo MapObject(NullHandler oReader)
        {
            ExportPartyInfo oExportPartyInfo = new ExportPartyInfo();
            oExportPartyInfo.ExportPartyInfoID = oReader.GetInt32("ExportPartyInfoID");
            oExportPartyInfo.Name = oReader.GetString("Name");
            return oExportPartyInfo;            
        }

          public static  ExportPartyInfo CreateObject(NullHandler oReader)
        {
            ExportPartyInfo oExportPartyInfo = new ExportPartyInfo();
            oExportPartyInfo=MapObject(oReader);
            return oExportPartyInfo;
        }

        private List<ExportPartyInfo> CreateObjects(IDataReader oReader)
        {
            List<ExportPartyInfo> oExportPartyInfos = new List<ExportPartyInfo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPartyInfo oItem = CreateObject(oHandler);
                oExportPartyInfos.Add(oItem);
            }
            return oExportPartyInfos;
        }
        #endregion

        #region Interface implementation
        public ExportPartyInfoService() { }


        public ExportPartyInfo Save(ExportPartyInfo oExportPartyInfo, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportPartyInfo.ExportPartyInfoID <= 0)
                {
                    reader = ExportPartyInfoDA.InsertUpdate(tc, oExportPartyInfo, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportPartyInfoDA.InsertUpdate(tc, oExportPartyInfo, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPartyInfo = new ExportPartyInfo();
                    oExportPartyInfo = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPartyInfo.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPartyInfo;
        }
        public string Delete(ExportPartyInfo oExportPartyInfo, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPartyInfoDA.Delete(tc, oExportPartyInfo, EnumDBOperation.Delete, nUserId);
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

        public ExportPartyInfo Get(int id, Int64 nUserID)
        {
            ExportPartyInfo oExportPartyInfo = new ExportPartyInfo();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPartyInfoDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPartyInfo = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPartyInfo", e);
                #endregion
            }

            return oExportPartyInfo;
        }
   
        public List<ExportPartyInfo> Gets( Int64 nUserID)
        {
            List<ExportPartyInfo> oExportPartyInfos = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPartyInfoDA.Gets(tc);
                oExportPartyInfos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPartyInfos", e);
                #endregion
            }

            return oExportPartyInfos;
        }
        public List<ExportPartyInfo> Gets(int nContractorID,Int64 nUserID)
        {
            List<ExportPartyInfo> oExportPartyInfos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPartyInfoDA.Gets(tc, nContractorID);
                oExportPartyInfos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPartyInfos", e);
                #endregion
            }

            return oExportPartyInfos;
        }
   
   
       
        #endregion
    }
}
