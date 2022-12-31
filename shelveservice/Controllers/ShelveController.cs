using Consul;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using shelveservice.Application.Shelve.Commands;
using shelveservice.Application.Shelve.Dtos;

namespace shelveservice.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShelveController : ControllerBase
    {

        private readonly IMediator _mediator;

        public ShelveController(IMediator mediator)
        {
            _mediator = mediator;
        }     

        [HttpPost]
        [Authorize(Policy = "AuthorizedClient")]
        public async Task<IActionResult> Post([FromBody] ShelveCreateDto model)
        {
            return Ok(await this._mediator.Send(new CreateShelveCommand()
            {
                UserId = model.UserId,                
                Name= model.Name
            }
            ).ConfigureAwait(false));           
        }
    }
}