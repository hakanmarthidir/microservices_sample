using Consul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reviewservice.Application.Review.Commands;
using reviewservice.Application.Review.Dtos;

namespace reviewservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }     

        [HttpPost]
        [Authorize(Policy = "AuthorizedClient")]
        public async Task<IActionResult> Post([FromBody] ReviewCreateDto model)
        {
            return Ok(await this._mediator.Send(new CreateReviewCommand()
            {
                UserId = model.UserId,
                BookId = model.BookId,
                Rating = model.Rating,
                Comment= model.Comment
            }
            ).ConfigureAwait(false));           
        }
    }
}