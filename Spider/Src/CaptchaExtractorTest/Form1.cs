using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using CaptchaExtractor;
using System.Web;
using System.Net;
using System.Threading;
using CaptchaExtractorTest;

//|5|1|a|s|p|x
namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog newf = new OpenFileDialog();
            if (newf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap newM = null;
                try
                {
                    newM = new Bitmap(newf.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);   
                    //throw;
                }
                if (newM != null)
                {
                    SourcePic.Image = newM;
                }

            }
        }

        /// <summary>
        /// 识别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (SourcePic.Image != null)
            {
                int ColorOffset = 5;
                int SplitColorNum = 150;
                try
                {
                    ColorOffset = int.Parse(textBox2.Text);                    
                }
                catch (Exception)
                {                                       
                }
                try
                {
                    SplitColorNum = int.Parse(textBox3.Text);
                }
                catch (Exception)
                {
                }
                var result = Extractor.Run(new Bitmap(SourcePic.Image),ColorOffset,SplitColorNum , "");
                textBox1.Text = result.Text;
                //图片
                if (result.LessColorbit!= null)
                {
                    ResultPic.Visible = true;
                    ResultPic.Image = result.LessColorbit;
                }
                else
                {
                    ResultPic.Visible = false;
                    ResultPic.Image = null;
                }

                //图片1
                if (result.GrayscaleBit != null)
                {
                    ResultPic1.Visible = true;
                    ResultPic1.Image = result.GrayscaleBit;
                }
                else
                {
                    ResultPic1.Visible = false;
                    ResultPic1.Image = null;
                }
                //图片2
                if (result.LessColorbit != null)
                {
                    ResultPic2.Visible = true;
                    ResultPic2.Image = result.LessColorbit;
                }
                else
                {
                    ResultPic2.Visible = false;
                    ResultPic2.Image = null;
                }
                //图片3
                if (result.Matchbit != null)
                {
                    ResultPic3.Visible = true;
                    ResultPic3.Image = result.Matchbit;
                }
                else
                {
                    ResultPic3.Visible = false;
                    ResultPic3.Image = null;
                }
                //图片4
                if (result.Filterbit != null)
                {
                    ResultPic4.Visible = true;
                    ResultPic4.Image = result.Filterbit;
                }
                else
                {
                    ResultPic4.Visible = false;
                    ResultPic4.Image = null;
                }
                //图片5
                buildPic(result.ResolveImages);
                //图片6
                if (result.Findbit != null)
                {
                    ResultPic5.Visible = true;
                    ResultPic5.Image = result.Findbit;
                }
                else
                {
                    ResultPic5.Visible = false;
                    ResultPic5.Image = null;
                }
            }
        }


        private void buildPic(List<Bitmap> value)
        {
            if (panel1.Controls.Count > 0)
            {
                panel1.Controls.Clear();
            }
                int ControlHeight = 0;
                foreach (Bitmap tmpB in value)
                {
                    if (tmpB != null)
                    {
                        PictureBox newP = new PictureBox();
                        newP.Image = tmpB;
                        newP.Width = tmpB.Width;
                        newP.Height = tmpB.Height;
                        newP.Location = new Point(0, ControlHeight);
                        panel1.Controls.Add(newP);
                        ControlHeight += newP.Height + 1;
                    }
                }
            
        }
        /// <summary>
        /// 学习
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            //这个是学习
            if (SourcePic.Image != null)
            {
                int ColorOffset = 5;
                int SplitColorNum = 150;
               
                Extractor.Run(new Bitmap(SourcePic.Image), ColorOffset, SplitColorNum,textBox4.Text );
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (text_Url.Text.Length > 0)
                {
                    //不验证Url地址是否正确
                    HttpWebRequest newR = (HttpWebRequest)HttpWebRequest.Create(text_Url.Text );
                    HttpWebResponse newRes = (HttpWebResponse)newR.GetResponse();
                    Stream newS = newRes.GetResponseStream();
                    MemoryStream newMs = new MemoryStream();
                    byte[] tmpb = null;
                    while (true)
                    {
                        tmpb = new byte[1024];
                        int ReNum = newS.Read(tmpb, 0, 1024);
                        if (ReNum > 0)
                        {
                            newMs.Write(tmpb, 0, ReNum);
                        }
                        else
                            break;
                        Thread.Sleep(1);
                    }
                    try
                    {
                        Bitmap ddd = new Bitmap(newMs);
                        SourcePic.Image = ddd;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        newRes.Close();
                        newS.Close();
                    }
                }
                else
                    throw new Exception("请输入验证码的Url地址");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
