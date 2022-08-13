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
    public class PurchaseReturnRegisterService : MarshalByRefObject, IPurchaseReturnRegisterService
    {
        #region Private functions and declaration
        private PurchaseReturnRegister MapObject(NullHandler oReader)
        {
            PurchaseReturnRegister oPurchaseReturnRegister = new PurchaseReturnRegister();
            oPurchaseReturnRegister.PurchaseReturnID = oReader.GetInt32("PurchaseReturnID");
            oPurchaseReturnRegister.ProductID = oReader.GetInt32("ProductID");
            oPurchaseReturnRegister.LotID = oReader.GetInt32("LotID");
            oPurchaseReturnRegister.SupplierID = oReader.GetInt32("SupplierID");
            oPurchaseReturnRegister.PurchaseReturnNo = oReader.GetString("PurchaseReturnNo");
            oPurchaseReturnRegister.ProductName = oReader.GetString("ProductName");
            oPurchaseReturnRegister.LotNo = oReader.GetString("LotNo");
            oPurchaseReturnRegister.StoreName = oReader.GetString("StoreName");
            oPurchaseReturnRegister.SupplierName = oReader.GetString("SupplierName");
            oPurchaseReturnRegister.LCNo = oReader.GetString("LCNo");
            oPurchaseReturnRegister.StyleNo = oReader.GetString("StyleNo");
            oPurchaseReturnRegister.MUSymbol = oReader.GetString("MUSymbol");
            oPurchaseReturnRegister.RefNo = oReader.GetString("RefNo");
            oPurchaseReturnRegister.Qty = oReader.GetDouble("Qty");
            oPurchaseReturnRegister.Rate = oReader.GetDouble("Rate");
            oPurchaseReturnRegister.RefType = (EnumPurchaseReturnType)oReader.GetInt32("RefType");
            oPurchaseReturnRegister.ReturnDate = oReader.GetDateTime("ReturnDate");
            oPurchaseReturnRegister.RefDate = oReader.GetDateTime("RefDate");
            return oPurchaseReturnRegister;
        }

        private PurchaseReturnRegister CreateObject(NullHandler oReader)
        {
            PurchaseReturnRegister oPurchaseReturnRegister = new PurchaseReturnRegister();
            oPurchaseReturnRegister = MapObject(oReader);
            return oPurchaseReturnRegister;
        }

        private List<PurchaseReturnRegister> CreateObjects(IDataReader oReader)
        {
            List<PurchaseReturnRegister> oPurchaseReturnRegister = new List<PurchaseReturnRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseReturnRegister oItem = CreateObject(oHandler);
                oPurchaseReturnRegister.Add(oItem);
            }
            return oPurchaseReturnRegister;
        }

        #endregion

        #region Interface implementation
        public PurchaseReturnRegisterService() { }

        public List<PurchaseReturnRegister> GetsPurchaseReturnRegister(string sSQL, Int64 nUserID)
        {
            List<PurchaseReturnRegister> oPurchaseReturnRegisters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseReturnRegisterDA.GetsPurchaseReturnRegister(tc, sSQL);
                oPurchaseReturnRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseReturnRegister", e);
                #endregion
            }
            return oPurchaseReturnRegisters;
        }
        #endregion
    }
}
