using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Base.DataAccess;
using ICS.Base.Client.BOFoundation;
using ICS.Base.Utility;
using ICS.Base.FrameWork;
using ICS.Base.Client.Utility;

namespace ESimSol.Services.Services
{
    public class PrintSetupService : MarshalByRefObject, IPrintSetupService
    {
        #region Private functions and declaration
        private PrintSetup MapObject(NullHandler oReader)
        {
            PrintSetup oPS = new PrintSetup();
            oPS.BankID = oReader.GetInt32("BankID");
            oPS.Length = oReader.GetDouble("Length");
            oPS.Width = oReader.GetDouble("Width");

            oPS.PaymentMethodX = oReader.GetDouble("PaymentMethodX");
            oPS.paymentMethodY = oReader.GetDouble("paymentMethodY");
            oPS.paymentMethodL = oReader.GetDouble("paymentMethodL");
            oPS.paymentMethodW = oReader.GetDouble("paymentMethodW");
            oPS.paymentMethodFS = oReader.GetDouble("paymentMethodFS");

            oPS.DateX = oReader.GetDouble("DateX");
            oPS.DateY = oReader.GetDouble("DateY");
            oPS.DateL = oReader.GetDouble("DateL");
            oPS.DateW = oReader.GetDouble("DateW");
            oPS.DateFS = oReader.GetDouble("DateFS");

            oPS.PayToX = oReader.GetDouble("PayToX");
            oPS.PayToY = oReader.GetDouble("PayToY");
            oPS.PayToL = oReader.GetDouble("PayToL");
            oPS.PayToW = oReader.GetDouble("PayToW");
            oPS.PayToFS = oReader.GetDouble("PayToFS");

            oPS.AmountWordX = oReader.GetDouble("AmountWordX");
            oPS.AmountWordY = oReader.GetDouble("AmountWordY");
            oPS.AmountWordL = oReader.GetDouble("AmountWordL");
            oPS.AmountWordW = oReader.GetDouble("AmountWordW");
            oPS.AmountWordFS = oReader.GetDouble("AmountWordFS");

            oPS.AmountX = oReader.GetDouble("AmountX");
            oPS.AmountY = oReader.GetDouble("AmountY");
            oPS.AmountL = oReader.GetDouble("AmountL");
            oPS.AmountW = oReader.GetDouble("AmountW");
            oPS.AmountFS = oReader.GetDouble("AmountFS");

            oPS.TBookNoX = oReader.GetDouble("TBookNoX");
            oPS.TBookNoY = oReader.GetDouble("TBookNoY");
            oPS.TBookNoL = oReader.GetDouble("TBookNoL");
            oPS.TBookNoW = oReader.GetDouble("TBookNoW");
            oPS.TBookNoFS = oReader.GetDouble("TBookNoFS");

            oPS.TPaymentTypeX = oReader.GetDouble("TPaymentTypeX");
            oPS.TPaymentTypeY = oReader.GetDouble("TPaymentTypeY");
            oPS.TPaymentTypeL = oReader.GetDouble("TPaymentTypeL");
            oPS.TPaymentTypeW = oReader.GetDouble("TPaymentTypeW");
            oPS.TPaymentTypeFS = oReader.GetDouble("TPaymentTypeFS");

            oPS.TDateX = oReader.GetDouble("TDateX");
            oPS.TDateY = oReader.GetDouble("TDateY");
            oPS.TDateL = oReader.GetDouble("TDateL");
            oPS.TDateW = oReader.GetDouble("TDateW");
            oPS.TDateFS = oReader.GetDouble("TDateFS");

            oPS.TPayToX = oReader.GetDouble("TPayToX");
            oPS.TPayToY = oReader.GetDouble("TPayToY");
            oPS.TPayToL = oReader.GetDouble("TPayToL");
            oPS.TPayToW = oReader.GetDouble("TPayToW");
            oPS.TPayToFS = oReader.GetDouble("TPayToFS");

            oPS.TForNoteX = oReader.GetDouble("TForNoteX");
            oPS.TForNoteY = oReader.GetDouble("TForNoteY");
            oPS.TForNoteL = oReader.GetDouble("TForNoteL");
            oPS.TForNoteW = oReader.GetDouble("TForNoteW");
            oPS.TForNoteFS = oReader.GetDouble("TForNoteFS");

            oPS.TAmountX = oReader.GetDouble("TAmountX");
            oPS.TAmountY = oReader.GetDouble("TAmountY");
            oPS.TAmountL = oReader.GetDouble("TAmountL");
            oPS.TAmountW = oReader.GetDouble("TAmountW");
            oPS.TAmountFS = oReader.GetDouble("TAmountFS");

            oPS.TVoucherNoX = oReader.GetDouble("TVoucherNoX");
            oPS.TVoucherNoY = oReader.GetDouble("TVoucherNoY");
            oPS.TVoucherNoL = oReader.GetDouble("TVoucherNoL");
            oPS.TVoucherNoW = oReader.GetDouble("TVoucherNoW");
            oPS.TVoucherNoFS = oReader.GetDouble("TVoucherNoFS");

            oPS.DateFormat = oReader.GetString("DateFormat");
            oPS.IsSplit = oReader.GetBoolean("IsSplit");
            oPS.PrinterGraceWidth = oReader.GetDouble("PrinterGraceWidth");
            return oPS;
        }

        private PrintSetup CreateObject(NullHandler oReader)
        {
            PrintSetup oPrintSetup = new PrintSetup();
            oPrintSetup = MapObject(oReader);
            return oPrintSetup;
        }

        private List<PrintSetup> CreateObjects(IDataReader oReader)
        {
            List<PrintSetup> oPrintSetup = new List<PrintSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PrintSetup oItem = CreateObject(oHandler);
                oPrintSetup.Add(oItem);
            }
            return oPrintSetup;
        }

        #endregion

        #region Interface implementation
        public PrintSetupService() { }

        public PrintSetup Save(PrintSetup oPrintSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                

                PrintSetupDA.InsertUpdate(tc, oPrintSetup, nUserId);


                NullHandler oReader = new NullHandler();
               
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oPrintSetup;
        }


        public PrintSetup SaveForHistory(PrintSetup oPrintSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = PrintSetupDA.Update(tc, oPrintSetup, nUserId);


                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPrintSetup = new PrintSetup();
                    oPrintSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save Product. Because of " + e.Message, e);
                #endregion
            }
            return oPrintSetup;
        }

        public PrintSetup Get(int id, Int64 nUserId)
        {
            PrintSetup oPrintSetup = new PrintSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PrintSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPrintSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oPrintSetup;
        }
        public List<PrintSetup> GetsByBook(int nBookID, Int64 nUserId)
        {
            List<PrintSetup> oPrintSetup = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PrintSetupDA.GetsByBook(tc, nBookID);
                oPrintSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oPrintSetup;
        }

        public List<PrintSetup> Gets(Int64 nUserId)
        {
            List<PrintSetup> oPrintSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PrintSetupDA.Gets(tc);
                oPrintSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Product", e);
                #endregion
            }

            return oPrintSetup;
        }
        #endregion
    }
}
