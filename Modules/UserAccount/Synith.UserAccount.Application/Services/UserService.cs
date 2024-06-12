using Synith.UserAccount.Domain.DataTransferObjects.User;
using Synith.UserAccount.Domain.Enums;

namespace Synith.UserAccount.Application.Services;
public class UserService : EntityService<User, UserRetrieve>, IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly UserAccountDbContext _context;
    private readonly IUserValidator _validator;
    private readonly ICryptographyService _cryptography;
    private readonly ITokenService _token;

    public UserService(ILogger<UserService> logger, UserAccountDbContext context, IUserValidator validator,
        ICryptographyService cryptography, ITokenService token) : base(logger, context)
    {
        _logger = logger;
        _context = context;
        _validator = validator;
        _cryptography = cryptography;
        _token = token;
        OnBeforeRetrieve += UserService_OnBeforeRetrieve;
    }

    private IQueryable<UserRetrieve> UserService_OnBeforeRetrieve(IQueryable<User> users)
    {
        return users.Select(x => new UserRetrieve
        {
            Id = x.Id,
            Username = x.Username,
            Email = x.Email,
            Language = x.Language,
            Status = x.Status,
            Roles = x.Roles.Select(r => new UserRoleRetrieve
            {
                Id = r.RoleId,
                Name = r.Role.Name
            })
        });
    }

    protected override UserRetrieve ToRetrieveDto(User user)
    {
        return user.ToRetrieveDto();
    }

    protected override UserRetrieve Decrypt(UserRetrieve user)
    {
        user.Username = _cryptography.Decrypt(user.Username);
        user.Email = _cryptography.Decrypt(user.Email);
        return user;
    }

    private void Secure(User user)
    {
        user.Username = _cryptography.Encrypt(user.Username);
        user.Password = _cryptography.Hash(user.Password);
        user.Email = _cryptography.Encrypt(user.Email);
    }

    public async Task<UserRetrieve> CreateAsync(UserCreate userCreate)
    {
        _logger.LogInformation("Creating user({username}).", userCreate.Username);

        User user = userCreate.ToEntity();

        ValidationResult result = _validator
            .IncludePassword()
            .IncludeDefaultValidations()
            .IncludeMustBeZeroId()
            .Validate(user);

        if (result.IsValid)
        {
            Secure(user);
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            user.Language = (await _context.Languages.FindAsync(user.LanguageId))!;
            _logger.LogInformation("User({username}) has been successfully created", userCreate.Username);

            if (userCreate.Roles != null && userCreate.Roles.Any())
            {
                await UpdateUserRolesAsync(user.Id, userCreate.Roles);
            }

            return Decrypt(user.ToRetrieveDto());
        }
        else
        {
            string errorJson = result.ToJson();
            _logger.LogInformation("One or more user details is invalid.");
            _logger.LogError("{message}", errorJson);
            throw new ValidationException(errorJson);
        }
    }

    public async Task<UserRetrieve> UpdateAsync(UserUpdate userUpdate)
    {
        _logger.LogInformation("Updating user({userId}).", userUpdate.Id);

        User user = userUpdate.ToEntity();

        ValidationResult result = _validator
            .IncludeDefaultValidations()
            .IncludeId()
            .Validate(user);

        if (result.IsValid)
        {
            if (!await _context.Users.AnyAsync(x => x.Id == user.Id))
                throw new KeyNotFoundException(ErrorMessageProvider.NotFound($"{nameof(User)}.{nameof(User.Id)}", user.Id.ToString()));

            Secure(user);
            _context.Attach(user);
            _context.Entry(user).Property(nameof(User.Username)).IsModified = true;
            _context.Entry(user).Property(nameof(User.Email)).IsModified = true;
            _context.Entry(user).Property(nameof(User.LanguageId)).IsModified = true;
            await _context.SaveChangesAsync();
            user.Language = (await _context.Languages.FindAsync(user.LanguageId))!;
            _logger.LogInformation("User({userId}) has been successfully created", user.Id);

            if (userUpdate.Roles != null)
            {
                await UpdateUserRolesAsync(user.Id, userUpdate.Roles);
            }

            return Decrypt(user.ToRetrieveDto());
        }
        else
        {
            string errorJson = result.ToJson();
            _logger.LogInformation("One or more user details is invalid.");
            _logger.LogError("{message}", errorJson);
            throw new ValidationException(errorJson);
        }
    }

    public async Task<IEnumerable<Language>> RetrieveLanguagesAsync()
    {
        _logger.LogInformation("Retreiving languages.");
        IEnumerable<Language> languages = await _context.Languages.ToListAsync();
        _logger.LogInformation("Languages have been successfully retreived.");
        return languages;
    }

    public async Task<IEnumerable<UserRoleRetrieve>> RetrieveUserRolesAsync(int userId)
    {
        _logger.LogInformation("Retreiving user roles({userId}).", userId);
        IEnumerable<UserRoleRetrieve> roles = await _context.UserRoles
            .Where(x => x.UserId == userId && x.Role.Status != Core.Enums.GenericStatus.Inactive)
            .Select(x => new UserRoleRetrieve
            {
                Id = x.Role.Id,
                Name = x.Role.Name
            })
            .ToListAsync();
        _logger.LogInformation("User roles({userId}) have been successfully retreived.", userId);
        return roles;
    }

    public async Task UpdateUserRolesAsync(int userId, IEnumerable<int> roleIds)
    {
        _logger.LogInformation("Updating user roles({userId}).", userId);

        string sql = "EXEC [uac].[User_UpdateUserRoles] @UserId, @RoleIds";

        List<SqlParameter> parameters =
            [
                new("@UserId", userId),
                new("@RoleIds", string.Join(",", roleIds))
            ];

        await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        _logger.LogInformation("User roles({userId}) have been successfully updated.", userId);
    }

    public async Task<IEnumerable<UserSetting>> RetrieveSettingAsync(int userId)
    {
        _logger.LogInformation("Retreiving user settings.");
        IEnumerable<UserSetting> settings = await _context.UserSettings.Where(x => x.UserId == userId).ToListAsync();
        _logger.LogInformation("User settings have been successfully retreived.");
        return settings;
    }

    public async Task UpdatePasswordAsync(UserPasswordChange passwordChange)
    {
        _logger.LogInformation("Updating password({userId})", passwordChange.UserId);

        if (passwordChange.UserId != _token.UserId)
        {
            throw new ValidationException(ErrorMessageProvider.NotAuthorized());
        }

        UserRetrieve? login = null;

        try
        {
            login = await ValidateLoginAsync(new UserLogin
            {
                Username = _token.Username,
                Password = passwordChange.CurrentPassword
            });
        }
        catch (ValidationException ex)
        {
            if (ex.Message == ErrorMessageProvider.Login.Invalid())
            {
                throw new ValidationException(ErrorMessageProvider.InvalidPassword());
            }
        }

        var user = new User
        {
            Id = login!.Id,
            Password = passwordChange.NewPassword
        };

        ValidationResult result = _validator
            .IncludePassword()
            .IncludeId()
            .Validate(user);

        if (result.IsValid)
        {
            Secure(user);
            _context.Attach(user);
            _context.Entry(user).Property(nameof(User.Password)).IsModified = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation("Password({userId}) have been successfully updated.", passwordChange.UserId);
        }
        else
        {
            string errorJson = result.ToJson();
            _logger.LogInformation("New password is invalid.");
            _logger.LogError("{message}", errorJson);
            throw new ValidationException(errorJson);
        }
    }

    public async Task UpdateSettingAsync(UserSetting setting)
    {
        _logger.LogInformation("Updating user setting({userId}.{name})", setting.UserId, setting.Name);
        string sql = "EXEC [uac].[User_UpdateSettings] @UserId, @Name, @Value";

        List<SqlParameter> parameters =
            [
                new("@UserId", setting.UserId),
                new("@Name", setting.Name),
                new("@Value", setting.Value)
            ];

        await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        _logger.LogInformation("User setting({userId}.{name}) have been successfully updated.", setting.UserId, setting.Name);
    }

    public async Task<UserRetrieve> ValidateLoginAsync(UserLogin login)
    {
        _logger.LogInformation("Validating login({username}).", login.Username);

        User? user = await _context.Users
            .Where(user => user.Username == _cryptography.Encrypt(login.Username))
            .Include(user => user.Language)
            .Include(user => user.Roles)
            .AsNoTracking()
            .SingleOrDefaultAsync();

        if (user == null || !_cryptography.IsPasswordValid(login.Password, user.Password))
        {
            _logger.LogInformation("Invalid login({username}).", login.Username);
            throw new ValidationException(ErrorMessageProvider.Login.Invalid());
        }

        switch (user.Status)
        {
            case UserStatus.Inactive:
                _logger.LogInformation("Inactive login({username}).", login.Username);
                throw new ValidationException(ErrorMessageProvider.Login.Inactive());
            case UserStatus.Pending:
                _logger.LogInformation("Pending login({username}).", login.Username);
                throw new ValidationException(ErrorMessageProvider.Login.Pending());
            case UserStatus.Locked:
                _logger.LogInformation("Locked login({username}).", login.Username);
                throw new ValidationException(ErrorMessageProvider.Login.Locked());
            case UserStatus.Active:
                _logger.LogInformation("Login({username}) has been successfully validated.", login.Username);
                return Decrypt(user.ToRetrieveDto());
            default:
                _logger.LogInformation("Invalid login({username}).", login.Username);
                throw new ValidationException(ErrorMessageProvider.Invalid(nameof(User.Status), user.Status.ToString()));
        }
    }
}
