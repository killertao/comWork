using System;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.IO;
using System.Text;

namespace Org.Mentalis.Proxy
{
	/// <summary>
	/// Common 的摘要说明。
	/// </summary>
	public class Common
	{
		public Common()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		
		public static IPAddress GetRandLocalIP() 
		{
			try 
			{
				IPHostEntry he = Dns.Resolve(Dns.GetHostName());
				int iIndext = GetRandomNumber(0,he.AddressList.Length); 
				return he.AddressList[iIndext]; 
			} 
			catch 
			{
				return IPAddress.Any;
			}
		}

		public static int GetRandomNumber(int min,int max)
		{
			Random ro = new Random();
			return ro.Next (min,max);
		}


		public static void log(string errMsg,string logFileName)
		{

		 
			string logPath=ConfigurationSettings.AppSettings["LogPath"];
			System.DateTime d=System.DateTime.Now;
			string filename=logPath + logFileName + d.Date.ToShortDateString () + ".txt";
			
			try 
			{
				StreamWriter  din ; 
				if(File.Exists(filename))
					din = new StreamWriter( File.Open(filename, FileMode.Append ),Encoding.GetEncoding (936) );	
				else
					din = new StreamWriter( File.Open(filename, FileMode.Create  ),Encoding.GetEncoding (936) );	

				din.Write  (errMsg+"\r\n") ;
				din.Close();
			}
			catch 
			{
				;
			}
		 
		
		}



	}
}
