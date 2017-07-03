using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ChaYeFeng
{
    public class CYFXMLHelper
    {
        #region 公共变量
        static XmlDocument xmldoc;
        static XmlElement xmlelem;
        #endregion

        #region 创建根节点
        /// <summary>
        /// 创建带版本号的xml文档对象
        /// </summary>
        /// <param name="version"></param>
        /// <param name="Encode"></param>
        /// <returns></returns>
        public static XmlDocument CreateXmlDocWithVersionEncode(string version, string Encode)
        {
            xmldoc = CYFXMLHelper.CreateXmlDoc();
            XmlDeclaration xmlDecl;
            xmlDecl = xmldoc.CreateXmlDeclaration(version, Encode, null);
            xmldoc.AppendChild(xmlDecl);
            return xmldoc;
        }

        /// <summary>
        /// 创建xml文档
        /// </summary>
        /// <param name="name">根节点名称</param>
        /// <param name="type">根节点的一个属性</param>
        /// <returns></returns>
        public static XmlDocument CreateXmlDocWithNameType(string name, string type)
        {
            xmldoc = new XmlDocument();
            try
            {
                xmldoc.LoadXml("<" + name + "/>");
                xmlelem = xmldoc.DocumentElement;
                xmlelem.SetAttribute("type", type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return xmldoc;
        }

        /// <summary>
        /// 创建空的xml文档对象
        /// </summary>
        /// <returns></returns>
        public static XmlDocument CreateXmlDoc()
        {
            xmldoc = new XmlDocument();
            return xmldoc;
        }
        #endregion

        #region 根据DataTable生成xml
        /// <summary>
        /// 根据DataTable生成xml
        /// </summary>
        /// <param name="curDoc">xml对象</param>
        /// <param name="dt">DataTable</param>
        public static void CreateXmlFromDataTable(XmlDocument curDoc, DataTable dt)
        {
            if (dt == null)
                return;
            if (string.IsNullOrEmpty(dt.TableName))
                dt.TableName = "Table";
            xmlelem = curDoc.CreateElement(dt.TableName);
            curDoc.AppendChild(xmlelem);
            if (dt.Columns.Count > 0 && dt.Rows.Count > 0)
            {
                XmlElement tempElem = null;
                XmlElement rowElem = null;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    rowElem = curDoc.CreateElement("DataTable");
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        tempElem = curDoc.CreateElement(dt.Columns[j].ColumnName.Trim());
                        tempElem.InnerText = dt.Rows[i][j].ToString();
                        rowElem.AppendChild(tempElem);
                    }
                    xmlelem.AppendChild(rowElem);
                }
            }
        }
        #endregion

        #region xml转换为string
        /// <summary>
        /// 将xml转换为string
        /// </summary>
        /// <param name="curXmlDoc"></param>
        /// <param name="includeCodeType"></param>
        /// <param name="inDent"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        public static string XmlDocToString(XmlDocument curXmlDoc, bool includeCodeType = false, bool inDent = false)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = inDent;
            settings.OmitXmlDeclaration = !includeCodeType;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriter writer = XmlTextWriter.Create(ms, settings);
                curXmlDoc.Save(writer);
                using (StreamReader sr = new StreamReader(ms, Encoding.UTF8))
                {
                    ms.Position = 0;
                    string xmlString = sr.ReadToEnd();
                    return xmlString;
                }
            }
        }
        #endregion

        #region 将xml文件里面的内容读取后转化为DataSet
        /// <summary>
        /// 将xml文件里面的内容读取后转化为DataSet
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static DataSet XmlToDataSet(string xmlData)
        {
            DataSet result = null;
            if (!string.IsNullOrEmpty(xmlData))
            {
                result = new DataSet();
                result.ReadXml(new StringReader(xmlData));
            }
            return result;
        }
        #endregion

        #region 获取xml文件下某个节点的数据
        /// <summary>
        /// 获取xml文件下某个节点下的数据
        /// </summary>
        /// <param name="curXmlDoc">文档对象</param>
        /// <param name="node">节点名</param>
        /// <param name="attribute">属性名</param>
        /// <returns></returns>
        public static string Read(XmlDocument curXmlDoc, string node, string attribute)
        {
            string value = "";
            if (curXmlDoc != null)
            {
                try
                {
                    XmlNode curNode = xmldoc.SelectSingleNode(node);
                    value = (attribute.Equals("") ? curNode.InnerText : curNode.Attributes[attribute].Value);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return value;
        }
        #endregion

        #region 在xml插入字符串
        /// <summary>
        /// 在xml插入字符串
        /// </summary>
        /// <param name="curXmlDoc">xml文件</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名</param>
        /// <param name="attribute">属性名</param>
        /// <param name="value">值</param>
        public static void Insert(XmlDocument curXmlDoc, string node, string element, string attribute, string value)
        {
            try
            {
                if (curXmlDoc != null)
                {
                    XmlNode newNode = curXmlDoc.SelectSingleNode(node);
                    if (string.IsNullOrEmpty(element))
                    {
                        if (!string.IsNullOrEmpty(attribute))
                        {
                            xmlelem = (XmlElement)newNode;
                            xmlelem.SetAttribute(attribute, value);
                        }
                    }
                    else
                    {
                        xmlelem = curXmlDoc.CreateElement(element);
                        if (string.IsNullOrEmpty(attribute))
                            xmlelem.InnerText = value;
                        else
                            xmlelem.SetAttribute(attribute, value);
                        newNode.AppendChild(xmlelem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 修改节点或节点属性数据
        /// <summary>
        /// 修改节点或节点属性数据
        /// </summary>
        /// <param name="curXmlDoc">xml对象</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名</param>
        /// <param name="value">值</param>
        public static void Update(XmlDocument curXmlDoc, string node, string attribute, string value)
        {
            try
            {
                if (curXmlDoc != null)
                {
                    XmlNode curNode = curXmlDoc.SelectSingleNode(node);
                    xmlelem = (XmlElement)curNode;
                    if (string.IsNullOrEmpty(attribute))
                        xmlelem.InnerText = value;
                    else
                    {
                        xmlelem.SetAttribute(attribute, value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除xml节点数据
        public static void Delete(XmlDocument curXmlDoc, string node, string attribute)
        {
            try
            {
                if (curXmlDoc != null)
                {
                    XmlNode curNode = curXmlDoc.SelectSingleNode(node);
                    xmlelem = (XmlElement)curNode;
                    if (string.IsNullOrEmpty(attribute))
                        curNode.ParentNode.RemoveChild(curNode);
                    else
                        xmlelem.RemoveAttribute(attribute);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 读xml资源到dataset中
        /// <summary>
        /// 读xml资源到dataset中
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string source)
        {
            try
            {
                DataSet ds = new DataSet();
                xmldoc = new XmlDocument();
                xmldoc.LoadXml(source);
                XmlNodeReader xnr = new XmlNodeReader(xmldoc);
                ds.ReadXml(xnr);
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取中xml文件中指定节点的数据
        /// <summary>
        /// 获取中xml文件中指定节点的数据
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        public static string GetNodeInfoByNodeName(XmlDocument doc, string nodeName)
        {
            string xmlString = "";
            XmlElement root = doc.DocumentElement;
            XmlNode node = root.SelectSingleNode(nodeName);
            if (node != null)
                xmlString = node.InnerText;
            return xmlString;
        }
        #endregion

        #region 读取xml资源到DataTable中
        /// <summary>
        /// 读取xml资源到DataTable中
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetTable(string source, string tableName)
        {
            DataTable dt = new DataTable();
            DataSet ds = GetDataSet(source);
            if (ds.Tables.Count > 0 && ds.Tables.Contains(tableName))
                dt = ds.Tables[tableName];
            return dt;
        }
        #endregion

        #region 读取xml资源中指定的DataTable的指定行指定列名的值
        /// <summary>
        /// 读取xml资源中指定的DataTable的指定行指定列的值
        /// </summary>
        /// <param name="source">xml资源</param>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="ColumnIndex">列名</param>
        /// <returns></returns>
        public static object GetTableCell(string source, string tableName, int rowIndex, string ColumnName)
        {
            DataTable dt = GetTable(source, tableName);
            int columnIndex;
            if (dt.Columns.Contains(ColumnName))
            {
                columnIndex = dt.Columns.IndexOf(ColumnName);
                return GetTableCell(source, tableName, rowIndex, columnIndex);
            }
            return null;
        }
        #endregion

        #region 读取xml资源中指定的DataTable的指定行指定列号的值
        /// <summary>
        /// 读取xml资源中指定的DataTable的指定行指定列号的值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="tableName"></param>
        /// <param name="rowIndex"></param>
        /// <param name="ColumnIndex"></param>
        /// <returns></returns>
        public static object GetTableCell(string source, string tableName, int rowIndex, int ColumnIndex)
        {
            DataTable dt = GetTable(source, tableName);
            if (dt.Columns.Count > 0 && dt.Rows.Count > 0
                && dt.Columns.Count > ColumnIndex && dt.Rows.Count > rowIndex)
            {
                return dt.Rows[rowIndex][ColumnIndex];
            }
            else
                return null;
        }
        #endregion

        #region 添加子节点
        public static void AddNode(XmlDocument curDoc, XmlNode node)
        {
            curDoc.AppendChild(node);
        }
        #endregion

        #region 将DataTable写入xml文件中
        /// <summary>
        /// 指定DataTable生产xml文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="fileName"></param>
        public static void DataTableWriteToXml(DataTable dt, string fileName)
        {
            DataSet ds = new DataSet();
            DataTable newdt = dt.Copy();
            ds.Tables.Add(newdt);
            ds.WriteXml(fileName);
        }
        #endregion

        #region 将DataTable以指定的根结点名称方式写入xml文件
        /// <summary>
        /// 将DataTable以指定的根结点名称方式写入xml文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="rootName"></param>
        /// <param name="fileName"></param>
        public static void SaveTableToFile(DataTable dt, string rootName, string fileName)
        {
            DataSet ds = new DataSet(rootName);
            DataTable newdt = dt.Copy();
            ds.Tables.Add(newdt);
            ds.WriteXml(fileName);
        }
        #endregion

        #region 使用DataSet方式更新xml文件节点
        public static bool UpdateTableCell(string fileName,string tableName,int rowIndex,string colName,string content)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(fileName);
            if(!ds.Tables.Contains(tableName))
            return false;
            DataTable dt = ds.Tables[tableName];
            if (dt.Rows.Count > rowIndex && dt.Columns.Contains(colName))
            {
                dt.Rows[rowIndex][colName] = content;
                ds.WriteXml(fileName);
                return true;
            }
            return false;
        }
        #endregion
    }
}
