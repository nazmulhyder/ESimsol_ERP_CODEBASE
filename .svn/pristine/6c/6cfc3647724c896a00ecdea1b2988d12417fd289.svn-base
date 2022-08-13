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

    public class SparePartsRequisitionDetailService : MarshalByRefObject, ISparePartsRequisitionDetailService
    {
        #region Private functions and declaration
        private SparePartsRequisitionDetail MapObject(NullHandler oReader)
        {
            SparePartsRequisitionDetail oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
            oSparePartsRequisitionDetail.SparePartsRequisitionDetailID = oReader.GetInt32("SparePartsRequisitionDetailID");
            oSparePartsRequisitionDetail.SparePartsRequisitionID = oReader.GetInt32("SparePartsRequisitionID");
            oSparePartsRequisitionDetail.ProductID = oReader.GetInt32("ProductID");
            oSparePartsRequisitionDetail.UnitID = oReader.GetInt32("UnitID");
            oSparePartsRequisitionDetail.Quantity = oReader.GetDouble("Quantity");
            oSparePartsRequisitionDetail.TotalLotBalance = oReader.GetDouble("TotalLotBalance");
            oSparePartsRequisitionDetail.ProductCode = oReader.GetString("ProductCode");
            oSparePartsRequisitionDetail.ProductName = oReader.GetString("ProductName");
            oSparePartsRequisitionDetail.Remarks = oReader.GetString("Remarks");
            oSparePartsRequisitionDetail.SparePartsRequisitionDetailLogID = oReader.GetInt32("SparePartsRequisitionDetailLogID");
            oSparePartsRequisitionDetail.SparePartsRequisitionLogID = oReader.GetInt32("SparePartsRequisitionLogID");
            return oSparePartsRequisitionDetail;
        }

        private SparePartsRequisitionDetail CreateObject(NullHandler oReader)
        {
            SparePartsRequisitionDetail oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
            oSparePartsRequisitionDetail = MapObject(oReader);
            return oSparePartsRequisitionDetail;
        }

        private List<SparePartsRequisitionDetail> CreateObjects(IDataReader oReader)
        {
            List<SparePartsRequisitionDetail> oSparePartsRequisitionDetail = new List<SparePartsRequisitionDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SparePartsRequisitionDetail oItem = CreateObject(oHandler);
                oSparePartsRequisitionDetail.Add(oItem);
            }
            return oSparePartsRequisitionDetail;
        }

        #endregion

        #region Interface implementation
        public SparePartsRequisitionDetailService() { }

        public SparePartsRequisitionDetail Save(SparePartsRequisitionDetail oSparePartsRequisitionDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<SparePartsRequisitionDetail> _oSparePartsRequisitionDetails = new List<SparePartsRequisitionDetail>();
            oSparePartsRequisitionDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = SparePartsRequisitionDetailDA.InsertUpdate(tc, oSparePartsRequisitionDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSparePartsRequisitionDetail = new SparePartsRequisitionDetail();
                    oSparePartsRequisitionDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSparePartsRequisitionDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oSparePartsRequisitionDetail;
        }


        public SparePartsRequisitionDetail Get(int SparePartsRequisitionDetailID, Int64 nUserId)
        {
            SparePartsRequisitionDetail oAccountHead = new SparePartsRequisitionDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SparePartsRequisitionDetailDA.Get(tc, SparePartsRequisitionDetailID);
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
                throw new ServiceException("Failed to Get SparePartsRequisitionDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<SparePartsRequisitionDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<SparePartsRequisitionDetail> oSparePartsRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsRequisitionDetailDA.Gets(LabDipOrderID, tc);
                oSparePartsRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsRequisitionDetail", e);
                #endregion
            }

            return oSparePartsRequisitionDetail;
        }

        public List<SparePartsRequisitionDetail> GetsLog(int id, Int64 nUserID)
        {
            List<SparePartsRequisitionDetail> oSparePartsRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsRequisitionDetailDA.GetsLog(id, tc);
                oSparePartsRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsRequisitionDetail", e);
                #endregion
            }

            return oSparePartsRequisitionDetail;
        }

        public List<SparePartsRequisitionDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<SparePartsRequisitionDetail> oSparePartsRequisitionDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsRequisitionDetailDA.Gets(tc, sSQL);
                oSparePartsRequisitionDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsRequisitionDetail", e);
                #endregion
            }

            return oSparePartsRequisitionDetail;
        }
        #endregion
    }
  
    
   
}
