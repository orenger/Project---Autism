using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace VideoWeb
{
    public class VideosArr
    {
        VideoDataTableAdapters.videosTableAdapter db = new VideoDataTableAdapters.videosTableAdapter();
        VideoDataTableAdapters.CommentsTableAdapter dbm = new VideoDataTableAdapters.CommentsTableAdapter();

        public VideosArr()
        {

        }

        public void RemoveVideo(int _videoId)
        {
            db.DeleteVideo(_videoId);
            dbm.DeleteVideoComments(_videoId);
                                 
        }

        public void AddVideoToDB(string _url, string _name, string _description)
        {
            if (_description == null)
                _description = "No Description";
            if (_url == null)
                _url = "/Videos/SampleVideo_1080x720_1mb.mp4";
            if (_name == null)
                _name = "No Name";

            db.InsertVideo(_url,_name, _description);
        }

        public List<Video> GetVideos()
        {
            VideoData.videosDataTable DT = db.GetVideos();
            List<Video> videos = new List<Video>();
            foreach (var item in DT)
            {
                Video vid = new Video();
                vid.Id = item.Video_ID;
                if (item.Url == null)
                    vid.Url = "";
                else
                    vid.Url = item.Url;
                if (item.Description == null)
                    vid.Description = "No Description";
                else
                    vid.Description = item.Description;
                if (item.Name == null)
                    vid.Name = "No Name";
                else
                    vid.Name = item.Name;

                videos.Add(vid);
            }

            return videos;

        }

        internal Video GetVideoById(int videoId)
        {
            VideoData.videosDataTable DT = db.GetVideos();
            foreach (var item in DT)
            {
                if (item.Video_ID == videoId)
                {
                    Video vid = new Video(item.Video_ID, item.Url, item.Name, item.Description);
                    return vid;
                }
            }
            return new Video();
        }
    }

    public class VideoDb: DbContext
    {
        public DbSet<Video> Videos { get; set; }
    }
    public class Video
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public List<Comment> Comments { get; set; }
        public string Name { get; set; }

        public Video(int _id, string _url,string _name, string _description)
        {
            Id = _id; 
            Url = _url;
            Name = _name;
            Description = _description;
            Comments = new List<Comment>();

        }

        public Video()
        {
            Comments = new List<Comment>();
        }
    }
}