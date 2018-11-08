using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using SuperBlogLambda.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SuperBlogLambda.Pages
{
    public class IndexModel : PageModel
    {
        private IConfiguration Configuration { get; }
        public List<PostModel> Posts = new List<PostModel>();

        public IndexModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IActionResult> OnGet(string lang)
        {
            lang = lang ?? "EN";
            ViewData["Lang"] = lang;
           
            try
            {
                using (var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(Configuration["AWS_REGION"])))
                {
                    var listObjectsAsyncResponse = await s3Client.ListObjectsAsync(new ListObjectsRequest()
                    {
                        BucketName = Configuration["BUCKETNAME"],
                        Prefix = lang.ToUpper()
                    });

                    GetObjectRequest request = new GetObjectRequest
                    {
                        BucketName = Configuration["BUCKETNAME"]
                    };

                    GetObjectResponse response;

                    foreach (S3Object s3Object in listObjectsAsyncResponse.S3Objects)
                    {
                        request.Key = s3Object.Key;
                        response = await s3Client.GetObjectAsync(request);

                        StreamReader reader = new StreamReader(response.ResponseStream);

                        Posts.Add(new PostModel()
                        {
                            postTitle = s3Object.Key.ToString().Substring(3).Replace("_", " ").Replace(".txt", ""),
                            postContent = reader.ReadToEnd(),
                            postTime = s3Object.LastModified
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Page();
        }
    }
}
