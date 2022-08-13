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
    public class YarnChallanRegisterService : MarshalByRefObject, IYarnChallanRegisterService
    {
        #region Private functions and declaration

        private YarnChallanRegister MapObject(NullHandler oReader)
        {
            YarnChallanRegister oYarnChallanRegister = new YarnChallanRegister();
            oYarnChallanRegister.KnittingYarnChallanDetailID = oReader.GetInt32("KnittingYarnChallanDetailID");
            oYarnChallanRegister.KnittingYarnChallanID = oReader.GetInt32("KnittingYarnChallanID");
            oYarnChallanRegister.KnittingOrderDetailID = oReader.GetInt32("KnittingOrderDetailID");
            oYarnChallanRegister.BUID = oReader.GetInt32("BUID");
            oYarnChallanRegister.ChallanNo = oReader.GetString("ChallanNo");
            oYarnChallanRegister.ChallanDate = oReader.GetDateTime("ChallanDate");
            oYarnChallanRegister.StyleID = oReader.GetInt32("StyleID");
            oYarnChallanRegister.StyleNo = oReader.GetString("StyleNo");
            oYarnChallanRegister.BuyerID = oReader.GetInt32("BuyerID");
            oYarnChallanRegister.BuyerName = oReader.GetString("BuyerName");
            oYarnChallanRegister.FactoryID = oReader.GetInt32("FactoryID");
            oYarnChallanRegister.FactoryName = oReader.GetString("FactoryName");
            oYarnChallanRegister.ProductID = oReader.GetInt32("ProductID");
            oYarnChallanRegister.ProductName = oReader.GetString("ProductName");
            oYarnChallanRegister.ProductCode = oReader.GetString("ProductCode");
            oYarnChallanRegister.ColorID = oReader.GetInt32("ColorID");
            oYarnChallanRegister.ColorName = oReader.GetString("ColorName");
            oYarnChallanRegister.Brand = oReader.GetString("Brand");
            oYarnChallanRegister.BrandShortName = oReader.GetString("BrandShortName");
            oYarnChallanRegister.LotID = oReader.GetInt32("LotID");
            oYarnChallanRegister.LotNo = oReader.GetString("LotNo");
            oYarnChallanRegister.StoreID = oReader.GetInt32("StoreID");
            oYarnChallanRegister.StoreName = oReader.GetString("StoreName");
            oYarnChallanRegister.MUnitID = oReader.GetInt32("MUnitID");
            oYarnChallanRegister.MUShortName = oReader.GetString("MUShortName");
            oYarnChallanRegister.Qty = oReader.GetDouble("Qty");
            oYarnChallanRegister.BagQty = oReader.GetDouble("BagQty");
            oYarnChallanRegister.KnittingOrderNo = oReader.GetString("KnittingOrderNo");
            oYarnChallanRegister.PAM = oReader.GetString("PAM");
            oYarnChallanRegister.ReportLayout = (EnumReportLayout)oReader.GetInt32("ReportLayout");

            return oYarnChallanRegister;
        }

        private YarnChallanRegister CreateObject(NullHandler oReader)
        {
            YarnChallanRegister oYarnChallanRegister = new YarnChallanRegister();
            oYarnChallanRegister = MapObject(oReader);
            return oYarnChallanRegister;
        }

        private List<YarnChallanRegister> CreateObjects(IDataReader oReader)
        {
            List<YarnChallanRegister> oYarnChallanRegister = new List<YarnChallanRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                YarnChallanRegister oItem = CreateObject(oHandler);
                oYarnChallanRegister.Add(oItem);
            }
            return oYarnChallanRegister;
        }

        #endregion

        #region Interface implementation
        public List<YarnChallanRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<YarnChallanRegister> oYarnChallanRegisters = new List<YarnChallanRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnChallanRegisterDA.Gets(tc, sSQL);
                oYarnChallanRegisters = CreateObjects(reader);
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
            return oYarnChallanRegisters;
        }

        #endregion
    }

}
