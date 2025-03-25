using System.Net.Http;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace OnePrivateNavigation.Helpers
{
    public class FaviconHelper
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FaviconHelper(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
        {
            _httpClientFactory = httpClientFactory;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string?> GetAndSaveFaviconAsync(string websiteUrl)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient();
                var baseUri = new Uri(websiteUrl);
                var faviconUrl = await GetFaviconUrlAsync(client, baseUri);

                if (string.IsNullOrEmpty(faviconUrl))
                {
                    return null;
                }

                var faviconBytes = await DownloadFaviconAsync(client, faviconUrl, baseUri);
                if (faviconBytes == null || faviconBytes.Length == 0)
                {
                    return null;
                }

                var fileName = await SaveFaviconAsync(faviconBytes, baseUri.Host);
                return fileName;
            }
            catch
            {
                return null;
            }
        }

        private async Task<string?> GetFaviconUrlAsync(HttpClient client, Uri baseUri)
        {
            // 从baseUri中获取根路由
            var rootUrl = baseUri.GetLeftPart(UriPartial.Authority);

            // 尝试直接访问/favicon.ico
            var directFaviconUrl = new Uri(rootUrl + "/favicon.ico");
            try
            {
                var response = await client.GetAsync(directFaviconUrl);
                if (response.IsSuccessStatusCode)
                {
                    return directFaviconUrl.ToString();
                }
            }
            catch
            {
                // 忽略错误，继续尝试其他方法
            }

            // 尝试从HTML中获取favicon链接
            try
            {
                var html = await client.GetStringAsync(baseUri);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var iconLink = doc.DocumentNode.SelectNodes("//link[@rel='icon' or @rel='shortcut icon']");
                if (iconLink != null && iconLink.Any())
                {
                    var href = iconLink.First().GetAttributeValue("href", "");
                    if (!string.IsNullOrEmpty(href))
                    {
                        return new Uri(baseUri, href).ToString();
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        private async Task<byte[]?> DownloadFaviconAsync(HttpClient client, string faviconUrl, Uri baseUri)
        {
            try
            {
                var response = await client.GetAsync(faviconUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
            catch
            {
                // 忽略错误
            }
            return null;
        }

        private async Task<string> SaveFaviconAsync(byte[] faviconBytes, string domain)
        {
            var fileName = $"favicon_{domain}_{DateTime.Now.Ticks}.ico";
            var filePath = Path.Combine(AppContext.BaseDirectory, "favicons", fileName);

            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }

            await File.WriteAllBytesAsync(filePath, faviconBytes);
            return $"/favicons/{fileName}";
        }
    }
}