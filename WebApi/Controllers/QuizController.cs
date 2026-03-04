using Microsoft.AspNetCore.Mvc;
using MyLittlePony_Conexy.Application.Models;
using MyLittlePony_Conexy.Application.Services;

namespace MyLittlePony_Conexy.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuizController(IQuizService quizService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<QuestionDto>>> GetQuiz(CancellationToken cancellationToken)
    {
        var questions = await quizService.GetQuizAsync(cancellationToken);
        return Ok(questions);
    }

    [HttpPost("result")]
    public async Task<ActionResult<QuizResultDto>> CalculateResult(
        [FromBody] QuizResultRequestDto request,
        CancellationToken cancellationToken)
    {
        if (request.SelectedOptionIds is null || request.SelectedOptionIds.Count == 0)
        {
            return BadRequest("At least one option id must be provided.");
        }

        var result = await quizService.CalculateResultAsync(request.SelectedOptionIds, cancellationToken);
        return Ok(result);
    }
}

