
using System.Drawing;
using System.Windows.Forms;
using Crawler.Helper;
namespace Crawler
{
    public partial class test : Form
    {

        public test()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            var cracker = new Cracker();
            var img = Image.FromFile("C:/Users/admin/Desktop/"+"yzm.png");
           Bitmap bitmap=new Bitmap(img);
           var result = cracker.Read(bitmap);
        }
    }
}
