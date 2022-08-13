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
    public class PTUUnit2DistributionService : MarshalByRefObject, IPTUUnit2DistributionService
    {
        #region Private functions and declaration
        private PTUUnit2Distribution MapObject(NullHandler oReader)
        {
            PTUUnit2Distribution oPTUUnit2Distribution = new PTUUnit2Distribution();
            oPTUUnit2Distribution.PTUUnit2DistributionID = oReader.GetInt32("PTUUnit2DistributionID");
            oPTUUnit2Distribution.LotID = oReader.GetInt32("LotID");
            oPTUUnit2Distribution.PTUUnit2ID = oReader.GetInt32("PTUUnit2ID");
            oPTUUnit2Distribution.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oPTUUnit2Distribution.Qty = oReader.GetDouble("Qty");
            oPTUUnit2Distribution.LotBalance = oReader.GetDouble("LotBalance");
            oPTUUnit2Distribution.LotNo = oReader.GetString("LotNo");
            oPTUUnit2Distribution.MUName = oReader.GetString("MUName");
            oPTUUnit2Distribution.WorkingUnitName = oReader.GetString("WorkingUnitName");
            oPTUUnit2Distribution.ProductName = oReader.GetString("ProductName");
            oPTUUnit2Distribution.ColorName = oReader.GetString("ColorName");
            oPTUUnit2Distribution.PINo = oReader.GetString("PINo");
            oPTUUnit2Distribution.BuyerName = oReader.GetString("BuyerName");
            oPTUUnit2Distribution.TransactionTime = oReader.GetDateTime("TransactionTime");
            
            return oPTUUnit2Distribution;
        }

        private PTUUnit2Distribution CreateObject(NullHandler oReader)
        {
            PTUUnit2Distribution oPTUUnit2Distribution = new PTUUnit2Distribution();
            oPTUUnit2Distribution = MapObject(oReader);
            return oPTUUnit2Distribution;
        }

        private List<PTUUnit2Distribution> CreateObjects(IDataReader oReader)
        {
            List<PTUUnit2Distribution> oPTUUnit2Distribution = new List<PTUUnit2Distribution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PTUUnit2Distribution oItem = CreateObject(oHandler);
                oPTUUnit2Distribution.Add(oItem);
            }
            return oPTUUnit2Distribution;
        }

        #endregion

        #region Interface implementation
        public PTUUnit2DistributionService() { }


        public PTUUnit2Distribution PTUTransfer(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID)
        {   
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DistributionDA.PTUTransfer(oPTUUnit2Distribution, tc, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2Distribution = new  PTUUnit2Distribution();
                    oPTUUnit2Distribution = CreateObject(oReader);
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
                throw new ServiceException(e.Message, e);
                #endregion
            }

            return oPTUUnit2Distribution;
        }

        public PTUUnit2Distribution ReceiveInReadyeStock(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DistributionDA.ReceiveInReadyeStock(oPTUUnit2Distribution, tc, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2Distribution = new PTUUnit2Distribution();
                    oPTUUnit2Distribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPTUUnit2Distribution = new PTUUnit2Distribution();
                oPTUUnit2Distribution.ErrorMessage = "Failed to Operation because of " + e.Message.Split('!')[0];
                #endregion
            }

            return oPTUUnit2Distribution;
        }

        //

        public PTUUnit2Distribution ReceiveInAvilableStock(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DistributionDA.ReceiveInAvilableStock(oPTUUnit2Distribution, tc, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2Distribution = new PTUUnit2Distribution();
                    oPTUUnit2Distribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
               oPTUUnit2Distribution = new PTUUnit2Distribution();
               oPTUUnit2Distribution.ErrorMessage = "Failed to Operation because of "+e.Message.Split('!')[0];
            }

            return oPTUUnit2Distribution;
        }

        public PTUUnit2Distribution PTUTransferSubContract(PTUUnit2Distribution oPTUUnit2Distribution, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DistributionDA.PTUTransferSubContract(oPTUUnit2Distribution, tc, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPTUUnit2Distribution = new PTUUnit2Distribution();
                    oPTUUnit2Distribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPTUUnit2Distribution = new PTUUnit2Distribution();
                oPTUUnit2Distribution.ErrorMessage = "Failed to Operation because of " + e.Message.Split('!')[0];
                #endregion
            }

            return oPTUUnit2Distribution;
        }

        public List<PTUUnit2Distribution> Gets(int nUserID)
        {
            List<PTUUnit2Distribution> oPTUUnit2Distribution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PTUUnit2DistributionDA.Gets(tc);
                oPTUUnit2Distribution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUUnit2Distribution", e);
                #endregion
            }

            return oPTUUnit2Distribution;
        }

        public List<PTUUnit2Distribution> Gets(int nShelfID, int nUserID)
        {
            List<PTUUnit2Distribution> oPTUUnit2Distribution = new List<PTUUnit2Distribution>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PTUUnit2DistributionDA.Gets(tc, nShelfID);
                oPTUUnit2Distribution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUUnit2Distribution", e);
                #endregion
            }

            return oPTUUnit2Distribution;
        }
     
        public List<PTUUnit2Distribution> Gets(string sSQL, int nUserID)
        {
            List<PTUUnit2Distribution> oPTUUnit2Distribution = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_PTUUnit2Distribution where PTUUnit2DistributionID in (1,2,80,272,347,370,60,45)";
                }
                reader = PTUUnit2DistributionDA.Gets(tc, sSQL);
                oPTUUnit2Distribution = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PTUUnit2Distribution", e);
                #endregion
            }

            return oPTUUnit2Distribution;
        }

        public List<PTUUnit2Distribution> ConfirmPTUUnit2Distribution(List<PTUUnit2Distribution> oPTUUnit2Distributions, int nLotID, Int64 nUserID)
        {
            Lot oLot = new Lot();
            TransactionContext tc = null;
            PTUUnit2Distribution objPTUUnit2Distribution = new PTUUnit2Distribution();
            List<PTUUnit2Distribution> objPTUUnit2Distributions = new List<PTUUnit2Distribution>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                
                reader = LotDA.Get(tc, nLotID, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLot = new Lot();
                    oLot = LotService.CreateObject(oReader);
                }
                reader.Close();
                double dBalance = 0;
                foreach(PTUUnit2Distribution obj in oPTUUnit2Distributions)
                {
                    dBalance = dBalance + obj.Qty;
                }

                double nBalance = (oLot.Balance - dBalance);
                if (nBalance < 0)
                {
                    nBalance = (nBalance * (-1));
                }
                if (nBalance > 0.5)
                {
                    throw new Exception("Lot Balance & PI Wise Tag Balance Must Be Equal!");
                }
                IDataReader readerPTUUnit2 = null;
                foreach (PTUUnit2Distribution temp in oPTUUnit2Distributions)
                {
                    readerPTUUnit2 = PTUUnit2DistributionDA.ConfirmPTUUnit2Distribution(tc, temp, nUserID);
                    NullHandler oReaderPTUUnit2 = new NullHandler(readerPTUUnit2);
                    if (readerPTUUnit2.Read())
                    {
                        objPTUUnit2Distribution = new PTUUnit2Distribution();
                        objPTUUnit2Distribution = CreateObject(oReaderPTUUnit2);
                        objPTUUnit2Distributions.Add(objPTUUnit2Distribution);
                    }
                    readerPTUUnit2.Close();
                }
                tc.End();
              }
               catch (Exception e)
                {
                    #region Handle Exception
                    if (tc != null)
                        tc.HandleError();
                    objPTUUnit2Distributions = new List<PTUUnit2Distribution>();
                    PTUUnit2Distribution oPTUUnit2Distribution = new PTUUnit2Distribution();
                    oPTUUnit2Distribution.ErrorMessage = e.Message;
                    objPTUUnit2Distributions.Add(oPTUUnit2Distribution);

                    //ExceptionLog.Write(e);
                    //throw new ServiceException("Failed to Save List . Because of " + e.Message, e);
                    #endregion
                }
                return objPTUUnit2Distributions;
        }
        #endregion
    }   
    
    
   
}
