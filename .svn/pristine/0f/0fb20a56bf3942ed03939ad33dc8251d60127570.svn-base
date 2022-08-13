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
    public class ChequeSetupService : MarshalByRefObject, IChequeSetupService
    {
        #region Private functions and declaration
        private ChequeSetup MapObject(NullHandler oReader)
        {
            ChequeSetup oChequeSetup = new ChequeSetup();
            oChequeSetup.ChequeSetupID = oReader.GetInt32("ChequeSetupID");
            oChequeSetup.ChequeSetupName = oReader.GetString("ChequeSetupName");
            oChequeSetup.Length = oReader.GetDouble("Length");
            oChequeSetup.Width = oReader.GetDouble("Width");
            oChequeSetup.PaymentMethodX = oReader.GetDouble("PaymentMethodX");
            oChequeSetup.paymentMethodY = oReader.GetDouble("paymentMethodY");
            oChequeSetup.paymentMethodL = oReader.GetDouble("paymentMethodL");
            oChequeSetup.paymentMethodW = oReader.GetDouble("paymentMethodW");
            oChequeSetup.paymentMethodFS = oReader.GetDouble("paymentMethodFS");
            oChequeSetup.DateX = oReader.GetDouble("DateX");
            oChequeSetup.DateY = oReader.GetDouble("DateY");
            oChequeSetup.DateL = oReader.GetDouble("DateL");
            oChequeSetup.DateW = oReader.GetDouble("DateW");
            oChequeSetup.DateFS = oReader.GetDouble("DateFS");
            oChequeSetup.PayToX = oReader.GetDouble("PayToX");
            oChequeSetup.PayToY = oReader.GetDouble("PayToY");
            oChequeSetup.PayToL = oReader.GetDouble("PayToL");
            oChequeSetup.PayToW = oReader.GetDouble("PayToW");
            oChequeSetup.PayToFS = oReader.GetDouble("PayToFS");
            oChequeSetup.AmountWordX = oReader.GetDouble("AmountWordX");
            oChequeSetup.AmountWordY = oReader.GetDouble("AmountWordY");
            oChequeSetup.AmountWordL = oReader.GetDouble("AmountWordL");
            oChequeSetup.AmountWordW = oReader.GetDouble("AmountWordW");
            oChequeSetup.AmountWordFS = oReader.GetDouble("AmountWordFS");
            oChequeSetup.AmountX = oReader.GetDouble("AmountX");
            oChequeSetup.AmountY = oReader.GetDouble("AmountY");
            oChequeSetup.AmountL = oReader.GetDouble("AmountL");
            oChequeSetup.AmountW = oReader.GetDouble("AmountW");
            oChequeSetup.AmountFS = oReader.GetDouble("AmountFS");
            oChequeSetup.TBookNoX = oReader.GetDouble("TBookNoX");
            oChequeSetup.TBookNoY = oReader.GetDouble("TBookNoY");
            oChequeSetup.TBookNoL = oReader.GetDouble("TBookNoL");
            oChequeSetup.TBookNoW = oReader.GetDouble("TBookNoW");
            oChequeSetup.TBookNoFS = oReader.GetDouble("TBookNoFS");
            oChequeSetup.TPaymentTypeX = oReader.GetDouble("TPaymentTypeX");
            oChequeSetup.TPaymentTypeY = oReader.GetDouble("TPaymentTypeY");
            oChequeSetup.TPaymentTypeL = oReader.GetDouble("TPaymentTypeL");
            oChequeSetup.TPaymentTypeW = oReader.GetDouble("TPaymentTypeW");
            oChequeSetup.TPaymentTypeFS = oReader.GetDouble("TPaymentTypeFS");
            oChequeSetup.TDateX = oReader.GetDouble("TDateX");
            oChequeSetup.TDateY = oReader.GetDouble("TDateY");
            oChequeSetup.TDateL = oReader.GetDouble("TDateL");
            oChequeSetup.TDateW = oReader.GetDouble("TDateW");
            oChequeSetup.TDateFS = oReader.GetDouble("TDateFS");
            oChequeSetup.TPayToX = oReader.GetDouble("TPayToX");
            oChequeSetup.TPayToY = oReader.GetDouble("TPayToY");
            oChequeSetup.TPayToL = oReader.GetDouble("TPayToL");
            oChequeSetup.TPayToW = oReader.GetDouble("TPayToW");
            oChequeSetup.TPayToFS = oReader.GetDouble("TPayToFS");
            oChequeSetup.TForNoteX = oReader.GetDouble("TForNoteX");
            oChequeSetup.TForNoteY = oReader.GetDouble("TForNoteY");
            oChequeSetup.TForNoteL = oReader.GetDouble("TForNoteL");
            oChequeSetup.TForNoteW = oReader.GetDouble("TForNoteW");
            oChequeSetup.TForNoteFS = oReader.GetDouble("TForNoteFS");
            oChequeSetup.TAmountX = oReader.GetDouble("TAmountX");
            oChequeSetup.TAmountY = oReader.GetDouble("TAmountY");
            oChequeSetup.TAmountL = oReader.GetDouble("TAmountL");
            oChequeSetup.TAmountW = oReader.GetDouble("TAmountW");
            oChequeSetup.TAmountFS = oReader.GetDouble("TAmountFS");
            oChequeSetup.TVoucherNoX = oReader.GetDouble("TVoucherNoX");
            oChequeSetup.TVoucherNoY = oReader.GetDouble("TVoucherNoY");
            oChequeSetup.TVoucherNoL = oReader.GetDouble("TVoucherNoL");
            oChequeSetup.TVoucherNoW = oReader.GetDouble("TVoucherNoW");
            oChequeSetup.TVoucherNoFS = oReader.GetDouble("TVoucherNoFS");
            oChequeSetup.DateFormat = oReader.GetString("DateFormat");
            oChequeSetup.IsSplit = oReader.GetBoolean("IsSplit");
            oChequeSetup.DateSpace = oReader.GetDouble("DateSpace");
            oChequeSetup.Ischecked = oReader.GetBoolean("Ischecked");
            oChequeSetup.PrinterGraceWidth = oReader.GetDouble("PrinterGraceWidth");
            oChequeSetup.ChequeImageInByte = oReader.GetBytes("ChequeImageInByte");
            oChequeSetup.ChequeImageSize = oReader.GetInt32("ChequeImageSize");
            return oChequeSetup;
        }

        private ChequeSetup CreateObject(NullHandler oReader)
        {
            ChequeSetup oChequeSetup = new ChequeSetup();
            oChequeSetup = MapObject(oReader);
            return oChequeSetup;
        }

        private List<ChequeSetup> CreateObjects(IDataReader oReader)
        {
            List<ChequeSetup> oChequeSetups = new List<ChequeSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChequeSetup oItem = CreateObject(oHandler);
                oChequeSetups.Add(oItem);
            }
            return oChequeSetups;
        }

        #endregion

        #region Interface implementation
        public ChequeSetupService() { }

        public ChequeSetup Save(ChequeSetup oChequeSetup, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {                
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                ChequeSetup oTempChequeSetup = new ChequeSetup();
                oTempChequeSetup.ChequeImageInByte = oChequeSetup.ChequeImageInByte;
                oChequeSetup.ChequeImageInByte = null;

                if (oChequeSetup.ChequeSetupID <= 0)
                {
                    reader = ChequeSetupDA.InsertUpdate(tc, oChequeSetup, EnumDBOperation.Insert, nCurrentUserID);
                }
                else
                {
                    reader = ChequeSetupDA.InsertUpdate(tc, oChequeSetup, EnumDBOperation.Update, nCurrentUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeSetup = new ChequeSetup();
                    oChequeSetup = CreateObject(oReader);
                }
                reader.Close();

                oTempChequeSetup.ChequeSetupID = oChequeSetup.ChequeSetupID;
                if (oTempChequeSetup.ChequeImageInByte != null)
                {
                    ChequeSetupDA.UpdatePhoto(tc, oTempChequeSetup);
                    oChequeSetup.ChequeImageInByte = oTempChequeSetup.ChequeImageInByte;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChequeSetup.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ChequeSetup. Because of " + e.Message, e);
                #endregion
            }
            
            return oChequeSetup;
        }
        public string Delete(int nChequeSetupID, int nCurrentUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChequeSetup oChequeSetup = new ChequeSetup();
                oChequeSetup.ChequeSetupID = nChequeSetupID;
                AuthorizationRoleDA.CheckUserPermission(tc, nCurrentUserID, EnumModuleName.ChequeSetup, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "ChequeSetup", nChequeSetupID);
                ChequeSetupDA.Delete(tc, oChequeSetup, EnumDBOperation.Delete, nCurrentUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data Delete Successfully.";
        }

        public ChequeSetup Get(int id, int nCurrentUserID)
        {
            ChequeSetup oChequeSetup = new ChequeSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ChequeSetup", e);
                #endregion
            }

            return oChequeSetup;
        }
        public List<ChequeSetup> Gets(int nCurrentUserID)
        {
            List<ChequeSetup> oChequeSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeSetupDA.Gets(tc);
                oChequeSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeSetup", e);
                #endregion
            }

            return oChequeSetup;
        }
        public List<ChequeSetup> GetsWithoutImage(int nCurrentUserID)
        {
            List<ChequeSetup> oChequeSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeSetupDA.GetsWithoutImage(tc);
                oChequeSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeSetup", e);
                #endregion
            }

            return oChequeSetup;
        }

        

        

        public List<ChequeSetup> GetsByName(string sName, int nCurrentUserID)
        {
            List<ChequeSetup> oChequeSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChequeSetupDA.GetsByName(tc, sName);
                oChequeSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeSetup", e);
                #endregion
            }

            return oChequeSetups;
        }

        public List<ChequeSetup> Gets(string sSQL, int nCurrentUserID)
        {
            List<ChequeSetup> oChequeSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeSetupDA.Gets(tc, sSQL);
                oChequeSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChequeSetup", e);
                #endregion
            }

            return oChequeSetups;
        }        
        #endregion
    }
}
