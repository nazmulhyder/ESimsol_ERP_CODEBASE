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
    public class FGQCDetailService : MarshalByRefObject, IFGQCDetailService
    {
        #region Private functions and declaration

        private FGQCDetail MapObject(NullHandler oReader)
        {
            FGQCDetail oFGQCDetail = new FGQCDetail();
            oFGQCDetail.FGQCDetailID = oReader.GetInt32("FGQCDetailID");
            oFGQCDetail.FGQCID = oReader.GetInt32("FGQCID");
            oFGQCDetail.RefType = (EnumFGQCRefType)oReader.GetInt32("RefType");
            oFGQCDetail.RefTypeInt = oReader.GetInt32("RefType");
            oFGQCDetail.RefID = oReader.GetInt32("RefID");
            oFGQCDetail.ProductID = oReader.GetInt32("ProductID");
            oFGQCDetail.MUnitID = oReader.GetInt32("MUnitID");
            oFGQCDetail.Qty = oReader.GetDouble("Qty");
            oFGQCDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFGQCDetail.StoreID = oReader.GetInt32("StoreID");
            oFGQCDetail.ProductCode = oReader.GetString("ProductCode");
            oFGQCDetail.ProductName = oReader.GetString("ProductName");
            oFGQCDetail.MUName = oReader.GetString("MUName");
            oFGQCDetail.RefNo = oReader.GetString("RefNo");
            oFGQCDetail.StoreName = oReader.GetString("StoreName");
            return oFGQCDetail;
        }

        private FGQCDetail CreateObject(NullHandler oReader)
        {
            FGQCDetail oFGQCDetail = new FGQCDetail();
            oFGQCDetail = MapObject(oReader);
            return oFGQCDetail;
        }

        private List<FGQCDetail> CreateObjects(IDataReader oReader)
        {
            List<FGQCDetail> oFGQCDetail = new List<FGQCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FGQCDetail oItem = CreateObject(oHandler);
                oFGQCDetail.Add(oItem);
            }
            return oFGQCDetail;
        }

        #endregion

        #region Interface implementation

        public List<FGQCDetail> Gets(int id, Int64 nUserID)
        {
            List<FGQCDetail> oFGQCDetails = new List<FGQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FGQCDetailDA.Gets(id, tc);
                oFGQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FGQCDetail oFGQCDetail = new FGQCDetail();
                oFGQCDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFGQCDetails;
        }

        public List<FGQCDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FGQCDetail> oFGQCDetails = new List<FGQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FGQCDetailDA.Gets(tc, sSQL);
                oFGQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FGQCDetail", e);
                #endregion
            }
            return oFGQCDetails;
        }

        public List<FGQCDetail> FGQCProcess(FGQC oFGQC, Int64 nUserID)
        {
            List<FGQCDetail> oFGQCDetails = new List<FGQCDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FGQCDetailDA.FGQCProcess(tc, oFGQC);
                oFGQCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FGQCDetail", e);
                #endregion
            }
            return oFGQCDetails;
        }

        #endregion
    }
}
