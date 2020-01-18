using Codity.Services.ResponseModels.Interfaces;

namespace Codity.Services.ResponseModels.DTOs.Shared
{
    public class ProgrammingLanguageDTO : IResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
