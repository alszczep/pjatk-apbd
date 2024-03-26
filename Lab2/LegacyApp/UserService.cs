using System;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository ClientRepository { get; }
        private Func<IUserCreditService> UserCreditServiceFactory { get; }

        public UserService()
        {
            this.ClientRepository = new ClientRepository();
            this.UserCreditServiceFactory = () => new UserCreditService();
        }

        public UserService(IClientRepository clientRepository, Func<IUserCreditService> userCreditServiceFactory)
        {
            this.ClientRepository = clientRepository;
            this.UserCreditServiceFactory = userCreditServiceFactory;
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
            if (!Validators.IsFullNameValid(firstName, lastName))
            {
                return null;
            }

            if (!Validators.IsEmailValid(email))
            {
                return null;
            }

            if (!Validators.IsAgeOlderOrEqualTo21(DateTime.Now, dateOfBirth))
            {
                return null;
            }

            var client = this.ClientRepository.GetById(clientId);

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
                using (var userCreditService = this.UserCreditServiceFactory())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = this.UserCreditServiceFactory())
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
