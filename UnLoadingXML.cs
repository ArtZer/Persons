using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace Persons
{
    class UnLoadingXML
    {
        public void UnLoading(DataSet ds)
        {
            var xmldoc = new XmlDocument();
            xmldoc.InnerXml = ds.GetXml();
            xmldoc.Save("Table");
        }
    }
}
