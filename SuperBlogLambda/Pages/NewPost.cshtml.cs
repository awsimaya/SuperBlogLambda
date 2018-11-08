using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SuperBlogLambda.Constants;
using Amazon.S3.Model;

namespace SuperBlogLambda.Pages
{
    public class NewPostModel : PageModel
    {
        public static string Message { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string postTitle, string postContent)
        {
            var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.USEast1);
            GetObjectRequest request = new GetObjectRequest();

            var result = await s3Client.PutObjectAsync(new PutObjectRequest()
            {
                ContentBody = postContent,
                BucketName = ValueConstants.BUCKETNAME,
                Key = $"EN/{postTitle.ToString().Replace(" ", "_")}.txt"
            });

            Message = result.HttpStatusCode == System.Net.HttpStatusCode.OK ? "Posted" : "Error";

            return Page();
        }
    }
}