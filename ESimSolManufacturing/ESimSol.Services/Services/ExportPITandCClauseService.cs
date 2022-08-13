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

    public class ExportPITandCClauseService : MarshalByRefObject, IExportPITandCClauseService
    {
        #region Private functions and declaration
        private ExportPITandCClause MapObject(NullHandler oReader)
        {
            ExportPITandCClause oExportPITandCClause = new ExportPITandCClause();
            oExportPITandCClause.ExportPITandCClauseID = oReader.GetInt32("ExportPITandCClauseID");
            oExportPITandCClause.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPITandCClause.ExportPITandCClauseLogID = oReader.GetInt32("ExportPITandCClauseLogID");
            oExportPITandCClause.ExportPILogID = oReader.GetInt32("ExportPILogID");
            oExportPITandCClause.TermsAndCondition = oReader.GetString("TermsAndCondition");
            oExportPITandCClause.DocFor = (EnumDocFor)oReader.GetInt32("DocFor");
            oExportPITandCClause.CaptionName = oReader.GetString("CaptionName");
            oExportPITandCClause.ExportTnCCaptionID = oReader.GetInt32("ExportTnCCaptionID");
            oExportPITandCClause.DocForInInt =oReader.GetInt32("DocFor");          
            return oExportPITandCClause;
        }

        private ExportPITandCClause CreateObject(NullHandler oReader)
        {
            ExportPITandCClause oExportPITandCClause = new ExportPITandCClause();
            oExportPITandCClause = MapObject(oReader);
            return oExportPITandCClause;
        }

        private List<ExportPITandCClause> CreateObjects(IDataReader oReader)
        {
            List<ExportPITandCClause> oExportPITandCClause = new List<ExportPITandCClause>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPITandCClause oItem = CreateObject(oHandler);
                oExportPITandCClause.Add(oItem);
            }
            return oExportPITandCClause;
        }

        #endregion

        #region Interface implementation
        public ExportPITandCClauseService() { }

        public ExportPITandCClause Save(ExportPITandCClause oExportPITandCClause, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPITandCClause.ExportPITandCClauseID <= 0)
                {
                    reader = ExportPITandCClauseDA.InsertUpdate(tc, oExportPITandCClause, EnumDBOperation.Insert, nUserId, "");
                }
                else
                {
                    reader = ExportPITandCClauseDA.InsertUpdate(tc, oExportPITandCClause, EnumDBOperation.Update, nUserId, "");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPITandCClause = new ExportPITandCClause();
                    oExportPITandCClause = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPITandCClause.ErrorMessage = e.Message;
                #endregion
            }
            return oExportPITandCClause;
        }

        public string Delete(ExportPITandCClause oExportPITandCClause, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ExportPITandCClauseDA.Delete(tc, oExportPITandCClause, EnumDBOperation.Delete, nUserId, "");
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
            return "Data Delete Successfully";
        }
        public string DeleteALL(ExportPITandCClause oExportPITandCClause, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                ExportPITandCClauseDA.DeleteALL(tc, oExportPITandCClause);
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
            return Global.DeleteMessage;
        }

        public string PITandCClauseSave(List<ExportPITandCClause> oExportPITandCClauses, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<ExportPITandCClause> _oExportPITandCClauses = new List<ExportPITandCClause>();
            ExportPITandCClause oExportPITandCClause = new ExportPITandCClause();
            oExportPITandCClause.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (ExportPITandCClause oItem in oExportPITandCClauses)
                {
                    IDataReader reader;
                    reader = ExportPITandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert,  nUserID, "");
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportPITandCClause = new ExportPITandCClause();
                        oExportPITandCClause = CreateObject(oReader);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportPITandCClause.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PITandCClause. Because of " + e.Message, e);
                #endregion
            }
            return "Save Successfully !";
        }

        public ExportPITandCClause Get(int id, Int64 nUserId)
        {
            ExportPITandCClause oAccountHead = new ExportPITandCClause();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPITandCClauseDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ExportPITandCClause> Gets(int nPIID, Int64 nUserID)
        {
            List<ExportPITandCClause> oExportPITandCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPITandCClauseDA.Gets(tc, nPIID);
                oExportPITandCClause = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oExportPITandCClause;
        }

        public List<ExportPITandCClause> GetsPILog(int id, Int64 nUserID)
        {
            List<ExportPITandCClause> oExportPITandCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPITandCClauseDA.GetsPILog(tc, id);
                oExportPITandCClause = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oExportPITandCClause;
        }


        public List<ExportPITandCClause> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportPITandCClause> oExportPITandCClause = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPITandCClauseDA.Gets(tc, sSQL);
                oExportPITandCClause = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PITandCClause", e);
                #endregion
            }

            return oExportPITandCClause;
        }
        #endregion
    }

}
