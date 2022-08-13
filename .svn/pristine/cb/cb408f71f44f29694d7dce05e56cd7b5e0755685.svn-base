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
    public class PTUDistributionService : MarshalByRefObject, IPTUDistributionService
    {
        #region Private functions and declaration
        private PTUDistribution  MapObject( NullHandler oReader)
        {
            PTUDistribution oPTUDistribution=new PTUDistribution();
            oPTUDistribution.PTUID = oReader.GetInt32("PTUID");
            oPTUDistribution.PTUDistributionID = oReader.GetInt32("PTUDistributionID");
            oPTUDistribution.LotID = oReader.GetInt32("LotID");
            oPTUDistribution.Qty = oReader.GetDouble("Qty");
            oPTUDistribution.LocationName = oReader.GetString("LocationName");
            oPTUDistribution.OperationUnitName = oReader.GetString("OperationUnitName");
            oPTUDistribution.LotNo = oReader.GetString("LotNo");
            oPTUDistribution.OrderNo = oReader.GetString("OrderNo");
            oPTUDistribution.ProductName = oReader.GetString("ProductName");
            oPTUDistribution.ColorName = oReader.GetString("ColorName");
            oPTUDistribution.ProductID = oReader.GetInt32("ProductID");
            oPTUDistribution.UnitPrice = oReader.GetDouble("UnitPrice");
            return oPTUDistribution;
        }

        private PTUDistribution CreateObject(NullHandler oReader)
        {
            PTUDistribution oPTUDistribution = new PTUDistribution();
            oPTUDistribution=MapObject(oReader);
            return oPTUDistribution;
        }

        private List<PTUDistribution> CreateObjects(IDataReader oReader)
        {
            List<PTUDistribution> oPTUDistributions = new List<PTUDistribution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTUDistribution oItem = CreateObject(oHandler);
                oPTUDistributions.Add(oItem);
            }
            return oPTUDistributions;
        }
        #endregion

        #region Interface implementation
        public PTUDistributionService() { }



        public PTUDistribution Get(int nID, Int64 nUserID)
        {
            PTUDistribution oPTUDistribution = new PTUDistribution();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUDistributionDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUDistribution = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PTUDistribution", e);
                #endregion
            }

            return oPTUDistribution;
        }
        public PTUDistribution Get(int nPTUID, int nLotID, Int64 nUserID)
        {
            PTUDistribution oPTUDistribution = new PTUDistribution();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PTUDistributionDA.Get(tc, nPTUID, nLotID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUDistribution = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PTUDistribution", e);
                #endregion
            }

            return oPTUDistribution;
        }
   
        public List<PTUDistribution> Gets(int nPTUID, Int64 nUserID)
        {
            List<PTUDistribution> oPTUDistributions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUDistributionDA.Gets(tc, nPTUID);
                oPTUDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUDistributions", e);
                #endregion
            }

            return oPTUDistributions;
        }
        public List<PTUDistribution> GetsByLot(int nLotID, Int64 nUserID)
        {
            List<PTUDistribution> oPTUDistributions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUDistributionDA.GetsByLot(tc, nLotID);
                oPTUDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUDistributions", e);
                #endregion
            }

            return oPTUDistributions;
        }
        public List<PTUDistribution> Gets(int nPTUID, int nStoreID, Int64 nUserID)
        {
            List<PTUDistribution> oPTUDistributions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUDistributionDA.Gets(tc, nPTUID,nStoreID);
                oPTUDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUDistributions", e);
                #endregion
            }

            return oPTUDistributions;
        }

        public List<PTUDistribution> Gets(string sSQL, Int64 nUserID)
        {
            List<PTUDistribution> oPTUDistributions = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUDistributionDA.Gets(tc, sSQL);
                oPTUDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUDistributions", e);
                #endregion
            }

            return oPTUDistributions;
        }

        public PTUDistribution PTUToPTU_Transfer(PTUDistribution oPTUDistribution, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader;
                reader = PTUDistributionDA.PTUToPTU_Transfer(tc, oPTUDistribution, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUDistribution = CreateObject(oReader);
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
                oPTUDistribution.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oPTUDistribution;
        }
        #endregion
    }
}
