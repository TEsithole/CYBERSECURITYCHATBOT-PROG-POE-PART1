using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace CyberSecurityChatbot
{
    class Program
    {
        private static ChatbotService? _chatbotService;
        private static UIService? _uiService;

        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.WindowWidth = 120;
            Console.WindowHeight = 50;

            // Initialize services
            _chatbotService = new ChatbotService();
            _uiService = new UIService();

            // Play voice greeting
            PlayVoiceGreeting();

            // Display ASCII art
            _uiService.DisplayLogo();

            // Welcome user
            WelcomeUser();

            // Start conversation loop
            StartChatLoop();
        }

        private static void PlayVoiceGreeting()
        {
            try
            {
                // Try multiple paths for the audio file
                string[] possiblePaths = new string[]
                {
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "greeting.wav"),
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources", "greeting.wav")
                };

                string audioPath = null;
                foreach (string path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        audioPath = path;
                        break;
                    }
                }

                if (audioPath != null)
                {
                    using (var player = new SoundPlayer(audioPath))
                    {
                        player.PlaySync();
                    }
                }
                else
                {
                    _uiService?.DisplayColoredText("🔊 Audio file not found. Continuing without audio...", ConsoleColor.Yellow);
                    _uiService?.DisplayColoredText("   (Place greeting.wav in the Resources folder for voice greeting)", ConsoleColor.DarkYellow);
                }
            }
            catch (Exception ex)
            {
                _uiService?.DisplayColoredText($"⚠️ Error playing audio: {ex.Message}", ConsoleColor.Yellow);
            }
        }

        private static void WelcomeUser()
        {
            _uiService?.DisplayColoredText("\n═══════════════════════════════════════════════════════════════════════════════", ConsoleColor.Cyan);
            _uiService?.DisplayColoredText("              WELCOME TO THE CYBERSECURITY AWARENESS BOT", ConsoleColor.Cyan);
            _uiService?.DisplayColoredText("═══════════════════════════════════════════════════════════════════════════════", ConsoleColor.Cyan);
            Console.WriteLine();
            Console.WriteLine("🔐 Your trusted companion for online safety in South Africa 🔐");
            Console.WriteLine();

            Console.Write("Please enter your name: ");
            string userName = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(userName))
            {
                _uiService?.DisplayColoredText("⚠️ Name cannot be empty. Please enter your name: ", ConsoleColor.Yellow);
                userName = Console.ReadLine();
            }

            _chatbotService?.SetUserName(userName);

            Console.WriteLine();
            _uiService?.DisplayColoredText($"👋 Hello, {userName}! Welcome to the Cybersecurity Awareness Bot!", ConsoleColor.Green);
            Console.WriteLine();

            _uiService?.TypingEffect($"💬 I'm here to help you stay safe online in South Africa, {userName}.", ConsoleColor.Cyan, 30);
            Console.WriteLine();
            _uiService?.TypingEffect("📚 You can ask me about password safety, phishing scams, and safe browsing practices.", ConsoleColor.Cyan, 30);
            Console.WriteLine();
            _uiService?.TypingEffect("💡 Type 'exit' or 'quit' to end the conversation.", ConsoleColor.Cyan, 30);
            Console.WriteLine();
        }

        private static void StartChatLoop()
        {
            bool isRunning = true;

            while (isRunning)
            {
                _uiService?.DisplaySectionHeader("💬 Chat with the Cybersecurity Bot");
                Console.Write("You: ");

                string userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    _uiService?.TypingEffect("🤔 I didn't quite understand that. Could you rephrase?", ConsoleColor.Yellow, 30);
                    continue;
                }

                string lowerInput = userInput.ToLower().Trim();
                if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "goodbye" || lowerInput == "bye")
                {
                    Console.WriteLine();
                    _uiService?.DisplayColoredText("═══════════════════════════════════════════════════════════════════════════════", ConsoleColor.Green);
                    _uiService?.DisplayColoredText("👋 Thank you for using the Cybersecurity Awareness Bot!", ConsoleColor.Green);
                    _uiService?.TypingEffect("🛡️ Remember: Stay safe, stay aware, stay secure online!", ConsoleColor.Cyan, 30);
                    Console.WriteLine();
                    _uiService?.DisplayColoredText("🌟 Goodbye! Have a safe day! 🌟", ConsoleColor.Green);
                    _uiService?.DisplayColoredText("═══════════════════════════════════════════════════════════════════════════════", ConsoleColor.Green);
                    isRunning = false;
                    continue;
                }

                string response = _chatbotService?.GetResponse(userInput) ?? "I'm not sure how to respond to that.";

                Console.WriteLine();
                _uiService?.TypingEffect($"🤖 Cybersecurity Bot: {response}", ConsoleColor.Magenta, 25);
                Console.WriteLine();

                // Show helpful tips based on topic
                if (lowerInput.Contains("password") || lowerInput.Contains("passwords"))
                {
                    _uiService?.DisplayColoredText("💡 Tip: Use a password manager to generate and store strong passwords securely!", ConsoleColor.Cyan);
                }
                else if (lowerInput.Contains("phishing") || lowerInput.Contains("scam"))
                {
                    _uiService?.DisplayColoredText("💡 Tip: When in doubt, don't click! Verify the sender's email address first.", ConsoleColor.Cyan);
                }
                else if (lowerInput.Contains("browsing") || lowerInput.Contains("browser"))
                {
                    _uiService?.DisplayColoredText("💡 Tip: Always look for the padlock icon (🔒) in the address bar for secure sites.", ConsoleColor.Cyan);
                }
                Console.WriteLine();
            }
        }
    }

    // ===== UI SERVICE =====
    public class UIService
    {
        public void DisplayLogo()
        {
            string logo = @"
    ╔══════════════════════════════════════════════════════════════════════════════════════╗
    ║                                                                                      ║
    ║     ██████╗██╗   ██╗██████╗ ███████╗██████╗ ███████╗███████╗██╗  ██╗               ║
    ║    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗██╔════╝██╔════╝██║  ██║               ║
    ║    ██║      ╚████╔╝ ██████╔╝███████╗██████╔╝███████╗███████╗███████║               ║
    ║    ██║       ╚██╔╝  ██╔══██╗╚════██║██╔══██╗╚════██║╚════██║██╔══██║               ║
    ║    ╚██████╗   ██║   ██████╔╝███████║██║  ██║███████║███████║██║  ██║               ║
    ║     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝╚══════╝╚══════╝╚═╝  ╚═╝               ║
    ║                                                                                      ║
    ║              🔐  CYBERSECURITY AWARENESS BOT  🔐                                    ║
    ║                 Protecting South Africans Online                                     ║
    ║                                                                                      ║
    ║     ╔═══════════════════════════════════════════════════════════════════════════╗    ║
    ║     ║  🇿🇦  South Africa's Cybersecurity Education Initiative  🇿🇦           ║    ║
    ║     ╚═══════════════════════════════════════════════════════════════════════════╝    ║
    ║                                                                                      ║
    ╚══════════════════════════════════════════════════════════════════════════════════════╝";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(logo);
            Console.ResetColor();
            Thread.Sleep(800);
        }

        public void DisplayColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public void DisplaySectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\n{new string('═', 70)}");
            Console.WriteLine($"  {title}");
            Console.WriteLine($"{new string('═', 70)}");
            Console.ResetColor();
        }

        public void TypingEffect(string message, ConsoleColor color, int delayMs = 20)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delayMs);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public void DisplayBorderedMessage(string message, ConsoleColor borderColor = ConsoleColor.Cyan)
        {
            int padding = 4;
            int width = message.Length + padding * 2;

            Console.ForegroundColor = borderColor;
            Console.WriteLine($"┌{new string('─', width)}┐");
            Console.WriteLine($"│{new string(' ', padding)}{message}{new string(' ', padding)}│");
            Console.WriteLine($"└{new string('─', width)}┘");
            Console.ResetColor();
        }
    }

    // ===== CHATBOT SERVICE =====
    public class ChatbotService
    {
        private string? _userName;
        private Dictionary<string, List<string>>? _responses;
        private Dictionary<string, string>? _keywords;
        private Random? _random;

        public ChatbotService()
        {
            _random = new Random();
            InitializeResponses();
            InitializeKeywords();
        }

        private void InitializeResponses()
        {
            _responses = new Dictionary<string, List<string>>
            {
                ["greeting"] = new List<string>
                {
                    "Hello! I'm feeling great, thank you for asking! How can I help you stay safe online today?",
                    "I'm doing well! Cybersecurity is my passion, and I'm here to help you.",
                    "I'm fantastic! Ready to share some important cybersecurity tips with you!"
                },
                ["purpose"] = new List<string>
                {
                    "My purpose is to help South Africans stay safe online by providing cybersecurity education and awareness. In South Africa, cybercrime is on the rise, and I'm here to help you protect yourself!",
                    "I'm here to educate you about cybersecurity threats and how to protect yourself online. South Africa faces unique cybersecurity challenges.",
                    "My goal is to make cybersecurity knowledge accessible to everyone in South Africa. Knowledge is power when it comes to staying safe online!"
                },
                ["what_ask"] = new List<string>
                {
                    "You can ask me about password safety, phishing scams, safe browsing, or general cybersecurity tips. I'm especially knowledgeable about threats facing South Africans!",
                    "I can help with topics like creating strong passwords, identifying phishing emails, and safe online practices. Feel free to ask anything about digital safety!",
                    "Ask me about cybersecurity threats, prevention tips, or how to stay safe on the internet. I'm here to help you become more cyber-aware!"
                },
                ["password"] = new List<string>
                {
                    "🔐 Password safety is crucial! Use strong, unique passwords for each account and enable two-factor authentication. Never share your passwords with anyone!",
                    "Create passwords with at least 12 characters, including uppercase, lowercase, numbers, and special characters. Consider using a password manager to keep track of them securely.",
                    "South Africans are often targeted by password attacks. Always use different passwords for different accounts and change them regularly. Use 2FA whenever possible!"
                },
                ["phishing"] = new List<string>
                {
                    "🎣 Phishing attacks try to trick you into sharing sensitive information. Always verify email senders before clicking links. In South Africa, phishing scams often target banking details.",
                    "Look out for suspicious emails asking for personal information. Legitimate companies never ask for passwords via email. Check for spelling mistakes and unusual sender addresses.",
                    "Phishing is a major threat in South Africa. Always check the email address, hover over links before clicking, and never share personal information through email or SMS."
                },
                ["browsing"] = new List<string>
                {
                    "🌐 Safe browsing involves using HTTPS websites, avoiding suspicious downloads, and keeping your browser updated. Always use secure websites (https), avoid suspicious downloads, and keep your browser updated.",
                    "Use ad-blockers and privacy extensions. Be cautious of pop-ups and avoid entering personal information on unsecured sites. Look for the padlock icon (🔒) in your browser!",
                    "In South Africa, mobile browsing is common. Always download apps from official stores, avoid public Wi-Fi for sensitive transactions, and keep your devices updated."
                },
                ["south_africa"] = new List<string>
                {
                    "🇿🇦 South Africa faces unique cybersecurity challenges. Common threats include banking fraud, phishing scams, and social engineering attacks. Always stay vigilant!",
                    "South Africa has seen a significant rise in cyberattacks targeting individuals and businesses. The Department of Cybersecurity is working to educate citizens about these threats.",
                    "As a South African, you're part of a growing digital community. Protect yourself by staying informed about local scams and cyber threats affecting our country."
                },
                ["default"] = new List<string>
                {
                    "I didn't quite understand that. Could you rephrase your question? Try asking about password safety, phishing, or safe browsing.",
                    "I'm not sure about that, but I specialize in cybersecurity topics. Would you like to know about password safety, phishing, or safe browsing?",
                    "That's an interesting question! I focus on cybersecurity awareness. You might want to ask about creating strong passwords, spotting phishing emails, or safe browsing habits."
                }
            };
        }

        private void InitializeKeywords()
        {
            _keywords = new Dictionary<string, string>
            {
                ["how are you"] = "greeting",
                ["how are you doing"] = "greeting",
                ["what is your purpose"] = "purpose",
                ["why do you exist"] = "purpose",
                ["what can i ask you"] = "what_ask",
                ["what can you help with"] = "what_ask",
                ["what do you do"] = "purpose",
                ["password"] = "password",
                ["passwords"] = "password",
                ["phishing"] = "phishing",
                ["scam"] = "phishing",
                ["scams"] = "phishing",
                ["browsing"] = "browsing",
                ["safe browsing"] = "browsing",
                ["browser"] = "browsing",
                ["south africa"] = "south_africa",
                ["south african"] = "south_africa",
                ["sa"] = "south_africa"
            };
        }

        public void SetUserName(string name)
        {
            _userName = name;
        }

        public string GetResponse(string input)
        {
            string lowerInput = input.ToLower().Trim();

            // Check for keyword matches
            if (_keywords != null)
            {
                foreach (var keyword in _keywords)
                {
                    if (lowerInput.Contains(keyword.Key))
                    {
                        if (_responses != null && _responses.ContainsKey(keyword.Value))
                        {
                            var responses = _responses[keyword.Value];
                            if (responses != null && responses.Count > 0 && _random != null)
                            {
                                return responses[_random.Next(responses.Count)];
                            }
                        }
                    }
                }
            }

            // Check if it's a greeting
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                return $"Hello {_userName}! How can I help you with cybersecurity today?";
            }

            // Check if it's a thank you
            if (lowerInput.Contains("thank") || lowerInput.Contains("thanks"))
            {
                return $"You're welcome, {_userName}! Remember, staying safe online is everyone's responsibility. Is there anything else I can help you with?";
            }

            return GetDefaultResponse();
        }

        private string GetDefaultResponse()
        {
            if (_responses != null && _responses.ContainsKey("default") && _random != null)
            {
                var defaultResponses = _responses["default"];
                if (defaultResponses != null && defaultResponses.Count > 0)
                {
                    return defaultResponses[_random.Next(defaultResponses.Count)];
                }
            }
            return "I'm not sure how to respond to that. Please ask about cybersecurity topics like passwords, phishing, or safe browsing.";
        }
    }
}