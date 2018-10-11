using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace SuperBlogLambda.Pages
{
    public class NewPostModel : PageModel
    {
        private IConfiguration Configuration { get; }

        public static string Message { get; set; }

        public NewPostModel(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync(string postTitle, string postContent)
        {
            var s3Client = new AmazonS3Client(Amazon.RegionEndpoint.GetBySystemName(Configuration["AWS_REGION"]));
            GetObjectRequest request = new GetObjectRequest();

            var result = await s3Client.PutObjectAsync(new PutObjectRequest()
            {
                ContentBody = postContent,
                BucketName = Configuration["BUCKETNAME"],
                Key = $"EN/{postTitle.ToString().Replace(" ", "_")}.txt"

            });

            Message = result.HttpStatusCode == System.Net.HttpStatusCode.OK ? "Posted" : "Error";

            return Page();
        }
    }
}