using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaseClasses
{
    public static class FillModels
    {
        static readonly EqualsString equals = new EqualsString(); //比较大小
        public static List<T> CreateModels<T>(this SqlDataReader reader) where T : new()
        {
            List<T> lstT = new List<T>();
            if (reader == null || !reader.HasRows)
            {
                return lstT;
            }
            Type type = typeof(T);

            var cacheKey = type.FullName;
            var proertyInfos = GetPropertyInfos(cacheKey, type);

            var propertyNames = proertyInfos.Select(p => p.Name).ToList();

            while (reader.Read())
            {
                T t = new T();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    string colName = reader.GetName(i);
                    if (propertyNames.Contains(colName, equals))
                    {
                        string piName = propertyNames.FirstOrDefault(p => p.ToLower() == colName.ToLower());

                        PropertyInfo pi = type.GetProperty(piName);
                        if (pi != null && pi.CanWrite)
                        {
                            object value = reader[colName];
                            Type piType = pi.PropertyType;

                            value = Convert.ChangeType(value, piType);
                            if (value != DBNull.Value)
                                pi.SetValue(t, value, null);
                        }
                    }
                }
                lstT.Add(t);
            }
            return lstT;
        }

        static List<PropertyInfo> GetPropertyInfos(string cacheKey, Type type)
        {
            var proertyInfos = CacheHelper.Get<List<PropertyInfo>>(cacheKey);
            if (proertyInfos == null || proertyInfos.Count <= 0)
            {
                proertyInfos = type.GetProperties().ToList();
                CacheHelper.Add(cacheKey, proertyInfos, 1440);
            }
            return proertyInfos;
        }
        public static List<T> CreateModels<T>(this DataTable dt) where T : new()
        {
            List<T> lstT = new List<T>();
            if (dt == null || dt.DefaultView.Count <= 0)
            {
                return lstT;
            }
            Type type = typeof(T);


            var propertyInfos = GetPropertyInfos(type.FullName, type);
            foreach (DataRowView rv in dt.DefaultView)
            {
                T t = new T();
                foreach (PropertyInfo pi in propertyInfos)
                {
                    if (!pi.CanWrite)
                        continue;
                    if (dt.Columns.Contains(pi.Name))
                    {
                        object value = rv[pi.Name];
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                pi.SetValue(t, value, null);
                            }
                            catch
                            {
                                value = Convert.ChangeType(value, pi.PropertyType);
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                lstT.Add(t);
            }
            return lstT;
        }

        /// <summary>
        /// DataSet To List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ds"></param>
        /// <param name="tabName">需要转换List的表名称</param>
        /// <returns></returns>
        public static List<T> CreateModels<T>(this DataSet ds, string tabName = "") where T : new()
        {
            List<T> lstT = new List<T>();
            if (ds == null || ds.Tables.Count <= 0)
                return lstT;
            var dt = ds.Tables[0];
            if (!string.IsNullOrWhiteSpace(tabName))
            {
                dt = ds.Tables[tabName];
            }
            if (dt == null || dt.DefaultView.Count <= 0)
            {
                return lstT;
            }
            Type type = typeof(T);


            var propertyInfos = GetPropertyInfos(type.FullName, type);
            foreach (DataRowView rv in dt.DefaultView)
            {
                T t = new T();
                foreach (PropertyInfo pi in propertyInfos)
                {
                    if (!pi.CanWrite)
                        continue;
                    if (dt.Columns.Contains(pi.Name))
                    {
                        object value = rv[pi.Name];
                        if (value != DBNull.Value)
                        {
                            try
                            {
                                pi.SetValue(t, value, null);
                            }
                            catch
                            {
                                value = Convert.ChangeType(value, pi.PropertyType);
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                lstT.Add(t);
            }
            return lstT;
        }


        public static T CreateModel<T>(this DataTable dt) where T : new()
        {
            T t = new T();
            if (dt == null || dt.DefaultView.Count <= 0)
            {
                return t;
            }
            Type type = typeof(T);


            var propertyInfos = GetPropertyInfos(type.FullName, type);

            var row = dt.DefaultView[0];
            if (row == null)
            {
                return t;
            }
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (!pi.CanWrite)
                    continue;

                if (dt.Columns.Contains(pi.Name))
                {
                    object value = row[pi.Name];
                    Type piType = pi.PropertyType;

                    if (value == DBNull.Value) continue;
                    if (pi.PropertyType.BaseType == typeof(Enum))
                    {
                        var temp = Enum.ToObject(pi.PropertyType, value);
                        pi.SetValue(t, temp, null);
                        continue;
                    }
                    try
                    {
                        pi.SetValue(t, value, null);
                    }
                    catch
                    {
                        value = Convert.ChangeType(value, piType);
                        pi.SetValue(t, value, null);
                    }
                }
            }
            return t;
        }


        public static T CreateModel<T>(this DataSet ds) where T :class, new()
        {
            T t = new T();
            if (ds == null || ds.Tables.Count <= 0)
                return null;
            var dt = ds.Tables[0];
            if (dt == null || dt.DefaultView.Count <= 0)
            {
                return t;
            }
            return dt.CreateModel<T>();
        }
    }

    public class EqualsString : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return x.ToLower().Trim() == y.ToLower().Trim();
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}
