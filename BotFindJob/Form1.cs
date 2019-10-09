using MihaZupan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;

namespace BotFindJob
{
	public partial class Form1 : Form
	{
		BackgroundWorker bw;
		public Form1()
		{
			InitializeComponent();

			this.bw = new BackgroundWorker();
			this.bw.DoWork += this.bw_DoWork;
		}
		async void bw_DoWork(object sender, DoWorkEventArgs e)
		{
			var worker = sender as BackgroundWorker;
			var key = e.Argument as string; // получаем ключ из аргументов
			try
			{
				var proxy = new HttpToSocks5Proxy("207.180.238.12", 1080);
				var Bot = new TelegramBotClient(key, proxy);
				await Bot.SetWebhookAsync("");
				int offset = 0;
				while(true)
				{
					var updates = await Bot.GetUpdatesAsync(offset);//получаем массив обновлений
					foreach(var update in updates)
					{
						//Console.WriteLine(update.Type);
						var message = update.Message;
						if(message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
						{
							if(message.Text == "/saysomething")
							{
								await Bot.SendTextMessageAsync(message.Chat.Id, "test", replyToMessageId: message.MessageId);
							}
						}
						offset = update.Id + 1;
					}
				}
			}
			catch(Telegram.Bot.Exceptions.ApiRequestException ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		void BtnRunClick(object sender, EventArgs e)
		{
			var text =; // сюда писать токен
			if(!this.bw.IsBusy)//если не запущен
			{
				this.bw.RunWorkerAsync(text);//запускаем
			}
		}
		private void Form1_Load(object sender, EventArgs e)
		{

		}
	}
}
