using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class NOADetailService : MarshalByRefObject, INOADetailService
    {
        #region Private functions and declaration
        public static NOADetail MapObject(NullHandler oReader)
        {
            NOADetail oNOADetail = new NOADetail();
            oNOADetail.NOADetailLogID = oReader.GetInt32("NOADetailLogID");
            oNOADetail.NOALogID = oReader.GetInt32("NOALogID");
            oNOADetail.NOADetailID = oReader.GetInt32("NOADetailID");
            oNOADetail.NOAID = oReader.GetInt32("NOAID");
            oNOADetail.LPP = oReader.GetInt32("LPP");
            oNOADetail.Date = oReader.GetDateTime("Date");
            oNOADetail.ProductID = oReader.GetInt32("ProductID");
            oNOADetail.PQDetailID = oReader.GetInt32("PQDetailID");
            oNOADetail.SupplierID = oReader.GetInt32("SupplierID");
            oNOADetail.ProductCode = oReader.GetString("ProductCode");
            oNOADetail.Rate = oReader.GetString("Rate");
            oNOADetail.MPRNO = oReader.GetString("MPRNO");
            oNOADetail.ProductName = oReader.GetString("ProductName");
            oNOADetail.ProductSpec = oReader.GetString("ProductSpec");
            oNOADetail.PurchaseQty = oReader.GetDouble("PurchaseQty");
            oNOADetail.ApprovedRate = oReader.GetDouble("ApprovedRate");
            oNOADetail.Note = oReader.GetString("Note");
            oNOADetail.PurchaseQty = oReader.GetDouble("PurchaseQty");
            oNOADetail.RequiredQty = oReader.GetDouble("RequiredQty");
            oNOADetail.MUnitID = oReader.GetInt32("MUnitID");
            oNOADetail.MUnitName = oReader.GetString("MUnitName");
            oNOADetail.UnitSymbol = oReader.GetString("UnitSymbol");
            return oNOADetail;
        }
        public static NOADetail CreateObject(NullHandler oReader)
        {
            NOADetail oNOADetail = new NOADetail();
            oNOADetail = MapObject(oReader);
            return oNOADetail;
        }
        public static List<NOADetail> CreateObjects(IDataReader oReader)
        {
            List<NOADetail> oNOADetail = new List<NOADetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                NOADetail oItem = CreateObject(oHandler);
                oNOADetail.Add(oItem);
            }
            return oNOADetail;
        }
        #endregion

        #region Interface implementation
        public NOADetail Save(NOADetail oNOADetail, int nUserID)
        {
            NOA oNOA = new NOA();
            oNOA = oNOADetail.NOA;
            TransactionContext tc = null;

            List<NOADetail> oEPIDetails = new List<NOADetail>();

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                #region NOA Part
                if (oNOA != null)
                {
                    reader = NOADA.InsertUpdate(tc, oNOA, EnumDBOperation.Insert, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oNOA = new NOA();
                        oNOA = NOAService.CreateObject(oReader);
                        oNOADetail.NOAID = oNOA.NOAID;
                    }
                    if (oNOA.NOAID <= 0)
                    {
                        oNOADetail = new NOADetail();
                        oNOADetail.ErrorMessage = "Invalid NOA";
                        return oNOADetail;
                    }
                    reader.Close();
                }
                #endregion

                if (oNOADetail.NOADetails.Count() > 0)
                {
                    NOADetail oEPIDteail = new NOADetail();
                    foreach (NOADetail oItem in oNOADetail.NOADetails)
                    {
                        if (oItem.NOADetailID <= 0)
                        {
                            if (oNOADetail.NOAID > 0) { oItem.NOAID = oNOADetail.NOAID; }
                            reader = NOADetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                            oReader = new NullHandler(reader);
                            if (reader.Read())
                            {
                                oEPIDteail = new NOADetail();
                                oEPIDteail = CreateObject(oReader);
                                oEPIDetails.Add(oEPIDteail);
                            }
                            reader.Close();
                        }
                    }
                }
                else
                {
                    IDataReader readerdetail;
                    if (oNOADetail.NOADetailID <= 0)
                    {
                        readerdetail = NOADetailDA.InsertUpdate(tc, oNOADetail, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = NOADetailDA.InsertUpdate(tc, oNOADetail, EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oNOADetail = new NOADetail();
                        oNOADetail = CreateObject(oReaderDetail);
                    }
                    readerdetail.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oNOADetail = new NOADetail();
                oNOADetail.ErrorMessage = e.Message;

                oNOA = new NOA();
                oEPIDetails = new List<NOADetail>();
                #endregion
            }

            oNOADetail.NOA = oNOA;
            oNOADetail.NOADetails = oEPIDetails;
            return oNOADetail;
        }

        public NOADetail Get(int id, int nUserId)
        {
            NOADetail oAccountHead = new NOADetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = NOADetailDA.Get(tc, id);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oAccountHead;
        }

        public List<SupplierRateProcess> Delete(int nNOAID, int nNOADetailID,int nUserId)
        {
            TransactionContext tc = null;
            List<SupplierRateProcess> oSupplierRateProcesss = new List<SupplierRateProcess>();

            try
            {
                tc = TransactionContext.Begin(true);
                NOADetail oNOADelete = new NOADetail();
                oNOADelete.NOAID = nNOAID;
                oNOADelete.NOADetailID = nNOADetailID;
                NOADetailDA.Delete(tc, oNOADelete, EnumDBOperation.Delete, nUserId,"");
                IDataReader reader = null;
                reader = SupplierRateProcessDA.Gets(nNOAID, tc);
                oSupplierRateProcesss = SupplierRateProcessService.CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                SupplierRateProcess oNewSupplierRateProcess = new SupplierRateProcess();
                oNewSupplierRateProcess.ErrorMessage = e.Message.Split('~')[0];
                oSupplierRateProcesss.Add(oNewSupplierRateProcess);
                return oSupplierRateProcesss;
                #endregion
            }
            return oSupplierRateProcesss;
        }

        public List<NOADetail> Gets(int nUserId)
        {
            List<NOADetail> oNOADetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADetailDA.Gets(tc);
                oNOADetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOADetail;
        }
        public List<NOADetail> Gets(string sSQL, int nUserId)
        {
            List<NOADetail> oNOADetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADetailDA.Gets(sSQL,tc);
                oNOADetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOADetail;
        }
        

        public List<NOADetail> Gets(long nNOAId, int nUserId)
        {
            List<NOADetail> oNOADetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADetailDA.Gets(tc, nNOAId);
                oNOADetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOADetail;
        }
        public List<NOADetail> GetsByLog(long nNOAId, int nUserId)
        {
            List<NOADetail> oNOADetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADetailDA.GetsByLog(tc, nNOAId);
                oNOADetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOADetail;
        }
        public List<NOADetail> GetsBy(int nNOAId, int nContractorID, int nUserId)
        {
            List<NOADetail> oNOADetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = NOADetailDA.GetsBy(tc, nNOAId, nContractorID);
                oNOADetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oNOADetail;
        }
        #endregion
    }   
    
}
