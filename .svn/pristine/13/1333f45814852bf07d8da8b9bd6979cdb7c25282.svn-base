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

    public class QCTemplateService : MarshalByRefObject, IQCTemplateService
    {
        #region Private functions and declaration
        private QCTemplate MapObject(NullHandler oReader)
        {
            QCTemplate oQCTemplate = new QCTemplate();
            oQCTemplate.QCTemplateID = oReader.GetInt32("QCTemplateID");
            oQCTemplate.CreateBy = oReader.GetInt32("CreateBy");
            oQCTemplate.TemplateNo = oReader.GetString("TemplateNo");
            oQCTemplate.TemplateName = oReader.GetString("TemplateName");
            oQCTemplate.CreateByName = oReader.GetString("CreateByName");
            oQCTemplate.CreateDate = oReader.GetDateTime("CreateDate");
            oQCTemplate.Note = oReader.GetString("Note");
           
            
            return oQCTemplate;
        }

        private QCTemplate CreateObject(NullHandler oReader)
        {
            QCTemplate oQCTemplate = new QCTemplate();
            oQCTemplate = MapObject(oReader);
            return oQCTemplate;
        }

        private List<QCTemplate> CreateObjects(IDataReader oReader)
        {
            List<QCTemplate> oQCTemplate = new List<QCTemplate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                QCTemplate oItem = CreateObject(oHandler);
                oQCTemplate.Add(oItem);
            }
            return oQCTemplate;
        }

        #endregion

        #region Interface implementation
        public QCTemplateService() { }

        public QCTemplate Save(QCTemplate oQCTemplate, Int64 nUserID)
        {
            List<QCTemplateDetail> oQCTemplateDetails = new List<QCTemplateDetail>();
            oQCTemplateDetails = oQCTemplate.QCTemplateDetails;
            string sQCTemplateDetailIDs = "";
            bool bIsInitialSave = oQCTemplate.bIsInitialSave;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oQCTemplate.QCTemplateID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.QCTemplate, EnumRoleOperationType.Add);
                    reader = QCTemplateDA.InsertUpdate(tc, oQCTemplate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.QCTemplate, EnumRoleOperationType.Edit);
                    reader = QCTemplateDA.InsertUpdate(tc, oQCTemplate, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oQCTemplate = new QCTemplate();
                    oQCTemplate = CreateObject(oReader);
                }
                reader.Close();
                if (!bIsInitialSave)
                {
                    #region QC Tamplete Detail Part
                    if (oQCTemplateDetails != null)
                    {
                        foreach (QCTemplateDetail oItem in oQCTemplateDetails)
                        {
                            IDataReader readerdetail;
                            oItem.QCTemplateID = oQCTemplate.QCTemplateID;
                            if (oItem.QCTemplateDetailID <= 0)
                            {
                                readerdetail = QCTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,false);
                            }
                            else
                            {
                                readerdetail = QCTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,false);
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sQCTemplateDetailIDs = sQCTemplateDetailIDs + oReaderDetail.GetString("QCTemplateDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sQCTemplateDetailIDs.Length > 0)
                        {
                            sQCTemplateDetailIDs = sQCTemplateDetailIDs.Remove(sQCTemplateDetailIDs.Length - 1, 1);
                        }
                        QCTemplateDetail oQCTemplateDetail = new QCTemplateDetail();
                        oQCTemplateDetail.QCTemplateID = oQCTemplate.QCTemplateID;
                        QCTemplateDetailDA.Delete(tc, oQCTemplateDetail, EnumDBOperation.Delete, nUserID, sQCTemplateDetailIDs);
                    }

                    #endregion
                }
                else
                {
                    #region QC Templete Detail for Initial Save
                    foreach (QCTemplateDetail oItem in oQCTemplateDetails)
                    {
                        IDataReader readerdetail;
                        oItem.QCTemplateID = oQCTemplate.QCTemplateID;
                        readerdetail = QCTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,true);
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            
                        }
                        readerdetail.Close();
                    }
                    #endregion
                }

                #region Template Get
                #region REfresh Sequence
                QCTemplateDetail oNewQCTemplateDetail = new QCTemplateDetail();
                oNewQCTemplateDetail.QCTemplateID =oQCTemplate.QCTemplateID;
                QCTemplateDetailDA.UpDown(tc, oNewQCTemplateDetail, true);
                #endregion
                reader = QCTemplateDA.Get(tc, oQCTemplate.QCTemplateID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oQCTemplate = CreateObject(oReader);
                }
                reader.Close();
                //Detail Gets
                reader = QCTemplateDetailDA.Gets(oQCTemplate.QCTemplateID, tc);
                oQCTemplate.QCTemplateDetails = QCTemplateDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oQCTemplate = new QCTemplate();
                oQCTemplate.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oQCTemplate;
        }


        public QCTemplate UpDown(QCTemplateDetail oQCTemplateDetail, Int64 nUserID)
        {
            QCTemplate oQCTemplate = new QCTemplate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                QCTemplateDetailDA.UpDown(tc, oQCTemplateDetail, false);
                #region Template Get
                reader = QCTemplateDA.Get(tc, oQCTemplateDetail.QCTemplateID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oQCTemplate = CreateObject(oReader);
                }
                reader.Close();
                //Detail Gets
                reader = QCTemplateDetailDA.Gets(oQCTemplate.QCTemplateID, tc);
                oQCTemplate.QCTemplateDetails = QCTemplateDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oQCTemplate = new QCTemplate();
                oQCTemplate.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oQCTemplate;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                QCTemplate oQCTemplate = new QCTemplate();
                oQCTemplate.QCTemplateID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.QCTemplate, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "QCTemplate", id);
                QCTemplateDA.Delete(tc, oQCTemplate, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete QCTemplate. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public QCTemplate Get(int id, Int64 nUserId)
        {
            QCTemplate oQCTemplate = new QCTemplate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = QCTemplateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oQCTemplate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get QCTemplate", e);
                #endregion
            }

            return oQCTemplate;
        }


        public List<QCTemplate> Gets(Int64 nUserID)
        {
            List<QCTemplate> oQCTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCTemplateDA.Gets(tc);
                oQCTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCTemplate", e);
                #endregion
            }

            return oQCTemplate;
        }

        
        public List<QCTemplate> Gets(string sSQL, Int64 nUserID)
        {
            List<QCTemplate> oQCTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = QCTemplateDA.Gets(tc, sSQL);
                oQCTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCTemplate", e);
                #endregion
            }

            return oQCTemplate;
        }

        #endregion
    }   
    
   
}
