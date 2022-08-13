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
    public class CostSheetPackageDetailService : MarshalByRefObject, ICostSheetPackageDetailService
    {
        #region Private functions and declaration
        private CostSheetPackageDetail MapObject(NullHandler oReader)
        {
            CostSheetPackageDetail oCostSheetPackageDetail = new CostSheetPackageDetail();

            oCostSheetPackageDetail.CostSheetPackageDetailID = oReader.GetInt32("CostSheetPackageDetailID");
            oCostSheetPackageDetail.CostSheetPackageID = oReader.GetInt32("CostSheetPackageID");
            oCostSheetPackageDetail.ProductID = oReader.GetInt32("ProductID");
            oCostSheetPackageDetail.ProductCode = oReader.GetString("ProductCode");
            oCostSheetPackageDetail.ProductName = oReader.GetString("ProductName");
            oCostSheetPackageDetail.Description = oReader.GetString("Description");
            return oCostSheetPackageDetail;
        }

        private CostSheetPackageDetail CreateObject(NullHandler oReader)
        {
            CostSheetPackageDetail oCostSheetPackageDetail = new CostSheetPackageDetail();
            oCostSheetPackageDetail = MapObject(oReader);
            return oCostSheetPackageDetail;
        }

        private List<CostSheetPackageDetail> CreateObjects(IDataReader oReader)
        {
            List<CostSheetPackageDetail> oCostSheetPackageDetail = new List<CostSheetPackageDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostSheetPackageDetail oItem = CreateObject(oHandler);
                oCostSheetPackageDetail.Add(oItem);
            }
            return oCostSheetPackageDetail;
        }

        #endregion

        #region Interface implementation
        public CostSheetPackageDetailService() { }



        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CostSheetPackageDetail oCostSheetPackageDetail = new CostSheetPackageDetail();
                oCostSheetPackageDetail.CostSheetPackageDetailID = id;
                CostSheetPackageDetailDA.Delete(tc, oCostSheetPackageDetail, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete CostSheetPackageDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public CostSheetPackageDetail Get(int id, Int64 nUserId)
        {
            CostSheetPackageDetail oAccountHead = new CostSheetPackageDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CostSheetPackageDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CostSheetPackageDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CostSheetPackageDetail> Gets(Int64 nUserID)
        {
            List<CostSheetPackageDetail> oCostSheetPackageDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetPackageDetailDA.Gets(tc);
                oCostSheetPackageDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSheetPackageDetail", e);
                #endregion
            }

            return oCostSheetPackageDetail;
        }

        public List<CostSheetPackageDetail> GetsByCostSheetID(int id, Int64 nUserID)
        {
            List<CostSheetPackageDetail> oCostSheetPackageDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSheetPackageDetailDA.GetsByCostSheetID(id, tc);
                oCostSheetPackageDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Order Negotiation Detail", e);
                #endregion
            }

            return oCostSheetPackageDetails;
        }


        #endregion
    }   

    
   
}
