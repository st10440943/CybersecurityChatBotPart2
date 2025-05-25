using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using NAudio.Wave;

namespace CybersecurityChatBot
{
    internal class Program
    {
        static string userInterest = "";

        static void Main()
        {
            PlayGreetingAudio("cyber_greeting.wav");

            Console.Title = "Cybersecurity Awareness Bot";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"   _______     __     ______     ______     ______     ______   __  __      
     / ___  /\   /\ \   / ____/\   /\  __ \   /\  ___\   /\__  _\ /\ \_\ \     
  \/__/\ \/   \ \ \ / /\__\//   \ \ \/\ \  \ \  __\   \/_/\ \/ \ \  __ \       
      _\ \ \    \ \ \\\_\__/      \ \_____\  \ \_____\    \ \_\  \ \_\ \_\      
      \/_/ /     \ \/_/\/__/       \/_____/   \/_____/     \/_/   \/_/\/_/       
           /\_\       \/_/                                                         
           \/_/                                                                  
              C Y B E R S E C U R I T Y   A W A R E N E S S   B O T ");

            Console.Write("Hey there! What's your name? ");
            Console.ForegroundColor = ConsoleColor.White;
            string userName = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(userName)) userName = "User";

            Console.ForegroundColor = ConsoleColor.Green;
            TypeText($"\nWelcome, {userName}! I'm your Cybersecurity Awareness Bot.");
            TypeText("Before we start, what is your cybersecurity experience level? (beginner / intermediate / pro)");

            string level = Console.ReadLine()?.ToLower().Trim();
            ShowTipsByLevel(level);

            TypeText("\nAsk me anything about staying safe online, or type 'exit' to leave.\n");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{userName}: ");
                string input = Console.ReadLine()?.ToLower().Trim();

                if (input == "exit" || input == "quit")
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    TypeText("Bot: It was nice chatting with you. Stay safe online!");
                    break;
                }

                if (string.IsNullOrEmpty(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    TypeText("Bot: Please provide a valid question. I didn't quite catch that.");
                    continue;
                }

                if (input.Contains("interested in privacy"))
                {
                    userInterest = "privacy";
                    TypeText("Bot: Great! I'll remember that you're interested in privacy.", ConsoleColor.Yellow);
                    continue;
                }

                HandleUserQuery(input, userName);
            }
        }

        static void HandleUserQuery(string input, string userName)
        {
            Dictionary<string, string> responses = new Dictionary<string, string>
            {
                {"how are you", "I'm fully patched and running smoothly! Always ready to help you stay safe online." },
                {"what is your purpose", "I'm here to help you stay safe online and raise awareness about potential threats." },
                {"what can i ask you about", "You can ask about phishing, strong passwords, 2FA, malware, VPNs, scams, or privacy." },
                {"strong passwords", "Use a mix of uppercase, lowercase, numbers, and symbols. Avoid repeating passwords!" },
                {"password", "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords." },
                {"two-factor authentication", "2FA adds extra security by requiring both your password and a code from your phone." },
                {"cyber tips", "Keep software updated, avoid public Wi-Fi for sensitive tasks, and never overshare personal info." },
                {"malware", "Malware can infect your system silently. Use antivirus tools and avoid unknown downloads." },
                {"encryption", "Encryption keeps your data safe from intruders. Always prefer secure websites (https)." },
                {"vpn", "VPNs protect your internet traffic from spying, especially on public Wi-Fi." },
                {"social engineering", "Scammers use manipulation to trick people into giving up sensitive data. Be skeptical!" },
                {"scam", "Online scams come in many forms. Be wary of too-good-to-be-true deals or urgent requests." },
                {"privacy", "Control app permissions and limit what you share online. Privacy is key to security." }
            };

            // Sentiment Detection
            if (input.Contains("worried"))
            {
                TypeText("Bot: It's okay to feel worried. Cybersecurity can seem scary, but I'm here to help.");
                return;
            }
            else if (input.Contains("frustrated"))
            {
                TypeText("Bot: I understand. Let’s take it one step at a time. Ask me anything.");
                return;
            }
            else if (input.Contains("curious"))
            {
                TypeText("Bot: Curiosity is a great mindset for cybersecurity. What are you curious about?");
                return;
            }

            // Random Response for "phishing"
            if (input.Contains("phishing"))
            {
                string[] tips = {
                    "Be cautious of emails asking for personal info. Look closely at URLs and sender addresses.",
                    "Never click on suspicious links, especially from unknown senders.",
                    "Real companies won't ask you to verify sensitive info by email.",
                    "Hover over links before clicking. Check for misleading domains."
                };
                Random r = new Random();
                TypeText($"Bot: {tips[r.Next(tips.Length)]}");
                return;
            }

            // Keyword containment check in dictionary keys
            foreach (var key in responses.Keys)
            {
                if (input.Contains(key))
                {
                    TypeText($"Bot: {responses[key]}");

                    if (!string.IsNullOrEmpty(userInterest) && input.Contains(userInterest))
                    {
                        TypeText($"Bot: Since you're interested in {userInterest}, make sure your social accounts have strong privacy settings.");
                    }
                    return;
                }
            }

            // Fallback when no known keyword found
            TypeText($"Bot: I'm not sure about that, {userName}. Would you like some cybersecurity tips? (yes/no)");
            string reply = Console.ReadLine()?.ToLower().Trim();
            if (reply == "yes")
            {
                ShareCyberTips();
            }
            else
            {
                TypeText("Bot: No problem! I'm here if you need help.");
            }
        }

        static void ShareCyberTips()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            TypeText("Bot: Here are some basic cybersecurity tips:");
            TypeText("- Use strong, unique passwords.");
            TypeText("- Be cautious of suspicious emails and links.");
            TypeText("- Enable two-factor authentication.");
            TypeText("- Keep your software and antivirus up to date.\n");
        }

        static void ShowTipsByLevel(string level)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            switch (level)
            {
                case "beginner":
                    TypeText("Bot: Let's start with the basics:");
                    TypeText("- Don’t reuse passwords.");
                    TypeText("- Watch out for phishing scams.");
                    TypeText("- Use antivirus software.");
                    break;
                case "intermediate":
                    TypeText("Bot: You're doing great! Here's more:");
                    TypeText("- Use a password manager.");
                    TypeText("- Enable 2FA.");
                    TypeText("- Stay updated on social engineering tactics.");
                    break;
                case "pro":
                    TypeText("Bot: You're a cybersecurity pro!");
                    TypeText("- Audit your digital presence.");
                    TypeText("- Use encrypted communication tools.");
                    TypeText("- Explore ethical hacking in labs.");
                    break;
                default:
                    TypeText("Bot: No problem. Here’s how to stay safe:");
                    ShareCyberTips();
                    break;
            }
            Console.WriteLine();
        }

        static void PlayGreetingAudio(string filePath)
        {
            try
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (File.Exists(fullPath))
                {
                    SoundPlayer player = new SoundPlayer(fullPath);
                    player.PlaySync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: The file '{filePath}' was not found.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing audio: {ex.Message}");
            }
        }

        static void TypeText(string message, ConsoleColor color = ConsoleColor.Magenta, int delay = 25)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
