using Microsoft.AspNetCore.Mvc;
using MediatR;
using bookcatalogservice.Application.Genre.Dtos;
using bookcatalogservice.Application.Genre.Commands;
using bookcatalogservice.Application.Genre.Queries;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bookcatalogservice.Controllers
{
    [Route("api/v1/catalog/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize(Policy = "AuthorizedClient")]
        [HttpGet]
        public async Task<IActionResult> GetGenres([FromQuery] int page, int pageSize)
        {
            return Ok(await this._mediator.Send(new GetAllGenresQuery() { Page = page, PageSize = pageSize }));
        }

        [Authorize(Policy = "AuthorizedClient")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreCreateDto model)
        {
            return Ok(await this._mediator.Send(new CreateGenreCommand()
            {
                Name = model.Name,
                IsPopular = model.IsPopular
            }
            ).ConfigureAwait(false));
        }
    }
}

