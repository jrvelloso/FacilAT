using FaturasHandler.Data.Models;
using FaturasHandler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FaturasHandler.WebAPI.Controllers
{
    [ApiController]
    [Route("api/declaracoes-iva")]
    public class DeclaracoesIvaController : ControllerBase
    {
        private readonly IIVADeclarationService _declaracaoIvaService;
        public DeclaracoesIvaController(IIVADeclarationService declaracaoIvaService) => _declaracaoIvaService = declaracaoIvaService;

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetDeclaracoesIva(Guid userId, CancellationToken cancellationToken) =>
            Ok(await _declaracaoIvaService.GetAllByUserAsync(userId, cancellationToken));

        [HttpPost]
        public async Task<IActionResult> CreateDeclaracaoIva([FromBody] IVADeclaration declaracao, Guid userId, CancellationToken cancellationToken)
        {
            var result = await _declaracaoIvaService.AddAsync(declaracao, userId, cancellationToken);
            return CreatedAtAction(nameof(GetDeclaracoesIva), new { userId = declaracao.UserId }, result);
        }
    }
}