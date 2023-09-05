using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epidi.Notifications.Interface
{
	public interface IMessages
	{
		void SendEmailViaSMTP(string receiverEmail, string subject, string body);
		Task<string> SendEmailViaSendGrid(string receiverEmail, string subject, string body);
	}
}
