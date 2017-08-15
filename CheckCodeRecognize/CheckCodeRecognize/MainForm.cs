using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace CheckCodeRecognize
{
    public partial class MainForm : Form
    {
        //学习库保存的路径
        private readonly string path;



        public MainForm()
        {
            InitializeComponent();
            txtFZ.Text = threshold.ToString();
            txtEZ.Text = ezFZ.ToString();
            txtBJFZ.Text = bjfz.ToString();
            path = AppDomain.CurrentDomain.BaseDirectory + "Sample\\";
        }

        //色差阀值 越小消除的杂色越多

        double threshold => String.IsNullOrWhiteSpace(txtFZ.Text) ? 150d : double.Parse(txtFZ.Text);


        //二值阀值 越大效果越不明显
        double ezFZ => String.IsNullOrWhiteSpace(txtEZ.Text) ? 0.6d : double.Parse(txtEZ.Text);


        //背景近似度阀值
        double bjfz => String.IsNullOrWhiteSpace(txtBJFZ.Text) ? 80d : double.Parse(txtBJFZ.Text);
        //图片路径
        private string imgPath = string.Empty;
        //每个字符最小宽度
        public int MinWidthPerChar = 12;
        //每个字符最大宽度
        public int MaxWidthPerChar = 28;
        //每个字符最小高度
        public int MinHeightPerChar = 12;
        private void btnRecognize_Click(object sender, EventArgs e)
        {
            try
            {
                //ezFZ = Convert.ToDouble(txtEZ.Text);
                //threshold = Convert.ToDouble(txtFZ.Text);
                //bjfz = Convert.ToDouble(txtBJFZ.Text);
            }
            catch (Exception ex)
            {
                txtFZ.Text = threshold.ToString();
                txtEZ.Text = ezFZ.ToString();
                txtBJFZ.Text = bjfz.ToString();
            }
        }

        /// <summary>
        /// 获取色差
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns></returns>
        private double GetColorDif(Color color1, Color color2)
        {
            return Math.Sqrt((Math.Pow((color1.R - color2.R), 2) +
                Math.Pow((color1.G - color2.G), 2) +
                Math.Pow((color1.B - color2.B), 2)));
        }

        //原始图像 重置时用
        private Bitmap oldBitMap;

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "正在获取验证码...";
            picbox.Image = new Bitmap(Image.FromFile("C:/Users/admin/Desktop/21.png"));
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.gjjx.com.cn/member/captcha/");
           // HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://wenshu.court.gov.cn/ValiCode/CreateCode/?guid=" + CreateGuid());
           // HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //using (var stream = response.GetResponseStream())
            //{
            //    toolStripStatusLabel1.Text = "获取验证码成功";
            //    Bitmap img = new Bitmap(stream);
            //    picbox.Image = img;
            //    oldBitMap = (Bitmap)img.Clone();
            //}
            //request.BeginGetResponse(new AsyncCallback(ResponseReady), request);
        }

        /// <summary>
        /// 异步响应验证码
        /// </summary>
        /// <param name="asyncResult"></param>
        private void ResponseReady(IAsyncResult asyncResult)
        {
            WebRequest request = asyncResult.AsyncState as WebRequest;
            WebResponse response = request.EndGetResponse(asyncResult);
            using (var stream = response.GetResponseStream())
            {
                toolStripStatusLabel1.Text = "获取验证码成功";
                Bitmap img = new Bitmap(stream);
                picbox.Image = img;
                oldBitMap = (Bitmap)img.Clone();
            }
        }

        //默认保存和选取文件路径
        string defaultDirectory = @"C:\temp\CheckCode\";

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (picbox.Image != null)
            {

                if (Directory.Exists(defaultDirectory) == false)
                {
                    Directory.CreateDirectory(defaultDirectory);
                }
                picbox.Image.Save(defaultDirectory + DateTime.Now.ToFileTime() + ".jpg");
            }
        }

        /// <summary>
        /// 二值化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEZ_Click(object sender, EventArgs e)
        {
            EZH();
        }

        /// <summary>
        /// 二值化 遍历每个点 点的亮度小于阀值 则认为是黑色 否则是白色
        /// </summary>
        private void EZH()
        {
            if (picbox.Image != null)
            {
                var img = new Bitmap(picbox.Image);
                for (var x = 0; x < img.Width; x++)
                {
                    for (var y = 0; y < img.Height; y++)
                    {
                        Color color = img.GetPixel(x, y);
                        if (color.GetBrightness() < ezFZ)
                        {
                            img.SetPixel(x, y, Color.Black);
                        }
                        else
                        {
                            img.SetPixel(x, y, Color.White);
                        }
                    }
                }
                picbox.Image = img;
                pbNormal.Image = picbox.Image;
            }
        }

        /// <summary>
        /// 去背景
        /// 把图片中最多的一部分颜色视为背景色 选出来后替换为白色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDropBG_Click(object sender, EventArgs e)
        {
            if (picbox.Image == null)
            {
                return;
            }
            Bitmap img = new Bitmap(picbox.Image);
            //key 颜色  value颜色对应的数量
            Dictionary<Color, int> colorDic = new Dictionary<Color, int>();
            //获取图片中每个颜色的数量
            for (var x = 0; x < img.Width; x++)
            {
                for (var y = 0; y < img.Height; y++)
                {
                    //删除边框
                    if (y == 0 || y == img.Height)
                    {
                        img.SetPixel(x, y, Color.White);
                    }

                    var color = img.GetPixel(x, y);
                    var colorRGB = color.ToArgb();

                    if (colorDic.ContainsKey(color))
                    {
                        colorDic[color] = colorDic[color] + 1;
                    }
                    else
                    {
                        colorDic[color] = 1;
                    }
                }
            }
            //图片中最多的颜色
            Color maxColor = colorDic.OrderByDescending(o => o.Value).FirstOrDefault().Key;
            //图片中最少的颜色
            Color minColor = colorDic.OrderBy(o => o.Value).FirstOrDefault().Key;

            Dictionary<int[], double> maxColorDifDic = new Dictionary<int[], double>();
            //查找 maxColor 最接近颜色
            for (var x = 0; x < img.Width; x++)
            {
                for (var y = 0; y < img.Height; y++)
                {
                    maxColorDifDic.Add(new int[] { x, y }, GetColorDif(img.GetPixel(x, y), maxColor));
                }
            }
            //去掉和maxColor接近的颜色 即 替换成白色
            var maxColorDifList = maxColorDifDic.OrderBy(o => o.Value).Where(o => o.Value < bjfz).ToArray();
            foreach (var kv in maxColorDifList)
            {
                img.SetPixel(kv.Key[0], kv.Key[1], Color.White);
            }
            picbox.Image = img;
            pbNormal.Image = picbox.Image;
        }

        /// <summary>
        /// 去干扰
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDropDisturb_Click(object sender, EventArgs e)
        {
            if (picbox.Image == null)
            {
                return;
            }
            Bitmap img = new Bitmap(picbox.Image);
            byte[] p = new byte[9]; //最小处理窗口3*3
            byte s;
            //去干扰线
            for (var x = 0; x < img.Width; x++)
            {
                for (var y = 0; y < img.Height; y++)
                {
                    Color currentColor = img.GetPixel(x, y);
                    int color = currentColor.ToArgb();

                    if (x > 0 && y > 0 && x < img.Width - 1 && y < img.Height - 1)
                    {
                        #region 中值滤波效果不好
                        ////取9个点的值
                        //p[0] = img.GetPixel(x - 1, y - 1).R;
                        //p[1] = img.GetPixel(x, y - 1).R;
                        //p[2] = img.GetPixel(x + 1, y - 1).R;
                        //p[3] = img.GetPixel(x - 1, y).R;
                        //p[4] = img.GetPixel(x, y).R;
                        //p[5] = img.GetPixel(x + 1, y).R;
                        //p[6] = img.GetPixel(x - 1, y + 1).R;
                        //p[7] = img.GetPixel(x, y + 1).R;
                        //p[8] = img.GetPixel(x + 1, y + 1).R;
                        ////计算中值
                        //for (int j = 0; j < 5; j++)
                        //{
                        //    for (int i = j + 1; i < 9; i++)
                        //    {
                        //        if (p[j] > p[i])
                        //        {
                        //            s = p[j];
                        //            p[j] = p[i];
                        //            p[i] = s;
                        //        }
                        //    }
                        //}
                        ////      if (img.GetPixel(x, y).R < dgGrayValue)
                        //img.SetPixel(x, y, Color.FromArgb(p[4], p[4], p[4]));    //给有效值付中值
                        #endregion

                        //上 x y+1
                        double upDif = GetColorDif(currentColor, img.GetPixel(x, y + 1));
                        //下 x y-1
                        double downDif = GetColorDif(currentColor, img.GetPixel(x, y - 1));
                        //左 x-1 y
                        double leftDif = GetColorDif(currentColor, img.GetPixel(x - 1, y));
                        //右 x+1 y
                        double rightDif = GetColorDif(currentColor, img.GetPixel(x + 1, y));
                        //左上
                        double upLeftDif = GetColorDif(currentColor, img.GetPixel(x - 1, y + 1));
                        //右上
                        double upRightDif = GetColorDif(currentColor, img.GetPixel(x + 1, y + 1));
                        //左下
                        double downLeftDif = GetColorDif(currentColor, img.GetPixel(x - 1, y - 1));
                        //右下
                        double downRightDif = GetColorDif(currentColor, img.GetPixel(x + 1, y - 1));

                        ////四面色差较大
                        //if (upDif > threshold && downDif > threshold && leftDif > threshold && rightDif > threshold)
                        //{
                        //    img.SetPixel(x, y, Color.White);
                        //}
                        //三面色差较大
                        if ((upDif > threshold && downDif > threshold && leftDif > threshold)
                            //|| (downDif > threshold && leftDif > threshold && rightDif > threshold)
                            || (upDif > threshold && leftDif > threshold && rightDif > threshold)
                            || (upDif > threshold && downDif > threshold && rightDif > threshold))
                        {
                            img.SetPixel(x, y, Color.White);
                        }

                        List<int[]> xLine = new List<int[]>();
                        //去横向干扰线  原理 如果这个点上下有很多白色像素则认为是干扰
                        for (var x1 = x + 1; x1 < x + 7; x1++)
                        {
                            if (x1 >= img.Width)
                            {
                                break;
                            }

                            if (img.GetPixel(x1, y + 1).ToArgb() == Color.White.ToArgb()
                                && img.GetPixel(x1, y - 1).ToArgb() == Color.White.ToArgb())
                            {
                                xLine.Add(new int[] { x1, y });
                            }
                        }
                        if (xLine.Count() >= 4)
                        {
                            foreach (var xpoint in xLine)
                            {
                                img.SetPixel(xpoint[0], xpoint[1], Color.White);
                            }
                        }

                        //去竖向干扰线

                    }
                }
            }
            picbox.Image = img;
            pbNormal.Image = picbox.Image;
        }


        private void btnReset_Click(object sender, EventArgs e)
        {
            if (oldBitMap != null)
            {
                picbox.Image = oldBitMap;
            }
        }

        private void WriteSampleFont(string c, Font font, string path)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            using (Bitmap img = new Bitmap(50, 50))
            {
                using (Graphics g = Graphics.FromImage(img))
                {
                    g.Clear(Color.White);
                    SolidBrush drawBrush = new SolidBrush(Color.Black);
                    //g.RotateTransform(10.0f);
                    g.DrawString(c.ToString(), font, drawBrush, 0, 0);
                    string filePath = path + c + ".jpg";
                    //必须这样保存 否则会报GDI+一般错误
                    Bitmap img2 = (Bitmap)img.Clone();
                    img2.Save(filePath, ImageFormat.Jpeg);
                    img2.Dispose();
                }
            }
        }



        /// <summary>
        /// 先竖向分割，再横向分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (picbox.Image == null)
            {
                return;
            }
            Bitmap img = new Bitmap(picbox.Image);
            List<int[]> xCutPointList = GetXCutPointList(img);
            List<int[]> yCutPointList = GetYCutPointList(xCutPointList, img);

            PictureBox[] pbList = { pbSplit1, pbSplit2, pbSplit3, pbSplit4, pbSplit5, pbSplit6 };
            foreach (var pb in pbList)
            {
                if (pb.Image != null)
                {
                    pb.Image.Dispose();
                    pb.Image = null;
                }
            }
            //对分割的部分划线
            for (int i = 0; i < xCutPointList.Count(); i++)
            {
                int xStart = xCutPointList[i][0];
                int xEnd = xCutPointList[i][1];
                int yStart = yCutPointList[i][0];
                int yEnd = yCutPointList[i][1];
                //DrawLineForChar(xStart, xEnd, yStart, yEnd, img);
                if (i >= 6) break;
                pbList[i].Image = (Bitmap)AcquireRectangleImage(img,
                    new Rectangle(xStart, yStart, xEnd - xStart + 1, yEnd - yStart + 1));
            }
            picbox.Image = img;
        }

        /// <summary>
        /// 给每个字符画边框
        /// </summary>
        /// <param name="xStart"></param>
        /// <param name="xEnd"></param>
        /// <param name="yStart"></param>
        /// <param name="yEnd"></param>
        /// <param name="img"></param>
        private void DrawLineForChar(int xStart, int xEnd, int yStart, int yEnd, Bitmap img)
        {
            for (var x = xStart; x <= xEnd; x++)
            {
                //上
                img.SetPixel(x, yStart, Color.Red);
                //下
                img.SetPixel(x, yEnd, Color.Red);
            }
            for (var y = yStart; y <= yEnd; y++)
            {
                //左
                img.SetPixel(xStart, y, Color.Red);
                //右
                img.SetPixel(xEnd, y, Color.Red);
            }
        }

        /// <summary>
        /// 根据XCutPointList获取每个字符左上、右下坐标用于剪切
        /// 1、从中间查找 上下交替 找到有两行空白为止
        /// </summary>
        /// <param name="xCutPointList">List int[xstart xend]</param>
        /// <returns>每个字符上、下坐标 List int[ystart yend]</returns>
        private List<int[]> GetYCutPointList(List<int[]> xCutPointList, Bitmap img)
        {
            List<int[]> list = new List<int[]>();
            int yMiddle = img.Height / 2;
            //某区域内黑色像素最小阀值
            int minBlackPXInY = 1;
            //y开始区域黑色像素数量
            int yStartBlackPointCount = 100;
            //y结束区域黑色像素数量
            int yEndBlackPointCount = 100;
            int yStart = 0;//y开始坐标
            int yEnd = 0;//y结束坐标

            foreach (var xPoint in xCutPointList)
            {
                //重置为100否则出错
                yStartBlackPointCount = 100;
                yEndBlackPointCount = 100;
                //从中间往两边偏移 直到像素小于阀值
                for (var i = 0; i < yMiddle; i++)
                {
                    if (yStartBlackPointCount > minBlackPXInY)
                    {
                        yStart = yMiddle - i;
                        yStartBlackPointCount = GetBlackPXCountInY(yStart, -2, xPoint[0], xPoint[1], img);
                    }
                    if (yEndBlackPointCount > minBlackPXInY || yEnd - yStart < MinHeightPerChar)
                    {
                        yEnd = yMiddle + i;
                        yEndBlackPointCount = GetBlackPXCountInY(yEnd, 2, xPoint[0], xPoint[1], img);
                    }
                    if (yStartBlackPointCount <= minBlackPXInY
                        && yEndBlackPointCount <= minBlackPXInY
                        && yEnd - yStart >= MinHeightPerChar)
                    {
                        break;
                    }
                }

                list.Add(new int[] { yStart, yEnd });
            }
            return list;
        }

        /// <summary>
        /// 获取分割后某区域的黑色像素
        /// </summary>
        /// <param name="startY"></param>
        /// <param name="offset"></param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        private int GetBlackPXCountInY(int startY, int offset, int startX, int endX, Bitmap img)
        {
            int blackPXCount = 0;
            int startY1 = offset > 0 ? startY : startY + offset;
            int offset1 = offset > 0 ? startY + offset : startY;
            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY1; y < offset1; y++)
                {
                    if (y >= img.Height)
                    {
                        continue;
                    }
                    if (img.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                    {
                        blackPXCount++;
                    }
                }
            }
            return blackPXCount;
        }

        /// <summary>
        /// 获取一个垂直区域内的黑色像素
        /// </summary>
        /// <param name="startX">开始x</param>
        /// <param name="offset">左偏移像素</param>
        /// <returns></returns>
        private int GetBlackPXCountInX(int startX, int offset, Bitmap img)
        {
            int blackPXCount = 0;
            for (int x = startX; x < startX + offset; x++)
            {
                if (x >= img.Width)
                {
                    continue;
                }
                for (var y = 0; y < img.Height; y++)
                {
                    if (img.GetPixel(x, y).ToArgb() == Color.Black.ToArgb())
                    {
                        blackPXCount++;
                    }
                }
            }
            return blackPXCount;
        }

        /// <summary>
        /// 获取竖向分割点
        /// </summary>
        /// <param name="img"></param>
        /// <returns>List int[xstart xend]</returns>
        private List<int[]> GetXCutPointList(Bitmap img)
        {
            //分割点  List<int[xstart xend]>
            List<int[]> xCutList = new List<int[]>();
            int startX = -1;//-1表示在寻找开始节点
            for (var x = 0; x < img.Width; x++)
            {
                if (startX == -1)//开始点
                {
                    int blackPXCount = GetBlackPXCountInX(x, 3, img);
                    //如果大于有效像素则是开始节点
                    if (blackPXCount > 6)
                    {
                        startX = x;
                    }
                }
                else//结束点
                {
                    if (x == img.Width - 1)//判断是否最后一列
                    {
                        xCutList.Add(new int[] { startX, x });
                        break;
                    }
                    else if (x >= startX + MinWidthPerChar)//隔开一定距离才能结束分割
                    {
                        int blackPXCount = GetBlackPXCountInX(x, 3, img);
                        //小于等于阀值则是结束节点
                        if (blackPXCount <= 2)
                        {
                            if (x > startX + MaxWidthPerChar)
                            {
                                //大于最大字符的宽度应该是两个字符粘连到一块了 从中间分开
                                int middleX = startX + (x - startX) / 2;
                                xCutList.Add(new int[] { startX, middleX });
                                xCutList.Add(new int[] { middleX + 1, x });
                            }
                            else
                            {
                                //验证黑色像素是否太少
                                blackPXCount = GetBlackPXCountInX(startX, x - startX, img);
                                if (blackPXCount <= 10)
                                {
                                    startX = -1;
                                }
                                else
                                {
                                    xCutList.Add(new int[] { startX, x });
                                }
                            }
                            startX = -1;
                        }
                    }
                }
            }
            return xCutList;
        }

        /// <summary>
        /// 截取图像的矩形区域
        /// </summary>
        /// <param name="source">源图像对应picturebox1</param>
        /// <param name="rect">矩形区域，如上初始化的rect</param>
        /// <returns>矩形区域的图像</returns>
        public static Image AcquireRectangleImage(Image source, Rectangle rect)
        {
            if (source == null || rect.IsEmpty) return null;
            //Bitmap bmSmall = new Bitmap(rect.Width, rect.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Bitmap bmSmall = new Bitmap(rect.Width, rect.Height, source.PixelFormat);

            using (Graphics grSmall = Graphics.FromImage(bmSmall))
            {
                grSmall.DrawImage(source,
                                  new System.Drawing.Rectangle(0, 0, bmSmall.Width, bmSmall.Height),
                                  rect,
                                  GraphicsUnit.Pixel);
                grSmall.Dispose();
            }
            return bmSmall;
        }

        private void btnStudy_Click(object sender, EventArgs e)
        {
            PictureBox[] pbControlList = { pbSplit1, pbSplit2, pbSplit3, pbSplit4, pbSplit5, pbSplit6 };
            TextBox[] txtControlList = { txtC1, txtC2, txtC3, txtC4, txtC5, txtC6 };

            for (int i = 0; i < pbControlList.Length; i++)
            {
                var img = pbControlList[i].Image;
                var txt = txtControlList[i].Text;
                if (img != null && string.IsNullOrEmpty(txt) == false)
                {
                    string detailPath = path + txt.ToUpper() + "/";
                    if (Directory.Exists(detailPath) == false)
                    {
                        Directory.CreateDirectory(detailPath);
                    }
                    int fileName = 0;
                    string[] fileNameList = Directory.GetFiles(detailPath);
                    if (fileNameList != null && fileNameList.Length > 0)
                    {
                        fileName = fileNameList.Select(o => Convert.ToInt32(Path.GetFileNameWithoutExtension(o))).OrderBy(n => n).Last() + 1;
                    }
                    string filePath = detailPath + fileName + ".jpg";
                    img.Save(filePath);
                }
            }
            for (int i = 0; i < txtControlList.Length; i++)
            {
                txtControlList[i].Text = string.Empty;
            }
        }

        /// <summary>
        /// 用所有的学习的图片对比当前图片
        /// 最后取黑色重叠区域最多的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecognize_Click_1(object sender, EventArgs e)
        {
            lblResult.Text = string.Empty;
            PictureBox[] pbControlList = { pbSplit1, pbSplit2, pbSplit3, pbSplit4, pbSplit5, pbSplit6 };
            for (int i = 0; i < pbControlList.Length; i++)
            {
                if (pbControlList[i].Image == null)
                {
                    continue;
                }
                var img = new Bitmap(pbControlList[i].Image);
                if (img == null)
                {
                    continue;
                }
                string[] detailPathList = Directory.GetDirectories(path);
                if (detailPathList == null || detailPathList.Length == 0)
                {
                    continue;
                }
                string resultString = string.Empty;
                //config.txt 文件中指定了识别字母的顺序
                string configPath = path + "config.txt";
                if (!File.Exists(configPath))
                {
                    MessageBox.Show("config.txt文件不存在");
                    return;
                }
                string configString = File.ReadAllText(configPath);
                double maxRate = 0;//相似度  最大1
                foreach (char resultChar in configString)
                {
                    string charPath = path + resultChar.ToString();
                    if (!Directory.Exists(charPath))
                    {
                        continue;
                    }
                    string[] fileNameList = Directory.GetFiles(charPath);
                    if (fileNameList == null || fileNameList.Length == 0)
                    {
                        continue;
                    }


                    foreach (string filename in fileNameList)
                    {
                        Bitmap imgSample = new Bitmap(filename);
                        //过滤宽高相差太大的
                        if (Math.Abs(imgSample.Width - img.Width) >= 6
                            || Math.Abs(imgSample.Height - img.Height) >= 6)
                        {
                            continue;
                        }
                        //当前相似度
                        double currentRate = CompareImg(imgSample, img);
                        if (currentRate > maxRate)
                        {
                            maxRate = currentRate;
                            resultString = resultChar.ToString();
                        }
                        imgSample.Dispose();
                    }
                }
                lblResult.Text = lblResult.Text + resultString;
            }
        }

        /// <summary>
        /// 返回两图比较的相似度 最大1
        /// </summary>
        /// <param name="compareImg">对比图</param>
        /// <param name="mainImg">要识别的图</param>
        /// <returns></returns>
        public double CompareImg(Bitmap compareImg, Bitmap mainImg)
        {
            int img1x = compareImg.Width;
            int img1y = compareImg.Height;
            int img2x = mainImg.Width;
            int img2y = mainImg.Height;
            //最小宽度
            double min_x = img1x > img2x ? img2x : img1x;
            //最小高度
            double min_y = img1y > img2y ? img2y : img1y;

            double score = 0;
            //重叠的黑色像素
            for (var x = 0; x < min_x; x++)
            {
                for (var y = 0; y < min_y; y++)
                {
                    if (compareImg.GetPixel(x, y).ToArgb() == Color.Black.ToArgb()
                        && compareImg.GetPixel(x, y).ToArgb() == mainImg.GetPixel(x, y).ToArgb())
                    {
                        score++;
                    }
                }
            }
            double originalBlackCount = 0;
            //对比图片的黑色像素
            for (var x = 0; x < img1x; x++)
            {
                for (var y = 0; y < img1y; y++)
                {
                    if (Color.Black.ToArgb() == compareImg.GetPixel(x, y).ToArgb())
                    {
                        originalBlackCount++;
                    }
                }
            }
            return score / originalBlackCount;
        }


        private string CreateGuid()
        {
            //function createGuid()
            //{
            //    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
            //}
            string str = "";
            Random rd = new Random();
            for (int i = 0; i < 4; i++)
            {
                double s = (1 + (rd.Next(10000000)) / 10000000.0) * 0x10000;
                str += ((int)Math.Round(s)).ToString("X").Substring(1) + "-";
            }
            return str.TrimEnd('-');
        }

    }
}
