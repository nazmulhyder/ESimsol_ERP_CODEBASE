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
    public class KnittingOrderRegisterService : MarshalByRefObject, IKnittingOrderRegisterService
    {
        #region Private functions and declaration

        private KnittingOrderRegister MapObject(NullHandler oReader)
        {
            KnittingOrderRegister oKnittingOrderRegister = new KnittingOrderRegister();
            oKnittingOrderRegister.KnittingOrderID = oReader.GetInt32("KnittingOrderID");
            oKnittingOrderRegister.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
            oKnittingOrderRegister.OrderDate = oReader.GetDateTime("OrderDate");
            oKnittingOrderRegister.StartDate = oReader.GetDateTime("StartDate");
            oKnittingOrderRegister.ApproxCompleteDate = oReader.GetDateTime("ApproxCompleteDate");
            oKnittingOrderRegister.BusinessSessionName = oReader.GetString("BusinessSessionName");
            oKnittingOrderRegister.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oKnittingOrderRegister.FactoryID = oReader.GetInt32("FactoryID");
            oKnittingOrderRegister.FactoryName = oReader.GetString("FactoryName");

            oKnittingOrderRegister.KnittingYarnID = oReader.GetInt32("KnittingYarnID");
            oKnittingOrderRegister.YarnID = oReader.GetInt32("YarnID");
            oKnittingOrderRegister.YarnName = oReader.GetString("YarnName");
            oKnittingOrderRegister.YarnCode = oReader.GetString("YarnCode");
            oKnittingOrderRegister.YarnChallanQty = oReader.GetDouble("YarnChallanQty");
            oKnittingOrderRegister.YarnMUnitID = oReader.GetInt32("YarnMUnitID");
            oKnittingOrderRegister.YarnMUnitSymbol = oReader.GetString("YarnMUnitSymbol");
            oKnittingOrderRegister.YarnConsumptionQty = oReader.GetDouble("YarnConsumptionQty");
            oKnittingOrderRegister.YarnReturnQty = oReader.GetDouble("YarnReturnQty");
            oKnittingOrderRegister.YarnProcessLossQty = oReader.GetDouble("YarnProcessLossQty");
            oKnittingOrderRegister.YarnBalanceQty = oReader.GetDouble("YarnBalanceQty");

            oKnittingOrderRegister.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oKnittingOrderRegister.FabricID = oReader.GetInt32("FabricID");
            oKnittingOrderRegister.FabricName = oReader.GetString("FabricName");
            oKnittingOrderRegister.StyleNo = oReader.GetString("StyleNo");
            oKnittingOrderRegister.PAM = oReader.GetString("PAM");
            oKnittingOrderRegister.StyleID = oReader.GetInt32("StyleID");
            oKnittingOrderRegister.OrderType = (EnumKnittingOrderType)oReader.GetInt32("OrderType");
            oKnittingOrderRegister.BuyerName = oReader.GetString("BuyerName");
            oKnittingOrderRegister.GSM = oReader.GetString("GSM");
            oKnittingOrderRegister.MICDia = oReader.GetString("MICDia");
            oKnittingOrderRegister.FinishDia = oReader.GetString("FinishDia");
            oKnittingOrderRegister.ColorID = oReader.GetInt32("ColorID");
            oKnittingOrderRegister.ColorName = oReader.GetString("ColorName");
            oKnittingOrderRegister.FabricQty = oReader.GetDouble("FabricQty");
            oKnittingOrderRegister.FabricUnitPrice = oReader.GetDouble("FabricUnitPrice");
            oKnittingOrderRegister.FabricMUnitID = oReader.GetInt32("FabricMUnitID");
            oKnittingOrderRegister.FabricMUnitSymbol = oReader.GetString("FabricMUnitSymbol");
            oKnittingOrderRegister.FabricAmount = oReader.GetDouble("FabricAmount");
            oKnittingOrderRegister.BrandName = oReader.GetString("BrandName");
            oKnittingOrderRegister.FabricRecvQty = oReader.GetDouble("FabricRecvQty");
            oKnittingOrderRegister.FabricStyleQty = oReader.GetDouble("FabricStyleQty");
            oKnittingOrderRegister.FabricYetRecvQty = oReader.GetDouble("FabricYetRecvQty");

            return oKnittingOrderRegister;
        }

        private KnittingOrderRegister CreateObject(NullHandler oReader)
        {
            KnittingOrderRegister oKnittingOrderRegister = new KnittingOrderRegister();
            oKnittingOrderRegister = MapObject(oReader);
            return oKnittingOrderRegister;
        }

        private List<KnittingOrderRegister> CreateObjects(IDataReader oReader)
        {
            List<KnittingOrderRegister> oKnittingOrderRegister = new List<KnittingOrderRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                KnittingOrderRegister oItem = CreateObject(oHandler);
                oKnittingOrderRegister.Add(oItem);
            }
            return oKnittingOrderRegister;
        }

        #endregion

        #region Interface implementation


        public KnittingOrderRegister Get(int id, Int64 nUserId)
        {
            KnittingOrderRegister oKnittingOrderRegister = new KnittingOrderRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = KnittingOrderRegisterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnittingOrderRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get KnittingOrderRegister", e);
                #endregion
            }
            return oKnittingOrderRegister;
        }

        public List<KnittingOrderRegister> Gets(int nKnittingOrderID, Int64 nUserID)
        {
            List<KnittingOrderRegister> oKnittingOrderRegisters = new List<KnittingOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderRegisterDA.Gets(tc, nKnittingOrderID);
                oKnittingOrderRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                KnittingOrderRegister oKnittingOrderRegister = new KnittingOrderRegister();
                oKnittingOrderRegister.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oKnittingOrderRegisters;
        }

        public List<KnittingOrderRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<KnittingOrderRegister> oKnittingOrderRegisters = new List<KnittingOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderRegisterDA.Gets(tc, sSQL);
                oKnittingOrderRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingOrderRegister", e);
                #endregion
            }
            return oKnittingOrderRegisters;
        }

        public List<KnittingOrderRegister> GetsForOrderStatusWise(string sSQL, Int64 nUserID)
        {
            List<KnittingOrderRegister> oKnittingOrderRegisters = new List<KnittingOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnittingOrderRegisterDA.GetsForOrderStatusWise(tc, sSQL);
                oKnittingOrderRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnittingOrderRegister", e);
                #endregion
            }
            return oKnittingOrderRegisters;
        }

        #endregion 
    }

}
