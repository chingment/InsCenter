﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace System
{
    public static class DbContextDatabaseExtensions
    {
        public static List<T> ToList<T>(this DataTable dt, bool dateTimeToString = true) where T : class, new()
        {
            List<T> list = new List<T>();

            if (dt != null)
            {
                List<PropertyInfo> infos = new List<PropertyInfo>();

                Array.ForEach<PropertyInfo>(typeof(T).GetProperties(), p =>
                {
                    if (dt.Columns.Contains(p.Name) == true)
                    {
                        infos.Add(p);
                    }
                });//获取类型的属性集合

                SetList<T>(list, infos, dt, dateTimeToString);
            }

            return list;
        }

        private static void SetList<T>(List<T> list, List<PropertyInfo> infos, DataTable dt, bool dateTimeToString) where T : class, new()
        {
            foreach (DataRow dr in dt.Rows)
            {
                T model = new T();

                infos.ForEach(p =>
                {
                    if (dr[p.Name] != DBNull.Value)//判断属性在不为空
                    {
                        object tempValue = dr[p.Name];
                        if (dr[p.Name].GetType() == typeof(DateTime) && dateTimeToString == true)//判断是否为时间
                        {
                            tempValue = dr[p.Name].ToString();
                        }
                        try
                        {
                            p.SetValue(model, tempValue, null);//设置
                        }
                        catch { }
                    }
                });
                list.Add(model);
            }
        }
    }
}
