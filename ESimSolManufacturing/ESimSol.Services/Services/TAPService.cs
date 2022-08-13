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

    public class TAPService : MarshalByRefObject, ITAPService
    {
        #region Private functions and declaration
        private TAP MapObject(NullHandler oReader)
        {
            TAP oTAP = new TAP();
            oTAP.TAPID = oReader.GetInt32("TAPID");
            oTAP.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oTAP.PODate = oReader.GetDateTime("PODate");
            oTAP.OrderQty = oReader.GetDouble("OrderQty");
            oTAP.LeadTime = oReader.GetInt32("LeadTime");
            oTAP.ApprovalDuration = oReader.GetInt32("ApprovalDuration");
            oTAP.ProductionLeadTime = oReader.GetInt32("ProductionLeadTime");
            oTAP.FabricLeadTime = oReader.GetInt32("FabricLeadTime");
            oTAP.ProductionTime = oReader.GetInt32("ProductionTime");
            oTAP.BufferingDays = oReader.GetInt32("BufferingDays");
            oTAP.PlanNo = oReader.GetString("PlanNo");
            oTAP.TAPStatus = (EnumTAPStatus) oReader.GetInt32("TAPStatus");
            oTAP.TAPStatusInInt = oReader.GetInt32("TAPStatus");
            oTAP.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oTAP.PlanDate = oReader.GetDateTime("PlanDate");
            oTAP.PlanBy = oReader.GetInt32("PlanBy");
            oTAP.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oTAP.ApprovedByName = oReader.GetString("ApprovedByName");
            oTAP.Remarks = oReader.GetString("Remarks");
            oTAP.StyleNo = oReader.GetString("StyleNo");
            oTAP.BrandName = oReader.GetString("BrandName");
            oTAP.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oTAP.RecapShipmentDate = oReader.GetDateTime("RecapShipmentDate");
            oTAP.FactoryShipmentDate = oReader.GetDateTime("FactoryShipmentDate");
            oTAP.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oTAP.BuyerID = oReader.GetInt32("BuyerID");
            oTAP.BuyerName = oReader.GetString("BuyerName");
            oTAP.OrderDate = oReader.GetDateTime("OrderDate");
            oTAP.Quantity = oReader.GetDouble("Quantity");
            oTAP.FOB = oReader.GetDouble("FOB");
            oTAP.Amount = oReader.GetDouble("Amount");
            oTAP.PlanByName = oReader.GetString("PlanByName");
            oTAP.TAPExecutionID = oReader.GetInt32("TAPExecutionID");
            oTAP.ProductionFactoryID = oReader.GetInt32("ProductionFactoryID");
            oTAP.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oTAP.YarnCategoryName = oReader.GetString("YarnCategoryName");
            oTAP.TSType = (EnumTSType)oReader.GetInt32("TSType");
            oTAP.TampleteName = oReader.GetString("TampleteName");
            oTAP.UnitName = oReader.GetString("UnitName");
            oTAP.MerchandiserName = oReader.GetString("MerchandiserName");
            oTAP.ProductName = oReader.GetString("ProductName");
            oTAP.BUID = oReader.GetInt32("BUID");
            oTAP.TotalTask = oReader.GetDouble("TotalTask");
            oTAP.CompleteTask = oReader.GetDouble("CompleteTask");
            oTAP.PendingTask = oReader.GetDouble("PendingTask");
            oTAP.EstimatedDay = oReader.GetInt32("EstimatedDay");

            return oTAP;
        }

        private TAP CreateObject(NullHandler oReader)
        {
            TAP oTAP = new TAP();
            oTAP = MapObject(oReader);
            return oTAP;
        }

        private List<TAP> CreateObjects(IDataReader oReader)
        {
            List<TAP> oTAP = new List<TAP>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TAP oItem = CreateObject(oHandler);
                oTAP.Add(oItem);
            }
            return oTAP;
        }

        #endregion

        #region Interface implementation
        public TAPService() { }

        public TAP Save(TAP oTAP, Int64 nUserID)
        {
            List<TAPDetail> oTAPDetails = new List<TAPDetail>();
            oTAPDetails = oTAP.TAPDetails;
            string sTAPDetailIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTAP.TAPID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAP, EnumRoleOperationType.Add);
                    reader = TAPDA.InsertUpdate(tc, oTAP, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAP, EnumRoleOperationType.Edit);
                    reader = TAPDA.InsertUpdate(tc, oTAP, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = new TAP();
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                #region TAP Detail Part
                if (oTAPDetails != null)
                {
                    foreach (TAPDetail oItem in oTAPDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TAPID = oTAP.TAPID;
                        if (oItem.TAPDetailID <= 0)
                        {
                            readerdetail = TAPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = TAPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTAPDetailIDs = sTAPDetailIDs + oReaderDetail.GetString("TAPDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTAPDetailIDs.Length > 0)
                    {
                        sTAPDetailIDs = sTAPDetailIDs.Remove(sTAPDetailIDs.Length - 1, 1);
                    }
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "TAPDetail", EnumRoleOperationType.Delete);//request of Shipon vai
                    TAPDetail oTAPDetail = new TAPDetail();
                    oTAPDetail.TAPID = oTAP.TAPID;
                    TAPDetailDA.Delete(tc, oTAPDetail, EnumDBOperation.Delete, nUserID, sTAPDetailIDs);
                }

                #endregion
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAP = new TAP();
                oTAP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAP;
        }

        public TAP AcceptRevise(TAP oTAP, Int64 nUserID)
        {
            List<TAPDetail> oTAPDetails = new List<TAPDetail>();
            oTAPDetails = oTAP.TAPDetails;
            string sTAPDetailIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAP, EnumRoleOperationType.Revise);
                reader = TAPDA.AcceptRevise(tc, oTAP, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = new TAP();
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                #region TAP Detail Part
                if (oTAPDetails != null)
                {
                    foreach (TAPDetail oItem in oTAPDetails)
                    {
                        if (!oItem.ExecutionIsDone)
                        {
                            IDataReader readerdetail;
                            oItem.TAPID = oTAP.TAPID;
                            if (oItem.TAPDetailID <= 0)
                            {
                                readerdetail = TAPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = TAPDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sTAPDetailIDs = sTAPDetailIDs + oReaderDetail.GetString("TAPDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        else
                        {
                            sTAPDetailIDs = sTAPDetailIDs + oItem.TAPDetailID.ToString() + ",";
                        }
                    }
                    if (sTAPDetailIDs.Length > 0)
                    {
                        sTAPDetailIDs = sTAPDetailIDs.Remove(sTAPDetailIDs.Length - 1, 1);
                    }
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "TAPDetail", EnumRoleOperationType.Delete);//request of Shipon vai
                    TAPDetail oTAPDetail = new TAPDetail();
                    oTAPDetail.TAPID = oTAP.TAPID;
                    TAPDetailDA.Delete(tc, oTAPDetail, EnumDBOperation.Delete, nUserID, sTAPDetailIDs);
                }

                #endregion

                //#region SEt Sequence
                //TAPDetail oNewTAPDetail = new TAPDetail();
                //oNewTAPDetail.TAPID = oTAP.TAPID;
                //TAPDetailDA.UpDown(tc, oNewTAPDetail, true);
                //#endregion

                #region Get TAP With Detail
                reader = TAPDA.RearangeTAPSequence(tc, oTAP.TAPID);//Rearange TaP Sequence
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = new TAP();
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                reader = TAPDetailDA.Gets(oTAP.TAPID, tc);
                oTAP.TAPDetails = TAPDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAP = new TAP();
                oTAP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAP;
        }

        public TAP SaveFactoryTAP(TAP oTAP, Int64 nUserID)
        {
            List<TAPDetail> oTAPDetails = new List<TAPDetail>();
            oTAPDetails = oTAP.TAPDetails;
            string sTAPDetailIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTAP.TAPID <= 0)
                {

                    oTAP.TAPID = oTAP.nPriviousTAPID;//if not get from factory tap 
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAP, EnumRoleOperationType.Add);
                    reader = TAPDA.InsertUpdateSaveFactoryTAP(tc, oTAP, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAP, EnumRoleOperationType.Edit);
                    reader = TAPDA.InsertUpdateSaveFactoryTAP(tc, oTAP, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = new TAP();
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                #region TAP Detail Part
                if (oTAPDetails != null)
                {
                    foreach (TAPDetail oItem in oTAPDetails)
                    {
                        IDataReader readerdetail;
                        oItem.TAPID = oTAP.TAPID;
                        if (oItem.TAPDetailID <= 0)
                        {
                            readerdetail = TAPDetailDA.InsertUpdateFactoryTAP(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = TAPDetailDA.InsertUpdateFactoryTAP(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sTAPDetailIDs = sTAPDetailIDs + oReaderDetail.GetString("TAPDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sTAPDetailIDs.Length > 0)
                    {
                        sTAPDetailIDs = sTAPDetailIDs.Remove(sTAPDetailIDs.Length - 1, 1);
                    }
                    TAPDetail oTAPDetail = new TAPDetail();
                    oTAPDetail.TAPID = oTAP.TAPID;
                    TAPDetailDA.DeleteFactoryTAP(tc, oTAPDetail, EnumDBOperation.Delete, nUserID, sTAPDetailIDs);
                }

                #endregion

                #region SEt Sequence
                TAPDetail oNewTAPDetail = new TAPDetail();
                oNewTAPDetail.TAPID = oTAP.TAPID;
                TAPDetailDA.UpDownFactoryTAP(tc, oNewTAPDetail, true);
                #endregion
                #region Get TAP With Detail
                reader = TAPDA.GetFactoryTAP(tc, oTAP.TAPID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = new TAP();
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                reader = TAPDetailDA.FactoryTAPGets(oTAP.TAPID, tc);
                oTAP.TAPDetails = TAPDetailService.CreateObjects(reader);
                reader.Close();
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAP = new TAP();
                oTAP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAP;
        }

        public TAP UpDown(TAPDetail oTAPDetail, Int64 nUserID)
        {
            TAP oTAP = new TAP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                TAPDetailDA.UpDown(tc, oTAPDetail, false);
                #region Template Get
                reader = TAPDA.Get(tc, oTAPDetail.TAPID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                //Detail Gets
                reader = TAPDetailDA.Gets(oTAP.TAPID, tc);
                oTAP.TAPDetails = TAPDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAP = new TAP();
                oTAP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAP;
        }
        public TAP UpdateApprovePlanDate(TAPDetail oTAPDetail, Int64 nUserID)
        {
            TAP oTAP = new TAP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                TAPDetailDA.UpdateApprovePlanDate(tc, oTAPDetail);
                #region Template Get
                reader = TAPDA.Get(tc, oTAPDetail.TAPID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                //Detail Gets
                reader = TAPDetailDA.Gets(oTAP.TAPID, tc);
                oTAP.TAPDetails = TAPDetailService.CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTAP = new TAP();
                oTAP.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oTAP;
        }
        

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                TAP oTAP = new TAP();
                oTAP.TAPID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.TAP, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, EnumModuleName.TAP.ToString(), id);
                TAPDA.Delete(tc, oTAP, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete TAP. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public TAP ChangeStatus(TAP oTAP, ApprovalRequest oApprovalRequest, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTAP.TAPActionType == EnumTAPActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.TAP, EnumRoleOperationType.Approved);
                }
                
                reader = TAPDA.ChangeStatus(tc, oTAP, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTAP = new TAP();
                    oTAP = CreateObject(oReader);
                }
                reader.Close();
                if (oTAP.TAPStatus == EnumTAPStatus.Request_for_Revise)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                }
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
                oTAP.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save TAPDetail. Because of " + e.Message, e);
                #endregion
            }
            return oTAP;
        }

        public TAP Get(int id, Int64 nUserId)
        {
            TAP oAccountHead = new TAP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TAPDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get TAP", e);
                #endregion
            }

            return oAccountHead;
        }


        public TAP GetByRecap(int ORID, Int64 nUserId)
        {
            TAP oAccountHead = new TAP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TAPDA.GetByRecap(tc, ORID);
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
                throw new ServiceException("Failed to Get TAP", e);
                #endregion
            }

            return oAccountHead;
        }

        public TAP GetByHIA(int nHIAID, Int64 nUserId)
        {
            TAP oAccountHead = new TAP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TAPDA.GetByHIA(tc, nHIAID);
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
                throw new ServiceException("Failed to Get TAP", e);
                #endregion
            }

            return oAccountHead;
        }
        public TAP GetFactoryTAP(int id, Int64 nUserId)
        {
            TAP oAccountHead = new TAP();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TAPDA.GetFactoryTAP(tc, id);
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
                throw new ServiceException("Failed to Get TAP", e);
                #endregion
            }

            return oAccountHead;
        }
   
        public List<TAP> Gets(Int64 nUserID)
        {
            List<TAP> oTAP = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPDA.Gets(tc);
                oTAP = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAP", e);
                #endregion
            }

            return oTAP;
        }

        public List<TAP> Gets(string sSQL, Int64 nUserID)
        {
            List<TAP> oTAP = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TAPDA.Gets(tc, sSQL);
                oTAP = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TAP", e);
                #endregion
            }

            return oTAP;
        }

        #endregion
    }   
    
    
   
}
