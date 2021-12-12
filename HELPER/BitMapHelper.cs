using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HELPER
{
    public class BitMapHelper
    {
        public static string BitMapToBase64(Bitmap bitmap)
        {
            var str = "";
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);

                var bytes = stream.ToArray();
                str = Convert.ToBase64String(bytes);
            }

            return str;
        }

        public static Bitmap Base64ToBitMap(string img)
        {
            byte[] imageBytes = Base64.Decode(img, Base64Flags.Default);
            Bitmap decodedImage = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
            return decodedImage;
        }

        public static byte[] Base64ToByteArray(string img)
        {
            return BitmapToByteArray(Base64ToBitMap(img));
        }

        public static byte[] BitmapToByteArray(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, ms);
            return ms.ToArray();
        }

        public static Bitmap ByteArrayToBitmap(byte[] bytes)
        {
            return BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
        }

        public static Bitmap ResourctToBitmap(Resources resources, int resource)
        {
            return BitmapFactory.DecodeResource(resources, resource);
        }

        public static MemoryStream BitmapToMemoryStream(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                return stream;
            }
        }

        /// <summary>
        /// Convert ImageView image to string
        /// </summary>
        /// <param name="image">The Imageview</param>
        /// <returns>string represents the ImageView image</returns>
        public static string ImageViewToBase64(ImageView image)
        {
            Bitmap bitMap = ((BitmapDrawable)image.Drawable).Bitmap;
            return BitMapToBase64(bitMap);
        }
    }
}
