﻿using System;
using System.IO;
using System.Runtime.Serialization;

using TumblThree.Domain.Models.Files;

namespace TumblThree.Domain.Models.Blogs
{
    [DataContract]
    public class TumblrLikedByBlog : Blog
    {
        public static Blog Create(string url, string location)
        {
            var blog = new TumblrLikedByBlog()
            {
                Url = ExtractUrl(url),
                Name = ExtractName(url),
                BlogType = BlogTypes.tlb,
                Location = location,
                Online = true,
                Version = "3",
                DateAdded = DateTime.Now,
            };

            Directory.CreateDirectory(location);
            Directory.CreateDirectory(Path.Combine(Directory.GetParent(location).FullName, blog.Name));

            blog.ChildId = Path.Combine(location, blog.Name + "_files." + blog.BlogType);
            if (!File.Exists(blog.ChildId))
            {
                IFiles files = new TumblrLikedByBlogFiles(blog.Name, blog.Location);
                files.Save();
                files = null;
            }

            return blog;
        }

        protected new static string ExtractName(string url) => url.Split('/')[5];

        protected new static string ExtractUrl(string url)
        {
            if (url.StartsWith("http://"))
                url = url.Insert(4, "s");
            int blogNameLength = url.Split('/')[5].Length;
            var urlLength = 32;
            return url.Substring(0, blogNameLength + urlLength);
        }
    }
}
