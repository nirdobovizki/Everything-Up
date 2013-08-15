/*
       Copyright 2011 Nir Dobovizki

       Licensed under the Apache License, Version 2.0 (the "License");
       you may not use this file except in compliance with the License.
       You may obtain a copy of the License at

         http://www.apache.org/licenses/LICENSE-2.0

       Unless required by applicable law or agreed to in writing, software
       distributed under the License is distributed on an "AS IS" BASIS,
       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
       See the License for the specific language governing permissions and
       limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.ComponentModel;
using ServerSelfTest.Base;


namespace ServerSelfTest.Utils
{
    public class XmlConfigLoader : IConfigLoader
    {
        private string ConfigFile { get { return System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\EverythingUp.config.xml"); } }

        public Config Load()
        {
            var result = new List<TestDescription>();

            var doc = new XmlDocument();
            doc.Load(ConfigFile);

            foreach (XmlElement testNode in doc.SelectNodes("//EverythingUp/Tests/Test"))
            {
                var typeAttribute = testNode.Attributes["Type"];
                if (typeAttribute == null)
                {
                    throw new Exception("Problem in test xml file, missing Type attribute on test");
                }
                var testTypeName = typeAttribute.Value;

                var nameAttribute = testNode.Attributes["Name"];
                if (nameAttribute == null)
                {
                    throw new Exception("Problem in test xml file, missing Name attribute on test");
                }
                var testName = nameAttribute.Value;

                var testType = Type.GetType("ServerSelfTest.Tests." + testTypeName + "Test, ServerSelfTest");
                if (testType == null)
                {
                    throw new Exception("Problem in test xml file, can't find class names \"" + testTypeName + "Test\"");
                }

                if (testType.FindInterfaces((i, o) => i == typeof(ITest), null).Length == 0)
                {
                    throw new Exception("Problem in test xml file, class \"" + testTypeName + "Test\" does not implement ITest");
                }

                var testDesc = new TestDescription
                    {
                        Name = testName,
                        Test = (ITest)Activator.CreateInstance(testType),
                    };

                foreach (XmlAttribute attribute in testNode.Attributes)
                {
                    if(attribute.Name == "Type" || attribute.Name == "Name")
                    {
                        continue;
                    }
                    AddToTest(testDesc.Test, testType, attribute.Name, attribute.Value);
                }

                foreach (XmlNode childNode in testNode.ChildNodes)
                {
                    if (childNode.NodeType == XmlNodeType.Element)
                    {
                        AddToTest(testDesc.Test, testType, childNode.LocalName, childNode.InnerText);
                    }
                }

                result.Add(testDesc);
            }

            return new Config() { Tests = result, Tag = System.IO.File.GetLastWriteTime(ConfigFile) };
        }

        private void AddToTest(ITest test, Type testType, string propertyName, string value)
        {
            var propInfo = testType.GetProperty(propertyName);
            if (propInfo == null)
            {
                throw new Exception("Problem in test xml file, class \"" + testType.Name + "Test\" does have property named \"" + propertyName + "\"");
            }
            propInfo.SetValue(
                test,
                TypeDescriptor.GetConverter(propInfo.PropertyType).ConvertFromInvariantString(value),
                null);
        }


        public bool NeedReload(object tag)
        {
            return tag == null || !(tag is DateTime) || System.IO.File.GetLastWriteTime(ConfigFile) != (DateTime)tag;
        }

        public void Save(Config config)
        {
            throw new NotImplementedException();
        }
    }
}