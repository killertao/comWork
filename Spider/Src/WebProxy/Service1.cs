using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Threading;
using Org.Mentalis.Proxy;

namespace ProxyServer
{
	public class Service1 : System.ServiceProcess.ServiceBase
	{

		/// <summary> 
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;
		public Thread threadGo; 
		public Proxy prx ;
		public Service1()
		{
			// 该调用是 Windows.Forms 组件设计器所必需的。
			InitializeComponent();

			// TODO: 在 InitComponent 调用后添加任何初始化
		}

		// 进程的主入口点
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// 同一进程中可以运行多个用户服务。若要将
			//另一个服务添加到此进程，请更改下行
			// 以创建另一个服务对象。例如，
			//
			//   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new Service1() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}


		#region 组件设计器生成的代码
		/// <summary> 
		/// 设计器支持所需的方法 - 不要使用代码编辑器 
		/// 修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// Service1
			// 
			this.ServiceName = "2SP_ProxyService";

		}
		#endregion

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing ) 
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// 设置具体的操作，以便服务可以执行它的工作。
		/// </summary>
		protected override void OnStart(string[] args)
		{
			try 
			{
				string dir =  System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;//Environment.CurrentDirectory;
				dir =  dir.Substring(0,dir.LastIndexOf("\\"));
				if (!dir.Substring(dir.Length - 1, 1).Equals(@"\"))
					dir += @"\";
				prx = new Proxy(dir + "config.xml");

				threadGo = new Thread(new ThreadStart(prx.Start));
				threadGo.Start();

				 
			} 
			catch 
			{
				Console.WriteLine("The program ended abnormally!");
			}

			
 
			
		}
 
		/// <summary>
		/// 停止此服务。
		/// </summary>
		protected override void OnStop()
		{
			try
			{
				prx.Stop();

				

			}
			catch 
			{
				Console.WriteLine("The program Stop error!");
			}


			if(threadGo != null)
				threadGo.Abort();
			
 
		}
	}
}
