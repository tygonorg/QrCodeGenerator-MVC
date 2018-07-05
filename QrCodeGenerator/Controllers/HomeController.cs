using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using Zen.Barcode;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace QrCodeGenerator.Controllers
{
	public class HomeController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			ViewData["token"] = "";

			return View();
		}
		[HttpPost]
		public ActionResult Index(String token)
		{
			Console.Write(token);
			ViewData["gen"] = true;
			ViewData["token"] = token;
			return View();
		}
		private Bitmap imageOuterEdge(Bitmap img, int size)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            Color transparent = Color.FromArgb(0, 0, 0, 0);
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    if (!isColourSame(img.GetPixel(i, j), transparent))
                    {
                        bmp.SetPixel(i, j, Color.White);
                    }

                }
            }
            return bmp;
        }
		private bool isColourSame(Color a, Color b)
        {
            if (a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A)
                return true;
            return false;
        }
		public ActionResult Imagez(String token)
        {
			CodeQrBarcodeDraw qrCode = BarcodeDrawFactory.CodeQr;
            Image img = qrCode.Draw(token, 100, 6);
            string path = Server.MapPath("~/App_Data/logo2.png");

            Image image_m = Image.FromFile(path);


            Bitmap baseImage;
            Bitmap overlayImage;
            baseImage = (Bitmap)img;
            overlayImage = (Bitmap)image_m;

            var finalImage = new Bitmap(baseImage.Width, baseImage.Height, PixelFormat.Format32bppArgb);
            var graphics = Graphics.FromImage(finalImage);
            graphics.CompositingMode = CompositingMode.SourceOver;
            float overlayPosX = baseImage.Width / 2 - 40f;
            float overlayPosY = baseImage.Height / 2 - 40f;
            graphics.DrawImage(baseImage, 0, 0);




            graphics.DrawImage(imageOuterEdge(overlayImage, 2), overlayPosX, overlayPosY, 80f, 80f);
            //overlayImage.MakeTransparent();
            graphics.DrawImage(overlayImage, baseImage.Width / 2 - 30f, baseImage.Height / 2 - 30f, 60f, 60f);
            // finalImage.MakeTransparent();


            //ViewData["bitmap"] = finalImage;
			var bitmap = finalImage;
			var bitmapBytes = ImageToByte2(bitmap);
            return File(bitmapBytes, "image/jpeg"); //Return as file result
        }
		private  byte[] ImageToByte2(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }



	}
}
