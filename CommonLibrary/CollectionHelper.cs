using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Xml.Serialization;

namespace Library
{
    public class CollectionHelper
    {
        #region ConvertToDataTable
        public static DataTable ConvertToDataTable<T>(IList<T> list)
        {
            DataTable table = CreateDataTable<T>();
            Type entityType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (T item in list)
            {
                DataRow row = table.NewRow();

                foreach (PropertyDescriptor prop in properties)
                {
                    try
                    {
                        row[prop.Name] = prop.GetValue(item);
                    }
                    catch { }
                }

                table.Rows.Add(row);
            }

            return table;
        }

        public static DataTable CreateDataTable<T>()
        {
            Type entityType = typeof(T);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, prop.PropertyType);
            }

            return table;
        }
        #endregion

        #region ConvertToList
        public static IList<T> ConvertToList<T>(DataTable table)
        {
            if (table == null)
            {
                return null;
            }

            List<DataRow> rows = new List<DataRow>();

            foreach (DataRow row in table.Rows)
            {
                rows.Add(row);
            }

            return ConvertTo<T>(rows);
        }

        private static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;

            if (rows != null)
            {
                list = new List<T>();

                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }

            return list;
        }

        private static T CreateItem<T>(DataRow row)
        {
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();

                foreach (DataColumn column in row.Table.Columns)
                {
                    PropertyInfo prop = obj.GetType().GetProperty(column.ColumnName);
                    try
                    {
                        object value = row[column.ColumnName];
                        prop.SetValue(obj, value, null);
                    }
                    catch
                    {
                        // You can log something here
                        throw;
                    }
                }
            }

            return obj;
        }
        #endregion

        #region SerializableDictionary
        [XmlRoot("dictionary")]
        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
        {
            #region IXmlSerializable Members

            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));
                bool wasEmpty = reader.IsEmptyElement;
                reader.Read();

                if (wasEmpty)
                    return;

                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("item");
                    reader.ReadStartElement("key");
                    TKey key = (TKey)keySerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadStartElement("value");
                    TValue value = (TValue)valueSerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    this.Add(key, value);
                    reader.ReadEndElement();
                    reader.MoveToContent();
                }

                reader.ReadEndElement();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

                foreach (TKey key in this.Keys)
                {
                    writer.WriteStartElement("item");
                    writer.WriteStartElement("key");
                    keySerializer.Serialize(writer, key);
                    writer.WriteEndElement();
                    writer.WriteStartElement("value");
                    TValue value = this[key];
                    valueSerializer.Serialize(writer, value);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
            }
            #endregion
        }
        #endregion
    }
}
