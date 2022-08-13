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
    public class KnitDyeingGrayChallanService : MarshalByRefObject, IKnitDyeingGrayChallanService
    {
        #region Private functions and declaration

        private KnitDyeingGrayChallan MapObject(NullHandler oReader)
        {
            KnitDyeingGrayChallan oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
            oKnitDyeingGrayChallan.KnitDyeingGrayChallanID = oReader.GetInt32("KnitDyeingGrayChallanID");
            oKnitDyeingGrayChallan.KnitDyeingBatchID = oReader.GetInt32("KnitDyeingBatchID");
            oKnitDyeingGrayChallan.BUID = oReader.GetInt32("BUID");
            oKnitDyeingGrayChallan.ChallanNo = oReader.GetString("ChallanNo");
            oKnitDyeingGrayChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oKnitDyeingGrayChallan.DisburseBy = oReader.GetInt32("DisburseBy");
            oKnitDyeingGrayChallan.Remarks = oReader.GetString("Remarks");
            oKnitDyeingGrayChallan.TruckNo = oReader.GetString("TruckNo");
            oKnitDyeingGrayChallan.DriverName = oReader.GetString("DriverName");
            oKnitDyeingGrayChallan.DisburseByName = oReader.GetString("DisburseByName");
            oKnitDyeingGrayChallan.BatchNo = oReader.GetString("BatchNo");
            oKnitDyeingGrayChallan.BUShortName = oReader.GetString("BUShortName");
            oKnitDyeingGrayChallan.BUName = oReader.GetString("BUName");
            oKnitDyeingGrayChallan.BUAddress = oReader.GetString("BUAddress");
            return oKnitDyeingGrayChallan;
        }

        private KnitDyeingGrayChallan CreateObject(NullHandler oReader)
        {
            KnitDyeingGrayChallan oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
            oKnitDyeingGrayChallan = MapObject(oReader);
            return oKnitDyeingGrayChallan;
        }

        private List<KnitDyeingGrayChallan> CreateObjects(IDataReader oReader)
        {
            List<KnitDyeingGrayChallan> oKnitDyeingGrayChallan = new List<KnitDyeingGrayChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnitDyeingGrayChallan oItem = CreateObject(oHandler);
                oKnitDyeingGrayChallan.Add(oItem);
            }
            return oKnitDyeingGrayChallan;
        }

        #endregion

        #region Interface implementation
        public KnitDyeingGrayChallan Save(KnitDyeingGrayChallan oKnitDyeingGrayChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            KnitDyeingGrayChallan _oKnitDyeingGrayChallan = oKnitDyeingGrayChallan;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnitDyeingGrayChallan.KnitDyeingGrayChallanID <= 0)
                {
                    reader = KnitDyeingGrayChallanDA.InsertUpdate(tc, oKnitDyeingGrayChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = KnitDyeingGrayChallanDA.InsertUpdate(tc, oKnitDyeingGrayChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
                    oKnitDyeingGrayChallan = CreateObject(oReader);
                }
                reader.Close();
                if (oKnitDyeingGrayChallan.KnitDyeingGrayChallanID > 0)
                {
                    string ids = "";
                    foreach (KnitDyeingGrayChallanDetail item in _oKnitDyeingGrayChallan.KnitDyeingGrayChallanDetails)
                    {
                        if (item.KnitDyeingGrayChallanDetailID > 0)
                        {
                            ids = ids + item.KnitDyeingGrayChallanDetailID + ",";
                        }
                    }
                    if (ids.Length > 0) ids = ids.Remove(ids.Length - 1);
                    IDataReader detailReader;
                    if (_oKnitDyeingGrayChallan.KnitDyeingGrayChallanDetails.Count > 0)
                    {

                        foreach (KnitDyeingGrayChallanDetail oItem in _oKnitDyeingGrayChallan.KnitDyeingGrayChallanDetails)
                        {
                            oItem.KnitDyeingGrayChallanID = oKnitDyeingGrayChallan.KnitDyeingGrayChallanID;
                            if (oItem.KnitDyeingGrayChallanDetailID <= 0)
                            {
                                detailReader = KnitDyeingGrayChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, ids);
                            }
                            else
                            {
                                detailReader = KnitDyeingGrayChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, ids);
                            }
                            NullHandler oDetailReader = new NullHandler(detailReader);
                            if (detailReader.Read())
                            {

                            }
                            detailReader.Close();
                        }
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
                    oKnitDyeingGrayChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingGrayChallan;
        }
        public KnitDyeingGrayChallan Disburse(KnitDyeingGrayChallan oKnitDyeingGrayChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = KnitDyeingGrayChallanDA.InsertUpdate(tc, oKnitDyeingGrayChallan, EnumDBOperation.Disburse, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
                    oKnitDyeingGrayChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save KnitDyeingGrayChallan. Because of " + e.Message, e);
                #endregion
            }
            return oKnitDyeingGrayChallan;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnitDyeingGrayChallan oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
                oKnitDyeingGrayChallan.KnitDyeingGrayChallanID = id;
                DBTableReferenceDA.HasReference(tc, "KnitDyeingGrayChallan", id);
                KnitDyeingGrayChallanDA.Delete(tc, oKnitDyeingGrayChallan, EnumDBOperation.Delete, nUserId);
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

        public KnitDyeingGrayChallan Get(int id, Int64 nUserId)
        {
            KnitDyeingGrayChallan oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnitDyeingGrayChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingGrayChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnitDyeingGrayChallan", e);
                #endregion
            }
            return oKnitDyeingGrayChallan;
        }

        public List<KnitDyeingGrayChallan> Gets(Int64 nUserID)
        {
            List<KnitDyeingGrayChallan> oKnitDyeingGrayChallans = new List<KnitDyeingGrayChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingGrayChallanDA.Gets(tc);
                oKnitDyeingGrayChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnitDyeingGrayChallan oKnitDyeingGrayChallan = new KnitDyeingGrayChallan();
                oKnitDyeingGrayChallan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnitDyeingGrayChallans;
        }

        public List<KnitDyeingGrayChallan> Gets(string sSQL, Int64 nUserID)
        {
            List<KnitDyeingGrayChallan> oKnitDyeingGrayChallans = new List<KnitDyeingGrayChallan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingGrayChallanDA.Gets(tc, sSQL);
                oKnitDyeingGrayChallans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnitDyeingGrayChallan", e);
                #endregion
            }
            return oKnitDyeingGrayChallans;
        }

        #endregion
    }

}
