-- 1. Insérer l'utilisateur ANONYMOUS
INSERT INTO [AspNetUsers] (
    [Id],
    [TwoFactorType],
    [BirthDay],
    [Gender],
    [CreatedOn],
    [FirstName],
    [LastName],
    [IsMfaForce],
    [IsLdapLogin],
    [Department],
    [ProfilePicture],
    [IsEnabled],
    [RolesCombined],
    [UserName],
    [NormalizedUserName],
    [Email],
    [NormalizedEmail],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [ConcurrencyStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEnd],
    [LockoutEnabled],
    [AccessFailedCount]
)
VALUES (
    '00000000-0000-0000-0000-000000000000', -- Id fixe
    0, -- [TwoFactorType]
    GETUTCDATE(), -- [BirthDay] par défaut
    0, -- [Gender] (Unknown)
    GETUTCDATE(), -- [CreatedOn]
    'Anonymous', -- [FirstName]
    'User', -- [LastName]
    0, -- [IsMfaForce]
    0, -- [IsLdapLogin]
    NULL, -- [Department]
    NULL, -- [ProfilePicture]
    1, -- [IsEnabled]
    'Guest', -- [RolesCombined]
    'anonymous.user', -- [UserName]
    'ANONYMOUS.USER', -- [NormalizedUserName]
    'anonymous@local.app', -- [Email]
    'ANONYMOUS@LOCAL.APP', -- [NormalizedEmail]
    0, -- [EmailConfirmed]
    NULL, -- [PasswordHash]
    NULL, -- [SecurityStamp]
    NULL, -- [ConcurrencyStamp]
    NULL, -- [PhoneNumber]
    0, -- [PhoneNumberConfirmed]
    0, -- [TwoFactorEnabled]
    NULL, -- [LockoutEnd]
    0, -- [LockoutEnabled]
    0  -- [AccessFailedCount]
);

-- 2. Associer l'utilisateur au rôle
INSERT INTO [dbo].[AspNetUserRoles] (
    [UserId],
    [RoleId]
)
VALUES (
    '00000000-0000-0000-0000-000000000000',
    'f8a527ac-d7f6-4d9d-aca6-46b2261b042b'
);
