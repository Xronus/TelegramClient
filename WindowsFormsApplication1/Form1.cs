using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TLSharp.Core;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    { 
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var store = new FileSessionStore();
            var client = new TelegramClient(store, "session", 48047, "d966777e04c48a1747cccb7714cb66a9"); //завести аккаунт https://my.telegram.org/auth взять API_ID(48047) и API_HASH

            await client.Connect();

            var hash = await client.SendCodeRequest("79162202111"); //номер телефона с которого отправляем сообщение

            var code = "71938"; // вставляем сюда проверочный код пришедший на телефон

            var user = await client.MakeAuth("79162202111", hash, code); // авторизируемся

            var res = await client.ImportContactByPhoneNumber("79151232302"); //записываем номер на который отправляем сообщение

            await client.SendMessage(res.Value, "Test message from TelegramClient"); //отправляем сообщение

        }
    }
}
