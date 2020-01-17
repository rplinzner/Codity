using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;
using Codity.Services.Interfaces;

namespace Codity.Services.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBaseRepository<ProgrammingLanguage> _programmingLanguageRepository;
        private readonly IEmailSenderService _emailSenderService;

        public StatisticService(
            IPostRepository postRepository,
            IUserRepository userRepository,
            IBaseRepository<ProgrammingLanguage> programmingLanguageRepository,
            IEmailSenderService emailSenderService)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _programmingLanguageRepository = programmingLanguageRepository;
            _emailSenderService = emailSenderService;
        }

        public async Task SendWeeklyStatisticSummary()
        {
            var programmingLanguages = await _programmingLanguageRepository.GetAllAsync();
            var lastWeekPosts = await _postRepository.GetAllByAsync(c => c.CreationDate > DateTime.Now.AddDays(-7),
                                                                      false, c => c.Likes, c => c.Comments,
                                                                      c => c.CodeSnippet);
            var statisticDictionary = new Dictionary<string, (int, int, int)>();

            foreach (var programmingLanguage in programmingLanguages)
            {
                var postCounter = 0;
                var commentCounter = 0;
                var likeCounter = 0;

                foreach (var post in lastWeekPosts)
                {
                    if (post.Text.ToLower().Contains(programmingLanguage.Name.ToLower())
                        || post.CodeSnippet.ProgrammingLanguageId == programmingLanguage.Id)
                    {
                        postCounter++;
                        commentCounter += post.Comments.Count;
                        likeCounter += post.Likes.Count;
                    }
                }

                statisticDictionary.Add(programmingLanguage.Name, (postCounter, commentCounter, likeCounter));
            }

            var statisticMessage = new StringBuilder();
            int counter = 1;

            statisticMessage.AppendLine("<strong>The largest number of posts by programming language</strong>");

            foreach (var post in statisticDictionary.OrderByDescending(c => c.Value.Item1))
            {
                statisticMessage.AppendLine($"<p>{counter++}. {post.Key} - {post.Value.Item1} posts</p>");
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
            await _emailSenderService.SendEmail("rplinzner@gmail.com", title, statisticMessage.ToString());
        }
    }
}
