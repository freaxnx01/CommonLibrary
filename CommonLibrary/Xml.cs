using System;
using System.Data;
using System.IO;
using System.Xml;

namespace Library.Xml
{
    public static class XmlUtility
    {
        #region GetDataTable
        public static DataTable GetDataTable(XmlNodeList nodelist)
        {
            DataTable table = null;
            XmlNode node = null;
            if (nodelist == null)
                return null;

            // get parameter names
            node = nodelist.Item(0);
            if (node == null)
                return null;

            XmlAttributeCollection attrCollection = node.Attributes;
            if (attrCollection == null)
                return null;
            if (attrCollection.Count == 0)
                return null;

            // create data table
            table = new DataTable();
            foreach (XmlAttribute attr in attrCollection)
            {
                table.Columns.Add(attr.Name);
            }

            // add rows
            DataRow row = null;
            foreach (XmlNode n in nodelist)
            {
                row = table.NewRow();
                foreach (XmlAttribute a in n.Attributes)
                {
                    row[a.Name] = a.Value;
                }
                table.Rows.Add(row);
            }

            table.AcceptChanges();
            return table;
        }
        #endregion

        #region FormatXml
        public static string FormatXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return FormatXml(doc);
        }

        public static string FormatXml(XmlDocument doc)
        {
            string data = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(sw))
                {
                    writer.Formatting = System.Xml.Formatting.Indented;
                    doc.WriteTo(writer);
                    data = sw.ToString();
                }
            }

            return data;
        }
        #endregion

        public static int Increment(string filePath, string xpathToCounterNode, int incrementStepSize)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpathToCounterNode);
                int counter = 0;
                if (xmlNode != null)
                {
                    if (int.TryParse(xmlNode.InnerText, out counter))
                    {
                        counter += incrementStepSize;
                        xmlNode.InnerText = counter.ToString();
                        xmlDoc.Save(filePath);
                        return counter;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return 0;
        }

        public static void WriteValueToXmlFile(string filePath, string xpathToCounterNode, string value)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(filePath);
            }

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filePath);
                XmlNode xmlNode = xmlDoc.SelectSingleNode(xpathToCounterNode);
                if (xmlNode != null)
                {
                    xmlNode.InnerText = value.ToString();
                    xmlDoc.Save(filePath);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
