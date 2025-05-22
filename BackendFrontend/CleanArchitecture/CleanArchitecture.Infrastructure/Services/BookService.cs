using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using CleanArchitecture.Core.DTOs;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Book;
using AutoMapper;
using System;
using CleanArchitecture.Core.Entities;
using java.lang.management;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace CleanArchitecture.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private static readonly HttpClient _httpClient = new HttpClient();
        private readonly IWebHostEnvironment _env;
        public async Task<string> ScanBarcodeBase64Async(string base64Image)
        {
            try
            {
                // Remove any data URI prefix (e.g., "data:image/png;base64,...")
                var base64Data = base64Image.Contains(',')
                    ? base64Image.Substring(base64Image.IndexOf(',') + 1)
                    : base64Image;

                // Decode the base64 string to byte array
                var imageBytes = Convert.FromBase64String(base64Data);

                // Create a temporary image file
                var uploadsDir = Path.Combine(_env.ContentRootPath, "TempUploads");
                Directory.CreateDirectory(uploadsDir);

                var fileName = Guid.NewGuid().ToString() + ".jpg";
                var imagePath = Path.Combine(uploadsDir, fileName);
                await File.WriteAllBytesAsync(imagePath, imageBytes);

                // Run BarcodeScanner.exe and pass image path
                var processInfo = new ProcessStartInfo
                {
                    FileName = "BarcodeScanner.exe",
                    Arguments = $"--image \"{imagePath}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                string output;
                string errors;

                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();

                    output = await process.StandardOutput.ReadToEndAsync();
                    errors = await process.StandardError.ReadToEndAsync();

                    process.WaitForExit();
                }

                // Log both outputs
                Console.WriteLine($"[STDOUT] {output}");
                if (!string.IsNullOrWhiteSpace(errors))
                {
                    Console.WriteLine($"[STDERR] {errors}");
                }

                // Clean up temporary file
                File.Delete(imagePath);

                return output.Trim();
            }
            catch (FormatException ex)
            {
                Console.WriteLine("Invalid base64 string: " + ex.Message);
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error running barcode scanner: " + ex.Message);
                return string.Empty;
            }
        }
        public async Task<string> ScanBarcodePathAsync(string imagePath)
        {
            try
            {
                // Execute BarcodeScanner.exe to process the image
                var processInfo = new ProcessStartInfo
                {
                    FileName = "BarcodeScanner.exe",  // Path to your barcode scanning executable
                    Arguments = $"--image {imagePath}",  // Pass the image file path as an argument
                    RedirectStandardOutput = true,  // Capture the output
                    UseShellExecute = false,  // Don't use the shell
                    CreateNoWindow = true  // Don't show the command window
                };
                string output;
                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();
                    // Capture the output from the BarcodeScanner.exe
                    output = await process.StandardOutput.ReadToEndAsync();
                    process.WaitForExit();
                }
                // Log the output from the barcode scanner
                Console.WriteLine($"BarcodeScanner.exe output: {output}");
                return output.Trim();
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"Error occurred: {ex.Message}");
                return string.Empty;  // Optionally return an empty string if an error occurred
            }
        }
        public async Task<string> ScanBarcodeAsync(IFormFile image)
        {
            try
            {
                var uploadsDir = Path.Combine(_env.ContentRootPath, "TempUploads");
                Directory.CreateDirectory(uploadsDir); // Ensure directory is created
                var filePath = Path.Combine(uploadsDir, Guid.NewGuid() + Path.GetExtension(image.FileName));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Log success in saving file
                Console.WriteLine($"File saved to: {filePath}");

                // Execute BarcodeScanner.exe
                var processInfo = new ProcessStartInfo
                {
                    FileName = "BarcodeScanner.exe",
                    Arguments = $"--image \"{filePath}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                string output;
                using (var process = new Process { StartInfo = processInfo })
                {
                    process.Start();
                    output = await process.StandardOutput.ReadToEndAsync();
                    process.WaitForExit();
                }

                // Log the output from the process
                Console.WriteLine($"BarcodeScanner.exe output: {output}");

                // Optional: delete file after processing
                File.Delete(filePath);

                return output.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
                return string.Empty;  // Optionally return empty if an error occurred
            }
        }



        private async Task<(string url, JObject item)> GetMainUrlAsync(string isbn, string apiKey)
        {
            var searchUrl = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}&key={apiKey}";
            var response = await _httpClient.GetStringAsync(searchUrl);
            Console.WriteLine(response);

            var json = JObject.Parse(response);
            var item = json["items"]?.FirstOrDefault() as JObject;
            if (item == null) throw new Exception("Book not found.");

            string id = item["id"]?.ToString();
            string language = item["volumeInfo"]?["language"]?.ToString();
            string title = item["volumeInfo"]?["title"]?.ToString();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(language) || string.IsNullOrEmpty(title))
                throw new Exception("Invalid book data received.");

            string name = System.Net.WebUtility.UrlEncode(title.Replace(" ", "_"));

            string url = $"https://www.google.com.tr/books/edition/{name}/{id}?hl={language}&key={apiKey}";
            return (url, item);
        }


        public async Task<BookDto> SearchBookAsync(string isbn, string apiKey)
        {
            if (!(isbn.Length == 10 || isbn.Length == 13))
                throw new ArgumentException("ISBN must be either 10 or 13 digits long.");
            if (apiKey.Length != 39)
                throw new ArgumentException("API key must be 39 characters long.");

            var (mainUrl, item) = await GetMainUrlAsync(isbn, apiKey);
            //var html = await _httpClient.GetStringAsync(mainUrl);
            return ParseBookData(item, mainUrl);
        }

        private BookDto ParseBookData(JObject item, string infoUrl)
        {
            //var doc = new HtmlDocument();
            //doc.LoadHtml(html);

            //// Assuming you have parsed the JSON from the webpage already.
            //var metadata = new Dictionary<string, string>();

            //// Here you would parse the metadata from the JSON directly
            //var json = JObject.Parse(html);  // Assuming the HTML actually contains this JSON data, but it may be raw data.
            //var item = json["items"]?.FirstOrDefault();
            if (item == null)
                throw new Exception("Book data not found.");

            var volumeInfo = item["volumeInfo"];
            var saleInfo = item["saleInfo"];
            var accessInfo = item["accessInfo"];

            //Console.WriteLine("DTO DECLARE:::::::::::::::::::::");
            // Initialize the DTO with default values
            BookDto dto = new BookDto
            {
                Local_isbn = null,
                Title = null,
                Pages = 0,
                ReleaseDate = null,
                PublisherId = 0,
                LibrarianId = 0,
                InfoUrl = infoUrl,
                ContentSource = "GoogleBooks",

                // Optional fields with null defaults
                Type = null,
                Category = null,
                AdditionDate = null,
                Content = null,
                ContentLanguage = null,
                Image = null,
                Price = null,
                Duration = null,
                ContentLink = null,
                Format = null,
                publishingstatus = null,
                Weight = null,
                Dimensions = null,
                Material = null,
                Color = null
            };
            // Parse Title

            if (volumeInfo?["title"] != null)
                dto.Title = volumeInfo["title"].ToString();

            // Parse ISBNs (both 10 and 13 digits)
            if (volumeInfo?["industryIdentifiers"] != null)
            {
                var isbn10 = volumeInfo["industryIdentifiers"]
                                .FirstOrDefault(id => id["type"]?.ToString() == "ISBN_10")?["identifier"]?.ToString();
                var isbn13 = volumeInfo["industryIdentifiers"]
                                .FirstOrDefault(id => id["type"]?.ToString() == "ISBN_13")?["identifier"]?.ToString();

                if (!string.IsNullOrEmpty(isbn13))
                {
                    dto.Local_isbn = isbn13;
                    dto.Id = long.Parse(isbn13);
                }
                else if (!string.IsNullOrEmpty(isbn10))
                {
                    dto.Local_isbn = isbn10;
                    dto.Id = long.Parse(isbn10);
                }
            }

            // Parse Page Count
            if (volumeInfo?["pageCount"] != null)
                dto.Pages = (int)volumeInfo["pageCount"];

            // Parse Release Date
            if (volumeInfo?["publishedDate"] != null)
                dto.ReleaseDate = volumeInfo["publishedDate"].ToString();

            // Parse Language
            if (volumeInfo?["language"] != null)
                dto.ContentLanguage = volumeInfo["language"].ToString();

            // Parse Info Link
            if (volumeInfo?["infoLink"] != null)
                dto.InfoUrl = volumeInfo["infoLink"].ToString();

            // Parse Preview Link (Optional)
            if (volumeInfo?["previewLink"] != null)
                dto.ContentLink = volumeInfo["previewLink"].ToString();

            return dto;
        }

        public BookService(
            IBookRepository repository,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _repository = repository;
            _mapper = mapper;
            _env = env;
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetByISBNAsync(long isbn)
        {
            var book = await _repository.GetByISBNAsync(isbn);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetByNameAsync(string name)
        {
            var books = await _repository.GetByNameAsync(name); // Use repository
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task CreateAsync(BookDto dto)
        {
            var book = _mapper.Map<Book>(dto);
            await _repository.AddAsync(book);
        }

        public async Task UpdateAsync(long isbn, BookDto dto)
        {
            var existing = await _repository.GetByISBNAsync(isbn);
            if (existing == null) throw new Exception("Book not found");

            _mapper.Map(dto, existing);
            await _repository.UpdateAsync(existing);
        }

        public async Task DeleteAsync(long isbn)
        {
            var book = await _repository.GetByISBNAsync(isbn);
            if (book == null) throw new Exception("Book not found");

            await _repository.DeleteAsync(book);
        }
    }

}

