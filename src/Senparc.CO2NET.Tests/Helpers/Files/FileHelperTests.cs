using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Senparc.CO2NET.Helpers;

namespace Senparc.CO2NET.Tests.Helpers
{
    [TestClass]
    public class FileHelperTests
    {
        [TestMethod]
        public void GetFileStreamTest()
        {
            var fileName = UnitTestHelper.RootPath + "TestEntities\\Logo.jpg";
            var stream = FileHelper.GetFileStream(fileName);
            Assert.IsNotNull(stream);
            Assert.IsTrue(stream.Length > 0);
            Console.WriteLine(stream.Length);
            Assert.AreEqual(117025, stream.Length);//ֻ�Ե�ǰLogo.jpg��Ч
        }

        [TestMethod]
        public void DownLoadFileFromUrlTest()
        {
            var url = "https://sdk.weixin.senparc.com//images/v2/ewm_01.png";
            var savePath = UnitTestHelper.RootPath + $"TestEntities\\download-{SystemTime.Now.ToString("yyyyMMdd-HHmmss")}.jpg";

            FileHelper.DownLoadFileFromUrl(BaseTest.serviceProvider, url, savePath);

            Assert.IsTrue(File.Exists(savePath));

            //ɾ���ļ�
            File.Delete(savePath);
        }
    }
}
