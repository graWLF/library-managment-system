using AutoMapper;
using CleanArchitecture.Core.DTOs.Author;
using CleanArchitecture.Core.DTOs.Book;
using CleanArchitecture.Core.DTOs.Borrower;
using CleanArchitecture.Core.DTOs.Borrowing;
using CleanArchitecture.Core.DTOs.Branch;
using CleanArchitecture.Core.DTOs.Librarian;
using CleanArchitecture.Core.DTOs.Publisher;
using CleanArchitecture.Core.DTOs.Registration;
using CleanArchitecture.Core.DTOs.Supervisor;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Features.Categories.Queries.GetAllCategories;
using CleanArchitecture.Core.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Core.Features.Products.Queries.GetAllProducts;

namespace CleanArchitecture.Core.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
            CreateMap<GetAllCategoriesQuery, GetAllCategoriesParameter>();
            CreateMap<Category, GetAllCategoriesViewModel>().ReverseMap();
            // Book
            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>()
                .ForMember(dest => dest.ID, opt => opt.Ignore());

            // Author
            CreateMap<Author, AuthorDTO>();
            CreateMap<AuthorDTO, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Borrower
            CreateMap<Borrower, BorrowerDTO>();
            CreateMap<BorrowerDTO, Borrower>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Borrowing
            CreateMap<Borrowing, BorrowingDTO>();
            CreateMap<BorrowingDTO, Borrowing>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Branch
            CreateMap<Branch, BranchDTO>();
            CreateMap<BranchDTO, Branch>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Librarian
            CreateMap<Librarian, LibrarianDTO>();
            CreateMap<LibrarianDTO, Librarian>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Publisher
            CreateMap<Publisher, PublisherDTO>();
            CreateMap<PublisherDTO, Publisher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Supervisor
            CreateMap<Supervisor, SupervisorDTO>();
            CreateMap<SupervisorDTO, Supervisor>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
            
            // Registration
            CreateMap<Registration, RegistrationDTO>();
            CreateMap<RegistrationDTO, Registration>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
