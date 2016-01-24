using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoWeb
{
    public class CommentArr
    {
        VideoDataTableAdapters.CommentsTableAdapter db = new VideoDataTableAdapters.CommentsTableAdapter();

        public CommentArr()
        {

        }

        public void AddCommentToDB(int _videoId, string _name,string _email, 
            string _description, string _start, string _end)
        {
            if (_videoId <= 0)
                throw new Exception("wrong video id");
            if (_name == null)
                _name = "No Name";
            if (_description == null)
                _description = "No Description";
            if (_email == null)
                _email = "No Email";
            if (_start == null)
                _start = "No Start Time";
            if (_end == null)
                _end = "No End Time";

            db.InsertComment(_email,_name,_videoId, _description, _start, _end);
        }

        public List<Comment> GetComments()
        {
            VideoData.CommentsDataTable DT = db.GetComments();
            List<Comment> comments = new List<Comment>();
            foreach (var item in DT)
            {
                Comment com = new Comment() { CommentId = item.Comment_ID, VideoId = item.Video_ID, EndTime = item.EndTime,
                    StartTime = item.StartTime, Email = item.Email, Name = item.Name, Message = item.Comment};
                comments.Add(com);
            }

            return comments;

        }
    }
    public class Comment
    {
        public int CommentId { get; set; }
        public int VideoId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }



    }
}