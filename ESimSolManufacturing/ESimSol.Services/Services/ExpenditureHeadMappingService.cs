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
    [Serializable]
    public class ExpenditureHeadMappingService : MarshalByRefObject, IExpenditureHeadMappingService
    {
        #region Private functions and declaration
        private ExpenditureHeadMapping MapObject(NullHandler oReader)
        {
            ExpenditureHeadMapping oExpenditureHeadMapping = new ExpenditureHeadMapping();
            oExpenditureHeadMapping.ExpenditureHeadMappingID= oReader.GetInt32("ExpenditureHeadMappingID");
            oExpenditureHeadMapping.ExpenditureHeadID = oReader.GetInt32("ExpenditureHeadID");
            oExpenditureHeadMapping.DrCrType = oReader.GetInt32("DrCrType");
            oExpenditureHeadMapping.OperationTypeInt = oReader.GetInt32("OperationType");
            oExpenditureHeadMapping.OperationType = (EnumExpenditureType)oReader.GetInt32("OperationType");
           
            
            
            return oExpenditureHeadMapping;
        }

        private ExpenditureHeadMapping CreateObject(NullHandler oReader)
        {
            ExpenditureHeadMapping oExpenditureHeadMapping = new ExpenditureHeadMapping();
            oExpenditureHeadMapping=MapObject(oReader);
            return oExpenditureHeadMapping;
        }

        private List<ExpenditureHeadMapping> CreateObjects(IDataReader oReader)
        {
            List<ExpenditureHeadMapping> oExpenditureHeadMappings = new List<ExpenditureHeadMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExpenditureHeadMapping oItem = CreateObject(oHandler);
                oExpenditureHeadMappings.Add(oItem);
            }
            return oExpenditureHeadMappings;
        }
        #endregion

        #region Interface implementation
        public ExpenditureHeadMappingService() { }

        #region New Version
        public ExpenditureHeadMapping Get(int nExpenditureHeadMappingID, Int64 nUserId)
        {
            ExpenditureHeadMapping oExpenditureHeadMapping = new ExpenditureHeadMapping();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExpenditureHeadMappingDA.Get(tc, nExpenditureHeadMappingID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExpenditureHeadMapping = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Import Invoice Product", e);
                #endregion
            }

            return oExpenditureHeadMapping;
        }

        public List<ExpenditureHeadMapping> Gets(int nExpenditureHeadID, Int64 nUserId)
        {
            List<ExpenditureHeadMapping> oExpenditureHeadMappings = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExpenditureHeadMappingDA.Gets(nExpenditureHeadID, tc);
                oExpenditureHeadMappings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import Invoice Products", e);
                #endregion
            }
            return oExpenditureHeadMappings;
        }

        public List<ExpenditureHeadMapping> Gets(string sSQL, Int64 nUserId)
        {
            List<ExpenditureHeadMapping> oExpenditureHeadMapping = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExpenditureHeadMappingDA.Gets(tc, sSQL);
                oExpenditureHeadMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExpenditureHeadMapping", e);
                #endregion
            }

            return oExpenditureHeadMapping;
        }

   

        #endregion

        public string Delete(ExpenditureHeadMapping oExpenditureHeadMapping, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExpenditureHeadMappingDA.Delete(tc, oExpenditureHeadMapping, EnumDBOperation.Delete, nUserID,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
    

        #endregion
    }
}
