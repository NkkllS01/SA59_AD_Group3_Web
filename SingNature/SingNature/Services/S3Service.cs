using Amazon.S3;
using Amazon.S3.Model;
using Amazon;
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

        public S3Service(IConfiguration configuration, ILogger<S3Service> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // ✅ Read configuration values from appsettings.json
            string accessKey = configuration["DigitalOcean:AccessKey"];
            string secretKey = configuration["DigitalOcean:SecretKey"];
            _bucketName = configuration["DigitalOcean:BucketName"];
            string endpointUrl = configuration["DigitalOcean:EndpointUrl"];
            _cdnUrl = configuration["DigitalOcean:CdnUrl"];

            // ✅ Ensure all required configurations are present
            if (string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) ||
                string.IsNullOrEmpty(endpointUrl) || string.IsNullOrEmpty(_bucketName) || string.IsNullOrEmpty(_cdnUrl))
            {
                _logger.LogError("❌ Missing DigitalOcean Spaces configuration. Please check appsettings.json.");
                throw new Exception("DigitalOcean Spaces configuration is incomplete. Please check appsettings.json.");
            }

            _logger.LogInformation("🚀 DigitalOcean S3 Service initialized.");
            _logger.LogInformation($"🌍 Endpoint URL: {endpointUrl}");
            _logger.LogInformation($"🔗 CDN URL: {_cdnUrl}");

            // ✅ Initialize Amazon S3 Client for DigitalOcean Spaces
            var clientConfig = new AmazonS3Config
            {
                ServiceURL = endpointUrl, // ✅ Required for DigitalOcean Spaces
                ForcePathStyle = true  // ✅ Required for DigitalOcean Spaces
            };

            _s3Client = new AmazonS3Client(accessKey, secretKey, clientConfig);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                if (fileStream == null || fileStream.Length == 0)
                    throw new ArgumentException("❌ File stream is empty or invalid", nameof(fileStream));

                // ✅ Generate a unique filename
                string objectKey = $"uploads/{Guid.NewGuid()}_{fileName}";

                var putRequest = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = objectKey,
                    InputStream = fileStream,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead  // ✅ Make the file publicly accessible
                };

                _logger.LogInformation($"📤 Uploading file: {objectKey}...");

                var response = await _s3Client.PutObjectAsync(putRequest);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    string fileUrl = $"{_cdnUrl}/{objectKey}";
                    _logger.LogInformation($"✅ Upload successful: {fileUrl}");
                    return fileUrl; // ✅ Return correct file URL using CDN
                }

                throw new Exception($"❌ Upload failed, HTTP Status Code: {response.HttpStatusCode}");
            }
            catch (AmazonS3Exception s3Ex)
            {
                _logger.LogError($"🛑 AmazonS3 Exception: {s3Ex.Message}", s3Ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"🛑 Upload failed: {ex.Message}", ex);
                throw;
            }
        }
    }
}