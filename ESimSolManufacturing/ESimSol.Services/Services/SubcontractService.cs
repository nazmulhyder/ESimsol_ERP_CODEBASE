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
    public class SubcontractService : MarshalByRefObject, ISubcontractService
    {
        #region Private functions and declaration
        private Subcontract MapObject(NullHandler oReader)
        {
            Subcontract oSubcontract = new Subcontract();
            oSubcontract.SubcontractID = oReader.GetInt32("SubcontractID");
            oSubcontract.SubcontractNo = oReader.GetString("SubcontractNo");
            oSubcontract.ContractStatus = (EnumSubContractStatus)oReader.GetInt32("ContractStatus"); ;
            oSubcontract.ContractStatusInt = oReader.GetInt32("ContractStatus");
            oSubcontract.IssueBUID = oReader.GetInt32("IssueBUID");
            oSubcontract.ContractBUID = oReader.GetInt32("ContractBUID");
            oSubcontract.PTU2ID = oReader.GetInt32("PTU2ID");
            oSubcontract.IssueDate = oReader.GetDateTime("IssueDate");
            oSubcontract.ExportSCID = oReader.GetInt32("ExportSCID");
            oSubcontract.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            oSubcontract.ProductID = oReader.GetInt32("ProductID");
            oSubcontract.ColorID = oReader.GetInt32("ColorID");
            oSubcontract.MoldRefID = oReader.GetInt32("MoldRefID");
            oSubcontract.UintID = oReader.GetInt32("UintID");
            oSubcontract.Qty = oReader.GetDouble("Qty");
            oSubcontract.RateUnit = oReader.GetInt32("RateUnit");
            oSubcontract.UnitPrice = oReader.GetDouble("UnitPrice");
            oSubcontract.CurrencyID = oReader.GetInt32("CurrencyID");
            oSubcontract.CRate = oReader.GetDouble("CRate");
            oSubcontract.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oSubcontract.Remarks = oReader.GetString("Remarks");
            oSubcontract.IssueBUName = oReader.GetString("IssueBUName");
            oSubcontract.IssueBUShortName = oReader.GetString("IssueBUShortName");
            oSubcontract.ContarctBUName = oReader.GetString("ContarctBUName");
            oSubcontract.ContarctBUShortName = oReader.GetString("ContarctBUShortName");
            oSubcontract.PINo = oReader.GetString("PINo");
            oSubcontract.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oSubcontract.PIStatusInt = oReader.GetInt32("PIStatus");
            oSubcontract.ExportSCDate = oReader.GetDateTime("ExportSCDate");
            oSubcontract.ContractorName = oReader.GetString("ContractorName");
            oSubcontract.BuyerName = oReader.GetString("BuyerName");
            oSubcontract.ProductCode = oReader.GetString("ProductCode");
            oSubcontract.ProductName = oReader.GetString("ProductName");
            oSubcontract.ColorName = oReader.GetString("ColorName");
            oSubcontract.UnitName = oReader.GetString("UnitName");
            oSubcontract.UnitSymbol = oReader.GetString("UnitSymbol");
            oSubcontract.MoldName = oReader.GetString("MoldName");
            oSubcontract.ApprovedByName = oReader.GetString("ApprovedByName");
            oSubcontract.CurrencyName = oReader.GetString("CurrencyName");
            oSubcontract.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oSubcontract.ProductionCapacity = oReader.GetDouble("ProductionCapacity");
            oSubcontract.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oSubcontract.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oSubcontract.ReceivedByName = oReader.GetString("ReceivedByName");
            oSubcontract.ReceivedDate = oReader.GetDateTime("ReceivedDate");

            return oSubcontract;
        }

        private Subcontract CreateObject(NullHandler oReader)
        {
            Subcontract oSubcontract = new Subcontract();
            oSubcontract = MapObject(oReader);
            return oSubcontract;
        }

        private List<Subcontract> CreateObjects(IDataReader oReader)
        {
            List<Subcontract> oSubcontract = new List<Subcontract>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Subcontract oItem = CreateObject(oHandler);
                oSubcontract.Add(oItem);
            }
            return oSubcontract;
        }

        #endregion

        #region Interface implementation
        public SubcontractService() { }

        public Subcontract Save(Subcontract oSubcontract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSubcontract.SubcontractID <= 0)
                {
                    reader = SubcontractDA.InsertUpdate(tc, oSubcontract, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = SubcontractDA.InsertUpdate(tc, oSubcontract, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSubcontract = new Subcontract();
                    oSubcontract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Subcontract. Because of " + e.Message, e);
                #endregion
            }
            return oSubcontract;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Subcontract oSubcontract = new Subcontract();
                oSubcontract.SubcontractID = id;
                SubcontractDA.Delete(tc, oSubcontract, EnumDBOperation.Delete, nUserId);
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

        public Subcontract Approved(Subcontract oSubcontract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SubcontractDA.Approved(tc, oSubcontract, nUserID);
                IDataReader reader = SubcontractDA.Get(tc, oSubcontract.SubcontractID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSubcontract = new Subcontract();
                    oSubcontract = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Subcontract. Because of " + e.Message, e);
                oSubcontract = new Subcontract();
                oSubcontract.ErrorMessage = e.Message;
                #endregion
            }
            return oSubcontract;
        }


        public Subcontract Received(Subcontract oSubcontract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //SubcontractDA.Received(tc, oSubcontract, nUserID);
                IDataReader reader = SubcontractDA.InsertUpdate(tc, oSubcontract, EnumDBOperation.Receive, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSubcontract = new Subcontract();
                    oSubcontract = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Subcontract. Because of " + e.Message, e);
                oSubcontract = new Subcontract();
                oSubcontract.ErrorMessage = e.Message;
                #endregion
            }
            return oSubcontract;
        }
        public Subcontract SendToProduction(Subcontract oSubcontract, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                IDataReader reader = SubcontractDA.SendToProduction(tc, oSubcontract, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSubcontract = new Subcontract();
                    oSubcontract = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Subcontract. Because of " + e.Message, e);
                oSubcontract = new Subcontract();
                oSubcontract.ErrorMessage = e.Message;
                #endregion
            }
            return oSubcontract;
        }

        public Subcontract Get(int id, Int64 nUserId)
        {
            Subcontract oSubcontract = new Subcontract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = SubcontractDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSubcontract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Subcontract", e);
                #endregion
            }
            return oSubcontract;
        }
        public List<Subcontract> Gets(Int64 nUserID)
        {
            List<Subcontract> oSubcontracts = new List<Subcontract>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SubcontractDA.Gets(tc);
                oSubcontracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Subcontract", e);
                #endregion
            }
            return oSubcontracts;
        }
        public List<Subcontract> Gets(string sSQL,Int64 nUserID)
        {
            List<Subcontract> oSubcontracts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SubcontractDA.Gets(tc,sSQL);
                oSubcontracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Subcontract", e);
                #endregion
            }
            return oSubcontracts;
        }
        #endregion
    }   
}