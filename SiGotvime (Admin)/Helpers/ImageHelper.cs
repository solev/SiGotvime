using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Food_App_Service.Helpers
{

    public class ImageResult
    {
        public bool Success { get; set; }
        public string ImageName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ImageHelper
    {
        // set default size here
        public int Width { get; set; }

        public int Height { get; set; }

        // folder for the upload, you can put this in the web.config
        private readonly string UploadPath = "~/Content/images/";

        public string CreateFileName(string fileName,int counter = 0,bool cropped = false)
        {
            
            string prepend = cropped?"cropped_":"full_";
            string finalFileName = string.Format("{0}{1}_{2}", prepend, counter.ToString(), fileName);
            if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(UploadPath + finalFileName)))
            {
                //file exists => add country try again
                return CreateFileName(fileName, ++counter,cropped);
            }
            return finalFileName;
        }

        public ImageResult RenameUploadFile(HttpPostedFileBase file)
        {
            var fileName = Path.GetFileName(file.FileName).Replace(" ","");

            string finalFileName = CreateFileName(fileName);
            //file doesn't exist, upload item but validate first
            return UploadFile(file, finalFileName);
        }

        private ImageResult UploadFile(HttpPostedFileBase file, string fileName)
        {
            ImageResult imageResult = new ImageResult { Success = true, ErrorMessage = null };

            var path = Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fileName);
            string extension = Path.GetExtension(file.FileName);
            ImageFormat format = ValidateExtension(extension);
            //make sure the file is valid
            if (format == null)
            {
                imageResult.Success = false;
                imageResult.ErrorMessage = "Invalid Extension";
                return imageResult;
            }

            try
            {
                file.SaveAs(path);

                Image imgOriginal = Image.FromFile(path);

                //pass in whatever value you want
                Image imgActual = Scale(imgOriginal);
                imgOriginal.Dispose();
                //imgActual.Save(path);

                SaveTo(imgActual, path, format, 80);
                imgActual.Dispose();

                imageResult.ImageName = fileName;

                return imageResult;
            }
            catch (Exception ex)
            {
                // you might NOT want to show the exception error for the user
                // this is generally logging or testing

                imageResult.Success = false;
                imageResult.ErrorMessage = ex.Message;
                return imageResult;
            }
        }

        private ImageFormat ValidateExtension(string extension)
        {
            extension = extension.ToLower();
            switch (extension)
            {
                case ".jpg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Gif;
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpeg":
                    return ImageFormat.Jpeg;
                default:
                    return null;
            }
        }

        public Image cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            Bitmap bmpCrop = bmpImage.Clone(cropArea,
            bmpImage.PixelFormat);
            return (Image)(bmpCrop);
        }

        private Image Scale(Image imgPhoto)
        {
            float sourceWidth = imgPhoto.Width;
            float sourceHeight = imgPhoto.Height;
            float destHeight = 0;
            float destWidth = 0;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            // force resize, might distort image
            if (Width != 0 && Height != 0)
            {
                destWidth = Width;
                destHeight = Height;
            }
            // change size proportially depending on width or height
            else if (Height != 0)
            {
                destWidth = (float)(Height * sourceWidth) / sourceHeight;
                destHeight = Height;
            }
            else
            {
                destWidth = Width;
                destHeight = (float)(sourceHeight * Width / sourceWidth);
            }

            Bitmap bmPhoto = new Bitmap((int)destWidth, (int)destHeight,
                                        PixelFormat.Format32bppPArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, (int)destWidth, (int)destHeight),
                new Rectangle(sourceX, sourceY, (int)sourceWidth, (int)sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;
        }


        public ImageResult SaveNewImage(Image image,string fileName)
        {
            string fullFileName = CreateFileName(fileName,0,true);
            var path = Path.Combine(HttpContext.Current.Request.MapPath(UploadPath), fullFileName);
            string extension = Path.GetExtension(fullFileName);

            ImageFormat format = ValidateExtension(extension);
            if(format == null)
            {
                return new ImageResult
                {
                    Success = false,
                    ErrorMessage="Invalid Extension"
                };
            }
            Image newImage = Scale(image);
            SaveTo(newImage, path, format, 80);
            return new ImageResult
            {
                Success = true,
                ImageName = fullFileName 
            };
        }
        /// <summary>
        /// Saves the image to  specified stream and format.
        /// </summary>
        /// <param name="image">The image to save.</param>
        /// <param name="outputStream">The stream to used.</param>
        /// <param name="format">The format of new image.</param>
        /// <param name="quality">The quality of the image in percent.</param>
        public void SaveTo(Image image, string path, ImageFormat format, int quality)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            if (format == ImageFormat.Gif)
            {
                image.Save(path, ImageFormat.Gif);
            }
            else if (format == ImageFormat.Jpeg)
            {
                image.Save(path, encoders[1], encoderParameters);
            }
            else if (format == ImageFormat.Png)
            {
                image.Save(path, encoders[4], encoderParameters);
            }
            else if (format == ImageFormat.Bmp)
            {
                image.Save(path, encoders[0], encoderParameters);
            }
            else
            {
                image.Save(path, format);
            }
            
        }
    }
}