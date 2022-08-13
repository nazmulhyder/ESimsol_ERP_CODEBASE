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
    public class PerformanceIncentiveSlabService : MarshalByRefObject, IPerformanceIncentiveSlabService
    {
        #region Private functions and declaration
        private PerformanceIncentiveSlab MapObject(NullHandler oReader)
        {
            PerformanceIncentiveSlab oPerformanceIncentiveSlab = new PerformanceIncentiveSlab();

            oPerformanceIncentiveSlab.PISlabID = oReader.GetInt32("PISlabID");
            oPerformanceIncentiveSlab.PIID = oReader.GetInt32("PIID");
            oPerformanceIncentiveSlab.MinPoint = oReader.GetDouble("MinPoint");
            oPerformanceIncentiveSlab.MaxPoint = oReader.GetDouble("MaxPoint");
            oPerformanceIncentiveSlab.Value = oReader.GetDouble("Value");
            oPerformanceIncentiveSlab.IsPercentOfRate = oReader.GetBoolean("IsPercentOfRate");
            
            return oPerformanceIncentiveSlab;

        }

        private PerformanceIncentiveSlab CreateObject(NullHandler oReader)
        {
            PerformanceIncentiveSlab oPerformanceIncentiveSlab = MapObject(oReader);
            return oPerformanceIncentiveSlab;
        }

        private List<PerformanceIncentiveSlab> CreateObjects(IDataReader oReader)
        {
            List<PerformanceIncentiveSlab> oPerformanceIncentiveSlabs = new List<PerformanceIncentiveSlab>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PerformanceIncentiveSlab oItem = CreateObject(oHandler);
                oPerformanceIncentiveSlabs.Add(oItem);
            }
            return oPerformanceIncentiveSlabs;
        }

        #endregion

        #region Interface implementation
        public PerformanceIncentiveSlabService() { }

        public PerformanceIncentiveSlab IUD(PerformanceIncentiveSlab oPerformanceIncentiveSlab, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = PerformanceIncentiveSlabDA.IUD(tc, oPerformanceIncentiveSlab, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveSlab = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oPerformanceIncentiveSlab.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPerformanceIncentiveSlab.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oPerformanceIncentiveSlab.PIID = 0;
                #endregion
            }
            return oPerformanceIncentiveSlab;
        }


        public PerformanceIncentiveSlab Get(int nPIID, Int64 nUserId)
        {
            PerformanceIncentiveSlab oPerformanceIncentiveSlab = new PerformanceIncentiveSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveSlabDA.Get(nPIID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveSlab = CreateObject(oReader);
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

                oPerformanceIncentiveSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveSlab;
        }

        public PerformanceIncentiveSlab Get(string sSQL, Int64 nUserId)
        {
            PerformanceIncentiveSlab oPerformanceIncentiveSlab = new PerformanceIncentiveSlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PerformanceIncentiveSlabDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPerformanceIncentiveSlab = CreateObject(oReader);
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

                oPerformanceIncentiveSlab.ErrorMessage = e.Message;
                #endregion
            }

            return oPerformanceIncentiveSlab;
        }

        public List<PerformanceIncentiveSlab> Gets(Int64 nUserID)
        {
            List<PerformanceIncentiveSlab> oPerformanceIncentiveSlab = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveSlabDA.Gets(tc);
                oPerformanceIncentiveSlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentiveSlab", e);
                #endregion
            }
            return oPerformanceIncentiveSlab;
        }

        public List<PerformanceIncentiveSlab> Gets(string sSQL, Int64 nUserID)
        {
            List<PerformanceIncentiveSlab> oPerformanceIncentiveSlab = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PerformanceIncentiveSlabDA.Gets(sSQL, tc);
                oPerformanceIncentiveSlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PerformanceIncentiveSlab", e);
                #endregion
            }
            return oPerformanceIncentiveSlab;
        }

        #endregion


    }
}
