using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq; // LINQ 쿼리를 사용하기 위해

namespace testMouse.Helpers
{
    class XmlHelper
    {
        private readonly string Path = @"Config\config.xml";
        private readonly string DevPath = @"..\..\Config\config.xml";
        private static XmlHelper _instance { get; set; }
        public static XmlHelper Instance
        {
            get
            {
                return _instance ?? (_instance = new XmlHelper());
            }
        }

        public string Get(string key)
        {
            XDocument xdoc = XDocument.Load(Path);
            XElement item = xdoc.Descendants("item")
                    .FirstOrDefault(e => e.Element("position")?.Value == key);

            if (item != null)
            {
                return item.Element("coordinate")?.Value;
            }
            else
            {
                Console.WriteLine($"Item with name '{key}' not found.");
                return "fail";
            }
        }

        public void Save(string key, string value)
        {
            XDocument xdoc = XDocument.Load(Path);

            // 해당 키를 가진 엘리먼트 찾기
            XElement item = xdoc.Descendants("item")
                                .FirstOrDefault(e => e.Element("position")?.Value == key.ToString());

            if (item != null)
            {
                // 키가 존재하면 값 업데이트
                item.SetElementValue("coordinate", value);
            }
            else
            {
                // 키가 존재하지 않으면 새 엘리먼트 추가
                xdoc.Element("items").Add(
                    new XElement("item",
                        new XElement("position", key),
                        new XElement("coordinate", value)
                    )
                );
            }

            // 변경사항을 다시 파일로 저장
            xdoc.Save(Path);
            //xdoc.Save(DevPath);
        }

    }
}
