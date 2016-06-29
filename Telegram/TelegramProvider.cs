using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TLSharp.Core;

namespace Telegram
{
    public class TelegramProvider
    {
        private string UserNameToSendMessage { get; set; }
        private string NumberToSendMessage = "89151232302";
        private string NumberToAuthenticate = "79257680013";
        private string apiHash = "e94f967ac95b8c0b616404688a872d6a";

        private int apiId = 41075;


        public async Task AuthTelegram()
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);  //завести аккаунт https://my.telegram.org/auth взять API_ID(48047) и API_HASH

            await client.Connect();

            var hash = await client.SendCodeRequest(NumberToAuthenticate); //номер телефона с которого отправляем сообщение

            var code = "43441"; // вставляем сюда проверочный код пришедший на телефон (после активации не используем)
            /// ВАЖНО в папке Debug создается файл session.dat он то и нужен для дальнейшей рассылки

            var user = await client.MakeAuth(NumberToAuthenticate, hash, code); // авторизируемся

            var res = await client.ImportContactByPhoneNumber(NumberToAuthenticate); //записываем номер на который отправляем сообщение

            await client.SendMessage(res.Value, @"start"); //отправляем сообщение
        }

        /// <summary>
        /// Отправка сообщений
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="media">файл для отправки</param>
        /// <returns></returns>
        public async Task SendingMessage(string message, string media, string NumberToSend)
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);

            await client.Connect();
            var res = await client.ImportContactByPhoneNumber(NumberToSend);

            if (message != string.Empty)
            {
                await client.SendMessage(res.Value, message);
            }
            if (media != string.Empty)
            {
                var mediaFile = await client.UploadFile("picture.png", File.ReadAllBytes(media));
                var state = await client.SendMediaMessage(res.Value, mediaFile);
            }
        }


        /// <summary>
        /// Проверка на наличие номера в Telegram
        /// </summary>
        /// <param name="NumberToSend"></param>
        /// <returns></returns>
        public async Task<bool> CheckPhones(string NumberToSend)
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", apiId, apiHash);
            await client.Connect();

            var result = await client.IsPhoneRegistered(NumberToSend);

            return result;
        }


    }
}
