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
		/// ����������������
		/// </summary>
		private System.ComponentModel.Container components = null;
		public Thread threadGo; 
		public Proxy prx ;
		public Service1()
		{
			// �õ����� Windows.Forms ��������������ġ�
			InitializeComponent();

			// TODO: �� InitComponent ���ú�����κγ�ʼ��
		}

		// ���̵�����ڵ�
		static void Main()
		{
			System.ServiceProcess.ServiceBase[] ServicesToRun;
	
			// ͬһ�����п������ж���û�������Ҫ��
			//��һ��������ӵ��˽��̣����������
			// �Դ�����һ������������磬
			//
			//   ServicesToRun = New System.ServiceProcess.ServiceBase[] {new Service1(), new MySecondUserService()};
			//
			ServicesToRun = new System.ServiceProcess.ServiceBase[] { new Service1() };

			System.ServiceProcess.ServiceBase.Run(ServicesToRun);
		}


		#region �����������ɵĴ���
		/// <summary> 
		/// �����֧������ķ��� - ��Ҫʹ�ô���༭�� 
		/// �޸Ĵ˷��������ݡ�
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
		/// ������������ʹ�õ���Դ��
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
		/// ���þ���Ĳ������Ա�������ִ�����Ĺ�����
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
		/// ֹͣ�˷���
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
