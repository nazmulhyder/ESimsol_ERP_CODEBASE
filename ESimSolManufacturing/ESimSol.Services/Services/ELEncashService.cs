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
    public class ELEncashService : MarshalByRefObject, IELEncashService
    {
        #region Private functions and declaration
        private ELEncash MapObject(NullHandler oReader)
        {
            ELEncash oELEncash = new ELEncash();
            oELEncash.ELEncashID = oReader.GetInt32("ELEncashID");
            oELEncash.EmpLeaveLedgerID = oReader.GetInt32("EmpLeaveLedgerID");
            oELEncash.DeclarationDate = oReader.GetDateTime("DeclarationDate");
            oELEncash.ConsiderELCount = oReader.GetInt32("ConsiderELCount");
            oELEncash.EncashELCount = oReader.GetDouble("EncashELCount");
            oELEncash.GrossSalary = oReader.GetDouble("GrossSalary");
            oELEncash.BasicAmount = oReader.GetDouble("BasicAmount");
            oELEncash.EncashAmount = oReader.GetDouble("EncashAmount");
            oELEncash.DPTName = oReader.GetString("DPTName");
            oELEncash.DSGName = oReader.GetString("DSGName");
            oELEncash.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oELEncash.Stamp = oReader.GetDouble("Stamp");
            oELEncash.ApproveBy = oReader.GetInt32("ApproveBy");
            oELEncash.ApproveDate = oReader.GetDateTime("ApproveDate");
            oELEncash.EmployeeCode = oReader.GetString("EmployeeCode");
            oELEncash.EmployeeName = oReader.GetString("EmployeeName");
            oELEncash.ApproveByName = oReader.GetString("ApproveByName");
            oELEncash.PresencePerLeave = oReader.GetDouble("PresencePerLeave");
            
            return oELEncash;
        }

        private ELEncash CreateObject(NullHandler oReader)
        {
            ELEncash oELEncash = MapObject(oReader);
            return oELEncash;
        }

        private List<ELEncash> CreateObjects(IDataReader oReader)
        {
            List<ELEncash> oELEncash = new List<ELEncash>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ELEncash oItem = CreateObject(oHandler);
                oELEncash.Add(oItem);
            }
            return oELEncash;
        }
        #endregion

        #region Interface implementation
        public ELEncashService() { }

        public List<ELEncash> Gets(string sSQL, Int64 nUserID)
        {
            List<ELEncash> oELEncash = new List<ELEncash>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ELEncashDA.Gets(tc, sSQL);
                oELEncash = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ELEncash", e);
                #endregion
            }
            return oELEncash;
        }

        public string RollBackEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, Int64 nUserID)
        {
            ELEncash oELEncash = new ELEncash();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = ELEncashDA.RollBackEncashedEL(tc,sELEncashIDs, dtDeclarationDate, nUserID);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oELEncash = new ELEncash();
                oELEncash.ErrorMessage = e.Message;
                #endregion
            }
            return oELEncash.ErrorMessage;
        }

        public List<ELEncash> ApproveEncashedEL(string sELEncashIDs, DateTime dtDeclarationDate, Int64 nUserID)
        {
            List<ELEncash> oELEncashs = new List<ELEncash>();
            ELEncash oELEncash = new ELEncash();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = null;
                reader = ELEncashDA.ApproveEncashedEL(tc, sELEncashIDs, dtDeclarationDate, nUserID);
                oELEncashs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oELEncashs = new List<ELEncash>();
                oELEncash = new ELEncash();
                oELEncash.ErrorMessage = e.Message;
                oELEncashs.Add(oELEncash);
                #endregion
            }
            return oELEncashs;
        }
        #endregion
    }
}
