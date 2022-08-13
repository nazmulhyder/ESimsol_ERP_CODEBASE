using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Linq;
namespace ESimSol.Services.Services
{
    public class LotBaseTestResultService : MarshalByRefObject, ILotBaseTestResultService
    {
        #region Private functions and declaration
        private static LotBaseTestResult MapObject(NullHandler oReader)
        {
            LotBaseTestResult oLotBaseTestResult = new LotBaseTestResult();
            oLotBaseTestResult.LotBaseTestResultID = oReader.GetInt32("LotBaseTestResultID");
            oLotBaseTestResult.LotBaseTestID = oReader.GetInt32("LotBaseTestID");
            oLotBaseTestResult.LotID = oReader.GetInt32("LotID");
            oLotBaseTestResult.Qty = oReader.GetDouble("Qty");
            oLotBaseTestResult.Note = oReader.GetString("Note");
            oLotBaseTestResult.Name = oReader.GetString("Name");
            oLotBaseTestResult.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oLotBaseTestResult.UserName = oReader.GetString("UserName");
            return oLotBaseTestResult;
        }

        public static LotBaseTestResult CreateObject(NullHandler oReader)
        {
            LotBaseTestResult oLotBaseTestResult = MapObject(oReader);
            return oLotBaseTestResult;
        }

        private List<LotBaseTestResult> CreateObjects(IDataReader oReader)
        {
            List<LotBaseTestResult> oLotBaseTestResults = new List<LotBaseTestResult>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LotBaseTestResult oItem = CreateObject(oHandler);
                oLotBaseTestResults.Add(oItem);
            }
            return oLotBaseTestResults;
        }

        #endregion

        #region Interface implementation
        public LotBaseTestResultService() { }

        public LotBaseTestResult IUD(LotBaseTestResult oLotBaseTestResult, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    
                    reader = LotBaseTestResultDA.IUD(tc, oLotBaseTestResult, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLotBaseTestResult = new LotBaseTestResult();
                        oLotBaseTestResult = CreateObject(oReader);
                    }
                    reader.Close();
                    

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = LotBaseTestResultDA.IUD(tc, oLotBaseTestResult, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oLotBaseTestResult.ErrorMessage = Global.DeleteMessage;
                }
                else
                {
                    throw new Exception("Invalid Operation In Service");
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBaseTestResult = new LotBaseTestResult();
                oLotBaseTestResult.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oLotBaseTestResult;
        }

        public LotBaseTestResult Get(int nLotBaseTestResultID, Int64 nUserId)
        {
            LotBaseTestResult oLotBaseTestResult = new LotBaseTestResult();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LotBaseTestResultDA.Get(tc, nLotBaseTestResultID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLotBaseTestResult = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBaseTestResult = new LotBaseTestResult();
                oLotBaseTestResult.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oLotBaseTestResult;
        }

        public List<LotBaseTestResult> Gets(string sSQL, Int64 nUserID)
        {
            List<LotBaseTestResult> oLotBaseTestResults = new List<LotBaseTestResult>();
            LotBaseTestResult oLotBaseTestResult = new LotBaseTestResult();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LotBaseTestResultDA.Gets(tc, sSQL);
                oLotBaseTestResults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLotBaseTestResult.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oLotBaseTestResults.Add(oLotBaseTestResult);
                #endregion
            }

            return oLotBaseTestResults;
        }


      

        #endregion
    }
}
