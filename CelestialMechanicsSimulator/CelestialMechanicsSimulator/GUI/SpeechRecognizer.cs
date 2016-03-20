using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Speech;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;

namespace CelestialMechanicsSimulator.GUI
{
    public class SpeechRecognizer
    {
        public Solarsystem Handler { get; private set; }
        public Dictionary<String, Action> CommandList { get; private set; }

        public bool IsRunning { get; set; }

        private SpeechRecognitionEngine speechRecognizer;
        private Thread recognizerThread;
        private Choices recognizerChoices;

        //TODO: IMPLEMENT REDO UNDO:: SEE STACK<T>

        //TEST::
        private string lastSentence;

        public SpeechRecognizer(Solarsystem handler)
        {
            this.Handler = handler;
            this.CommandList = new Dictionary<string, Action>();

            speechRecognizer = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("de-DE"));
            speechRecognizer.SetInputToDefaultAudioDevice();

            recognizerChoices = new Choices();
            recognizerChoices.Add(new string[] { "Erneut" });
        }

        public void Run()
        {
            GrammarBuilder gb = new GrammarBuilder(recognizerChoices);
            Grammar g = new Grammar(gb);
            speechRecognizer.LoadGrammar(g);

            speechRecognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>((object sender, SpeechRecognizedEventArgs e) =>
            {
                foreach (var data in CommandList)
                {
                    if (data.Key == e.Result.Text)
                    {
                        data.Value.Invoke();
                        lastSentence = e.Result.Text;
                        break;
                    }
                }

                if (e.Result.Text == "Erneut")
                    CommandList[lastSentence].Invoke();
            });

            IsRunning = true;
            if (recognizerThread == null)
            {
                recognizerThread = new Thread(new ThreadStart(() =>
                {
                    while (IsRunning)
                    {
                        speechRecognizer.Recognize();
                    }
                }));
                recognizerThread.Start();
            }
            else throw new Exception("Recognizer is already running...");
        }

        public void Stop()
        {
            recognizerThread.Abort();
            IsRunning = false;
            recognizerThread = null;
        }

        public void AddCommand(string key, Action task)
        {
            if (!CommandList.Keys.Contains(key) && !IsRunning)
            {
                CommandList.Add(key, task);
                recognizerChoices.Add(new string[] { key });
            }
        }


    }
}
