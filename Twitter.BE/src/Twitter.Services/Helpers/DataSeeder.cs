using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Twitter.Data.Context;
using Twitter.Data.Model;
using Twitter.Repositories.Interfaces;

namespace Twitter.Services.Helpers
{
    public class DataSeeder
    {
        private readonly TwitterDbContext _dbContext;
        private readonly IBaseRepository<Language> _languageRepository;
        private readonly IBaseRepository<Gender> _genderRepository;
        private readonly IBaseRepository<ProgrammingLanguage> _programmingLanguageRepository;
        private readonly ITweetRepository _tweetRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;

        public DataSeeder(
            TwitterDbContext dbContext,
            IBaseRepository<Language> languageRepository,
            IBaseRepository<Gender> genderRepository,
            IBaseRepository<ProgrammingLanguage> programmingLanguageRepository,
            ITweetRepository tweetRepository,
            IBaseRepository<User> userRepository,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _languageRepository = languageRepository;
            _genderRepository = genderRepository;
            _programmingLanguageRepository = programmingLanguageRepository;
            _tweetRepository = tweetRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public void EnsureSeedData()
        {
            SeedLanguages().Wait();
            SeedGender().Wait();
            SeedUsers().Wait();
            SeedProgrammingLanguages().Wait();
            SeedPostsComments().Wait();
        }

        private async Task SeedLanguages()
        {
            if (!await _dbContext.Languages.AnyAsync())
            {
                var languages = new Language[]
                {
                    new Language
                    {
                        Name="English",
                        Code="en"
                    },
                    new Language
                    {
                        Name="Polish",
                        Code="pl"
                    }
                };

                await _languageRepository.AddRangeAsync(languages);
            }
        }

        private async Task SeedProgrammingLanguages()
        {
            if (!await _dbContext.ProgrammingLanguages.AnyAsync())
            {
                var languages = new ProgrammingLanguage[]
                {
                    new ProgrammingLanguage
                    {
                        Name="C#"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Javascript"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Python"
                    },
                    new ProgrammingLanguage
                    {
                        Name="C++"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Typescript"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Java"
                    }
                };

                await _programmingLanguageRepository.AddRangeAsync(languages);
            }
        }

        private async Task SeedUsers()
        {
            if (!await _dbContext.Users.AnyAsync())
            {
                var languages = await _languageRepository.GetAllAsync();

                var users = new List<User>();


                for (int i = 0; i < 100; i++)
                {
                    var userFaker = new Faker<User>()
                        .RuleFor(o => o.Email, f => f.Internet.Email())
                        .RuleFor(o => o.UserName, f => f.Internet.Email())
                        .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                        .RuleFor(o => o.LastName, f => f.Name.LastName())
                        .RuleFor(o => o.AboutMe, f => f.Lorem.Paragraph(10))
                        .RuleFor(o => o.BirthDay, f => f.Date.Between(new DateTime(1970, 1, 30), new DateTime(1999, 12, 30)))
                        .RuleFor(o => o.Image, f => f.Internet.Avatar())
                        .RuleFor(o => o.GenderId, f => f.Random.Int(1, 2))
                        .RuleFor(o => o.Settings, f => new Settings
                        {
                            IsDarkTheme = f.Random.Bool(),
                            LanguageId = f.PickRandom(languages).Id
                        })
                        .Generate();

                    users.Add(userFaker);
                }

                foreach (var user in users)
                {
                    var result = await _userManager.CreateAsync(user, "P@ssw0rd");
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    await _userManager.ConfirmEmailAsync(user, token);
                }
            }
        }

        private async Task SeedPostsComments()
        {
            if (!await _dbContext.Tweets.AnyAsync())
            {
                var programmingLanguages = await _programmingLanguageRepository.GetAllAsync();
                var users = (await _userRepository.GetPagedAsync(1, 5)).ToList();

                var tweets = new List<Tweet>();
                for (int i = 0; i < 50; i++)
                {
                    var tweetFaker = new Faker<Tweet>()
                        .RuleFor(o => o.Text, f => f.Lorem.Paragraph(5))
                        .RuleFor(o => o.CreationDate, f => f.Date.Between(DateTime.Now.AddDays(-7), DateTime.Now.AddHours(-2)))
                        .RuleFor(o => o.CodeSnippet, f => new CodeSnippet
                        {
                            Text = f.Lorem.Paragraphs(5),
                            ProgrammingLanguageId = f.PickRandom(programmingLanguages).Id
                        })
                        .RuleFor(o => o.Comments, f => f.Make(5, c => new Comment
                        {
                            AuthorId = f.PickRandom(users).Id,
                            CreationDate = f.Date.Between(DateTime.Now.AddHours(-2), DateTime.Now),
                            Text = f.Lorem.Paragraphs(2)
                        }))
                        .RuleFor(o => o.AuthorId, f => f.PickRandom(users).Id)
                        .Generate();

                    tweets.Add(tweetFaker);
                }
                await _tweetRepository.AddRangeAsync(tweets);
            }
        }

        private async Task SeedGender()
        {
            if (!await _dbContext.Genders.AnyAsync())
            {
                var genders = new Gender[]
                {
                    new Gender
                    {
                        Name="Man"
                    },
                     new Gender
                    {
                        Name="Woman"
                    }
                };

                await _genderRepository.AddRangeAsync(genders);
            }
        }
    }
}
