using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        private readonly UserManager<User> _userManager;

        public DataSeeder(
            TwitterDbContext dbContext,
            IBaseRepository<Language> languageRepository,
            IBaseRepository<Gender> genderRepository,
            IBaseRepository<ProgrammingLanguage> programmingLanguageRepository,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _languageRepository = languageRepository;
            _genderRepository = genderRepository;
            _programmingLanguageRepository = programmingLanguageRepository;
            _userManager = userManager;
        }

        public void EnsureSeedData()
        {
            SeedLanguages().Wait();
            SeedGender().Wait();
            SeedUsers().Wait();
            SeedProgrammingLanguages().Wait();
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
