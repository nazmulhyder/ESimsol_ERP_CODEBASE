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
    public class OperationStageService : MarshalByRefObject, IOperationStageService
    {
        #region Private functions and declaration
        private OperationStage MapObject(NullHandler oReader)
        {
            OperationStage oOperationStage = new OperationStage();
            oOperationStage.OperationStageID = oReader.GetInt32("OperationStageID");
            oOperationStage.Name = oReader.GetString("Name");
            oOperationStage.OperationStageEnum = (EnumOperationStage)oReader.GetInt16("OperationStage");
            return oOperationStage;
        }

        private OperationStage CreateObject(NullHandler oReader)
        {
            OperationStage oOperationStages = MapObject(oReader);
            return oOperationStages;
        }

        private List<OperationStage> CreateObjects(IDataReader oReader)
        {
            List<OperationStage> oOperationStages = new List<OperationStage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OperationStage oItem = CreateObject(oHandler);
                oOperationStages.Add(oItem);
            }
            return oOperationStages;
        }

        #endregion

        #region Interface implementation
        public OperationStageService() { }

        public OperationStage IUD(OperationStage oOperationStage, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
              
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update )
                {
                    reader = OperationStageDA.IUD(tc, oOperationStage, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOperationStage = new OperationStage();
                        oOperationStage = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = OperationStageDA.IUD(tc, oOperationStage, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oOperationStage.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oOperationStage = new OperationStage();
                oOperationStage.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oOperationStage;
        }

        public OperationStage Get(int nSUPCRID, Int64 nUserId)
        {
            OperationStage oOperationStage = new OperationStage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OperationStageDA.Get(tc, nSUPCRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOperationStage = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oOperationStage = new OperationStage();
                oOperationStage.ErrorMessage = ex.Message;
                #endregion
            }

            return oOperationStage;
        }

        public List<OperationStage> Gets(string sSQL, Int64 nUserID)
        {
            List<OperationStage> oOperationStages = new List<OperationStage>();
            OperationStage oOperationStage = new OperationStage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = OperationStageDA.Gets(tc, sSQL);
                oOperationStages = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oOperationStage.ErrorMessage = ex.Message;
                oOperationStages.Add(oOperationStage);
                #endregion
            }

            return oOperationStages;
        }
        public List<OperationStage> Gets( Int64 nUserID)
        {
            List<OperationStage> oOperationStages = new List<OperationStage>();
            OperationStage oOperationStage = new OperationStage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = OperationStageDA.Gets(tc);
                oOperationStages = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oOperationStage.ErrorMessage = ex.Message;
                oOperationStages.Add(oOperationStage);
                #endregion
            }

            return oOperationStages;
        }
        #endregion
    }
}
