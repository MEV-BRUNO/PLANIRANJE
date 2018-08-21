using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Planiranje.Controllers
{
	public class PlaniranjeSession
	{
		public int PedagogId { get; set; }
		public static PlaniranjeSession Trenutni
		{
			get
			{
				PlaniranjeSession session = (PlaniranjeSession)HttpContext.Current.Session["id_pedagog"];
				HttpContext.Current.Session.Timeout = 1440;
				if (session == null)
				{
					session = new PlaniranjeSession();
					HttpContext.Current.Session["id_pedagog"] = session;
				}
				return session;
			}
		}
	}
}