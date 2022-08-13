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

    public class PackageTemplateService : MarshalByRefObject, IPackageTemplateService
    {
        #region Private functions and declaration
        private PackageTemplate MapObject(NullHandler oReader)
        {
            PackageTemplate oPackageTemplate = new PackageTemplate();
            oPackageTemplate.PackageTemplateID = oReader.GetInt32("PackageTemplateID");
            oPackageTemplate.PackageNo = oReader.GetString("PackageNo");
            oPackageTemplate.PackageName = oReader.GetString("PackageName");
            oPackageTemplate.Note = oReader.GetString("Note");
            oPackageTemplate.BUID = oReader.GetInt32("BUID");
            return oPackageTemplate;
        }

        private PackageTemplate CreateObject(NullHandler oReader)
        {
            PackageTemplate oPackageTemplate = new PackageTemplate();
            oPackageTemplate = MapObject(oReader);
            return oPackageTemplate;
        }

        private List<PackageTemplate> CreateObjects(IDataReader oReader)
        {
            List<PackageTemplate> oPackageTemplate = new List<PackageTemplate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PackageTemplate oItem = CreateObject(oHandler);
                oPackageTemplate.Add(oItem);
            }
            return oPackageTemplate;
        }

        #endregion

        #region Interface implementation
        public PackageTemplateService() { }


        public PackageTemplate Save(PackageTemplate oPackageTemplate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                List<PackageTemplateDetail> oPackageTemplateDetails = new List<PackageTemplateDetail>();
                PackageTemplateDetail oPackageTemplateDetail = new PackageTemplateDetail();
                oPackageTemplateDetails = oPackageTemplate.PackageTemplateDetails;
                string sPackageTemplateDetailIDs = "";

                #region Package Template part
                IDataReader reader;
                if (oPackageTemplate.PackageTemplateID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PackageTemplate, EnumRoleOperationType.Add);
                    reader = PackageTemplateDA.InsertUpdate(tc, oPackageTemplate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PackageTemplate, EnumRoleOperationType.Edit);
                    reader = PackageTemplateDA.InsertUpdate(tc, oPackageTemplate, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPackageTemplate = new PackageTemplate();
                    oPackageTemplate = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Package Template Detail Part
                if (oPackageTemplateDetails != null)
                {
                    foreach (PackageTemplateDetail oItem in oPackageTemplateDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PackageTemplateID = oPackageTemplate.PackageTemplateID;
                        if (oItem.PackageTemplateDetailID <= 0)
                        {
                            readerdetail = PackageTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PackageTemplateDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPackageTemplateDetailIDs = sPackageTemplateDetailIDs + oReaderDetail.GetString("PackageTemplateDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPackageTemplateDetailIDs.Length > 0)
                    {
                        sPackageTemplateDetailIDs = sPackageTemplateDetailIDs.Remove(sPackageTemplateDetailIDs.Length - 1, 1);
                    }

                }
                #endregion

                oPackageTemplateDetail = new PackageTemplateDetail();
                oPackageTemplateDetail.PackageTemplateID = oPackageTemplate.PackageTemplateID;
                PackageTemplateDetailDA.Delete(tc, oPackageTemplateDetail, EnumDBOperation.Delete, nUserID, sPackageTemplateDetailIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oPackageTemplate.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PackageTemplate. Because of " + e.Message, e);
                #endregion
            }
            return oPackageTemplate;
        }


      


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PackageTemplate oPackageTemplate = new PackageTemplate();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PackageTemplate, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "PackageTemplate", id);
                oPackageTemplate.PackageTemplateID = id;
                PackageTemplateDA.Delete(tc, oPackageTemplate, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete PackageTemplate. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public PackageTemplate Get(int id, Int64 nUserId)
        {
            PackageTemplate oAccountHead = new PackageTemplate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PackageTemplateDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PackageTemplate", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<PackageTemplate> Gets(Int64 nUserID)
        {
            List<PackageTemplate> oPackageTemplate = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PackageTemplateDA.Gets(tc);
                oPackageTemplate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PackageTemplate", e);
                #endregion
            }

            return oPackageTemplate;
        }

        public List<PackageTemplate> Gets(string sSQL, Int64 nUserID)
        {
            List<PackageTemplate> oPackageTemplates = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PackageTemplateDA.Gets(tc, sSQL);
                oPackageTemplates = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Order Negotiation Sheet", e);
                #endregion
            }

            return oPackageTemplates;
        }

        #endregion
    }
    

}
