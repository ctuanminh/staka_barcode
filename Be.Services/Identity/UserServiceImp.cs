using Be.Common.Dtos.Identity;
using Be.Common.Request;
using Be.Common.Responses;
using Be.Common.Responses.KiotViet;
using Be.Common.utils;
using Be.Core.Entities.Identity;
using Be.Data.Data;
using Be.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace Be.Services.Identity
{
    public class UserServiceImp : ServiceResponse, IUserService
    {
        private readonly IdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IRepository _repository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserServiceImp(IdentityDbContext dbContext, 
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService, 
            IRepository repository, 
            IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtService = jwtService;
            _repository = repository;
            _httpClientFactory = httpClientFactory;
            _config = config;
            _httpClient = _httpClientFactory.CreateClient();
        }

        /// <summary>
        /// Login user with phone number and password.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ApiResponse> Login(UserLoginRequest request)
        {
            try
            {              
                var userExist = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
                if (userExist == null) return BadRequest("B", "Số điện thoại/mật khẩu không chính xác.");
                var loginResult =
                    _userManager.PasswordHasher.VerifyHashedPassword(userExist, userExist.PasswordHash, request.Password);

                if (loginResult == PasswordVerificationResult.Failed)
                {
                    return BadRequest("B", "Số điện thoại hoặc mật khẩu không chính xác.");
                }
                                
                var roles = new List<string> { "admin", "customer" };
                var accessToken = _jwtService.GenerateAccessToken(userExist.Id, userExist.Email, roles.ToList());
                return Ok(new
                {
                    AccessToken = accessToken,
                    User = new
                    {
                        Id = userExist.Id,
                        UserName = userExist.UserName,
                        Email = userExist.Email,
                        Roles = roles,
                        Phone = userExist.PhoneNumber,
                        UserType = userExist.UserType,
                        IsVendor = userExist.IsVendor
                    }
                });
            }
            catch (Exception ex)
            {

                return BadRequest("B", ex.ToString());
            }
            
        }

        public async Task<ApiResponse> AddUser(UserRequest request)
        {
            try
            {
                var userExist = await _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
                if (userExist != null)
                {
                    return BadRequest("B", "Phone number already exist!");
                }
                var phoneNumberExist = await _userManager.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
                if (phoneNumberExist) 
                {
                    return BadRequest("B", "Phone already exist");
                }
                var password = "123";
                if (request.Password != null) password = request.Password;
                var user = new ApplicationUser
                {
                    Id = 1,
                    UserName = request.Email,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    OwnerName = request.OwnerName,
                    EmployeeRole = request.EmployeeRole,
                    UserType = request.UserType,
                    EmployeeRoles = request.EmployeeRoles,
                    IsVendor = request.IsVendor,
                };
                var result = await _userManager.CreateAsync(user, password);
                if (result.Succeeded == false)
                {
                    var msgArr = result.Errors.Select(x => x.Description);
                    var msg = string.Join('-', msgArr + Environment.NewLine);
                    return BadRequest("B", msg);
                }

                if (request.RolesIds is { Count: > 0 })
                {
                    var roles = await _dbContext.Roles.ToListAsync();
                    var subRoles = await _dbContext.SubRoles.ToListAsync();
                    request.RolesIds = request.RolesIds.ToList();
                    var rolesSelect = roles.Where(x => request.RolesIds.Contains(x.Id))
                        .Select(x => new UserRole()
                        {
                            UserId = user.Id,
                            RoleId = x.Id
                        });
                    await _dbContext.AddRangeAsync(rolesSelect);
                    var subRolesSelect = subRoles.Where(x => request.RolesIds.Contains(x.Id))
                        .Select(x => new UserSubRole()
                        {
                            SubRoleId = x.Id,
                            UserId = user.Id
                        });
                    await _dbContext.AddRangeAsync(subRolesSelect);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok(new { UserId = user.Id });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<ApiResponse> GetAllRoles()
        {
            try
            {
                var roles = await _dbContext.Roles.Select(x => new RoleModelResponse
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToArrayAsync();
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        public async Task<ApiResponse> GetAllUsers(SearchUserRequest request)
        {
            var query = _userManager.Users.Where(x => x.IsVendor == false);
            if (!string.IsNullOrEmpty(request.Email))
            {
                query = query.Where(x => x.Email!.ToLower().Equals(request.Email.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                query = query.Where(x => x.PhoneNumber!.Contains(request.Phone));
            }

            query = query.OrderBy(x => x.Email);
            var page = await _repository.FindPagedAsync(query, pageIndex: request.PageIndex, pageSize: request.PageSize);
            var userModelResponse = new List<UserModelResponse>();
            var userIdsList = page.Items.Select(x => x.Id);
            var userSubRoles = await _dbContext.UserSubRoles
                .Where(x => userIdsList.Contains(Convert.ToInt64(x.UserId)))
                .ToListAsync();

            foreach (var user in page.Items)
            {
                var roleIds = await _dbContext.UserRoles.Where(x => x.UserId == user.Id)
                    .Select(x => x.RoleId).ToListAsync();
                var subRolesIds = userSubRoles.Where(x => x.UserId == user.Id)
                    .Select(x => x.SubRoleId);
                  roleIds.AddRange(subRolesIds);

                  var userModel = new UserModelResponse()
                  {
                      Id = user.Id,
                      Email = user.Email,
                      UserType = user.UserType,
                      Lock = user.LockoutEnabled,
                      UserName = user.UserName,
                      Phone = user.PhoneNumber,
                      RolesId = roleIds
                  };
                  userModelResponse.Add(userModel);
            }

            var pageResult = new PagedResult<UserModelResponse>()
            {
                PageSize = page.PageSize,
                PageIndex = page.PageIndex,
                Items = userModelResponse,
                TotalCount = page.TotalCount
            };
            return Ok(pageResult);
        }

        public async Task<ApiResponse> GetUserById(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                    return Ok(user);
                }
                return BadRequest("B", "User not exist");
            }
            catch (Exception e)
            {
                return BadRequest("B", e.ToString());
            }
        }

        public async Task<ApiResponse> AddRole(CreateRoleRequest request)
        {
            try
            {
                var role = new Role()
                {                   
                    Name = request.Name,
                    NormalizedName = request.Name,                    
                };
                await _dbContext.AddRangeAsync(role);
                await _dbContext.SaveChangesAsync();
                return Ok(role);
            }
            catch (Exception e)
            {
                return BadRequest("B", e.ToString());
            }
        }

        public async Task<ApiResponse> SyncRole(SyncRoleRequest request)
        {
            request.PageSize = 200;
            const string baseUrl = "https://api-man1.kiotviet.vn/api/setting/roles";

            var isHeaderReady = await KiotVietAuthHelper.AuthorizedHeadersAsync(_httpClient, _config, defaultToken: true);
            if (!isHeaderReady)
            {
                return null;
            }
            var url = QueryStringHelper.BuildQueryString(request, baseUrl);
            
            var response = await _httpClient.GetAsync(baseUrl);
            if(!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return BadRequest("Error", error);
            }
            var dataResponse = await response.Content.ReadAsStringAsync();  
            var kiotVietUserApiResopnse = JsonConvert.DeserializeObject<List<KiotVietRoleApiResponse>>(
                dataResponse,
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
            );

            if (kiotVietUserApiResopnse == null)
            {
                return BadRequest("B", "KiotViet user not found");
            }

            foreach (var item in kiotVietUserApiResopnse)
            {
                var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == item.Id);
                if(role == null)
                {
                    role = new Role()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        NormalizedName = item.Name.ToUpper()
                    };
                    await _dbContext.Roles.AddAsync(role);
                }
                else
                {
                    role.Name = item.Name;
                    role.NormalizedName = item.Name.ToUpper();
                    _dbContext.Roles.Update(role);
                }
            }
            await _dbContext.SaveChangesAsync();
            return Ok(new {});
        }

        public async Task<ApiResponse> SyncUser(SyncUserRequest request)
        {
            var baseUrl = "https://public.kiotapi.com/users";
            var numRequest = 2;
            request.PageSize = 100;
            request.CurrentItem = 0;

            var kiotVietUserApiList = new List<KiotVietUserApi>();
            for (int i = 1; i <= numRequest; i++)
            {
                var (Success, Content) = await KiotVietApiHelper.CallApiAsync(_httpClient, _config, baseUrl, request);
                if (!Success || string.IsNullOrEmpty(Content))
                {                    
                    return BadRequest("Không thể lấy dữ liệu từ KiotViet");
                }                
                kiotVietUserApiList.AddRange(JsonConvert.DeserializeObject<KiotVietUserApiResponse>(Content).Data);                                
                request.CurrentItem = (i) * request.PageSize;
            }

            // Lấy danh sách ID người dùng từ KiotViet
            // Để lấy toàn bộ user có trong db
            var userIdExistList = kiotVietUserApiList.Select(x => x.Id).ToList();

            var userExistList = await _userManager.Users
                .Where(x => userIdExistList.Contains(x.Id))
                .ToDictionaryAsync(u =>u.Id);

            foreach (var item in kiotVietUserApiList)
            {
                if (userExistList.TryGetValue(item.Id, out var userExist))
                {                    
                    userExist.UserName = item.UserName;
                    userExist.NormalizedUserName = item.GivenName;
                    userExist.PhoneNumber = item.MobilePhone;
                    userExist.Email = item.Email;
                    userExist.FullName = item.GivenName;
                    userExist.CreatedAt = DateTime.SpecifyKind(item.CreatedDate, DateTimeKind.Utc);
                    await _userManager.UpdateAsync(userExist);
                }
                else
                {
                    var newUser = new ApplicationUser
                    {
                        Id = item.Id,
                        UserName = item.UserName,
                        NormalizedUserName = item.GivenName,
                        FullName = item.GivenName,
                        PhoneNumber = item.MobilePhone,
                        Email = item.Email,
                        CreatedAt = DateTime.SpecifyKind(item.CreatedDate, DateTimeKind.Utc)
                    };
                    await _userManager.CreateAsync(newUser);
                }
            }            
            
            return Ok(new { });
        }

        public Task<ApiResponse> GetInfo(string token)
        {
            throw new NotImplementedException();
        }
    }
}