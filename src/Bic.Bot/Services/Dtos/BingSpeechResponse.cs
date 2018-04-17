using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BIC.Bot.Services.Dtos
{
    public class BingSpeechResponse
    {
    
            public string RecognitionStatus { get; set; }
            public string DisplayText { get; set; }
            public int Offset { get; set; }
            public int Duration { get; set; }
    }
}