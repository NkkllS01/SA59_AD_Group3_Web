using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SingNature.Services
{
    public class S3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _cdnUrl;
        private readonly ILogger<S3Service> _logger;

        public S3Service(IAmazonS3 s3Client, IConfiguration configuration, ILogger<S3Service> logger)
        {
            _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _bucketName = configuration["DigitalOcean:BucketName"];
            _cdnUrl = configuration["DigitalOcean:CdnUrl"];

            if (string.IsNullOrEmpty(_bucketName) || string.IsNullOrEmpty(_cdnUrl))
            {
                _logger.LogError("❌ DigitalOcean 配置缺失");
                throw new Exception("DigitalOcean Spaces 配置错误");
            }

            _logger.LogInformation("🚀 DigitalOcean S3 Service 初始化成功");
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                if (fileStream == null || fileStream.Length == 0)
                    throw new ArgumentException("❌ 文件流为空或无效", nameof(fileStream));

                string objectKey = $"uploads/{Guid.NewGuid()}_{fileName}";

                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = objectKey,
                    InputStream = fileStream,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                _logger.LogInformation($"📤 上传文件: {objectKey}...");

                var response = await _s3Client.PutObjectAsync(putRequest);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    string fileUrl = $"{_cdnUrl}/{objectKey}";
                    _logger.LogInformation($"✅ 上传成功: {fileUrl}");
                    return fileUrl;
                }

                throw new Exception($"❌ 上传失败, HTTP 状态码: {response.HttpStatusCode}");
            }
            catch (AmazonS3Exception s3Ex)
            {
                _logger.LogError($"🛑 AmazonS3 异常: {s3Ex.Message}", s3Ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"🛑 上传失败: {ex.Message}", ex);
                throw;
            }
        }
    }
}
