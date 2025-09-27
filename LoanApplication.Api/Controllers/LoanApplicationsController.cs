using LoanApplication.Api.Models;
using LoanApplication.Application.Commands;
using LoanApplication.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplication.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoanApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<LoanApplicationsController> _logger;

        public LoanApplicationsController(IMediator mediator, ILogger<LoanApplicationsController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitApplication([FromBody] LoanApplicationRequest request)
        {
            _logger.LogInformation("Received loan application submission for {ApplicantName}", request.ApplicantName);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid loan application request: {Errors}", string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return BadRequest(ModelState);
            }

            var command = new SubmitLoanApplicationCommand
            {
                ApplicantName = request.ApplicantName,
                ApplicantIncome = request.ApplicantIncome,
                LoanAmount = request.LoanAmount,
                LoanTermMonths = request.LoanTermMonths,
                CountryCode = request.CountryCode
            };

            var loanApplicationId = await _mediator.Send(command);

            var response = new LoanApplicationCreatedResponse { loanApplicationId = loanApplicationId };
            return CreatedAtAction(nameof(GetApplication), new { id = loanApplicationId }, response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplication(Guid id)
        {
            _logger.LogInformation("Retrieving loan application with ID: {Id}", id);

            var query = new GetLoanApplicationQuery(id);
            var application = await _mediator.Send(query);

            if (application == null)
            {
                _logger.LogWarning("Loan application with ID {Id} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Successfully retrieved loan application with ID: {Id}", id);

            return Ok(application);
        }
    }
}
