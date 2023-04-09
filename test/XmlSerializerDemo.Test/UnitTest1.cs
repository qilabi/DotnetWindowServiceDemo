using XmlSerializerDemo.Test.Models;
using Shouldly;
using System.Xml;
using System.Xml.Serialization;

namespace XmlSerializerDemo.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Should_XmlSerializer_ArrayStringList()
        {
            var xml = await File.ReadAllTextAsync("Xml/XMLFile1.xml");
            var type = typeof(MyEvent);
            var serializer = new XmlSerializer(type);
            var sr = new StringReader(xml);
            MyEvent? evt = (MyEvent?)serializer.Deserialize(sr);
            evt?.Student.IdList.Count().ShouldBe(4);
        }
        [Fact]
        public async Task Should_XmlReaderTagOneByOne()
        {
            if (File.Exists("log.log"))
                File.Delete("log.log");
            var xml = await File.ReadAllTextAsync("Xml/XMLFile1.xml");
            var sr = new StringReader(xml);

            using (var rdr = XmlReader.Create(sr))
            {
                var list = new List<string>();
                var i = 1;
                while (rdr.Read())
                {
                    list.Add($"{i}. Name='{rdr.Name}' , NodeType = {rdr.NodeType}, value={rdr.Value}");
                    i++;
                    if (rdr.NodeType == XmlNodeType.Element)
                    {
                        //通过rdr.Name得到节点名
                        string elementName = rdr.Name;
                        //await File.AppendAllTextAsync("log1.log", elementName + " element start");
                        if (elementName == "root")
                        {

                        }
                        //读取到cat元素 这时rdr.Read()读取到的内容为<cat color="white">
                        else if (elementName == "Descriptions")
                        {
                            //读取到节点内文本内容
                            if (rdr.Read())
                            {
                                //通过rdr.Value获得文本内容
                                Console.WriteLine("\t cat said:" + rdr.Value);
                            }
                        }
                    }
                    else if (rdr.NodeType == XmlNodeType.EndElement)
                    {
                        //在节点结束时也可以通过rdr.Name获得节点名字
                        string elementName = rdr.Name;
                        Console.WriteLine(elementName + " element end");
                    }
                }
                await File.AppendAllLinesAsync("log.log", list);
            }

        }
    }
}