using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Helpers;
using Senparc.CO2NET.Helpers.Serializers;

namespace Senparc.CO2NET.Tests.Helpers
{
    [TestClass]
    public  class SerializerHelperJsonTests
    {
        #region ���������µ����л����ԣ�����null���ض�ֵֵ���ض����Բ��ԣ�΢�Žӿ���������Ҫ��
        [TestMethod]
        public void GetJsonStringTest_Null()
        {
            var obj =
                new
                {
                    X =
                        new RootClass()
                        {
                            A = "Jeffrey",
                            B = 31,
                            C = null,
                            ElementClassA = new ElementClass() { A = "Jeffrey", B = null },
                            ElementClassB = null
                        },
                    Y = new
                    {
                        O = "0",
                        Z = (string)null
                    }
                };


            var dt1 = SystemTime.Now;

            {
                //�������κ����ã�����ԭʼJSON
                var json = SerializerHelper.GetJsonString(obj, jsonSetting: null);
                Console.WriteLine(json);
                var exceptedJson = "{\"X\":{\"A\":\"Jeffrey\",\"B\":31,\"C\":null,\"ElementClassA\":{\"A\":\"Jeffrey\",\"B\":null,\"RootClass\":null},\"ElementClassB\":null,\"ElementClass2\":null},\"Y\":{\"O\":\"0\",\"Z\":null}}";
                Assert.AreEqual(exceptedJson, json);
            }


            {
                //�������κ�����
                var json = SerializerHelper.GetJsonString(obj, new JsonSetting(false));
                Console.WriteLine(json);
                var exceptedJson = "{\"X\":{\"A\":\"Jeffrey\",\"B\":31,\"ElementClassA\":{\"A\":\"Jeffrey\",\"B\":null,\"RootClass\":null},\"ElementClassB\":null,\"ElementClass2\":null},\"Y\":{\"O\":\"0\",\"Z\":null}}";
                Assert.AreEqual(exceptedJson, json);
            }

            {
                //��������Ϊnull������
                var json = SerializerHelper.GetJsonString(obj, new JsonSetting(true));
                Console.WriteLine(json);
                var exceptedJson = "{\"X\":{\"A\":\"Jeffrey\",\"B\":31,\"ElementClassA\":{\"A\":\"Jeffrey\"}},\"Y\":{\"O\":\"0\"}}";
                Assert.AreEqual(exceptedJson, json);


                var obj2 = new RootClass()
                {
                    A = "Jeffrey",
                    B = 31,
                    C = null,
                    ElementClassA = new ElementClass() { A = "Jeffrey", B = null },
                    ElementClassB = null
                };

                var json2 = SerializerHelper.GetJsonString(obj2,
                                new JsonSetting(true, new List<string>(new[] { "B" })));

                var exceptedJson2 = "{\"A\":\"Jeffrey\",\"B\":31,\"ElementClassA\":{\"A\":\"Jeffrey\"}}";
                Console.WriteLine(json2);
                Assert.AreEqual(exceptedJson2, json2);

            }

            {
                //�����ض�Ϊnull������
                var json = SerializerHelper.GetJsonString(obj,
                                new JsonSetting(false, new List<string>(new[] { "Z" })));//Z���Իᱻ����
                Console.WriteLine(json);
                var exceptedJson = "{\"X\":{\"A\":\"Jeffrey\",\"B\":31,\"ElementClassA\":{\"A\":\"Jeffrey\",\"B\":null,\"RootClass\":null},\"ElementClassB\":null,\"ElementClass2\":null},\"Y\":{\"O\":\"0\"}}";
                Assert.AreEqual(exceptedJson, json);
            }

            {
                //�����ض�ֵ���ԣ������ض�ֵ���Լ�����null��
                var obj4 = new RootClass()
                {
                    A = "IGNORE",//�ᱻ����
                    B = 31,
                    C = null,
                    ElementClassA = null,
                    ElementClassB = null,
                    ElementClass2 = null
                };
                var json = SerializerHelper.GetJsonString(obj4, new JsonSetting(true));//Z���Իᱻ����
                Console.WriteLine(json);
                var exceptedJson = "{\"B\":31}";
                Assert.AreEqual(exceptedJson, json);
            }

            {
                //�����ض�ֵ���ԣ�ֻ�����ض�ֵ��������null��
                var obj4 = new RootClass()
                {
                    A = "IGNORE",//�ᱻ����
                    B = 31,
                    C = null,
                    ElementClassA = null,
                    ElementClassB = null,
                    ElementClass2 = null
                };
                var json = SerializerHelper.GetJsonString(obj4, new JsonSetting(false));//Z���Իᱻ����
                Console.WriteLine(json);
                var exceptedJson = "{\"B\":31,\"ElementClassA\":null,\"ElementClassB\":null,\"ElementClass2\":null}";
                Assert.AreEqual(exceptedJson, json);
            }

            {
                //�����ض�ֵ���ԣ���ƥ�䣬��˲����ԣ�
                var obj4 = new RootClass()
                {
                    A = "DO NET IGNORE",//���ᱻ����
                    B = 31,
                    C = null,
                    ElementClassA = null,
                    ElementClassB = null,
                    ElementClass2 = null
                };
                var json = SerializerHelper.GetJsonString(obj4, new JsonSetting(true));//Z���Իᱻ����
                Console.WriteLine(json);
                var exceptedJson = "{\"A\":\"DO NET IGNORE\",\"B\":31}";
                Assert.AreEqual(exceptedJson, json);
            }

            {
                //�����ض�����Ϊnull������
                var obj3 = new RootClass()
                {
                    A = "Jeffrey",
                    B = 31,
                    C = null,
                    ElementClassA = new ElementClass() { A = "Jeffrey", B = null },
                    ElementClassB = null,
                    ElementClass2 = null//���ᱻ����
                };

                var json3 = SerializerHelper.GetJsonString(obj3,
                             new JsonSetting(false, null, new List<Type>(new[] { typeof(ElementClass), typeof(ElementClass2) })));
                Console.WriteLine(json3);
                var exceptedJson3 = "{\"A\":\"Jeffrey\",\"B\":31,\"ElementClassA\":{\"A\":\"Jeffrey\",\"B\":null,\"RootClass\":null}}";
                Assert.AreEqual(exceptedJson3, json3);

            }



            Console.WriteLine(SystemTime.DiffTotalMS(dt1));
        }

        public class RootClass /*: JsonIgnoreNull, IJsonIgnoreNull*/
        {
            private string _private { get; set; }
            [JsonSetting.IgnoreValue("IGNORE")]
            public string A { get; set; }
            public int B { get; set; }
            [JsonSetting.IgnoreNull]
            public int? C { get; set; }
            public ElementClass ElementClassA { get; set; }
            public ElementClass ElementClassB { get; set; }
            public ElementClass2 ElementClass2 { get; set; }

            public RootClass()
            {
                _private = "Private";
            }
        }

        public class ElementClass /*: JsonIgnoreNull, IJsonIgnoreNull*/
        {
            public string A { get; set; }
            public string B { get; set; }
            public RootClass RootClass { get; set; }
        }

        public class ElementClass2
        {
            public string A { get; set; }
            public string B { get; set; }
            public RootClass RootClass { get; set; }
        }
        #endregion


        #region ���ӣ�Ƕ�����ͣ������л�����

        [TestMethod]
        public void GetObjectTest()
        {
            var json = "{\"A\":\"Jeffrey\",\"B\":31,\"ElementClassA\":{\"A\":\"Jeffrey\"}}";
            var result = SerializerHelper.GetObject<RootClass>(json);
            Assert.IsNotNull(result);

            Assert.AreEqual("Jeffrey", result.A);
            Assert.AreEqual(31, result.B);
            Assert.IsNotNull(result.ElementClassA);
            Assert.AreEqual("Jeffrey", result.ElementClassA.A);
            Assert.IsNull(result.ElementClassA.B);
            Assert.IsNull(result.ElementClassB);
            Assert.IsNull(result.ElementClass2);
        }

        #endregion


        #region �����л�/�����л�����

        [TestMethod]
        public void Simple_GetJsonStringTest()
        {
            var data = new Data()
            {
                Id = 1,
                Name = "Senparc",
                DateTime = new DateTime(2018, 6, 18, 14, 50, 30, 897, DateTimeKind.Local)
            };
            string json = SerializerHelper.GetJsonString(data);
            Console.WriteLine(json);
            Assert.AreEqual("{\"Id\":1,\"Name\":\"Senparc\",\"DateTime\":\"2018-06-18T14:50:30.897+08:00\"}", json);
        }


        [TestMethod]
        public void Simple_GetObjectTest()
        {
            var data = new Data()
            {
                Id = 1,
                Name = "Senparc",
                DateTime = new DateTime(2018, 6, 18, 14, 50, 30, 897, DateTimeKind.Local)
            };
            string json = "{\"Id\":1,\"Name\":\"Senparc\",\"DateTime\":\"2018-06-18T14:50:30.897+08:00\"}";
                var obj = SerializerHelper.GetObject<Data>(json);

            Assert.IsNotNull(obj);
            Assert.AreEqual(1, obj.Id);
            Assert.AreEqual("Senparc", obj.Name);
            Assert.AreEqual(new DateTime(2018, 6, 18, 14, 50, 30, 897, DateTimeKind.Local), obj.DateTime);
        }


        [Serializable]
        public class Data
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime DateTime { get; set; }
        }


        #endregion

        #region ExpandoObject����ת������

        [TestMethod]
        public void GetJsonStringTest_Expando()
        {
            dynamic test = new ExpandoObject();
            test.x = "Senparc.Weixin SDK";
            test.y = SystemTime.Now;

            var dt1 = SystemTime.Now;

            var json = SerializerHelper.GetJsonString(test);
            Console.WriteLine(json);

            Console.WriteLine(SystemTime.DiffTotalMS(dt1));

        }

        #endregion
    }
}
