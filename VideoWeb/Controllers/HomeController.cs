using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Mvc;
using VideoWeb;

namespace VideoWeb {
    
    public class HomeController : Controller
    {
        VideosArr videos = new VideosArr();
        CommentArr comments = new CommentArr();

        static string[] mediaExtensions = {
            ".OGG", ".AVI", ".MP4"
        };

        static bool IsMediaFile(string path)
        {
            return -1 != Array.IndexOf(mediaExtensions, Path.GetExtension(path).ToUpperInvariant());
        }


        [HttpPost, OutputCache(NoStore = true, Duration =1)]
        public async Task<ActionResult> Index(HttpPostedFileBase file, string name, string description)
        {                       

            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Videos"),
                                    Path.GetFileName(file.FileName));
                    if (!IsMediaFile(path))
                    {
                        throw new Exception("wrong video format");
                    }
                    
           
                    file.SaveAs(path);
                    Video vid = new Video();
                    vid.Url = "/Videos/" + file.FileName;
                    vid.Description = description;
                    vid.Name = name;
                    videos.AddVideoToDB(vid.Url, vid.Name, vid.Description);
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return View(videos.GetVideos());
        }

        // GET: Videos
        public async Task<ActionResult> Index()
        {           
            return View(videos.GetVideos());
        }

        [OutputCache(NoStore = true, Duration = 1)]
        public async Task<ActionResult> Video(int videoId)
        {
            Video vid = videos.GetVideoById(videoId);
            List<Comment> coms = new List<Comment>();
            coms = comments.GetComments();
            foreach (var item in coms)
            {
                if (item.VideoId==videoId)
                {
                    vid.Comments.Add(item);
                }               
            }

            return View(vid);
        }

        public async Task<ActionResult> DeleteVideo(bool confirm, int _videoId)
        { 
            videos.RemoveVideo(_videoId);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> AddComment(int videoId, string email, string name, string message, string startTime, string endTime)
        {
            Video vid = new Video();
            if (true)
                try
                {
                    vid = videos.GetVideoById(videoId);
                    Comment com = new Comment();
                    com.VideoId = videoId;
                    com.Email = email;
                    com.Message = message;
                    com.Name = name;
                    com.StartTime = startTime;
                    com.EndTime = endTime;
                    comments.AddCommentToDB(com.VideoId, com.Name, com.Email, com.Message, com.StartTime, com.EndTime);


                    ViewBag.Message = "comment added";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "not entered comment.";
            }
            return RedirectToAction("Video", "Home", new { videoId = vid.Id });
            //return View(Video(vid.Id));
        }
    }
}
