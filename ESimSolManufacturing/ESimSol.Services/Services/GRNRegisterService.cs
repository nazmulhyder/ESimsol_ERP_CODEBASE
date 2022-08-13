using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class GRNRegisterService : MarshalByRefObject, IGRNRegisterService
    {
        #region Private functions and declaration
        private GRNRegister MapObject(NullHandler oReader)
        {
            GRNRegister oGRNRegister = new GRNRegister();
            oGRNRegister.GRNDetailID = oReader.GetInt32("GRNDetailID");
            oGRNRegister.GRNID = oReader.GetInt32("GRNID");
            oGRNRegister.ProductID = oReader.GetInt32("ProductID");
            oGRNRegister.MUnitID = oReader.GetInt32("MUnitID");
            oGRNRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oGRNRegister.ReceivedQty = oReader.GetDouble("ReceivedQty");
            oGRNRegister.Amount = oReader.GetDouble("Amount");
            oGRNRegister.LotNo = oReader.GetString("LotNo");
            oGRNRegister.ColorName = oReader.GetString("ColorName");
            oGRNRegister.StoreName = oReader.GetString("StoreName");
            oGRNRegister.GRNNo = oReader.GetString("GRNNo");
            oGRNRegister.BUID = oReader.GetInt32("BUID");
            oGRNRegister.ChallanNo = oReader.GetString("ChallanNo");
            oGRNRegister.StyleNo = oReader.GetString("StyleNo");
            oGRNRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            oGRNRegister.GRNStatus = (EnumGRNStatus)oReader.GetInt32("GRNStatus");
            oGRNRegister.ContractorID = oReader.GetInt32("ContractorID");
            oGRNRegister.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oGRNRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oGRNRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oGRNRegister.GRNDate = oReader.GetDateTime("GRNDate");
            oGRNRegister.GLDate = oReader.GetDateTime("GLDate");

            oGRNRegister.GRNType = (EnumGRNType)oReader.GetInt32("GRNType");
            oGRNRegister.Remarks = oReader.GetString("Remarks");
            oGRNRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oGRNRegister.LCNo = oReader.GetString("LCNo");
            oGRNRegister.ProductCode = oReader.GetString("ProductCode");
            oGRNRegister.ProductName = oReader.GetString("ProductName");
            oGRNRegister.MUName = oReader.GetString("MUName");
            oGRNRegister.MUSymbol = oReader.GetString("MUSymbol");
            oGRNRegister.SupplierName = oReader.GetString("SupplierName");
            oGRNRegister.ReceivedByName = oReader.GetString("ReceivedByName");
            oGRNRegister.CurrencyName = oReader.GetString("CurrencyName");
            oGRNRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oGRNRegister.ConversionRate = oReader.GetInt32("RateUnit");
            oGRNRegister.CRate = oReader.GetInt32("CRate");
            oGRNRegister.RefQty = oReader.GetDouble("RefQty");
            oGRNRegister.ExtraQty = oReader.GetDouble("ExtraQty");

            oGRNRegister.BuyerID = oReader.GetInt32("BuyerID");
            oGRNRegister.BuyerName = oReader.GetString("BuyerName");
            oGRNRegister.RefObjectNo = oReader.GetString("RefObjectNo");
            oGRNRegister.BrandName = oReader.GetString("BrandName");
            oGRNRegister.PINo = oReader.GetString("PINo");
            oGRNRegister.PIQty = oReader.GetDouble("PIQty");
            oGRNRegister.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
            oGRNRegister.ProductTypeSt = oReader.GetString("ProductTypeSt");
            oGRNRegister.GroupName = oReader.GetString("GroupName");
            oGRNRegister.GatePassNo = oReader.GetString("GatePassNo");
            oGRNRegister.MRIRNo = oReader.GetString("MRIRNo");
            
            return oGRNRegister;
        }

        private GRNRegister CreateObject(NullHandler oReader)
        {
            GRNRegister oGRNRegister = new GRNRegister();
            oGRNRegister = MapObject(oReader);
            return oGRNRegister;
        }

        private List<GRNRegister> CreateObjects(IDataReader oReader)
        {
            List<GRNRegister> oGRNRegister = new List<GRNRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GRNRegister oItem = CreateObject(oHandler);
                oGRNRegister.Add(oItem);
            }
            return oGRNRegister;
        }

        #endregion

        #region Interface implementation
        public GRNRegisterService() { }        
        public List<GRNRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<GRNRegister> oGRNRegister = new List<GRNRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GRNRegisterDA.Gets(tc, sSQL);
                oGRNRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNRegister", e);
                #endregion
            }

            return oGRNRegister;
        }
        public List<GRNRegister> GetsTwo(string sSQL, Int64 nUserID)
        {
            List<GRNRegister> oGRNRegister = new List<GRNRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = GRNRegisterDA.GetsTwo(tc, sSQL);
                oGRNRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GRNRegister", e);
                #endregion
            }

            return oGRNRegister;
        }
        #endregion
    }
}
