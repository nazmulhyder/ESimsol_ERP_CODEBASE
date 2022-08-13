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
    public class KnittingYarnChallanService : MarshalByRefObject, IKnittingYarnChallanService
    {
        #region Private functions and declaration

        private KnittingYarnChallan MapObject(NullHandler oReader)
        {
            KnittingYarnChallan oKnittingYarnChallan = new KnittingYarnChallan();
            oKnittingYarnChallan.KnittingYarnChallanID = oReader.GetInt32("KnittingYarnChallanID");
            oKnittingYarnChallan.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingYarnChallan.ChallanNo = oReader.GetString("ChallanNo");
            oKnittingYarnChallan.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
            oKnittingYarnChallan.KnittingOrderDate = oReader.GetDateTime("KnittingOrderDate");
            oKnittingYarnChallan.BuyerName = oReader.GetString("BuyerName");
            oKnittingYarnChallan.StyleNo = oReader.GetString("StyleNo");
            oKnittingYarnChallan.KnittingOrderQty = oReader.GetDouble("KnittingOrderQty");
            oKnittingYarnChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oKnittingYarnChallan.DriverName = oReader.GetString("DriverName");
            oKnittingYarnChallan.CarNumber = oReader.GetString("CarNumber");
            oKnittingYarnChallan.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oKnittingYarnChallan.Remarks = oReader.GetString("Remarks");
            oKnittingYarnChallan.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oKnittingYarnChallan.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
            oKnittingYarnChallan.ApproveUser = oReader.GetString("ApproveUserName");
            oKnittingYarnChallan.BUID = oReader.GetInt32("BUID");
            oKnittingYarnChallan.OrderQty = oReader.GetDouble("OrderQty");
            oKnittingYarnChallan.FactoryName = oReader.GetString("FactoryName");
            oKnittingYarnChallan.PAM = oReader.GetInt32("PAM");

            return oKnittingYarnChallan;
        }

        private KnittingYarnChallan CreateObject(NullHandler oReader)
        {
            KnittingYarnChallan oKnittingYarnChallan = new KnittingYarnChallan();
            oKnittingYarnChallan = MapObject(oReader);
            return oKnittingYarnChallan;
        }

        private List<KnittingYarnChallan> CreateObjects(IDataReader oReader)
        {
            List<KnittingYarnChallan> oKnittingYarnChallan = new List<KnittingYarnChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingYarnChallan oItem = CreateObject(oHandler);
                oKnittingYarnChallan.Add(oItem);
            }
            return oKnittingYarnChallan;
        }

        #endregion

        #region Interface implementation
      
        public KnittingYarnChallan Save(KnittingYarnChallan oKnittingYarnChallan, Int64 nUserID)
        {
            //TransactionContext tc = null;
            //try
            //{
            //    tc = TransactionContext.Begin(true);
            //    IDataReader reader;
            //    if (oKnittingYarnChallan.KnittingYarnChallanID <= 0)
            //    {
            //        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarnChallan", EnumRoleOperationType.Add);
            //        reader = KnittingYarnChallanDA.InsertUpdate(tc, oKnittingYarnChallan, EnumDBOperation.Insert, nUserID);
            //    }
            //    else
            //    {
            //        //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "KnittingYarnChallan", EnumRoleOperationType.Edit);
            //        reader = KnittingYarnChallanDA.InsertUpdate(tc, oKnittingYarnChallan, EnumDBOperation.Update, nUserID);
            //    }
            //    NullHandler oReader = new NullHandler(reader);
            //    if (reader.Read())
            //    {
            //        oKnittingYarnChallan = new KnittingYarnChallan();
            //        oKnittingYarnChallan = CreateObject(oReader);
            //    }
            //    reader.Close();
            //    tc.End();
            //}
            //catch (Exception e)
            //{
            //    #region Handle Exception
            //    if (tc != null)
            //    {
            //        tc.HandleError();
            //        oKnittingYarnChallan = new KnittingYarnChallan();
            //        oKnittingYarnChallan.ErrorMessage = e.Message.Split('!')[0];
            //    }
            //    #endregion
            //}
            //return oKnittingYarnChallan;
            KnittingYarnChallanDetail oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
          
            KnittingYarnChallan oUG = new KnittingYarnChallan();
          
            oUG = oKnittingYarnChallan;
         
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region KnittingYarnChallan
                IDataReader reader;
                if (oKnittingYarnChallan.KnittingYarnChallanID <= 0)
                {
                   
                    reader = KnittingYarnChallanDA.InsertUpdate(tc, oKnittingYarnChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                   
                    reader = KnittingYarnChallanDA.InsertUpdate(tc, oKnittingYarnChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarnChallan = new KnittingYarnChallan();
                    oKnittingYarnChallan = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region KnittingYarnChallanDetail

                if (oKnittingYarnChallan.KnittingYarnChallanID > 0)
                {
                    string sKnittingYarnChallanIDs = "";
                    if (oUG.KnittingYarnChallanDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (KnittingYarnChallanDetail oDRD in oUG.KnittingYarnChallanDetails)
                        {
                            oDRD.KnittingYarnChallanID = oKnittingYarnChallan.KnittingYarnChallanID;
                            if (oDRD.KnittingYarnChallanDetailID <= 0)
                            {
                                readerdetail = KnittingYarnChallanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID,"");
                            }
                            else
                            {
                                readerdetail = KnittingYarnChallanDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID,"");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nKnittingYarnChallanDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nKnittingYarnChallanDetailID = oReaderDevRecapdetail.GetInt32("KnittingYarnChallanDetailID");
                                sKnittingYarnChallanIDs = sKnittingYarnChallanIDs + oReaderDevRecapdetail.GetString("KnittingYarnChallanDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sKnittingYarnChallanIDs.Length > 0)
                    {
                        sKnittingYarnChallanIDs = sKnittingYarnChallanIDs.Remove(sKnittingYarnChallanIDs.Length - 1, 1);
                    }
                    oKnittingYarnChallanDetail = new KnittingYarnChallanDetail();
                    oKnittingYarnChallanDetail.KnittingYarnChallanID = oKnittingYarnChallan.KnittingYarnChallanID;
                    KnittingYarnChallanDetailDA.Delete(tc, oKnittingYarnChallanDetail, EnumDBOperation.Delete, nUserID, sKnittingYarnChallanIDs);
                }

                #endregion

                

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingYarnChallan = new KnittingYarnChallan();
                    oKnittingYarnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingYarnChallan;
        }
        public KnittingYarnChallan Approve(KnittingYarnChallan oKnittingYarnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = KnittingYarnChallanDA.Approve(tc, oKnittingYarnChallan, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarnChallan = new KnittingYarnChallan();
                    oKnittingYarnChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnittingYarnChallan = new KnittingYarnChallan();
                    oKnittingYarnChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnittingYarnChallan;
           
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnittingYarnChallan oKnittingYarnChallan = new KnittingYarnChallan();
                oKnittingYarnChallan.KnittingYarnChallanID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "KnittingYarnChallan", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "KnittingYarnChallan", id);
                KnittingYarnChallanDA.Delete(tc, oKnittingYarnChallan, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public KnittingYarnChallan Get(int id, Int64 nUserId)
        {
            KnittingYarnChallan oKnittingYarnChallan = new KnittingYarnChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingYarnChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingYarnChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingYarnChallan", e);
                #endregion
            }
            return oKnittingYarnChallan;
        }

        public List<KnittingYarnChallan> Gets(Int64 nUserID)
        {
            List<KnittingYarnChallan> oKnittingYarnChallans = new List<KnittingYarnChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnChallanDA.Gets(tc);
                oKnittingYarnChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingYarnChallan oKnittingYarnChallan = new KnittingYarnChallan();
                oKnittingYarnChallan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingYarnChallans;
        }

        public List<KnittingYarnChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingYarnChallan> oKnittingYarnChallans = new List<KnittingYarnChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingYarnChallanDA.Gets(tc, sSQL);
                oKnittingYarnChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingYarnChallan", e);
                #endregion
            }
            return oKnittingYarnChallans;
        }

        #endregion
    }

}
