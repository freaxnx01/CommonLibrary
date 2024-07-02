using System;
using System.Collections.Generic;
using System.Reflection;

namespace Library
{
    public static class EnumHelper
    {
        #region TransformEnumToList
        public static List<EnumEntry> TransformEnumToList(Type enumType)
        {
            List<EnumEntry> enumEntries = new List<EnumEntry>();

            foreach (int item in Enum.GetValues(enumType))
            {
                enumEntries.Add(new EnumEntry(item, Enum.GetName(typeof(EnvironmentVariableTarget), item)));
            }

            return enumEntries;
        }

        public class EnumEntry
        {
            public int Value { get; set; }
            public string Name { get; set; }

            public EnumEntry(int value, string name)
            {
                this.Value = value;
                this.Name = name;
            }
        }
        #endregion

        #region TransformEnumToDictionary
        public static Dictionary<string, string> TransformEnumToDictionary(Type enumType)
        {
            Dictionary<string, string> enumEntries = new Dictionary<string, string>();

            string[] enumNames = Enum.GetNames(enumType);
            string[] enumValues = new string[Enum.GetValues(enumType).Length];
            int index = 0;

            foreach (int i in Enum.GetValues(enumType))
            {
                enumValues[index] = i.ToString();
                index++;
            }

            for (int i = 0; i < enumNames.Length; i++)
            {
                if (!enumEntries.ContainsKey(enumNames[i]))
                {
                    enumEntries.Add(enumNames[i], enumValues[i]);
                }
            }

            return enumEntries;
        }
        #endregion

        #region AssignValuesToFlaggedEnumProperty
        public static void AssignValuesToFlaggedEnumProperty(string data, char splitChar, object instance, string propertyName, Type enumType)
        {
            /*
                usage example: AssignValuesToFlaggedEnumProperty(basicAccessRights, ',', boClass, "BasicAccessRights", typeof(BoClass.AccessRights));

                basicAccessRights = "New, Get, Modify, Delete"

                [Flags]
                public enum AccessRights
                {
                    New = 1, Get = 2, Modify = 4, Delete = 8
                }
            */

            if (instance == null)
            {
                return;
            }

            Dictionary<string, string> enumEntries = TransformEnumToDictionary(enumType);

            int? valueToSet = null;

            foreach (string item in data.Split(new char[] { splitChar }))
            {
                int? enumValue = null;

                if (enumEntries.ContainsKey(item.Trim()))
                {
                    enumValue = Convert.ToInt32(enumEntries[item.Trim()]);
                }

                if (enumValue != null)
                {
                    if (valueToSet == null)
                    {
                        valueToSet = enumValue;
                    }
                    else
                    {
                        valueToSet |= enumValue;
                    }
                }
            }

            PropertyInfo pi = instance.GetType().GetProperty(propertyName);

            pi.SetValue(instance, valueToSet, new object[] { });
        }
        #endregion
    }
}
