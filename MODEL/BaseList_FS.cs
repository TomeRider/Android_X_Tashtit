using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;

//using HELPER;

namespace MODEL
{
    /*
    /// <summary>
    /// הגדרת פעולות העריכה
    /// </summary>
    //[DataContract]
    [Flags]
    public enum EditMode
    {
        //[EnumMember]
        Add,
        //[EnumMember]
        Modify,
        //[EnumMember]
        Delete,
        //[EnumMember]
        NONE
    };
    */

    /// <summary>
    /// הגדרת רשימת איברים עפ"י טיפוס מסויים
    /// </summary>
    /// <typeparam name="T">טיפוס הנתונים</typeparam>
    //[CollectionDataContract]
    public abstract class BaseList_FS<T> : List<T> where T : new()
    {
        #region DATA MEMBERS

        protected bool    sortList;             // האם למיין את הרשימה
        protected string  keyField = "IdFs";     //         שם שדה המפתח

        #endregion DATA MEMBERS

        #region CONSTRUCTORS

        public BaseList_FS()
        {
            sortList = true;
        }

        public BaseList_FS(string kf)
            : this()
        {
            keyField = kf;
        }

        #endregion CONSTRUCTORS

        #region PROPERTIES

        /// <summary>
        /// <== רשימת האיברים שלא נמחקו
        /// EntityStatus = Deleted-כל האיברים פרט לאלה ש
        /// </summary>
        public List<T> UndeletedList
        {
            get { return (this == null) ? null : this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) != EntityStatus.DELETED).ToList(); }
        }

        public List<T> InsertList { get; set; }
        public List<T> UpdateList { get; set; }
        public List<T> DeleteList { get; set; }

        #endregion PROPERTIES

        #region METHODS

        /// <summary>
        /// הוספת איבר לרשימה
        /// </summary>
        /// <param name="entity">האיבר להוספה</param>
        /// <returns>האם נוסף בהצלחה</returns>
        public new bool Add(T entity)
        {
            // אם האיבר לא קיים ברשימה ניתן להוסיפו ומיון הרשימה מחדש
            if (!Exist(entity))
            {
                entity.GetType().GetProperty("EntityStatus").SetValue(entity, EntityStatus.ADDED, null);
                base.Add(entity);

                SetObjects(entity);

                if (sortList) Sort();
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// שינוי איבר ברשימה
        /// מחליף איבר חדש באיבר הקיים ברשימה
        /// </summary>
        /// <param name="oldEntity">האיבר הקיים ברשימה</param>
        /// <param name="newEntity">האיבר החדש</param>
        /// <returns>האם שומה בהצלחה</returns>
        public bool Modify(T oldEntity, T newEntity)
        {
            bool entityExists;
            string  idFs;
            T    existEntity;
            EntityStatus status;

            idFs = (string)oldEntity.GetType().GetProperty("IdFs").GetValue(oldEntity, null);
            newEntity.GetType().GetProperty("IdFs").SetValue(newEntity, idFs, null);

            if (!oldEntity.Equals(newEntity))
            {
                status = (EntityStatus)oldEntity.GetType().GetProperty("EntityStatus").GetValue(oldEntity, null);

                entityExists = Exist(newEntity, out existEntity);

                if (!entityExists ||
                    entityExists && idFs == (string)existEntity.GetType().GetProperty("IdFs").GetValue(existEntity, null))
                {
                    oldEntity = Find(oldEntity);  // This row needed for WEB or WEB_SERVICE Projects
                    return Change(oldEntity, newEntity, status);
                }
                else
                    return false;
            }
            else
                return false;
        }

        private bool Change(T oldEntity, T newEntity, EntityStatus status)
        {
            //T entity = Find(oldEntity);         // איתור האיבר הקיים ברשימה
            //SetNewData(entity, newEntity);      //      קביעת הנתונים החדשים

            SetNewData(oldEntity, newEntity);

            /*
            if (oldEntity.GetType().GetProperty(keyField).GetValue(oldEntity, null) == null ||
                oldEntity.GetType().GetProperty(keyField).GetValue(oldEntity, null).ToString().Equals(""))
            {

            }
            */

            //     שינוי סטטוס הרשומה רק אם היא קיימת במסד הנתונים
            // אחרת היא רשומה חדשה ששונתה שיש להוסיפה למסד הנתונים
            /*
            if (oldEntity.GetType().GetProperty(keyField).GetValue(oldEntity, null).ToString().Equals("") ||
                 oldEntity.GetType().GetProperty(keyField).GetValue(oldEntity, null) == null)
            */
            if (oldEntity.GetType().GetProperty(keyField).GetValue(oldEntity, null) == null ||
                oldEntity.GetType().GetProperty(keyField).GetValue(oldEntity, null).ToString().Equals(""))
            {
                if (status == EntityStatus.ADDED)
                    oldEntity.GetType().GetProperty("EntityStatus").SetValue(oldEntity, EntityStatus.ADDED, null);
            }
            else
            {
                oldEntity.GetType().GetProperty("EntityStatus").SetValue(oldEntity, EntityStatus.MODIFIED, null);
            }

            SetObjects(oldEntity);

            if (sortList) Sort();            // מיון הרשימה מחדש
            return true;
        }

        /// <summary>
        /// מחיקת איבר מהרשימה
        /// </summary>
        /// <param name="entity">האיבר למחיקה</param>
        public void Delete(T entity)
        {
            //                                         אם שדה המפתח מכיל ערך <> 0
            // הרשומה קיימת במסד הנתונים ויש לסמן אותה ברשימה כמועמדת למחיקה
            //                                    אחרת יש להסיר את האיבר מהרשימה
            if (entity.GetType().GetProperty(keyField).GetValue(entity, null) != null &&
                !entity.GetType().GetProperty(keyField).GetValue(entity, null).ToString().Equals(""))
            {
                T e = Find(keyField, entity.GetType().GetProperty(keyField).GetValue(entity, null).ToString());
                e.GetType().GetProperty("EntityStatus").SetValue(e, EntityStatus.DELETED, null);
            }
            else
                Remove(entity);
        }

        /// <summary>
        /// חפש ישות עפ"י מאפיין
        /// </summary>
        /// <param name="property">המאפיין לחיפוש</param>
        /// <param name="value">הערך לחיפוש</param>
        /// <returns>הישות שנמצאה</returns>
        public T Find(string property, int value)
        {
            T t = new T();

            if (Count > 0)
                t = base.Find(item => Convert.ToInt32(t.GetType().GetProperty(property).GetValue(item, null)) == value);

            return t;
        }

        public T Find(string property, string value)
        {
            T t = new T();

            if (Count > 0)
                t = base.Find(item => t.GetType().GetProperty(property).GetValue(item, null).ToString().Trim() == value.Trim());

            return t;
        }

        public T Find(string property, EntityStatus value)
        {
            T t = new T();

            if (Count > 0)
                t = base.Find(item => (EntityStatus)t.GetType().GetProperty(property).GetValue(item, null) == value);

            return t;
        }

        public T Find(T entity)
        {
            T t = new T();

            if (Count > 0)
                t = base.Find(item => item.Equals(entity));
            else
                t = default(T);

            return t;
        }

        /// <summary>
        /// קביעת נתונים לאיבר החדש
        /// </summary>
        /// <param name="oldEntity">האיבר הישן</param>
        /// <param name="newEntity">האיבר החדש</param>
        protected void SetNewData(T oldEntity, T newEntity)
        {
            if (oldEntity.GetType() == newEntity.GetType())
            {
                /*
                Type type = oldEntity.GetType();
                PropertyInfo[] propertyInfo = type.GetProperties();

                foreach (PropertyInfo pi in propertyInfo)
                    if (pi.Name.ToUpper() != keyField.ToUpper() && pi.CanWrite)
                        pi.SetValue(oldEntity, newEntity.GetType().GetProperty(pi.Name).GetValue(newEntity, null), null);
                */

                Type type = oldEntity.GetType();
                //FieldInfo[] fieldInfo = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                // שליפת כל השדות כולל שדות של מחלקת הבסיס
                FieldInfo[] fieldInfo = GetFieldInfosIncludingBaseClasses(type, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

                foreach (FieldInfo pi in fieldInfo)
                    try
                    {
                        // טיפול בשדות של המחלקה הנוכחית
                        if (pi.Name.ToUpper() != keyField.ToUpper())
                            pi.SetValue(oldEntity, newEntity.GetType().GetField(pi.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(newEntity));
                    }
                    catch (Exception e)
                    {
                        // BaseType - טיפול בשדות של מחלקת האב
                        if (pi.Name.ToUpper() != keyField.ToUpper())
                            pi.SetValue(oldEntity, newEntity.GetType().BaseType.GetField(pi.Name, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy).GetValue(newEntity));
                    }
            }
        }

        ////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////
        //////
        public static FieldInfo[] GetFieldInfosIncludingBaseClasses(Type type, BindingFlags bindingFlags)
        {
            FieldInfo[] fieldInfos = type.GetFields(bindingFlags);

            // If this class doesn't have a base, don't waste any time
            if (type.BaseType == typeof(BaseEntity /*object*/))
            {
                return fieldInfos;
            }
            else
            {   // Otherwise, collect all types up to the furthest base class
                var currentType = type;
                var fieldComparer = new FieldInfoComparer();
                var fieldInfoList = new HashSet<FieldInfo>(fieldInfos, fieldComparer);
                while (currentType != typeof(BaseEntity /*object*/))
                {
                    fieldInfos = currentType.GetFields(bindingFlags);
                    fieldInfoList.UnionWith(fieldInfos);
                    currentType = currentType.BaseType;
                }
                return fieldInfoList.ToArray();
            }
        }
        private class FieldInfoComparer : IEqualityComparer<FieldInfo>
        {
            public bool Equals(FieldInfo x, FieldInfo y)
            {
                return x.DeclaringType == y.DeclaringType && x.Name == y.Name;
            }

            public int GetHashCode(FieldInfo obj)
            {
                return obj.Name.GetHashCode() ^ obj.DeclaringType.GetHashCode();
            }
        }
        //////
        ////////////////////////////////////////////////////////////////////
        ////////////////////////////////////////////////////////////////////

        /// <summary>
        /// השמת רשימה חיצונית
        /// </summary>
        /// <param name="list">הרשימה החיצונית</param>

        #endregion METHODS

        #region ABSTRACT / VIRTUAL METHODS

        /// <summary>
        /// שמירת הנתונים
        ///                                   מטודה זו נקראת לאחר ביצוע המטודה במחלקה היורשת
        ///      (הפעולות: 1. מחיקה מהרשימה של כל האיברים שנמחקו (לאחר המחיקה ממסד הנתונים
        /// (לאחר הוספה/עדכון במסד הנתונים) Unchanged-סימון כל האיברים ברשימה כ 
        /// </summary>
        public virtual bool Save()
        {   
            RemoveAll(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.DELETED);
            ForEach  (item => item.GetType().GetProperty("EntityStatus").SetValue(item, EntityStatus.UNCHANGED, null));

            return isUpdateOK;
        }

        protected bool isUpdateOK;
        
        /// <summary>
        /// Genereta InsertList, UpdateList, DeleteList
        /// </summary>
        protected void GenereteUpdateLists()
        {
            InsertList = this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.ADDED).ToList();
            UpdateList = this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.MODIFIED).ToList();
            DeleteList = this.Where(item => (EntityStatus)item.GetType().GetProperty("EntityStatus").GetValue(item, null) == EntityStatus.DELETED).ToList();
        }

        /// <summary>
        /// בדיקה אם איבר נמצא ברשימה
        /// </summary>
        /// <param name="entity">האיבר לחיפוש</param>
        /// <returns>true - object Exist / false - object Dosn't Exist</returns>
        public virtual bool Exist (T entity)
        {
            T t;
            return Exist(entity, out t);
        }

        /// <summary>
        /// בדיקה אם איבר נמצא ברשימה
        /// </summary>
        /// <param name="entity">האיבר לחיפוש</param>
        /// <param name="existEntity">האיבר שנמצא</param>
        /// <returns>true - object Exist / false - object dosn't Exist</returns>
        public abstract bool Exist(T entity, out T existEntity);

        /// <summary>
        /// מיון הרשימה
        /// </summary>
        public new abstract void Sort();

        /// <summary>
        ///                  פעולות נוספות הדרושות לרשימה ספציפית
        ///                           הפעולה מתבצעת ברשימה ספציפית
        /// אם לא נדרשת פעולה אין לרשום את המטודה ברשימה ספציפית
        /// </summary>
        /// <param name="entity">האיבר עליו מתבצעת הפעולה</param>
        public virtual void SetObjects(T entity) { }

        #endregion ABSTRACT / VIRTUAL METHODS
    }
}
