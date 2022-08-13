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

    public class RMRequisitionService : MarshalByRefObject, IRMRequisitionService
    {
        #region Private functions and declaration

        private static RMRequisition MapObject(NullHandler oReader)
        {
            RMRequisition oRMRequisition = new RMRequisition();
            oRMRequisition.RMRequisitionID = oReader.GetInt32("RMRequisitionID");
            oRMRequisition.RefNo = oReader.GetString("RefNo");
            oRMRequisition.BUID = oReader.GetInt32("BUID");
            oRMRequisition.RequisitionDate = oReader.GetDateTime("RequisitionDate");
            oRMRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oRMRequisition.Remarks = oReader.GetString("Remarks");
            oRMRequisition.PreparedByName = oReader.GetString("PreparedByName");
            oRMRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oRMRequisition.PINo = oReader.GetString("PINo");
            
            return oRMRequisition;
        }

        public static RMRequisition CreateObject(NullHandler oReader)
        {
            RMRequisition oRMRequisition = new RMRequisition();
            oRMRequisition = MapObject(oReader);
            return oRMRequisition;
        }

        private List<RMRequisition> CreateObjects(IDataReader oReader)
        {
            List<RMRequisition> oRMRequisition = new List<RMRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RMRequisition oItem = CreateObject(oHandler);
                oRMRequisition.Add(oItem);
            }
            return oRMRequisition;
        }

        #endregion

        #region Interface implementation
        public RMRequisition Save(RMRequisition oRMRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<RMRequisitionSheet> oRMRequisitionSheets = new List<RMRequisitionSheet>();
                List<RMRequisitionMaterial> oRMRequisitionMaterials = new List<RMRequisitionMaterial>();
                RMRequisitionMaterial oRMRequisitionMaterial = new RMRequisitionMaterial();
                RMRequisitionSheet oRMRequisitionSheet = new RMRequisitionSheet();
                oRMRequisitionSheets = oRMRequisition.RMRequisitionSheets;
                oRMRequisitionMaterials = oRMRequisition.RMRequisitionMaterials;
                string sRMRequisitionSheetIDs = ""; string sRMRequisitionMaterialIDs = "";
                int nTempRMRequisitionMaterialID = 0;

                #region RMRequisition part
                IDataReader reader;
                if (oRMRequisition.RMRequisitionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RMRequisition, EnumRoleOperationType.Add);
                    reader = RMRequisitionDA.InsertUpdate(tc, oRMRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RMRequisition, EnumRoleOperationType.Edit);
                    reader = RMRequisitionDA.InsertUpdate(tc, oRMRequisition, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisition = new RMRequisition();
                    oRMRequisition = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region RMRequisition  Sheet  Part
                if (oRMRequisitionSheets != null)
                {
                    foreach (RMRequisitionSheet oItem in oRMRequisitionSheets)
                    {
                        IDataReader readerdetail;
                        oItem.RMRequisitionID = oRMRequisition.RMRequisitionID;
                        if (oItem.RMRequisitionSheetID <= 0)
                        {
                            readerdetail = RMRequisitionSheetDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = RMRequisitionSheetDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sRMRequisitionSheetIDs = sRMRequisitionSheetIDs + oReaderDetail.GetString("RMRequisitionSheetID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sRMRequisitionSheetIDs.Length > 0)
                    {
                        sRMRequisitionSheetIDs = sRMRequisitionSheetIDs.Remove(sRMRequisitionSheetIDs.Length - 1, 1);
                    }
                    oRMRequisitionSheet = new RMRequisitionSheet();
                    oRMRequisitionSheet.RMRequisitionID = oRMRequisition.RMRequisitionID;
                    RMRequisitionSheetDA.Delete(tc, oRMRequisitionSheet, EnumDBOperation.Delete, nUserID, sRMRequisitionSheetIDs);

                }

                #endregion

                #region  RMRequisition Material Part
                if (oRMRequisitionMaterials != null)
                {
                    foreach (RMRequisitionMaterial oItem in oRMRequisitionMaterials)
                    {
                        IDataReader readerPackage;
                        oItem.RMRequisitionID = oRMRequisition.RMRequisitionID;
                        if (oItem.RMRequisitionMaterialID <= 0)
                        {
                            readerPackage = RMRequisitionMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                        }
                        else
                        {
                            readerPackage = RMRequisitionMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderPackage = new NullHandler(readerPackage);
                        if (readerPackage.Read())
                        {
                            nTempRMRequisitionMaterialID = oReaderPackage.GetInt32("RMRequisitionMaterialID");
                            sRMRequisitionMaterialIDs = sRMRequisitionMaterialIDs + oReaderPackage.GetString("RMRequisitionMaterialID") + ",";
                        }
                        readerPackage.Close();
                    }
                    if (sRMRequisitionMaterialIDs.Length > 0)
                    {
                        sRMRequisitionMaterialIDs = sRMRequisitionMaterialIDs.Remove(sRMRequisitionMaterialIDs.Length - 1, 1);
                    }
                    oRMRequisitionMaterial = new RMRequisitionMaterial();
                    oRMRequisitionMaterial.RMRequisitionID = oRMRequisition.RMRequisitionID;
                    RMRequisitionMaterialDA.Delete(tc, oRMRequisitionMaterial, EnumDBOperation.Delete, nUserID, sRMRequisitionMaterialIDs);

                }

                #endregion

                #region RM Requisition Get
                reader = RMRequisitionDA.Get(tc, oRMRequisition.RMRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisition = CreateObject(oReader);
                }
                reader.Close();
                #endregion

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
                oRMRequisition.ErrorMessage = Message;

                #endregion
            }
            return oRMRequisition;
        }
        public RMRequisition Approve(RMRequisition oRMRequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
             

                #region RM Requistion part
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.RMRequisition, EnumRoleOperationType.Approved);
                reader = RMRequisitionDA.InsertUpdate(tc, oRMRequisition, EnumDBOperation.Approval, nUserID);
      
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisition = new RMRequisition();
                    oRMRequisition = CreateObject(oReader);
                }
                reader.Close();
                #endregion


                #region RM Requisition Get
                reader = RMRequisitionDA.Get(tc, oRMRequisition.RMRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisition = CreateObject(oReader);
                }
                reader.Close();
                #endregion

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
                oRMRequisition.ErrorMessage = Message;

                #endregion
            }
            return oRMRequisition;
        }
        public string Delete(int nRMRequisitionID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RMRequisition oRMRequisition = new RMRequisition();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.RMRequisition, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "RMRequisition", nRMRequisitionID);
                oRMRequisition.RMRequisitionID = nRMRequisitionID;
                RMRequisitionDA.Delete(tc, oRMRequisition, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public RMRequisition Get(int id, Int64 nUserId)
        {
            RMRequisition oRMRequisition = new RMRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = RMRequisitionDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRMRequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get RMRequisition", e);
                #endregion
            }
            return oRMRequisition;
        }
        public List<RMRequisition> BUWiseGets(int nBUID, Int64 nUserID)
        {
            List<RMRequisition> oRMRequisitions = new List<RMRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMRequisitionDA.BUWiseGets(nBUID, tc);
                oRMRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                RMRequisition oRMRequisition = new RMRequisition();
                oRMRequisition.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oRMRequisitions;
        }
        public List<RMRequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<RMRequisition> oRMRequisitions = new List<RMRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RMRequisitionDA.Gets(tc, sSQL);
                oRMRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RMRequisition", e);
                #endregion
            }
            return oRMRequisitions;
        }
   
        #endregion
    }
    
   
}
