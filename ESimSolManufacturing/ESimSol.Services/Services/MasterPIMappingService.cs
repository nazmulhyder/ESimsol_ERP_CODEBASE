using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class MasterPIMappingService : MarshalByRefObject, IMasterPIMappingService
    {
        #region Private functions and declaration
        private MasterPIMapping MapObject(NullHandler oReader)
        {
            MasterPIMapping oMasterPIMapping = new MasterPIMapping();
            oMasterPIMapping.MasterPIMappingID = oReader.GetInt32("MasterPIMappingID");
            oMasterPIMapping.ExportPIID = oReader.GetInt32("ExportPIID");
            oMasterPIMapping.MasterPIID = oReader.GetInt32("MasterPIID");
            oMasterPIMapping.CurrencyID = oReader.GetInt32("CurrencyID");
            oMasterPIMapping.Amount = oReader.GetDouble("Amount");
            oMasterPIMapping.ContractorName = oReader.GetString("ContractorName");
            oMasterPIMapping.BankAccountNo = oReader.GetString("BankAccountNo");
            oMasterPIMapping.AccountName = oReader.GetString("AccountName");
            oMasterPIMapping.PINo = oReader.GetString("PINo");
            oMasterPIMapping.MasterPINo = oReader.GetString("MasterPINo");
            oMasterPIMapping.Currency = oReader.GetString("Currency");
            oMasterPIMapping.IssueDate = oReader.GetDateTime("IssueDate");
            oMasterPIMapping.ValidityDate = oReader.GetDateTime("ValidityDate");
            oMasterPIMapping.BranchAddress = oReader.GetString("BranchAddress");
            oMasterPIMapping.MKTPName = oReader.GetString("MKTPName");
            oMasterPIMapping.UnitSymbol = oReader.GetString("UnitSymbol");
            oMasterPIMapping.BranchName = oReader.GetString("BranchName");
            return oMasterPIMapping;
        }
        private MasterPIMapping CreateObject(NullHandler oReader)
        {
            MasterPIMapping oMasterPIMapping = new MasterPIMapping();
            oMasterPIMapping = MapObject(oReader);
            return oMasterPIMapping;
        }      
        private List<MasterPIMapping> CreateObjects(IDataReader oReader)
        {
            List<MasterPIMapping> oMasterPIMappings = new List<MasterPIMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MasterPIMapping oItem = CreateObject(oHandler);
                oMasterPIMappings.Add(oItem);
            }
            return oMasterPIMappings;
        }
        #endregion

        #region Interface implementation
        public MasterPIMapping Get(int id, Int64 nUserId)
        {
            MasterPIMapping oMasterPIMapping = new MasterPIMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterPIMappingDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterPIMapping = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MasterPIMapping", e);
                #endregion
            }

            return oMasterPIMapping;
        }
        public List<MasterPIMapping> GetsByMasterPI(int nMasterPIID, Int64 nUserId)
        {
            List<MasterPIMapping> oMasterPIMapping = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterPIMappingDA.GetsByMasterPI(tc, nMasterPIID);
                oMasterPIMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oMasterPIMapping;
        }
        public List<MasterPIMapping> Gets(Int64 nUserId)
        {
            List<MasterPIMapping> oMasterPIMapping = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MasterPIMappingDA.Gets(tc);
                oMasterPIMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIDetail", e);
                #endregion
            }

            return oMasterPIMapping;
        }
        public List<MasterPIMapping> Gets(string sSQL, Int64 nUserId)
        {
            List<MasterPIMapping> oMasterPIMapping = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterPIMappingDA.Gets(tc, sSQL);
                oMasterPIMapping = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterPIMapping", e);
                #endregion
            }

            return oMasterPIMapping;
        }
    
    
        #endregion
    }
}
