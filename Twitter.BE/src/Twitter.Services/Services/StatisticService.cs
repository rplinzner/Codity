using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;
using Twitter.Services.Interfaces;

namespace Twitter.Services.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly ITweetRepository _tweetRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<ProgrammingLanguage> _programmingLanguageRepository;
        private readonly IEmailSenderService _emailSenderService;

        public StatisticService(
            ITweetRepository tweetRepository,
            IUserRepository userRepository,
            IBaseRepository<ProgrammingLanguage> programmingLanguageRepository,
            IEmailSenderService emailSenderService)
        {
            _tweetRepository = tweetRepository;
            _userRepository = userRepository;
            _programmingLanguageRepository = programmingLanguageRepository;
            _emailSenderService = emailSenderService;
        }

        public async Task SendWeeklyStatisticSummary()
        {
            var programmingLanguages = await _programmingLanguageRepository.GetAllAsync();
            var lastWeekTweets = await _tweetRepository.GetAllByAsync(c => c.CreationDate > DateTime.Now.AddDays(-7),
                                                                      false, c => c.Likes, c => c.Comments,
                                                                      c => c.CodeSnippet);
            var statisticDictionary = new Dictionary<string, (int, int, int)>();

            foreach (var programmingLanguage in programmingLanguages)
            {
                var tweetCounter = 0;
                var commentCounter = 0;
                var likeCounter = 0;

                foreach (var tweet in lastWeekTweets)
                {
                    if (tweet.Text.ToLower().Contains(programmingLanguage.Name.ToLower())
                        || tweet.CodeSnippet.ProgrammingLanguageId == programmingLanguage.Id)
                    {
                        tweetCounter++;
                        commentCounter += tweet.Comments.Count;
                        likeCounter += tweet.Likes.Count;
                    }
                }

                statisticDictionary.Add(programmingLanguage.Name, (tweetCounter, commentCounter, likeCounter));
            }

            var statisticMessage = new StringBuilder();
            int counter = 1;

            statisticMessage.AppendLine("<strong>The largest number of tweets by programming language</strong>");

            foreach (var tweet in statisticDictionary.OrderByDescending(c => c.Value.Item1))
            {
                statisticMessage.AppendLine($"<p>{counter++}. {tweet.Key} - {tweet.Value.Item1} tweets</p>");
            }

            counter = 1;
            statisticMessage.AppendLine("<br><strong>The largest number of comments by programming language</strong>");

            foreach (var comment in statisticDictionary.OrderByDescending(c => c.Value.Item2))
            {
                statisticMessage.AppendLine($"<p>{counter++}. {comment.Key} - {comment.Value.Item2} comments</p>");

            }

            counter = 1;
            statisticMessage.AppendLine("<br><strong>The largest number of likes by programming language</strong>");

            foreach (var like in statisticDictionary.OrderByDescending(c => c.Value.Item3))
            {
                statisticMessage.AppendLine($"<p>{counter++}. {like.Key} - {like.Value.Item3} likes</p>");
            }

            var title = $"Weekly statistics {DateTime.Now.AddDays(-7).ToShortDateString()} - {DateTime.Now.ToShortDateString()}";

            var users = await _userRepository.GetAllAsync();

            //foreach (var user in users)
            //{
            //    await _emailSenderService.SendEmail(user.Email, title, statisticMessage.ToString());
            //}
            await _emailSenderService.SendEmail("damiansalata10@gmail.com", title, statisticMessage.ToString());
            await _emailSenderService.SendEmail("rplinzer@gmail.com", title, statisticMessage.ToString());
        }
    }
}
