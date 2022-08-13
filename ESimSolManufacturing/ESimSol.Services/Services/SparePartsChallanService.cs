using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class SparePartsChallanService : MarshalByRefObject, ISparePartsChallanService
    {
        #region Private functions and declaration
        private SparePartsChallan MapObject(NullHandler oReader)
        {
            SparePartsChallan oSparePartsChallan = new SparePartsChallan();
            oSparePartsChallan.SparePartsChallanID = oReader.GetInt32("SparePartsChallanID");
            oSparePartsChallan.SparePartsRequisitionID = oReader.GetInt32("SparePartsRequisitionID");
            oSparePartsChallan.ChallanNo = oReader.GetString("ChallanNo");
            oSparePartsChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oSparePartsChallan.SparePartsChallanID = oReader.GetInt32("SparePartsChallanID");
            oSparePartsChallan.StoreID = oReader.GetInt32("StoreID");
            oSparePartsChallan.StoreName = oReader.GetString("StoreName");
            oSparePartsChallan.DisburseBy = oReader.GetInt32("DisburseBy");
            oSparePartsChallan.DisburseByName = oReader.GetString("DisburseByName");
            oSparePartsChallan.Remarks = oReader.GetString("Remarks");
            oSparePartsChallan.CapitalResourceName = oReader.GetString("CapitalResourceName");
            return oSparePartsChallan;
        }

        private SparePartsChallan CreateObject(NullHandler oReader)
        {
            SparePartsChallan oSparePartsChallan = new SparePartsChallan();
            oSparePartsChallan = MapObject(oReader);
            return oSparePartsChallan;
        }

        private List<SparePartsChallan> CreateObjects(IDataReader oReader)
        {
            List<SparePartsChallan> oSparePartsChallan = new List<SparePartsChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SparePartsChallan oItem = CreateObject(oHandler);
                oSparePartsChallan.Add(oItem);
            }
            return oSparePartsChallan;
        }

        #endregion

        #region Interface implementation
        public SparePartsChallanService() { }
        public SparePartsChallan Save(SparePartsChallan oSparePartsChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<SparePartsChallanDetail> oSparePartsChallanDetails = new List<SparePartsChallanDetail>();
            string sSparePartsChallanDetailIDs = "";
            oSparePartsChallanDetails = oSparePartsChallan.SparePartsChallanDetails;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSparePartsChallan.SparePartsChallanID <= 0)
                {

                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsChallan, EnumRoleOperationType.Add);
                    reader = SparePartsChallanDA.InsertUpdate(tc, oSparePartsChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsChallan, EnumRoleOperationType.Edit);
                    //VoucherDA.CheckVoucherReference(tc, "SparePartsChallan", "SparePartsChallanID", oSparePartsChallan.SparePartsChallanID);
                    reader = SparePartsChallanDA.InsertUpdate(tc, oSparePartsChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsChallan = new SparePartsChallan();
                    oSparePartsChallan = CreateObject(oReader);
                }
                reader.Close();

                #region CR Detail Part
                if (oSparePartsChallanDetails != null)
                {
                    foreach (SparePartsChallanDetail oItem in oSparePartsChallanDetails)
                    {
                        if (oItem.LotID > 0)
                        {
                            IDataReader readerdetail;
                            oItem.SparePartsChallanID = oSparePartsChallan.SparePartsChallanID;
                            if (oItem.SparePartsChallanDetailID <= 0)
                            {
                                readerdetail = SparePartsChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = SparePartsChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sSparePartsChallanDetailIDs = sSparePartsChallanDetailIDs + oReaderDetail.GetString("SparePartsChallanDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sSparePartsChallanDetailIDs.Length > 0)
                    {
                        sSparePartsChallanDetailIDs = sSparePartsChallanDetailIDs.Remove(sSparePartsChallanDetailIDs.Length - 1, 1);
                    }
                    SparePartsChallanDetail oSparePartsChallanDetail = new SparePartsChallanDetail();
                    oSparePartsChallanDetail.SparePartsChallanID = oSparePartsChallan.SparePartsChallanID;
                    SparePartsChallanDetailDA.Delete(tc, oSparePartsChallanDetail, EnumDBOperation.Delete, nUserID, sSparePartsChallanDetailIDs);

                }

                #endregion

                #region Again Get CR
                reader = SparePartsChallanDA.Get(tc, oSparePartsChallan.SparePartsChallanID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsChallan = CreateObject(oReader);
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

                oSparePartsChallan = new SparePartsChallan();
                oSparePartsChallan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSparePartsChallan;
        }
        public SparePartsChallan Disburse(SparePartsChallan oSparePartsChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<SparePartsChallanDetail> oSparePartsChallanDetails = new List<SparePartsChallanDetail>();
            oSparePartsChallanDetails = oSparePartsChallan.SparePartsChallanDetails;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
          
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SparePartsChallan, EnumRoleOperationType.Edit);
                    //VoucherDA.CheckVoucherReference(tc, "SparePartsChallan", "SparePartsChallanID", oSparePartsChallan.SparePartsChallanID);
                reader = SparePartsChallanDA.Disburse(tc, oSparePartsChallan, EnumDBOperation.Disburse, nUserID);
                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsChallan = new SparePartsChallan();
                    oSparePartsChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSparePartsChallan = new SparePartsChallan();
                oSparePartsChallan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oSparePartsChallan;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SparePartsChallan oSparePartsChallan = new SparePartsChallan();
                oSparePartsChallan.SparePartsChallanID = id;
                SparePartsChallanDA.Delete(tc, oSparePartsChallan, EnumDBOperation.Delete, nUserId);
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
        public SparePartsChallan Get(int id, Int64 nUserId)
        {
            SparePartsChallan oSparePartsChallan = new SparePartsChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SparePartsChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get SparePartsChallan", e);
                #endregion
            }
            return oSparePartsChallan;
        }
        public List<SparePartsChallan> Gets(Int64 nUserID)
        {
            List<SparePartsChallan> oSparePartsChallans = new List<SparePartsChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsChallanDA.Gets(tc);
                oSparePartsChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsChallan", e);
                #endregion
            }
            return oSparePartsChallans;
        }
        public List<SparePartsChallan> Gets(string sSQL,Int64 nUserID)
        {
            List<SparePartsChallan> oSparePartsChallans = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsChallanDA.Gets(tc,sSQL);
                oSparePartsChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsChallan", e);
                #endregion
            }
            return oSparePartsChallans;
        }
        #endregion
    }   
}