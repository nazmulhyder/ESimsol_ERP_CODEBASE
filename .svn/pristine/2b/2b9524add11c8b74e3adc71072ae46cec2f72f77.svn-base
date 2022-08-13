using System;
using System.Collections;
using System.Data;
using System.Reflection;

namespace ICS.Core.Framework
{

    #region ObjectArrayAsObject
    public class ObjectArryay
    {
        private object[] _objects;

        public ObjectArryay(params object[] objects)
        {
            _objects = objects;
        }

        public int Length { get { return _objects.Length; } }

        public object this[int nIndex]
        {
            get
            {
                if (this.Length <= nIndex) return null;
                return _objects[nIndex];
            }
        }

        public override bool Equals(object obj)
        {
            ObjectArryay cacheParam2;

            cacheParam2 = obj as ObjectArryay;

            if ((object)cacheParam2 == null)
                return false;

            if (_objects.Length != cacheParam2._objects.Length)
                return false;


            for (int i = 0; i < _objects.Length; i++)
            {
                if (_objects[i] == null)
                {
                    if (cacheParam2._objects[i] != null)
                        return false;
                }
                else
                {
                    if (!_objects[i].Equals(cacheParam2._objects[i]))
                        return false;
                }
            }
            return true;
        }

        public static bool operator ==(ObjectArryay cacheParameter1, ObjectArryay cacheParameter2)
        {
            if ((object)cacheParameter1 != null)
                return cacheParameter1.Equals(cacheParameter2);
            else
                return ((object)cacheParameter2 == null);
        }

        public static bool operator !=(ObjectArryay cacheParameter1, ObjectArryay cacheParameter2)
        {
            if ((object)cacheParameter1 != null)
                return !cacheParameter1.Equals(cacheParameter2);
            else
                return ((object)cacheParameter2 != null);
        }

        public override int GetHashCode()
        {
            if (_objects.Length == 0)
                return base.GetHashCode();
            else
            {
                int hash = 0;
                for (int i = 0; i < _objects.Length; i++)
                {
                    if (_objects[i] != null)
                    {
                        hash ^= _objects[i].GetHashCode();
                    }
                }
                return hash;
            }
        }
    }
    #endregion

    #region Framework: BusinessObjects
    [Serializable]
    public abstract class CollectionBaseSortable : CollectionBase
    {
        public DataSet ToDataSet(String sProperties)
        {
            return (new CollectionBaseToDataSet(this)).ToDataSet(sProperties);
        }
    }
    #endregion

    #region Framework: IndexedBusinessObjects
    [Serializable]
    public abstract class IndexedBusinessObjects : CollectionBaseSortable
    {
        protected Hashtable _hashtable = new Hashtable();
        private int _autoKey = 0;
        protected virtual ID GetKeyID(BusinessObject item)
        {
            return item.ID;
        }

        protected int AddItem(BusinessObject item)
        {
            if (item.IsNew && item.ID.ToInt32 == 0)
            {
                _autoKey--;
                BusinessObject.Factory.SetID(item, new ID(_autoKey));
            }

            ID keyID = GetKeyID(item);

            int index;
            if (_hashtable.Contains(keyID))
            {
                index = (int)_hashtable[keyID];
                InnerList[index] = item;
            }
            else
            {
                index = InnerList.Add(item);
                _hashtable.Add(keyID, index);
            }
            return index;
        }
        protected void AddItem(BusinessObject item, int index)
        {
            if (item.IsNew && item.ID.ToInt32 == 0)
            {
                _autoKey--;
                BusinessObject.Factory.SetID(item, new ID(_autoKey));
            }

            ID keyID = GetKeyID(item);

            int curIndex;
            if (_hashtable.Contains(keyID))
            {
                curIndex = (int)_hashtable[keyID];
                InnerList[curIndex] = item;
            }
            else
            {
                InnerList.Insert(index, item);
                _hashtable.Add(keyID, index);
                // Update Index
                for (int i = index; i < InnerList.Count; i++)
                {
                    BusinessObject mc = (BusinessObject)InnerList[i];
                    _hashtable[GetKeyID(mc)] = i;

                }
            }
        }
        protected void Swap(BusinessObject obj1, BusinessObject obj2)
        {
            int nIndex1 = this.GetIndex(obj1.ID);
            int nIndex2 = this.GetIndex(obj2.ID);
            this.RemoveItem(obj1);
            this.RemoveItem(obj2);

            if (this.Count < nIndex1)
            {
                this.AddItem(obj2);
                this.AddItem(obj1);
            }
            else
            {
                this.AddItem(obj2, nIndex1);
                this.AddItem(obj1, nIndex2);
            }
        }

        public bool Contains(BusinessObject businessObject)
        {
            return InnerList.Contains(businessObject);
        }

        public bool Contains(ID keyID)
        {
            return _hashtable.Contains(keyID);
        }
        protected void RemoveItem(BusinessObject item)
        {
            ID key = GetKeyID(item);
            if (_hashtable.Contains(key))
            {
                int index = (int)_hashtable[key];
                _hashtable.Remove(key);

                InnerList.Remove(item);
                // Update Index

                for (int i = index; i < InnerList.Count; i++)
                {
                    BusinessObject mc = (BusinessObject)InnerList[i];
                    _hashtable[GetKeyID(mc)] = i;

                }
            }
        }

        protected override void OnClear()
        {
            base.OnClear();
            _hashtable.Clear();
        }

        protected BusinessObject GetItem(int index)
        {
            return (BusinessObject)InnerList[index];
        }
        protected int GetIndex(ID id)
        {
            int index = -1;
            if (_hashtable.Contains(id))
            {
                index = (int)_hashtable[id];
            }
            return index;
        }


        protected BusinessObject GetItem(ID id)
        {
            if (_hashtable.Contains(id))
            {
                int index = (int)_hashtable[id];
                return (BusinessObject)InnerList[index];
            }
            else
                return null;
        }

        protected BusinessObject GetItem(BusinessObject keyObject)
        {
            if (keyObject != null)
            {
                return GetItem(keyObject.ID);
            }
            else
                return null;
        }

        protected string IDInString()
        {
            string sReturn = "";
            foreach (BusinessObject oItem in this)
            {
                sReturn = sReturn + oItem.ObjectID.ToString() + ",";
            }
            if (sReturn.Length > 0) sReturn = sReturn.Remove(sReturn.Length - 1, 1);
            return sReturn;
        }
    }
    #endregion

    #region CollectionBaseToDataSet
    /// <summary>
    /// this class convert a collectionbase into DataSet
    /// </summary>
    public class CollectionBaseToDataSet
    {
        #region Private Declaration
        private CollectionBase _CollectionBaseData;
        #endregion

        #region Constructor
        public CollectionBaseToDataSet(CollectionBase Data)
        {
            _CollectionBaseData = Data;
        }
        #endregion

        #region Private Property
        private PropertyInfo[] _PropertyInfos = null;
        private PropertyInfo[] PropertyInfos
        {
            get
            {
                if (_PropertyInfos == null)
                {
                    _PropertyInfos = this.GetPropertyInfo();
                }
                return _PropertyInfos;
            }
        }

        private string _sProperty = "";
        #endregion

        #region Private Function
        private bool IsRequiredProperty(string sPName)
        {
            foreach (string item in _sProperty.Split(','))
            {
                if (item.Equals(sPName)) return true;
            }
            return false;
        }
        private PropertyInfo[] GetPropertyInfo()
        {
            if (_CollectionBaseData.Count > 0)
            {
                IEnumerator oEnumerator = _CollectionBaseData.GetEnumerator();
                oEnumerator.MoveNext();
                PropertyInfo[] temp1 = oEnumerator.Current.GetType().GetProperties();//BindingFlags.Default);
                PropertyInfo[] temp2 = new PropertyInfo[_sProperty.Split(',').Length];
                int i = 0;
                foreach (PropertyInfo item in temp1)
                {
                    if (!this.IsRequiredProperty(item.Name)) continue;
                    temp2[i++] = item;
                }
                return temp2;
            }
            return null;
        }
        private void DataChanged(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action != DataRowAction.Change) return;
            DataRow dr = e.Row;
            CollectionBase oCollectionBase = (CollectionBase)_CollectionBaseData;
            IEnumerator oEnumerator = oCollectionBase.GetEnumerator();

            while (oEnumerator.MoveNext())
            {
                if (this.IsSameObject(dr, oEnumerator.Current))
                {
                    this.FillBOObjectData(dr, oEnumerator.Current);
                }
            }
        }

        private DataTable CreateDataTable()
        {
            DataTable oDataTable = new DataTable("DataTable");
            oDataTable.RowChanged += new DataRowChangeEventHandler(this.DataChanged);

            foreach (PropertyInfo oProperty in this.PropertyInfos)
            {
                Type oType = oProperty.PropertyType; // oProperty.GetValue(DummyData,null).GetType();
                oDataTable.Columns.Add(oProperty.Name.ToString(), oType);
            }
            return oDataTable;
        }

        //		private DataTable CreateDataTable()
        //		{
        //			DataTable oDataTable = new DataTable("DataTable");
        //
        //			foreach (PropertyInfo oProperty in PropertyInfos)
        //			{
        //				oDataTable.Columns.Add(oProperty.Name.ToString());
        //			}
        //
        //			return oDataTable;
        //		}


        private DataSet CreateDataSet()
        {
            DataSet oDataSet = new DataSet("GridDataSet");
            oDataSet.Tables.Add(FillDataTable());
            return oDataSet;
        }


        //		private DataRow FillDataRow(DataRow oDataRow, object oData)
        //		{
        //			foreach (PropertyInfo oPropertyInfo in PropertyInfos)
        //			{
        //				oDataRow[oPropertyInfo.Name.ToString()] = oPropertyInfo.GetValue(oData,null);
        //			}
        //			return oDataRow;
        //		}


        private DataRow FillDataRow(DataRow oDataRow, object oData)
        {
            foreach (PropertyInfo oPropertyInfo in PropertyInfos)
            {
                Type oType = oPropertyInfo.GetValue(oData, null).GetType();
                oDataRow[oPropertyInfo.Name] = Convert.ChangeType(oPropertyInfo.GetValue(oData, null), oType);
            }
            return oDataRow;
        }

        private object FillBOObjectData(DataRow oDataRow, object oData)
        {
            foreach (PropertyInfo oPropertyInfo in PropertyInfos)
            {
                if (oPropertyInfo.Name == "ObjectID") continue;
                oPropertyInfo.SetValue(oData, oDataRow[oPropertyInfo.Name.ToString()], null);
            }
            return oData;
        }
        private bool IsSameObject(DataRow oDataRow, object oData)
        {
            foreach (PropertyInfo oPropertyInfo in PropertyInfos)
            {
                if (!oPropertyInfo.Name.Equals("ObjectID")) continue;
                Type oType = oPropertyInfo.GetValue(oData, null).GetType();
                return (oDataRow["ObjectID"].Equals(Convert.ChangeType(oPropertyInfo.GetValue(oData, null), oType)));
            }
            return false;
        }

        private DataTable FillDataTable()
        {
            CollectionBase oCollectionBase = (CollectionBase)_CollectionBaseData;

            IEnumerator oEnumerator = oCollectionBase.GetEnumerator();

            DataTable oDataTable = CreateDataTable();

            while (oEnumerator.MoveNext())
            {
                oDataTable.Rows.Add(FillDataRow(oDataTable.NewRow(), oEnumerator.Current));
            }
            return oDataTable;
        }

        #endregion

        #region Public Function

        public DataSet ToDataSet(String sProperties)
        {
            _sProperty = sProperties;
            return CreateDataSet();
        }

        #endregion
    }
    #endregion

    #region Framework: Child Collection
    [Serializable]
    public class ChildCollection : CollectionBase
    {
        protected BusinessObject _parent;
        protected ArrayList _removedItems = new ArrayList();

        protected object GetRemovedItems(Type type)
        {
            return _removedItems.ToArray(type);
        }

        public void SetParent(BusinessObject parent)
        {
            _parent = parent;

            // set the parent for contained items
            foreach (ChildBusinessObject item in InnerList)
            {
                item.SetParent(_parent);
            }
        }

        protected virtual int AddItem(ChildBusinessObject item)
        {
            item.SetParent(_parent);
            return InnerList.Add(item);
        }

        protected virtual void RemoveItem(ChildBusinessObject item)
        {
            if (item != null)
            {
                item.SetParent(null);
                if (!item.IsNew)
                    _removedItems.Add(item);
            }

            InnerList.Remove(item);
        }

        protected ChildBusinessObject GetItem(int index)
        {
            return (ChildBusinessObject)InnerList[index];
        }

        protected void SetItem(int index, ChildBusinessObject item)
        {
            if (InnerList[index] != null)
                ((ChildBusinessObject)InnerList[index]).SetParent(null);

            if (item != null)
                item.SetParent(_parent);

            InnerList[index] = item;
        }

        protected int GetIndex(ID id)
        {
            int index = -1;
            foreach (ChildBusinessObject oItem in InnerList)
            {
                index = index + 1;
                if (oItem.ID == id)
                {
                    return index;
                }
            }
            return -1;
        }

        public ChildCollection() { }
    }

    #endregion

    #region Framework: IndexedChildCollection
    [Serializable]
    public abstract class IndexedChildCollection : ChildCollection
    {
        protected Hashtable _hashtable = new Hashtable();
        private int _autoKey = 0;
        protected virtual ID GetKeyID(ChildBusinessObject item)
        {
            return item.ID;
        }

        protected override int AddItem(ChildBusinessObject item)
        {
            if (item.IsNew && item.ID.ToInt32 == 0)
            {
                _autoKey--;
                BusinessObject.Factory.SetID(item, new ID(_autoKey));
            }

            ID keyID = GetKeyID(item);
            int index;
            if (_hashtable.Contains(keyID))
            {
                index = (int)_hashtable[keyID];
                base.SetItem(index, item);
            }
            else
            {
                index = base.AddItem(item);
                _hashtable.Add(keyID, index);
            }

            return index;
        }
        protected void AddItem(ChildBusinessObject item, int index)
        {
            if (item.IsNew && item.ID.ToInt32 == 0)
            {
                _autoKey--;
                ChildBusinessObject.Factory.SetID(item, new ID(_autoKey));
            }

            ID keyID = GetKeyID(item);

            int curIndex;
            if (_hashtable.Contains(keyID))
            {
                curIndex = (int)_hashtable[keyID];
                InnerList[curIndex] = item;
            }
            else
            {
                InnerList.Insert(index, item);
                _hashtable.Add(keyID, index);
                // Update Index
                for (int i = index; i < InnerList.Count; i++)
                {
                    ChildBusinessObject mc = (ChildBusinessObject)InnerList[i];
                    _hashtable[GetKeyID(mc)] = i;

                }
            }
        }


        protected override void RemoveItem(ChildBusinessObject item)
        {
            ID key = GetKeyID(item);
            if (_hashtable.Contains(key))
            {
                int index = (int)_hashtable[key];
                _hashtable.Remove(key);

                base.RemoveItem(item);

                // Update Index

                for (int i = index; i < InnerList.Count; i++)
                {
                    ChildBusinessObject child = (ChildBusinessObject)InnerList[i];
                    _hashtable[GetKeyID(child)] = i;

                }
            }
        }
        protected void Swap(ChildBusinessObject obj1, ChildBusinessObject obj2)
        {
            int nIndex1 = this.GetIndex(obj1.ID);
            int nIndex2 = this.GetIndex(obj2.ID);
            this.RemoveItem(obj1);
            this.RemoveItem(obj2);

            if (this.Count < nIndex1)
            {
                this.AddItem(obj2);
                this.AddItem(obj1);
            }
            else
            {
                this.AddItem(obj2, nIndex1);
                this.AddItem(obj1, nIndex2);
            }
        }

        protected ChildBusinessObject GetItem(ID id)
        {
            if (_hashtable.Contains(id))
            {
                int index = (int)_hashtable[id];
                return (ChildBusinessObject)InnerList[index];
            }
            else
                return null;
        }

        protected ChildBusinessObject GetItem(BusinessObject keyObject)
        {
            if (keyObject != null)
            {
                return GetItem(keyObject.ID);
            }
            else
                return null;
        }

        protected override void OnClear()
        {
            base.OnClear();
            _hashtable.Clear();
        }
    }

    #endregion
}
