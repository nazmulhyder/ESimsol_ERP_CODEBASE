using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region Shelf
    public class Shelf : BusinessObject
    {
        public Shelf()
        {
            ShelfID = 0;
            ShelfNo = "";
            ShelfName = "";
            BUID = 0;
            Remarks = "";
            ErrorMessage = "";
            OrderInfos = new List<OrderInfo>();
        }
        #region Properties
        public int ShelfID { get; set; }
        public string ShelfNo { get; set; }
        public string ShelfName { get; set; }
        public int BUID { get; set; }
        public string Remarks { get; set; }
        public string ShelfNoOrName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Properties
        public List<OrderInfo> OrderInfos { get; set; }
        public List<Rack> Racks { get; set; }
        #endregion

        #region Functions

        public Shelf Get(int id, int nUserID)
        {
            return Shelf.Service.Get(id, nUserID);
        }
        public Shelf Save(int nUserID)
        {
            return Shelf.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return Shelf.Service.Delete(id, nUserID);
        }
        public static List<Shelf> Gets(int nUserID)
        {
            return Shelf.Service.Gets(nUserID);
        }
        public static List<Shelf> Gets(string sSQL, int nUserID)
        {
            return Shelf.Service.Gets(sSQL, nUserID);
        }
        public static List<Shelf> GetsByNoOrName(Shelf oShelf, int nUserID)
        {
            return Shelf.Service.GetsByNoOrName(oShelf, nUserID);
        }

        #endregion


        #region ServiceFactory
        internal static IShelfService Service
        {
            get { return (IShelfService)Services.Factory.CreateService(typeof(IShelfService)); }
        }
        #endregion
    }
    #endregion

    #region IShelf interface
    public interface IShelfService
    {
        Shelf Get(int id, int nUserID);
        List<Shelf> Gets(int nUserID);
        string Delete(int id, int nUserID);
        Shelf Save(Shelf oShelf, int nUserID);
        List<Shelf> Gets(string sSQL, int nUserID);
        List<Shelf> GetsByNoOrName(Shelf oShelf, int nUserID);

    }
    #endregion

    public class OrderInfo
    {
        public OrderInfo()
        {
            OrderID = 0;
            OrderNo = "";
            OrderDate = DateTime.Today;
            OrderQty = 0;
            Symbol = "";
        }
        public int OrderID { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderQty { get; set; }
        public string Symbol { get; set; }

        public string OrderDateSt
        {
            get
            {
                return this.OrderDate.ToString("dd MMM yyyy");
            }
        }

        public string OrderQtySt
        {
            get
            {
                return Global.MillionFormat(this.OrderQty,0);
            }
        }
    }
}
