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
    public class DORegisterService : MarshalByRefObject, IDORegisterService
    {
        #region Private functions and declaration
        private DORegister MapObject(NullHandler oReader)
        {
            DORegister oDORegister = new DORegister();
            oDORegister.ExportPIID = oReader.GetInt32("ExportPIID");
            oDORegister.PINo = oReader.GetString("PINo");
            oDORegister.PIDate = oReader.GetDateTime("PIDate");
            oDORegister.CustomerName = oReader.GetString("CustomerName");
            oDORegister.BuyerName = oReader.GetString("BuyerName");
            oDORegister.ProductName = oReader.GetString("MKTPName");
            oDORegister.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oDORegister.ProductName = oReader.GetString("ProductName");
            oDORegister.ColorName = oReader.GetString("ColorName");
            oDORegister.StyleNo = oReader.GetString("StyleNo");
            oDORegister.LCFileNo = oReader.GetString("LCFileNo");
            oDORegister.MUnit = oReader.GetString("MUnit");
            oDORegister.Qty = oReader.GetDouble("Qty");
            oDORegister.ChallanQty = oReader.GetDouble("ChallanQty");
            oDORegister.PIQty = oReader.GetDouble("PIQty");
            oDORegister.DOQty = oReader.GetDouble("DOQty");
            oDORegister.YetToDO = oReader.GetDouble("YetToDO");
            oDORegister.YetToChallan = oReader.GetDouble("YetToChallan");
            

            oDORegister.ErrorMessage = "";
            return oDORegister;
        }
        private DORegister CreateObject(NullHandler oReader)
        {
            DORegister oDORegister = new DORegister();
            oDORegister = MapObject(oReader);
            return oDORegister;
        }

        private List<DORegister> CreateObjects(IDataReader oReader)
        {
            List<DORegister> oDORegister = new List<DORegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DORegister oItem = CreateObject(oHandler);
                oDORegister.Add(oItem);
            }
            return oDORegister;
        }

        #endregion

        #region Interface implementation
        public DORegisterService() { }
        public List<DORegister> Gets(string sSQL, int nLayout, Int64 nUserID)
        {
            List<DORegister> oDORegisters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DORegisterDA.Gets(tc, sSQL, nLayout);
                oDORegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DORegister", e);
                #endregion
            }
            return oDORegisters;
        }
        #endregion
    }   
}
