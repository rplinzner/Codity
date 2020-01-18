using System.Collections.Generic;

namespace Codity.Services.Helpers
{
    public class GistModel
    {
        public string Description { get; set; }
        public bool Public { get; set; }
        public Dictionary<string, object> Files { get; set; }
    }
}
