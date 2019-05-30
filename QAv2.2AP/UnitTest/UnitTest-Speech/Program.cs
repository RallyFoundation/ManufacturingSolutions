using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace UnitTest_Speech
{
    class Program
    {
        static void Main(string[] args)
        {
            string content = "Max value is less than the min value of built-in rule.";

            SpeakOutText(content, "");

            SpeakOutTextAsync(content, "");

            Console.WriteLine(content);

            Console.Read();
        }

        static void SpeakOutText(string TextToSpeak, string VoiceName)
        {
            using (SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer())
            {
                if (!String.IsNullOrEmpty(VoiceName))
                {
                    speechSynthesizer.SelectVoice(VoiceName);
                }

                speechSynthesizer.SetOutputToDefaultAudioDevice();

                speechSynthesizer.Speak(TextToSpeak);
            }
        }

        static void SpeakOutTextAsync(string TextToSpeak, string VoiceName)
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

            if (!String.IsNullOrEmpty(VoiceName))
            {
                speechSynthesizer.SelectVoice(VoiceName);
            }

            speechSynthesizer.SetOutputToDefaultAudioDevice();

            speechSynthesizer.SpeakAsync(TextToSpeak);
        }
    }
}
