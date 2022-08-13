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
    public class DUReturnChallanService : MarshalByRefObject, IDUReturnChallanService
    {
        #region Private functions and declaration
        private DUReturnChallan MapObject(NullHandler oReader)
        {
            DUReturnChallan oDUReturnChallan = new DUReturnChallan();
            oDUReturnChallan.DUReturnChallanID = oReader.GetInt32("DUReturnChallanID");
            oDUReturnChallan.BUID = oReader.GetInt32("BUID");
            oDUReturnChallan.DUReturnChallanNo = oReader.GetString("DUReturnChallanNo");
            oDUReturnChallan.ReturnDate = oReader.GetDateTime("ReturnDate");
            oDUReturnChallan.DUDeliveryChallanID = oReader.GetInt32("DUDeliveryChallanID");
            oDUReturnChallan.OrderType = oReader.GetInt32("OrderType");
            oDUReturnChallan.PreaperByName = oReader.GetString("PreaperByName");
            oDUReturnChallan.ApprovedByName = oReader.GetString("ApprovedByName");
            oDUReturnChallan.Note = oReader.GetString("Note");
            oDUReturnChallan.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oDUReturnChallan.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oDUReturnChallan.DeliveryChallanNo = oReader.GetString("DeliveryChallanNo");
            oDUReturnChallan.RefChallanNo = oReader.GetString("RefChallanNo");
            oDUReturnChallan.VehicleInfo = oReader.GetString("VehicleInfo");
            //oDUReturnChallan.BUName = oReader.GetString("BUName");
            //oDUReturnChallan.PreaperByName = oReader.GetString("DONo");
            oDUReturnChallan.ContractorName = oReader.GetString("ContractorName");
            oDUReturnChallan.ApprovedByName = oReader.GetString("ApproveByName");
            //oDUReturnChallan.PI_SampleNo = oReader.GetString("PI_SampleNo");
            return oDUReturnChallan;
        }

        private DUReturnChallan CreateObject(NullHandler oReader)
        {
            DUReturnChallan oDUReturnChallan = new DUReturnChallan();
            oDUReturnChallan = MapObject(oReader);
            return oDUReturnChallan;
        }

        private List<DUReturnChallan> CreateObjects(IDataReader oReader)
        {
            List<DUReturnChallan> oDUReturnChallan = new List<DUReturnChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUReturnChallan oItem = CreateObject(oHandler);
                oDUReturnChallan.Add(oItem);
            }
            return oDUReturnChallan;
        }

        #endregion

        #region Interface implementation
        public DUReturnChallan Save(DUReturnChallan oDUReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sDUReturnChallanDetailIDs = "";
            List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
            DUReturnChallanDetail oDUReturnChallanDetail = new DUReturnChallanDetail();
            oDUReturnChallanDetails = oDUReturnChallan.DUReturnChallanDetails;

            try
            {
                tc = TransactionContext.Begin(true);

                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DUReturnChallan, ((nDBOperation == (short)EnumDBOperation.Insert) ? EnumRoleOperationType.Add : EnumRoleOperationType.Edit));
                    IDataReader reader;
                    if (oDUReturnChallan.DUReturnChallanID <= 0)
                    {
                        reader = DUReturnChallanDA.InsertUpdate(tc, oDUReturnChallan, (short)EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = DUReturnChallanDA.InsertUpdate(tc, oDUReturnChallan, (short)EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oDUReturnChallan = new DUReturnChallan();
                        oDUReturnChallan = CreateObject(oReader);
                    }
                    reader.Close();

                    #region  Detail Part
                    foreach (DUReturnChallanDetail oItem in oDUReturnChallanDetails)
                    {
                        IDataReader readerdetail;
                        oItem.DUReturnChallanID = oDUReturnChallan.DUReturnChallanID;
                        if (oItem.DUReturnChallanDetailID <= 0)
                        {
                            readerdetail = DUReturnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = DUReturnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDUReturnChallanDetailIDs = sDUReturnChallanDetailIDs + oReaderDetail.GetString("DUReturnChallanDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sDUReturnChallanDetailIDs.Length > 0)
                    {
                        sDUReturnChallanDetailIDs = sDUReturnChallanDetailIDs.Remove(sDUReturnChallanDetailIDs.Length - 1, 1);
                    }
                    oDUReturnChallanDetail = new DUReturnChallanDetail();
                    oDUReturnChallanDetail.DUReturnChallanID = oDUReturnChallan.DUReturnChallanID;
                    DUReturnChallanDetailDA.Delete(tc, oDUReturnChallanDetail, EnumDBOperation.Delete, nUserID, sDUReturnChallanDetailIDs);
                    #endregion

                 
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oDUReturnChallan = new DUReturnChallan();
                    oDUReturnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDUReturnChallan;
        }

        public DUReturnChallan Get(int id, Int64 nUserId)
        {
            DUReturnChallan oDUReturnChallan = new DUReturnChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = DUReturnChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUReturnChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DUReturnChallan", e);
                #endregion
            }
            return oDUReturnChallan;
        }

        public List<DUReturnChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<DUReturnChallan> oDUReturnChallans = new List<DUReturnChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUReturnChallanDA.Gets(tc, sSQL);
                oDUReturnChallans = CreateObjects(reader);
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
            return oDUReturnChallans;
        }

        public DUReturnChallan Approve(DUReturnChallan oDUReturnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid Return Order
                if (oDUReturnChallan.DUReturnChallanID <= 0)
                {
                    throw new Exception("Invalid Return Challan!");
                }
                if (oDUReturnChallan.ApprovedBy != 0)
                {
                    throw new Exception("Your selected Return Challan already approved!");
                }
                #endregion

                #region Return Challan Approve

                  //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.DUReturnChallan, EnumRoleOperationType.Approved);
                  IDataReader reader = DUReturnChallanDA.InsertUpdate(tc, oDUReturnChallan,(short)EnumDBOperation.Approval,  nUserID);
                  NullHandler oReader = new NullHandler(reader);
                  if (reader.Read())
                  {
                      oDUReturnChallan = CreateObject(oReader);
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
                    oDUReturnChallan = new DUReturnChallan();
                    oDUReturnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oDUReturnChallan;
        }


        public string Delete(DUReturnChallan oDUReturnChallan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                DUReturnChallanDA.Delete(tc, oDUReturnChallan, (short)EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        #endregion
    }

}
