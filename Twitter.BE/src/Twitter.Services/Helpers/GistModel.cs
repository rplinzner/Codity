using System.Collections.Generic;

namespace Twitter.Services.Helpers
{
    public class GistModel
    {
        public string Description { get; set; }
        public bool Public { get; set; }
        public Dictionary<string, object> Files { get; set; }
    }
}
