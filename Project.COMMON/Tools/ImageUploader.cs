using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Project.COMMON.Tools
{
    public static class ImageUploader
    {
        //Geriye string deger döndüren metodumuz parametre olarak resmin hangi dosya yolunda oldugunu ve hangi resmi alacagını bilmek zorunda...

        //HttpPostedFileBase => MVC'de eger Post yönteminde dosya geliyorsa bu, HttpPostedFileBase tipinde tutulur 


        public static string UploadImage(string serverPath, HttpPostedFileBase file)
        {
            if (file != null)
            {
                Guid uniqueName = Guid.NewGuid();
                // ~/Images/asdasdasd/asdasd.jpg
                // /Images/
                //Mvc'de ~ karakteri normal sekilde kök dizine cık olarak backend işlemlerinden gectikten sonra algılanmaz.. (@Url.Action helper'i kullanmadıgınız sürece algılanmaz ) ... Dolayısıyla bu string karakteri kaldırarak pathi düzenlemek daha saglıklıdır...
                serverPath = serverPath.Replace("~", string.Empty); //Eger gelen serverPath parametresinde ~(tilda) karakteri varsa onu boslukla degiştir.
                                                                    // "cagri.starwars.xwing.png"

                string[] fileArray = file.FileName.Split('.'); //burada Split metodu sayesinde ilgili yapının uzantısını aldık. Split metodu belirttigimiz char karakterinden metni bölerek bize bir array sunar.
                //"cagri","starwars","xwing","png"

                string extension = fileArray[fileArray.Length - 1].ToLower(); //dosya uzantısını yaklayarak kücük harflere cevirdik

                string fileName = $"{uniqueName}.{extension}";

                if (extension=="jpg"||extension=="gif"||extension=="png"||extension=="jpeg")
                {
                    if (File.Exists(HttpContext.Current.Server.MapPath(serverPath + fileName)))
                    {
                        return "1"; //ancak Guid kullandıgımız icin bu acıdan zaten güvendeyiz.. (Dosya zaten var kodu)
                    }

                    else
                    {
                        string filePath = HttpContext.Current.Server.MapPath(serverPath + fileName);
                        file.SaveAs(filePath);
                        return serverPath + fileName;
                    }

                }
                else
                {
                    return "2"; //Secilen dosya bir resim degildir.
                }







            }

            else
            {
                return "3";//Dosya bos kodu
            }

            

        }
    }
}