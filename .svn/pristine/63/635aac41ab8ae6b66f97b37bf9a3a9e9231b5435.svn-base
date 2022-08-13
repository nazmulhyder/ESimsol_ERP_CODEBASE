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
    public class ReturnChallanService : MarshalByRefObject, IReturnChallanService
    {
        #region Private functions and declaration
        private ReturnChallan MapObject(NullHandler oReader)
        {
            ReturnChallan oReturnChallan = new ReturnChallan();
            oReturnChallan.ReturnChallanID = oReader.GetInt32("ReturnChallanID");
            oReturnChallan.BUID = oReader.GetInt32("BUID");
            oReturnChallan.ReturnChallanNo = oReader.GetString("ReturnChallanNo");
            oReturnChallan.ReturnDate = oReader.GetDateTime("ReturnDate");
            oReturnChallan.ContractorID = oReader.GetInt32("ContractorID");
            oReturnChallan.ReceivedByName = oReader.GetString("ReceivedByName");
            oReturnChallan.ApprovedByName = oReader.GetString("ApprovedByName");
            oReturnChallan.Note = oReader.GetString("Note");
            oReturnChallan.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oReturnChallan.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oReturnChallan.BUName = oReader.GetString("BUName");
            oReturnChallan.DONo = oReader.GetString("DONo");
            oReturnChallan.ContractorName = oReader.GetString("ContractorName");
            oReturnChallan.PINo = oReader.GetString("PINo");   
            oReturnChallan.ProductNature =  (EnumProductNature)oReader.GetInt32("ProductNature");
            oReturnChallan.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oReturnChallan.ExportSCID = oReader.GetInt32("ExportSCID");
            oReturnChallan.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oReturnChallan.StoreName = oReader.GetString("StoreName");

            return oReturnChallan;
        }

        private ReturnChallan CreateObject(NullHandler oReader)
        {
            ReturnChallan oReturnChallan = new ReturnChallan();
            oReturnChallan = MapObject(oReader);
            return oReturnChallan;
        }

        private List<ReturnChallan> CreateObjects(IDataReader oReader)
        {
            List<ReturnChallan> oReturnChallan = new List<ReturnChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReturnChallan oItem = CreateObject(oHandler);
                oReturnChallan.Add(oItem);
            }
            return oReturnChallan;
        }

        #endregion

        #region Interface implementation
        public ReturnChallan IUD(ReturnChallan oReturnChallan, short nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sReturnChallanDetailIDs = "";
            List<ReturnChallanDetail> oReturnChallanDetails = new List<ReturnChallanDetail>();
            ReturnChallanDetail oReturnChallanDetail = new ReturnChallanDetail();
            oReturnChallanDetails = oReturnChallan.ReturnChallanDetails;

            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (short)EnumDBOperation.Insert || nDBOperation == (short)EnumDBOperation.Update)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReturnChallan, ((nDBOperation == (short)EnumDBOperation.Insert) ? EnumRoleOperationType.Add : EnumRoleOperationType.Edit));
                    IDataReader reader;
                    reader = ReturnChallanDA.InsertUpdate(tc, oReturnChallan, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oReturnChallan = new ReturnChallan();
                        oReturnChallan = CreateObject(oReader);
                    }
                    reader.Close();

                    #region Return Challan Detail 
                    foreach (ReturnChallanDetail oItem in oReturnChallanDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ReturnChallanID = oReturnChallan.ReturnChallanID;
                        if (oItem.ReturnChallanDetailID <= 0)
                        {
                            readerdetail = ReturnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ReturnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sReturnChallanDetailIDs = sReturnChallanDetailIDs + oReaderDetail.GetString("ReturnChallanDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sReturnChallanDetailIDs.Length > 0)
                    {
                        sReturnChallanDetailIDs = sReturnChallanDetailIDs.Remove(sReturnChallanDetailIDs.Length - 1, 1);
                    }
                    oReturnChallanDetail = new ReturnChallanDetail();
                    oReturnChallanDetail.ReturnChallanID = oReturnChallan.ReturnChallanID;
                    ReturnChallanDetailDA.Delete(tc, oReturnChallanDetail, EnumDBOperation.Delete, nUserID, sReturnChallanDetailIDs);
                    #endregion

                    #region Get Production Order
                    reader = ReturnChallanDA.Get(tc, oReturnChallan.ReturnChallanID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oReturnChallan = new ReturnChallan();
                        oReturnChallan = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReturnChallan, EnumRoleOperationType.Delete);
                    ReturnChallanDA.Delete(tc, oReturnChallan, nDBOperation, nUserID);
                    oReturnChallan = new ReturnChallan();
                    oReturnChallan.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oReturnChallan = new ReturnChallan();
                    oReturnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oReturnChallan;
        }

        public ReturnChallan Get(int id, Int64 nUserId)
        {
            ReturnChallan oReturnChallan = new ReturnChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ReturnChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReturnChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ReturnChallan", e);
                #endregion
            }
            return oReturnChallan;
        }

        public List<ReturnChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<ReturnChallan> oReturnChallans = new List<ReturnChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ReturnChallanDA.Gets(tc, sSQL);
                oReturnChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Return Order", e);
                #endregion
            }
            return oReturnChallans;
        }

        public ReturnChallan Approve(ReturnChallan oReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid Return Order
                if (oReturnChallan.ReturnChallanID <= 0)
                {
                    throw new Exception("Invalid Return Challan!");
                }
               
                #endregion

                #region Return Challan Approve

                  AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReturnChallan, EnumRoleOperationType.Approved);
                  IDataReader reader = ReturnChallanDA.InsertUpdate(tc, oReturnChallan,(short)EnumDBOperation.Approval,  nUserID);
                  NullHandler oReader = new NullHandler(reader);
                  if (reader.Read())
                  {
                      oReturnChallan = CreateObject(oReader);
                  }
                  reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oReturnChallan = new ReturnChallan();
                    oReturnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oReturnChallan;
        }


        public ReturnChallan Receive(ReturnChallan oReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid Return Order
                if (oReturnChallan.ReturnChallanID <= 0)
                {
                    throw new Exception("Invalid Return order!");
                }
                if (oReturnChallan.ReceivedBy != 0)
                {
                    throw new Exception("Your selected Return order already approved!");
                }
                #endregion

                #region Return Challan Approve

                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ReturnChallan, EnumRoleOperationType.Approved);
                IDataReader reader = ReturnChallanDA.Receive(tc, oReturnChallan.ReturnChallanID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oReturnChallan = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oReturnChallan = new ReturnChallan();
                    oReturnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oReturnChallan;
        }

        #endregion
    }

}
