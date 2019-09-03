using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnABot.Translator
{
    public class TextTranslatorEndpoint
    {
        public string subscriptionKey { get; set; }
        public string endpoint { get; set; }
        public string route { get; set; }
    }
}
