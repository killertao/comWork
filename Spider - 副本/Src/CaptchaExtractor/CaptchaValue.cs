using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing ;

namespace CaptchaExtractor
{
    /// <summary>
    /// 验证码字典存储内容
    /// </summary>
    [Serializable ()]
    public class CaptchaValue
    {
        /// <summary>
        /// 文字
        /// </summary>
        public String Text{get;set;}
        /// <summary>
        /// 点阵
        /// </summary> 
        public List<Point > Value{get;set;}
    }
}
