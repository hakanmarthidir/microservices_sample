using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using bookcatalogservice.Application.Book.Queries;
using bookcatalogservice.Application.Book.Dtos;
using bookcatalogservice.Application.Book.Commands;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bookcatalogservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] int page, int pageSize)
        {
            return Ok(await this._mediator.Send(new GetAllBooksQuery() { Page = page, PageSize = pageSize }));
        }

        // GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BookCreateDto model)
        {
            return Ok(await this._mediator.Send(new CreateBookCommand()
            {
                AuthorId = model.AuthorId,
                FirstPublishedDate = model.FirstPublishedYear,
                GenreId = model.GenreId,
                Name = model.BookName
            }).ConfigureAwait(false));
        }
    }
}

