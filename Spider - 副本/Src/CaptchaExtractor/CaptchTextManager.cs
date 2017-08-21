using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace CaptchaExtractor
{
    public static class CaptchTextManager
    {
        /// <summary>
        /// 学习，存在大小写存储问题
        /// </summary>
        /// <param name="value"></param>
        /// <param name="Text"></param>
        public static void Learn(List<Point> value, String Text)
        {
            if (value != null && !String.IsNullOrEmpty(Text))
            {
                String Dir = Application.StartupPath + @"\dic\";
                String Path = Application.StartupPath + @"\dic\" + Text + ".xml";
                if (!Directory.Exists(Dir))
                {
                    Directory.CreateDirectory(Dir);
                }
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
                using (FileStream newfs = new FileStream(Path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    XmlSerializer newS = new XmlSerializer(typeof ( CaptchaValue));

                    CaptchaValue newv = new CaptchaValue()
                    {
                         Text = Text ,
                          Value = value 
                    };
                    newS.Serialize(newfs, newv);
                }
            }
        }

        /// <summary>
        /// 取得当前所有的验证码字典
        /// </summary>
        /// <returns></returns>
        public static List<CaptchaValue > GetAll()
        {
            List<CaptchaValue> res = new List<CaptchaValue>();
            String Path = Application.StartupPath + @"\dic\";
            if (Directory.Exists(Path))
            {
                String[] allFile = Directory.GetFiles(Path);
                XmlSerializer newS = new XmlSerializer(typeof(CaptchaValue));
                foreach (String filePath in allFile)
                {
                    using (FileStream newfs = new FileStream(filePath , FileMode.OpenOrCreate, FileAccess.Read))
                    {
                        try
                        {
                            res.Add ((CaptchaValue ) newS.Deserialize(newfs));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            return res;
        }
    }
}
