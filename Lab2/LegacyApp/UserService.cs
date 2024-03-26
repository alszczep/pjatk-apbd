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

        public User? CreateUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
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

            Client client = this.ClientRepository.GetById(clientId);

            User user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            switch (client.Type)
            {
                case KnownClientTypes.VeryImportantClient:
                {
                    user.HasCreditLimit = false;

                    break;
                }
                case KnownClientTypes.ImportantClient:
                {
                    using (IUserCreditService userCreditService = this.UserCreditServiceFactory())
                    {
                        int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                        user.CreditLimit = creditLimit * 2;
                    }

                    break;
                }
                default:
                {
                    user.HasCreditLimit = true;
                    using (IUserCreditService userCreditService = this.UserCreditServiceFactory())
                    {
                        user.CreditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    }

                    break;
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
