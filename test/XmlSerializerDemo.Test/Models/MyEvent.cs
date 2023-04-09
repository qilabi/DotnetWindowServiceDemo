using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlSerializerDemo.Test.Models
{
    [XmlRoot(ElementName ="Event",Namespace ="")]
    public class MyEvent
    {
        public string Name { get; set; }
        public Student Student { get; set; }
        public int Age { get; set; }
      
    }
    public class Student
    {
        [XmlElement("Id")]
        public IdList IdList { get; set; }
    }
    public class IdList : List<string>, IXmlSerializable
    {
        public XmlSchema? GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (File.Exists("readXml.log"))
                File.Delete("readXml.log"); 
            var list = new List<string>();
            list.Add(CustomizeReader(reader)); 
            while (reader.Read())
            {
                list.Add(CustomizeReader(reader)); 
            }
            File.AppendAllLines("readXml.log", list);
        }
        private bool isStartTag = false;
        private string CustomizeReader(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == "Id") { isStartTag = true; }
            if (reader.NodeType == XmlNodeType.Text && isStartTag) this.Add(reader.Value);
            if (reader.NodeType == XmlNodeType.EndElement) { isStartTag = false; }
            return ($" LocalName='{reader.LocalName}', Name='{reader.Name}' , NodeType = {reader.NodeType}, value={reader.Value}");
           
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
