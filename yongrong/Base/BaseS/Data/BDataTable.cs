﻿using BaseS.File.Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BaseS.Data
{
    public static class BDataTable
    {
        /// <summary>
        /// 转化一个DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IEnumerable<T> list, bool isProperty = true)
        {
#if TRACE
            string.Empty.Debug();
#endif
            //创建属性的集合
            List<PropertyInfo> pList = null;
            List<FieldInfo> fList = null;

            //获得反射的入口
            Type type = typeof(T);
            DataTable dt = new DataTable();

            //把所有的public属性加入到集合 并添加DataTable的列
            if (isProperty)
            {
                pList = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(type.GetProperties()
                , p =>
                {
                    pList.Add(p);

                    Type columnType = p.PropertyType;

                    if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                        columnType = p.PropertyType.GetGenericArguments()[0];
                    }

                    dt.Columns.Add(p.Name, columnType);
                });
            }
            else
            {
                fList = new List<FieldInfo>();

                Array.ForEach<FieldInfo>(type.GetFields()
                , f =>
                {
                    fList.Add(f);

                    Type columnType = f.FieldType;

                    if (f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        // If it is NULLABLE, then get the underlying type. eg if "Nullable<int>" then this will return just "int"
                        columnType = f.FieldType.GetGenericArguments()[0];
                    }

                    dt.Columns.Add(f.Name, columnType);
                });
            }

            foreach (var item in list)
            {
                //创建一个DataRow实例
                DataRow row = dt.NewRow();

                //给row 赋值
                if (isProperty)
                {
                    pList.ForEach(p => row[p.Name] = p.GetValue(item));
                }
                else
                {
                    fList.ForEach(f => row[f.Name] = f.GetValue(item));
                }

                //加入到DataTable
                dt.Rows.Add(row);
            }
#if TRACE
            string.Format("转换成DataTable 记录总数:{0}", dt.Rows.Count).Info();
#endif
            return dt;
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="TResult">类型</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt, bool isProperty = true) where T : class, new()
        {
            //创建一个属性的列表
            List<PropertyInfo> prlist = null;
            List<FieldInfo> flist = null;
            DateTime start = DateTime.Now;

            //获取TResult的类型实例  反射的入口
            Type t = typeof(T);

            //获得TResult 的所有的Public 属性 并找出TResult属性和DataTable的列名称相同的属性(PropertyInfo) 并加入到属性列表
            if (isProperty)
            {
                prlist = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(t.GetProperties()
                    , p =>
                    {
                        if (-1 != dt.Columns.IndexOf(p.Name))
                        {
                            prlist.Add(p);
                        }
                    });
            }
            else
            {
                flist = new List<FieldInfo>();

                Array.ForEach<FieldInfo>(t.GetFields()
                    , f =>
                    {
                        if (-1 != dt.Columns.IndexOf(f.Name))
                        {
                            flist.Add(f);
                        }
                    });
            }

            //创建返回的集合
            List<T> oblist = new List<T>();

            foreach (DataRow row in dt.Rows)
            {
                //创建TResult的实例
                T ob = new T();

                //找到对应的数据  并赋值
                if (isProperty)
                {
                    prlist.ForEach(
                        p =>
                        {
                            if (row[p.Name] != DBNull.Value)
                                p.SetValue(ob, row[p.Name]);
                        });
                }
                else
                {
                    flist.ForEach(f =>
                    {
                        if (row[f.Name] != DBNull.Value)
                            f.SetValue(ob, row[f.Name]);
                    });
                }

                //放入到返回的集合中.
                oblist.Add(ob);
            }

            if (null != dt && 1 <= dt.Rows.Count)
            {
                ("<<<Return>>>:" + oblist.Count.ToString() + " Spend:" + (DateTime.Now - start).TotalMilliseconds.ToString()).Debug();
            }

            return oblist;
        }

        /// <summary>
        /// DataTable 转换为List 集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="fiedsType">字段类型:0默认全部(属性+字段) 1:属性部分 2:字段部分</param>
        /// <returns></returns>
        public static List<T> ToList_Old<T>(this DataTable dataTable, byte fiedsType = 0) where T : class, new()
        {
            List<T> list = new List<T>();

            if (null == dataTable || 0 == dataTable.Rows.Count)
            {
                return list;
            }

            List<string> colNameList = new List<string>();
            DateTime start = DateTime.Now;

            foreach (DataColumn dc in dataTable.Columns)
            {
                colNameList.Add(dc.ColumnName);
            }

            Type clsType = typeof(T);

            PropertyInfo[] ps = new PropertyInfo[] { };

            if (0 == fiedsType || 1 == fiedsType)
            {
                ps = clsType.GetProperties();
            }

            FieldInfo[] fs = new FieldInfo[] { };

            if (0 == fiedsType || 2 == fiedsType)
            {
                fs = clsType.GetFields();
            }

            dataTable.CaseSensitive = false;

            foreach (DataRow dr in dataTable.Rows)
            {
                T bean = new T();
                // T bean = Activator.CreateInstance<T>();

                foreach (PropertyInfo p in ps)
                {
                    if (colNameList.Any(a => 0 == string.Compare(a, p.Name, true)))
                    {
                        SetValue<T>(ref bean, p, dr[p.Name]);
                    }
                }

                foreach (FieldInfo f in fs)
                {
                    if (colNameList.Any(a => 0 == string.Compare(a, f.Name, true)))
                    {
                        SetValue<T>(ref bean, f, dr[f.Name]);
                    }
                }

                list.Add(bean);
            }

            if (null != dataTable && 1 <= dataTable.Rows.Count)
            {
                $"<<<Return>>>:{list.Count} Spend:{(DateTime.Now - start).TotalMilliseconds}".Debug();
            }

            return list;
        }

        /// <summary>
        /// 设置值，用于设置对象的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bean"></param>
        /// <param name="p"></param>
        /// <param name="dbValue"></param>
        private static void SetValue<T>(ref T bean, PropertyInfo p, object dbValue)
        {
            if (dbValue is DBNull)
            {
                return;
            }

            string typeName = p.PropertyType.Name;

            if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeName = p.PropertyType.GetGenericArguments()[0].Name;
            }

            // 两种数据类型相同,直接赋值
            if (dbValue.GetType().Name.Equals(typeName))
            {
                p.SetValue(bean, dbValue);
                return;
            }

            // 数据库类型与变量类型不一致,需要转换,以变量的类型为准
            try
            {
                switch (typeName)
                {
                    case "Boolean":
                        if ("F".Equals(dbValue))
                        {
                            p.SetValue(bean, false);
                        }
                        else if ("T".Equals(dbValue))
                        {
                            p.SetValue(bean, true);
                        }
                        else
                        {
                            p.SetValue(bean, Convert.ToBoolean(dbValue));
                        }
                        break;
                    case "Byte":
                        p.SetValue(bean, Convert.ToByte(dbValue));
                        break;
                    case "Char":
                        p.SetValue(bean, Convert.ToChar(dbValue));
                        break;
                    case "UInt16":
                        p.SetValue(bean, Convert.ToUInt16(dbValue));
                        break;
                    case "UInt32":
                        p.SetValue(bean, Convert.ToUInt32(dbValue));
                        break;
                    case "UInt64":
                        p.SetValue(bean, Convert.ToUInt64(dbValue));
                        break;
                    case "Int16":
                        p.SetValue(bean, Convert.ToInt16(dbValue));
                        break;
                    case "Int32":
                        p.SetValue(bean, Convert.ToInt32(dbValue));
                        break;
                    case "Int64":
                        p.SetValue(bean, Convert.ToInt64(dbValue));
                        break;
                    case "Single":
                        p.SetValue(bean, Convert.ToSingle(dbValue));
                        break;
                    case "Double":
                        p.SetValue(bean, Convert.ToDouble(dbValue));
                        break;
                    case "Decimal":
                        p.SetValue(bean, Convert.ToDecimal(dbValue));
                        break;
                    case "DateTime":
                        DateTime dt = DateTime.MinValue;

                        if (null != dbValue)
                        {
                            DateTime.TryParse(dbValue.ToString(), out dt);
                        }

                        p.SetValue(bean, dt);
                        break;
                    default:
                        if ("DateTime".Equals(dbValue.GetType().Name))
                        {
                            p.SetValue(bean, dbValue.ToString());
                        }
                        else
                        {
                            p.SetValue(bean, dbValue);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                // 存在dynamic对象 需要分两行处理,否则报 空指针异常
                string err = string.Format(" {0} Type:{1} Val:{2}\n Err:{3} {4}", p.Name, typeName, dbValue, e.Message, e.StackTrace);
                err.Warn();
            }
        }

        /// <summary>
        /// 设置值，用于设置对象的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bean"></param>
        /// <param name="p"></param>
        /// <param name="dbValue"></param>
        private static void SetValue<T>(ref T bean, FieldInfo p, object dbValue)
        {
            if (dbValue is DBNull)
            {
                return;
            }

            string typeName = p.FieldType.Name;

            if (p.FieldType.IsGenericType && p.FieldType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeName = p.FieldType.GetGenericArguments()[0].Name;
            }

            // 两种数据类型相同,直接赋值
            if (dbValue.GetType().Name.Equals(typeName))
            {
                p.SetValue(bean, dbValue);
                return;
            }

            // 数据库类型与变量类型不一致,需要转换,以变量的类型为准
            try
            {
                switch (typeName)
                {
                    case "Boolean":
                        if ("F".Equals(dbValue))
                        {
                            p.SetValue(bean, false);
                        }
                        else if ("T".Equals(dbValue))
                        {
                            p.SetValue(bean, true);
                        }
                        else
                        {
                            p.SetValue(bean, Convert.ToBoolean(dbValue));
                        }
                        break;
                    case "Byte":
                        p.SetValue(bean, Convert.ToByte(dbValue));
                        break;
                    case "Char":
                        p.SetValue(bean, Convert.ToChar(dbValue));
                        break;
                    case "UInt16":
                        p.SetValue(bean, Convert.ToUInt16(dbValue));
                        break;
                    case "UInt32":
                        p.SetValue(bean, Convert.ToUInt32(dbValue));
                        break;
                    case "UInt64":
                        p.SetValue(bean, Convert.ToUInt64(dbValue));
                        break;
                    case "Int16":
                        p.SetValue(bean, Convert.ToInt16(dbValue));
                        break;
                    case "Int32":
                        p.SetValue(bean, Convert.ToInt32(dbValue));
                        break;
                    case "Int64":
                        p.SetValue(bean, Convert.ToInt64(dbValue));
                        break;
                    case "Single":
                        p.SetValue(bean, Convert.ToSingle(dbValue));
                        break;
                    case "Double":
                        p.SetValue(bean, Convert.ToDouble(dbValue));
                        break;
                    case "Decimal":
                        p.SetValue(bean, Convert.ToDecimal(dbValue));
                        break;
                    case "DateTime":
                        DateTime dt = DateTime.MinValue;

                        if (null != dbValue)
                        {
                            DateTime.TryParse(dbValue.ToString(), out dt);
                        }

                        p.SetValue(bean, dt);
                        break;
                    default:
                        if ("DateTime".Equals(dbValue.GetType().Name))
                        {
                            p.SetValue(bean, dbValue.ToString());
                        }
                        else
                        {
                            p.SetValue(bean, dbValue);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                // 存在dynamic对象 需要分两行处理,否则报 空指针异常
                string err = string.Format(" {0} Type:{1} Val:{2}\n Err:{3} {4}", p.Name, typeName, dbValue, e.Message, e.StackTrace);
                err.Warn();
            }
        }

        /// <summary>
        /// 将集合类转换成DataTable
        /// </summary>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static DataTable ToDataTableTow(IList list)
        {
            DataTable result = new DataTable();

            if (list.Count > 0)
            {
                PropertyInfo[] propertys = list[0].GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        object obj = pi.GetValue(list[i], null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            return result;
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, bool isProperties = false)
        {
            return ToDataTable<T>(list, isProperties, null);
        }

        /// <summary>
        /// 将泛型集合类转换成DataTable
        /// </summary>
        /// <typeparam name="T">集合项类型</typeparam>
        /// <param name="list">集合</param>
        /// <param name="propertyName">需要返回的列的列名</param>
        /// <returns>数据集(表)</returns>
        public static DataTable ToDataTable<T>(IList<T> list, bool isProperties = false, params string[] propertyName)
        {
            List<string> propertyNameList = new List<string>();

            if (null != propertyName)
            {
                propertyNameList.AddRange(propertyName);
            }

            DataTable result = new DataTable();

            if (null != list && 0 >= list.Count)
            {
                return result;
            }

            if (isProperties)
            {
                PropertyInfo[] propertys = typeof(T).GetProperties();

                foreach (PropertyInfo pi in propertys)
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }

                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i], null);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i], null);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }
            else
            {
                T bean = Activator.CreateInstance<T>();
                Type clsType = typeof(T);

                foreach (FieldInfo pi in clsType.GetFields())
                {
                    if (propertyNameList.Count == 0)
                    {
                        result.Columns.Add(pi.Name, pi.FieldType);
                    }
                    else
                    {
                        if (propertyNameList.Contains(pi.Name))
                            result.Columns.Add(pi.Name, pi.FieldType);
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (FieldInfo pi in clsType.GetFields())
                    {
                        if (propertyNameList.Count == 0)
                        {
                            object obj = pi.GetValue(list[i]);
                            tempList.Add(obj);
                        }
                        else
                        {
                            if (propertyNameList.Contains(pi.Name))
                            {
                                object obj = pi.GetValue(list[i]);
                                tempList.Add(obj);
                            }
                        }
                    }
                    object[] array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
            }

            return result;
        }
    }
}
