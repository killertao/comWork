using System;
using System.Collections.Generic;
using System.Drawing ;
using System.Drawing.Imaging;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace CaptchaExtractor
{
    /// <summary>
    /// 解析器
    /// </summary>
    public static class Extractor
    {
        public static string Run(Bitmap value)
        {
            const int colorOffset = 5;
            const int splitColorNum = 150;
            return Run(value, colorOffset, splitColorNum, "").Text;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="value">图片内容</param>
        /// <param name="colorOffset">颜色偏移量</param>
        /// <param name="splitColorNum"></param>
        /// <param name="learnText">学习模式下图片内容的实际的值，非学习模式下为空</param>
        public static VerificationCodeResult Run(Bitmap value, int colorOffset, int splitColorNum, string learnText)
        {
            /*方法
             * 1、灰度
             * 2、降色
             * 3、计算
             * 3、滤色
             * 5、匹配
            */

            #region 第一步，灰度

            var grayscaleBit = new Bitmap(value.Width, value.Height);
            grayscaleBit.SetResolution(value.HorizontalResolution, value.VerticalResolution);

            var grayG = Graphics.FromImage(grayscaleBit);
            //var sp1 = new Point(0, 0);
            //var sp2 = new Point(value.Width, 0);
            //var sp3 = new Point(0, value.Height);

            var newia = new ImageAttributes();
            var colorMatrix = new ColorMatrix(
                new[]
                {
                    new[] {.3f, .3f, .3f, 0, 0},
                    new[] {.59f, .59f, .59f, 0, 0},
                    new[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
            newia.SetColorMatrix(colorMatrix);
            grayG.DrawImage(value, new Rectangle(0, 0, value.Width, value.Height), 0, 0, value.Width, value.Height, GraphicsUnit.Pixel, newia);

            #endregion

            //第三步的计算的颜色统计
            //Dictionary<int, int> ColorCollection = new Dictionary<int, int>();
            var colorList = new List<int>();

            #region 第二步，降色，并丢色

            var lessColorbit = new Bitmap(grayscaleBit);
            //var colorNum = 50;
            for (var i = 0; i < lessColorbit.Width; i++)
            {
                for (var j = 0; j < lessColorbit.Height; j++)
                {
                    //降色了
                    var currColor = lessColorbit.GetPixel(i, j);
                    //直接丢色

                    #region 丢色
                    var colorR = currColor.R > splitColorNum ? 255 : 0;
                    currColor = Color.FromArgb(255, colorR, colorR, colorR);
                    lessColorbit.SetPixel(i, j, currColor);
                    #endregion

                    #region 降色

                    //if (CurrColor.R % ColorNum  != 0)
                    //{
                    //    //不是5的余数时
                    //    ColorR = CurrColor.R / ColorNum  * ColorNum ;
                    //    CurrColor = Color.FromArgb(255, ColorR, ColorR, ColorR);
                    //    LessColorbit.SetPixel(i, j, CurrColor );
                    //}

                    #endregion

                    if (currColor.R != 255 && !colorList.Exists(c => c == currColor.R))
                    {
                        colorList.Add(currColor.R);
                    }
                    //if (ColorCollection.ContainsKey(CurrColor.R))
                    //{
                    //    ColorCollection[CurrColor.R] += 1;
                    //}
                    //else
                    //{
                    //    ColorCollection.Add(CurrColor.R, 1);
                    //}
                }
            }

            #endregion

            #region 第三步，计算

            //主要是为了计算深色           

            #endregion

            #region 滤色

            //颜色从深到浅，从左到右，逐步匹配
            //List<int> ColorList = from ss in ColorCollection 
            //                      select ss.Key 
            colorList.Sort();
            //存储被匹配掉之后的图片
            var matchbit = new Bitmap(lessColorbit);
            //存储被匹配出来的图片
            var filterbit = new Bitmap(lessColorbit);
            //var matchG = Graphics.FromImage(matchbit);

            var filterG = Graphics.FromImage(filterbit);
            filterG.Clear(Color.FromArgb(255, 255, 255, 255));


            // Bitmap FindImage = new Bitmap (5,5);
            // FindImage.SetResolution (72,72);
            // Graphics FindG = Graphics.FromImage (FindImage );
            // FindG.Clear (Color.FromArgb (255,188,167,20));
            //MatchG.Clear(Color.FromArgb(0, 0, 0, 0));
            //颜色偏移量
            var offsetValue = colorOffset;

            //现在只有两个颜色了，不需要用Dic来进行存储了
            //Dictionary<Point, List<Point>> FilterPointCollection = new Dictionary<Point, List<Point>>();
            var originalPoint = new List<List<Point>>();
            var dynamicPoint = new List<List<Point>>();
            for (var i = 0; i < colorList.Count; i++)
            {
                var matchColor = colorList[i];
                var isError = false;
                var lostNum = -999;
                //遍历颜色，从深往浅
                var topLeftP = new Point(lessColorbit.Width, lessColorbit.Height);
                var bottomRightP = new Point(0, 0);

                var findPoint = new List<Point>();
                //从左往右寻找 
                for (var x = 0; x < lessColorbit.Width; x++)
                {
                    var isFind = false;
                    for (var y = 0; y < lessColorbit.Height; y++)
                    {
                        if ((x == 37 || x == 36) &&
                            (y == 9 || y == 8))
                        {

                        }
                        //好了
                        if (ContrastColor(lessColorbit.GetPixel(x, y).R, matchColor, offsetValue))
                        {
                            if (x < topLeftP.X)
                                topLeftP.X = x;
                            if (y < topLeftP.Y)
                                topLeftP.Y = y;
                            if (x > bottomRightP.X)
                                bottomRightP.X = x;
                            if (y > bottomRightP.Y)
                                bottomRightP.Y = y;
                            findPoint.Add(new Point(x, y));
                            isFind = true;
                        }
                    }
                    if (isFind)
                    {
                        if (lostNum == -999)
                            lostNum = 0;
                        //如果太宽，则错了
                        if (((bottomRightP.X - topLeftP.X) > lessColorbit.Width/4) ||
                            ((bottomRightP.Y - topLeftP.Y) > lessColorbit.Height*0.8))
                        {
                            isError = true;
                            break;
                        }
                    }
                    else
                    {
                        lostNum = 1;
                    }
                    if (lostNum == 1)
                    {
                        //如果太窄，则错了 
                        if (bottomRightP.X - topLeftP.X >= 6)
                        {

                            //这里说明，矩阵存在，并且当前已经到了边界
                            //好吧，去处理下 
                            //MatchG.DrawImage(FindImage, new Rectangle(TopLeftP.X + 1, TopLeftP.Y + 1, BottomRightP.X - TopLeftP.X, BottomRightP.Y - TopLeftP.Y), new Rectangle(0, 0, FindImage.Width, FindImage.Height), GraphicsUnit.Pixel);
                            //goto ffff;
                            //fillPointColor(Matchbit,Filterbit , findPoint);
                            //if (FilterPointCollection.ContainsKey(TopLeftP))
                            //{
                            //foreach (Point FP in findPoint)
                            //{
                            //    if (!FilterPointCollection[TopLeftP].Exists(c => c.Equals(FP)))
                            //    {
                            //        FilterPointCollection[TopLeftP].Add(FP);
                            //    }
                            //}
                            //}
                            //else
                            //{
                            //    FilterPointCollection.Add(TopLeftP, findPoint);
                            //}
                            //保存在图片中的原像素点，以及相对相素点
                            originalPoint.Add(findPoint);
                            dynamicPoint.Add(GetDynamicPoint(findPoint));
                        }
                        findPoint = new List<Point>();
                        lostNum = -999;
                        topLeftP = new Point(matchbit.Width, matchbit.Height);
                        bottomRightP = new Point(0, 0);
                    }
                    if (isError)
                        break;
                }
            }
            ffff:

            #endregion

            var findbit = new Bitmap(lessColorbit, new Size(800, 60));

            //Findbit.SetResolution(72, 72);
            var findG = Graphics.FromImage(findbit);
            findG.Clear(Color.FromArgb(255, 255, 255, 255));

            var drawIndex = 0;
            var currHei = 0;
            var currWid = 0;
            var resolveImages = new List<Bitmap>();
            var isLearn = !String.IsNullOrEmpty(learnText);
            var textMatch = CaptchTextManager.GetAll();
            var text = "";
            //foreach (KeyValuePair<Point, List<Point>> tmpValue in FilterPointCollection)
            foreach (var tmpValue in dynamicPoint)
            {
                if (isLearn)
                {
                    if (String.IsNullOrEmpty(learnText))
                        break;
                    var currText = learnText[0].ToString();
                    if (currText != " ")
                    {
                        CaptchTextManager.Learn(tmpValue, currText);
                    }
                    if (learnText.Length == 1)
                    {
                        learnText = "";
                    }
                    else
                    {
                        learnText = learnText.Substring(1, learnText.Length - 1);
                    }
                }
                else
                {
                    //遍历结果
                    var tmpbit = new Bitmap(lessColorbit);
                    var tmpg = Graphics.FromImage(tmpbit);
                    tmpg.Clear(Color.FromArgb(255, 255, 255, 255));
                    var topLeft = new Point(tmpbit.Width, tmpbit.Height);
                    var bottomRight = new Point(0, 0);
                    foreach (var tmpP in tmpValue)
                    {
                        tmpbit.SetPixel(tmpP.X, tmpP.Y, Color.FromArgb(255, 0, 0, 0));
                        if (tmpP.X < topLeft.X)
                            topLeft.X = tmpP.X;
                        if (tmpP.Y < topLeft.Y)
                            topLeft.Y = tmpP.Y;
                        if (tmpP.X > bottomRight.X)
                            bottomRight.X = tmpP.X;
                        if (tmpP.Y > bottomRight.Y)
                            bottomRight.Y = tmpP.Y;
                    }
                    //好了，结束
                    var wid = bottomRight.X - topLeft.X + 1;
                    var hei = bottomRight.Y - topLeft.Y + 1;
                    if (currWid + wid > 800)
                    {
                        currWid = 0;
                        currHei += 20;
                    }
                    findG.DrawImage(tmpbit, new Rectangle(currWid, currHei, wid, hei), new Rectangle(topLeft.X, topLeft.Y, wid, hei),
                        GraphicsUnit.Pixel);
                    currWid += wid + 5;
                }

                //好了，解码
                text += ResloveCodebyDic(textMatch, tmpValue, 3);

                //ResolveImages.Add(ResolveCode(tmpValue.Value));
            }

            return new VerificationCodeResult
            {
                Text = text,
                GrayscaleBit = grayscaleBit,
                LessColorbit = lessColorbit,
                Matchbit = matchbit,
                Filterbit = filterbit,
                ResolveImages = resolveImages,
                Findbit = findbit,
            };
        }

        private static  void FillPointColor(Bitmap b,Bitmap f, List<Point> value)
        {
            //Graphics ddd = Graphics.FromImage(b);
            foreach (var pp in value)
            {
                b.SetPixel(pp.X, pp.Y , Color.FromArgb(255, 255, 255, 255));
                f.SetPixel(pp.X, pp.Y, Color.FromArgb(255, 0, 0, 0));
            }
        }
        /// <summary>
        /// 匹配颜色
        /// </summary>
        /// <param name="currCol">当前的颜色</param>
        /// <param name="matchCol">参照颜色</param>
        /// <param name="offsetValue">偏移量</param>
        /// <returns></returns>
        private static Boolean ContrastColor(int currCol, int matchCol, int offsetValue)
        {
            if (( currCol == matchCol )||
                (currCol < matchCol &&currCol + offsetValue >=matchCol )||
                (currCol > matchCol && currCol - offsetValue <= matchCol ))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 把取得的像素点，重置到左上角
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static List<Point> GetDynamicPoint(List<Point> value)
        {
            var topLeft = new Point(1000, 1000);
            //Point BottomRight = new Point(0, 0);
            //第一步，找出顶点
            foreach (var tmpv in value)
            {
                if (tmpv.X < topLeft.X)
                    topLeft.X = tmpv.X;
                if (tmpv.Y < topLeft.Y)
                    topLeft.Y = tmpv.Y;
            }
            var res = new List<Point>();
            for (var i = 0; i < value.Count;i++ )
            {
                var tmpv = value[i];
                res.Add (new Point(tmpv.X - topLeft.X, tmpv.Y - topLeft.Y));
            }
            return res;
        }

        /// <summary>
        /// 智能解码，不用了
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static Bitmap ResolveCode(List<Point> value)
        {
            var newf = new Font("宋体",18);
            //第一步，计算长宽
            var drawbit = new Bitmap(1500, 30);
            drawbit.SetResolution(72, 72);
            var drawG = Graphics.FromImage(drawbit);
            drawG.Clear(Color.FromArgb(255, 255, 255, 255));

            var drawWidth = 0;

            var topLeft = new Point(200,200);
            var bottomRight = new Point(0, 0);
            foreach (var tmpP in value )
            {
                drawbit.SetPixel(tmpP.X, tmpP.Y, Color.FromArgb(255, 0, 0, 0));
                if (tmpP.X < topLeft.X)
                    topLeft.X = tmpP.X;
                if (tmpP.Y < topLeft.Y)
                    topLeft.Y = tmpP.Y;
                if (tmpP.X > bottomRight.X)
                    bottomRight.X = tmpP.X;
                if (tmpP.Y > bottomRight.Y)
                    bottomRight.Y = tmpP.Y;
            }
            //好了，结束
            var wid = bottomRight.X - topLeft.X + 1;
            var hei = bottomRight.Y - topLeft.Y + 1;

            drawWidth = topLeft .X + wid + 1;
            var dic = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (var i = 0; i < dic.Length;i++ )
            {
                var tmpC = dic[i];
                var newB = new Bitmap(200, 200);
                newB.SetResolution(72, 72);
                var newG = Graphics.FromImage(newB);
                newG.Clear(Color.FromArgb(255, 255, 255, 255));
                newG.DrawString(tmpC.ToString (), newf, new SolidBrush(Color.FromArgb(255, 0, 0, 0)), new RectangleF(topLeft, new SizeF(wid, hei)));
                //好了，来比比
                //取百分比吧
                var donePointNum = 0;
                foreach (var tmpP in value)
                {
                    if (newB.GetPixel(tmpP.X, tmpP.Y).R != 255)
                    {
                        //成了
                        newB.SetPixel(tmpP.X, tmpP.Y, Color.FromArgb(255, 128, 128, 128));
                        donePointNum++;
                    }
                }
                drawG.DrawImage(newB, new Rectangle(drawWidth, topLeft.Y, wid, hei), new Rectangle(topLeft.X, topLeft.Y, wid, hei), GraphicsUnit.Pixel);
                drawWidth += wid + 1;
            }

            return drawbit;
        }

        /// <summary>
        /// 字典解码
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="value"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static String ResloveCodebyDic(List<CaptchaValue> dic, List<Point> value,int offset)
        {
            var text = "¤";
            if (dic != null && dic.Count > 0 && value != null && value.Count > 0)
            {
                //Dictionary<String, int> Result = new Dictionary<string, int>();                
                var allMaxNum = 0;
                foreach (var tmpV in dic)
                {
                    //Result.Add(tmpV.Text, 0);
                    var maxNum = 0;
                    //值有效，可以继续 
                    for (var i = -1*offset ; i <= offset; i++)
                    {
                        for (var j = -1*offset ; j <= offset; j++)
                        {
                            var matchNum = MatchPoint(tmpV.Value, value, i, j);
                            if (matchNum > maxNum)
                                maxNum = matchNum;
                        }
                    }
                    if (maxNum > allMaxNum )
                    {
                        allMaxNum = maxNum ;
                        if (maxNum * 2 >= tmpV.Value.Count )
                        {
                            text = tmpV.Text;
                        }
                    }
                    //Result[tmpV.Text] = MaxNum;
                }
                //好了，执行完了                       
            }
            return text;
        }
        private static int MatchPoint(List<Point> p1, List<Point> p2, int xOffset, int yOffset)
        {
            var matchNum = 0;
            if (p1 != null && p1.Count > 0 &&
                p2 != null && p2.Count > 0)
            {
                foreach (var tmpP in p1)
                {
                    if (p2.Exists(c => c.X == tmpP.X + xOffset && c.Y == tmpP.Y + yOffset))
                    {
                        //能找到，说明成功了
                        matchNum++;
                    }
                }
            }
            return matchNum;
        }
    }

    /// <summary>
    /// 识别结果
    /// </summary>
    public class VerificationCodeResult
    {
        public string Text { get; set; }

        public Bitmap LessColorbit { get; set; }
        public Bitmap GrayscaleBit{ get; set; }
        public Bitmap Matchbit{ get; set; }
        public Bitmap Filterbit{ get; set; }
        public List<Bitmap> ResolveImages { get; set; }
        public Bitmap Findbit{ get; set; }
    }
}
