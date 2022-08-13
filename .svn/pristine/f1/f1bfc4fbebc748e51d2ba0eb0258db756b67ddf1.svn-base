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
    public class MktSaleTargetService : MarshalByRefObject, IMktSaleTargetService
    {
        #region Private functions and declaration
        private MktSaleTarget MapObject(NullHandler oReader)
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            oMktSaleTarget.MktSaleTargetID = oReader.GetInt32("MktSaleTargetID");
            oMktSaleTarget.MarketingAccountID = oReader.GetInt32("MarketingAccountID");
            oMktSaleTarget.OrderType = (EnumFabricRequestType)oReader.GetInt32("OrderType");
            oMktSaleTarget.Date = oReader.GetDateTime("Date");
            oMktSaleTarget.Value = oReader.GetDouble("Value");
            oMktSaleTarget.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oMktSaleTarget.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
            oMktSaleTarget.LastUpdateByName = oReader.GetString("LastUpdateByName");

            oMktSaleTarget.ContractorID = oReader.GetInt32("ContractorID");
            oMktSaleTarget.ContractorName = oReader.GetString("ContractorName");
            oMktSaleTarget.BuyerPosition = oReader.GetString("BuyerPosition");
            oMktSaleTarget.OrderQty = oReader.GetDouble("OrderQty");
            oMktSaleTarget.BuyerName = oReader.GetString("BuyerName");
            oMktSaleTarget.GroupHeadName = oReader.GetString("GroupHeadName");
            oMktSaleTarget.ReceiveQty = oReader.GetDouble("ReceiveQty");
            oMktSaleTarget.Month = oReader.GetInt32("Month");
            oMktSaleTarget.Year = oReader.GetInt32("Year");
            oMktSaleTarget.BPercent = oReader.GetDouble("BPercent");
            oMktSaleTarget.BuyerPercentID = oReader.GetInt32("BuyerPercentID");
            oMktSaleTarget.ApproveBy = oReader.GetInt32("ApproveBy");
            oMktSaleTarget.ApprovedByName = oReader.GetString("ApprovedByName");
            oMktSaleTarget.ApproveDate = oReader.GetDateTime("ApproveDate");
            oMktSaleTarget.Name = oReader.GetString("Name");
            oMktSaleTarget.WeaveType = oReader.GetInt32("WeaveType");
            oMktSaleTarget.FinishType = oReader.GetInt32("FinishType");
            oMktSaleTarget.FinishTypeName = oReader.GetString("FinishTypeName");
            oMktSaleTarget.WeaveTypeName = oReader.GetString("WeaveTypeName");
            oMktSaleTarget.ProductID = oReader.GetInt32("ProductID");
            oMktSaleTarget.ProductName = oReader.GetString("ProductName");
            oMktSaleTarget.Construction = oReader.GetString("Construction");
            oMktSaleTarget.Amount = oReader.GetDouble("Amount");
            return oMktSaleTarget;
        }
        private MktSaleTarget CreateObject(NullHandler oReader)
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            oMktSaleTarget = MapObject(oReader);
            return oMktSaleTarget;
        }
        private List<MktSaleTarget> CreateObjects(IDataReader oReader)
        {
            List<MktSaleTarget> oMktSaleTarget = new List<MktSaleTarget>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MktSaleTarget oItem = CreateObject(oHandler);
                oMktSaleTarget.Add(oItem);
            }
            return oMktSaleTarget;
        }
        #endregion
        #region Interface implementation
        public MktSaleTarget Save(MktSaleTarget oMktSaleTarget, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMktSaleTarget.MktSaleTargetID <= 0)
                {

                    reader = MktSaleTargetDA.InsertUpdate(tc, oMktSaleTarget, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MktSaleTargetDA.InsertUpdate(tc, oMktSaleTarget, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMktSaleTarget = new MktSaleTarget();
                    oMktSaleTarget = CreateObject(oReader);
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
                    oMktSaleTarget = new MktSaleTarget();
                    oMktSaleTarget.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oMktSaleTarget;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MktSaleTarget oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.MktSaleTargetID = id;
                DBTableReferenceDA.HasReference(tc, "MktSaleTarget", id);
                MktSaleTargetDA.Delete(tc, oMktSaleTarget, EnumDBOperation.Delete, nUserId);
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
        public MktSaleTarget Get(int id, Int64 nUserId)
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = MktSaleTargetDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMktSaleTarget = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get MktSaleTarget", e);
                #endregion
            }
            return oMktSaleTarget;
        }
        public List<MktSaleTarget> Gets(Int64 nUserID)
        {
            List<MktSaleTarget> oMktSaleTargets = new List<MktSaleTarget>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MktSaleTargetDA.Gets(tc);
                oMktSaleTargets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                MktSaleTarget oMktSaleTarget = new MktSaleTarget();
                oMktSaleTarget.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oMktSaleTargets;
        }
        public List<MktSaleTarget> Gets(string sSQL, Int64 nUserID)
        {
            List<MktSaleTarget> oMktSaleTargets = new List<MktSaleTarget>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MktSaleTargetDA.Gets(tc, sSQL);
                oMktSaleTargets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MktSaleTarget", e);
                #endregion
            }
            return oMktSaleTargets;
        }

        public List<MktSaleTarget> Approve(List<MktSaleTarget> oMktSaleTargets, Int64 nUserID)
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> oTempMktSaleTargets = new List<MktSaleTarget>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (MktSaleTarget oItem in oMktSaleTargets)
                {
                    if (oItem.ApproveBy == 0)
                    {
                        IDataReader reader;
                        reader = MktSaleTargetDA.InsertUpdate(tc, oItem, EnumDBOperation.Approval, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oMktSaleTarget = new MktSaleTarget();
                            oMktSaleTarget = CreateObject(oReader);
                            oTempMktSaleTargets.Add(oMktSaleTarget);
                        }
                        reader.Close();

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
                    oTempMktSaleTargets = new List<MktSaleTarget>();
                    oMktSaleTarget = new MktSaleTarget();
                    oMktSaleTarget.ErrorMessage = e.Message.Split('!')[0];
                    oTempMktSaleTargets.Add(oMktSaleTarget);
                }
                #endregion
            }
            return oTempMktSaleTargets;
        }

        public List<MktSaleTarget> UndoApprove(List<MktSaleTarget> oMktSaleTargets, Int64 nUserID)
        {
            MktSaleTarget oMktSaleTarget = new MktSaleTarget();
            List<MktSaleTarget> oTempMktSaleTargets = new List<MktSaleTarget>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (MktSaleTarget oItem in oMktSaleTargets)
                {
                    if (oItem.ApproveBy != 0)
                    {
                        IDataReader reader;
                        reader = MktSaleTargetDA.InsertUpdate(tc, oItem, EnumDBOperation.UnApproval, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oMktSaleTarget = new MktSaleTarget();
                            oMktSaleTarget = CreateObject(oReader);
                            oTempMktSaleTargets.Add(oMktSaleTarget);
                        }
                        reader.Close();

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
                    oTempMktSaleTargets = new List<MktSaleTarget>();
                    oMktSaleTarget = new MktSaleTarget();
                    oMktSaleTarget.ErrorMessage = e.Message.Split('!')[0];
                    oTempMktSaleTargets.Add(oMktSaleTarget);
                }
                #endregion
            }
            return oTempMktSaleTargets;
        }

        #endregion
    }
}
