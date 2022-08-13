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
    public class FabricPlanOrderService : MarshalByRefObject, IFabricPlanOrderService
    {
        #region Private functions and declaration

        private FabricPlanOrder MapObject(NullHandler oReader)
        {
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            oFabricPlanOrder.FabricPlanOrderID = oReader.GetInt32("FabricPlanOrderID");
            oFabricPlanOrder.FabricID = oReader.GetInt32("FabricID");
            //oFabricPlanOrder.FSCDID = oReader.GetInt32("FSCDID");
            oFabricPlanOrder.RefType = (EnumFabricPlanRefType)oReader.GetInt16("RefType");
            oFabricPlanOrder.RefID = oReader.GetInt32("RefID");
            oFabricPlanOrder.ColumnCount = oReader.GetInt32("ColumnCount");
            oFabricPlanOrder.RefNo = oReader.GetString("RefNo");
            oFabricPlanOrder.Reed = oReader.GetInt32("Reed");
            oFabricPlanOrder.Dent = oReader.GetDouble("Dent");
            oFabricPlanOrder.GSM = oReader.GetDouble("GSM");
            oFabricPlanOrder.Pick = oReader.GetInt32("Pick");
            oFabricPlanOrder.Warp = oReader.GetString("Warp");
            oFabricPlanOrder.Weft = oReader.GetString("Weft");
            oFabricPlanOrder.Weave = oReader.GetInt32("Weave");
            oFabricPlanOrder.RepeatSize = oReader.GetString("RepeatSize");
            oFabricPlanOrder.WeaveName = oReader.GetString("WeaveName");
            oFabricPlanOrder.Note = oReader.GetString("Note");
            return oFabricPlanOrder;
        }

        private FabricPlanOrder CreateObject(NullHandler oReader)
        {
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            oFabricPlanOrder = MapObject(oReader);
            return oFabricPlanOrder;
        }

        private List<FabricPlanOrder> CreateObjects(IDataReader oReader)
        {
            List<FabricPlanOrder> oFabricPlanOrder = new List<FabricPlanOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPlanOrder oItem = CreateObject(oHandler);
                oFabricPlanOrder.Add(oItem);
            }
            return oFabricPlanOrder;
        }

        #endregion

        #region Interface implementation

        public FabricPlanOrder Save(FabricPlanOrder oFabricPlanOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<FabricPlan> oFabricPlans = new List<FabricPlan>();
                List<FabricPlanDetail> oFabricPlanDetails = new List<FabricPlanDetail>();
                List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
                oFabricPlans = oFabricPlanOrder.FabricPlans;
                oFabricPlanRepeats = oFabricPlanOrder.FabricPlanRepeats;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricPlanOrder.FabricPlanOrderID <= 0)
                {
                    reader = FabricPlanOrderDA.InsertUpdate(tc, oFabricPlanOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricPlanOrderDA.InsertUpdate(tc, oFabricPlanOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanOrder = new FabricPlanOrder();
                    oFabricPlanOrder = CreateObject(oReader);
                }
                reader.Close();

                #region Fabric Plan
                if (oFabricPlans != null)
                {
                    //string sExportPITandCClauseIDs = "";
                    foreach (FabricPlan oItem in oFabricPlans)
                    {
                        IDataReader readertnc;
                        oItem.FabricPlanOrderID = oFabricPlanOrder.FabricPlanOrderID;
                        oItem.RefID = oFabricPlanOrder.RefID;
                        oItem.RefType = oFabricPlanOrder.RefType;
                        if (oItem.FabricPlanID <= 0)
                        {
                            readertnc = FabricPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readertnc = FabricPlanDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                           // sExportPITandCClauseIDs = sExportPITandCClauseIDs + oReaderTNC.GetString("ExportPITandCClauseID") + ",";
                            oItem.FabricPlanID = oReaderTNC.GetInt32("FabricPlanID");
                        }

                       
                        foreach (FabricPlanDetail oItem2 in oItem.FabricPlanDetails)
                        {
                            oItem2.FabricPlanID = oItem.FabricPlanID;
                            oFabricPlanDetails.Add(oItem2);
                        }
                       
                        readertnc.Close();
                    }
                    oFabricPlanOrder.FabricPlans = oFabricPlans;
                }
                #endregion
                #region Fabric Plan Detail
                if (oFabricPlanDetails != null)
                {
                    //string sExportPITandCClauseIDs = "";
                    foreach (FabricPlanDetail oItem in oFabricPlanDetails)
                    {
                        IDataReader readerFPD;
                        //oItem.FabricPlanOrderID = oFabricPlanOrder.FabricPlanOrderID;
                        if (oItem.FabricPlanDetailID <= 0)
                        {
                            readerFPD = FabricPlanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerFPD = FabricPlanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }

                        NullHandler oReaderTNC = new NullHandler(readerFPD);

                        readerFPD.Close();
                    }
                    oFabricPlanOrder.FabricPlanDetails = oFabricPlanDetails;
                }
                #endregion
                #region Fabric Plan Repeats
                if (oFabricPlanRepeats != null)
                {
                    foreach (FabricPlanRepeat oItem in oFabricPlanRepeats)
                    {
                        IDataReader readerFPR;
                        oItem.FabricPlanOrderID = oFabricPlanOrder.FabricPlanOrderID;
                        if (oItem.FabricPlanRepeatID <= 0)
                        {
                            readerFPR = FabricPlanRepeatDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerFPR = FabricPlanRepeatDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderTNC = new NullHandler(readerFPR);
                        readerFPR.Close();
                    }
                    oFabricPlanOrder.FabricPlanRepeats = oFabricPlanRepeats;
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricPlanOrder;
        }               
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.FabricPlanOrderID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "FabricPlanOrder", EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "FabricPlanOrder", id);
                FabricPlanOrderDA.Delete(tc, oFabricPlanOrder, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public FabricPlanOrder Get(int id, Int64 nUserId)
        {
            FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricPlanOrderDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanOrder = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricPlanOrder", e);
                #endregion
            }
            return oFabricPlanOrder;
        }

        public List<FabricPlanOrder> Gets(int nFabricPlanOrderID, Int64 nUserID)
        {
            List<FabricPlanOrder> oFabricPlanOrders = new List<FabricPlanOrder>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanOrderDA.Gets(tc, nFabricPlanOrderID);
                oFabricPlanOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricPlanOrders;
        }

        public List<FabricPlanOrder> Gets(int nRefID,int nRefType, Int64 nUserID)
        {
            List<FabricPlanOrder> oFabricPlanOrders = new List<FabricPlanOrder>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanOrderDA.Gets(tc, nRefID, nRefType);
                oFabricPlanOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlanOrder oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricPlanOrders;
        }

        public List<FabricPlanOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricPlanOrder> oFabricPlanOrders = new List<FabricPlanOrder>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanOrderDA.Gets(tc, sSQL);
                oFabricPlanOrders = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricPlanOrder", e);
                #endregion
            }
            return oFabricPlanOrders;
        }

        public FabricPlanOrder CopyFabricPlans(FabricPlanOrder oFabricPlanOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricPlanOrderDA.CopyFabricPlan(tc, oFabricPlanOrder, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricPlanOrder = new FabricPlanOrder();
                oFabricPlanOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricPlanOrder;
        }

        #endregion
    }

}
