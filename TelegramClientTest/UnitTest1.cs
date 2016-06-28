using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TLSharp.Core;
using System.Threading.Tasks;
using TLSharp.Core.Network;
using TLSharp.Core.Auth;
using System.IO;

namespace TelegramClientTest
{
    [TestClass]
    public class UnitTest1
    {
        private string UserNameToSendMessage { get; set; }
        private string NumberToSendMessage = "з"; 
        private string NumberToAuthenticate = "з";
        private string apiHash = "з";
     
        private int apiId = 41075;

        [TestMethod]
        public async Task TestMethod1()
        {

            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);  //завести аккаунт https://my.telegram.org/auth взять API_ID(48047) и API_HASH

            await client.Connect();

            var hash = await client.SendCodeRequest(NumberToAuthenticate); //номер телефона с которого отправляем сообщение

            var code = "43441"; // вставляем сюда проверочный код пришедший на телефон

            var user = await client.MakeAuth(NumberToAuthenticate, hash, code); // авторизируемся

            var res = await client.ImportContactByPhoneNumber(NumberToAuthenticate); //записываем номер на который отправляем сообщение

            await client.SendMessage(res.Value, @"проверка"); //отправляем сообщение
        }

        [TestMethod]  /// отправка сообщения
        public async Task SendingMessage()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);
            
            await client.Connect();

            Assert.IsTrue(client.IsUserAuthorized());

            var res = await client.ImportContactByPhoneNumber(NumberToSendMessage);

            Assert.IsNotNull(res);
            var sms = "Привет";
            await client.SendMessage(res.Value, sms);
        }


        [TestMethod] /// проверка подключения к серверам
        public async Task AuthenticationWorks()
        {
            using (var transport = new TcpTransport("149.154.167.51", 443))
            {
                var authKey = await Authenticator.DoAuthentication(transport);

                Assert.IsNotNull(authKey.AuthKey.Data);
            }
        }

        [TestMethod] /// полчаем последние 5 сообщений
        public async Task GetHistory()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);
            await client.Connect();

            Assert.IsTrue(client.IsUserAuthorized());

            var res = await client.ImportContactByPhoneNumber(NumberToSendMessage);

            Assert.IsNotNull(res);

            var hist = await client.GetMessagesHistoryForContact(res.Value, 0, 5);

            Assert.IsNotNull(hist);
        }

        [TestMethod] /// отправка сообщения с фото
        public async Task UploadAndSendMedia()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);
            await client.Connect();

            Assert.IsTrue(client.IsUserAuthorized());

            var res = await client.ImportContactByPhoneNumber(NumberToSendMessage);

            Assert.IsNotNull(res);

            var file = File.ReadAllBytes(@"C:\Users\Admin\Downloads\pikachu.png");

            var mediaFile = await client.UploadFile("pikachu.png", file);

            Assert.IsNotNull(mediaFile);

            var state = await client.SendMediaMessage(res.Value, mediaFile);

            Assert.IsTrue(state);
        }

        [TestMethod] /// Проверьте телефоны
        public async Task CheckPhones()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);
            await client.Connect();

            var result = await client.IsPhoneRegistered(NumberToSendMessage);
            Assert.IsTrue(result);
        }

    }
}
