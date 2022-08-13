using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class ExportPISignatoryService : MarshalByRefObject, IExportPISignatoryService
    {
        #region Private functions and declaration

        private ExportPISignatory MapObject(NullHandler oReader)
        {
            ExportPISignatory oExportPISignatory = new ExportPISignatory();
            oExportPISignatory.ExportPISignatoryID = oReader.GetInt32("ExportPISignatoryID");
            oExportPISignatory.ApprovalHeadID = oReader.GetInt32("ApprovalHeadID");
            oExportPISignatory.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPISignatory.SLNo = oReader.GetInt32("SLNo");
            oExportPISignatory.ReviseNo = oReader.GetInt32("ReviseNo");
            oExportPISignatory.RequestTo = oReader.GetInt32("RequestTo");
            oExportPISignatory.ApproveBy = oReader.GetInt32("ApproveBy");
            oExportPISignatory.ApproveDate = oReader.GetDateTime("ApproveDate");
            oExportPISignatory.IsApprove = oReader.GetBoolean("IsApprove");
            oExportPISignatory.Name_Request = oReader.GetString("Name_Request");
            oExportPISignatory.HeadName = oReader.GetString("HeadName");
            oExportPISignatory.Note = oReader.GetString("Note");
            return oExportPISignatory;
        }

        private ExportPISignatory CreateObject(NullHandler oReader)
        {
            ExportPISignatory oExportPISignatory = new ExportPISignatory();
            oExportPISignatory = MapObject(oReader);
            return oExportPISignatory;
        }

        private List<ExportPISignatory> CreateObjects(IDataReader oReader)
        {
            List<ExportPISignatory> oExportPISignatory = new List<ExportPISignatory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPISignatory oItem = CreateObject(oHandler);
                oExportPISignatory.Add(oItem);
            }
            return oExportPISignatory;
        }

        #endregion

        #region Interface implementation

        public List<ExportPISignatory> SaveAll(List<ExportPISignatory> oExportPISignatorys, string sExportPISignatoryID, Int64 nUserID)
        {
            ExportPISignatory oExportPISignatory = new ExportPISignatory();
            List<ExportPISignatory> oExportPISignatorys_Return = new List<ExportPISignatory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPISignatorys != null)
                {
                    ExportPISignatoryDA.DeleteAll(tc, oExportPISignatorys[0].ExportPIID, sExportPISignatoryID);
                    foreach (ExportPISignatory oItem in oExportPISignatorys)
                    {
                        if (oItem.ExportPISignatoryID <= 0)
                        {
                            reader = ExportPISignatoryDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = ExportPISignatoryDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oExportPISignatory = new ExportPISignatory();
                            oExportPISignatory = CreateObject(oReader);
                            oExportPISignatorys_Return.Add(oExportPISignatory);
                        }
                        reader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPISignatorys_Return = new List<ExportPISignatory>();
                oExportPISignatory = new ExportPISignatory();
                oExportPISignatory.ErrorMessage = e.Message.Split('~')[0];
                oExportPISignatorys_Return.Add(oExportPISignatory);

                #endregion
            }
            return oExportPISignatorys_Return;
        }
        public ExportPISignatory Save(ExportPISignatory oExportPISignatory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPISignatory.ExportPISignatoryID <= 0)
                {
                    reader = ExportPISignatoryDA.InsertUpdate(tc, oExportPISignatory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportPISignatoryDA.InsertUpdate(tc, oExportPISignatory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPISignatory = new ExportPISignatory();
                    oExportPISignatory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPISignatory = new ExportPISignatory();
                oExportPISignatory.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPISignatory;
        }
        public string Delete(ExportPISignatory oExportPISignatory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "ExportPISignatory", EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "ExportPISignatory", id);
                ExportPISignatoryDA.Delete(tc, oExportPISignatory, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ExportPISignatory Get(int id, Int64 nUserId)
        {
            ExportPISignatory oExportPISignatory = new ExportPISignatory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportPISignatoryDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPISignatory = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPISignatory", e);
                #endregion
            }
            return oExportPISignatory;
        }
        public List<ExportPISignatory> Gets(int nExportPIID, Int64 nUserID)
        {
            List<ExportPISignatory> oExportPISignatorys = new List<ExportPISignatory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPISignatoryDA.Gets(tc, nExportPIID);
                oExportPISignatorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExportPISignatory oExportPISignatory = new ExportPISignatory();
                oExportPISignatory.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportPISignatorys;
        }
        public List<ExportPISignatory> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportPISignatory> oExportPISignatorys = new List<ExportPISignatory>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPISignatoryDA.Gets(tc, sSQL);
                oExportPISignatorys = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPISignatory", e);
                #endregion
            }
            return oExportPISignatorys;
        }

        #endregion
    }

}
