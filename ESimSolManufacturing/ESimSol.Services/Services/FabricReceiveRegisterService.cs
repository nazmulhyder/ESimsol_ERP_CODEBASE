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
  public  class FabricReceiveRegisterService : MarshalByRefObject, IFabricReceiveRegisterService
    {
      private FabricReceiveRegister MapObject(NullHandler oReader)
      {
          FabricReceiveRegister oFabricReceiveRegister = new FabricReceiveRegister();
          oFabricReceiveRegister.KnittingFabricReceiveDetailID = oReader.GetInt32("KnittingFabricReceiveDetailID");
          oFabricReceiveRegister.KnittingFabricReceiveID = oReader.GetInt32("KnittingFabricReceiveID");
          oFabricReceiveRegister.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
          oFabricReceiveRegister.FabricID = oReader.GetInt32("FabricID");
          oFabricReceiveRegister.ReceiveStoreID = oReader.GetInt32("ReceiveStoreID");
          oFabricReceiveRegister.LotID = oReader.GetInt32("LotID");
          oFabricReceiveRegister.NewLotNo = oReader.GetString("NewLotNo");
          oFabricReceiveRegister.MUnitID = oReader.GetInt32("MUnitID");
          oFabricReceiveRegister.Qty = oReader.GetDouble("Qty");
          oFabricReceiveRegister.ProcessLossQty = oReader.GetDouble("ProcessLossQty");
          oFabricReceiveRegister.FabricReceiveDetailsRemarks = oReader.GetString("FabricReceiveDetailsRemarks");
          oFabricReceiveRegister.GSM = oReader.GetString("GSM");
          oFabricReceiveRegister.MICDia = oReader.GetString("MICDia");
          oFabricReceiveRegister.FinishDia = oReader.GetString("FinishDia");
          oFabricReceiveRegister.MUnitName = oReader.GetString("MUnitName");
          oFabricReceiveRegister.OperationUnitName = oReader.GetString("OperationUnitName");
          oFabricReceiveRegister.FabricName = oReader.GetString("FabricName");
          oFabricReceiveRegister.FabricCode = oReader.GetString("FabricCode");
          oFabricReceiveRegister.LotNo = oReader.GetString("LotNo");
          oFabricReceiveRegister.LotBalance = oReader.GetDouble("LotBalance");
          oFabricReceiveRegister.LotMUSymbol = oReader.GetString("LotMUSymbol");
          oFabricReceiveRegister.PAM = oReader.GetInt32("PAM");
          oFabricReceiveRegister.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
          oFabricReceiveRegister.ReceiveNo = oReader.GetString("ReceiveNo");
          oFabricReceiveRegister.ReceiveDate = oReader.GetDateTime("ReceiveDate");
          oFabricReceiveRegister.PartyChallanNo = oReader.GetString("PartyChallanNo");
          oFabricReceiveRegister.FabricReceiveRemarks = oReader.GetString("FabricReceiveRemarks");
          oFabricReceiveRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
          oFabricReceiveRegister.ApprovedByName = oReader.GetString("ApprovedByName");
          oFabricReceiveRegister.BUID = oReader.GetInt32("BUID");
          oFabricReceiveRegister.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
          oFabricReceiveRegister.KnittingOrderDate = oReader.GetDateTime("KnittingOrderDate");
          oFabricReceiveRegister.BuyerName = oReader.GetString("BuyerName");
          oFabricReceiveRegister.StyleNo = oReader.GetString("StyleNo");
          oFabricReceiveRegister.OrderQty = oReader.GetDouble("OrderQty");
          oFabricReceiveRegister.BusinessSessionName = oReader.GetString("BusinessSessionName");
          oFabricReceiveRegister.OrderType = (EnumKnittingOrderType)oReader.GetInt32("OrderType");
          oFabricReceiveRegister.FactoryName = oReader.GetString("FactoryName");
          oFabricReceiveRegister.StartDate = oReader.GetDateTime("StartDate");
          oFabricReceiveRegister.FactoryID = oReader.GetInt32("FactoryID");
          oFabricReceiveRegister.StyleID = oReader.GetInt32("StyleID");
          oFabricReceiveRegister.BuyerID = oReader.GetInt32("BuyerID");

          return oFabricReceiveRegister;
      }
      private FabricReceiveRegister CreateObject(NullHandler oReader)
      {
          FabricReceiveRegister oFabricReceiveRegister = new FabricReceiveRegister();
          oFabricReceiveRegister = MapObject(oReader);
          return oFabricReceiveRegister;
      }
      private List<FabricReceiveRegister> CreateObjects(IDataReader oReader)
      {
          List<FabricReceiveRegister> oFabricReceiveRegister = new List<FabricReceiveRegister>();
          NullHandler oHandler = new NullHandler(oReader);
          while (oReader.Read())
          {
              FabricReceiveRegister oItem = CreateObject(oHandler);
              oFabricReceiveRegister.Add(oItem);
          }
          return oFabricReceiveRegister;
      }
      #region Interface implementation
      public List<FabricReceiveRegister> Gets(string sSQL, Int64 nUserID)
      {
          List<FabricReceiveRegister> oFabricReceiveRegisters = new List<FabricReceiveRegister>();
          TransactionContext tc = null;
          try
          {
              tc = TransactionContext.Begin();
              IDataReader reader = null;
              reader = FabricReceiveRegisterDA.Gets(tc, sSQL);
              oFabricReceiveRegisters = CreateObjects(reader);
              reader.Close();
              tc.End();
          }
          catch (Exception e)
          {
              #region Handle Exception
              if (tc != null)
                  tc.HandleError();
              ExceptionLog.Write(e);
              throw new ServiceException("Failed to Get YarnChallanRegister", e);
              #endregion
          }
          return oFabricReceiveRegisters;
      }

      #endregion
    }
}
