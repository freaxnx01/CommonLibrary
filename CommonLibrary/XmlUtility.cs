using System.Xml;

namespace Library.Misc
{
    public static class XmlUtility
    {
        #region GetPathFromNode
        public static string GetPathFromNode(XmlNode baseNode)
        {
            string path = "";
            XmlNodeList nodes = null;
            if (baseNode.NodeType == XmlNodeType.Attribute)
            {
                nodes = baseNode.SelectNodes("ancestor::*");
            }
            else
            {
                nodes = baseNode.SelectNodes("ancestor-or-self::*");
                //nodes = baseNode.SelectNodes("ancestor-or-self::* | ancestor-or-self::@*");

            }
            foreach (XmlNode node in nodes)
            {
                int nodePosition =
            node.SelectNodes("preceding-sibling::*[local-name()='" + node.LocalName + "' and namespace-uri()='" + node.NamespaceURI + "']").Count + 1;
                path += "/" + GetQName(node) + "[" + nodePosition.ToString() + "]";
            }
            if (baseNode.NodeType == XmlNodeType.Attribute)
                path += "/" + GetQName(baseNode);
            return (path);
        }

        private static string GetQName(XmlNode node)
        {
            string qname = string.Empty;

            if (node.NamespaceURI.CompareTo(string.Empty) != 0)
            {
                if (node.Prefix.CompareTo(string.Empty) != 0)
                {
                    //If the prefix is present, use it.

                    if (node.NodeType == XmlNodeType.Attribute) { qname = "@"; }
                    qname = qname + node.Prefix + ":" + node.LocalName;
                }
                else
                {
                    //The node is in the default namespace, the prefix is not
                    //present.
                    if (node.NodeType == XmlNodeType.Attribute)
                        qname = "@*[local-name() = '" + node.LocalName + "' and namespace-uri()='" + node.NamespaceURI + "']";
                    else
                        //QName is a misnomer here, but the current node belongs in a non-prefixed namespace...
                        qname = "node()[local-name() = '" + node.LocalName + "' and namespace-uri()='" + node.NamespaceURI + "']";
                }
            }
            else
            {
                if (node.NodeType == XmlNodeType.Attribute)
                    qname = "@" + node.Name;
                else
                    qname = node.Name;
            } return (qname);
        }
        #endregion
    }
}
