using API.Domain;
using API.EFCore;

namespace API.DataAccess.Initializer
{
    public class DbInitializer
    {
        private readonly IdentityServerDbContext _context;

        private readonly int _nbClaims;
        private readonly int _nbApplications;

        private readonly Random _random = new();

        public DbInitializer(IdentityServerDbContext context, int nbApplications = 5, int nbClaims = 5)
        {
            _context = context;
            _nbApplications = nbApplications;
            _nbClaims = nbClaims;
        }

        public void Initialize()
        {
            var conInfos = CreateConnectionInformations();
            _context.Set<ConnectionInformation>().AddRange(conInfos);
            _context.SaveChanges();


            var securityServerApplication = CreateSecurityServerApplication();
            _context.Set<Application>().Add(securityServerApplication);
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var tempApplications = CreateApplications().ToArray();
            var applicationsClaims = CreateApplicationsClaims().ToArray();
            var applications = LinkApplicationWithApplicationClaims(tempApplications, applicationsClaims).ToArray();
            var users = CreateUsers();

            _context.Set<Application>().AddRange(applications);
            _context.Set<User>().AddRange(users);

            _context.SaveChanges();

            var userNr = _context.Set<User>().First();
            var securityServerApplicationClaims = CreateSecurityServerApplicationClaims(securityServerApplication);

            _context.Set<ApplicationClaim>().AddRange(securityServerApplicationClaims);

            _context.SaveChanges();

            var securityServerClaims = _context.Set<ApplicationClaim>().Where(x => x.Application == securityServerApplication).ToArray();
            var userApplicationClaims = CreateSecurityServerUserApplicationClaims(userNr, securityServerClaims.First(), securityServerClaims.Last());
            _context.Set<UserApplicationClaim>().AddRange(userApplicationClaims);

            _context.SaveChanges();
        }

        private Application CreateSecurityServerApplication()
        {
            var app = new Application()
            {
                Name = "Security server",
                Secret = Guid.NewGuid().ToString()
            };

            return app;
        }

        private IEnumerable<ConnectionInformation> CreateConnectionInformations()
        {
            var allConnectionInformations = new List<ConnectionInformation>();

            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                var con = new ConnectionInformation()
                {
                    Date = DateTime.Now,
                    HttpResult = random.Next(100, 500),
                    Ip = "170.1.20." + i,
                    UserId = 1
                };

                allConnectionInformations.Add(con);

            }

            return allConnectionInformations;
        }

        private static IEnumerable<ApplicationClaim> CreateSecurityServerApplicationClaims(Application application)
        {
            var claimAge = new ApplicationClaim()
            {
                ClaimType = "Number",
                ClaimValue = "Age",
                Application = application
            };
            yield return claimAge;


            var claimName = new ApplicationClaim()
            {
                ClaimType = "String",
                ClaimValue = "Name",
                Application = application
            };

            yield return claimName;
        }

        private static IEnumerable<UserApplicationClaim> CreateSecurityServerUserApplicationClaims(User user, ApplicationClaim ageClaim, ApplicationClaim nameClaim)
        {

            var ageUserApplicationClaim = new UserApplicationClaim()
            {
                ApplicationClaim = ageClaim,
                User = user,
                Value = "18"
            };

            yield return ageUserApplicationClaim;

            var nameUserApplicationClaim = new UserApplicationClaim()
            {
                ApplicationClaim = nameClaim,
                User = user,
                Value = "Nicosto"
            };

            yield return nameUserApplicationClaim;
        }


        private IEnumerable<Application> CreateApplications()
        {
            for (int i = 0; i < _nbApplications; i++)
            {
                var app = new Application
                {
                    Name = DataMocker.ApplicationNames().ElementAt(_random.Next(0, DataMocker.ApplicationNames().Count() - 1)),
                    Secret = Guid.NewGuid().ToString()
                };

                yield return app;
            }
        }

        private IEnumerable<ApplicationClaim> CreateApplicationsClaims()
        {
            for (int i = 0; i < _nbClaims; i++)
            {
                var appClaims = new ApplicationClaim
                {
                    ClaimType = DataMocker.ClaimTypes().ElementAt(_random.Next(0, DataMocker.ClaimTypes().Count() - 1)),
                    ClaimValue = DataMocker.ClaimValues().ElementAt(_random.Next(0, DataMocker.ClaimValues().Count() - 1))
                };

                yield return appClaims;
            }
        }

        private IEnumerable<Application> LinkApplicationWithApplicationClaims(IEnumerable<Application> applications, IEnumerable<ApplicationClaim> applicationsClaims)
        {
            foreach (var application in applications)
            {
                int nbClaims = _random.Next(0, applicationsClaims.Count() - 1);

                List<ApplicationClaim> applicationsClaimsList = new(nbClaims);

                for (int i = 0; i < nbClaims; i++)
                {
                    int rndClaim = _random.Next(0, applicationsClaims.Count() - 1);
                    applicationsClaimsList.Add(applicationsClaims.ElementAt(rndClaim));
                }

                application.ApplicationsClaims = applicationsClaimsList;

                yield return application;
            } 
        }

        private static IEnumerable<User> CreateUsers()
        {
            yield return new User()
            {
                Email = "nr",
                Firstname = "Nicolas",
                Lastname = "Richet",
                Password = "ADCB/GoEEAM7J/0dFG+kjhl/m9M3EEh8jUxksRJEAaSt7hpu4/3FoZYiEC8sqF7ang==",
                Picture = "https://pbs.twimg.com/media/D0w-NI5X0AIGBuw.jpg",
            };
        }


    }
}
