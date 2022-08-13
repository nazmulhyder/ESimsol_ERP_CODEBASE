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
    public class ReturnChallanRegisterService : MarshalByRefObject, IReturnChallanRegisterService
    {
        #region Private functions and declaration
        private ReturnChallanRegister MapObject(NullHandler oReader)
        {
            ReturnChallanRegister oReturnChallanDetail = new ReturnChallanRegister();
            oReturnChallanDetail.ReturnChallanDetailID = oReader.GetInt32("ReturnChallanDetailID");
            oReturnChallanDetail.ReturnChallanID = oReader.GetInt32("ReturnChallanID");
            oReturnChallanDetail.DeliveryChallanDetailID = oReader.GetInt32("DeliveryChallanDetailID");
            oReturnChallanDetail.DeliveryChallanID = oReader.GetInt32("DeliveryChallanID");
            oReturnChallanDetail.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            
            oReturnChallanDetail.BUID = oReader.GetInt32("BUID");
            oReturnChallanDetail.LotID = oReader.GetInt32("LotID");
            oReturnChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oReturnChallanDetail.MUnitID = oReader.GetInt32("MUnitID");
            oReturnChallanDetail.Qty = oReader.GetDouble("Qty");
            oReturnChallanDetail.Note = oReader.GetString("Note");
            oReturnChallanDetail.ContractorName = oReader.GetString("ContractorName");
            oReturnChallanDetail.ReceivedByName = oReader.GetString("ReceivedByName");
            oReturnChallanDetail.Remarks = oReader.GetString("Remarks");            
            oReturnChallanDetail.DeliveryChallanNo = oReader.GetString("DeliveryChallanNo");
            oReturnChallanDetail.PINo = oReader.GetString("PINo");
            oReturnChallanDetail.ProductName = oReader.GetString("ProductName");
            oReturnChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oReturnChallanDetail.MUnit = oReader.GetString("MUnit");
            oReturnChallanDetail.LotNo = oReader.GetString("LotNo");
            oReturnChallanDetail.DONo = oReader.GetString("DONo");
            oReturnChallanDetail.DeliveryChallanQty = oReader.GetDouble("DeliveryChallanQty");
            oReturnChallanDetail.YetToReturnQty = oReader.GetDouble("YetToReturnQty");
            oReturnChallanDetail.ReturnChallanNo = oReader.GetString("ReturnChallanNo");
            oReturnChallanDetail.Symbol = oReader.GetString("Symbol");
            oReturnChallanDetail.ReturnDate = oReader.GetDateTime("ReturnDate");
            
            return oReturnChallanDetail;
        }

        private ReturnChallanRegister CreateObject(NullHandler oReader)
        {
            ReturnChallanRegister oReturnChallanDetail = new ReturnChallanRegister();
            oReturnChallanDetail = MapObject(oReader);
            return oReturnChallanDetail;
        }

        private List<ReturnChallanRegister> CreateObjects(IDataReader oReader)
        {
            List<ReturnChallanRegister> oReturnChallanDetail = new List<ReturnChallanRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ReturnChallanRegister oItem = CreateObject(oHandler);
                oReturnChallanDetail.Add(oItem);
            }
            return oReturnChallanDetail;
        }

        #endregion

        #region Interface implementation

     

        public List<ReturnChallanRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ReturnChallanRegister> oReturnChallanDetails = new List<ReturnChallanRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ReturnChallanRegisterDA.Gets(tc, sSQL);
                oReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ReturnChallanRegister", e);
                #endregion
            }
            return oReturnChallanDetails;
        }

        #endregion
    }

}
