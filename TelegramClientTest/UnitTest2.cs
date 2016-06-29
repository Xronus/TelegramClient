using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace TelegramClientTest
{
    [TestClass]
    public class UnitTest2
    {
        private string NumberToSendMessage = "89151232302";
        private string NumberToAuthenticate = "79257680013";
        private string apiHash = "e94f967ac95b8c0b616404688a872d6a";

        private int apiId = 41075;

        [TestMethod]
        public async Task TestCheckPhone()
        {
            var s = new Telegram.TelegramProvider();
            var d = await s.CheckPhones(NumberToSendMessage);
        }

        [TestMethod]
        public async Task TestMessage()
        {
            var s = new Telegram.TelegramProvider();
             await s.SendingMessage("ddd",string.Empty, NumberToSendMessage);

        }
    }
}
