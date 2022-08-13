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

    public class TAPTemplateService : MarshalByRefObject, ITAPTemplateService
    {
        #region Private functions and declaration
        private TAPTemplate MapObject(NullHandler oReader)
        {
            TAPTemplate oTAPTemplate = new TAPTemplate();
            oTAPTemplate.TAPTemplateID = oReader.GetInt32("TAPTemplateID");
            oTAPTemplate.CreateBy = oReader.GetInt32("CreateBy");
            oTAPTemplate.TemplateNo = oReader.GetString("TemplateNo");
            oTAPTemplate.TemplateName = oReader.GetString("TemplateName");
            oTAPTemplate.CreateByName = oReader.GetString("CreateByName");
            oTAPTemplate.CreateDate = oReader.GetDateTime("CreateDate");
            oTAPTemplate.Remarks = oReader.GetString("Remarks");
            oTAPTemplate.TampleteType = (EnumTSType)oReader.GetInt32("TampleteType");
            oTAPTemplate.CalculationType = (EnumCalculationType)oReader.GetInt32("CalculationType");
            
            return oTAPTemplate;
        }

        private TAPTemplate CreateObject(NullHandler oReader)
        {
            TAPTemplate oTAPTemplate = new TAPTemplate();
            oTAPTemplate = MapObject(oReader);
            return oTAPTemplate;
        }

        private List<TAPTemplate> CreateObjects(IDataReader oReader)
        {
            List<TAPTemplate> oTAPTemplate = new List<TAPTemplate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TAPTemplate oItem = CreateObject(oHandler);
                oTAPTemplate.Add(oItem);
            }
            return oTAPTemplate;
        }

        #endregion

        #region Interface implementation
        public TAPTemplateService() { }

        public TAPTemplate Save(TAPTemplate oTAPTemplate, Int64 nUserID)
        {
            List<TAPTemplateDetail> oTAPTemplateDetails = new List<TAPTemplateDetail>();
            oTAPTemplateDetails = oTAPTemplate.TAPTemplateDetails;
            string sTAPTemplateDetailIDs = "";
            bool bIsInitialSave = oTAPTemplate.bIsInitialSave;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTAPTemplate.TAPTemplateID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAPTemplate, EnumRoleOperationType.Add);
                    reader = TAPTemplateDA.InsertUpdate(tc, oTAPTemplate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAPTemplate, EnumRoleOperationType.Edit);
                    reader = TAPTemplateDA.InsertUpdate(tc, oTAPTemplate, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAPTemplate = new TAPTemplate();
                    oTAPTemplate = CreateObject(oReader);
                }
                reader.Close();
                if (!bIsInitialSave)
                {
                    #region TAP Tamplete Detail Part
                    if (oTAPTemplateDetails != null)
                    {
                        foreach (TAPTemplateDetail oItem in oTAPTemplateDetails)
                        {
                            IDataReader readerdetail;
                            oItem.TAPTemplateID = oTAPTemplate.TAPTemplateID;
                            if (oItem.TAPTemplateDetailID <= 0)
                            {
                                readerdetail = TAPTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,false);
                            }
                            else
                            {
                                readerdetail = TAPTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,false);
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sTAPTemplateDetailIDs = sTAPTemplateDetailIDs + oReaderDetail.GetString("TAPTemplateDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sTAPTemplateDetailIDs.Length > 0)
                        {
                            sTAPTemplateDetailIDs = sTAPTemplateDetailIDs.Remove(sTAPTemplateDetailIDs.Length - 1, 1);
                        }
                        TAPTemplateDetail oTAPTemplateDetail = new TAPTemplateDetail();
                        oTAPTemplateDetail.TAPTemplateID = oTAPTemplate.TAPTemplateID;
                        TAPTemplateDetailDA.Delete(tc, oTAPTemplateDetail, EnumDBOperation.Delete, nUserID, sTAPTemplateDetailIDs);
                    }

                    #endregion
                }
                else
                {
                    #region TAP Templete Detail for Initial Save
                    foreach (TAPTemplateDetail oItem in oTAPTemplateDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TAPTemplateID = oTAPTemplate.TAPTemplateID;
                        readerdetail = TAPTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,true);
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
                TAPTemplateDetail oNewTAPTemplateDetail = new TAPTemplateDetail();
                oNewTAPTemplateDetail.TAPTemplateID =oTAPTemplate.TAPTemplateID;
                TAPTemplateDetailDA.UpDown(tc, oNewTAPTemplateDetail, true);
                #endregion
                reader = TAPTemplateDA.Get(tc, oTAPTemplate.TAPTemplateID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAPTemplate = CreateObject(oReader);
                }
                reader.Close();
                //Detail Gets
                reader = TAPTemplateDetailDA.Gets(oTAPTemplate.TAPTemplateID, tc);
                oTAPTemplate.TAPTemplateDetails = TAPTemplateDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAPTemplate = new TAPTemplate();
                oTAPTemplate.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAPTemplate;
        }


        public TAPTemplate UpDown(TAPTemplateDetail oTAPTemplateDetail, Int64 nUserID)
        {
            TAPTemplate oTAPTemplate = new TAPTemplate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                TAPTemplateDetailDA.UpDown(tc, oTAPTemplateDetail, false);
                #region Template Get
                reader = TAPTemplateDA.Get(tc, oTAPTemplateDetail.TAPTemplateID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAPTemplate = CreateObject(oReader);
                }
                reader.Close();
                //Detail Gets
                reader = TAPTemplateDetailDA.Gets(oTAPTemplate.TAPTemplateID, tc);
                oTAPTemplate.TAPTemplateDetails = TAPTemplateDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAPTemplate = new TAPTemplate();
                oTAPTemplate.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAPTemplate;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TAPTemplate oTAPTemplate = new TAPTemplate();
                oTAPTemplate.TAPTemplateID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.TAPTemplate, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "TAPTemplate", id);
                TAPTemplateDA.Delete(tc, oTAPTemplate, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TAPTemplate. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TAPTemplate Get(int id, Int64 nUserId)
        {
            TAPTemplate oTAPTemplate = new TAPTemplate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TAPTemplateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAPTemplate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get TAPTemplate", e);
                #endregion
            }

            return oTAPTemplate;
        }


        public List<TAPTemplate> Gets(Int64 nUserID)
        {
            List<TAPTemplate> oTAPTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDA.Gets(tc);
                oTAPTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplate", e);
                #endregion
            }

            return oTAPTemplate;
        }

        public List<TAPTemplate> GetsByTemplateType(int nTemplateType, Int64 nUserID)
        {
            List<TAPTemplate> oTAPTemplate = new List<TAPTemplate>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDA.GetsByTemplateType(nTemplateType, tc);
                oTAPTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplate", e);
                #endregion
            }

            return oTAPTemplate;
        }
        
        public List<TAPTemplate> Gets(string sSQL, Int64 nUserID)
        {
            List<TAPTemplate> oTAPTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPTemplateDA.Gets(tc, sSQL);
                oTAPTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAPTemplate", e);
                #endregion
            }

            return oTAPTemplate;
        }

        #endregion
    }   
    
   
}
