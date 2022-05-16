using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Social.Core;
using Social.Core.Domain;
using Social.Core.Paging;
using Social.Core.ViewModel;
using Social.Core.ViewModel.Users;
using Social.Data;

namespace Social.Service.User
{
    public class UserService : IUserService
    {
        private readonly IRepository<Core.Domain.User> _usersRepository;
        private readonly IRepository<UsersRequest> _requestRepository;

        public UserService(IRepository<Core.Domain.User> usersRepository, IRepository<UsersRequest> requestRepository)
        {
            _usersRepository = usersRepository;
            _requestRepository = requestRepository;
        }


        public Task<Core.Domain.User> GetCustomerByMobileAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<Core.Domain.User> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _usersRepository.TableNoTracking
                        orderby c.Id
                        where c.Email == email && c.Deleted == false
                        select c;
            var user = await query.FirstOrDefaultAsync();

            return user;
        }



        public UserRegisterViewModel Register(UserRegistrationRequest model)
        {
            var result = new UserRegisterViewModel();
            try
            {
                var isvalidEmail = CommonHelper.IsValidEmail(model.Email);
                var user = new Core.Domain.User();
                if (isvalidEmail)
                {
                    user = GetUserByEmailAsync(model.Email).Result;
                }
                else
                {
                    result.RegisterStatus = RegisterStatus.EmailNotValid;

                }

                if (user != null)
                {
                    result.RegisterStatus = RegisterStatus.AlreadyExsist;
                }
                else
                {
                    string passwordSalt = CommonHelper.CreateSaltKey(5);
                    model.Password = CommonHelper.CreatePasswordHash(model.Password, passwordSalt);
                    var newUser = new Core.Domain.User()
                    {
                        Active = true,
                        Deleted = false,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserGuid = Guid.NewGuid(),
                        Password = model.Password,
                        PasswordFormat = PasswordFormat.Hashed,
                        PasswordSalt = passwordSalt,
                        CreatedOnUtc = DateTime.Now
                    };
                    _usersRepository.Add(newUser);
                    result.RegisterStatus = RegisterStatus.Success;
                }
            }
            catch (Exception e)
            {
                result.RegisterStatus = RegisterStatus.Failed;
                result.Message = e.Message;
            }
            return result;
        }

        private int UserIdGetByGuid(Guid id)
        {
            return _usersRepository.TableNoTracking.Where(p => p.UserGuid == id).Select(p => p.Id)
                .FirstOrDefault();

        }
        public UserProfile UserGetByGuid(Guid id, int currentUserId)
        {
            int userId = UserIdGetByGuid(id);

            return (from u in _usersRepository.TableNoTracking
                    where u.Deleted == false && u.Active && u.UserGuid == id
                    select new UserProfile()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserGuid = u.UserGuid,
                        UserId = u.Id,
                        Email = u.Email,
                        CreatedOnUtc = u.CreatedOnUtc,
                        StateRequest = _requestRepository.TableNoTracking.Where(p => p.UserId == currentUserId && p.RequestUserId == u.Id).Select(p => p.StateRequest).FirstOrDefault()
                    }
                 ).FirstOrDefault();

        }

        public void RequestFriendAdd(int userId, Guid guidUser)
        {
            int requestUserId = UserIdGetByGuid(guidUser);
            var item = _requestRepository.TableNoTracking.FirstOrDefault(p => p.UserId == userId && p.RequestUserId == requestUserId);

            if (item == null)
            {
                _requestRepository.Add(new UsersRequest()
                {
                    StateRequest = (int)StateRequestEnum.RequestFriend,
                    RequestUserId = requestUserId,
                    UserId = userId
                });
            }

        }
        public void RequestFriendRemove(int userId, Guid guidUser)
        {
            int requestUserId = UserIdGetByGuid(guidUser);
            var item = _requestRepository.TableNoTracking.FirstOrDefault(p => p.UserId == userId && p.RequestUserId == requestUserId);

            if (item != null)
            {
                _requestRepository.Delete(item);
            }

        }
        public PagedResult<UserResponseModel> Search(string name, int pageNo = 1)
        {
            var query = (from u in _usersRepository.TableNoTracking
                         where u.Deleted == false && u.Active
                         select new UserResponseModel()
                         {
                             FirstName = u.FirstName,
                             LastName = u.LastName,
                             UserGuid = u.UserGuid,
                             UserId = u.Id,
                             Email = u.Email,
                             CreatedOnUtc = u.CreatedOnUtc
                         }
                );
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.FirstName == name);

            var paged = query.GetPaged(pageNo, 20);



            return paged;
        }

    }
}
