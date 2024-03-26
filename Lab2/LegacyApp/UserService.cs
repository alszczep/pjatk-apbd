using System;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository clientRepository;
        private Func<IUserCreditService> userCreditServiceFactory;

        public UserService(IClientRepository clientRepository, Func<IUserCreditService> userCreditServiceFactory)
        {
            this.clientRepository = clientRepository;
            this.userCreditServiceFactory = userCreditServiceFactory;
        }

        public UserService()
        {
            this.clientRepository = new ClientRepository();
            this.userCreditServiceFactory = () => new UserCreditService();
        }
        
        
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            User? user = this.CreateUser(firstName, lastName, email, dateOfBirth, clientId);
            if (user is null)
            {
                return false;
            }
            
            UserDataAccess.AddUser(user);
            
            return true;
        }
        
        private User? CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                return null;
            }

            if (!email.Contains("@") && !email.Contains("."))
            {
                return null;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;

            if (age < 21)
            {
                return null;
            }

            var client = this.clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = this.userCreditServiceFactory())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = this.userCreditServiceFactory())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return null;
            }

            
            return user;
        }
    }
}
