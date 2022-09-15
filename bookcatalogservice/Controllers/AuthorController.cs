using Microsoft.AspNetCore.Mvc;
using MediatR;
using bookcatalogservice.Application.Author.Queries;
using bookcatalogservice.Application.Author.Dtos;
using bookcatalogservice.Application.Author.Commands;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bookcatalogservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AuthorizedClient")]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors([FromQuery] int page, int pageSize)
        {
            return Ok(await this._mediator.Send(new GetAllAuthorsQuery() { Page = page, PageSize = pageSize }));
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthorCreateDto model)
        {
            return Ok(await this._mediator.Send(new CreateAuthorCommand()
            {
                Name = model.Name,
                HasNobel = model.HasNobel
            }
            ).ConfigureAwait(false));
        }
    }
}

