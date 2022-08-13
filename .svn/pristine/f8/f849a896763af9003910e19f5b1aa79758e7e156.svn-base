using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class EmployeeProductionReceiveDetailService : MarshalByRefObject, IEmployeeProductionReceiveDetailService
    {
        #region Private functions and declaration
        private EmployeeProductionReceiveDetail MapObject(NullHandler oReader)
        {
            EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetail = new EmployeeProductionReceiveDetail();
            oEmployeeProductionReceiveDetail.EPSRDID = oReader.GetInt32("EPSRDID");
            oEmployeeProductionReceiveDetail.EPSID = oReader.GetInt32("EPSID");
            oEmployeeProductionReceiveDetail.RcvQty = oReader.GetDouble("RcvQty");
            oEmployeeProductionReceiveDetail.Rate = oReader.GetDouble("Rate");
            oEmployeeProductionReceiveDetail.CurrencyID = oReader.GetInt32("CurrencyID");
            oEmployeeProductionReceiveDetail.RcvBy = oReader.GetInt32("RcvBy");
            oEmployeeProductionReceiveDetail.RcvByDate = oReader.GetDateTime("RcvByDate");

            //derive
            oEmployeeProductionReceiveDetail.StyleNo = oReader.GetString("StyleNo");
            oEmployeeProductionReceiveDetail.ColorName = oReader.GetString("ColorName");
            oEmployeeProductionReceiveDetail.SizeCategoryName = oReader.GetString("SizeCategoryName");
            oEmployeeProductionReceiveDetail.GarmentPart = oReader.GetInt32("GarmentPart");
            oEmployeeProductionReceiveDetail.ProductionRate = oReader.GetDouble("ProductionRate");
            oEmployeeProductionReceiveDetail.ProductionNote = oReader.GetString("ProductionNote");
            oEmployeeProductionReceiveDetail.GPName = oReader.GetString("GPName");
            return oEmployeeProductionReceiveDetail;

        }

        private EmployeeProductionReceiveDetail CreateObject(NullHandler oReader)
        {
            EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetail = MapObject(oReader);
            return oEmployeeProductionReceiveDetail;
        }

        private List<EmployeeProductionReceiveDetail> CreateObjects(IDataReader oReader)
        {
            List<EmployeeProductionReceiveDetail> oEmployeeProductionReceiveDetail = new List<EmployeeProductionReceiveDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeProductionReceiveDetail oItem = CreateObject(oHandler);
                oEmployeeProductionReceiveDetail.Add(oItem);
            }
            return oEmployeeProductionReceiveDetail;
        }

        #endregion

        #region Interface implementation
        public EmployeeProductionReceiveDetailService() { }

        public EmployeeProductionReceiveDetail IUD(EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetail, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeProductionReceiveDetailDA.IUD(tc, oEmployeeProductionReceiveDetail, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeProductionReceiveDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeProductionReceiveDetail.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeProductionReceiveDetail.EPSID = 0;
                #endregion
            }
            return oEmployeeProductionReceiveDetail;
        }


        public EmployeeProductionReceiveDetail Get(int nEPSID, Int64 nUserId)
        {
            EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetail = new EmployeeProductionReceiveDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeProductionReceiveDetailDA.Get(nEPSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProductionReceiveDetail = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeProductionReceiveDetail", e);
                oEmployeeProductionReceiveDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeProductionReceiveDetail;
        }
        public List<EmployeeProductionReceiveDetail> Gets(Int64 nUserID)
        {
            List<EmployeeProductionReceiveDetail> oEmployeeProductionReceiveDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeProductionReceiveDetailDA.Gets(tc);
                oEmployeeProductionReceiveDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeProductionReceiveDetail", e);
                #endregion
            }
            return oEmployeeProductionReceiveDetail;
        }

        public List<EmployeeProductionReceiveDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeProductionReceiveDetail> oEmployeeProductionReceiveDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeProductionReceiveDetailDA.Gets(sSQL, tc);
                oEmployeeProductionReceiveDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeProductionReceiveDetail", e);
                #endregion
            }
            return oEmployeeProductionReceiveDetail;
        }


        #endregion

    }
}
