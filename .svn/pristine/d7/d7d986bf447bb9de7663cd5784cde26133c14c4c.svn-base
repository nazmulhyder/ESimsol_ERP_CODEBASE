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

    public class CostSheetPackageService : MarshalByRefObject, ICostSheetPackageService
    {
        #region Private functions and declaration
        private CostSheetPackage MapObject(NullHandler oReader)
        {
            CostSheetPackage oCostSheetPackage = new CostSheetPackage();
            oCostSheetPackage.CostSheetPackageID = oReader.GetInt32("CostSheetPackageID");
            oCostSheetPackage.CostSheetID = oReader.GetInt32("CostSheetID");
            oCostSheetPackage.PackageName = oReader.GetString("PackageName");
            oCostSheetPackage.Price = oReader.GetDouble("Price");
            oCostSheetPackage.Remark = oReader.GetString("Remark");
            return oCostSheetPackage;
        }

        private CostSheetPackage CreateObject(NullHandler oReader)
        {
            CostSheetPackage oCostSheetPackage = new CostSheetPackage();
            oCostSheetPackage = MapObject(oReader);
            return oCostSheetPackage;
        }

        private List<CostSheetPackage> CreateObjects(IDataReader oReader)
        {
            List<CostSheetPackage> oCostSheetPackage = new List<CostSheetPackage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostSheetPackage oItem = CreateObject(oHandler);
                oCostSheetPackage.Add(oItem);
            }
            return oCostSheetPackage;
        }

        #endregion

        #region Interface implementation
        public CostSheetPackageService() { }


        public CostSheetPackage Save(CostSheetPackage oCostSheetPackage, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                List<CostSheetPackageDetail> oCostSheetPackageDetails = new List<CostSheetPackageDetail>();
                CostSheetPackageDetail oCostSheetPackageDetail = new CostSheetPackageDetail();
                oCostSheetPackageDetails = oCostSheetPackage.CostSheetPackageDetails;
                string sCostSheetPackageDetailIDs = "";

                #region Package Template part
                IDataReader reader;
                if (oCostSheetPackage.CostSheetPackageID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostSheetPackage, EnumRoleOperationType.Add);
                    reader = CostSheetPackageDA.InsertUpdate(tc, oCostSheetPackage, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CostSheetPackage, EnumRoleOperationType.Edit);
                    reader = CostSheetPackageDA.InsertUpdate(tc, oCostSheetPackage, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSheetPackage = new CostSheetPackage();
                    oCostSheetPackage = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Package Template Detail Part
                if (oCostSheetPackageDetails != null)
                {
                    foreach (CostSheetPackageDetail oItem in oCostSheetPackageDetails)
                    {
                        IDataReader readerdetail;
                        oItem.CostSheetPackageID = oCostSheetPackage.CostSheetPackageID;
                        if (oItem.CostSheetPackageDetailID <= 0)
                        {
                            readerdetail = CostSheetPackageDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = CostSheetPackageDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sCostSheetPackageDetailIDs = sCostSheetPackageDetailIDs + oReaderDetail.GetString("CostSheetPackageDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sCostSheetPackageDetailIDs.Length > 0)
                    {
                        sCostSheetPackageDetailIDs = sCostSheetPackageDetailIDs.Remove(sCostSheetPackageDetailIDs.Length - 1, 1);
                    }

                }
                #endregion

                oCostSheetPackageDetail = new CostSheetPackageDetail();
                oCostSheetPackageDetail.CostSheetPackageID = oCostSheetPackage.CostSheetPackageID;
                CostSheetPackageDetailDA.Delete(tc, oCostSheetPackageDetail, EnumDBOperation.Delete, nUserID);
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
                oCostSheetPackage.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save CostSheetPackage. Because of " + e.Message, e);
                #endregion
            }
            return oCostSheetPackage;
        }





        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CostSheetPackage oCostSheetPackage = new CostSheetPackage();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CostSheetPackage, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "CostSheetPackage", id);
                oCostSheetPackage.CostSheetPackageID = id;
                CostSheetPackageDA.Delete(tc, oCostSheetPackage, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete CostSheetPackage. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public CostSheetPackage Get(int id, Int64 nUserId)
        {
            CostSheetPackage oAccountHead = new CostSheetPackage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostSheetPackageDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CostSheetPackage", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CostSheetPackage> Gets(Int64 nUserID)
        {
            List<CostSheetPackage> oCostSheetPackage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetPackageDA.Gets(tc);
                oCostSheetPackage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetPackage", e);
                #endregion
            }

            return oCostSheetPackage;
        }

        public List<CostSheetPackage> Gets(int id, Int64 nUserID)
        {
            List<CostSheetPackage> oCostSheetPackage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetPackageDA.Gets(id,tc);
                oCostSheetPackage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetPackage", e);
                #endregion
            }

            return oCostSheetPackage;
        }

        public List<CostSheetPackage> Gets(string sSQL, Int64 nUserID)
        {
            List<CostSheetPackage> oCostSheetPackages = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetPackageDA.Gets(tc, sSQL);
                oCostSheetPackages = CreateObjects(reader);
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

            return oCostSheetPackages;
        }

        #endregion
    }
    
   
}
