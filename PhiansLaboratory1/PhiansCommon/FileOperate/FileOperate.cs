using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PhiansCommon
{
    /// <summary>
    /// 文件保存操作类
    /// </summary>
  public  class FileOperate
    {

    /// <summary>
    /// 文件保存操作不带格式验证
    /// </summary>
    /// <param name="files">文件集合</param>
    /// <param name="targetFolderUrl">保存文件夹路径（相对）</param>
    /// <returns></returns>
      public Resultinfo Filesave(HttpFileCollection files,string TargetFolderUrl)
    
    {
        Resultinfo Resultinfo_ = new Resultinfo();
        List<FileInfo_> FileInfoTmp = new List<FileInfo_>();//临时文件信息集合
   
        try
        {    // 判断文件夹是否存在
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(TargetFolderUrl)))
            {
                ///没有文件夹创建一个
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(TargetFolderUrl));
               
            }   
            //循环保存处理文件
            foreach (string item in files)
            {
                    FileInfo_ filemodel = new FileInfo_();//临时文件信息          
                    filemodel.FileName = Path.GetFileNameWithoutExtension(files[item].FileName);//获取无扩展名的文件名
                    filemodel.FileFormat = Path.GetExtension(files[item].FileName).ToLower();  //文件扩展名   
                    //string[] fileName_ = fileName.Split('_');             
                    filemodel.FileRelativeUrl = TargetFolderUrl + Guid.NewGuid() + filemodel.FileFormat;
                    filemodel.FileAbsoluteUrl = HttpContext.Current.Server.MapPath(filemodel.FileRelativeUrl);
                    //保存文件
                    files[item].SaveAs(filemodel.FileAbsoluteUrl);
                    if (!File.Exists(filemodel.FileAbsoluteUrl))
                    {
                        Resultinfo_.Sucess = false;                  
                        Resultinfo_.ReturnContent = "保存文件失败，失败文件：" + filemodel.FileName;
                        //只要文件上传失败删除已经上传的文件
                        try
                        {

                            foreach (var item2 in FileInfoTmp)
                            {
                   
                                if (File.Exists(item2.FileAbsoluteUrl)) 
                                {
                                    File.Delete(item2.FileAbsoluteUrl);
                                }
                            }
                        }
                        catch 
                        {
                                              
                        }
                        break;
                    }
                    FileInfoTmp.Add(filemodel);
                    //返回成功的信息
                    Resultinfo_.Sucess = true;                         
            }

            Resultinfo_.FileInfo = FileInfoTmp;

            return Resultinfo_;

        }
        catch (Exception)
        {
            //异常删除已经上传的文件
            try
            {

                foreach (var item2 in FileInfoTmp)
                {
                    if (File.Exists(item2.FileAbsoluteUrl))
                    {
                        File.Delete(item2.FileAbsoluteUrl);
                    }
                }
            }
            catch
            {

            }
            throw;
        }
      }

      /// <summary>
      /// 文件保存操作带个格式验证
      /// </summary>
      /// <param name="files">文件集合</param>
      /// <param name="targetFolderUrl">保存文件夹路径（相对）</param>
      /// <param name="Extension">Extension支持文件格式</param>
    
      /// <returns></returns>
      public Resultinfo Filesave(HttpFileCollection files, string targetFolderUrl, string[] Extension)
      {
          Resultinfo Resultinfo_ = new Resultinfo();
          List<FileInfo_> FileInfoTmp = new List<FileInfo_>();//临时文件信息集合
           
          try
          {
              //文件格式验证
              foreach (string item in files) {
                  string FileName = Path.GetFileName(files[item].FileName);//获取无扩展名的文件名
               string  FileFormat = Path.GetExtension(files[item].FileName).ToLower();  //文件扩展名   
               int findExtensionidnex = Array.IndexOf(Extension, FileFormat);
                  //不支持的文件格式不做上传操作
               if (findExtensionidnex == -1) {
                   Resultinfo_.Sucess = false;
                   Resultinfo_.ReturnContent = "文件格式上传错误：" + FileName + ",请上传：" ;
                   foreach (var item2 in Extension)
                   {
                       Resultinfo_.ReturnContent += item2 + "、";
                   }
                   Resultinfo_.ReturnContent = Resultinfo_.ReturnContent.Remove(Resultinfo_.ReturnContent.Length - 1, 1);
                   return Resultinfo_;
               }
              }
              
              // 判断文件夹是否存在
              if (!Directory.Exists(HttpContext.Current.Server.MapPath(targetFolderUrl)))
              {
                  ///没有文件夹创建一个
                  Directory.CreateDirectory(HttpContext.Current.Server.MapPath(targetFolderUrl));

              }
              //循环保存处理文件
              foreach (string item in files)
              {
                  FileInfo_ filemodel = new FileInfo_();//临时文件信息          
                  filemodel.FileName = Path.GetFileNameWithoutExtension(files[item].FileName);//获取无扩展名的文件名
                  filemodel.FileFormat = Path.GetExtension(files[item].FileName).ToLower();  //文件扩展名   
                  //string[] fileName_ = fileName.Split('_');             
                  filemodel.FileRelativeUrl = targetFolderUrl + Guid.NewGuid() + filemodel.FileFormat;
                  filemodel.FileAbsoluteUrl = HttpContext.Current.Server.MapPath(filemodel.FileRelativeUrl);
                  //保存文件
                  files[item].SaveAs(filemodel.FileAbsoluteUrl);
                  if (!File.Exists(filemodel.FileAbsoluteUrl))
                  {
                      Resultinfo_.Sucess = false;
                      Resultinfo_.ReturnContent = "保存文件失败，失败文件：" + filemodel.FileName;
                      //只要文件上传失败删除已经上传的文件
                      try
                      {

                          foreach (var item2 in FileInfoTmp)
                          {

                              if (File.Exists(item2.FileAbsoluteUrl))
                              {
                                  File.Delete(item2.FileAbsoluteUrl);
                              }
                          }
                      }
                      catch
                      {

                      }
                      break;
                  }
                  FileInfoTmp.Add(filemodel);
                  //返回成功的信息
                  Resultinfo_.Sucess = true;
              }

              Resultinfo_.FileInfo = FileInfoTmp;

              return Resultinfo_;

          }
          catch (Exception)
          {
              //异常删除已经上传的文件
              try
              {

                  foreach (var item2 in FileInfoTmp)
                  {
                      if (File.Exists(item2.FileAbsoluteUrl))
                      {
                          File.Delete(item2.FileAbsoluteUrl);
                      }
                  }
              }
              catch
              {

              }
              throw;
          }
      }
      /// <summary>
      /// 文件保存操作
      /// </summary>
      /// <param name="files">文件集合</param>
      /// <param name="targetFolderUrl">保存文件夹路径（相对）</param>
      /// <param name="Extension">Extension支持文件格式</param>
      /// <param name="FileNames">保存文件名集合</param>
      /// <returns></returns>
      public Resultinfo Filesave(HttpFileCollection files, string TargetFolderUrl, string[] Extension,string[] FileNames)
      {
          Resultinfo Resultinfo_ = new Resultinfo();
          List<FileInfo_> FileInfoTmp = new List<FileInfo_>();//临时文件信息集合

          try
          {
              if (files.Count != FileNames.Length) {

                  Resultinfo_.Sucess = false;
                  Resultinfo_.ReturnContent = "保存文件数量和文件名称一致";
                  return Resultinfo_;
              }

              int i = 0;
              //文件格式验证
              foreach (string item in files)
              {
                  string FileName = Path.GetFileName(files[item].FileName);//获取无扩展名的文件名
                  string FileFormat = Path.GetExtension(files[item].FileName).ToLower();  //文件扩展名   
                  int findExtensionidnex = Array.IndexOf(Extension, FileFormat);
                  //不支持的文件格式不做上传操作
                  if (findExtensionidnex == -1)
                  {
                      Resultinfo_.Sucess = false;
                      Resultinfo_.ReturnContent = "文件格式上传错误：" + FileName + ",请上传：";
                      foreach (var item2 in Extension)
                      {
                          Resultinfo_.ReturnContent += item2 + "、";
                      }
                      Resultinfo_.ReturnContent = Resultinfo_.ReturnContent.Remove(Resultinfo_.ReturnContent.Length - 1, 1);
                      return Resultinfo_;
                  }
              }

              // 判断文件夹是否存在
              if (!Directory.Exists(HttpContext.Current.Server.MapPath(TargetFolderUrl)))
              {
                  ///没有文件夹创建一个
                  Directory.CreateDirectory(HttpContext.Current.Server.MapPath(TargetFolderUrl));

              }
              //循环保存处理文件
              foreach (string item in files)
              {
                  FileInfo_ filemodel = new FileInfo_();//临时文件信息          
                  filemodel.FileName = Path.GetFileNameWithoutExtension(files[item].FileName);//获取无扩展名的文件名
                  filemodel.FileFormat = Path.GetExtension(files[item].FileName).ToLower();  //文件扩展名   
                  //string[] fileName_ = fileName.Split('_');             
                  filemodel.FileRelativeUrl = TargetFolderUrl + FileNames[i] + filemodel.FileFormat;
                  filemodel.FileAbsoluteUrl = HttpContext.Current.Server.MapPath(filemodel.FileRelativeUrl);
                  //保存文件
                  files[item].SaveAs(filemodel.FileAbsoluteUrl);
                  if (!File.Exists(filemodel.FileAbsoluteUrl))
                  {
                      Resultinfo_.Sucess = false;
                      Resultinfo_.ReturnContent = "保存文件失败，失败文件：" + filemodel.FileName;
                      //只要文件上传失败删除已经上传的文件
                      try
                      {

                          foreach (var item2 in FileInfoTmp)
                          {

                              if (File.Exists(item2.FileAbsoluteUrl))
                              {
                                  File.Delete(item2.FileAbsoluteUrl);
                              }
                          }
                      }
                      catch
                      {

                      }
                      break;
                  }
                  FileInfoTmp.Add(filemodel);
                  //返回成功的信息
                  i++;
                  Resultinfo_.Sucess = true;
              }

              Resultinfo_.FileInfo = FileInfoTmp;

              return Resultinfo_;

          }
          catch (Exception)
          {
              //异常删除已经上传的文件
              try
              {

                  foreach (var item2 in FileInfoTmp)
                  {
                      if (File.Exists(item2.FileAbsoluteUrl))
                      {
                          File.Delete(item2.FileAbsoluteUrl);
                      }
                  }
              }
              catch
              {

              }
              throw;
          }
      }
    }

    /// <summary>
    /// 文件上传结果
    /// </summary>
  public class Resultinfo {

   
      /// <summary>
      ///成功后的文件集合
      /// </summary>
      public List< FileInfo_> FileInfo
      {
          get;
          set;
      }
      /// <summary>
      /// 是否成功1，成功 0失败
      /// </summary>
      public bool Sucess
      {
          get;
          set;
      }
      /// <summary>
      /// 返回信息
      /// </summary>
      public string ReturnContent
      {
          get;
          set;
      }

  }

    /// <summary>
    /// 文件信息
    /// </summary>
  public class FileInfo_ {
      /// <summary>
      /// 文件名字
      /// </summary>
      public string  FileName
      {
          get;
          set;
      }

      /// <summary>
      /// 文件格式
      /// </summary>
      public string FileFormat
      {
          get;
          set;
      }
      /// <summary>
      /// 文件绝对路径
      /// </summary>
      public string FileAbsoluteUrl
      {
          get;
          set;
      }

      /// <summary>
      /// 文件相对路径
      /// </summary>
      public string FileRelativeUrl
      {
          get;
          set;
      }
  }
}
