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

namespace CleanArchitecture.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _repository;
        private readonly IMapper _mapper;
        private static readonly HttpClient _httpClient = new HttpClient();

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




        public BookService(IBookRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

