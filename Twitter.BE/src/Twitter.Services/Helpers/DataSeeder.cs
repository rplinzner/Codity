using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
                        Name="English"
                    },
                    new Language
                    {
                        Name="Polish"
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
                var users = new User[]
                {
                    new User
                    {
                        FirstName="Jan",
                        LastName="Kowalski",
                        Email="jan.kowalski@gmail.com",
                        UserName="jan.kowalski@gmail.com",
                        Settings = new Settings
                        {
                            IsDarkTheme = true,
                            LanguageId = languages.ElementAt(0).Id
                        }
                    },
                     new User
                    {
                        FirstName="Jadwiga",
                        LastName="Kowalska",
                        Email="jadwiga.kowalska@gmail.com",
                        UserName="jadwiga.kowalska@gmail.com",
                        Settings = new Settings
                        {
                            IsDarkTheme = true,
                            LanguageId = languages.ElementAt(1).Id
                        }
                    },
                };

                foreach (var user in users)
                {
                    var result = await _userManager.CreateAsync(user, "Password");
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
