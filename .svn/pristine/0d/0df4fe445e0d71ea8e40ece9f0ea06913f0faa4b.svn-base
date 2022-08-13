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
    public class ExportTermsAndConditionService : MarshalByRefObject, IExportTermsAndConditionService
    {
        #region Private functions and declaration
        private ExportTermsAndCondition MapObject(NullHandler oReader)
        {
            ExportTermsAndCondition oExportTermsAndCondition = new ExportTermsAndCondition();
            oExportTermsAndCondition.ExportTermsAndConditionID = oReader.GetInt32("ExportTermsAndConditionID");
            oExportTermsAndCondition.Clause = oReader.GetString("Clause");
            oExportTermsAndCondition.ClauseType = oReader.GetInt32("ClauseType");
            oExportTermsAndCondition.ExportTnCCaptionID = oReader.GetInt32("ExportTnCCaptionID");
            oExportTermsAndCondition.CaptionName = oReader.GetString("CaptionName");
            oExportTermsAndCondition.Note = oReader.GetString("Note");
            oExportTermsAndCondition.BUID = oReader.GetInt32("BUID");
            oExportTermsAndCondition.DocFor =  (EnumDocFor)oReader.GetInt32("DocFor");
            oExportTermsAndCondition.DocForInInt =  oReader.GetInt32("DocFor");
            oExportTermsAndCondition.SLNo = oReader.GetInt32("SLNo");
            oExportTermsAndCondition.Activity = oReader.GetBoolean("Activity");
            oExportTermsAndCondition.BUName = oReader.GetString("BUName");
            
            
            return oExportTermsAndCondition;
        }

        private ExportTermsAndCondition CreateObject(NullHandler oReader)
        {
            ExportTermsAndCondition oExportTermsAndCondition = new ExportTermsAndCondition();
            oExportTermsAndCondition = MapObject(oReader);
            return oExportTermsAndCondition;
        }

        private List<ExportTermsAndCondition> CreateObjects(IDataReader oReader)
        {
            List<ExportTermsAndCondition> oExportTermsAndConditions = new List<ExportTermsAndCondition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportTermsAndCondition oItem = CreateObject(oHandler);
                oExportTermsAndConditions.Add(oItem);
            }
            return oExportTermsAndConditions;
        }

        #endregion

        #region Interface implementation
        public ExportTermsAndConditionService() { }

        public ExportTermsAndCondition Save(ExportTermsAndCondition oExportTermsAndCondition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportTermsAndCondition.ExportTermsAndConditionID <= 0)
                {
                    reader = ExportTermsAndConditionDA.InsertUpdate(tc, oExportTermsAndCondition, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ExportTermsAndConditionDA.InsertUpdate(tc, oExportTermsAndCondition, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportTermsAndCondition = new ExportTermsAndCondition();
                    oExportTermsAndCondition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportTermsAndCondition.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oExportTermsAndCondition;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
                ExportTermsAndCondition oExportTermsAndCondition = new ExportTermsAndCondition();
                oExportTermsAndCondition.ExportTermsAndConditionID = id;
                ExportTermsAndConditionDA.Delete(tc, oExportTermsAndCondition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ExportTermsAndCondition Get(int id, Int64 nUserId)
        {
            ExportTermsAndCondition oExportTermsAndCondition = new ExportTermsAndCondition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportTermsAndConditionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportTermsAndCondition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportTermsAndCondition;
        }

        public List<ExportTermsAndCondition> GetsByTypeAndBU(string sDocFors, string BUs, Int64 nUserId)
        {
            List<ExportTermsAndCondition> oExportTermsAndConditions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportTermsAndConditionDA.GetsByTypeAndBU(sDocFors, BUs, tc);
                oExportTermsAndConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportTermsAndConditions;
        }

        public List<ExportTermsAndCondition> Gets(bool bActivity, int nBUID,Int64 nUserId)
        {
            List<ExportTermsAndCondition> oExportTermsAndConditions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportTermsAndConditionDA.Gets(tc, bActivity, nBUID);
                oExportTermsAndConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportTermsAndConditions;
        }

        //BUWiseGets
        public List<ExportTermsAndCondition> BUWiseGets( int nBUID, Int64 nUserId)
        {
            List<ExportTermsAndCondition> oExportTermsAndConditions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportTermsAndConditionDA.BUWiseGets(tc,  nBUID);
                oExportTermsAndConditions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oExportTermsAndConditions;
        }
        public string ActivatePITandCSetup(ExportTermsAndCondition oExportTermsAndCondition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {


                tc = TransactionContext.Begin(true);
                ExportTermsAndConditionDA.ActivatePITandCSetup(tc, oExportTermsAndCondition);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Activate PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return "Activate sucessfully";
        }

        public ExportTermsAndCondition RefreshSequence(ExportTermsAndCondition oExportTermsAndCondition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oExportTermsAndCondition.ExportTermsAndConditions.Count > 0)
                {
                    foreach (ExportTermsAndCondition oItem in oExportTermsAndCondition.ExportTermsAndConditions)
                    {
                        if (oItem.ExportTermsAndConditionID > 0 && oItem.SLNo > 0)
                        {
                            ExportTermsAndConditionDA.UpdateSequence(tc, oItem);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportTermsAndCondition = new ExportTermsAndCondition();
                oExportTermsAndCondition.ErrorMessage = e.Message;
                #endregion
            }
            return oExportTermsAndCondition;
        }


        #endregion
    }
}
