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
    public class HIAUserAssignService : MarshalByRefObject, IHIAUserAssignService
    {
        #region Private functions and declaration
        private HIAUserAssign MapObject(NullHandler oReader)
        {
            HIAUserAssign oHIAUserAssign = new HIAUserAssign();
            oHIAUserAssign.HIAUserAssignID = oReader.GetInt32("HIAUserAssignID");
            oHIAUserAssign.HIASetupID = oReader.GetInt32("HIASetupID");
            oHIAUserAssign.UserID = oReader.GetInt32("UserID");
            oHIAUserAssign.LocationName = oReader.GetString("LocationName");
            oHIAUserAssign.LogInID = oReader.GetString("LogInID");
            oHIAUserAssign.UserName = oReader.GetString("UserName");
            oHIAUserAssign.EmployeeNameCode = oReader.GetString("EmployeeNameCode");
            return oHIAUserAssign;
        }

        private HIAUserAssign CreateObject(NullHandler oReader)
        {
            HIAUserAssign oHIAUserAssign = new HIAUserAssign();
            oHIAUserAssign = MapObject(oReader);
            return oHIAUserAssign;
        }

        private List<HIAUserAssign> CreateObjects(IDataReader oReader)
        {
            List<HIAUserAssign> oHIAUserAssign = new List<HIAUserAssign>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HIAUserAssign oItem = CreateObject(oHandler);
                oHIAUserAssign.Add(oItem);
            }
            return oHIAUserAssign;
        }

        #endregion

        #region Interface implementation
        public HIAUserAssignService() { }

        public HIAUserAssign Save(HIAUserAssign oHIAUserAssign, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<HIAUserAssign> _oHIAUserAssigns = new List<HIAUserAssign>();
            oHIAUserAssign.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = HIAUserAssignDA.InsertUpdate(tc, oHIAUserAssign, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHIAUserAssign = new HIAUserAssign();
                    oHIAUserAssign = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oHIAUserAssign.ErrorMessage = e.Message;
                #endregion
            }
            return oHIAUserAssign;
        }

        public HIAUserAssign Get(int nInvoicePurchaseDetailID, Int64 nUserId)
        {
            HIAUserAssign oAccountHead = new HIAUserAssign();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HIAUserAssignDA.Get(tc, nInvoicePurchaseDetailID);
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
                throw new ServiceException("Failed to Get InvoicePurchaseDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<HIAUserAssign> Gets(int nInvoicePurchaseID, Int64 nUserID)
        {
            List<HIAUserAssign> oInvoicePurchaseDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HIAUserAssignDA.Gets(nInvoicePurchaseID, tc);
                oInvoicePurchaseDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InvoicePurchaseDetail", e);
                #endregion
            }

            return oInvoicePurchaseDetail;
        }

        public List<HIAUserAssign> Gets(string sSQL, Int64 nUserID)
        {
            List<HIAUserAssign> oHIAUserAssign = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HIAUserAssignDA.Gets(tc, sSQL);
                oHIAUserAssign = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get InvoicePurchaseDetail", e);
                #endregion
            }

            return oHIAUserAssign;
        }
        #endregion
    }
}
