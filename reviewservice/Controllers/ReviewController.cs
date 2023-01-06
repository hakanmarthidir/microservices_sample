using Consul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using reviewservice.Application.Review.Commands;
using reviewservice.Application.Review.Dtos;
using reviewservice.Application.Review.Queries;

namespace reviewservice.Controllers
{
    [Authorize(Policy = "AuthorizedClient")]
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
        public async Task<IActionResult> Post([FromBody] ReviewCreateDto model)
        {
            Request.Headers.TryGetValue("Authorization", out var authorization);

            return Ok(await this._mediator.Send(new CreateReviewCommand()
            {
                Token = authorization,
                BookId = model.BookId,
                Rating = model.Rating,
                Comment= model.Comment,
                DateRead = model.DateRead
            }
            ).ConfigureAwait(false));           
        }

        [HttpGet("reviewedbooks/{page}/{pagesize}")]              
        public async Task<IActionResult> GetReviewedBooks(int page,  int pagesize)
        {
            //TODO: handle with middleware or action filter [FromHeader] string authorization
            Request.Headers.TryGetValue("Authorization", out var authorization);

            return Ok(await this._mediator.Send(new ReviewedBookListQuery() { 
                Page = page, 
                PageSize=pagesize, 
                Token= authorization 
            }).ConfigureAwait(false));

        }

        [HttpGet("reviewedbooksgrpc/{page}/{pagesize}")]     
        public async Task<IActionResult> GetReviewedBooksGrpc(int page, int pagesize)
        {            
            Request.Headers.TryGetValue("Authorization", out var authorization);
            return Ok(await this._mediator.Send(new ReviewedBookListGrpcQuery()
            {
                Page = page,
                PageSize = pagesize,
                Token = authorization
            }).ConfigureAwait(false));

        }
    }
}