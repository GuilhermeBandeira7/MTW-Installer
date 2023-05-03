using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UtilsCore
{
    public class ClassComparisonCore
    {
        #region SINGLETON

        private static ClassComparisonCore instance = null;

        private ClassComparisonCore()
        {

        }

        public static ClassComparisonCore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClassComparisonCore();
                }
                return instance;
            }
        }


        #endregion

        public bool CompareObjects(object objA, object objB)
        {
            Type objType = objA.GetType();
            FieldInfo[] fields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            try
            {
                foreach (var field in fields)
                {
                    if (field.GetValue(objA) != null && field.GetValue(objB) != null)
                    {
                        if ((field.GetValue(objA).GetType() == typeof(string) ||
                              field.GetValue(objA).GetType() == typeof(long) ||
                              field.GetValue(objA).GetType() == typeof(int) ||
                              field.GetValue(objA).GetType() == typeof(bool) ||
                              field.GetValue(objA).GetType() == typeof(DateTime) ||
                              field.GetValue(objA).GetType() == typeof(double) ||
                              field.GetValue(objA).GetType().IsEnum))
                        {
                            if (field.GetValue(objA).ToString() != field.GetValue(objB).ToString())
                            {
                                return false;
                            }
                        }
                        else if (typeof(IDictionary).IsAssignableFrom(field.GetValue(objA).GetType()))
                        {
                            IDictionary idictA = (IDictionary)field.GetValue(objA);
                            IDictionary idictB = (IDictionary)field.GetValue(objB);

                            if (idictB.Count != idictA.Count)
                                return false;

                            foreach (object key in idictA.Keys)
                            {
                                if (!idictB.Contains(key))
                                    return false;

                                if (!CompareObjects(idictA[key], idictB[key]))
                                    return false;
                            }

                            foreach (object key in idictB.Keys)
                            {
                                if (!idictA.Contains(key))
                                    return false;

                                if (!CompareObjects(idictA[key], idictB[key]))
                                    return false;
                            }



                        }
                        else if (!CompareObjects(field.GetValue(objA), field.GetValue(objB)))
                        {
                            return false;
                        }
                    }
                }

            }
            catch
            {
                return false;
            }

            return true;

        }


        public bool CompareDictionary(object objA, object objB)
        {
            Type objType = objA.GetType();
            FieldInfo[] fields = objType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            try
            {
                IDictionary idictA = (IDictionary)objA;
                IDictionary idictB = (IDictionary)objB;

                if (idictB.Count != idictA.Count)
                    return false;

                foreach (object key in idictA.Keys)
                {
                    if (!idictB.Contains(key))
                        return false;

                    if (!CompareObjects(idictA[key], idictB[key]))
                        return false;
                }

                foreach (object key in idictB.Keys)
                {
                    if (!idictA.Contains(key))
                        return false;

                    if (!CompareObjects(idictA[key], idictB[key]))
                        return false;
                }

            }
            catch 
            {
                return false;
            }

            return true;

        }

        public void UpdateObjects(object objA, object objB, int ignoreValue)
        {
            Type objAType = objA.GetType();
            FieldInfo[] fieldsA = objAType.GetFields();

            Type objBType = objB.GetType();
            FieldInfo[] fieldsB = objBType.GetFields();

            if (objAType != objBType)
            {
                objA = objB;
                return;
            }

            try
            {
                foreach (var field in fieldsA)
                {
                    if (field.GetValue(objA) != null && field.GetValue(objB) != null)
                    {
                        if ((field.GetValue(objA).GetType() == typeof(string) ||
                              field.GetValue(objA).GetType() == typeof(long) ||
                              field.GetValue(objA).GetType() == typeof(int) ||
                              field.GetValue(objA).GetType() == typeof(bool) ||
                              field.GetValue(objA).GetType() == typeof(DateTime) ||
                              field.GetValue(objA).GetType() == typeof(double) ||
                              field.GetValue(objA).GetType().IsEnum))
                        {
                            if (field.GetValue(objA).ToString() != field.GetValue(objB).ToString())
                            {
                                if (field.GetValue(objA).GetType() == typeof(int))
                                {
                                    if(int.Parse(field.GetValue(objB).ToString()) != ignoreValue)
                                        field.SetValue(objA, field.GetValue(objB));
                                }
                                else
                                {
                                    field.SetValue(objA, field.GetValue(objB));
                                }
                               
                            }
                        }
                        else if (typeof(IDictionary).IsAssignableFrom(field.GetValue(objA).GetType()))
                        {

                        }
                        else
                        {
                            UpdateObjects(field.GetValue(objA), field.GetValue(objB), ignoreValue);
                        }
                    }
                }
            }
            catch
            {

            }


        }
    }
}
