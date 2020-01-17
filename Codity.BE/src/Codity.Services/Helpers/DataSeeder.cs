using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Codity.Data.Context;
using Codity.Data.Model;
using Codity.Repositories.Interfaces;

namespace Codity.Services.Helpers
{
    public class DataSeeder
    {
        private readonly CodityDbContext _dbContext;
        private readonly IBaseRepository<Language> _languageRepository;
        private readonly IBaseRepository<Gender> _genderRepository;
        private readonly IBaseRepository<ProgrammingLanguage> _programmingLanguageRepository;
        private readonly IPostRepository _postRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;

        public DataSeeder(
            CodityDbContext dbContext,
            IBaseRepository<Language> languageRepository,
            IBaseRepository<Gender> genderRepository,
            IBaseRepository<ProgrammingLanguage> programmingLanguageRepository,
            IPostRepository postRepository,
            IBaseRepository<User> userRepository,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _languageRepository = languageRepository;
            _genderRepository = genderRepository;
            _programmingLanguageRepository = programmingLanguageRepository;
            _postRepository = postRepository;
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
                        Name="C#",
                        Code="cs"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Javascript",
                        Code="javascript"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Python",
                        Code="python"
                    },
                    new ProgrammingLanguage
                    {
                        Name="C++",
                        Code="cpp"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Typescript",
                        Code="typescript"
                    },
                    new ProgrammingLanguage
                    {
                        Name="Java",
                        Code="java"
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
            if (!await _dbContext.Posts.AnyAsync())
            {
                var programmingLanguages = await _programmingLanguageRepository.GetAllAsync();
                var users = (await _userRepository.GetPagedAsync(1, 5)).ToList();
                var code = "        private async Task SeedProgrammingLanguages()\n"
                         + "        {\n            if (!await _dbContext.ProgrammingLanguages.AnyAsync())\n"
                         + "            {\n                var languages = new ProgrammingLanguage[]\n"
                         + "                {\n                    new ProgrammingLanguage\n                    {\n"
                         + "                        Name=\"cs\"\n                    },\n"
                         + "                    new ProgrammingLanguage\n                    {\n"
                         + "                        Name=\"javascript\"\n                    },\n"
                         + "                    new ProgrammingLanguage\n                    {\n"
                         + "                        Name=\"python\"\n                    },\n"
                         + "                    new ProgrammingLanguage\n                    {\n"
                         + "                        Name=\"cpp\"\n                    },\n"
                         + "                    new ProgrammingLanguage\n                    {\n"
                         + "                        Name=\"typescript\"\n                    },\n"
                         + "                    new ProgrammingLanguage\n                    {\n"
                         + "                        Name=\"java\"\n                    }\n"
                         + "                };\n                await _programmingLanguageRepository.AddRangeAsync(languages);\n"
                         + "            }\n        }";

                var posts = new List<Post>();
                for (int i = 0; i < 50; i++)
                {
                    var postFaker = new Faker<Post>()
                        .RuleFor(o => o.Text, f => f.Lorem.Paragraph(5))
                        .RuleFor(o => o.CreationDate, f => f.Date.Between(DateTime.Now.AddDays(-7), DateTime.Now.AddHours(-2)))
                        .RuleFor(o => o.CodeSnippet, f => new CodeSnippet
                        {
                            Text = code,
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

                    posts.Add(postFaker);
                }
                await _postRepository.AddRangeAsync(posts);
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
