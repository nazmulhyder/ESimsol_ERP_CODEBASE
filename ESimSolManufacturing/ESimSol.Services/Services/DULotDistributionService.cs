using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class DULotDistributionService : MarshalByRefObject, IDULotDistributionService
    {
        #region Private functions and declaration
        private DULotDistribution  MapObject( NullHandler oReader)
        {
            DULotDistribution oDULotDistribution=new DULotDistribution();
            oDULotDistribution.DODID = oReader.GetInt32("DODID");
            oDULotDistribution.DULotDistributionID = oReader.GetInt32("DULotDistributionID");
            oDULotDistribution.LotID = oReader.GetInt32("LotID");
            oDULotDistribution.Qty = oReader.GetDouble("Qty");
            oDULotDistribution.LotBalance = oReader.GetDouble("LotBalance");
            oDULotDistribution.LocationName = oReader.GetString("LocationName");
            oDULotDistribution.OperationUnitName = oReader.GetString("OperationUnitName");
            oDULotDistribution.LotNo = oReader.GetString("LotNo");
            oDULotDistribution.OrderNo = oReader.GetString("OrderNo");
            oDULotDistribution.ProductName = oReader.GetString("ProductName");
            oDULotDistribution.ColorName = oReader.GetString("ColorName");
            oDULotDistribution.ProductID = oReader.GetInt32("ProductID");
            oDULotDistribution.UnitPrice = oReader.GetDouble("UnitPrice");
            oDULotDistribution.IsFinish = oReader.GetBoolean("IsFinish");
            oDULotDistribution.IsRaw = oReader.GetBoolean("IsRaw");
            oDULotDistribution.MUName = oReader.GetString("MUName");
            oDULotDistribution.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            
            return oDULotDistribution;
        }

        private DULotDistribution CreateObject(NullHandler oReader)
        {
            DULotDistribution oDULotDistribution = new DULotDistribution();
            oDULotDistribution=MapObject(oReader);
            return oDULotDistribution;
        }

        private List<DULotDistribution> CreateObjects(IDataReader oReader)
        {
            List<DULotDistribution> oDULotDistributions = new List<DULotDistribution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DULotDistribution oItem = CreateObject(oHandler);
                oDULotDistributions.Add(oItem);
            }
            return oDULotDistributions;
        }
        #endregion

        #region Interface implementation
        public DULotDistributionService() { }


        public DULotDistribution Get(int nID, Int64 nUserID)
        {
            DULotDistribution oDULotDistribution = new DULotDistribution();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DULotDistributionDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDULotDistribution = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get DULotDistribution", e);
                #endregion
            }

            return oDULotDistribution;
        }
        public List<DULotDistribution> Gets(int nDODID, int nWUID, Int64 nUserID)
        {
            List<DULotDistribution> oDULotDistributions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DULotDistributionDA.Gets(tc, nDODID, nWUID);
                oDULotDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DULotDistributions", e);
                #endregion
            }

            return oDULotDistributions;
        }
        public List<DULotDistribution> GetsByWU(int nDODID, string nWUIDs, Int64 nUserID)
        {
            List<DULotDistribution> oDULotDistributions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DULotDistributionDA.GetsByWU(tc, nDODID, nWUIDs);
                oDULotDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DULotDistributions", e);
                #endregion
            }

            return oDULotDistributions;
        }
        public List<DULotDistribution> GetsBy(int nDODID, Int64 nUserID)
        {
            List<DULotDistribution> oDULotDistributions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DULotDistributionDA.GetsBy(tc, nDODID);
                oDULotDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DULotDistributions", e);
                #endregion
            }

            return oDULotDistributions;
        }
        public List<DULotDistribution> GetsByLot(int nLotID, Int64 nUserID)
        {
            List<DULotDistribution> oDULotDistributions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DULotDistributionDA.GetsByLot(tc, nLotID);
                oDULotDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DULotDistributions", e);
                #endregion
            }

            return oDULotDistributions;
        }
   
        public List<DULotDistribution> Gets(string sSQL, Int64 nUserID)
        {
            List<DULotDistribution> oDULotDistributions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DULotDistributionDA.Gets(tc, sSQL);
                oDULotDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DULotDistributions", e);
                #endregion
            }

            return oDULotDistributions;
        }

        public DULotDistribution Save_Transfer(DULotDistribution oDULotDistribution, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DULotDistributionDA.Save_Transfer(tc, oDULotDistribution, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDULotDistribution = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUClaimOrder", e);
                oDULotDistribution.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDULotDistribution;
        }
        public DULotDistribution Save_Reduce(DULotDistribution oDULotDistribution, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = DULotDistributionDA.Save_Reduce(tc, oDULotDistribution, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDULotDistribution = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get DUClaimOrder", e);
                oDULotDistribution.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oDULotDistribution;
        }
        //public string Delete(int id, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        DULotDistribution oDULotDistribution = new DULotDistribution();
        //        oDULotDistribution.DULotDistributionID = id;
        //        DBTableReferenceDA.HasReference(tc, "DULotDistribution", id);
        //        DULotDistributionDA.Delete(tc, oDULotDistribution, EnumDBOperation.Delete, nUserId);
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exceptionif (tc != null)
        //        tc.HandleError();
        //        return e.Message.Split('!')[0];
        //        #endregion
        //    }
        //    return Global.DeleteMessage;
        //}
        #endregion
    }
}

